// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open System.Windows
open Elmish.DSL.DSL
open Elmish.DSL.DSLDomain
open Elmish.ProgramTypes
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


open MVU

[<STAThread>]
[<EntryPoint>]
let main argv = 
    
    mkMVUProgram init view update
    |> run
    
    0
