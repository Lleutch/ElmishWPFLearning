﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open System.Windows
open Elmish.DSL.DSL
open Elmish.ProgramTypes
open Elmish.Program
open Algae.Style

module MVU =
    open System.Windows.Media
    open Elmish.VDom.VirtualProperty.FsWPFRepresentation

    type ModelError =
        | TooManyClicks
        interface IError with
            member x.Description () =
                match x with
                | TooManyClicks -> "Number of Expected clicks has been reached"


    type Model = 
        { IsClicked : bool 
          Clicks : int 
          Ticks : int }

    type Msg = 
        | MsgUpdate
        | IncrementTick

    let init () = 
        let initModel = 
            { IsClicked = false 
              Clicks = 0 
              Ticks = 0 }
        initModel,NoEffect

            

    let update msg (model:Model) =  
        match msg with 
        | MsgUpdate -> 
            let outcome = 
                if false && model.Clicks > 6 then
                    Error TooManyClicks
                else
                    Success 
                        { model with
                              IsClicked = not model.IsClicked 
                              Clicks = model.Clicks + 1 }
            let command =
                if model.Clicks = 1 then
                    let timerTick dispatch =
                        async{
                            let timer = new System.Timers.Timer(1000.)
                            timer.Elapsed.Subscribe (fun _ -> dispatch IncrementTick ) |> ignore
                            timer.Enabled <- true
                            timer.Start()
                        }
                    AsyncDispatcher timerTick
                else
                    NoEffect
            outcome,command
        | IncrementTick ->
            let outcome = Success { model with Ticks = model.Ticks + 1 }
            outcome,NoEffect



    let viewGridChildren (model:Model) = 

        [   
            
            if model.IsClicked then 
                yield WPF.button( style = 
                                    new Style(Row = 0, 
                                              Width = 110., 
                                              Height = 46., 
                                              VerticalAlignment = VerticalAlignment.Top) ,
                                  Content = ("Say Hello!" |> box), 
                                  Click = fun _ -> MsgUpdate    ) 
                yield WPF.textBlock( style = new Style(Row = 2), Text = "Hello WPF!")
            else
                yield WPF.button( style = 
                                    new Style(Row = 0, 
                                              VerticalAlignment = VerticalAlignment.Top) ,
                                  Content = ("Say Hello!" |> box), 
                                  Click = fun _ -> MsgUpdate    ) 
                              
            yield WPF.textBlock( style = new Style(Row = 1), Text = (sprintf "Ticks : %i !" model.Ticks) )
            yield WPF.progressBar( style = new Style(Row = 3), IsIndeterminate = true)
            yield WPF.stackPanel( 
                    style = new Style(Row = 4) ,
                    Children = 
                        [ yield WPF.checkBox()
                          yield WPF.rectangle( Fill = Colors.Black
                                              ,Stroke = Colors.Red
                                              ,StrokeThickness = 10.0
                                              ,style = 
                                                new Style( Height = 100. ,
                                                           Width  = 100.)  
                                             )
                          yield WPF.line( Stroke = Colors.Red
                                         ,StrokeThickness = 3.5
                                         ,Coordinate = {X1 = 0.0; Y1 = 0.0; X2 = 0.0; Y2 = 50.0}
                                        )
                          yield WPF.polyline( Stroke = Colors.Blue
                                             ,StrokeThickness = 3.5
                                             ,Points = 
                                                [   
                                                    {X = 50.0 ; Y = 0.0}
                                                    {X = 50.0 ; Y = 50.0}
                                                    {X = 0.0 ; Y = 0.0}
                                                    {X = 0.0 ; Y = 50.0}
                                                ]
                                             )
                          yield WPF.textBox(Text = "What's up")]
                   )

        ]

    
    let view (model:Model) =
        WPF.window( WPF.grid( RowDefinitions = 
                                [  {Height=5;Unit=GridUnitType.Star}
                                   {Height=3;Unit=GridUnitType.Star}
                                   {Height=3;Unit=GridUnitType.Star}
                                   {Height=3;Unit=GridUnitType.Star}
                                   {Height=15;Unit=GridUnitType.Star}
                                ],                              
                              Children = viewGridChildren model ),
                    style = 
                        new Style(Width = 500., 
                                  Height = 300.) )                






    let viewGridChildrenAlgae (model:Model) = 

        [   
            
            if model.IsClicked then 
                yield Algae.button( content = "Say Hello!", 
                                    Click = fun _ -> MsgUpdate    ) 
                yield WPF.textBlock( style = new Style(Row = 2), Text = "Hello WPF!")
            else
                yield Algae.button( content = "Say Hello!", 
                                    Click = fun _ -> MsgUpdate    ) 
                              
            yield WPF.textBlock( style = new Style(Row = 1), Text = (sprintf "Ticks : %i !" model.Ticks) )
            yield Algae.progressBar( style = new Style(Row = 3), IsIndeterminate = true)
            yield WPF.stackPanel( 
                    style = new Style(Row = 4) ,
                    Children = 
                        [ yield Algae.checkBox("test")
                          yield WPF.rectangle( Fill = Colors.Black
                                              ,Stroke = Colors.Red
                                              ,StrokeThickness = 10.0
                                              ,style = 
                                                new Style( Height = 100. ,
                                                           Width  = 100.)  
                                             )
                          yield WPF.line( Stroke = Colors.Red
                                         ,StrokeThickness = 3.5
                                         ,Coordinate = {X1 = 0.0; Y1 = 0.0; X2 = 0.0; Y2 = 50.0}
                                        )
                          yield WPF.polyline( Stroke = Colors.Blue
                                             ,StrokeThickness = 3.5
                                             ,Points = 
                                                [   
                                                    {X = 50.0 ; Y = 0.0}
                                                    {X = 50.0 ; Y = 50.0}
                                                    {X = 0.0 ; Y = 0.0}
                                                    {X = 0.0 ; Y = 50.0}
                                                ]
                                             )
                          yield Algae.textBox(text = "What's up")
                          yield Algae.comboBox( children = [for i in 1 .. 4 -> Algae.textBox(text = "What's up") ] )
                          yield Algae.radioButton(content = "hola")
                          yield Algae.slider()
                          yield Algae.toggleButton(content = "hola2")
                          
                          
                        ]
                   )

        ]

    
    let viewAlgae (model:Model) =
        Algae.window( WPF.grid( RowDefinitions = 
                                    [  {Height=5;Unit=GridUnitType.Star}
                                       {Height=3;Unit=GridUnitType.Star}
                                       {Height=3;Unit=GridUnitType.Star}
                                       {Height=3;Unit=GridUnitType.Star}
                                       {Height=15;Unit=GridUnitType.Star}
                                    ],                              
                              Children = viewGridChildrenAlgae model )  )              
    


