
module MVU =
    open System.Windows
    open Elmish
    open Elmish.DSL
    open Elmish.DSL.DSL
    open Elmish.ProgramTypes
    open ElmishStyle.Chart.Bar
    open Elmish.VDom.VirtualProperty.FsWPFRepresentation
    open ElmishStyle.Chart.ChartColor

    let init () = 
        ()
            

    let update _ _  =  Success ()        
        


    let viewGridChildren () = 
        let h = 800.
        let w = 800.
        let data:Coordinate list = [ {X=0.;Y=20. } ; {X=0.;Y=200. } ; {X=0.;Y=50. } ; {X=0.;Y=150. } ; {X=0.;Y=10. } ; {X=0.;Y= 15. }]
        //let data:Coordinate list = 
            //[ for i in 1..25 ->  {X=0.;Y=20. * (i|> float) } ]

        [   yield bar h w data (Palette.Blue)
        ]

    
    let view () =
        WPF.window( WPF.grid( RowDefinitions = 
                                [  {Height=1;Unit=GridUnitType.Star}
                                   {Height=1;Unit=GridUnitType.Star}
                                ],  
                              ColumnDefinitions =
                                [  {Width =1;Unit=GridUnitType.Star}
                                   {Width =1;Unit=GridUnitType.Star}
                                   {Width =1;Unit=GridUnitType.Star}
                                ],  
                              Children = viewGridChildren () ),
                    style = 
                        new Style(Width = 1000., 
                                  Height = 1000.) )                


module run =
    open MVU
    open System
    open Elmish.Program

    [<STAThread>]
    [<EntryPoint>]
    let main argv = 
    
        mkMVUSimple init view update
        |> run
    
        0



     