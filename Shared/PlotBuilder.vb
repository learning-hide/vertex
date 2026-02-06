Imports System
Imports System.Collections.Generic
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Series

Namespace SmartDCP.[Shared]
	' Token: 0x0200001C RID: 28
	Public Class PlotBuilder
		' Token: 0x060000DF RID: 223 RVA: 0x000057EA File Offset: 0x000039EA
		Public Sub New(test As Test, type As PlotType)
			Me._test = test
			Me._type = type
		End Sub

		' Token: 0x060000E0 RID: 224 RVA: 0x00005800 File Offset: 0x00003A00
		Public Function BuildModel(showTitle As Boolean) As PlotModel
			Dim plotModel As PlotModel = New PlotModel()
			plotModel.IsLegendVisible = False
			If showTitle Then
				plotModel.Title = Me.PlotTitle
			End If
			If PlotType.DepthVsCbr = Me._type Then
				Dim logarithmicAxis As LogarithmicAxis = New LogarithmicAxis()
				logarithmicAxis.IsPanEnabled = False
				logarithmicAxis.IsZoomEnabled = False
				logarithmicAxis.Position = AxisPosition.Bottom
				logarithmicAxis.MajorGridlineStyle = LineStyle.Solid
				logarithmicAxis.MinorGridlineStyle = LineStyle.Dot
				logarithmicAxis.Title = Me.XAxisTitle
				logarithmicAxis.Unit = Me.XAxisUnits
				logarithmicAxis.Maximum = 100.0
				logarithmicAxis.Minimum = 1.0
				plotModel.Axes.Add(logarithmicAxis)
			Else
				Dim linearAxis As LinearAxis = New LinearAxis()
				linearAxis.IsPanEnabled = False
				linearAxis.IsZoomEnabled = False
				linearAxis.Position = AxisPosition.Bottom
				linearAxis.MajorGridlineStyle = LineStyle.Solid
				linearAxis.MinorGridlineStyle = LineStyle.Dot
				linearAxis.Title = Me.XAxisTitle
				linearAxis.Unit = Me.XAxisUnits
				plotModel.Axes.Add(linearAxis)
			End If
			Dim linearAxis2 As LinearAxis = New LinearAxis()
			linearAxis2.IsPanEnabled = False
			linearAxis2.IsZoomEnabled = False
			linearAxis2.Position = AxisPosition.Left
			linearAxis2.MajorGridlineStyle = LineStyle.Solid
			linearAxis2.MinorGridlineStyle = LineStyle.Dot
			linearAxis2.Title = Me.YAxisTitle
			linearAxis2.Unit = Me.YAxisUnits
			linearAxis2.StartPosition = 1.0
			linearAxis2.EndPosition = 0.0
			plotModel.Axes.Add(linearAxis2)
			Dim lineSeries As LineSeries = New LineSeries()
			lineSeries.Color = OxyColors.Blue
			lineSeries.LineStyle = LineStyle.Solid
			lineSeries.Title = "Values"
			For Each tuple As Tuple(Of Double, Double) In Me.PlotData
				lineSeries.Points.Add(New DataPoint(tuple.Item1, tuple.Item2))
			Next
			plotModel.Series.Add(lineSeries)
			Return plotModel
		End Function

		' Token: 0x17000026 RID: 38
		' (get) Token: 0x060000E1 RID: 225 RVA: 0x000059F0 File Offset: 0x00003BF0
		Private ReadOnly Property PlotTitle As String
			Get
				Select Case Me._type
					Case PlotType.DepthVsDelta
						Return "Delta Plot"
					Case PlotType.DepthVsBlow
						Return "Depth Plot"
					Case PlotType.DepthVsCbr
						Return "CBR Plot"
					Case Else
						Return String.Empty
				End Select
			End Get
		End Property

		' Token: 0x17000027 RID: 39
		' (get) Token: 0x060000E2 RID: 226 RVA: 0x00005A30 File Offset: 0x00003C30
		Private ReadOnly Property PlotData As List(Of Tuple(Of Double, Double))
			Get
				Dim list As List(Of Tuple(Of Double, Double)) = New List(Of Tuple(Of Double, Double))()
				Select Case Me._type
					Case PlotType.DepthVsDelta
						For i As Integer = 0 To Me._test.Samples.Count - 1
							list.Add(New Tuple(Of Double, Double)(Me._test.Samples(i).Delta / 10.0, Me._test.Samples(i).Depth / 10.0))
						Next
					Case PlotType.DepthVsBlow
						For j As Integer = 0 To Me._test.Samples.Count - 1
							list.Add(New Tuple(Of Double, Double)(CDbl((j + 1)), Me._test.Samples(j).Depth / 10.0))
						Next
					Case PlotType.DepthVsCbr
						For k As Integer = 0 To Me._test.Samples.Count - 1
							list.Add(New Tuple(Of Double, Double)(Me._test.Samples(k).CBR, Me._test.Samples(k).Depth / 10.0))
						Next
				End Select
				Return list
			End Get
		End Property

		' Token: 0x17000028 RID: 40
		' (get) Token: 0x060000E3 RID: 227 RVA: 0x00005B7C File Offset: 0x00003D7C
		Private ReadOnly Property XAxisTitle As String
			Get
				Select Case Me._type
					Case PlotType.DepthVsDelta
						Return "Detla"
					Case PlotType.DepthVsBlow
						Return "Blow"
					Case PlotType.DepthVsCbr
						Return "CBR"
					Case Else
						Return String.Empty
				End Select
			End Get
		End Property

		' Token: 0x17000029 RID: 41
		' (get) Token: 0x060000E4 RID: 228 RVA: 0x00005BBC File Offset: 0x00003DBC
		Private ReadOnly Property XAxisUnits As String
			Get
				Select Case Me._type
					Case PlotType.DepthVsDelta
						Return "cm"
					Case PlotType.DepthVsBlow
						Return "Count"
					Case PlotType.DepthVsCbr
						Return "%"
					Case Else
						Return String.Empty
				End Select
			End Get
		End Property

		' Token: 0x1700002A RID: 42
		' (get) Token: 0x060000E5 RID: 229 RVA: 0x00005BFC File Offset: 0x00003DFC
		Private ReadOnly Property YAxisTitle As String
			Get
				Select Case Me._type
					Case PlotType.DepthVsDelta
						Return "Depth"
					Case PlotType.DepthVsBlow
						Return "Depth"
					Case PlotType.DepthVsCbr
						Return "Depth"
					Case Else
						Return String.Empty
				End Select
			End Get
		End Property

		' Token: 0x1700002B RID: 43
		' (get) Token: 0x060000E6 RID: 230 RVA: 0x00005C3C File Offset: 0x00003E3C
		Private ReadOnly Property YAxisUnits As String
			Get
				Select Case Me._type
					Case PlotType.DepthVsDelta
						Return "cm"
					Case PlotType.DepthVsBlow
						Return "cm"
					Case PlotType.DepthVsCbr
						Return "cm"
					Case Else
						Return String.Empty
				End Select
			End Get
		End Property

		' Token: 0x0400005A RID: 90
		Private _test As Test

		' Token: 0x0400005B RID: 91
		Private _type As PlotType
	End Class
End Namespace
