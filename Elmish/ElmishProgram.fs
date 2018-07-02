﻿namespace Elmish


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

    type Init<'Model> = unit -> 'Model
    type View<'Msg,'Model> = Dispatch<'Msg> -> 'Model -> ViewWindow
    type Update<'Msg,'Model> = 'Msg -> 'Model -> UMOutcome<'Model>

    type FuncProgram<'Msg,'Model> =
        { init          : Init<'Model>
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
    open VDom.VDom

    type Agent<'a> = MailboxProcessor<'a>

    type MessageProcessor<'Msg,'Model>(funcs:FuncProgram<'Msg,'Model>) =
        
        let modelProcessor (program:Program<'Msg,'Model>) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
            let rec aux (program:Program<'Msg,'Model>) (initial:bool) (oldView:ViewWindow) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
                async{
                    let! oldView =
                        async{
                            if initial then
                                let newWindow = program.funcs.view (inbox.Post) (program.model)
                                let updates = treeDiff oldView newWindow

                                do! Async.SwitchToContext sync
                                updateWindow window updates
                                do! Async.SwitchToThreadPool ()
                                return newWindow
                            else
                                return oldView
                        }
                    let! msg = inbox.Receive()


                    let UMModel = program.funcs.update msg (program.model)

                    match UMModel with
                    | Success model ->
                        let program = { program with model = model }

                        let newView = program.funcs.view (inbox.Post) (program.model)
                        let updates = treeDiff oldView newView

                        do! Async.SwitchToContext sync
                        updateWindow window updates
                        do! Async.SwitchToThreadPool ()
                
                        return! aux program false newView window sync inbox
                    | Error error ->
                        return! program.funcs.errorHandler window sync error
                }


            let stubWindow =

                let nodeElementGrid = 
                    { Tag = Tag.Grid
                      Properties = VProperties []
                      Events = VEvents [] }
                
                let tree = Tree (nodeElementGrid,[])
                { Tree = tree
                  Properties = VProperties []
                  Events = VEvents [] }

            aux program true stubWindow window sync inbox
        
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

    let withErrorHandler errorHandler (program:FuncProgram<_,_>) = { program with errorHandler = errorHandler }

    let run funcs = 
        let msgProcessor = MessageProcessor(funcs)
        msgProcessor.Wait()
        ()

    

