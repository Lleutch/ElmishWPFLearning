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
        | IsActive            of bool 
        // Layout Related
        | ColumnDefinitions   of ColumnDefinitionCollection 
        | RowDefinitions      of RowDefinitionCollection 
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
        | IsVisible           of bool 
        | Visibility          of Visibility 
        | Opacity             of float 

    type VProperties = VProperties of VProperty list  





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

    type ViewWindow = ViewWindow of NodeElement * Tree list


module Dom =
// TODO : define functions that allow simple usage of the DOM in a safer maner
    open VDom
    open System.Windows.Controls
    open System.Windows.Media
    open System.Windows.Input
    open System
    open System.ComponentModel

    let internal bindVProperties (x:'a option) (constructor: 'a -> VProperty) (list:VProperty list)  =
        match x with
        | None -> list
        | Some a -> (constructor a)::list
    
    let internal bindVEvents (x:option<'a -> unit>) (constructor: EvLambda<'a> -> VEvent) (list:VEvent list) =
        match x with
        | None -> list
        | Some a -> (constructor a)::list

    
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
            IsVisible           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            []
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
            |> bindVProperties x.IsVisible           VProperty.IsVisible          
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
                IsVisible           = None 
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
        let vprops  = properties.VProperties()
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
            IsVisible           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            Padding             : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            []
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
            |> bindVProperties  x.IsVisible             VProperty.IsVisible          
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
                IsVisible           = None
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
        let vprops  = properties.VProperties()
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
            ColumnDefinitions   : ColumnDefinitionCollection option
            RowDefinitions      : RowDefinitionCollection option
            Background          : Brush option 
            IsEnabled           : bool option 
            IsVisible           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            []
            |> bindVProperties  x.Column                VProperty.Column
            |> bindVProperties  x.ColumnSpan            VProperty.ColumnSpan         
            |> bindVProperties  x.Row                   VProperty.Row   
            |> bindVProperties  x.RowSpan               VProperty.RowSpan         
            |> bindVProperties  x.ColumnDefinitions     VProperty.ColumnDefinitions         
            |> bindVProperties  x.RowDefinitions        VProperty.RowDefinitions       
            |> bindVProperties  x.Background            VProperty.Background      
            |> bindVProperties  x.IsEnabled             VProperty.IsEnabled          
            |> bindVProperties  x.IsVisible             VProperty.IsVisible          
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
                IsVisible           = None  
                Width               = None  
                Height              = None  
                Opacity             = None  
                HorizontalAlignment = None  
                Margin              = None  
                VerticalAlignment   = None  
                Visibility          = None  }

    let grid (properties:GridProperties) (children:Tree list) =
        let vprops  = properties.VProperties()
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
            IsActive            : bool option
            Background          : Brush option 
            IsEnabled           : bool option 
            IsVisible           : bool option 
            Width               : float option
            Height              : float option
            Opacity             : float option
            HorizontalAlignment : HorizontalAlignment option
            Margin              : Thickness option
            VerticalAlignment   : VerticalAlignment option
            Visibility          : Visibility option     }
        member internal x.VProperties() =
            []
            |> bindVProperties  x.WindowStyle         VProperty.WindowStyle         
            |> bindVProperties  x.WindowState         VProperty.WindowState         
            |> bindVProperties  x.Title               VProperty.Title    
            |> bindVProperties  x.ResizeMode          VProperty.ResizeMode         
            |> bindVProperties  x.AllowsTransparency  VProperty.AllowsTransparency         
            |> bindVProperties  x.IsActive            VProperty.IsActive
            |> bindVProperties  x.Background          VProperty.Background         
            |> bindVProperties  x.IsEnabled           VProperty.IsEnabled          
            |> bindVProperties  x.IsVisible           VProperty.IsVisible          
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
                IsActive            = None  
                Background          = None  
                IsEnabled           = None  
                IsVisible           = None  
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
            { TextInput = None }

    let window (properties:WindowProperties) (events:WindowEvents) (children:Tree list) =
        let vprops  = properties.VProperties()
        let vevents = events.VEvents()
        let node =
            { Tag        = Tag.Window
              Properties = vprops   |> VProperties
              Events     = vevents  |> VEvents  }
        ViewWindow (node, children)




module VDomConverter =
    open System.Windows.Controls

// TODO : 
//  - Define differenciation of VDOM
//  - Tag VDOM such that when traversing it in parallel to real DOM
//    we can know which part of the real DOM we need to update
//  - Traverse real DOM in parallel to VDOM
    



    let f () = ()


module Program =
    open System.Threading

    type Agent<'a> = MailboxProcessor<'a>

    type AViewType = Window


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
          errorHandler  : AViewType -> SynchronizationContext -> IError -> Async<unit>}

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

    type Agent<'a> = MailboxProcessor<'a>

    type MessageProcessor<'Msg,'Model>(funcs:FuncProgram<'Msg,'Model>) =
        
        let modelProcessor (program:Program<'Msg,'Model>) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
            let rec aux (program:Program<'Msg,'Model>) (initial:bool) (window:Window) (sync:SynchronizationContext) (inbox:Agent<'Msg>) =
                async{
                    if initial then
                        do! Async.SwitchToContext sync
                        let newWindow = program.funcs.view (inbox.Post) (program.model)
                        let content = newWindow.Content
                        window.Content <- content
                        do! Async.SwitchToThreadPool ()


                    let! msg = inbox.Receive()


                    let UMModel = program.funcs.update msg (program.model)

                    match UMModel with
                    | Success model ->
                        let program = { program with model = model }

                        do! Async.SwitchToContext sync
                        let newWindow = program.funcs.view (inbox.Post) (program.model)
                        let content = newWindow.Content
                        window.Content <- content
                        do! Async.SwitchToThreadPool ()
                
                        return! aux program false window sync inbox
                    | Error error ->
                        return! program.funcs.errorHandler window sync error
                }
            
            aux program true window sync inbox
        
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
        let errorHandler (window:AViewType) sync (error:IError) = 
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

