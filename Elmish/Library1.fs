namespace Elmish

open System.Windows

module VDom =

    type Property =
        { Name : string
          Value : obj }
    type Properties = Properties of Property list  
    
    type EventHandler =
        { Name : string
          Handler : obj -> unit }
    type EventHandlers = EventHandlers of EventHandler list  
             

    type NodeElement = 
        { Tag : string
          Properties : Properties
          Events : EventHandlers
          //Style : SubTreeStyle
        }

    type Node =
        | Control of NodeElement
        | Container of NodeElement * Node list

    type Window = Window of NodeElement * Node list

module VDomConverter =
// TODO : 
//  - Define differenciation of VDOM
//  - Tag VDOM such that when traversing it in parallel to real DOM
//    we can now which part of the real DOM we need to update
//  - Traverse real DOM in parallel to VDOM
    let f () = ()

module Dom =
// TODO : define functions that allow simple usage of the DOM in a safer maner
    let f () = ()


module Program =
    open System.Threading

    type Agent<'a> = MailboxProcessor<'a>

    type AViewType = Window


    type IError =
        abstract member Description : unit -> string

    type UMOutcome<'Model> = 
        | Success of 'Model 
        | Error of IError

    type Dispatch<'Msg> = 'Msg -> unit

    type Init<'Model> = unit -> 'Model
    type View<'Msg,'Model> = Dispatch<'Msg> -> 'Model -> AViewType
    type Update<'Msg,'Model> = 'Msg -> 'Model -> UMOutcome<'Model>

    type FuncProgram<'Msg,'Model> =
        { init          : Init<'Model>
          view          : View<'Msg,'Model>
          update        : Update<'Msg,'Model> 
          errorHandler  : AViewType -> SynchronizationContext -> IError -> Async<unit>}

    type Program<'Msg,'Model> =
        { model     : 'Model
          funcs     : FuncProgram<'Msg,'Model> }



module UIThread = 
    open System.Threading


    /// taken and adapted from : http://www.fssnip.net/hL
    let launch_window_on_new_thread() =
        let mutable w = null
        let mutable c = null
        let h = new ManualResetEventSlim()
        let isAlive = new ManualResetEventSlim()
        let launcher() =
            w <- new Window()
            w.Loaded.Add(fun _ ->
                c <- SynchronizationContext.Current
                h.Set())
            w.Closed.Add(fun _ -> isAlive.Set() )

            w.Title <- "Title"
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
    open Program
    open System.Threading
    open UIThread
    open System.Diagnostics

    type Agent<'a> = MailboxProcessor<'a>

    type MessageProcessor<'Msg,'Model>(funcs:FuncProgram<'Msg,'Model>) =
        
        let modelProcessor (program:Program<'Msg,'Model>) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
            let rec aux (program:Program<'Msg,'Model>) (initial:bool) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
                async{
                    if initial then
                        do! Async.SwitchToContext sync
                        let newWindow = program.funcs.view (inbox.Post) (program.model)
                        let content = newWindow.Content
                        window.Content <- content
                        do! Async.SwitchToThreadPool ()


                    let! msg = inbox.Receive()


                    let UMModel = program.funcs.update msg (program.model)

                    match UMModel with
                    | Success model ->
                        let program = { program with model = model }

                        do! Async.SwitchToContext sync
                        let newWindow = program.funcs.view (inbox.Post) (program.model)
                        let content = newWindow.Content
                        window.Content <- content
                        do! Async.SwitchToThreadPool ()
                
                        return! aux program false window sync inbox
                    | Error error ->
                        return! program.funcs.errorHandler window sync error
                }
            
            aux program true window sync inbox
        
        let mutable isAlive = None
        do
            let (window,sync,mRes) = launch_window_on_new_thread ()

            let initialModel = funcs.init ()
            let program =
                { model = initialModel
                  funcs = funcs }

            let _ = Agent<'Msg>.Start (modelProcessor program window sync)
            isAlive <- Some mRes

        member __.Wait() =
            isAlive.Value.Wait()
            

    let mkMVUProgram init view update = 
        let errorHandler (window:AViewType) sync (error:IError) = 
            async{
                Debug.Print(sprintf "Error : %s" (error.Description()) )
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

    let withErrorHandler errorHandler (program:FuncProgram<_,_>) = { program with errorHandler = errorHandler }

    let run funcs = 
        let msgProcessor = MessageProcessor(funcs)
        msgProcessor.Wait()
        ()

    
