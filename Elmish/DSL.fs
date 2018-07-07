namespace Elmish

module DSL =    


    module DSLHelpers =
        open VDom.VDomTypes

        let internal bindVProperties (x:'a option) (constr: 'a -> VProperty) (listAndIndex:((VProperty*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> ((constr a,index)::list,index)
    
        let internal  bindWPFEvents (x:option<'a -> 'Msg>) (builder: (obj -> 'a -> unit) -> 'b) (constr: WPFLambda<'Msg,'a,'b> -> WPFEvent<'Msg>) (listAndIndex:((WPFEvent<'Msg>*int) list * int)) =
            let (list,index) = listAndIndex
            let index = index + 1
            match x with
            | None -> (list,index)
            | Some a -> ((constr (a,builder),index)::list,index)


    module DSL =
        open VDom.VDomTypes
        open DSLHelpers
        open System.Windows.Media
        open System.Windows
        open System.Windows.Input
        open System
        open System.ComponentModel

                //let column              = defaultArg column              -1
                //let columnSpan          = defaultArg columnSpan          -1
                //let row                 = defaultArg row                 -1
                //let rowSpan             = defaultArg rowSpan             -1
                //let background          = defaultArg background          null
                //let borderBrush         = defaultArg borderBrush         null
                //let borderThickness     = defaultArg borderThickness     (new Thickness(0.,0.,0.,0.))
                //let content             = defaultArg content             null
                //let fontFamily          = defaultArg fontFamily          (FontFamily("Segoe UI"))
                //let fontSize            = defaultArg fontSize            12.0
                //let fontWeight          = defaultArg fontWeight          FontWeights.Normal
                //let foreground          = defaultArg foreground          Brushes.Black
                //let isEnabled           = defaultArg isEnabled           true
                //let width               = defaultArg width               nan
                //let height              = defaultArg height              nan
                //let opacity             = defaultArg opacity             1.0
                //let horizontalAlignment = defaultArg horizontalAlignment HorizontalAlignment.Stretch
                //let margin              = defaultArg margin              (new Thickness(0.,0.,0.,0.))
                //let verticalAlignment   = defaultArg verticalAlignment   VerticalAlignment.Stretch
                //let visibility          = defaultArg visibility          Visibility.Visible         


        type WPF =    

            (*** ****************** ***) 
            (***      Button        ***) 
            (*** ****************** ***) 
            static member button( ?Column               : int 
                                 ,?ColumnSpan           : int 
                                 ,?Row                  : int 
                                 ,?RowSpan              : int 
                                 ,?Background           : Brush 
                                 ,?BorderBrush          : Brush 
                                 ,?BorderThickness      : Thickness 
                                 ,?Content              : obj  
                                 ,?FontFamily           : FontFamily 
                                 ,?FontSize             : float  
                                 ,?FontWeight           : FontWeight 
                                 ,?Foreground           : Brush  
                                 ,?IsEnabled            : bool 
                                 ,?Width                : float 
                                 ,?Height               : float 
                                 ,?Opacity              : float 
                                 ,?HorizontalAlignment  : HorizontalAlignment 
                                 ,?Margin               : Thickness 
                                 ,?VerticalAlignment    : VerticalAlignment 
                                 ,?Visibility           : Visibility  
                                 ,?Click                : RoutedEventArgs -> 'Msg ) = 
                           
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties Column               VProperty.Column         
                    |> bindVProperties ColumnSpan           VProperty.ColumnSpan         
                    |> bindVProperties Row                  VProperty.Row
                    |> bindVProperties RowSpan              VProperty.RowSpan         
                    |> bindVProperties Background           VProperty.Background         
                    |> bindVProperties BorderBrush          VProperty.BorderBrush        
                    |> bindVProperties BorderThickness      VProperty.BorderThickness    
                    |> bindVProperties Content              VProperty.Content     
                    |> bindVProperties FontFamily           VProperty.FontFamily         
                    |> bindVProperties FontSize             VProperty.FontSize          
                    |> bindVProperties FontWeight           VProperty.FontWeight         
                    |> bindVProperties Foreground           VProperty.Foreground         
                    |> bindVProperties IsEnabled            VProperty.IsEnabled          
                    |> bindVProperties Width                VProperty.Width           
                    |> bindVProperties Height               VProperty.Height             
                    |> bindVProperties Opacity              VProperty.Opacity            
                    |> bindVProperties HorizontalAlignment  VProperty.HorizontalAlignment
                    |> bindVProperties Margin               VProperty.Margin 
                    |> bindVProperties VerticalAlignment    VProperty.VerticalAlignment  
                    |> bindVProperties Visibility           VProperty.Visibility                        
                    |> fst
                let bindedVEvents () =
                    ([],0)
                    |> bindWPFEvents Click (fun x -> new RoutedEventHandler(x)) WPFClick                    
                    |> fst

                let vprops  = bindedVProperties ()
                let vevents = bindedVEvents ()
                let node =
                    { Tag        = Tag.Control Button
                      Properties = vprops  |> VProperties
                      WPFEvents  = vevents |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])                



            (*** ****************** ***) 
            (***      TextBlock     ***) 
            (*** ****************** ***) 
            static member textBlock( ?Column                : int 
                                    ,?ColumnSpan            : int 
                                    ,?Row                   : int 
                                    ,?RowSpan               : int 
                                    ,?Background            : Brush 
                                    ,?Text                  : string 
                                    ,?TextAlignment         : TextAlignment 
                                    ,?TextWrapping          : TextWrapping 
                                    ,?FontFamily            : FontFamily 
                                    ,?FontSize              : float 
                                    ,?FontWeight            : FontWeight 
                                    ,?Foreground            : Brush 
                                    ,?IsEnabled             : bool 
                                    ,?Width                 : float 
                                    ,?Height                : float 
                                    ,?Opacity               : float 
                                    ,?HorizontalAlignment   : HorizontalAlignment 
                                    ,?Margin                : Thickness 
                                    ,?Padding               : Thickness 
                                    ,?VerticalAlignment     : VerticalAlignment 
                                    ,?Visibility            : Visibility                
                                    ,?TextInput             : TextCompositionEventArgs -> 'Msg ) =                   
            
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  Column                VProperty.Column
                    |> bindVProperties  ColumnSpan            VProperty.ColumnSpan         
                    |> bindVProperties  Row                   VProperty.Row   
                    |> bindVProperties  RowSpan               VProperty.RowSpan         
                    |> bindVProperties  Background            VProperty.Background         
                    |> bindVProperties  TextAlignment         VProperty.TextAlignment        
                    |> bindVProperties  TextWrapping          VProperty.TextWrapping    
                    |> bindVProperties  Text                  VProperty.Text     
                    |> bindVProperties  FontFamily            VProperty.FontFamily         
                    |> bindVProperties  FontSize              VProperty.FontSize          
                    |> bindVProperties  FontWeight            VProperty.FontWeight         
                    |> bindVProperties  Foreground            VProperty.Foreground         
                    |> bindVProperties  IsEnabled             VProperty.IsEnabled          
                    |> bindVProperties  Width                 VProperty.Width           
                    |> bindVProperties  Height                VProperty.Height             
                    |> bindVProperties  Opacity               VProperty.Opacity            
                    |> bindVProperties  HorizontalAlignment   VProperty.HorizontalAlignment
                    |> bindVProperties  Margin                VProperty.Margin 
                    |> bindVProperties  Padding               VProperty.Padding             
                    |> bindVProperties  VerticalAlignment     VProperty.VerticalAlignment  
                    |> bindVProperties  Visibility            VProperty.Visibility       
                    |> fst

                let bindedVEvents () =
                    ([],0)
                    |> bindWPFEvents TextInput (fun x -> new TextCompositionEventHandler(x)) WPFTextInput
                    |> fst

                let vprops  = bindedVProperties ()
                let vevents = bindedVEvents ()
                let node =
                    { Tag        = Tag.Control TextBlock
                      Properties = vprops  |> VProperties
                      WPFEvents  = vevents |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, [])      


            (*** ****************** ***) 
            (***      Grid          ***) 
            (*** ****************** ***) 
            static member grid( ?Column             : int 
                               ,?ColumnSpan         : int 
                               ,?Row                : int 
                               ,?RowSpan            : int 
                               ,?ColumnDefinitions  : ColDef list 
                               ,?RowDefinitions     : RowDef list 
                               ,?Background         : Brush  
                               ,?IsEnabled          : bool 
                               ,?Width              : float 
                               ,?Height             : float 
                               ,?Opacity            : float 
                               ,?HorizontalAlignment: HorizontalAlignment 
                               ,?Margin             : Thickness 
                               ,?VerticalAlignment  : VerticalAlignment 
                               ,?Visibility         : Visibility   
                               ,?Children           : WPFTree<'Msg> list  ) =
                                    
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  Column                VProperty.Column
                    |> bindVProperties  ColumnSpan            VProperty.ColumnSpan         
                    |> bindVProperties  Row                   VProperty.Row   
                    |> bindVProperties  RowSpan               VProperty.RowSpan         
                    |> bindVProperties  ColumnDefinitions     VProperty.ColumnDefinitions         
                    |> bindVProperties  RowDefinitions        VProperty.RowDefinitions       
                    |> bindVProperties  Background            VProperty.Background      
                    |> bindVProperties  IsEnabled             VProperty.IsEnabled          
                    |> bindVProperties  Width                 VProperty.Width           
                    |> bindVProperties  Height                VProperty.Height             
                    |> bindVProperties  Opacity               VProperty.Opacity            
                    |> bindVProperties  HorizontalAlignment   VProperty.HorizontalAlignment
                    |> bindVProperties  Margin                VProperty.Margin 
                    |> bindVProperties  VerticalAlignment     VProperty.VerticalAlignment  
                    |> bindVProperties  Visibility            VProperty.Visibility      
                    |> fst

                let vprops  = bindedVProperties ()
                let node =
                    { Tag        = Tag.Container Grid
                      Properties = vprops   |> VProperties
                      WPFEvents  = [] |> WPFEvents 
                      Events     = [] |> VEvents  }
                WPFTree (node, defaultArg Children [])


            (*** ****************** ***) 
            (***      StackPanel    ***) 
            (*** ****************** ***) 
            static member stackPanel( ?Column               : int 
                                     ,?ColumnSpan           : int 
                                     ,?Row                  : int 
                                     ,?RowSpan              : int 
                                     ,?Background           : Brush 
                                     ,?IsEnabled            : bool 
                                     ,?Width                : float 
                                     ,?Height               : float 
                                     ,?Opacity              : float 
                                     ,?HorizontalAlignment  : HorizontalAlignment 
                                     ,?Margin               : Thickness 
                                     ,?VerticalAlignment    : VerticalAlignment 
                                     ,?Visibility           : Visibility    
                                     ,?Children             : WPFTree<'Msg> list  ) =
                                     
                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  Column                VProperty.Column
                    |> bindVProperties  ColumnSpan            VProperty.ColumnSpan         
                    |> bindVProperties  Row                   VProperty.Row   
                    |> bindVProperties  RowSpan               VProperty.RowSpan         
                    |> bindVProperties  Background            VProperty.Background      
                    |> bindVProperties  IsEnabled             VProperty.IsEnabled          
                    |> bindVProperties  Width                 VProperty.Width           
                    |> bindVProperties  Height                VProperty.Height             
                    |> bindVProperties  Opacity               VProperty.Opacity            
                    |> bindVProperties  HorizontalAlignment   VProperty.HorizontalAlignment
                    |> bindVProperties  Margin                VProperty.Margin 
                    |> bindVProperties  VerticalAlignment     VProperty.VerticalAlignment  
                    |> bindVProperties  Visibility            VProperty.Visibility  
                    |> fst

                let vprops  = bindedVProperties ()
                let node =
                    { Tag        = Tag.Container StackPanel
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
                                 ,?Background           : Brush 
                                 ,?IsEnabled            : bool 
                                 ,?Width                : float 
                                 ,?Height               : float 
                                 ,?Opacity              : float 
                                 ,?HorizontalAlignment  : HorizontalAlignment 
                                 ,?Margin               : Thickness 
                                 ,?VerticalAlignment    : VerticalAlignment 
                                 ,?Visibility           : Visibility     
                                 ,?Activated            : EventArgs -> 'Msg 
                                 ,?Closed               : EventArgs -> 'Msg
                                 ,?Closing              : CancelEventArgs -> 'Msg
                                 ,?Deactivated          : EventArgs -> 'Msg
                                 ,?Loaded               : RoutedEventArgs -> 'Msg  ) =

                

                let bindedVProperties () =
                    ([],0)
                    |> bindVProperties  WindowStyle         VProperty.WindowStyle         
                    |> bindVProperties  WindowState         VProperty.WindowState         
                    |> bindVProperties  Title               VProperty.Title    
                    |> bindVProperties  ResizeMode          VProperty.ResizeMode         
                    |> bindVProperties  AllowsTransparency  VProperty.AllowsTransparency         
                    |> bindVProperties  Background          VProperty.Background         
                    |> bindVProperties  IsEnabled           VProperty.IsEnabled          
                    |> bindVProperties  Width               VProperty.Width           
                    |> bindVProperties  Height              VProperty.Height             
                    |> bindVProperties  Opacity             VProperty.Opacity            
                    |> bindVProperties  HorizontalAlignment VProperty.HorizontalAlignment
                    |> bindVProperties  Margin              VProperty.Margin 
                    |> bindVProperties  VerticalAlignment   VProperty.VerticalAlignment  
                    |> bindVProperties  Visibility          VProperty.Visibility      
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

