

// TODO : 
//  - Making sure of the correctness of the FFT calculation (find a cailbrated sound a create a DBSpl relation from that)
//  - Use tab controls etc... to display different VU-Meters
//  - build a small graph library more performant than just simple progressbar to show charts (based on wpf shapes ?)

module types =
    open NAudio.Wave
    open System

    type MeterType =
        | RMSMeter
        | PeakMeter
        | PeakToPeakMeter
        | FFTMeter
    
    type MeterResult =
        | RMSResult of float
        | PeakResult of float
        | PeakToPeakResult of float
        | FFTResult of Map<int*int,float> 

    type Model = 
        { waveIn : WaveInEvent
          Data : MeterResult 
          currentHandler : EventHandler<WaveInEventArgs> 
          meterDispatcher : MeterType -> unit 
          sampleDispatcher : float -> unit }

    type Msg = 
        | Play
        | Stop
        | NewHandler of EventHandler<WaveInEventArgs>
        | MeterDispatcher of (MeterType -> unit)
        | SampleDispatcher of (float -> unit)
        | ChangeAlgorithm of string
        | DataDisplay of MeterResult


module simpleViewer =
    open NAudio.Dsp
    open types
    open Elmish.ProgramTypes

    let sampleRate = 100000 // 44.1kHz
    let power = 10
    let fftSamplingRate = pown 2 power
    let otherSamplingRate = 4410
    let bands = 20

        
    let peakValue (data:float list) =
        data
        |> List.max
        |> PeakResult

    let peakToPeakValue (data:float list) =
        (data |> List.max,data |> List.min)
        |> fun (max,min) -> (max - min)/2.        
        |> PeakToPeakResult

    let rmsCalc (data:float list) =
        data
        |> List.map(fun x -> x*x)
        |> List.average
        |> sqrt

    let rmsValue = rmsCalc >> RMSResult

    
    let fftValue (data:float list) =
        //let maxfrequency = sampleRate / 20
        let maxfrequency = 8000 // 1kHz

        let map = 
            let mutable f = Map.empty<int*int,float list>
            for i in 0..(bands-1) do
                f <- f.Add ( (i*(maxfrequency/bands),(i+1)*(maxfrequency/bands)) , [] )
            f
        
        let isBetween ((min,max): int * int) (i:int) = i >= min && i < max

        let toFFT (data:float []) =
            let data = 
                data
                |> Array.mapi( fun index value ->
                    let mutable c = new Complex()
                    c.X <- (float32) (value * FastFourierTransform.HammingWindow(index, fftSamplingRate))
                    c.Y <- 0.f
                    c
                   )
            FastFourierTransform.FFT(true, power , data);
            data

        data
        |> List.rev
        |> List.toArray
        |> toFFT
        |> Array.skip 1
        |> Array.take (fftSamplingRate/2)
        |> Array.mapi(fun index v -> (index * sampleRate / fftSamplingRate,v) )
        |> Array.fold(fun (min,max,map) (freq,c) -> 
            if freq |> isBetween (min,max) then
                let l = Map.find (min,max) map
                let l = (c.X |> float)::l
                (min,max,map.Add ((min,max),l) )
            elif freq <= maxfrequency then
                let min = max
                let max = max + maxfrequency/bands
                let l = Map.find (min,max) map
                let l = (c.X |> float)::l
                (min,max,map.Add ((min,max),l) )    
            else
                (min,max,map)    
                
           ) (0,maxfrequency/bands,map)
        |> (fun (_,_,map) -> map)
        |> Map.map (fun _ v -> List.max v)
        |> FFTResult

    type MeterType with
        member x.GetValue (data:float list) =
            match x with
            | RMSMeter  -> rmsValue data
            | PeakMeter -> peakValue data
            | PeakToPeakMeter -> peakToPeakValue data
            | FFTMeter  -> fftValue data
        member x.SamplingRate =
            match x with
            | RMSMeter  
            | PeakMeter 
            | PeakToPeakMeter -> otherSamplingRate
            | FFTMeter -> fftSamplingRate
        static member All =
            FSharp.Reflection.FSharpType.GetUnionCases(typeof<MeterType>)
            |> Array.map( fun uc -> uc.Name )
            |> Array.toList
        static member From (str:string) =
            FSharp.Reflection.FSharpType.GetUnionCases(typeof<MeterType>)
            |> Array.find( fun uc -> uc.Name = str )
            |> (fun uc -> FSharp.Reflection.FSharpValue.MakeUnion(uc,[||]) :?> MeterType)

    type Agent<'a> = MailboxProcessor<'a>    
    type MsgAgent =
        | NewSample of float
        | NewMeter of MeterType

    type SimpleViewer(meter:MeterType,dispatch:Dispatch<Msg>) =
        let mutable meter = meter
        let samplingRate = meter.SamplingRate

        let sampleProcessor (inbox:Agent<MsgAgent>) =
            let rec aux (samples:float list) (sampleNumber:int) =
                async{
                    let! msg = inbox.Receive()
                    match msg with
                    | NewSample sample ->
                        if sampleNumber = samplingRate then
                            let level = meter.GetValue samples  // [0. .. 1.]
                            dispatch (DataDisplay level)
                            return! aux [] 0
                        else
                            return! aux (sample::samples) (sampleNumber + 1)     
                    | NewMeter newMeter ->
                        meter <- newMeter
                        return! aux samples sampleNumber     
                }

            aux [] 0
        
        let agent = Agent<MsgAgent>.Start sampleProcessor

        member __.NewSample(sample32) = agent.Post (NewSample sample32)
        member __.NewMeter(meter) = agent.Post (NewMeter meter)
            


