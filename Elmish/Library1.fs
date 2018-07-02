namespace Elmish

open System.Windows

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
  

    type VEvent =
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

    type VEvents = VEvents of (VEvent*int) list  




    type Tag =
        | Button 
        | TextBlock
        | Grid



    type NodeElement = 
        { Tag : Tag
          Properties : VProperties
          Events : VEvents }

    type Tree = Tree of NodeElement * Tree list

    type ViewWindow = 
        { Properties : VProperties
          Events : VEvents 
          Tree : Tree }
       

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
    
    let internal bindVEvents (x:option<'a -> unit>) (builder: (obj -> 'a -> unit) -> 'b) (constructor: 'b -> VEvent) (listAndIndex:((VEvent*int) list * int)) =
        let (list,index) = listAndIndex
        let index = index + 1
        match x with
        | None -> (list,index)
        | Some a -> 
            let event = builder (fun _ element -> a element)
            ((constructor event,index)::list,index)

            //let handler = new TextCompositionEventHandler(fun _ arg -> ti arg)    
     
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
            //| Window    -> new Window()     :> UIElement
    
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
           
        member x.EventAdd() =     
            match x with
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

        member x.EventDispose() =     
            match x with
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
            | _                                         -> failwith "Code mistake"
            )

    let internal addHandlerEvents events (uiElement : UIElement) = handleEvents (fun vevent -> vevent.EventAdd()) events uiElement
            
    let internal disposeHandlerEvents events (uiElement: UIElement) = handleEvents (fun vevent -> vevent.EventDispose()) events uiElement

        
    let rec private addToUIElement (uiElement : UIElement) (trees : Tree list) =
        for tree in trees do
            let (Tree (nodeElement,subTrees)) = tree
            let uiElement2 = nodeElement.Tag.Create()

            updateProperties (nodeElement.Properties) uiElement2
            addHandlerEvents (nodeElement.Events) uiElement2
            if not trees.IsEmpty then
                (uiElement :?> Panel).Children.Add(uiElement2) |> ignore
                addToUIElement uiElement2 subTrees

    type Tree with
        member x.Create() =
            let (Tree (nodeElement,trees)) = x      
            let uiElement = nodeElement.Tag.Create()
            updateProperties (nodeElement.Properties) uiElement 
            addHandlerEvents (nodeElement.Events) uiElement 
            addToUIElement uiElement trees
            uiElement


    type ViewWindow with
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
            let (Tree (nodeElement,trees)) = tree
            let uiElement = nodeElement.Tag.Create()

            updateProperties (nodeElement.Properties) uiElement
            addHandlerEvents (nodeElement.Events) uiElement

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
            ([],0)
            |> bindVEvents x.Click (fun x -> new RoutedEventHandler(x)) VEvent.Click
        static member Default = 
            { Click = None }

    let button (properties:ButtonProperties) (events:ButtonEvents) =
        let vprops  = properties.VProperties() |> fst
        let vevents = events.VEvents() |> fst
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
            ([],0)
            |> bindVEvents x.TextInput (fun x -> new TextCompositionEventHandler(x)) VEvent.TextInput
        static member Default = 
            { TextInput = None }

    let textBlock (properties:TextBlockProperties) (events:TextBlockEvents)  =
        let vprops  = properties.VProperties() |> fst
        let vevents = events.VEvents() |> fst
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
            ([],0)
            |> bindVEvents x.Activated   (fun x -> new EventHandler(x))         VEvent.Activated   
            |> bindVEvents x.Closed      (fun x -> new EventHandler(x))         VEvent.Closed      
            |> bindVEvents x.Closing     (fun x -> new CancelEventHandler(x))   VEvent.Closing  
            |> bindVEvents x.Deactivated (fun x -> new EventHandler(x))         VEvent.Deactivated 
            |> bindVEvents x.Loaded      (fun x -> new RoutedEventHandler(x))   VEvent.Loaded      

        static member Default = 
            {   Activated   = None 
                Closed      = None
                Closing     = None
                Deactivated = None
                Loaded      = None  }

    let window (properties:WindowProperties) (events:WindowEvents) (children:Tree) =
        let vprops  = properties.VProperties() |> fst
        let vevents = events.VEvents() |> fst
        { Tree = children
          Properties = vprops   |> VProperties
          Events     = vevents  |> VEvents  }




module VDomConverter =
    open System.Windows.Controls
    open VDom
    open Dom
    open System

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

    type Update =
        | UpEvents      of NodeLoc * RemoveEvents * AddEvents
        | UpProperties  of NodeLoc * VProperties
        | UpNode        of NodeLoc * Tree
        | AddNode       of NodeLoc * Tree
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

    let treeDiff (windowOld:ViewWindow) (windowNew:ViewWindow) =
        let rec aux (Tree (nodeOld,subsOld)) (Tree (nodeNew,subsNew)) (NodeLoc nodeLoc) =
            if nodeOld.Tag = nodeNew.Tag then
                match subsOld,subsNew with
                | [] , [] -> 
                    let upProps  = propertiesDifferences (nodeOld.Properties) (nodeNew.Properties) (NodeLoc nodeLoc)
                    let upEvents = eventsDifferences (nodeOld.Events) (nodeNew.Events) (NodeLoc nodeLoc)
                    upEvents::upProps
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
                        |> List.rev
                    updates
                | _ , _ ->
                    let rec aux2 (subsOld : Tree list) (subsNew : Tree list) (updates:Update list) (removeNodes : Update list) (revList:int list) (index:int) =
                        match subsOld,subsNew with
                        | [],[] -> (updates |> List.rev)@removeNodes
                        | _::tlOld , [] -> 
                            let nodeLoc = index::revList |> List.rev
                            let removeNode = RemoveNode (NodeLoc nodeLoc) 
                            let res = aux2 tlOld [] updates (removeNode::removeNodes) revList (index + 1)
                            res
                        | [] , hdNew::tlNew ->
                            let nodeLoc = index::revList |> List.rev
                            let update = AddNode (NodeLoc nodeLoc,hdNew) 
                            aux2 [] tlNew (update::updates) removeNodes revList (index + 1)
                        | hdOld::tlOld , hdNew::tlNew ->
                            let nodeLoc = index::revList |> List.rev
                            let newUpdates = aux hdOld hdNew (NodeLoc nodeLoc) 
                            aux2 tlOld tlNew (newUpdates@updates) removeNodes revList (index + 1)
                    let revList = List.rev nodeLoc
                    let res = aux2 subsOld subsNew [] [] revList 0
                    res
            else
                [UpNode (NodeLoc nodeLoc,(Tree (nodeNew,subsNew)))]

        let updateProperties = propertiesDifferences (windowOld.Properties) (windowNew.Properties) (NodeLoc [])
        let updateEvents     = eventsDifferences (windowOld.Events) (windowNew.Events) (NodeLoc [])
        let updates = aux (windowOld.Tree) (windowNew.Tree) (NodeLoc [0])
        updateEvents::updateProperties@updates

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
            w.Content <- new Controls.Grid()
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
                                let updates = treeDiff oldView newWindow

                                do! Async.SwitchToContext sync
                                updateWindow window updates
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

                let nodeElementGrid = 
                    { Tag = Tag.Grid
                      Properties = VProperties []
                      Events = VEvents [] }
                
                let tree = Tree (nodeElementGrid,[])
                { Tree = tree
                  Properties = VProperties []
                  Events = VEvents [] }

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

