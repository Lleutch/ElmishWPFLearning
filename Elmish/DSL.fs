namespace Elmish

module DSL =    


    module DSLDomain =
        open VDom.VDomTypes
        open System.Windows
        open System.Windows.Controls
        open System.Windows.Media
        open System.Windows.Input
        open System
        open System.ComponentModel

        let private bindVProperties (x:'a option) (constructor: 'a -> VProperty) (listAndIndex:((VProperty*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> ((constructor a,index)::list,index)
    
        let private bindVEvents (x:option<'a -> unit>) (builder: (obj -> 'a -> unit) -> 'b) (constructor: 'b -> VEvent) (listAndIndex:((VEvent*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> 
                let event = builder (fun _ element -> a element)
                ((constructor event,index)::list,index)

            
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





    module DSL =
        open VDom.VDomTypes
        open DSLDomain



        let button (properties:ButtonProperties) (events:ButtonEvents) =
            let vprops  = properties.VProperties() |> fst
            let vevents = events.VEvents() |> fst
            let node =
                { Tag        = Tag.Button
                  Properties = vprops  |> VProperties
                  Events     = vevents |> VEvents  }
            Tree (node, [])


        let textBlock (properties:TextBlockProperties) (events:TextBlockEvents)  =
            let vprops  = properties.VProperties() |> fst
            let vevents = events.VEvents() |> fst
            let node =
                { Tag        = Tag.TextBlock
                  Properties = vprops  |> VProperties
                  Events     = vevents |> VEvents  }
            Tree (node, [])


        let grid (properties:GridProperties) (children:Tree list) =
            let vprops  = properties.VProperties() |> fst
            let node =
                { Tag        = Tag.Grid
                  Properties = vprops   |> VProperties
                  Events     = []       |> VEvents  }
            Tree (node, children)


        let window (properties:WindowProperties) (events:WindowEvents) (children:Tree) =
            let vprops  = properties.VProperties() |> fst
            let vevents = events.VEvents() |> fst
            { Tree = children
              Properties = vprops   |> VProperties
              Events     = vevents  |> VEvents  }

