namespace ElmishStyle

module Chart =
    open System.Windows
    open Elmish
    open Elmish.DSL
    open Elmish.DSL.DSL
    open Elmish.ProgramTypes
    

    module ChartColor =
        open System.Windows.Media
        type ColorScheme =
            { Primary : Color 
              Light : Color
              Dark : Color  }
        

        type Palette =
            | Red 
            | Blue
            | Teal
            | Green 
            | Yellow 
            | Orange
            member x.Scheme =
                match x with
                | Red       ->
                    { Primary = Color.FromRgb(0xe5uy,0x39uy,0x35uy)
                      Light = Color.FromRgb(0xffuy,0x6fuy,0x60uy)
                      Dark = Color.FromRgb(0xabuy,0x00uy,0x0duy)
                    }

                | Blue      ->
                    { Primary = Color.FromRgb(0x1euy,0x88uy,0xe5uy)
                      Light = Color.FromRgb(0x6auy,0xb7uy,0xffuy)
                      Dark = Color.FromRgb(0x00uy,0x5cuy,0xb2uy)
                    }                
                
                | Teal      ->
                    { Primary = Color.FromRgb(0x00uy,0x89uy,0x7buy)
                      Light = Color.FromRgb(0x4euy,0xbauy,0xaauy)
                      Dark = Color.FromRgb(0x00uy,0x5buy,0x4fuy)
                    }                
                
                | Green     ->
                    { Primary = Color.FromRgb(0x43uy,0xa0uy,0x47uy)
                      Light = Color.FromRgb(0x76uy,0xd2uy,0x75uy)
                      Dark = Color.FromRgb(0x00uy,0x70uy,0x1auy)
                    }                
                
                | Yellow    ->
                    { Primary = Color.FromRgb(0xfduy,0xd8uy,0x35uy)
                      Light = Color.FromRgb(0xffuy,0xffuy,0x6buy)
                      Dark = Color.FromRgb(0xc6uy,0xa7uy,0x00uy)
                    }                
                
                | Orange    ->
                    { Primary = Color.FromRgb(0xfbuy,0x8cuy,0x00uy)
                      Light = Color.FromRgb(0xffuy,0xbduy,0x45uy)
                      Dark = Color.FromRgb(0xc2uy,0x5euy,0x00uy)
                    }


    module Bar =
        open Elmish.VDom.VirtualProperty.FsWPFRepresentation
        open System.Windows.Media
        open ChartColor

        let bar (h:float) (w:float) (data:Coordinate list) (palette:Palette)=
            
            let canvasStyle =
                new Style(  Column = 0
                           ,Row = 0
                           ,Height = h
                           ,Width = w )
            
            let n = 0.1    // 10%
            let f = 0.6    // 80%
            let g = (1.-f)/2.
            let abscissaThickness = 0.5
            let ordinateThickness = 0.5
            let noi = 5 //10 // Number of ordinate indexes


            let d1 = h*n 
            let d2 = w*n

            let N = data.Length 
            let dw = (w - (2.*d2))/ (N |> float)
            let ddata = dw*f
            
            let dstart i = g*dw + i*dw + d2

            let ordinateLine = 
                { X1 = d2 ; Y1 = d1 
                  X2 = d2 ; Y2 = h - d1 }
            let abscissaLine = 
                { X1 = d2 ; Y1 = h - d1 
                  X2 =  w - d2  ; Y2 = h - d1 }
            

            // TODO : Useless to remove
            let nOrdinatLines = 
                [ for i in 0..(N-1) do
                    yield
                        { ordinateLine with 
                            X1 = ordinateLine.X1 + ( (i |> float) * dw)
                            X2 = ordinateLine.X2 + ( (i |> float) * dw)  
                            Y2 = ordinateLine.Y2 + (d1 * 0.2) }
                    ]
            let nOrdinates = [ for ord in nOrdinatLines -> WPF.line(Stroke = Colors.Gray, StrokeThickness = 0.25 , Coordinate = ord) ]

            let maxValue = data |> List.map(fun d -> d.Y) |> List.max |> (fun m -> max m 0.)
            let minValue = data |> List.map(fun d -> d.Y) |> List.min |> (fun m -> min m 0.) 
            let maxDiff = 
                let diff = maxValue - minValue
                if diff = 0. then 
                    1.
                else 
                    diff
                
            let nois = [ for i in 0..noi -> minValue + ( (i |> float) * maxDiff / (noi |> float) ) ] 
            let abscissaLines = 
                [ for i in 0..noi do
                    yield
                        { abscissaLine with 
                            Y1 = abscissaLine.Y1 - ( (i |> float) * (h - (2.*d1)) / (noi |> float))
                            Y2 = abscissaLine.Y2 - ( (i |> float) * (h - (2.*d1)) / (noi |> float)) 
                            X1 = abscissaLine.X1 - (d2 * 0.2)                        
                        }   ]
                |> List.map(fun ord -> WPF.line(Stroke = Colors.Gray, StrokeThickness = 0.25 , Coordinate = ord) )
            
            let noisText =
                [ let mutable i = 0.
                  for n in nois do
                    let text = 
                        let digit = 
                            if n = 0. then
                                1.
                            elif n < 0. then
                                (log10 (abs n) |> floor) + 1. + 1.
                            else
                                (log10 n |> floor) + 1.

                        let size1Digit = 7.
                        WPF.textBlock(
                            Text = string n,
                            style = 
                              new Style( Width = d2 * 0.8
                                        ,Bottom = d1 + ( (i |> float) * (h - (2.*d1)) / (noi |> float))
                                        ,Left = d2 - (digit * size1Digit) - 5.
                                        ,Opacity = 0.75
                              )                      
                        )
                    i <- i + 1.
                    yield text

                ]
                

            let rectangles =


                [ let mutable i = 0.
                  for d in data do
                    let y = (d.Y / maxDiff) * (h - 2.*d1)
                

                    let rect = 
                        WPF.border(
                            style = 
                              new Style( Width = ddata
                                        ,Height = y
                                        ,Bottom = d1
                                        ,Left = (dstart i)
                                        ,BorderBrush = palette.Scheme.Dark
                                        ,BorderThickness = new Thickness(2.,2.,2.,0.)
                                        ,Background = palette.Scheme.Primary
                                        ,Opacity = 0.75
                              ) 
                        )
                    i <- i+1.
                    yield rect
                ]
            
            let ordinate = WPF.line(Stroke = Colors.Black, StrokeThickness = ordinateThickness, Coordinate = ordinateLine)
            let abscissa = WPF.line(Stroke = Colors.Black, StrokeThickness = abscissaThickness, Coordinate = abscissaLine)
            let canvas = WPF.canvas(style = canvasStyle,Children=rectangles@ordinate::abscissa::nOrdinates@abscissaLines@noisText)
            canvas
