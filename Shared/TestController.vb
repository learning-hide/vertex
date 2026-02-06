Imports System
Imports System.IO
Imports System.Threading

Namespace SmartDCP.[Shared]
	' Token: 0x02000020 RID: 32
	Public Class TestController
		' Token: 0x1400000D RID: 13
		' (add) Token: 0x060000F0 RID: 240 RVA: 0x00005FFC File Offset: 0x000041FC
		' (remove) Token: 0x060000F1 RID: 241 RVA: 0x00006034 File Offset: 0x00004234
		Public Event Sample As EventHandler(Of Sample)

		' Token: 0x1400000E RID: 14
		' (add) Token: 0x060000F2 RID: 242 RVA: 0x0000606C File Offset: 0x0000426C
		' (remove) Token: 0x060000F3 RID: 243 RVA: 0x000060A4 File Offset: 0x000042A4
		Public Event MeasurementBaselined As EventHandler

		' Token: 0x1400000F RID: 15
		' (add) Token: 0x060000F4 RID: 244 RVA: 0x000060DC File Offset: 0x000042DC
		' (remove) Token: 0x060000F5 RID: 245 RVA: 0x00006114 File Offset: 0x00004314
		Public Event MeasurementChanged As EventHandler

		' Token: 0x14000010 RID: 16
		' (add) Token: 0x060000F6 RID: 246 RVA: 0x0000614C File Offset: 0x0000434C
		' (remove) Token: 0x060000F7 RID: 247 RVA: 0x00006184 File Offset: 0x00004384
		Public Event MeasurementTimeout As EventHandler

		' Token: 0x14000011 RID: 17
		' (add) Token: 0x060000F8 RID: 248 RVA: 0x000061BC File Offset: 0x000043BC
		' (remove) Token: 0x060000F9 RID: 249 RVA: 0x000061F4 File Offset: 0x000043F4
		Public Event Exception As EventHandler(Of Exception)

		' Token: 0x060000FA RID: 250 RVA: 0x0000622C File Offset: 0x0000442C
		Public Sub New(rangefinder As IRangefinder, settings As ISettings)
			Me._rangefinder = rangefinder
			Me._settings = settings
			AddHandler Me._rangefinder.MeasurementChanged, Sub(sender As Object, args As EventArgs)
				Try
					Me.OnMeasurementChanged(CDbl(Me._rangefinder.Measurement))
				Catch ex As Exception
					Me.OnException(ex)
				End Try
			End Sub
		End Sub

		' Token: 0x1700002F RID: 47
		' (get) Token: 0x060000FB RID: 251 RVA: 0x00006290 File Offset: 0x00004490
		Public ReadOnly Property IsBluetoothSupported As Boolean
			Get
				Try
					Return Me._rangefinder.IsSupported
				Catch ex As Exception
				End Try
				Return True
			End Get
		End Property

		' Token: 0x060000FC RID: 252 RVA: 0x000062C4 File Offset: 0x000044C4
		Public Sub Start(name As String, weight As Weight)
			Dim now As DateTime = DateTime.Now
			Me._isPreviousDistanceValid = False
			Me._previousDistance = 0.0
			Me._name = name
			Me._path = Path.Combine(Test.Directory, String.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.csv", New Object() { now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second }))
			Me._weight = weight
			Me._startTicks = DateTime.Now.Ticks
			Me._depth = 0.0
			Me._isMeasurementPending = False
			Me._isForcePending = False
			Me._isTesting = True
			Me._lastMeasurement = DateTime.Now
			Me._lastMeasurementErrorDisplay = DateTime.Now
			Me._measurementState = TestController.MeasurementState.Waiting
			Me._rangefinder.Open(Me._settings.RangefinderName)
			Try
				If Not Me._rangefinder.IsConnected Then
					Me._rangefinder.Scan()
				End If
			Catch ex As Exception
				Me.OnException(ex)
			End Try
			Try
				Me._rangefinder.Measure()
			Catch ex2 As Exception
				Me.OnException(ex2)
			End Try
			If Me._measurementThread Is Nothing Then
				Me._measurementThread = AddressOf Me.MeasurementThread
			End If
			If Not Me._measurementThread.IsAlive Then
				Me._measurementThread.Start()
			End If
		End Sub

		' Token: 0x060000FD RID: 253 RVA: 0x00006470 File Offset: 0x00004670
		Public Sub [Stop]()
			Me._isTesting = False
			Me._rangefinder.Close()
			If Me._measurementThread IsNot Nothing Then
				If Me._measurementThread.IsAlive Then
					Me._measurementThread.Join(1000)
				End If
				Me._measurementThread = Nothing
			End If
		End Sub

		' Token: 0x060000FE RID: 254 RVA: 0x000064BC File Offset: 0x000046BC
		Public Sub Force()
			Me._isForcePending = True
		End Sub

		' Token: 0x060000FF RID: 255 RVA: 0x000064C5 File Offset: 0x000046C5
		Public Sub Pause()
			Me._isPaused = True
		End Sub

		' Token: 0x06000100 RID: 256 RVA: 0x000064CE File Offset: 0x000046CE
		Public Sub [Continue]()
			Me.Baseline()
			Me._isPaused = False
		End Sub

		' Token: 0x06000101 RID: 257 RVA: 0x000064DD File Offset: 0x000046DD
		Public Sub RemoveLastSample(sample As Sample)
			Me._depth -= sample.Delta
			Me.Baseline()
		End Sub

		' Token: 0x06000102 RID: 258 RVA: 0x000064F8 File Offset: 0x000046F8
		Private Sub Baseline()
			Me._isPreviousDistanceValid = False
			Me._previousDistance = 0.0
			Me._isMeasurementPending = False
			Me._isForcePending = False
			Me._lastMeasurement = DateTime.Now
			Me._lastMeasurementErrorDisplay = DateTime.Now
			Me._measurementState = TestController.MeasurementState.Waiting
		End Sub

		' Token: 0x06000103 RID: 259 RVA: 0x00006548 File Offset: 0x00004748
		Private Sub OnMeasurementChanged(measurement As Double)
			Me.OnMeasurementChanged()
			Me._isMeasurementPending = False
			If measurement <= 2.0 Then
				Me._lastMeasurement = DateTime.Now
				Me._currentDistance = measurement * 1000.0
				If Me._isTesting AndAlso Not Me._isPaused Then
					If Me._isPreviousDistanceValid Then
						Try
							If Interlocked.CompareExchange(TestController._lockFlag, 1, 0) = 0 AndAlso (Me._isForcePending OrElse Me._previousDistance - Me._currentDistance > Me._settings.SampleThreshold) AndAlso Me._measurementState = TestController.MeasurementState.Waiting Then
								Me._measurementSettleStartTick = DateTime.Now.Ticks
								Me._measurementState = TestController.MeasurementState.Settling
							End If
							Return
						Finally
							Interlocked.Decrement(TestController._lockFlag)
						End Try
					End If
					Me._previousDistance = Me._currentDistance
					Me._isPreviousDistanceValid = True
					Me.OnMeasurementBaselined()
				End If
			End If
		End Sub

		' Token: 0x06000104 RID: 260 RVA: 0x00006634 File Offset: 0x00004834
		Private Sub MeasurementThread()
			While Me._isTesting
				If Not Me._isPaused Then
					Try
						If TimeSpan.FromTicks(DateTime.Now.Ticks - Me._lastMeasurement.Ticks).TotalSeconds > 5.0 Then
							If TimeSpan.FromTicks(DateTime.Now.Ticks - Me._lastMeasurementErrorDisplay.Ticks).TotalSeconds > 15.0 Then
								Me.OnMeasurementTimeout()
								Me._lastMeasurementErrorDisplay = DateTime.Now
							End If
							Me._isMeasurementPending = False
							Try
								Me._rangefinder.Scan()
							Catch ex As Exception
								Me.OnException(ex)
							End Try
						End If
						Dim num As Double = Me._settings.SampleRate
						If Math.Abs(num) < 5E-324 Then
							num = Double.Epsilon
						End If
						If TimeSpan.FromTicks(DateTime.Now.Ticks - Me._lastMeasurement.Ticks).TotalMilliseconds > 1000.0 / num AndAlso Not Me._isMeasurementPending Then
							Try
								Me._rangefinder.Measure()
							Catch ex2 As Exception
								Me.OnException(ex2)
							End Try
							Me._isMeasurementPending = True
						End If
						If Me._measurementState = TestController.MeasurementState.Settling AndAlso TimeSpan.FromTicks(DateTime.Now.Ticks - Me._measurementSettleStartTick).TotalMilliseconds > Me._settings.SampleSettle Then
							If Me._isForcePending OrElse Me._previousDistance - Me._currentDistance > Me._settings.SampleThreshold Then
								Me._isForcePending = False
								Dim totalMilliseconds As Double = TimeSpan.FromTicks(DateTime.Now.Ticks - Me._startTicks).TotalMilliseconds
								Dim num2 As Double = Math.Max(Me._previousDistance - Me._currentDistance, 0.0)
								If Math.Abs(num2) < 5E-324 Then
									num2 = Double.Epsilon
								End If
								Me._depth += num2
								Dim num3 As Double
								If Weight.Small101Lbs = Me._weight Then
									num3 = 292.0 / Math.Pow(2.0 * num2, 1.12)
								Else
									num3 = 292.0 / Math.Pow(num2, 1.12)
								End If
								num3 = Math.Min(Math.Max(num3, 0.0), 100.0)
								Me.OnSample(New Sample() With { .Path = Me._path, .Time = totalMilliseconds, .Name = Me._name, .Depth = Me._depth, .Delta = num2, .CBR = num3 })
								Me._previousDistance = Me._currentDistance
							End If
							Me._measurementState = TestController.MeasurementState.Waiting
						End If
					Catch ex3 As Exception
						Me.OnException(ex3)
					End Try
				End If
				Thread.Sleep(100)
			End While
		End Sub

		' Token: 0x06000105 RID: 261 RVA: 0x00006978 File Offset: 0x00004B78
		Private Sub OnMeasurementBaselined()
			If Me.MeasurementBaselined IsNot Nothing Then
				Me.MeasurementBaselined(Me, EventArgs.Empty)
			End If
		End Sub

		' Token: 0x06000106 RID: 262 RVA: 0x00006993 File Offset: 0x00004B93
		Private Sub OnMeasurementChanged()
			If Me.MeasurementChanged IsNot Nothing Then
				Me.MeasurementChanged(Me, EventArgs.Empty)
			End If
		End Sub

		' Token: 0x06000107 RID: 263 RVA: 0x000069AE File Offset: 0x00004BAE
		Private Sub OnMeasurementTimeout()
			If Me.MeasurementTimeout IsNot Nothing Then
				Me.MeasurementTimeout(Me, EventArgs.Empty)
			End If
		End Sub

		' Token: 0x06000108 RID: 264 RVA: 0x000069C9 File Offset: 0x00004BC9
		Private Sub OnSample(e As Sample)
			If Me.Sample IsNot Nothing Then
				Me.Sample(Me, e)
			End If
		End Sub

		' Token: 0x06000109 RID: 265 RVA: 0x000069E0 File Offset: 0x00004BE0
		Private Sub OnException(e As Exception)
			If Me.Exception IsNot Nothing Then
				Me.Exception(Me, e)
			End If
		End Sub

		' Token: 0x0400006F RID: 111
		Private _rangefinder As IRangefinder

		' Token: 0x04000070 RID: 112
		Private _settings As ISettings

		' Token: 0x04000071 RID: 113
		Private _measurementThread As Thread

		' Token: 0x04000072 RID: 114
		Private _lastMeasurement As DateTime = DateTime.MinValue

		' Token: 0x04000073 RID: 115
		Private _lastMeasurementErrorDisplay As DateTime = DateTime.MinValue

		' Token: 0x04000074 RID: 116
		Private _startTicks As Long

		' Token: 0x04000075 RID: 117
		Private _isPreviousDistanceValid As Boolean

		' Token: 0x04000076 RID: 118
		Private _previousDistance As Double

		' Token: 0x04000077 RID: 119
		Private _currentDistance As Double

		' Token: 0x04000078 RID: 120
		Private _depth As Double

		' Token: 0x04000079 RID: 121
		Private _name As String = String.Empty

		' Token: 0x0400007A RID: 122
		Private _path As String = String.Empty

		' Token: 0x0400007B RID: 123
		Private _weight As Weight

		' Token: 0x0400007C RID: 124
		Private _isTesting As Boolean

		' Token: 0x0400007D RID: 125
		Private _isPaused As Boolean

		' Token: 0x0400007E RID: 126
		Private _isForcePending As Boolean

		' Token: 0x0400007F RID: 127
		Private _isMeasurementPending As Boolean

		' Token: 0x04000080 RID: 128
		Private Shared _lockFlag As Integer

		' Token: 0x04000081 RID: 129
		Private _measurementState As TestController.MeasurementState

		' Token: 0x04000082 RID: 130
		Private _measurementSettleStartTick As Long

		' Token: 0x02000041 RID: 65
		Private Enum MeasurementState
			' Token: 0x040007D4 RID: 2004
			Waiting
			' Token: 0x040007D5 RID: 2005
			Settling
		End Enum
	End Class
End Namespace
