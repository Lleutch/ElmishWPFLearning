namespace Elmish


module VDom =


    module VDomTypes =
        open System.Windows
        open System.Windows.Controls
        open System.Windows.Media
        open System.Windows.Input
        open System
        open System.ComponentModel

        type ColDef = 
            { Width : int
              Unit  : GridUnitType }

        type RowDef = 
            { Height : int
              Unit   : GridUnitType }

        type VProperty =
            // Window Related
            | WindowStyle         of WindowStyle
            | WindowState         of WindowState
            | Title               of string
            | ResizeMode          of ResizeMode
            | AllowsTransparency  of bool 
            // Layout Related
            | ColumnDefinitions   of ColDef list
            | RowDefinitions      of RowDef list
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

        type TaggedContainer =
            | Grid
            | StackPanel
            
        type TaggedControl  =
            | Button 
            | TextBlock
 
        type Tag =
            | Container of TaggedContainer
            | Control   of TaggedControl

        type NodeElement = 
            { Tag : Tag
              Properties : VProperties
              Events : VEvents }

        type Tree = Tree of NodeElement * Tree list

        type ViewWindow = 
            { Properties : VProperties
              Events : VEvents 
              Tree : Tree }
       




    module VDomExt =
        open System.Windows
        open System.Windows.Controls
        open VDomTypes




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
                | Container Grid        -> new Grid()       :> UIElement
                | Container StackPanel  -> new VirtualizingStackPanel() :> UIElement
                | Control   Button      -> new Button()     :> UIElement
                | Control   TextBlock   -> new TextBlock()  :> UIElement

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
                    | ColumnDefinitions   cd    -> GridUpdate (fun gr -> cd |> List.iter(fun c -> gr.ColumnDefinitions.Add(c.Convert())) ) 
                    | RowDefinitions      rd    -> GridUpdate (fun gr -> rd |> List.iter(fun r -> gr.RowDefinitions.Add(r.Convert())) ) 
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
            | []        -> UpProperties (nodeLoc, VProperties [])
            | vprops    -> UpProperties (nodeLoc, VProperties vprops)                

        let treeDiff (windowOld:ViewWindow) (windowNew:ViewWindow) =
            let rec aux (Tree (nodeOld,subsOld)) (Tree (nodeNew,subsNew)) (NodeLoc nodeLoc) =
                if nodeOld.Tag = nodeNew.Tag then
                    let upProps  = propertiesDifferences (nodeOld.Properties) (nodeNew.Properties) (NodeLoc nodeLoc)
                    let upEvents = eventsDifferences (nodeOld.Events) (nodeNew.Events) (NodeLoc nodeLoc)
                    let updates = 
                        match subsOld,subsNew with
                        | [] , [] -> []
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
                    upEvents::upProps::updates
                else
                    [UpNode (NodeLoc nodeLoc,(Tree (nodeNew,subsNew)))]

            let updateProperties = propertiesDifferences (windowOld.Properties) (windowNew.Properties) (NodeLoc [])
            let updateEvents     = eventsDifferences (windowOld.Events) (windowNew.Events) (NodeLoc [])
            let updates = aux (windowOld.Tree) (windowNew.Tree) (NodeLoc [0])
            updateEvents::updateProperties::updates

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

