Imports System
Imports Android.OS
Imports Android.Views
Imports Android.Widget
Imports AndroidX.Fragment.App
Imports OxyPlot.Xamarin.Android
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x0200000B RID: 11
	Friend Class TestPlotFragment
		Inherits Fragment

		' Token: 0x06000035 RID: 53 RVA: 0x00002C96 File Offset: 0x00000E96
		Public Sub New(test As Test)
			Me._test = test
		End Sub

		' Token: 0x06000036 RID: 54 RVA: 0x0000273F File Offset: 0x0000093F
		Public Overrides Sub OnCreate(savedState As Bundle)
			MyBase.OnCreate(savedState)
			MyBase.HasOptionsMenu = True
		End Sub

		' Token: 0x06000037 RID: 55 RVA: 0x00002CA5 File Offset: 0x00000EA5
		Public Overrides Function OnCreateView(inflater As LayoutInflater, container As ViewGroup, savedInstanceState As Bundle) As View
			Return inflater.Inflate(2131492909, container, False)
		End Function

		' Token: 0x06000038 RID: 56 RVA: 0x00002CB4 File Offset: 0x00000EB4
		Public Overrides Sub OnActivityCreated(savedInstanceState As Bundle)
			MyBase.OnActivityCreated(savedInstanceState)
			Dim tabHost As TabHost = MyBase.Activity.FindViewById(Of TabHost)(2131296416)
			tabHost.Setup()
			Dim tabSpec As TabHost.TabSpec = tabHost.NewTabSpec("tag1")
			tabSpec.SetContent(2131296412)
			tabSpec.SetIndicator("CBR Plot")
			tabHost.AddTab(tabSpec)
			tabSpec = tabHost.NewTabSpec("tag2")
			tabSpec.SetContent(2131296413)
			tabSpec.SetIndicator("Delta Plot")
			tabHost.AddTab(tabSpec)
			tabSpec = tabHost.NewTabSpec("tag3")
			tabSpec.SetContent(2131296414)
			tabSpec.SetIndicator("Depth Plot")
			tabHost.AddTab(tabSpec)
			Dim plotBuilder As PlotBuilder = New PlotBuilder(Me._test, PlotType.DepthVsCbr)
			MyBase.Activity.FindViewById(Of PlotView)(2131296371).Model = plotBuilder.BuildModel(False)
			Dim plotBuilder2 As PlotBuilder = New PlotBuilder(Me._test, PlotType.DepthVsDelta)
			MyBase.Activity.FindViewById(Of PlotView)(2131296372).Model = plotBuilder2.BuildModel(False)
			Dim plotBuilder3 As PlotBuilder = New PlotBuilder(Me._test, PlotType.DepthVsBlow)
			MyBase.Activity.FindViewById(Of PlotView)(2131296373).Model = plotBuilder3.BuildModel(False)
			Dim tabWidget As TabWidget = tabHost.TabWidget
			If tabWidget IsNot Nothing Then
				For i As Integer = 0 To tabWidget.ChildCount - 1
					Dim childAt As View = tabWidget.GetChildAt(i)
					If childAt IsNot Nothing Then
						childAt.SetBackgroundResource(2131230799)
					End If
				Next
			End If
		End Sub

		' Token: 0x06000039 RID: 57 RVA: 0x00002E16 File Offset: 0x00001016
		Public Overrides Sub OnCreateOptionsMenu(menu As IMenu, inflater As MenuInflater)
			MyBase.OnCreateOptionsMenu(menu, inflater)
			MyBase.Activity.MenuInflater.Inflate(2131558406, menu)
		End Sub

		' Token: 0x0600003A RID: 58 RVA: 0x00002E36 File Offset: 0x00001036
		Public Overrides Function OnOptionsItemSelected(item As IMenuItem) As Boolean
			If item.ItemId = 2131296324 Then
				If TypeOf MyBase.Activity Is MainActivity Then
					TryCast(MyBase.Activity, MainActivity).ShowTestList()
				End If
				Return True
			End If
			Return MyBase.OnOptionsItemSelected(item)
		End Function

		' Token: 0x04000012 RID: 18
		Private _test As Test
	End Class
End Namespace