open MVU

[<STAThread>]
[<EntryPoint>]
let main argv = 
    
    
    let view isNative = 
        if isNative then
            view
        else
            viewAlgae
    //mkMVUSimple init view update
    mkMVUProgram init (view false) update
    |> run
    
    0














        //// Create the application's main window
        //let mainWindow = new Window();
        //mainWindow.Title <- "Grid Sample";

        //// Create the Grid
        //let myGrid = new Grid();
        //myGrid.Width <- 250.;
        //myGrid.Height <- 300.;
        //myGrid.HorizontalAlignment <- HorizontalAlignment.Left;
        //myGrid.VerticalAlignment <- VerticalAlignment.Top;
        //myGrid.ShowGridLines <- true;

        //// Define the Columns
        //let colDef1 = new ColumnDefinition();
        //let colDef2 = new ColumnDefinition();
        //let colDef3 = new ColumnDefinition();
        //myGrid.ColumnDefinitions.Add(colDef1);
        //myGrid.ColumnDefinitions.Add(colDef2);
        //myGrid.ColumnDefinitions.Add(colDef3);

        //// Define the Rows
        //let rowDef1 = new RowDefinition();
        //rowDef1.Height <- new GridLength(8.0,GridUnitType.Star)
        //let rowDef2 = new RowDefinition();
        //rowDef2.Height <- new GridLength(4.0,GridUnitType.Star)
        //let rowDef3 = new RowDefinition();
        //rowDef3.Height <- new GridLength(2.0,GridUnitType.Star)
        //let rowDef4 = new RowDefinition();
        //rowDef4.Height <- new GridLength(6.0,GridUnitType.Star)
        
        //myGrid.RowDefinitions.Add(rowDef1);
        //myGrid.RowDefinitions.Add(rowDef2);
        //myGrid.RowDefinitions.Add(rowDef3);
        //myGrid.RowDefinitions.Add(rowDef4);

        //// Add the first text cell to the Grid
        //let txt1 = new TextBlock();
        //txt1.Text <- "2005 Products Shipped";
        //txt1.FontSize <- 20.; 
        //txt1.FontWeight <- FontWeights.Bold;
        //Grid.SetColumnSpan(txt1, 3);
        //Grid.SetRow(txt1, 0);

        //// Add the second text cell to the Grid
        //let txt2 = new TextBlock();
        //txt2.Text <- "Quarter 1";
        //txt2.FontSize <- 12.0;
        //txt2.FontWeight <- FontWeights.Bold;
        //Grid.SetRow(txt2, 1);
        //Grid.SetColumn(txt2, 0);

        //// Add the third text cell to the Grid
        //let txt3 = new TextBlock();
        //txt3.Text <- "Quarter 2";
        //txt3.FontSize <- 12.0;
        //txt3.FontWeight <- FontWeights.Bold;
        //Grid.SetRow(txt3, 1);
        //Grid.SetColumn(txt3, 1);

        //// Add the fourth text cell to the Grid
        //let txt4 = new TextBlock();
        //txt4.Text <- "Quarter 3";
        //txt4.FontSize <- 12.0;
        //txt4.FontWeight <- FontWeights.Bold;
        //Grid.SetRow(txt4, 1);
        //Grid.SetColumn(txt4, 2);

        //// Add the sixth text cell to the Grid
        //let txt5 = new TextBlock();
        //let db1 = 50000.0;
        //txt5.Text <- db1.ToString();
        //Grid.SetRow(txt5, 2);
        //Grid.SetColumn(txt5, 0);

        //// Add the seventh text cell to the Grid
        //let txt6 = new TextBlock();
        //let db2 = 100000.;
        //txt6.Text <- db2.ToString();
        //Grid.SetRow(txt6, 2);
        //Grid.SetColumn(txt6, 1);

        //// Add the final text cell to the Grid
        //let txt7 = new TextBlock();
        //let db3 = 150000.;
        //txt7.Text <- db3.ToString();
        //Grid.SetRow(txt7, 2);
        //Grid.SetColumn(txt7, 2);

        //// Total all Data and Span Three Columns
        //let txt8 = new TextBlock();
        //txt8.FontSize <- 16.;
        //txt8.FontWeight <- FontWeights.Bold;
        //txt8.Text <- "Total Units: " + (db1 + db2 + db3).ToString();
        //Grid.SetRow(txt8, 3);
        //Grid.SetColumnSpan(txt8, 3);

        //// Add the TextBlock elements to the Grid Children collection
        //myGrid.Children.Add(txt1) |> ignore
        //myGrid.Children.Add(txt2) |> ignore
        //myGrid.Children.Add(txt3) |> ignore
        //myGrid.Children.Add(txt4) |> ignore
        //myGrid.Children.Add(txt5) |> ignore
        //myGrid.Children.Add(txt6) |> ignore
        //myGrid.Children.Add(txt7) |> ignore
        //myGrid.Children.Add(txt8) |> ignore

        //// Add the Grid as the Content of the Parent Window Object
        //mainWindow.Content <- myGrid;
        //mainWindow.Show ();