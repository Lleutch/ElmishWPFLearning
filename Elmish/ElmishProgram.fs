namespace Elmish


module ProgramTypes =
    open System.Windows
    open System.Threading
    open VDom.VDomTypes

    type Agent<'a> = MailboxProcessor<'a>
    
    type IError =
        abstract member Description : unit -> string

    type UMOutcome<'Model> = 
        | Success of 'Model 
        | Error of IError

    type Dispatch<'Msg> = 'Msg -> unit

    type Cmd<'Msg> = 
        | AsyncDispatcher of (Dispatch<'Msg> -> Async<unit>)
        | AsyncMsg of Async<'Msg>
        | NoEffect

    type Init<'Msg,'Model> = unit -> ('Model * Cmd<'Msg>)
    type SimpleInit<'Model> = unit -> 'Model
    
    type Update<'Msg,'Model> = 'Msg -> 'Model -> (UMOutcome<'Model> * Cmd<'Msg>)
    type SimpleUpdate<'Msg,'Model> = 'Msg -> 'Model -> UMOutcome<'Model>
    
    
    type View<'Msg,'Model> = 'Model -> WPFWindow<'Msg>
   

    type FuncProgram<'Msg,'Model> =
        { init          : Init<'Msg,'Model>
          view          : View<'Msg,'Model>
          update        : Update<'Msg,'Model> 
          errorHandler  : Window -> SynchronizationContext -> IError -> Async<unit>}


    type Program<'Msg,'Model> =
        { model     : 'Model
          funcs     : FuncProgram<'Msg,'Model> }




module UIThread = 
    open System.Windows
    open System.Threading


    /// taken and adapted from : http://www.fssnip.net/hL
    let launch_window_on_new_thread() =
        let mutable w = null
        let mutable c = null
        let h = new ManualResetEventSlim()
        let isAlive = new ManualResetEventSlim()
        let launcher() =
            w <- new Window()
            w.Content <- new Controls.Grid()
            w.Loaded.Add(fun _ ->
                c <- SynchronizationContext.Current
                h.Set())
            w.Closed.Add(fun _ -> isAlive.Set() )

            let app = new Application()
            app.Run(w) |> ignore
        let thread = new Thread(launcher)
        thread.SetApartmentState(ApartmentState.STA)
        thread.IsBackground <- true
        thread.Name <- "UI thread"
        thread.Start()
        h.Wait()
        h.Dispose()
        w,c,isAlive




module Processor =
    open System.Windows
    open ProgramTypes
    open System.Threading
    open UIThread
    open VDom.VDomTypes
    open VDom.VDomExt
    open VDom.VDom
    open VirtualConvert


    type Agent<'a> = MailboxProcessor<'a>

    
    type CommandProcessor<'Msg>(dispatch) =

        let commandProcessor (dispatch: Dispatch<'Msg>) (inbox:Agent<Cmd<'Msg>>) =
            let rec aux () =
                async{
                    let! msg = inbox.Receive()
                    match msg with
                    | AsyncDispatcher commandDispatcher -> commandDispatcher dispatch |> Async.Start
                    | AsyncMsg command -> 
                        async{
                            let! msg = command
                            return dispatch msg
                        } |> Async.Start
                    | NoEffect -> ()
                    return! aux ()
                }

            aux ()
        
        let agent = Agent<Cmd<'Msg>>.Start (commandProcessor dispatch)

        member __.AddCommand (command : Cmd<'Msg>) = agent.Post command


    type MessageProcessor<'Msg,'Model>(funcs:FuncProgram<'Msg,'Model>) =
        
        let modelProcessor (program:Program<'Msg,'Model>) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =

            let commandProcessor = new CommandProcessor<'Msg>(inbox.Post)

            let rec aux (program:Program<'Msg,'Model>) (initial:bool) (oldView:WPFWindow<'Msg>) (window:Window) (sync:SynchronizationContext) =
                async{
                    let! oldView =
                        async{
                            if initial then
                                let wpfWindow = program.funcs.view (program.model)
                                let newWindow = wpfWindow.VirtualConvert(inbox.Post)
                                let updates = treeDiff oldView newWindow

                                do! Async.SwitchToContext sync
                                updateWindow window updates
                                do! Async.SwitchToThreadPool ()
                                return newWindow
                            else
                                return oldView
                        }
                    let! msg = inbox.Receive()

                    let UMModel,cmd = program.funcs.update msg (program.model)
                    commandProcessor.AddCommand cmd

                    match UMModel with
                    | Success model ->
                        let program = { program with model = model }

                        let wpfWindow = program.funcs.view (program.model)

                        let newView = wpfWindow.VirtualConvert(inbox.Post)

                        let updates = treeDiff oldView newView

                        do! Async.SwitchToContext sync
                        updateWindow window updates
                        do! Async.SwitchToThreadPool ()

                        return! aux program false newView window sync
                    | Error error ->
                        return! program.funcs.errorHandler window sync error
                }


            let stubWindow : WPFWindow<'Msg> =

                let nodeElementGrid : WPFNodeElement<'Msg> = 
                    { Tag = Tag.NodeContainer(Grid, [])
                      Properties = VProperties []
                      Events = VEvents [] 
                      WPFEvents = WPFEvents [] }
                
                let tree = WPFTree (nodeElementGrid)
                { Tree = tree
                  Properties = VProperties []
                  Events = VEvents [] 
                  WPFEvents = WPFEvents [] }

            aux program true stubWindow window sync
        
        let mutable isAlive = None
        do
            let (window,sync,mRes) = launch_window_on_new_thread ()

            let initialModel,initialSubs = funcs.init ()
            let program =
                { model = initialModel
                  funcs = funcs }

            let _ = Agent<'Msg>.Start (modelProcessor program window sync)
            isAlive <- Some mRes

        member __.Wait() =
            isAlive.Value.Wait()
            

module Program =
    open System.Windows
    open ProgramTypes
    open Processor       


    let mkMVUProgram init view update = 
        let errorHandler (window:Window) sync (error:IError) = 
            async{
                printfn "Error : %s" (error.Description()) 
                do! Async.SwitchToContext sync
                window.Close()
                do! Async.SwitchToThreadPool ()
            }


        let funcs =
            { init          = init
              view          = view
              update        = update 
              errorHandler  = errorHandler }
        funcs 

    let mkMVUSimple (init:SimpleInit<_>) view (update:SimpleUpdate<_,_>) =
        let newUpdate msg model = 
            update msg model , NoEffect

        let newInit () =
            init () , NoEffect

        mkMVUProgram newInit view newUpdate

    let withErrorHandler errorHandler (program:FuncProgram<_,_>) = { program with errorHandler = errorHandler }

    let run funcs = 
        let msgProcessor = MessageProcessor(funcs)
        msgProcessor.Wait()
        ()