module test =
    open System
    open NAudio.Wave
    open simpleViewer
    open Elmish.ProgramTypes
    open types

    let processSample (sampleDispatcher:float -> unit) (sample32:float) = 
        sampleDispatcher sample32

    let toPcmSample (data1:byte) (data2:byte) =
        let data1 = data1 |> int16 
        let data2 = data2 |> int16 
        let sample = (data2 <<< 8 ||| data1) |> float
        ( sample / (pown 2. 15) )


    let processSignal (sampleDispatcher:float -> unit) (data:WaveInEventArgs) =
        for index in 0..2..(data.BytesRecorded-1)  do 
            let pcmSample = toPcmSample (data.Buffer.[index]) (data.Buffer.[index + 1])
            processSample sampleDispatcher pcmSample
    
    let buildMicrophone () =       
        let waveIn = new WaveInEvent()
        waveIn
        
    let addProcessing (waveIn:WaveInEvent) (dispatch : Dispatch<Msg>) (meter:MeterType) =
        async{
            let simpleViewer = new SimpleViewer(meter,dispatch)
            let channelCount = 1 // mono

            waveIn.DeviceNumber <- 0 
            let handler = EventHandler<WaveInEventArgs>(fun _ arg -> processSignal (simpleViewer.NewSample) arg)
            waveIn.DataAvailable.AddHandler handler

            waveIn.WaveFormat <- new WaveFormat(sampleRate,channelCount)
            dispatch (NewHandler handler)
            dispatch (MeterDispatcher (simpleViewer.NewMeter))
            dispatch (SampleDispatcher (simpleViewer.NewSample))
        }
                
    let updateProcessingAlgorithm (waveIn:WaveInEvent) 
                                  (meter:MeterType)
                                  meterDispatcher 
                                  sampleDispatcher 
                                  (dispatch : Dispatch<Msg>) previousHandler =
        async{
            dispatch Stop
            
            waveIn.DataAvailable.RemoveHandler previousHandler
            let newHandler = EventHandler<WaveInEventArgs>(fun _ arg -> processSignal sampleDispatcher arg)
            waveIn.DataAvailable.AddHandler newHandler

            meterDispatcher meter
            dispatch (NewHandler newHandler)
            dispatch Play
        }


