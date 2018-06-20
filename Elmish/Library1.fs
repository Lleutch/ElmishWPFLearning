namespace Elmish

open System.Windows
open System
open System.Windows.Controls


module Program =

    type Agent<'a> = MailboxProcessor<'a>

    type AViewType = Window

    type Dispatch<'Msg> = 'Msg -> unit
    type Init<'Model> = unit -> 'Model
    type View<'Msg,'Model> = Dispatch<'Msg> -> 'Model -> AViewType
    type Update<'Msg,'Model> = 'Msg -> 'Model -> 'Model


    type FuncProgram<'Msg,'Model> =
        { init      : Init<'Model>
          view      : View<'Msg,'Model>
          update    : Update<'Msg,'Model> }

    type Program<'Msg,'Model> =
        { model     : 'Model
          funcs     : FuncProgram<'Msg,'Model> }



module UIThread = 
    open System.Threading
    open System.Windows
    open Program
    open System.Threading.Tasks
    open System.Windows.Threading

    type Buf<'T>() =
        let tcs = new Tasks.TaskCompletionSource<'T>()
        member __.Reply(res) =
            tcs.SetResult(res)
        member __.Wait() =
            async{
                let! res = Async.AwaitTask (tcs.Task)
                return res
            }
        member __.RWait() =
            tcs.Task.Wait()
            tcs.Task.Result


    type UpdateView<'Msg,'Model> = 
        { ReplyChannel : Buf<Buf<UpdateView<'Msg,'Model>>>
          Program : Program<'Msg,'Model>
          Inbox : Agent<'Msg>
        }


    let launch_window_on_new_thread () =
        let rec launcher (w:AViewType) (buf:Buf<UpdateView<'Msg,'Model>>) =
            async{
                
                let! updateView = buf.Wait()
                if w <> null then   
                    w.Close()

                let w = updateView.Program.funcs.view (updateView.Inbox.Post) (updateView.Program.model) 

                let h = new Buf<unit>()   
                w.Loaded.Add( fun _ -> h.Reply() )
                w.Show() |> ignore
                do! h.Wait()

                let buf = new Buf<UpdateView<'Msg,'Model>>()                
                updateView.ReplyChannel.Reply(buf)
                return! launcher w buf 
            }

        let buf = new Buf<UpdateView<'Msg,'Model>>()                        

        let thread = new Thread(fun () -> launcher null buf |> Async.StartImmediate)
        thread.SetApartmentState(ApartmentState.STA)
        thread.IsBackground <- true
        thread.Name <- sprintf "UI thread for '%s'" "Fahd"
        thread.Start() 
        buf

module Processor =
    open Program
    open System.Threading
    open UIThread

    type Agent<'a> = MailboxProcessor<'a>

    type MessageProcessor<'Msg,'Model>(funcs:FuncProgram<'Msg,'Model>) =

        let rec modelProcessor (program:Program<'Msg,'Model>) (buf:Buf<UpdateView<'Msg,'Model>> option) (inbox:Agent<'Msg>) =
            async{
                let! buf =
                    async{
                        match buf with
                        | None -> 
                            let buf = launch_window_on_new_thread ()
                            let replyChannel = new Buf<Buf<UpdateView<'Msg,'Model>>>()
                            let uv =    
                                { ReplyChannel = replyChannel
                                  Program = program
                                  Inbox = inbox  }
                
                            buf.Reply(uv)
                            let! newBuf = replyChannel.Wait()                             
                            return newBuf
                        | Some buf -> 
                            return buf
                    }

                let! msg = inbox.Receive()

                let model = program.funcs.update msg (program.model)

                let program = { program with model = model }
                let replyChannel = new Buf<Buf<UpdateView<'Msg,'Model>>>()
                let uv =    
                    { ReplyChannel = replyChannel
                      Program = program
                      Inbox = inbox  }
                
                buf.Reply(uv)
                let! newBuf = replyChannel.Wait()                             

                return! modelProcessor program (Some newBuf) inbox
            }

        do
            let initialModel = funcs.init ()
            let program =
                { model = initialModel
                  funcs = funcs }


            let agent = Agent<'Msg>.Start (modelProcessor program None)
            // TODO : temporary for testing purposes
            Async.Sleep 30000 |> Async.RunSynchronously
    
    let mkMVUProgram init view update = 
        let funcs =
            { init      = init
              view      = view
              update    = update }
        funcs 

    let run funcs = MessageProcessor(funcs)
    
        