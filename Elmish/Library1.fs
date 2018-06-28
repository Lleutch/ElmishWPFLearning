namespace Elmish

open System.Windows

//module v =
//    open System
//    open System.Windows.Controls
//    open System.Windows.Controls.Primitives

//    let t = new Button()
//    t.Click.Add(fun t -> () )
//    let t () =
        
//        let eventsList = 
//            [   typeof<Button       >.GetEvents()
//                typeof<Calendar     >.GetEvents()
//                typeof<CheckBox     >.GetEvents()
//                typeof<ComboBox     >.GetEvents()
//                typeof<ContextMenu  >.GetEvents()
//                typeof<DataGrid     >.GetEvents()
//                typeof<DatePicker   >.GetEvents()
//                typeof<Grid         >.GetEvents()
//                typeof<Image        >.GetEvents()
//                typeof<Label        >.GetEvents()
//                typeof<ListBox      >.GetEvents()
//                typeof<ListView     >.GetEvents()
//                typeof<Menu         >.GetEvents()
//                typeof<MenuItem     >.GetEvents()
//                typeof<PasswordBox  >.GetEvents()
//                typeof<Popup        >.GetEvents()
//                typeof<ProgressBar  >.GetEvents()
//                typeof<RadioButton  >.GetEvents()
//                typeof<ScrollViewer >.GetEvents()
//                typeof<Slider       >.GetEvents()
//                typeof<StackPanel   >.GetEvents()
//                typeof<TabControl   >.GetEvents()
//                typeof<TextBlock    >.GetEvents()
//                typeof<ToggleButton >.GetEvents()
//                typeof<ToolTip      >.GetEvents()
//                typeof<Window       >.GetEvents()
//            ] 
        
//        let crossEvents =
//            eventsList |> List.map(fun props -> props |> Array.map(fun prop -> prop.Name ) |> Set.ofArray)
//            |> Set.intersectMany
//            |> Set.toList
//        let res =
//            typeof<Button>.GetEvents()
//            |> List.ofArray
//            |> List.filter(fun ev -> crossEvents |> List.contains (ev.Name) )
//            |> List.sortBy(fun ev -> ev.EventHandlerType.Name)
//            |> List.map(fun ev -> ev.Name,ev.EventHandlerType,ev.DeclaringType )
//        res |> printfn "%A"
//        crossEvents |> printfn "%A"


    //type Properties =
    //        IsEnabled           : bool option 
    //        IsVisible           : bool option 
    //        Width               : float option
    //        Height              : float option
    //        Opacity             : float option
    //        HorizontalAlignment : HorizontalAlignment option
    //        Margin              : Thickness option
    //        VerticalAlignment   : VerticalAlignment option
    //        Visibility          : Visibility option 

    //type Events =
    //    { Click : unit }


    //let defaultProperties =
    //    {   IsInitialized       = None
    //        IsLoaded            = None
    //        AllowDrop           = None
    //        IsMouseOver         = None
    //        IsMouseCaptured     = None
    //        IsStylusCaptured    = None
    //        IsKeyboardFocused   = None
    //        IsFocused           = None
    //        IsEnabled           = None
    //        IsVisible           = None
    //        Focusable           = None
    //        IsSealed            = None
    //        ActualWidth         = None
    //        ActualHeight        = None
    //        Width               = None
    //        MinWidth            = None
    //        MaxWidth            = None
    //        Height              = None
    //        MinHeight           = None
    //        MaxHeight           = None
    //        Opacity             = None
    //        HorizontalAlignment = None
    //        Tag                 = None
    //        ToolTip             = None
    //        DesiredSize         = None
    //        RenderSize          = None
    //        Name                = None
    //        Uid                 = None
    //        Margin              = None
    //        VerticalAlignment   = None
    //        Visibility          = None  }
    //let t = new Button()

module VDom =
    open System.Windows.Controls
    open System.Windows.Media
    open System.Windows.Input
    open System
    open System.ComponentModel

    type VProperty =
        // Window Related
        | WindowStyle         of WindowStyle
        | WindowState         of WindowState
        | Title               of string
        | ResizeMode          of ResizeMode
        | AllowsTransparency  of bool 
        // Layout Related
        | ColumnDefinitions   of ColumnDefinition list
        | RowDefinitions      of RowDefinition list
        | Column              of int 
        | ColumnSpan          of int 
        | Row                 of int 
        | RowSpan             of int 
        | Width               of float 
        | Height              of float 
        | Padding             of Thickness
        | Margin              of Thickness 
        | HorizontalAlignment of HorizontalAlignment 
        | VerticalAlignment   of VerticalAlignment 
        // Text Related
        | Content             of obj 
        | Text                of string 
        | TextAlignment       of TextAlignment 
        | TextWrapping        of TextWrapping
        | FontFamily          of FontFamily 
        | FontSize            of float 
        | FontWeight          of FontWeight 
        // Styling
        | Background          of Brush 
        | BorderBrush         of Brush 
        | BorderThickness     of Thickness 
        | Foreground          of Brush 
        // Visibily related
        | IsEnabled           of bool 
        | Visibility          of Visibility 
        | Opacity             of float 

    type VProperties = VProperties of (VProperty*int) list  





    type EvLambda<'a> = 'a -> unit
        
    type VEvent =
        // Button Related
        | Click         of EvLambda<RoutedEventArgs>
        // Text Related
        | TextInput     of EvLambda<TextCompositionEventArgs>
        // Window Related
        | Activated     of EvLambda<EventArgs>
        | Closed        of EvLambda<EventArgs>
        | Closing       of EvLambda<CancelEventArgs>
        | Deactivated   of EvLambda<EventArgs>
        | Loaded        of EvLambda<RoutedEventArgs>

    type VEvents = VEvents of VEvent list  




    type Tag =
        | Button 
        | TextBlock
        | Grid
        | Window




    type NodeElement = 
        { Tag : Tag
          Properties : VProperties
          Events : VEvents }

    type Tree = Tree of NodeElement * Tree list

    type ViewWindow = ViewWindow of NodeElement * Tree