module MVU =
    open System
    open test
    open System.Windows
    open Elmish.DSL.DSL
    open Elmish.ProgramTypes
    open Elmish.VDom.VirtualProperty.FsWPFRepresentation
    open System.Windows.Controls
    open types
    open simpleViewer
    open NAudio.Wave

    let init () = 
        let initModel = 
            { waveIn = buildMicrophone()
              Data = FFTResult (Map.empty<int * int, float>) 
              currentHandler = EventHandler<WaveInEventArgs>(fun _ _ -> ()) 
              meterDispatcher = fun _ -> ()
              sampleDispatcher = fun _ -> ()    }

        initModel,Cmd.AsyncDispatcher (fun dispatch -> addProcessing (initModel.waveIn) dispatch FFTMeter )

            

    let update msg (model:Model) =  
        match msg with 
        | Play -> 
            model.waveIn.StartRecording()
            Success model,NoEffect
        | Stop -> 
            (model.waveIn.StopRecording())
            Success model,NoEffect
        | ChangeAlgorithm str ->
            let meter = MeterType.From str
            Success model ,AsyncDispatcher (fun dispatch -> updateProcessingAlgorithm (model.waveIn) meter (model.meterDispatcher) (model.sampleDispatcher) dispatch (model.currentHandler))
        | DataDisplay data ->
            Success { model with Data = data } ,NoEffect
        | NewHandler handler ->
            Success { model with currentHandler = handler } ,NoEffect
        | MeterDispatcher meterDispatcher ->
            Success { model with meterDispatcher = meterDispatcher } ,NoEffect
        | SampleDispatcher sampleDispatcher ->
            Success { model with sampleDispatcher = sampleDispatcher } ,NoEffect
        
        

    let viewProgressBars data =
        [ let mutable i = 0
          for KeyValue ((min,max),v) in data do
            i <- i + 1
            yield WPF.progressBar( style = new Style(Column = i-1,Row = 0), Value = v , Minimum = 0. , Maximum = 0.05 , Orientation=Orientation.Vertical)
            yield WPF.textBlock( style = new Style(Column = i-1,Row = 1,HorizontalAlignment = HorizontalAlignment.Center), Text = sprintf "%A" ((min + max) / 2)  )
        ]         

    let viewGrid (model:Model) =
        match model.Data with
        | RMSResult v
        | PeakResult v
        | PeakToPeakResult v ->
            WPF.progressBar( style = new Style(Row = 1, ColumnSpan = 3,HorizontalAlignment = HorizontalAlignment.Center, Width = 60.), Value = v , Minimum = 0. , Maximum = 1. , Orientation=Orientation.Vertical)
        | FFTResult data ->
            WPF.grid( style = new Style( Row = 1 , ColumnSpan = 3) 
                     ,ColumnDefinitions = [ for _ in 1..bands -> {Width =1;Unit=GridUnitType.Star} ]
                     ,RowDefinitions = [ for _ in 1..2 -> {Height= 1;Unit=GridUnitType.Star} ]
                     ,Children = viewProgressBars data)

    let viewGridChildren (model:Model) = 
        [   yield WPF.button( style = new Style(Row = 0,Column = 0) ,
                              Content = ("Play!" |> box), 
                              Click = fun _ -> Play ) 
            yield WPF.button( style = new Style(Row = 0,Column = 1) ,
                              Content = ("Stop!" |> box), 
                              Click = fun _ -> Stop ) 
            yield WPF.comboBox( children = [ for str in MeterType.All -> WPF.textBlock(Text = str) ] ,
                                style = new Style(Row = 0,Column = 2) ,
                                SelectionChanged = (fun args -> (args.AddedItems.Item 0 :?> TextBlock).Text |> ChangeAlgorithm) )
            yield viewGrid model
        ]

    
    let view (model:Model) =
        WPF.window( WPF.grid( RowDefinitions = 
                                [  {Height=1;Unit=GridUnitType.Star}
                                   {Height=5;Unit=GridUnitType.Star}
                                ],  
                              ColumnDefinitions =
                                [  {Width =1;Unit=GridUnitType.Star}
                                   {Width =1;Unit=GridUnitType.Star}
                                   {Width =1;Unit=GridUnitType.Star}
                                ],  
                              Children = viewGridChildren model ),
                    style = 
                        new Style(Width = 600., 
                                  Height = 600.) )                


open MVU
open System
open Elmish.Program

[<STAThread>]
[<EntryPoint>]
let main argv = 
    
    mkMVUProgram init view update
    |> run
    
    0



     