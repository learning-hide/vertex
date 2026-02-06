Imports System
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Android.Content
Imports Android.Media
Imports Android.OS
Imports Android.Views
Imports Android.Widget
Imports AndroidX.AppCompat.App
Imports AndroidX.Fragment.App
Imports SmartDCP.Android.Platform
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x0200000C RID: 12
	Friend Class TestFragment
		Inherits Fragment

		' Token: 0x0600003B RID: 59 RVA: 0x0000273F File Offset: 0x0000093F
		Public Overrides Sub OnCreate(savedState As Bundle)
			MyBase.OnCreate(savedState)
			MyBase.HasOptionsMenu = True
		End Sub

		' Token: 0x0600003C RID: 60 RVA: 0x00002E6B File Offset: 0x0000106B
		Public Overrides Function OnCreateView(inflater As LayoutInflater, container As ViewGroup, savedInstanceState As Bundle) As View
			Return inflater.Inflate(2131492907, container, False)
		End Function

		' Token: 0x0600003D RID: 61 RVA: 0x00002E7C File Offset: 0x0000107C
		Public Overrides Sub OnActivityCreated(savedInstanceState As Bundle)
			MyBase.OnActivityCreated(savedInstanceState)
			Me._settings = New Settings(MyBase.Activity)
			Me._rangefinder = New LeicaDistro(MyBase.Activity)
			Me._testController = New TestController(Me._rangefinder, Me._settings)
			AddHandler Me._testController.MeasurementBaselined, AddressOf Me.OnMeasurementBaselined
			AddHandler Me._testController.MeasurementChanged, AddressOf Me.OnMeasurementChanged
			AddHandler Me._testController.MeasurementTimeout, AddressOf Me.OnMeasurementTimeout
			AddHandler Me._testController.Sample, AddressOf Me.OnSample
			AddHandler Me._testController.Exception, AddressOf Me.OnException
			Me._vibrator = CType(MyBase.Activity.GetSystemService("vibrator"), Vibrator)
			Me._player = MediaPlayer.Create(MyBase.Activity, 2131623936)
			Me._player3x = MediaPlayer.Create(MyBase.Activity, 2131623937)
			Me._testNameEditText = MyBase.Activity.FindViewById(Of EditText)(2131296432)
			Me._testNamePromptTextView = MyBase.Activity.FindViewById(Of TextView)(2131296433)
			Me._weightSpinner = MyBase.Activity.FindViewById(Of Spinner)(2131296459)
			Dim arrayAdapter As ArrayAdapter = ArrayAdapter.CreateFromResource(MyBase.Activity, 2130903040, 2131492894)
			arrayAdapter.SetDropDownViewResource(17367049)
			Me._weightSpinner.Adapter = arrayAdapter
			If Me._settings.DefaultWeight = Weight.Small101Lbs Then
				Me._weightSpinner.SetSelection(0, True)
			Else
				Me._weightSpinner.SetSelection(1, True)
			End If
			Me._testNameEditText.Enabled = True
			Me._testNamePromptTextView.Enabled = True
			Me._weightSpinner.Enabled = True
			Me._sampleListView = MyBase.Activity.FindViewById(Of ListView)(2131296380)
			Me._adapter = New TestSampleAdapter(MyBase.Activity)
			Me._sampleListView.Adapter = Me._adapter
			Me.RegisterForContextMenu(Me._sampleListView)
		End Sub

		' Token: 0x0600003E RID: 62 RVA: 0x00003084 File Offset: 0x00001284
		Public Overrides Sub OnPrepareOptionsMenu(menu As IMenu)
			MyBase.OnPrepareOptionsMenu(menu)
			menu.Clear()
			If Me._state = TestState.[Default] Then
				MyBase.Activity.MenuInflater.Inflate(2131558403, menu)
				Return
			End If
			If TestState.Paused = Me._state Then
				MyBase.Activity.MenuInflater.Inflate(2131558404, menu)
				Return
			End If
			If TestState.Testing = Me._state Then
				MyBase.Activity.MenuInflater.Inflate(2131558405, menu)
			End If
		End Sub

		' Token: 0x0600003F RID: 63 RVA: 0x000030FC File Offset: 0x000012FC
		Public Overrides Function OnOptionsItemSelected(item As IMenuItem) As Boolean
			Dim itemId As Integer = item.ItemId
			If itemId <> 2131296324 Then
				Select Case itemId
					Case 2131296430
						Dim builder As AlertDialog.Builder = New AlertDialog.Builder(MyBase.Activity).SetTitle("Smart DCP").SetMessage("Are you sure you want to continue the test?")
						Dim text As String = "Cancel"
						Dim <>9__4_ As EventHandler(Of DialogClickEventArgs) = TestFragment.<>c.<>9__4_4
						Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__4_
						If <>9__4_ Is Nothing Then
							Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
							End Sub
							eventHandler = eventHandler2
							TestFragment.<>c.<>9__4_4 = eventHandler2
						End If
						builder.SetNegativeButton(text, eventHandler).SetPositiveButton("OK", Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
							Me.OnTestContinue(Me._testNameEditText.Text)
						End Sub).Create().Show()
						Return True
					Case 2131296431
						Me.OnTestForce()
						Return True
					Case 2131296435
						Dim builder2 As AlertDialog.Builder = New AlertDialog.Builder(MyBase.Activity).SetTitle("Smart DCP").SetMessage("Are you sure you want to pause the test?")
						Dim text2 As String = "Cancel"
						Dim <>9__4_2 As EventHandler(Of DialogClickEventArgs) = TestFragment.<>c.<>9__4_2
						Dim eventHandler3 As EventHandler(Of DialogClickEventArgs) = <>9__4_2
						If <>9__4_2 Is Nothing Then
							Dim eventHandler4 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
							End Sub
							eventHandler3 = eventHandler4
							TestFragment.<>c.<>9__4_2 = eventHandler4
						End If
						builder2.SetNegativeButton(text2, eventHandler3).SetPositiveButton("OK", Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
							Me.OnTestPause()
						End Sub).Create().Show()
						Return True
					Case 2131296436
						If Not PermissionsHelper.CheckAndRequestBluetoothPermissions(Me) Then
							Return True
						End If
						Dim text3 As String = Me._testNameEditText.Text
						Dim weight As Weight = If((Me._weightSpinner.SelectedItemPosition = 0), Weight.Small101Lbs, Weight.Large176Lbs)
						Me._settings.DefaultWeight = weight
						If Not String.IsNullOrEmpty(text3) Then
							Me.OnTestStart(text3, weight)
						Else
							Dim builder3 As AlertDialog.Builder = New AlertDialog.Builder(MyBase.Activity).SetTitle("Smart DCP").SetMessage("Please enter a name.")
							Dim text4 As String = "OK"
							Dim <>9__4_3 As EventHandler(Of DialogClickEventArgs) = TestFragment.<>c.<>9__4_6
							Dim eventHandler5 As EventHandler(Of DialogClickEventArgs) = <>9__4_3
							If <>9__4_3 Is Nothing Then
								Dim eventHandler6 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
								End Sub
								eventHandler5 = eventHandler6
								TestFragment.<>c.<>9__4_6 = eventHandler6
							End If
							builder3.SetPositiveButton(text4, eventHandler5).Create().Show()
						End If
						Return True
					Case 2131296437
						Dim builder4 As AlertDialog.Builder = New AlertDialog.Builder(MyBase.Activity).SetTitle("Smart DCP").SetMessage("Are you sure you want to stop the test?")
						Dim text5 As String = "Cancel"
						Dim <>9__4_4 As EventHandler(Of DialogClickEventArgs) = TestFragment.<>c.<>9__4_0
						Dim eventHandler7 As EventHandler(Of DialogClickEventArgs) = <>9__4_4
						If <>9__4_4 Is Nothing Then
							Dim eventHandler8 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
							End Sub
							eventHandler7 = eventHandler8
							TestFragment.<>c.<>9__4_0 = eventHandler8
						End If
						builder4.SetNegativeButton(text5, eventHandler7).SetPositiveButton("OK", Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
							Me.OnTestStop()
						End Sub).Create().Show()
						Return True
				End Select
				Return MyBase.OnOptionsItemSelected(item)
			End If
			Me.OnTestCancel()
			Return True
		End Function

		' Token: 0x06000040 RID: 64 RVA: 0x00003338 File Offset: 0x00001538
		Public Overrides Sub OnCreateContextMenu(menu As IContextMenu, v As View, menuInfo As IContextMenuContextMenuInfo)
			MyBase.OnCreateContextMenu(menu, v, menuInfo)
			Dim adapterContextMenuInfo As AdapterView.AdapterContextMenuInfo = TryCast(menuInfo, AdapterView.AdapterContextMenuInfo)
			If adapterContextMenuInfo IsNot Nothing AndAlso Me._adapter.Count > 0 AndAlso adapterContextMenuInfo.Position = Me._adapter.Count - 1 Then
				Dim sample As Sample = Me._adapter(adapterContextMenuInfo.Position)
				If sample IsNot Nothing Then
					menu.SetHeaderTitle(sample.Name)
				End If
				menu.Add(0, 0, 0, "Delete")
			End If
		End Sub

		' Token: 0x06000041 RID: 65 RVA: 0x000033AC File Offset: 0x000015AC
		Public Overrides Function OnContextItemSelected(item As IMenuItem) As Boolean
			Dim adapterContextMenuInfo As AdapterView.AdapterContextMenuInfo = TryCast(item.MenuInfo, AdapterView.AdapterContextMenuInfo)
			If adapterContextMenuInfo IsNot Nothing AndAlso item.ItemId = 0 Then
				Dim sample As Sample = Me._adapter(adapterContextMenuInfo.Position)
				Me._adapter.Remove(adapterContextMenuInfo.Position)
				Me._testController.RemoveLastSample(sample)
				Return True
			End If
			Return MyBase.OnContextItemSelected(item)
		End Function

		' Token: 0x06000042 RID: 66 RVA: 0x00003408 File Offset: 0x00001608
		Private Sub OnTestStart(name As String, weight As Weight)
			Me._testNameEditText.Enabled = False
			Me._testNamePromptTextView.Enabled = False
			Me._weightSpinner.Enabled = False
			Me._state = TestState.Testing
			MyBase.Activity.InvalidateOptionsMenu()
			If TypeOf MyBase.Activity Is MainActivity Then
				TryCast(MyBase.Activity, MainActivity).Subtitle = name
			End If
			Me._testController.Start(name, weight)
			MyBase.Activity.RunOnUiThread(Sub()
				If Me._alertDialog IsNot Nothing Then
					Me._alertDialog.Dismiss()
					Me._alertDialog = Nothing
				End If
				Dim builder As AlertDialog.Builder = New AlertDialog.Builder(MyBase.Activity).SetTitle("Smart DCP").SetMessage("Acquiring distance from range finder...")
				Dim text As String = "OK"
				Dim <>9__7_ As EventHandler(Of DialogClickEventArgs) = TestFragment.<>c.<>9__7_1
				Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__7_
				If <>9__7_ Is Nothing Then
					Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> ev As DialogClickEventArgs)
					End Sub
					eventHandler = eventHandler2
					TestFragment.<>c.<>9__7_1 = eventHandler2
				End If
				Me._alertDialog = builder.SetPositiveButton(text, eventHandler).Create()
				Me._alertDialog.Show()
			End Sub)
		End Sub

		' Token: 0x06000043 RID: 67 RVA: 0x00003490 File Offset: 0x00001690
		Private Sub OnTestStop()
			Me._state = TestState.[Default]
			Try
				MyBase.Activity.InvalidateOptionsMenu()
			Catch ex As Exception
				Me.OnException(Me, ex)
			End Try
			Me._testController.[Stop]()
			Me._rangefinder = Nothing
			Try
				Test.Write(Me._adapter.Samples)
			Catch ex2 As Exception
				Me.OnException(Me, ex2)
			End Try
			Dim mainActivity As MainActivity = TryCast(MyBase.Activity, MainActivity)
			If mainActivity Is Nothing Then
				Return
			End If
			mainActivity.ShowTestList()
		End Sub

		' Token: 0x06000044 RID: 68 RVA: 0x0000351C File Offset: 0x0000171C
		Private Sub OnTestPause()
			Me._state = TestState.Paused
			MyBase.Activity.InvalidateOptionsMenu()
			If TypeOf MyBase.Activity Is MainActivity Then
				TryCast(MyBase.Activity, MainActivity).Subtitle = "Paused"
			End If
			Me._testController.Pause()
		End Sub

		' Token: 0x06000045 RID: 69 RVA: 0x00003568 File Offset: 0x00001768
		Private Sub OnTestContinue(name As String)
			Me._state = TestState.Testing
			MyBase.Activity.InvalidateOptionsMenu()
			If TypeOf MyBase.Activity Is MainActivity Then
				TryCast(MyBase.Activity, MainActivity).Subtitle = name
			End If
			Me._testController.[Continue]()
		End Sub

		' Token: 0x06000046 RID: 70 RVA: 0x000035A5 File Offset: 0x000017A5
		Private Sub OnTestForce()
			Me._testController.Force()
		End Sub

		' Token: 0x06000047 RID: 71 RVA: 0x000035B2 File Offset: 0x000017B2
		Private Sub OnTestCancel()
			Dim mainActivity As MainActivity = TryCast(MyBase.Activity, MainActivity)
			If mainActivity Is Nothing Then
				Return
			End If
			mainActivity.ShowTestList()
		End Sub

		' Token: 0x06000048 RID: 72 RVA: 0x000035C9 File Offset: 0x000017C9
		Private Sub OnMeasurementTimeout(sender As Object, e As EventArgs)
			If MyBase.Activity IsNot Nothing AndAlso Not MyBase.Activity.IsDestroyed Then
				MyBase.Activity.RunOnUiThread(Sub()
					If Me._alertDialog IsNot Nothing Then
						Me._alertDialog.Dismiss()
						Me._alertDialog = Nothing
					End If
					Dim builder As AlertDialog.Builder = New AlertDialog.Builder(MyBase.Activity).SetTitle("Smart DCP").SetMessage("Unable to measure distance. Please check the range finder.")
					Dim text As String = "OK"
					Dim <>9__13_ As EventHandler(Of DialogClickEventArgs) = TestFragment.<>c.<>9__13_1
					Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__13_
					If <>9__13_ Is Nothing Then
						Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> ev As DialogClickEventArgs)
						End Sub
						eventHandler = eventHandler2
						TestFragment.<>c.<>9__13_1 = eventHandler2
					End If
					Me._alertDialog = builder.SetPositiveButton(text, eventHandler).Create()
					Me._alertDialog.Show()
				End Sub)
			End If
		End Sub

		' Token: 0x06000049 RID: 73 RVA: 0x000035F7 File Offset: 0x000017F7
		Private Sub OnMeasurementChanged(sender As Object, e As EventArgs)
			If MyBase.Activity IsNot Nothing AndAlso Not MyBase.Activity.IsDestroyed Then
				MyBase.Activity.RunOnUiThread(Sub()
					Try
						If Me._alertDialog IsNot Nothing Then
							Me._alertDialog.Dismiss()
							Me._alertDialog = Nothing
						End If
					Catch ex As Exception
					End Try
				End Sub)
			End If
		End Sub

		' Token: 0x0600004A RID: 74 RVA: 0x00003625 File Offset: 0x00001825
		Private Sub OnMeasurementBaselined(sender As Object, e As EventArgs)
			If MyBase.Activity IsNot Nothing AndAlso Not MyBase.Activity.IsDestroyed Then
				MyBase.Activity.RunOnUiThread(Sub()
					Try
						If Me._player3x IsNot Nothing Then
							Me._player3x.Start()
						End If
					Catch ex As Exception
					End Try
					For i As Integer = 0 To 3 - 1
						Try
							Dim vibrator As Vibrator = Me._vibrator
							If vibrator IsNot Nothing Then
								vibrator.Vibrate(100L)
							End If
						Catch ex2 As Exception
						End Try
						Thread.Sleep(500)
					Next
				End Sub)
			End If
		End Sub

		' Token: 0x0600004B RID: 75 RVA: 0x00003654 File Offset: 0x00001854
		Private Sub UserNotification()
			Try
				Dim player As MediaPlayer = Me._player
				If player IsNot Nothing Then
					player.Start()
				End If
			Catch ex As Exception
			End Try
			Try
				Dim vibrator As Vibrator = Me._vibrator
				If vibrator IsNot Nothing Then
					vibrator.Vibrate(100L)
				End If
			Catch ex2 As Exception
			End Try
		End Sub

		' Token: 0x0600004C RID: 76 RVA: 0x000036B0 File Offset: 0x000018B0
		Private Sub OnSample(sender As Object, sample As Sample)
			If MyBase.Activity IsNot Nothing AndAlso Not MyBase.Activity.IsDestroyed Then
				MyBase.Activity.RunOnUiThread(Sub()
					Dim num As Integer = Me._adapter.Add(sample)
					Me._sampleListView.SmoothScrollToPositionFromTop(num, 0)
				End Sub)
			End If
			Me.UserNotification()
		End Sub

		' Token: 0x0600004D RID: 77 RVA: 0x000029C6 File Offset: 0x00000BC6
		Private Sub OnException(sender As Object, ex As Exception)
			Dim mainActivity As MainActivity = TryCast(MyBase.Activity, MainActivity)
			If mainActivity Is Nothing Then
				Return
			End If
			mainActivity.OnException(Me, ex)
		End Sub

		' Token: 0x04000013 RID: 19
		Private _settings As Settings

		' Token: 0x04000014 RID: 20
		Private _rangefinder As LeicaDistro

		' Token: 0x04000015 RID: 21
		Private _testController As TestController

		' Token: 0x04000016 RID: 22
		Private _vibrator As Vibrator

		' Token: 0x04000017 RID: 23
		Private _player As MediaPlayer

		' Token: 0x04000018 RID: 24
		Private _player3x As MediaPlayer

		' Token: 0x04000019 RID: 25
		Private _sampleListView As ListView

		' Token: 0x0400001A RID: 26
		Private _adapter As TestSampleAdapter

		' Token: 0x0400001B RID: 27
		Private _alertDialog As AlertDialog

		' Token: 0x0400001C RID: 28
		Private _state As TestState

		' Token: 0x0400001D RID: 29
		Private _testNameEditText As EditText

		' Token: 0x0400001E RID: 30
		Private _testNamePromptTextView As TextView

		' Token: 0x0400001F RID: 31
		Private _weightSpinner As Spinner
	End Class
End Namespace