open VDom



module Dom =
// TODO : define functions that allow simple usage of the DOM in a safer maner
    open VDom
    open System.Windows.Controls
    open System.Windows.Media
    open System.Windows.Input
    open System
    open System.ComponentModel

    let internal bindVProperties (x:'a option) (constructor: 'a -> VProperty) (listAndIndex:((VProperty*int) list * int)) =
        let (list,index) = listAndIndex
        let index = index + 1
        match x with
        | None -> (list,index)
        | Some a -> ((constructor a,index)::list,index)
    
    let internal bindVEvents (x:option<'a -> unit>) (constructor: EvLambda<'a> -> VEvent) (list:VEvent list) =
        match x with
        | None -> list
        | Some a -> (constructor a)::list

     
    type WPFObjectUpdate =
        | ButtonUpdate              of (Button -> unit)
        | TextBlockUpdate           of (TextBlock -> unit)
        | GridUpdate                of (Grid -> unit)
        | WindowUpdate              of (Window -> unit)
        | UIElementUpdate           of (UIElement -> unit)
        | FrameworkElementUpdate    of (FrameworkElement -> unit)
        | ControlUpdate             of (Control -> unit)

    type Tag with
        member x.Create() =
            match x with
            | Button    -> new Button()     :> UIElement
            | TextBlock -> new TextBlock()  :> UIElement
            | Grid      -> new Grid()       :> UIElement
            | Window    -> new Window()     :> UIElement
    
    //let w = new Button() 
   
    //w.Width

    //match FrameworkElementUpdate (fun fw -> fw.Width <- 5000.)  with
    //| FrameworkElementUpdate lambda -> lambda w
    //| _ -> ()

    // TODO : Define something cleaner as currently it is not ok yet
    type VProperty with
        member x.PropertyUpdate() =
            match x with
                // Window Related
                | WindowStyle         ws    -> WindowUpdate (fun w -> w.WindowStyle <- ws) 
                | WindowState         ws    -> WindowUpdate (fun w -> w.WindowState <- ws) 
                | Title               t     -> WindowUpdate (fun w -> w.Title <- t) 
                | ResizeMode          rm    -> WindowUpdate (fun w -> w.ResizeMode <- rm) 
                | AllowsTransparency  at    -> WindowUpdate (fun w -> w.AllowsTransparency <- at) 
                // Layout Related
                | ColumnDefinitions   cd    -> GridUpdate (fun gr -> cd |> List.iter(fun c -> gr.ColumnDefinitions.Add(c)) ) 
                | RowDefinitions      rd    -> GridUpdate (fun gr -> rd |> List.iter(fun r -> gr.RowDefinitions.Add(r)) ) 
                | Column              c     -> UIElementUpdate (fun ui -> Grid.SetColumn(ui,c)) 
                | ColumnSpan          cs    -> UIElementUpdate (fun ui -> Grid.SetColumnSpan(ui,cs)) 
                | Row                 r     -> UIElementUpdate (fun ui -> Grid.SetRow(ui,r)) 
                | RowSpan             rs    -> UIElementUpdate (fun ui -> Grid.SetRowSpan(ui,rs)) 
                | Width               w     -> FrameworkElementUpdate (fun fw -> fw.Width <- w) 
                | Height              h     -> FrameworkElementUpdate (fun fw -> fw.Height <- h) 
                | Padding             p     -> TextBlockUpdate (fun tb -> tb.Padding <- p)
                | Margin              m     -> FrameworkElementUpdate (fun fw -> fw.Margin <- m)
                | HorizontalAlignment ha    -> FrameworkElementUpdate (fun fw -> fw.HorizontalAlignment <- ha)
                | VerticalAlignment   va    -> FrameworkElementUpdate (fun fw -> fw.VerticalAlignment <- va)
                // Text Related
                | Content             c     -> ButtonUpdate (fun b -> b.Content <- c)
                | Text                t     -> TextBlockUpdate (fun tb -> tb.Text <- t) 
                | TextAlignment       ta    -> TextBlockUpdate (fun tb -> tb.TextAlignment <- ta)
                | TextWrapping        tw    -> TextBlockUpdate (fun tb -> tb.TextWrapping <- tw)
                | FontFamily          ff    -> TextBlockUpdate (fun tb -> tb.FontFamily <- ff)
                | FontSize            fs    -> TextBlockUpdate (fun tb -> tb.FontSize <- fs)
                | FontWeight          fw    -> TextBlockUpdate (fun tb -> tb.FontWeight <- fw)
                // Styling
                | Background          b     -> ControlUpdate (fun c -> c.Background <- b)
                | BorderBrush         bb    -> ControlUpdate (fun c -> c.BorderBrush <- bb)
                | BorderThickness     bt    -> ControlUpdate (fun c -> c.BorderThickness <- bt) 
                | Foreground          f     -> ControlUpdate (fun c -> c.Foreground <- f) 
                // Visibily related
                | IsEnabled           ie    -> UIElementUpdate (fun ui -> ui.IsEnabled <- ie) 
                | Visibility          v     -> UIElementUpdate (fun ui -> ui.Visibility <- v)
                | Opacity             o     -> UIElementUpdate (fun ui -> ui.Opacity <- o)

    type VEvent with
        member x.EventUpdate() =
            match x with
            // Button Related
            | Click         c ->    ButtonUpdate (fun b -> b.Click.Add c)
            // Text Related
            | TextInput     ti ->   TextBlockUpdate (fun tb -> tb.TextInput.Add ti) 
            // Window Related
            | Activated     a ->    WindowUpdate (fun w -> w.Activated.Add a) 
            | Closed        c ->    WindowUpdate (fun w -> w.Closed.Add c) 
            | Closing       c ->    WindowUpdate (fun w -> w.Closing.Add c) 
            | Deactivated   d ->    WindowUpdate (fun w -> w.Deactivated.Add d) 
            | Loaded        l ->    WindowUpdate (fun w -> w.Loaded.Add l) 


        



    let updateProperties properties (uiElement : UIElement) =
        let (VProperties vprops) = properties
        vprops
        |> List.iter( fun (vprop,_) ->
            match vprop.PropertyUpdate() with
            | ButtonUpdate              buttonUp        -> buttonUp        (uiElement :?> Button) 
            | TextBlockUpdate           textBlockUp     -> textBlockUp     (uiElement :?> TextBlock)
            | GridUpdate                gridUp          -> gridUp          (uiElement :?> Grid)
            | UIElementUpdate           uiElementUp     -> uiElementUp     uiElement 
            | FrameworkElementUpdate    frameworkElemUp -> frameworkElemUp (uiElement :?> FrameworkElement)
            | ControlUpdate             controlUp       -> controlUp       (uiElement :?> Control)
            | _                                         -> failwith "Code mistake"
            )

    let private updateEvents events (uiElement : UIElement) =
        let (VEvents vevents) = events
        vevents
        |> List.iter( fun vevent ->
            match vevent.EventUpdate() with
            | ButtonUpdate              buttonUp        -> buttonUp        (uiElement :?> Button) 
            | TextBlockUpdate           textBlockUp     -> textBlockUp     (uiElement :?> TextBlock)
            | GridUpdate                gridUp          -> gridUp          (uiElement :?> Grid)
            | UIElementUpdate           uiElementUp     -> uiElementUp     uiElement 
            | FrameworkElementUpdate    frameworkElemUp -> frameworkElemUp (uiElement :?> FrameworkElement)
            | ControlUpdate             controlUp       -> controlUp       (uiElement :?> Control)
            | _                                         -> failwith "Code mistake"
            )



    let rec private addToUIElement (uiElement : UIElement) (trees : Tree list) =
        for tree in trees do
            let (Tree (nodeElement,subTrees)) = tree
            let uiElement2 = nodeElement.Tag.Create()

            updateProperties (nodeElement.Properties) uiElement2
            updateEvents (nodeElement.Events) uiElement2
            if not trees.IsEmpty then
                (uiElement :?> Panel).Children.Add(uiElement2) |> ignore
                addToUIElement uiElement2 subTrees

    type Tree with
        member x.Create() =
            let (Tree (nodeElement,trees)) = x      
            let uiElement = nodeElement.Tag.Create()
            addToUIElement uiElement trees
            uiElement


    type ViewWindow with
        member x.Build() =
            let (ViewWindow (nodeElement, tree)) = x
            let window = nodeElement.Tag.Create() :?> Window
            let _ = 
                let (VProperties vprops) = nodeElement.Properties
                vprops
                |> List.iter( fun (vprop,_) ->
                    match vprop.PropertyUpdate() with
                    | WindowUpdate              windowUp        -> windowUp window
                    | UIElementUpdate           uiElementUp     -> uiElementUp window
                    | FrameworkElementUpdate    frameworkElemUp -> frameworkElemUp window
                    | ControlUpdate             controlUp       -> controlUp window
                    | _                                         -> failwith "Code mistake"
                   )
                let (VEvents vevents) = nodeElement.Events
                vevents
                |> List.iter( fun vevent ->
                    match vevent.EventUpdate() with
                    | WindowUpdate              windowUp        -> windowUp window
                    | _                                         -> failwith "Code mistake"
                   )
            let (Tree (nodeElement,trees)) = tree
            let uiElement = nodeElement.Tag.Create()

            updateProperties (nodeElement.Properties) uiElement
            updateEvents (nodeElement.Events) uiElement

            window.Content <- uiElement
            addToUIElement uiElement trees
            window

    /// Window
    //typeof<Window       >.GetProperties() XXX 

    /// Layout
    //typeof<Grid         >.GetProperties() XXX   
    //typeof<StackPanel   >.GetProperties()

    /// Control
    //typeof<Button       >.GetProperties() XXX
    //typeof<ComboBox     >.GetProperties()
    //typeof<ListView     >.GetProperties()
    //typeof<ProgressBar  >.GetProperties()
    //typeof<TextBlock    >.GetProperties() XXX

    

    (*** ****************** ***) 
    (***      Button        ***) 
    (*** ****************** ***) 
    type ButtonProperties =
        {      
            Column              : int option
            ColumnSpan          : int option
            Row                 : int option
            RowSpan             : int option
            Background          : Brush option 
            BorderBrush         : Brush option 
            BorderThickness     : Thickness option 
            Content             : obj option 
            FontFamily          : FontFamily option 
            FontSize            : float option 
            FontWeight          : FontWeight option 
            Foreground          : Brush option 
            IsEnabled           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            ([],0)
            |> bindVProperties x.Column              VProperty.Column         
            |> bindVProperties x.ColumnSpan          VProperty.ColumnSpan         
            |> bindVProperties x.Row                 VProperty.Row
            |> bindVProperties x.RowSpan             VProperty.RowSpan         
            |> bindVProperties x.Background          VProperty.Background         
            |> bindVProperties x.BorderBrush         VProperty.BorderBrush        
            |> bindVProperties x.BorderThickness     VProperty.BorderThickness    
            |> bindVProperties x.Content             VProperty.Content     
            |> bindVProperties x.FontFamily          VProperty.FontFamily         
            |> bindVProperties x.FontSize            VProperty.FontSize          
            |> bindVProperties x.FontWeight          VProperty.FontWeight         
            |> bindVProperties x.Foreground          VProperty.Foreground         
            |> bindVProperties x.IsEnabled           VProperty.IsEnabled          
            |> bindVProperties x.Width               VProperty.Width           
            |> bindVProperties x.Height              VProperty.Height             
            |> bindVProperties x.Opacity             VProperty.Opacity            
            |> bindVProperties x.HorizontalAlignment VProperty.HorizontalAlignment
            |> bindVProperties x.Margin              VProperty.Margin 
            |> bindVProperties x.VerticalAlignment   VProperty.VerticalAlignment  
            |> bindVProperties x.Visibility          VProperty.Visibility       
        static member Default =
            {   Column              = None
                ColumnSpan          = None
                Row                 = None
                RowSpan             = None
                Background          = None 
                BorderBrush         = None 
                BorderThickness     = None 
                Content             = None 
                FontFamily          = None 
                FontSize            = None 
                FontWeight          = None 
                Foreground          = None 
                IsEnabled           = None 
                Width               = None 
                Height              = None 
                Opacity             = None 
                HorizontalAlignment = None 
                Margin              = None 
                VerticalAlignment   = None 
                Visibility          = None  }

    type ButtonEvents =
        { Click : option<RoutedEventArgs -> unit> }
        member internal x.VEvents() =
            []
            |> bindVEvents x.Click VEvent.Click
        static member Default = 
            { Click = None }

    let button (properties:ButtonProperties) (events:ButtonEvents) =
        let vprops  = properties.VProperties() |> fst
        let vevents = events.VEvents()
        let node =
            { Tag        = Tag.Button
              Properties = vprops  |> VProperties
              Events     = vevents |> VEvents  }
        Tree (node, [])


    (*** ****************** ***) 
    (***      TextBlock     ***) 
    (*** ****************** ***) 
    type TextBlockProperties =
        {   
            Column              : int option
            ColumnSpan          : int option
            Row                 : int option
            RowSpan             : int option
            Background          : Brush option 
            Text                : string option 
            TextAlignment       : TextAlignment option
            TextWrapping        : TextWrapping option
            FontFamily          : FontFamily option 
            FontSize            : float option 
            FontWeight          : FontWeight option 
            Foreground          : Brush option 
            IsEnabled           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            Padding             : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            ([],0)
            |> bindVProperties  x.Column                VProperty.Column
            |> bindVProperties  x.ColumnSpan            VProperty.ColumnSpan         
            |> bindVProperties  x.Row                   VProperty.Row   
            |> bindVProperties  x.RowSpan               VProperty.RowSpan         
            |> bindVProperties  x.Background            VProperty.Background         
            |> bindVProperties  x.TextAlignment         VProperty.TextAlignment        
            |> bindVProperties  x.TextWrapping          VProperty.TextWrapping    
            |> bindVProperties  x.Text                  VProperty.Text     
            |> bindVProperties  x.FontFamily            VProperty.FontFamily         
            |> bindVProperties  x.FontSize              VProperty.FontSize          
            |> bindVProperties  x.FontWeight            VProperty.FontWeight         
            |> bindVProperties  x.Foreground            VProperty.Foreground         
            |> bindVProperties  x.IsEnabled             VProperty.IsEnabled          
            |> bindVProperties  x.Width                 VProperty.Width           
            |> bindVProperties  x.Height                VProperty.Height             
            |> bindVProperties  x.Opacity               VProperty.Opacity            
            |> bindVProperties  x.HorizontalAlignment   VProperty.HorizontalAlignment
            |> bindVProperties  x.Margin                VProperty.Margin 
            |> bindVProperties  x.Padding               VProperty.Padding             
            |> bindVProperties  x.VerticalAlignment     VProperty.VerticalAlignment  
            |> bindVProperties  x.Visibility            VProperty.Visibility       
        static member Default =
            {   
                Column              = None
                ColumnSpan          = None
                Row                 = None
                RowSpan             = None
                Background          = None
                Text                = None
                TextAlignment       = None
                TextWrapping        = None
                FontFamily          = None
                FontSize            = None
                FontWeight          = None
                Foreground          = None
                IsEnabled           = None
                Width               = None
                Height              = None
                Opacity             = None
                HorizontalAlignment = None
                Margin              = None
                Padding             = None
                VerticalAlignment   = None
                Visibility          = None  }

    type TextBlockEvents =
        { TextInput : option<TextCompositionEventArgs -> unit> }
        member x.VEvents() =
            []
            |> bindVEvents x.TextInput VEvent.TextInput
        static member Default = 
            { TextInput = None }

    let textBlock (properties:TextBlockProperties) (events:TextBlockEvents)  =
        let vprops  = properties.VProperties() |> fst
        let vevents = events.VEvents()
        let node =
            { Tag        = Tag.TextBlock
              Properties = vprops  |> VProperties
              Events     = vevents |> VEvents  }
        Tree (node, [])


    (*** ****************** ***) 
    (***      Grid          ***) 
    (*** ****************** ***) 
    type GridProperties =
        {   
            Column              : int option
            ColumnSpan          : int option
            Row                 : int option
            RowSpan             : int option
            ColumnDefinitions   : ColumnDefinition list option
            RowDefinitions      : RowDefinition list option
            Background          : Brush option 
            IsEnabled           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            ([],0)
            |> bindVProperties  x.Column                VProperty.Column
            |> bindVProperties  x.ColumnSpan            VProperty.ColumnSpan         
            |> bindVProperties  x.Row                   VProperty.Row   
            |> bindVProperties  x.RowSpan               VProperty.RowSpan         
            |> bindVProperties  x.ColumnDefinitions     VProperty.ColumnDefinitions         
            |> bindVProperties  x.RowDefinitions        VProperty.RowDefinitions       
            |> bindVProperties  x.Background            VProperty.Background      
            |> bindVProperties  x.IsEnabled             VProperty.IsEnabled          
            |> bindVProperties  x.Width                 VProperty.Width           
            |> bindVProperties  x.Height                VProperty.Height             
            |> bindVProperties  x.Opacity               VProperty.Opacity            
            |> bindVProperties  x.HorizontalAlignment   VProperty.HorizontalAlignment
            |> bindVProperties  x.Margin                VProperty.Margin 
            |> bindVProperties  x.VerticalAlignment     VProperty.VerticalAlignment  
            |> bindVProperties  x.Visibility            VProperty.Visibility       
        static member Default =
            {   
                Column              = None
                ColumnSpan          = None
                Row                 = None
                RowSpan             = None
                ColumnDefinitions   = None  
                RowDefinitions      = None  
                Background          = None  
                IsEnabled           = None  
                Width               = None  
                Height              = None  
                Opacity             = None  
                HorizontalAlignment = None  
                Margin              = None  
                VerticalAlignment   = None  
                Visibility          = None  }

    let grid (properties:GridProperties) (children:Tree list) =
        let vprops  = properties.VProperties() |> fst
        let node =
            { Tag        = Tag.Grid
              Properties = vprops   |> VProperties
              Events     = []       |> VEvents  }
        Tree (node, children)


    (*** ****************** ***) 
    (***      Window        ***) 
    (*** ****************** ***) 
    type WindowProperties =
        {   
            WindowStyle         : WindowStyle option
            WindowState         : WindowState option
            Title               : string option
            ResizeMode          : ResizeMode option
            AllowsTransparency  : bool option
            Background          : Brush option 
            IsEnabled           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            ([],0)
            |> bindVProperties  x.WindowStyle         VProperty.WindowStyle         
            |> bindVProperties  x.WindowState         VProperty.WindowState         
            |> bindVProperties  x.Title               VProperty.Title    
            |> bindVProperties  x.ResizeMode          VProperty.ResizeMode         
            |> bindVProperties  x.AllowsTransparency  VProperty.AllowsTransparency         
            |> bindVProperties  x.Background          VProperty.Background         
            |> bindVProperties  x.IsEnabled           VProperty.IsEnabled          
            |> bindVProperties  x.Width               VProperty.Width           
            |> bindVProperties  x.Height              VProperty.Height             
            |> bindVProperties  x.Opacity             VProperty.Opacity            
            |> bindVProperties  x.HorizontalAlignment VProperty.HorizontalAlignment
            |> bindVProperties  x.Margin              VProperty.Margin 
            |> bindVProperties  x.VerticalAlignment   VProperty.VerticalAlignment  
            |> bindVProperties  x.Visibility          VProperty.Visibility       
        static member Default =
            {   
                WindowStyle         = None  
                WindowState         = None  
                Title               = None  
                ResizeMode          = None  
                AllowsTransparency  = None  
                Background          = None  
                IsEnabled           = None  
                Width               = None  
                Height              = None  
                Opacity             = None  
                HorizontalAlignment = None  
                Margin              = None  
                VerticalAlignment   = None  
                Visibility          = None  }

    type WindowEvents =
        {   
            Activated   : option<EventArgs -> unit> 
            Closed      : option<EventArgs -> unit>
            Closing     : option<CancelEventArgs -> unit>
            Deactivated : option<EventArgs -> unit>
            Loaded      : option<RoutedEventArgs -> unit>
        }
        member x.VEvents() =
            []
            |> bindVEvents x.Activated   VEvent.Activated   
            |> bindVEvents x.Closed      VEvent.Closed      
            |> bindVEvents x.Closing     VEvent.Closing  
            |> bindVEvents x.Deactivated VEvent.Deactivated 
            |> bindVEvents x.Loaded      VEvent.Loaded      

        static member Default = 
            {   Activated   = None 
                Closed      = None
                Closing     = None
                Deactivated = None
                Loaded      = None  }

    let window (properties:WindowProperties) (events:WindowEvents) (children:Tree) =
        let vprops  = properties.VProperties() |> fst
        let vevents = events.VEvents()
        let node =
            { Tag        = Tag.Window
              Properties = vprops   |> VProperties
              Events     = vevents  |> VEvents  }
        ViewWindow (node, children)




module VDomConverter =
    open System.Windows.Controls
    open VDom
    open Dom

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
    
    type Update =
        | UpProperties  of NodeLoc * VProperties
        | UpNode        of NodeLoc * Tree
        | AddNode       of NodeLoc * Tree
        | RemoveNode    of NodeLoc 

    
    /// finds the differences to do for 2 list of properties 
    let private propertiesDifferences (VProperties propOld) (VProperties propNew) (nodeLoc : NodeLoc) =
        // Because of the bind function called to generate the list, the order of the list is reversed
        // thus we inverse the behaviour for when comparing the index of the OLD property element and the index of the 
        // NEW property element.
        let rec aux (propOld:(VProperty*int) list) (propNew:(VProperty*int) list) (updates : (VProperty*int) list) =
            match propOld,propNew with
            | (hdOld,indOld)::tlOld , (hdNew,indNew)::tlNew ->
                if indOld > indNew then
                    aux tlOld propNew ((hdOld,indOld)::updates)
                elif indOld < indNew then
                    aux propOld tlNew ((hdNew,indNew)::updates)
                else
                    if hdOld = hdNew then
                        aux tlOld tlNew updates
                    else
                        aux tlOld tlNew ((hdNew,indNew)::updates)
            
            | [] , (hd,ind)::tl -> aux [] tl ((hd,ind)::updates)
            | (hd,ind)::tl , [] -> aux tl [] ((hd,ind)::updates)
            | [] , [] -> updates
        
        match (aux propOld propNew []) with
        | []        -> []
        | vprops    -> [UpProperties (nodeLoc, VProperties vprops)]                

    let treeDiff (ViewWindow (windowOld,subTreeOld)) (ViewWindow (windowNew,subTreeNew)) =
        let rec aux (Tree (nodeOld,subsOld)) (Tree (nodeNew,subsNew)) (NodeLoc nodeLoc) =
            if nodeOld.Tag = nodeNew.Tag then
                match subsOld,subsNew with
                | [] , [] -> 
                    let up = propertiesDifferences (nodeOld.Properties) (nodeNew.Properties) (NodeLoc nodeLoc)
                    up
                | [] , _ -> 
                    let updates = 
                        subsNew
                        |> List.mapi(fun index sub -> 
                            let revList = List.rev nodeLoc
                            let nodeLoc = index::revList |> List.rev
                            AddNode ((NodeLoc nodeLoc),sub) 
                           )
                    updates
                | _ , [] ->
                    let updates = 
                        subsNew
                        |> List.mapi(fun index _ -> 
                            let revList = List.rev nodeLoc
                            let nodeLoc = index::revList |> List.rev
                            RemoveNode (NodeLoc nodeLoc) 
                           )
                    updates
                | _ , _ ->
                    let rec aux2 (subsOld : Tree list) (subsNew : Tree list) (updates:Update list) (revList:int list) (index:int) =
                        match subsOld,subsNew with
                        | [],[] -> updates |> List.rev
                        | _::tlOld , [] -> 
                            let nodeLoc = index::revList |> List.rev
                            let update = RemoveNode (NodeLoc nodeLoc) 
                            aux2 tlOld [] (update::updates) revList (index + 1)
                        | [] , hdNew::tlNew ->
                            let nodeLoc = index::revList |> List.rev
                            let update = AddNode (NodeLoc nodeLoc,hdNew) 
                            aux2 [] tlNew (update::updates) revList (index + 1)
                        | hdOld::tlOld , hdNew::tlNew ->
                            let nodeLoc = index::revList |> List.rev
                            let newUpdates = aux hdOld hdNew (NodeLoc nodeLoc) 
                            aux2 tlOld tlNew (newUpdates@updates) revList (index + 1)
                    let revList = List.rev nodeLoc
                    aux2 subsOld subsNew [] revList 0
            else
                [UpNode (NodeLoc nodeLoc,(Tree (nodeNew,subsNew)))]

        let update = propertiesDifferences (windowOld.Properties) (windowNew.Properties) (NodeLoc [])
        let updates = aux subTreeOld subTreeNew (NodeLoc [0])
        update@updates

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

    let updateWindow (window:Window) (updates : Update list) = 
        let rec aux (updates : Update list) =
            match updates with
            | [] -> ()
            | update::tl ->
                match update with
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

    let w =
        window { WindowProperties.Default with 
                    Width = Some 300. 
                    Height = Some 300. }
               WindowEvents.Default
               (grid GridProperties.Default
                      [ 
                        button { ButtonProperties.Default with Content = Some ("Say Hello!" |> box) }
                               ButtonEvents.Default   
                      ]
                )
               

    let w1 =
        window { WindowProperties.Default with 
                    Width = Some 500. 
                    Height = Some 300. }
               WindowEvents.Default
               ( grid GridProperties.Default
                      [ 
                        grid GridProperties.Default
                              [ button { ButtonProperties.Default with
                                            Width = Some 55.
                                            Height = Some 23.
                                            Margin = Some (new Thickness(96., 50., 107., 0.))
                                            VerticalAlignment = Some VerticalAlignment.Top
                                            Content = Some ("Say Hello!" |> box) }
                                        { ButtonEvents.Default with Click = Some (fun _ -> () ) }        
                        
                                button { ButtonProperties.Default with Content = Some ("Say Hello!" |> box) }
                                       ButtonEvents.Default   
                              ] 
                        button { ButtonProperties.Default with
                                    Width = Some 55.
                                    Height = Some 23.
                                    Margin = Some (new Thickness(90., 50., 107., 0.))
                                    VerticalAlignment = Some VerticalAlignment.Top
                                    Content = Some ("Say Hello!" |> box) }
                                { ButtonEvents.Default with Click = Some (fun _ -> () ) }        
                        
                      ]
               )        

    let diff = treeDiff w w1

    //let window = w1.Build()
    //window
    //let children = (window.Content :?> Grid).Children
    //(children.Item(1) :?> Grid).Children


    let p1 = 
        { ButtonProperties.Default with
                                        Width = Some 55.
                                        Height = Some 23.
                                        Margin = Some (new Thickness(90., 50., 107., 04.))
                                        Content = Some ("Say Hello!" |> box) }

    let p2 = 
        { ButtonProperties.Default with
                                        Height = Some 23.
                                        Margin = Some (new Thickness(90., 50., 107., 0.))
                                        VerticalAlignment = Some VerticalAlignment.Top
                                        Content = Some ("Ssay Hello!" |> box) }

    let d = propertiesDifferences (VProperties (p1.VProperties() |> fst)) (VProperties (p2.VProperties() |> fst)) (NodeLoc [])






    let f () = ()


module Program =
    open System.Threading

    type Agent<'a> = MailboxProcessor<'a>

    type AViewType = ViewWindow


    type IError =
        abstract member Description : unit -> string

    type UMOutcome<'Model> = 
        | Success of 'Model 
        | Error of IError

    type Dispatch<'Msg> = 'Msg -> unit

    type Init<'Model> = unit -> 'Model
    type View<'Msg,'Model> = Dispatch<'Msg> -> 'Model -> AViewType
    type Update<'Msg,'Model> = 'Msg -> 'Model -> UMOutcome<'Model>

    type FuncProgram<'Msg,'Model> =
        { init          : Init<'Model>
          view          : View<'Msg,'Model>
          update        : Update<'Msg,'Model> 
          errorHandler  : Window -> SynchronizationContext -> IError -> Async<unit>}

    type Program<'Msg,'Model> =
        { model     : 'Model
          funcs     : FuncProgram<'Msg,'Model> }



module UIThread = 
    open System.Threading


    /// taken and adapted from : http://www.fssnip.net/hL
    let launch_window_on_new_thread() =
        let mutable w = null
        let mutable c = null
        let h = new ManualResetEventSlim()
        let isAlive = new ManualResetEventSlim()
        let launcher() =
            w <- new Window()
            w.Loaded.Add(fun _ ->
                c <- SynchronizationContext.Current
                h.Set())
            w.Closed.Add(fun _ -> isAlive.Set() )

            w.Title <- "Title"
            let app = new Application()
            app.Run(w) |> ignore
        let thread = new Thread(launcher)
        thread.SetApartmentState(ApartmentState.STA)
        thread.IsBackground <- true
        thread.Name <- "UI thread"
        thread.Start()
        h.Wait()
        h.Dispose()
        w,c,isAlive



module Processor =
    open Program
    open System.Threading
    open UIThread
    open System.Diagnostics
    open VDomConverter
    open Dom

    type Agent<'a> = MailboxProcessor<'a>

    type MessageProcessor<'Msg,'Model>(funcs:FuncProgram<'Msg,'Model>) =
        
        let modelProcessor (program:Program<'Msg,'Model>) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
            let rec aux (program:Program<'Msg,'Model>) (initial:bool) (oldView:AViewType) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
                async{
                    let! oldView =
                        async{
                            if initial then
                                let newWindow = program.funcs.view (inbox.Post) (program.model)
                                do! Async.SwitchToContext sync
                                window.Content <- newWindow.Build().Content
                                do! Async.SwitchToThreadPool ()
                                return newWindow
                            else
                                return oldView
                        }
                    let! msg = inbox.Receive()


                    let UMModel = program.funcs.update msg (program.model)

                    match UMModel with
                    | Success model ->
                        let program = { program with model = model }

                        let newView = program.funcs.view (inbox.Post) (program.model)
                        let updates = treeDiff oldView newView

                        do! Async.SwitchToContext sync
                        updateWindow window updates
                        do! Async.SwitchToThreadPool ()
                
                        return! aux program false newView window sync inbox
                    | Error error ->
                        return! program.funcs.errorHandler window sync error
                }


            let stubWindow =
                let nodeElementWindow = 
                    { Tag = Tag.Window
                      Properties = VProperties []
                      Events = VEvents [] }

                let nodeElementGrid = 
                    { Tag = Tag.Grid
                      Properties = VProperties []
                      Events = VEvents [] }
                
                let tree = Tree (nodeElementGrid,[])
                ViewWindow (nodeElementWindow,tree)

            aux program true stubWindow window sync inbox
        
        let mutable isAlive = None
        do
            let (window,sync,mRes) = launch_window_on_new_thread ()

            let initialModel = funcs.init ()
            let program =
                { model = initialModel
                  funcs = funcs }

            let _ = Agent<'Msg>.Start (modelProcessor program window sync)
            isAlive <- Some mRes

        member __.Wait() =
            isAlive.Value.Wait()
            

    let mkMVUProgram init view update = 
        let errorHandler (window:Window) sync (error:IError) = 
            async{
                Debug.Print(sprintf "Error : %s" (error.Description()) )
                do! Async.SwitchToContext sync
                window.Close()
                do! Async.SwitchToThreadPool ()
            }


        let funcs =
            { init          = init
              view          = view
              update        = update 
              errorHandler  = errorHandler }
        funcs 

    let withErrorHandler errorHandler (program:FuncProgram<_,_>) = { program with errorHandler = errorHandler }

    let run funcs = 
        let msgProcessor = MessageProcessor(funcs)
        msgProcessor.Wait()
        ()

    









    
//module Dom =
//    open VDom
//    open System.Windows.Controls
//    open System.Windows.Media
    

//    type Properties =
//        {   IsInitialized       : bool option              
//            IsLoaded            : bool option
//            AllowDrop           : bool option
//            IsMouseOver         : bool option
//            IsMouseCaptured     : bool option
//            IsStylusCaptured    : bool option
//            IsKeyboardFocused   : bool option
//            IsFocused           : bool option
//            IsEnabled           : bool option 
//            IsVisible           : bool option 
//            Focusable           : bool option
//            IsSealed            : bool option
//            ActualWidth         : float option
//            ActualHeight        : float option
//            Width               : float option
//            MinWidth            : float option
//            MaxWidth            : float option
//            Height              : float option
//            MinHeight           : float option
//            MaxHeight           : float option
//            Opacity             : float option
//            HorizontalAlignment : HorizontalAlignment option
//            Tag                 : obj option
//            ToolTip             : obj option
//            DesiredSize         : Size option
//            RenderSize          : Size option
//            Name                : string option
//            Margin              : Thickness option
//            VerticalAlignment   : VerticalAlignment option
//            Visibility          : Visibility option }

//    type Events =
//        { Click : unit }


//    let defaultProperties =
//        {   IsInitialized       = None
//            IsLoaded            = None
//            AllowDrop           = None
//            IsMouseOver         = None
//            IsMouseCaptured     = None
//            IsStylusCaptured    = None
//            IsKeyboardFocused   = None
//            IsFocused           = None
//            IsEnabled           = None
//            IsVisible           = None
//            Focusable           = None
//            IsSealed            = None
//            ActualWidth         = None
//            ActualHeight        = None
//            Width               = None
//            MinWidth            = None
//            MaxWidth            = None
//            Height              = None
//            MinHeight           = None
//            MaxHeight           = None
//            Opacity             = None
//            HorizontalAlignment = None
//            Tag                 = None
//            ToolTip             = None
//            DesiredSize         = None
//            RenderSize          = None
//            Name                = None
//            Uid                 = None
//            Margin              = None
//            VerticalAlignment   = None
//            Visibility          = None  }


//    /// Layout
//    //typeof<Grid         >.GetProperties()
//    //typeof<StackPanel   >.GetProperties()

//    /// Control
//    //typeof<Button       >.GetProperties()
//    //typeof<ComboBox     >.GetProperties()
//    //typeof<Label        >.GetProperties()
//    //typeof<ListView     >.GetProperties()
//    //typeof<ProgressBar  >.GetProperties()
//    //typeof<TextBlock    >.GetProperties()


//    //type VProperty =
//    //    { Name : string
//    //      Value : obj }
//    //type VProperties = VProperties of VProperty list  
    
//    //type EventHandler =
//    //    { Name : string
//    //      Handler : obj -> unit }
//    //type EventHandlers = EventHandlers of EventHandler list  
             

//    //type NodeElement = 
//    //    { Tag : string
//    //      Properties : VProperties
//    //      Events : EventHandlers
//    //      //Style : SubTreeStyle
//    //    }

//    let yieldOnValue (x:'a option) (name:string) (list:VProperty list)  =
//        match x with
//        | None -> list
//        | Some a -> {Name = name ; Value = box a}::list

//    type ButtonProperties =
//        {   Background      : Brush option 
//            BorderBrush     : Brush option 
//            BorderThickness : Thickness option 
//            Content         : obj option 
//            FontFamily      : FontFamily option 
//            FontSize        : float option 
//            FontWeight      : FontWeight option 
//            Foreground      : Brush option }
//        member internal x.VProperties() =
//            []
//            |> yieldOnValue x.BorderBrush       "BorderBrush"     
//            |> yieldOnValue x.BorderThickness   "BorderThickness"
//            |> yieldOnValue x.Content           "Content"   
//            |> yieldOnValue x.FontFamily        "FontFamily"
//            |> yieldOnValue x.FontSize          "FontSize"  
//            |> yieldOnValue x.FontWeight        "FontWeight"
//            |> yieldOnValue x.Foreground        "Foreground"       

//    let defaultButtonProperties =
//        {   Background      = None
//            BorderBrush     = None
//            BorderThickness = None
//            Content         = Some (box "hey")
//            FontFamily      = None 
//            FontSize        = Some 50.
//            FontWeight      = None 
//            Foreground      = None }
//    let v = defaultButtonProperties.VProperties()

//    let button (buttonProperties:ButtonProperties) (properties:Properties) (events:Events) (subTrees:Tree list) = ()



 































//    let button (name:string) (content:string) (width:int) (height:int) (hoir) = 
//        let b = new Button()
//        b        

