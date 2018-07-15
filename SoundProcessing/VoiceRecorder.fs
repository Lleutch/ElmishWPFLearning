

// TODO : 
//  - Making sure of the correctness of the FFT calculation (find a cailbrated sound a create a DBSpl relation from that)
//  - Use tab controls etc... to display different VU-Meters
//  - build a small graph library more performant than just simple progressbar to show charts (based on wpf shapes ?)

module types =
    open NAudio.Wave

    type Model = 
        { waveIn : WaveInEvent
          Data : Map<int * int, float> }

    type Msg = 
        | Play
        | Stop
        | DataDisplay of Map<int * int, float>


module simpleViewer =
    open NAudio.Dsp
    open types
    open Elmish.ProgramTypes

    let sampleRate = 44100 // 44.1kHz
    let power = 12
    let fftSamplingRate = pown 2 power
    let otherSamplingRate = 4410
    let bands = 10

    let printing level =
        //let spaces = String.replicate (level) " "
        printf "%s" (String.replicate 60 " ")
        System.Console.SetCursorPosition(0, System.Console.CursorTop)
        //printf "%s|" spaces
        printf "%s %i" (String.replicate 30 " ") level
        System.Console.SetCursorPosition(0, System.Console.CursorTop)


    type MeterResult =
        | RMSResult of float
        | PeakResult of float
        | PeakToPeakResult of float
        | FFTResult of Map<int*int,float> 
        
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
        let halfSampleRate = sampleRate / 2 
        let map = 
            let mutable f = Map.empty<int*int,float list>
            for i in 0..(bands-1) do
                f <- f.Add ( (i*(halfSampleRate/bands),(i+1)*(halfSampleRate/bands)) , [] )
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
            FastFourierTransform.FFT(true, power - 1 , data);
            data

        data
        |> List.rev
        |> List.take (fftSamplingRate/2)
        |> List.toArray
        |> toFFT
        |> Array.mapi(fun index v -> (index * sampleRate / fftSamplingRate,v) )
        |> Array.fold(fun (min,max,map) (freq,c) -> 
            if freq |> isBetween (min,max) then
                let l = Map.find (min,max) map
                let l = (c.X |> float)::l
                (min,max,map.Add ((min,max),l) )
            else
                let min = max
                let max = max + halfSampleRate/bands
                let l = Map.find (min,max) map
                let l = (c.X |> float)::l
                (min,max,map.Add ((min,max),l) )                
           ) (0,halfSampleRate/bands,map)
        |> (fun (_,_,map) -> map)
        |> Map.map (fun _ v -> List.max v)
        |> FFTResult

    type MeterType =
        | RMSMeter
        | PeakMeter
        | PeakToPeakMeter
        | FFTMeter
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
        


    type Agent<'a> = MailboxProcessor<'a>    
    type MsgAgent =
        | NewSample of float

    type SimpleViewer(meter:MeterType,dispatch:Dispatch<Msg>) =
        let samplingRate = meter.SamplingRate

        let sampleProcessor (inbox:Agent<MsgAgent>) =
            let rec aux (samples:float list) (sampleNumber:int) =
                async{
                    let! msg = inbox.Receive()
                    match msg with
                    | NewSample sample ->
                        if sampleNumber = samplingRate then
                            let level = meter.GetValue samples  // [0. .. 1.]
                            match level with
                            | RMSResult res
                            | PeakResult res
                            | PeakToPeakResult res -> 
                                let level = res * 10. |> int      // [0 .. 10]
                                printing level
                            | FFTResult res -> dispatch (DataDisplay res)
                            return! aux [] 0
                        else
                            return! aux (sample::samples) (sampleNumber + 1)                            
                }

            aux [] 0
        
        let agent = Agent<MsgAgent>.Start sampleProcessor

        member __.NewSample(sample32) = agent.Post (NewSample sample32)
    


module test =
    open NAudio.Wave
    open simpleViewer
    open Elmish.ProgramTypes
    open types

    let processSample (simpleViewer:SimpleViewer) (sample32:float) = 
        simpleViewer.NewSample sample32

    let toPcmSample (data1:byte) (data2:byte) =
        let data1 = data1 |> int16 
        let data2 = data2 |> int16 
        let sample = (data2 <<< 8 ||| data1) |> float
        ( sample / (pown 2. 15) )


    let processSignal (simpleViewer:SimpleViewer) (data:WaveInEventArgs) =
        for index in 0..2..(data.BytesRecorded-1)  do 
            let pcmSample = toPcmSample (data.Buffer.[index]) (data.Buffer.[index + 1])
            processSample simpleViewer pcmSample
    
    let buildMicrophone () =       
        let waveIn = new WaveInEvent()
        waveIn
        
    let addProcessing (waveIn:WaveInEvent) (dispatch : Dispatch<Msg>) =
        async{
            let simpleViewer = new SimpleViewer(FFTMeter,dispatch)
            let channelCount = 1 // mono

            waveIn.DeviceNumber <- 0 
            waveIn.DataAvailable.Add(processSignal simpleViewer)

            waveIn.WaveFormat <- new WaveFormat(sampleRate,channelCount)
        }
                


module MVU =
    open test
    open System.Windows
    open Elmish.DSL.DSL
    open Elmish.ProgramTypes
    open Elmish.VDom.VirtualProperty.FsWPFRepresentation
    open System.Windows.Controls
    open types
    open simpleViewer

    let init () = 
        let initModel = 
            { waveIn = buildMicrophone()
              Data = Map.empty<int * int, float> }
        initModel,AsyncDispatcher (fun dispatch -> addProcessing (initModel.waveIn) dispatch )

            

    let update msg (model:Model) =  
        match msg with 
        | Play -> 
            model.waveIn.StartRecording()
            Success model,NoEffect
        | Stop -> 
            (model.waveIn.StopRecording())
            Success model,NoEffect
        | DataDisplay data ->
            Success { model with Data = data } ,NoEffect


    let viewProgressBars (model:Model) =
        [ let mutable i = 0
          for KeyValue (_,v) in model.Data do
            i <- i + 1
            yield WPF.progressBar( style = new Style(Column = i-1), Value = v , Minimum = 0. , Maximum = 0.05 , Orientation=Orientation.Vertical)
        ]        

    let viewGrid children =
        WPF.grid( style = new Style( Row = 1 , ColumnSpan = 2) 
                  , ColumnDefinitions = [ for _ in 1..bands -> {Width =1;Unit=GridUnitType.Star} ]
                  , Children = children)

    let viewGridChildren (model:Model) = 
        [   yield WPF.button( style = new Style(Row = 0,Column = 0) ,
                              Content = ("Play!" |> box), 
                              Click = fun _ -> Play ) 
            yield WPF.button( style = new Style(Row = 0,Column = 1) ,
                              Content = ("Stop!" |> box), 
                              Click = fun _ -> Stop ) 
            yield viewGrid (viewProgressBars model)
        ]

    
    let view (model:Model) =
        WPF.window( WPF.grid( RowDefinitions = 
                                [  {Height=1;Unit=GridUnitType.Star}
                                   {Height=5;Unit=GridUnitType.Star}
                                ],  
                              ColumnDefinitions =
                                [  {Width =1;Unit=GridUnitType.Star}
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



     