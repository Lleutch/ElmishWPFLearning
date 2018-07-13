namespace Elmish

module DSL =    


    module DSLHelpers =
        open VDom.VDomTypes

        let t = Container Grid

        let (|IsContainer|_|) (constr: 'a -> VProperty) (tag:Tag) =
            match tag with 
            | Container _ -> Some constr
            | _           -> None

        let (|IsControl|_|) (constr: 'a -> VProperty) (tag:Tag) =
            match tag with 
            | Control _ -> Some constr
            | _         -> None
        
        let (>||) ((|AP1|_|): Tag -> ('a -> VProperty) option) ((|AP2|_|): Tag -> ('a -> VProperty) option) =
            fun (tag:Tag) ->
                match (|AP1|_|) tag with
                | Some constr -> Some constr
                | None ->
                    match (|AP2|_|) tag with
                    | Some constr -> Some constr
                    | None        -> None                    


        let internal bindVProperties (x:'a option) (constr: 'a -> VProperty) (listAndIndex:((VProperty*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> ((constr a,index)::list,index)

        let internal bindVPropertiesWithMatch (x:'a option) (AP:(Tag -> ('a -> VProperty) option)) (tag:Tag) (listAndIndex:((VProperty*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> 
                match AP tag with
                | Some constr -> ((constr a,index)::list,index)
                | None        -> (list,index)

        let internal  bindWPFEvents (x:option<'a -> 'Msg>) (builder: (obj -> 'a -> unit) -> 'b) (constr: WPFLambda<'Msg,'a,'b> -> WPFEvent<'Msg>) (listAndIndex:((WPFEvent<'Msg>*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> ((constr (a,builder),index)::list,index)


    module DSL =
        open System
        open System.Windows
        open System.Windows.Media
        open System.Windows.Input
        open System.Windows.Controls
        open System.ComponentModel
        open VDom.VDomTypes
        open DSLHelpers





        type Style( //1. UIElement: 
                     ?Column               
                    ,?ColumnSpan           
                    ,?Row                  
                    ,?RowSpan              
                    ,?IsEnabled
                    ,?IsFocused
                    ,?IsVisible
                    ,?Opacity
                    ,?Visibility
                    //2. FrameworkElement :
                    ,?Height
                    ,?HorizontalAlignment
                    ,?Margin
                    ,?VerticalAlignment
                    ,?Width
                    //3. Control :
                    ,?BorderBrush
                    ,?BorderThickness
                    ,?FontFamily
                    ,?FontSize
                    ,?FontStretch
                    ,?FontStyle
                    ,?FontWeight
                    ,?Foreground
                    ,?HorizontalContentAlignment
                    ,?Padding
                    ,?VerticalContentAlignment
                    //3. Panel :
                    ,?Background  // SHARED BETWEEN : Panel, Control
                    
                    ) =

            member private __.BindedVProperties (list, tag : Tag) =
                list
                //1. UIElement: 
                |> bindVProperties Column       (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.Column        x)))
                |> bindVProperties ColumnSpan   (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.ColumnSpan    x)))        
                |> bindVProperties Row          (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.Row           x)))        
                |> bindVProperties RowSpan      (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.RowSpan       x)))        
                |> bindVProperties IsEnabled    (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.IsEnabled     x)))
                |> bindVProperties Opacity      (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.Opacity       x)))
                |> bindVProperties Visibility   (fun x -> VProperty.VStyle (UIElementStyle (UIElementStyle.Visibility    x)))
                //2. FrameworkElement :
                |> bindVProperties Height               (fun x -> VProperty.VStyle (FrameworkElementStyle (FrameworkElementStyle.Height                 x)))
                |> bindVProperties HorizontalAlignment  (fun x -> VProperty.VStyle (FrameworkElementStyle (FrameworkElementStyle.HorizontalAlignment    x)))
                |> bindVProperties Margin               (fun x -> VProperty.VStyle (FrameworkElementStyle (FrameworkElementStyle.Margin                 x)))
                |> bindVProperties VerticalAlignment    (fun x -> VProperty.VStyle (FrameworkElementStyle (FrameworkElementStyle.VerticalAlignment      x)))
                |> bindVProperties Width                (fun x -> VProperty.VStyle (FrameworkElementStyle (FrameworkElementStyle.Width                  x)))
                //3. Control :
                |> bindVPropertiesWithMatch Background                   ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.Background                   x)))) tag
                |> bindVPropertiesWithMatch BorderBrush                  ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.BorderBrush                  x)))) tag
                |> bindVPropertiesWithMatch BorderThickness              ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.BorderThickness              x)))) tag
                |> bindVPropertiesWithMatch FontFamily                   ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.FontFamily                   x)))) tag
                |> bindVPropertiesWithMatch FontSize                     ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.FontSize                     x)))) tag
                |> bindVPropertiesWithMatch FontStretch                  ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.FontStretch                  x)))) tag
                |> bindVPropertiesWithMatch FontStyle                    ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.FontStyle                    x)))) tag
                |> bindVPropertiesWithMatch FontWeight                   ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.FontWeight                   x)))) tag
                |> bindVPropertiesWithMatch Foreground                   ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.Foreground                   x)))) tag
                |> bindVPropertiesWithMatch HorizontalContentAlignment   ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.HorizontalContentAlignment   x)))) tag
                |> bindVPropertiesWithMatch Padding                      ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.Padding                      x)))) tag
                |> bindVPropertiesWithMatch VerticalContentAlignment     ((|IsControl|_|) (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.VerticalContentAlignment     x)))) tag
                //3. Panel :
                |> bindVPropertiesWithMatch 
                        Background  
                        (    ((|IsContainer|_|) (fun x -> VProperty.VStyle (PanelStyle (PanelStyle.Background     x))))    
                         >|| ((|IsControl|_|)   (fun x -> VProperty.VStyle (ControlStyle (ControlStyle.Background x))))  )  
                        tag
        
            static member internal PropagateStyle (style:Style option) tag list =
                match style with
                | Some style -> style.BindedVProperties(list,tag)
                | None       -> list
            

        type WPF =    

            (*** ****************** ***) 
            (***      Button        ***) 
            (*** ****************** ***) 
            static member button( ?Content              : obj  
                                 ,?Click                : RoutedEventArgs -> 'Msg 
                                 ,?style                : Style ) = 
                           
                let tag = Tag.Control Button
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties Content              VProperty.Content  
                    |> Style.PropagateStyle style tag 
                    |> fst
                let bindedVEvents () =
                    ([],0)
                    |> bindWPFEvents Click (fun x -> new RoutedEventHandler(x)) WPFClick                    
                    |> fst

                let vprops  = bindedVProperties ()
                let vevents = bindedVEvents ()
                let node =
                    { Tag        = tag
                      Properties = vprops  |> VProperties
                      WPFEvents  = vevents |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])                



            (*** ****************** ***) 
            (***      TextBlock     ***) 
            (*** ****************** ***) 
            static member textBlock( ?Text                  : string 
                                    ,?TextAlignment         : TextAlignment 
                                    ,?TextWrapping          : TextWrapping 
                                    ,?TextInput             : TextCompositionEventArgs -> 'Msg 
                                    ,?style                 : Style ) =                   
            
                let tag = Tag.Control TextBlock
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  TextAlignment         VProperty.TextAlignment        
                    |> bindVProperties  TextWrapping          VProperty.TextWrapping    
                    |> bindVProperties  Text                  VProperty.Text     
                    |> Style.PropagateStyle style tag 
                    |> fst

                let bindedVEvents () =
                    ([],0)
                    |> bindWPFEvents TextInput (fun x -> new TextCompositionEventHandler(x)) WPFTextInput
                    |> fst

                let vprops  = bindedVProperties ()
                let vevents = bindedVEvents ()
                let node =
                    { Tag        = tag
                      Properties = vprops  |> VProperties
                      WPFEvents  = vevents |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])      


            (*** ****************** ***) 
            (***      CheckBox      ***) 
            (*** ****************** ***) 
            static member checkBox( ?Content              : obj  
                                   ,?IsChecked            : bool
                                   ,?IsEnabled            : bool
                                   ,?IsThreeState         : bool
                                   ,?Click                : RoutedEventArgs -> 'Msg 
                                   ,?Checked              : RoutedEventArgs -> 'Msg 
                                   ,?Unchecked            : RoutedEventArgs -> 'Msg 
                                   ,?Indeterminate        : RoutedEventArgs -> 'Msg  
                                   ,?style                : Style   ) = 
                           
                let tag = Tag.Control CheckBox
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties Content              VProperty.Content     
                    |> bindVProperties IsChecked            VProperty.IsChecked          
                    |> bindVProperties IsEnabled            VProperty.IsEnabled          
                    |> bindVProperties IsThreeState         VProperty.IsThreeState          
                    |> Style.PropagateStyle style tag 
                    |> fst
                let bindedVEvents () =
                    ([],0)
                    |> bindWPFEvents Click (fun x -> new RoutedEventHandler(x)) WPFClick                    
                    |> bindWPFEvents Checked (fun x -> new RoutedEventHandler(x)) WPFChecked                    
                    |> bindWPFEvents Unchecked (fun x -> new RoutedEventHandler(x)) WPFUnchecked                    
                    |> bindWPFEvents Indeterminate (fun x -> new RoutedEventHandler(x)) WPFIndeterminate                    
                    |> fst

                let vprops  = bindedVProperties ()
                let vevents = bindedVEvents ()
                let node =
                    { Tag        = tag
                      Properties = vprops  |> VProperties
                      WPFEvents  = vevents |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])                


            (*** ****************** ***) 
            (***    ProgressBar     ***) 
            (*** ****************** ***) 

            static member progressBar( ?IsIndeterminate     : bool
                                      ,?Value               : float
                                      ,?Minimum             : float
                                      ,?Maximum             : float 
                                      ,?style               : Style ) = 
                           
                let tag = Tag.Control ProgressBar
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties IsIndeterminate      VProperty.IsIndeterminate                        
                    |> bindVProperties Value                VProperty.Value                        
                    |> bindVProperties Minimum              VProperty.Minimum                        
                    |> bindVProperties Maximum              VProperty.Maximum                        
                    |> Style.PropagateStyle style tag 
                    |> fst

                let vprops  = bindedVProperties ()
                let node =
                    { Tag        = tag
                      Properties = vprops  |> VProperties
                      WPFEvents  = [] |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])                



            //(*** ****************** ***) 
            //(***    ComboBox        ***) 
            //(*** ****************** ***) 
            ////let c = new System.Windows.Controls.ComboBox()
            ////let ci = new System.Windows.Controls.ComboBoxItem()
            ////ci.conten
            ////c.SelectionChanged

            //static member comboBox( ?Column              : int 
            //                       ,?ColumnSpan          : int 
            //                       ,?Row                 : int 
            //                       ,?RowSpan             : int 
            //                       ,?Background          : Brush 
            //                       ,?BorderBrush         : Brush 
            //                       ,?BorderThickness     : Thickness 
            //                       ,?FontFamily          : FontFamily 
            //                       ,?FontSize            : float  
            //                       ,?FontWeight          : FontWeight 
            //                       ,?Foreground          : Brush 
            //                       ,?IsEnabled           : bool
            //                       ,?Width               : float 
            //                       ,?Height              : float 
            //                       ,?Opacity             : float 
            //                       ,?HorizontalAlignment : HorizontalAlignment 
            //                       ,?Margin              : Thickness 
            //                       ,?VerticalAlignment   : VerticalAlignment 
            //                       ,?Visibility          : Visibility  
            //                       ,?IsIndeterminate     : bool
            //                       ,?Value               : float
            //                       ,?Minimum             : float
            //                       ,?Maximum             : float 
            //                       ,?SelectionChanged    : SelectionChangedEventArgs -> 'Msg  ) = 
                           
            //    let bindedVProperties () =
            //        ([],0)
            //        |> bindVProperties Column               VProperty.Column         
            //        |> bindVProperties ColumnSpan           VProperty.ColumnSpan         
            //        |> bindVProperties Row                  VProperty.Row
            //        |> bindVProperties RowSpan              VProperty.RowSpan         
            //        |> bindVProperties Background           VProperty.Background         
            //        |> bindVProperties BorderBrush          VProperty.BorderBrush        
            //        |> bindVProperties BorderThickness      VProperty.BorderThickness    
            //        |> bindVProperties FontFamily           VProperty.FontFamily         
            //        |> bindVProperties FontSize             VProperty.FontSize          
            //        |> bindVProperties FontWeight           VProperty.FontWeight         
            //        |> bindVProperties Foreground           VProperty.Foreground         
            //        |> bindVProperties IsEnabled            VProperty.IsEnabled          
            //        |> bindVProperties Width                VProperty.Width           
            //        |> bindVProperties Height               VProperty.Height             
            //        |> bindVProperties Opacity              VProperty.Opacity            
            //        |> bindVProperties HorizontalAlignment  VProperty.HorizontalAlignment
            //        |> bindVProperties Margin               VProperty.Margin 
            //        |> bindVProperties VerticalAlignment    VProperty.VerticalAlignment  
            //        |> bindVProperties Visibility           VProperty.Visibility                        
            //        |> bindVProperties IsIndeterminate      VProperty.IsIndeterminate                        
            //        |> bindVProperties Value                VProperty.Value                        
            //        |> bindVProperties Minimum              VProperty.Minimum                        
            //        |> bindVProperties Maximum              VProperty.Maximum                        
            //        |> fst

            //    let bindedVEvents () =
            //        ([],0)
            //        |> bindWPFEvents SelectionChanged (fun x -> new SelectionChangedEventHandler(x)) WPFSelectionChanged
            //        |> fst

            //    let vprops  = bindedVProperties ()
            //    let vevents = bindedVEvents ()
            //    let node =
            //        { Tag        = Tag.Control ComboBox
            //          Properties = vprops  |> VProperties
            //          WPFEvents  = vevents |> WPFEvents 
            //          Events     = [] |> VEvents  }
            //    WPFTree (node, [])                

            (*** ****************** ***) 
            (***    TextBox         ***) 
            (*** ****************** ***) 

            static member textBox( ?Text                : string 
                                  ,?style               : Style   ) = 
                           
                let tag = Tag.Control TextBox
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties Text                 VProperty.TextForTextBox                        
                    |> Style.PropagateStyle style tag 
                    |> fst

                let vprops  = bindedVProperties ()
                let node =
                    { Tag        = tag
                      Properties = vprops  |> VProperties
                      WPFEvents  = [] |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])                



            (*** ****************** ***) 
            (***      Grid          ***) 
            (*** ****************** ***) 
            static member grid( ?ColumnDefinitions  : ColDef list 
                               ,?RowDefinitions     : RowDef list 
                               ,?style              : Style
                               ,?Children           : WPFTree<'Msg> list  ) =
                                    
                let tag = Tag.Container Grid
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  ColumnDefinitions     VProperty.ColumnDefinitions         
                    |> bindVProperties  RowDefinitions        VProperty.RowDefinitions       
                    |> Style.PropagateStyle style tag 
                    |> fst

                let vprops  = bindedVProperties ()
                let node =
                    { Tag        = tag
                      Properties = vprops   |> VProperties
                      WPFEvents  = [] |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, defaultArg Children [])


            (*** ****************** ***) 
            (***      StackPanel    ***) 
            (*** ****************** ***) 
            static member stackPanel( ?style            : Style
                                     ,?Children         : WPFTree<'Msg> list ) =
                                     
                let tag = Tag.Container StackPanel
                let bindedVProperties () =
                    ([],0)
                    |> Style.PropagateStyle style tag 
                    |> fst

                let vprops  = bindedVProperties ()
                let node =
                    { Tag        = tag
                      Properties = vprops   |> VProperties
                      WPFEvents  = [] |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, defaultArg Children [])




            (*** ****************** ***) 
            (***      Window        ***) 
            (*** ****************** ***) 
            static member window( children              : WPFTree<'Msg>
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

                // This is a fake tag to allow style construction for a window 
                // a window is a control like a button
                let tag = Tag.Control TaggedControl.Button

                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  WindowStyle         VProperty.WindowStyle         
                    |> bindVProperties  WindowState         VProperty.WindowState         
                    |> bindVProperties  Title               VProperty.Title    
                    |> bindVProperties  ResizeMode          VProperty.ResizeMode         
                    |> bindVProperties  AllowsTransparency  VProperty.AllowsTransparency         
                    |> Style.PropagateStyle style tag 
                    |> fst

                let bindedVEvents () =
                    ([],0)
                    |> bindWPFEvents Activated   (fun x -> new EventHandler(x))         WPFActivated   
                    |> bindWPFEvents Closed      (fun x -> new EventHandler(x))         WPFClosed      
                    |> bindWPFEvents Closing     (fun x -> new CancelEventHandler(x))   WPFClosing  
                    |> bindWPFEvents Deactivated (fun x -> new EventHandler(x))         WPFDeactivated 
                    |> bindWPFEvents Loaded      (fun x -> new RoutedEventHandler(x))   WPFLoaded      
                    |> fst

                let vprops  = bindedVProperties ()
                let vevents = bindedVEvents ()
                { Tree = children
                  Properties = vprops   |> VProperties
                  WPFEvents  = vevents |> WPFEvents 
                  Events     = [] |> VEvents  }

    
    