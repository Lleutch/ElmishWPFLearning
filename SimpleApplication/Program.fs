// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System.Windows
open System
open System.Windows.Controls
open Elmish.Processor
open Elmish.Program


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
            if model.Clicks > 2 then
                Error TooManyClicks
            else
                Success 
                    { IsClicked = not model.IsClicked 
                      Clicks = model.Clicks + 1  }

    let view dispatch (model:Model) =
        let window = Window()
        window.Width <- 300.
        window.Height <- 300.

        let grid = new Grid();

        let label1 = new Label()
        label1.Margin <- new Thickness(84.,115.,74.,119.)

        let button1 = new Button()
        button1.Content <- "Say Hello!"
        button1.Height <- 23.
        button1.Margin <- new Thickness(96., 50., 107., 0.)
        button1.VerticalAlignment <- VerticalAlignment.Top
        button1.Click.Add (fun _ -> dispatch MsgUpdate)
        if model.IsClicked then

            label1.Content <- "Hello WPF!"
            grid.Children.Add(button1) |> ignore
            grid.Children.Add(label1) |> ignore
        else

            grid.Children.Add(button1) |> ignore

        window.Content <- grid
        window


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
