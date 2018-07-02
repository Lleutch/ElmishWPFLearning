// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System.Windows
open System
open System.Windows.Controls
open Elmish.Processor
open Elmish.Program
open Elmish.VDom
open Elmish.Dom


module MVU =

    type ModelError =
        | TooManyClicks
        interface IError with
            member x.Description () =
                match x with
                | TooManyClicks -> "Number of Expected clicks has been reached"


    type Model = 
        { IsClicked : bool 
          Clicks : int  }

    type Msg = MsgUpdate

    let init () = 
        { IsClicked = false 
          Clicks = 0 }

    let update msg (model:Model) =  
        match msg with 
        | MsgUpdate -> 
            if model.Clicks > 6 then
                Error TooManyClicks
            else
                Success 
                    { IsClicked = not model.IsClicked 
                      Clicks = model.Clicks + 1  }


    let viewGridChildren dispatch (model:Model) = 
        [   if model.IsClicked then                       
                yield button { ButtonProperties.Default with
                                    Width = Some 55.
                                    Height = Some 23.
                                    Margin = Some (new Thickness(90., 50., 107., 0.))
                                    VerticalAlignment = Some VerticalAlignment.Top
                                    Content = Some ("Say Hello!" |> box) }
                                { ButtonEvents.Default with Click = Some (fun _ -> dispatch MsgUpdate) }   
                yield  textBlock { TextBlockProperties.Default with
                                    Margin = Some (new Thickness(84.,115.,74.,119.))
                                    Text = Some "Hello WPF!" }
                                    TextBlockEvents.Default   

            //elif model.Clicks > 4 then
            //    yield button { ButtonProperties.Default with
            //                        Width = Some 55.
            //                        Height = Some 23.
            //                        Margin = Some (new Thickness(90., 50., 107., 0.))
            //                        VerticalAlignment = Some VerticalAlignment.Top
            //                        Content = Some ("No Events!" |> box) }
            //                 ButtonEvents.Default
            else
                yield button { ButtonProperties.Default with
                                    Width = Some 55.
                                    Height = Some 23.
                                    Margin = Some (new Thickness(90., 50., 107., 0.))
                                    VerticalAlignment = Some VerticalAlignment.Top
                                    Content = Some ("Say Hello!" |> box) }
                                { ButtonEvents.Default with Click = Some (fun _ -> dispatch MsgUpdate) }                                           
        ]

    
    let view dispatch (model:Model) =

        window { WindowProperties.Default with 
                    Width = Some 500. 
                    Height = Some 300. }
               WindowEvents.Default
               ( grid GridProperties.Default ((viewGridChildren dispatch model)@(viewGridChildren dispatch model)) )        
        //let window = Window()
        //window.Width <- 300.
        //window.Height <- 300.

        //let grid = new Grid();

        //let label1 = new Label()
        //label1.Margin <- new Thickness(84.,115.,74.,119.)

        //let button1 = new Button()
        //button1.Content <- "Say Hello!"
        //button1.Height <- 23.
        //button1.Margin <- new Thickness(96., 50., 107., 0.)
        //button1.VerticalAlignment <- VerticalAlignment.Top
        //button1.Click.Add (fun _ -> dispatch MsgUpdate)
        //if model.IsClicked then

        //    label1.Content <- "Hello WPF!"
        //    grid.Children.Add(button1) |> ignore
        //    grid.Children.Add(label1) |> ignore
        //else

        //    grid.Children.Add(button1) |> ignore

        //window.Content <- grid
        //window

        //window.Show()

open MVU

[<STAThread>]
[<EntryPoint>]
let main argv = 
    
    let prog =
        mkMVUProgram init view update
        |> run
    0
    //printfn "%A" argv
    //0 // return an integer exit code
