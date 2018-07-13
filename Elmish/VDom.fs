namespace Elmish


module VDom =
    open System.Windows
    open System.Windows.Media


    module VDomTypes =
        open System.Windows.Input
        open System
        open System.ComponentModel

        type ColDef = 
            { Width : int
              Unit  : GridUnitType }

        type RowDef = 
            { Height : int
              Unit   : GridUnitType }

        type UIElementStyle =
            | Column        of int 
            | ColumnSpan    of int 
            | Row           of int 
            | RowSpan       of int 
            | IsEnabled     of bool
            | Opacity       of float
            | Visibility    of Visibility

        type FrameworkElementStyle = 
            | Height                of float
            | HorizontalAlignment   of HorizontalAlignment
            | Margin                of Thickness
            | VerticalAlignment     of VerticalAlignment
            | Width                 of float
            
        type ControlStyle =
            | Background                    of Brush
            | BorderBrush                   of Brush
            | BorderThickness               of Thickness
            | FontFamily                    of FontFamily
            | FontSize                      of float
            | FontStretch                   of FontStretch
            | FontStyle                     of FontStyle
            | FontWeight                    of FontWeight
            | Foreground                    of Brush
            | HorizontalContentAlignment    of HorizontalAlignment
            | Padding                       of Thickness
            | VerticalContentAlignment      of VerticalAlignment
                        
        type PanelStyle =
            | Background of Brush
       
        type VStyle = 
            | UIElementStyle of UIElementStyle
            | FrameworkElementStyle of FrameworkElementStyle
            | ControlStyle of ControlStyle
            | PanelStyle of PanelStyle


        type VProperty =
            | VStyle              of VStyle
            // Window Related
            | WindowStyle         of WindowStyle
            | WindowState         of WindowState
            | Title               of string
            | ResizeMode          of ResizeMode
            | AllowsTransparency  of bool 
            // Layout Related
            | ColumnDefinitions   of ColDef list
            | RowDefinitions      of RowDef list
            // Text Related
            | Content             of obj 
            | Text                of string 
            | TextAlignment       of TextAlignment 
            | TextWrapping        of TextWrapping
            // Element state related
            | IsEnabled           of bool
            | IsThreeState        of bool 
            | IsChecked           of bool 
            // Progress related
            | IsIndeterminate     of bool                      
            | Value               of float         
            | Minimum             of float           
            | Maximum             of float      
            | TextForTextBox      of string

        type VProperties = VProperties of (VProperty*int) list  
          
        type VEvent =
            | NoEvent 
            // Button Related
            | Click         of RoutedEventHandler
            // Text Related
            | TextInput     of TextCompositionEventHandler
            // Window Related
            | Activated     of EventHandler
            | Closed        of EventHandler
            | Closing       of CancelEventHandler
            | Deactivated   of EventHandler
            | Loaded        of RoutedEventHandler
            // CheckBox Related
            | Checked       of RoutedEventHandler
            | Unchecked     of RoutedEventHandler
            | Indeterminate of RoutedEventHandler


        type VEvents = VEvents of (VEvent*int) list              // Built from TmpEvents with the use of the dispatcher hidden from the user

        type TaggedContainer =
            | Grid
            | StackPanel
            
        type TaggedControl  =
            | Button 
            | TextBlock
            | CheckBox
            | ProgressBar
            | TextBox 

            //| Slider
            //| RadioButton

 
        type Tag =
            | Container of TaggedContainer
            | Control   of TaggedControl


        type WPFLambda<'Msg,'args,'handler> = ('args -> 'Msg) * ((obj -> 'args -> unit) -> 'handler)

        type WPFEvent<'Msg> = 
            // Button Related
            | WPFClick          of WPFLambda<'Msg,RoutedEventArgs,RoutedEventHandler>
            // Text Related
            | WPFTextInput      of WPFLambda<'Msg,TextCompositionEventArgs,TextCompositionEventHandler> 
            // Window Related
            | WPFActivated      of WPFLambda<'Msg,EventArgs,EventHandler>
            | WPFClosed         of WPFLambda<'Msg,EventArgs,EventHandler>
            | WPFClosing        of WPFLambda<'Msg,CancelEventArgs,CancelEventHandler>
            | WPFDeactivated    of WPFLambda<'Msg,EventArgs,EventHandler>
            | WPFLoaded         of WPFLambda<'Msg,RoutedEventArgs,RoutedEventHandler>
            // CheckBox Related
            | WPFChecked        of WPFLambda<'Msg,RoutedEventArgs,RoutedEventHandler>
            | WPFUnchecked      of WPFLambda<'Msg,RoutedEventArgs,RoutedEventHandler>
            | WPFIndeterminate  of WPFLambda<'Msg,RoutedEventArgs,RoutedEventHandler>
            //// ComboBox Related
            //| WPFSelectionChanged of WPFLambda<'Msg,SelectionChangedEventArgs,SelectionChangedEventHandler>

        type WPFEvents<'Msg> = WPFEvents of (WPFEvent<'Msg>*int) list    // Temporary, making the events independent of the dispatcher


        type WPFNodeElement<'Msg>  = 
            { Tag : Tag
              Properties : VProperties
              Events : VEvents 
              WPFEvents : WPFEvents<'Msg>   }

        type WPFTree<'Msg>  = WPFTree of WPFNodeElement<'Msg> * WPFTree<'Msg>  list

        type WPFWindow<'Msg>  = 
            { Properties : VProperties
              Events : VEvents 
              WPFEvents : WPFEvents<'Msg>  
              Tree : WPFTree<'Msg> }


    module VDomDefaultValues =
        open System
        open System.Windows
        open System.Windows.Media
        open VDomTypes

        // TODO : Specify/Document default values

        (*** General ***)
        // This is not necessarily applicable to all controls/ UIElement
        // however it is applicable to at least 2 differents elements.
        // The default value taken by WPF framework might not be the default value applied by this DSL
        let internal background          = null 
        let internal column              = 0
        let internal columnSpan          = 1
        let internal foreground          = Brushes.Black
        let internal fontFamily          = Media.FontFamily("Segoe UI")
        let internal fontSize            = 12.0 
        let internal fontWeight          = FontWeights.Normal
        let internal fontStretch         = FontStretches.Normal
        let internal fontStyle           = FontStyles.Normal
        let internal height              = nan 
        let internal horizontalAlignment = HorizontalAlignment.Stretch 
        let internal isEnabled           = true
        let internal margin              = new Thickness(0.,0.,0.,0.)
        let internal opacity             = 1.0 
        let internal padding             = new Thickness(0.,0.,0.,0.)
        let internal row                 = 0
        let internal rowSpan             = 1
        let internal verticalAlignment   = VerticalAlignment.Stretch
        let internal visibility          = Visibility.Visible 
        let internal width               = nan 
        let internal borderBrush         = null 
        let internal borderThickness     = new Thickness(0.,0.,0.,0.)
        let internal horizontalContentAlignment = HorizontalAlignment.Left
        let internal verticalContentAlignment   = VerticalAlignment.Top

        (*** Button ***)
        let internal button_Content              = null 

        (*** TextBlock ***)
        let internal textBlock_text            = "" 
        let internal textBlock_textAlignment   = TextAlignment.Left
        let internal textBlock_textWrapping    = TextWrapping.NoWrap

        (*** CheckBox ***)
        let internal checkBox_IsEnabled     = false 
        let internal checkBox_IsChecked     = false 
        let internal checkBox_IsThreeState  = false 

        (*** ProgressBar ***)
        let internal progressBar_IsIndeterminate    = false 
        let internal progressBar_Value              = 0.0 
        let internal progressBar_Minimum            = 0.0 
        let internal progressBar_Maximum            = 100.0 
  
        
        (*** Grid ***)
        let internal grid_ColumnDefinitions  = []
        let internal grid_RowDefinitions     = []

        (*** StackPanel ***)


        (*** Window ***)
        let internal window_AllowsTransparency   = false 
        let internal window_Title                = ""
        let internal window_WindowStyle          = WindowStyle.SingleBorderWindow 
        let internal window_WindowState          = WindowState.Normal
        let internal window_ResizeMode           = ResizeMode.CanResize 
        //let window_Visibility           = Visibility.Collapsed 


        let getVpropertyDefaultValue (vProp : VProperty) =
            match vProp with
            // UIElementStyle
            | VStyle (UIElementStyle (Column     _ ))    -> VStyle (UIElementStyle (Column       column))
            | VStyle (UIElementStyle (ColumnSpan _ ))    -> VStyle (UIElementStyle (ColumnSpan   columnSpan))
            | VStyle (UIElementStyle (Row        _ ))    -> VStyle (UIElementStyle (Row          row))
            | VStyle (UIElementStyle (RowSpan    _ ))    -> VStyle (UIElementStyle (RowSpan      rowSpan))
            | VStyle (UIElementStyle (UIElementStyle.IsEnabled  _ ))    -> VStyle (UIElementStyle (UIElementStyle.IsEnabled    isEnabled))
            | VStyle (UIElementStyle (Opacity    _ ))    -> VStyle (UIElementStyle (Opacity      opacity)) 
            | VStyle (UIElementStyle (Visibility _ ))    -> VStyle (UIElementStyle (Visibility   visibility))
            // FrameworkElementStyle
            | VStyle (FrameworkElementStyle (Height                _ )) -> VStyle (FrameworkElementStyle (Height              height))
            | VStyle (FrameworkElementStyle (HorizontalAlignment   _ )) -> VStyle (FrameworkElementStyle (HorizontalAlignment horizontalAlignment))
            | VStyle (FrameworkElementStyle (Margin                _ )) -> VStyle (FrameworkElementStyle (Margin              margin))
            | VStyle (FrameworkElementStyle (VerticalAlignment     _ )) -> VStyle (FrameworkElementStyle (VerticalAlignment   verticalAlignment))
            | VStyle (FrameworkElementStyle (Width                 _ )) -> VStyle (FrameworkElementStyle (Width               width))
            // ControlStyle =
            | VStyle (ControlStyle (ControlStyle.Background     _)) -> VStyle (ControlStyle ( ControlStyle.Background    background))
            | VStyle (ControlStyle (BorderBrush                 _)) -> VStyle (ControlStyle ( BorderBrush                borderBrush))
            | VStyle (ControlStyle (BorderThickness             _)) -> VStyle (ControlStyle ( BorderThickness            borderThickness))
            | VStyle (ControlStyle (FontFamily                  _)) -> VStyle (ControlStyle ( FontFamily                 fontFamily))
            | VStyle (ControlStyle (FontSize                    _)) -> VStyle (ControlStyle ( FontSize                   fontSize))
            | VStyle (ControlStyle (FontStretch                 _)) -> VStyle (ControlStyle ( FontStretch                fontStretch))
            | VStyle (ControlStyle (FontStyle                   _)) -> VStyle (ControlStyle ( FontStyle                  fontStyle))
            | VStyle (ControlStyle (FontWeight                  _)) -> VStyle (ControlStyle ( FontWeight                 fontWeight))
            | VStyle (ControlStyle (Foreground                  _)) -> VStyle (ControlStyle ( Foreground                 foreground))
            | VStyle (ControlStyle (HorizontalContentAlignment  _)) -> VStyle (ControlStyle ( HorizontalContentAlignment horizontalContentAlignment))
            | VStyle (ControlStyle (Padding                     _)) -> VStyle (ControlStyle ( Padding                    padding))
            | VStyle (ControlStyle (VerticalContentAlignment    _)) -> VStyle (ControlStyle ( VerticalContentAlignment   verticalContentAlignment))
            // PanelStyle =
            | VStyle (PanelStyle (PanelStyle.Background     _)) -> VStyle (PanelStyle ( PanelStyle.Background    background))            
            // Window Related
            | WindowStyle         _ ->  WindowStyle         window_WindowStyle 
            | WindowState         _ ->  WindowState         window_WindowState
            | Title               _ ->  Title               window_Title
            | ResizeMode          _ ->  ResizeMode          window_ResizeMode
            | AllowsTransparency  _ ->  AllowsTransparency  window_AllowsTransparency
            // Layout Related          
            | ColumnDefinitions   _ ->  ColumnDefinitions   grid_ColumnDefinitions
            | RowDefinitions      _ ->  RowDefinitions      grid_RowDefinitions
            // Text Related            
            | Content             _ ->  Content             button_Content
            | Text                _ ->  Text                textBlock_text
            | TextAlignment       _ ->  TextAlignment       textBlock_textAlignment
            | TextWrapping        _ ->  TextWrapping        textBlock_textWrapping
            // Element state related
            | IsEnabled           _ ->  IsEnabled           checkBox_IsEnabled
            | IsThreeState        _ ->  IsThreeState        checkBox_IsThreeState
            | IsChecked           _ ->  IsChecked           checkBox_IsChecked
            // Progress related
            | IsIndeterminate     _ ->  IsIndeterminate     progressBar_IsIndeterminate
            | Value               _ ->  Value               progressBar_Value
            | Minimum             _ ->  Minimum             progressBar_Minimum
            | Maximum             _ ->  Maximum             progressBar_Maximum
            | TextForTextBox      _ ->  TextForTextBox      textBlock_text



    module VDomExt =
        open System.Windows
        open System.Windows.Controls
        open VDomTypes
        open System
        open System.Windows.Controls.Primitives

        type ColDef with
            member x.Convert() = 
                let cd = new ColumnDefinition()
                let width = new GridLength(x.Width |> float,x.Unit)
                cd.Width <- width
                cd

        type RowDef with
            member x.Convert() = 
                let rd = new RowDefinition()
                let height = new GridLength(x.Height |> float,x.Unit)
                rd.Height <- height
                rd
              
        type WPFObjectUpdate =
            | UIElementUpdate           of (UIElement -> unit)
            | FrameworkElementUpdate    of (FrameworkElement -> unit)
            | ControlUpdate             of (Control -> unit)
            | PanelUpdate               of (Panel -> unit)
            | WindowUpdate              of (Window -> unit)
            | ButtonUpdate              of (Button -> unit)
            | TextBlockUpdate           of (TextBlock -> unit)
            | CheckBoxUpdate            of (CheckBox -> unit)
            | GridUpdate                of (Grid -> unit)
            | RangeBaseUpdate           of (RangeBase -> unit)
            | ProgressBarUpdate         of (ProgressBar -> unit)
            | TextBoxUpdate             of (TextBox -> unit)

        
        type Tag with
            member x.Create() =
                match x with
                | Container Grid        -> new Grid()       :> UIElement
                | Container StackPanel  -> new VirtualizingStackPanel() :> UIElement
                | Control   Button      -> new Button()     :> UIElement
                | Control   TextBlock   -> new TextBlock()  :> UIElement
                | Control   CheckBox    -> new CheckBox()   :> UIElement
                | Control   ProgressBar -> new ProgressBar()    :> UIElement
                | Control   TextBox     -> new TextBox()    :> UIElement


        // TODO : Define something cleaner as currently it is not ok yet
        type VProperty with
            member x.PropertyUpdate() =
                match x with
                // UIElementStyle
                | VStyle (UIElementStyle (Column     c ))    -> UIElementUpdate (fun ui -> Grid.SetColumn(ui,c))
                | VStyle (UIElementStyle (ColumnSpan cs ))   -> UIElementUpdate (fun ui -> Grid.SetColumnSpan(ui,cs))
                | VStyle (UIElementStyle (Row        r ))    -> UIElementUpdate (fun ui -> Grid.SetRow(ui,r))
                | VStyle (UIElementStyle (RowSpan    rs ))   -> UIElementUpdate (fun ui -> Grid.SetRowSpan(ui,rs)) 
                | VStyle (UIElementStyle (UIElementStyle.IsEnabled  ie ))   -> UIElementUpdate (fun ui -> ui.IsEnabled <- ie) 
                | VStyle (UIElementStyle (Opacity    o ))    -> UIElementUpdate (fun ui -> ui.Opacity <- o) 
                | VStyle (UIElementStyle (Visibility v ))    -> UIElementUpdate (fun ui -> ui.Visibility <- v) 
                // FrameworkElementStyle
                | VStyle (FrameworkElementStyle (Height                h ))     -> FrameworkElementUpdate (fun fw -> fw.Height <- h)
                | VStyle (FrameworkElementStyle (HorizontalAlignment   ha ))    -> FrameworkElementUpdate (fun fw -> fw.HorizontalAlignment <- ha)
                | VStyle (FrameworkElementStyle (Margin                m ))     -> FrameworkElementUpdate (fun fw -> fw.Margin <- m)
                | VStyle (FrameworkElementStyle (VerticalAlignment     va ))    -> FrameworkElementUpdate (fun fw -> fw.VerticalAlignment <- va)
                | VStyle (FrameworkElementStyle (Width                 w ))     -> FrameworkElementUpdate (fun fw -> fw.Width <- w)
                // ControlStyle =
                | VStyle (ControlStyle (ControlStyle.Background     b ))    -> ControlUpdate (fun c -> c.Background <- b)
                | VStyle (ControlStyle (BorderBrush                 bb ))   -> ControlUpdate (fun c -> c.BorderBrush <- bb)
                | VStyle (ControlStyle (BorderThickness             bt ))   -> ControlUpdate (fun c -> c.BorderThickness <- bt)
                | VStyle (ControlStyle (FontFamily                  ff ))   -> ControlUpdate (fun c -> c.FontFamily <- ff)
                | VStyle (ControlStyle (FontSize                    fs ))   -> ControlUpdate (fun c -> c.FontSize <- fs)
                | VStyle (ControlStyle (FontStretch                 fs ))   -> ControlUpdate (fun c -> c.FontStretch <- fs)
                | VStyle (ControlStyle (FontStyle                   fs ))   -> ControlUpdate (fun c -> c.FontStyle <- fs)
                | VStyle (ControlStyle (FontWeight                  fw ))   -> ControlUpdate (fun c -> c.FontWeight <- fw)
                | VStyle (ControlStyle (Foreground                  f ))    -> ControlUpdate (fun c -> c.Foreground <- f)
                | VStyle (ControlStyle (HorizontalContentAlignment  hca ))  -> ControlUpdate (fun c -> c.HorizontalContentAlignment <- hca)
                | VStyle (ControlStyle (Padding                     p ))    -> ControlUpdate (fun c -> c.Padding <- p)
                | VStyle (ControlStyle (VerticalContentAlignment    vca ))  -> ControlUpdate (fun c -> c.VerticalContentAlignment <- vca)
                // PanelStyle =
                | VStyle (PanelStyle (PanelStyle.Background     b )) -> PanelUpdate (fun p -> p.Background <-b)
                // Window Related
                | WindowStyle         ws    -> WindowUpdate (fun w -> w.WindowStyle <- ws) 
                | WindowState         ws    -> WindowUpdate (fun w -> w.WindowState <- ws) 
                | Title               t     -> WindowUpdate (fun w -> w.Title <- t) 
                | ResizeMode          rm    -> WindowUpdate (fun w -> w.ResizeMode <- rm) 
                | AllowsTransparency  at    -> WindowUpdate (fun w -> w.AllowsTransparency <- at) 
                // Layout Related
                | ColumnDefinitions   cd    -> GridUpdate (fun gr -> cd |> List.iter(fun c -> gr.ColumnDefinitions.Add(c.Convert())) ) 
                | RowDefinitions      rd    -> GridUpdate (fun gr -> rd |> List.iter(fun r -> gr.RowDefinitions.Add(r.Convert())) ) 
                // Text Related
                | Content             c     -> ButtonUpdate (fun b -> b.Content <- c)
                | Text                t     -> TextBlockUpdate (fun tb -> tb.Text <- t) 
                | TextAlignment       ta    -> TextBlockUpdate (fun tb -> tb.TextAlignment <- ta)
                | TextWrapping        tw    -> TextBlockUpdate (fun tb -> tb.TextWrapping <- tw)
                // Element state related
                | IsEnabled           ie    -> CheckBoxUpdate  (fun ui -> ui.IsEnabled <- ie)
                | IsThreeState        its   -> CheckBoxUpdate  (fun ui -> ui.IsThreeState <- its)
                | IsChecked           ic    -> CheckBoxUpdate  (fun ui -> ui.IsChecked <- Nullable(ic))
                // Progress related
                | Value               v     -> RangeBaseUpdate ( fun rb -> rb.Value <- v )
                | Minimum             m     -> RangeBaseUpdate ( fun rb -> rb.Minimum <- m )
                | Maximum             m     -> RangeBaseUpdate ( fun rb -> rb.Maximum <- m )
                | IsIndeterminate     ii    -> ProgressBarUpdate ( fun pb -> pb.IsIndeterminate <- ii )
                | TextForTextBox      ttb    -> TextBoxUpdate ( fun tb -> tb.Text <- ttb )

        type VEvent with           
            member x.EventAdd() =     
                match x with
                | NoEvent   -> failwith "fail"
                // Button Related
                | Click         c ->    ButtonUpdate (fun b -> b.Click.AddHandler c)
                // Text Related
                | TextInput     ti ->   TextBlockUpdate (fun tb -> tb.TextInput.AddHandler ti) 
                // Window Related
                | Activated     a ->    WindowUpdate (fun w -> w.Activated.AddHandler a) 
                | Closed        c ->    WindowUpdate (fun w -> w.Closed.AddHandler c) 
                | Closing       c ->    WindowUpdate (fun w -> w.Closing.AddHandler c) 
                | Deactivated   d ->    WindowUpdate (fun w -> w.Deactivated.AddHandler d) 
                | Loaded        l ->    WindowUpdate (fun w -> w.Loaded.AddHandler l) 
                // CheckBox Related
                | Checked       c ->    CheckBoxUpdate (fun cb -> cb.Checked.AddHandler c )
                | Unchecked     u ->    CheckBoxUpdate (fun cb -> cb.Unchecked.AddHandler u )
                | Indeterminate i ->    CheckBoxUpdate (fun cb -> cb.Indeterminate.AddHandler i )

            member x.EventDispose() =     
                match x with
                | NoEvent   -> failwith "fail"
                // Button Related
                | Click         c ->    ButtonUpdate (fun b -> b.Click.RemoveHandler c)
                // Text Related
                | TextInput     ti ->   TextBlockUpdate (fun tb -> tb.TextInput.RemoveHandler ti) 
                // Window Related
                | Activated     a ->    WindowUpdate (fun w -> w.Activated.RemoveHandler a) 
                | Closed        c ->    WindowUpdate (fun w -> w.Closed.RemoveHandler c) 
                | Closing       c ->    WindowUpdate (fun w -> w.Closing.RemoveHandler c) 
                | Deactivated   d ->    WindowUpdate (fun w -> w.Deactivated.RemoveHandler d) 
                | Loaded        l ->    WindowUpdate (fun w -> w.Loaded.RemoveHandler l) 
                // CheckBox Related
                | Checked       c ->    CheckBoxUpdate (fun cb -> cb.Checked.RemoveHandler c )
                | Unchecked     u ->    CheckBoxUpdate (fun cb -> cb.Unchecked.RemoveHandler u )
                | Indeterminate i ->    CheckBoxUpdate (fun cb -> cb.Indeterminate.RemoveHandler i )



        

    

        let updateProperties properties (uiElement : UIElement) =
            let (VProperties vprops) = properties
            vprops
            |> List.iter( fun (vprop,_) ->
                match vprop.PropertyUpdate() with
                | ButtonUpdate              buttonUp        -> buttonUp        (uiElement :?> Button) 
                | TextBlockUpdate           textBlockUp     -> 
                    let e = vprops
                    textBlockUp     (uiElement :?> TextBlock)
                | GridUpdate                gridUp          -> gridUp          (uiElement :?> Grid)
                | UIElementUpdate           uiElementUp     -> uiElementUp     uiElement 
                | FrameworkElementUpdate    frameworkElemUp -> frameworkElemUp (uiElement :?> FrameworkElement)
                | ControlUpdate             controlUp       -> controlUp       (uiElement :?> Control)
                | PanelUpdate               panelUp         -> panelUp         (uiElement :?> Panel)
                | RangeBaseUpdate           rangeBaseUp     -> rangeBaseUp     (uiElement :?> RangeBase)
                | ProgressBarUpdate         progressBarUp   -> progressBarUp   (uiElement :?> ProgressBar)
                | CheckBoxUpdate            checkBoxUp      -> checkBoxUp      (uiElement :?> CheckBox)
                | WindowUpdate              windowUp        -> windowUp        (uiElement :?> Window)
                | TextBoxUpdate             textBoxUp       -> textBoxUp       (uiElement :?> TextBox)
                )

        let private handleEvents (action:VEvent -> WPFObjectUpdate) events (uiElement : UIElement) =              
            let (VEvents vevents) = events
            vevents
            |> List.iter( fun (vevent,_) ->
                match action vevent with
                | ButtonUpdate              buttonUp        -> buttonUp        (uiElement :?> Button) 
                | TextBlockUpdate           textBlockUp     -> textBlockUp     (uiElement :?> TextBlock)
                | GridUpdate                gridUp          -> gridUp          (uiElement :?> Grid)
                | UIElementUpdate           uiElementUp     -> uiElementUp     uiElement 
                | FrameworkElementUpdate    frameworkElemUp -> frameworkElemUp (uiElement :?> FrameworkElement)
                | ControlUpdate             controlUp       -> controlUp       (uiElement :?> Control)
                | PanelUpdate               panelUp         -> panelUp         (uiElement :?> Panel)
                | RangeBaseUpdate           rangeBaseUp     -> rangeBaseUp     (uiElement :?> RangeBase)
                | ProgressBarUpdate         progressBarUp   -> progressBarUp   (uiElement :?> ProgressBar)
                | CheckBoxUpdate            checkBoxUp      -> checkBoxUp      (uiElement :?> CheckBox)
                | WindowUpdate              windowUp        -> windowUp        (uiElement :?> Window)
                | TextBoxUpdate             textBoxUp       -> textBoxUp       (uiElement :?> TextBox)
                )

        let internal addHandlerEvents events (uiElement : UIElement) = handleEvents (fun vevent -> vevent.EventAdd()) events uiElement
            
        let internal disposeHandlerEvents events (uiElement: UIElement) = handleEvents (fun vevent -> vevent.EventDispose()) events uiElement

        
        let rec private addToUIElement (uiElement : UIElement) (trees : WPFTree<'Msg> list) =
            for tree in trees do
                let (WPFTree (nodeElement,subTrees)) = tree
                let uiElement2 = nodeElement.Tag.Create()

                updateProperties (nodeElement.Properties) uiElement2
                addHandlerEvents (nodeElement.Events) uiElement2
                if not trees.IsEmpty then
                    (uiElement :?> Panel).Children.Add(uiElement2) |> ignore
                    addToUIElement uiElement2 subTrees

        type WPFTree<'Msg> with
            member x.Create() =
                let (WPFTree (nodeElement,trees)) = x      
                let uiElement = nodeElement.Tag.Create()
                updateProperties (nodeElement.Properties) uiElement 
                addHandlerEvents (nodeElement.Events) uiElement 
                addToUIElement uiElement trees
                uiElement


        type WPFWindow<'Msg> with
            member x.Build() =
                let tree = x.Tree
                //let (ViewWindow (nodeElement, tree)) = x
                let window = new Window() //nodeElement.Tag.Create() :?> Window
                let _ = 
                    let (VProperties vprops) = x.Properties
                    vprops
                    |> List.iter( fun (vprop,_) ->
                        match vprop.PropertyUpdate() with
                        | WindowUpdate              windowUp        -> windowUp window
                        | UIElementUpdate           uiElementUp     -> uiElementUp window
                        | FrameworkElementUpdate    frameworkElemUp -> frameworkElemUp window
                        | ControlUpdate             controlUp       -> controlUp window
                        | _                                         -> failwith "Code mistake"
                       )
                    let (VEvents vevents) = x.Events
                    vevents
                    |> List.iter( fun (vevent,_) ->
                        match vevent.EventAdd() with
                        | WindowUpdate              windowUp        -> windowUp window
                        | _                                         -> failwith "Code mistake"
                       )
                let (WPFTree (nodeElement,trees)) = tree
                let uiElement = nodeElement.Tag.Create()

                updateProperties (nodeElement.Properties) uiElement
                addHandlerEvents (nodeElement.Events) uiElement

                window.Content <- uiElement
                addToUIElement uiElement trees
                window


               

        type WPFEvent<'Msg> with
            member x.VirtualConvert(dispatch) : VEvent =
                let buildHandler getMsg handlerLambda =
                    ( fun _ args -> 
                        let msg = getMsg args
                        dispatch msg )
                    |> handlerLambda
                match x with
                // Button Related
                | WPFClick         (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Click
                // Text Related                              
                | WPFTextInput     (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> TextInput
                // Window Related                            
                | WPFActivated     (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Activated
                | WPFClosed        (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Closed
                | WPFClosing       (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Closing
                | WPFDeactivated   (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Deactivated
                | WPFLoaded        (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Loaded
                // CheckBox Related                          
                | WPFChecked       (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Checked
                | WPFUnchecked     (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Unchecked 
                | WPFIndeterminate (getMsg,handlerLambda)    -> buildHandler getMsg handlerLambda |> Indeterminate 

        type WPFEvents<'Msg> with 
            member x.VirtualConvert(dispatch) : VEvents =
                let (WPFEvents wpfEventList) = x
                wpfEventList 
                |> List.map(fun (wpfEvent,index) -> (wpfEvent.VirtualConvert(dispatch),index))
                |> VEvents

        type WPFNodeElement<'Msg> with
            member x.VirtualConvert(dispatch) : WPFNodeElement<'Msg> =
                { x with Events = x.WPFEvents.VirtualConvert(dispatch) }

        type WPFTree<'Msg> with
            member x.VirtualConvert(dispatch) : WPFTree<'Msg> =
                let rec aux (wpfTree:WPFTree<'Msg>) =
                    let (WPFTree (wpfNodeElement,wpfTrees)) = wpfTree
                    let vNodeElement = wpfNodeElement.VirtualConvert(dispatch)
                    let vTrees = 
                        match wpfTrees with 
                        | [] -> []
                        | _  -> wpfTrees |> List.map(fun wpfTree -> aux wpfTree )
                    WPFTree (vNodeElement,vTrees)    
                aux x
         

        type WPFWindow<'Msg> with
            member x.VirtualConvert(dispatch) : WPFWindow<'Msg> =            
                { x with 
                      Events = x.WPFEvents.VirtualConvert(dispatch)  
                      Tree = x.Tree.VirtualConvert(dispatch) }





    module VDom =
        open System.Windows
        open System.Windows.Controls
        open VDomTypes
        open VDomExt
    // TODO : 
    // X - Define differenciation of VDOM
    // X - Tag VDOM such that when traversing it in parallel to real DOM
    //    we can know which part of the real DOM we need to update
    // X - Traverse real DOM in parallel to VDOM
    // TODO :
    //  -   The implementation is super basic with super bad perf I would suspect, 
    //      Think of a way to make/implement optimized type-safe algorithms for 
    //      doing tree diffing => update list + updating UI Elements
    //  -   Have a more well defined Domain Model to ensure the safety of the implementation


        // Locate the node as a list of children position
        type NodeLoc = NodeLoc of int list
    

        type AddEvents      = AddEvents of VEvents
        type RemoveEvents   = RemoveEvents of VEvents

        type Update<'Msg> =
            | UpEvents      of NodeLoc * RemoveEvents * AddEvents
            | UpProperties  of NodeLoc * VProperties
            | UpNode        of NodeLoc * WPFTree<'Msg>
            | AddNode       of NodeLoc * WPFTree<'Msg>
            | RemoveNode    of NodeLoc 



        let private eventsDifferences (VEvents evOld) (VEvents evNew) (nodeLoc : NodeLoc) =
            let rec aux (evOld:(VEvent*int) list) (evNew:(VEvent*int) list) (removeEvents : (VEvent*int) list) (addEvents : (VEvent*int) list) =
                match evOld,evNew with
                | (hdOld,indOld)::tlOld , (hdNew,indNew)::tlNew ->
                    if indOld > indNew then
                        aux tlOld evNew ((hdOld,indOld)::removeEvents) addEvents
                    elif indOld < indNew then
                        aux evOld tlNew removeEvents ((hdNew,indNew)::addEvents)
                    else
                        aux tlOld tlNew  ((hdOld,indOld)::removeEvents) ((hdNew,indNew)::addEvents)
                    
                | [] , (hd,ind)::tl -> aux [] tl removeEvents ((hd,ind)::addEvents)
                | (hd,ind)::tl , [] -> aux tl [] ((hd,ind)::removeEvents) addEvents
                | [] , [] -> (removeEvents,addEvents)
        
            match (aux evOld evNew [] []) with
            | [] , []           -> UpEvents (nodeLoc, RemoveEvents (VEvents []) , AddEvents (VEvents []) )
            | [] , addEvents    -> UpEvents (nodeLoc, RemoveEvents (VEvents []) , AddEvents (VEvents addEvents) )
            | remEvents , []    -> UpEvents (nodeLoc, RemoveEvents (VEvents remEvents) , AddEvents (VEvents []) )
            | remEvents , addEvents -> UpEvents (nodeLoc, RemoveEvents (VEvents remEvents) , AddEvents (VEvents addEvents) )


    
        /// finds the differences to do for 2 list of properties 
        let private propertiesDifferences (VProperties propOld) (VProperties propNew) (nodeLoc : NodeLoc) =
            // Because of the bind function called to generate the list, the order of the list is reversed
            // thus we inverse the behaviour for when comparing the index of the OLD property element and the index of the 
            // NEW property element.
            let rec aux (propOld:(VProperty*int) list) (propNew:(VProperty*int) list) (updates : (VProperty*int) list) =
                match propOld,propNew with
                | (hdOld,indOld)::tlOld , (hdNew,indNew)::tlNew ->
                    if indOld > indNew then
                        aux tlOld propNew ((VDomDefaultValues.getVpropertyDefaultValue hdOld,indOld)::updates)
                    elif indOld < indNew then
                        aux propOld tlNew ((hdNew,indNew)::updates)
                    else
                        if hdOld = hdNew then
                            aux tlOld tlNew updates
                        else
                            aux tlOld tlNew ((hdNew,indNew)::updates)
            
                | [] , (hd,ind)::tl -> aux [] tl ((hd,ind)::updates)
                | (hd,ind)::tl , [] -> aux tl [] ((VDomDefaultValues.getVpropertyDefaultValue hd,ind)::updates)
                | [] , [] -> updates
        
            match (aux propOld propNew []) with
            | []        -> UpProperties (nodeLoc, VProperties [])
            | vprops    -> UpProperties (nodeLoc, VProperties vprops)                


        let treeDiff (windowOld:WPFWindow<'Msg>) (windowNew:WPFWindow<'Msg>) =
            let nodeDiffs (nodeOld:WPFNodeElement<'Msg>) (nodeNew:WPFNodeElement<'Msg>) (NodeLoc nodeLoc) : (Update<'Msg> * Update<'Msg>) option =
                if nodeOld.Tag = nodeNew.Tag then
                    let upProps  = propertiesDifferences (nodeOld.Properties) (nodeNew.Properties) (NodeLoc nodeLoc)
                    let upEvents = eventsDifferences (nodeOld.Events) (nodeNew.Events) (NodeLoc nodeLoc)
                    Some (upEvents,upProps)
                else 
                    None
            
            
            let rec aux (oldTrees:WPFTree<'Msg> list) (newTrees:WPFTree<'Msg> list) (NodeLoc revList) (lineLoc:int) (updates: Update<'Msg> list) =
                match oldTrees,newTrees with
                | [] , []   -> updates
                | [] , _    -> 
                    let ups = 
                        newTrees
                        |> List.mapi(fun index sub -> 
                            let nodeLoc = (lineLoc + index)::revList |> List.rev
                            AddNode ((NodeLoc nodeLoc),sub) 
                            )
                        |> List.rev
                    ups@updates                    
                | _ , []    -> 
                    let ups = 
                        oldTrees
                        |> List.mapi(fun index _ -> 
                            let nodeLoc = (lineLoc + index)::revList |> List.rev
                            RemoveNode (NodeLoc nodeLoc) 
                            )
                    ups@updates                
                | hdOld::tlOld , hdNew::tlNew   ->
                    let nl = 
                        let nl = (lineLoc::revList) |> List.rev
                        nl 
                    let (WPFTree (oldNode,oldTrees)) = hdOld
                    let (WPFTree (newNode,newTrees)) = hdNew
                    let diff = nodeDiffs oldNode newNode (NodeLoc nl)
                    let ups = 
                        match diff with
                        | None -> aux [] [] (NodeLoc (List.rev nl)) 0 ((UpNode((NodeLoc nl),hdNew))::updates)
                        | Some (upEvents,upProps) -> aux oldTrees newTrees (NodeLoc (List.rev nl)) 0 (upEvents::upProps::updates)
                    aux tlOld tlNew (NodeLoc revList) (lineLoc + 1) ups

            let upProps  = propertiesDifferences (windowOld.Properties) (windowNew.Properties) (NodeLoc [])
            let upEvents = eventsDifferences (windowOld.Events) (windowNew.Events) (NodeLoc [])

            let updates = aux [windowOld.Tree] [windowNew.Tree] (NodeLoc []) 0 []

            upEvents::upProps::(List.rev updates)




        let private getUIElement (window:Window) (nodeLoc:int list) = 
            let rec aux (uiElement:UIElement) (nodeLoc:int list) =
                match nodeLoc with
                | [] -> uiElement
                | hd::tl ->
                    let uiElement = (uiElement :?> Panel).Children.Item(hd)
                    aux uiElement tl

            match nodeLoc with
            | [] -> window :> UIElement
            | _  -> aux (window.Content :?> UIElement) nodeLoc.Tail

        let updateWindow (window:Window) (updates : Update<'Msg> list) = 
            let rec aux (updates : Update<'Msg> list) =
                match updates with
                | [] -> ()
                | update::tl ->
                    match update with
                    | UpEvents      ((NodeLoc nodeLoc), RemoveEvents removeEvents, AddEvents addEvents) ->
                        let uiElement = getUIElement window nodeLoc
                        disposeHandlerEvents removeEvents uiElement
                        addHandlerEvents addEvents uiElement
                    | UpProperties  ((NodeLoc nodeLoc),vprops) ->
                        let uiElement = getUIElement window nodeLoc
                        updateProperties vprops uiElement
                    | UpNode        ((NodeLoc nodeLoc),tree) ->
                        let revList = List.rev nodeLoc
                        let index = revList.Head
                        let parent = (getUIElement window (revList.Tail |> List.rev)) :?> Panel
                        parent.Children.RemoveAt(index)
                        parent.Children.Insert(index,tree.Create())
                    | AddNode       ((NodeLoc nodeLoc),tree) ->
                        let revList = List.rev nodeLoc
                        let index = revList.Head
                        let parent = (getUIElement window (revList.Tail |> List.rev)) :?> Panel
                        parent.Children.Insert(index,tree.Create())
                    | RemoveNode    (NodeLoc nodeLoc) ->
                        let revList = List.rev nodeLoc
                        let index = revList.Head
                        let parent = (getUIElement window (revList.Tail |> List.rev)) :?> Panel
                        parent.Children.RemoveAt(index)
                
                    aux tl
            aux updates