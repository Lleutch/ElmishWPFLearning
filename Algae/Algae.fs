namespace Algae


module Style =
    open System.Windows

    module Coloring =
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


    open Coloring 
    open System.Windows
    open Elmish.DSL.DSL
    open System.Windows.Controls
    open System.Windows.Media
    open System.Windows.Markup
    open System
    open System.Windows.Controls.Primitives
    open Elmish.VDom.VDomTypes
    open System.ComponentModel


    type Algae =
        static member button( content   : string  
                             ,?palette  : Palette 
                             ,?Click    : RoutedEventArgs -> 'Msg 
                             ,?style    : Style ) = 
            
            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,VerticalAlignment = VerticalAlignment.Center
                    ,Padding = new Thickness(7.)
                    ,Background = Colors.Transparent
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
                
            WPF.button( Content = content
                       ,style = defaultArg style newStyle
                       ,?Click = Click)

        static member textBox( 
                               text                         : string
                              ,?palette                     : Palette
                              ,?CanEnter                    : bool
                              ,?CanTab                      : bool
                              ,?Language                    : XmlLanguage
                              ,?SpellCheck                  : bool
                              ,?TextAlignment               : TextAlignment
                              ,?TextDecorations             : TextDecorationCollection
                              ,?TextWrapping                : TextWrapping
                              ,?VerticalScrollBarVisibility : ScrollBarVisibility            
                              ,?style                       : Style   ) = 

            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,VerticalAlignment = VerticalAlignment.Center
                    ,Padding = new Thickness(7.)
                    ,Background = Colors.Transparent
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.Normal
                    ,FontSize = 14.)
            
            WPF.textBox(
                    CanEnter                    = defaultArg CanEnter true
                   ,CanTab                      = defaultArg CanTab true
                   ,?Language                   = Language 
                   ,SpellCheck                  = defaultArg SpellCheck false
                   ,Text                        = text
                   ,TextAlignment               = defaultArg TextAlignment (Windows.TextAlignment.Left)
                   ,?TextDecorations             = TextDecorations
                   ,TextWrapping                = defaultArg TextWrapping (Windows.TextWrapping.Wrap)
                   ,VerticalScrollBarVisibility = defaultArg VerticalScrollBarVisibility ScrollBarVisibility.Auto
                   ,style                       = defaultArg style newStyle
                )


        static member checkBox( content                : string  
                                ,?palette              : Palette
                                ,?IsChecked            : bool
                                ,?IsThreeState         : bool
                                ,?Click                : RoutedEventArgs -> 'Msg 
                                ,?Checked              : RoutedEventArgs -> 'Msg 
                                ,?Unchecked            : RoutedEventArgs -> 'Msg 
                                ,?Indeterminate        : RoutedEventArgs -> 'Msg  
                                ,?style                : Style   ) = 
                           

            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.checkBox(
                    Content             = content 
                   ,IsChecked           = defaultArg IsChecked false 
                   ,IsThreeState        = defaultArg IsThreeState false
                   ,?Click              = Click
                   ,?Checked            = Checked
                   ,?Unchecked          = Unchecked
                   ,?Indeterminate      = Indeterminate
                   ,style               = defaultArg style newStyle
                )        



        static member toggleButton( content         : string  
                                   ,?palette        : Palette
                                   ,?IsChecked      : bool
                                   ,?IsThreeState   : bool
                                   ,?Click          : RoutedEventArgs -> 'Msg 
                                   ,?Checked        : RoutedEventArgs -> 'Msg 
                                   ,?Unchecked      : RoutedEventArgs -> 'Msg 
                                   ,?Indeterminate  : RoutedEventArgs -> 'Msg  
                                   ,?style          : Style   ) = 
                           

            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.toggleButton(
                    Content             = content 
                   ,IsChecked           = defaultArg IsChecked false 
                   ,IsThreeState        = defaultArg IsThreeState false
                   ,?Click              = Click
                   ,?Checked            = Checked
                   ,?Unchecked          = Unchecked
                   ,?Indeterminate      = Indeterminate
                   ,style               = defaultArg style newStyle
                )        
        


        static member radioButton(  content         : string  
                                   ,?palette        : Palette
                                   ,?IsChecked      : bool
                                   ,?IsThreeState   : bool
                                   ,?Click          : RoutedEventArgs -> 'Msg 
                                   ,?Checked        : RoutedEventArgs -> 'Msg 
                                   ,?Unchecked      : RoutedEventArgs -> 'Msg 
                                   ,?Indeterminate  : RoutedEventArgs -> 'Msg  
                                   ,?style          : Style   ) = 
                           

            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.radioButton(
                    Content             = content 
                   ,IsChecked           = defaultArg IsChecked false 
                   ,IsThreeState        = defaultArg IsThreeState false
                   ,?Click              = Click
                   ,?Checked            = Checked
                   ,?Unchecked          = Unchecked
                   ,?Indeterminate      = Indeterminate
                   ,style               = defaultArg style newStyle
                )        


                
        static member progressBar( ?IsIndeterminate : bool
                                  ,?palette         : Palette
                                  ,?Value           : float
                                  ,?Minimum         : float
                                  ,?Maximum         : float
                                  ,?Orientation     : Orientation
                                  ,?ValueChanged    : RoutedPropertyChangedEventArgs<double> -> 'Msg
                                  ,?style           : Style ) = 


            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.progressBar(
                    IsIndeterminate = defaultArg IsIndeterminate true
                   ,?Value          = Value
                   ,?Minimum        = Minimum
                   ,?Maximum        = Maximum
                   ,?Orientation    = Orientation
                   ,?ValueChanged   = ValueChanged
                   ,style           = defaultArg style newStyle
                )        
    

        static member slider( ?IsIndeterminate      : bool
                             ,?palette              : Palette
                             ,?AutoToolTipPlacement : AutoToolTipPlacement
                             ,?AutoToolTipPrecision : int
                             ,?TickFrequency        : float 
                             ,?TickPlacement        : TickPlacement
                             ,?Ticks                : DoubleCollection
                             ,?Value                : float
                             ,?Minimum              : float
                             ,?Maximum              : float 
                             ,?ValueChanged         : RoutedPropertyChangedEventArgs<double> -> 'Msg
                             ,?style                : Style ) = 
            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.slider(
                    ?IsIndeterminate      = IsIndeterminate      
                   ,?AutoToolTipPlacement = AutoToolTipPlacement 
                   ,?AutoToolTipPrecision = AutoToolTipPrecision 
                   ,?TickFrequency        = TickFrequency        
                   ,?TickPlacement        = TickPlacement        
                   ,?Ticks                = Ticks                
                   ,?Value                = Value                
                   ,?Minimum              = Minimum              
                   ,?Maximum              = Maximum              
                   ,?ValueChanged         = ValueChanged         
                   ,?style                = style                 
                )        
        


        static member comboBox( children                     : WPFTree<'Msg> list
                                ,?palette                    : Palette
                                ,?IsEditable                 : bool
                                ,?Text                       : string
                                ,?SelectedItem               : obj
                                ,?IsTextSearchEnabled        : bool
                                ,?IsTextSearchCaseSensitive  : bool
                                ,?SelectionChanged           : SelectionChangedEventArgs -> 'Msg
                                ,?style                      : Style ) = 

            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.comboBox(
                    children                    = children                     
                   ,?IsEditable                 = IsEditable                  
                   ,?Text                       = Text                        
                   ,?SelectedItem               = SelectedItem                
                   ,?IsTextSearchEnabled        = IsTextSearchEnabled         
                   ,?IsTextSearchCaseSensitive  = IsTextSearchCaseSensitive   
                   ,?SelectionChanged           = SelectionChanged            
                   ,?style                      = style                       
                )        
        


        static member window( children              : WPFTree<'Msg>
                             ,?palette              : Palette
                             ,?WindowStyle          : WindowStyle 
                             ,?WindowState          : WindowState 
                             ,?Title                : string 
                             ,?ResizeMode           : ResizeMode 
                             ,?AllowsTransparency   : bool 
                             ,?Activated            : EventArgs -> 'Msg 
                             ,?Closed               : EventArgs -> 'Msg
                             ,?Closing              : CancelEventArgs -> 'Msg
                             ,?Deactivated          : EventArgs -> 'Msg
                             ,?Loaded               : RoutedEventArgs -> 'Msg  
                             ,?style                : Style) =

            let colorScheme = 
                let palette = defaultArg palette Palette.Blue
                palette.Scheme
            let newStyle =
                new Style(
                    HorizontalAlignment = HorizontalAlignment.Center 
                    ,BorderBrush = colorScheme.Primary
                    ,BorderThickness = new Thickness(2.)
                    ,Foreground  = colorScheme.Primary
                    ,FontWeight = FontWeights.SemiBold
                    ,FontSize = 14.)
            
            WPF.window(
                    children              = children              
                   ,?WindowStyle          = WindowStyle          
                   ,?WindowState          = WindowState          
                   ,?Title                = Title                
                   ,?ResizeMode           = ResizeMode           
                   ,?AllowsTransparency   = AllowsTransparency   
                   ,?Activated            = Activated            
                   ,?Closed               = Closed               
                   ,?Closing              = Closing              
                   ,?Deactivated          = Deactivated          
                   ,?Loaded               = Loaded                
                   ,?style                = style                         
                )        
    