Imports System
Imports System.Runtime.CompilerServices
Imports Android.App
Imports Android.Bluetooth
Imports Android.Content
Imports Android.Content.PM
Imports Android.OS
Imports Android.Views
Imports Android.Widget
Imports AndroidX.AppCompat.App
Imports AndroidX.AppCompat.Widget
Imports AndroidX.Fragment.App
Imports SmartDCP.Android.UI
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android
	' Token: 0x02000004 RID: 4
	<Activity(Label := "Smart DCP", Icon := "@drawable/icon", Theme := "@style/MyTheme")>
	Public Class MainActivity
		Inherits AppCompatActivity

		' Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		Protected Overrides Sub OnCreate(bundle As Bundle)
			MyBase.OnCreate(bundle)
			Me.RequestedOrientation = ScreenOrientation.Portrait
			Me.SetContentView(2131492896)
			Me.SetSupportActionBar(MyBase.FindViewById(Of Toolbar)(2131296447))
			Me.SupportActionBar.Title = "Smart DCP"
			Me.ShowTestList()
		End Sub

		' Token: 0x17000001 RID: 1
		' (get) Token: 0x06000005 RID: 5 RVA: 0x000020CD File Offset: 0x000002CD
		' (set) Token: 0x06000006 RID: 6 RVA: 0x000020DA File Offset: 0x000002DA
		Public Property Subtitle As String
			Get
				Return Me.SupportActionBar.Subtitle
			End Get
			Set(value As String)
				Me.SupportActionBar.Subtitle = value
			End Set
		End Property

		' Token: 0x06000007 RID: 7 RVA: 0x000020E8 File Offset: 0x000002E8
		Public Sub ShowTest()
			Dim bluetooth As BluetoothAdapter = BluetoothAdapter.DefaultAdapter
			If bluetooth Is Nothing Then
				If Me._alertDialog IsNot Nothing Then
					Me._alertDialog.Dismiss()
					Me._alertDialog = Nothing
				End If
				Dim builder As AndroidX.AppCompat.App.AlertDialog.Builder = New AndroidX.AppCompat.App.AlertDialog.Builder(Me).SetTitle("Smart DCP").SetMessage("This device does not support Bluetooth.")
				Dim text As String = "OK"
				Dim <>9__4_ As EventHandler(Of DialogClickEventArgs) = MainActivity.<>c.<>9__4_2
				Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__4_
				If <>9__4_ Is Nothing Then
					Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
					End Sub
					eventHandler = eventHandler2
					MainActivity.<>c.<>9__4_2 = eventHandler2
				End If
				Me._alertDialog = builder.SetPositiveButton(text, eventHandler).Create()
				Me._alertDialog.Show()
				Return
			End If
			If bluetooth.IsEnabled Then
				Me.InternalShowTest()
				Return
			End If
			If Me._alertDialog IsNot Nothing Then
				Me._alertDialog.Dismiss()
				Me._alertDialog = Nothing
			End If
			Dim builder2 As AndroidX.AppCompat.App.AlertDialog.Builder = New AndroidX.AppCompat.App.AlertDialog.Builder(Me).SetTitle("Smart DCP").SetMessage("Would you like to enable Bluetooth?").SetPositiveButton("Yes", Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
				Try
					bluetooth.Enable()
				Catch ex As Exception
					Me.OnException(Me, ex)
				End Try
				Me.InternalShowTest()
			End Sub)
			Dim text2 As String = "No"
			Dim <>9__4_2 As EventHandler(Of DialogClickEventArgs) = MainActivity.<>c.<>9__4_1
			Dim eventHandler3 As EventHandler(Of DialogClickEventArgs) = <>9__4_2
			If <>9__4_2 Is Nothing Then
				Dim eventHandler4 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
				End Sub
				eventHandler3 = eventHandler4
				MainActivity.<>c.<>9__4_1 = eventHandler4
			End If
			Me._alertDialog = builder2.SetNegativeButton(text2, eventHandler3).Create()
			Me._alertDialog.Show()
		End Sub

		' Token: 0x06000008 RID: 8 RVA: 0x0000222C File Offset: 0x0000042C
		Private Sub InternalShowTest()
			MyBase.FindViewById(Of SearchView)(2131296386).Visibility = ViewStates.Gone
			Me.Subtitle = "New Test"
			Dim testFragment As TestFragment = New TestFragment()
			testFragment.RetainInstance = True
			Dim fragmentTransaction As FragmentTransaction = Me.SupportFragmentManager.BeginTransaction()
			Dim fragment As Fragment = Me.SupportFragmentManager.FindFragmentById(2131296344)
			If fragment IsNot Nothing Then
				fragmentTransaction.Remove(fragment)
			End If
			fragmentTransaction.Add(2131296344, testFragment)
			fragmentTransaction.Commit()
		End Sub

		' Token: 0x06000009 RID: 9 RVA: 0x000022A0 File Offset: 0x000004A0
		Public Sub ShowTestList()
			Dim searchView As SearchView = MyBase.FindViewById(Of SearchView)(2131296386)
			searchView.Visibility = ViewStates.Visible
			searchView.SetQuery(String.Empty, True)
			Me.SupportActionBar.Subtitle = String.Empty
			If Me._testListFragment Is Nothing Then
				Me._testListFragment = New TestListFragment()
				Dim testListFragment As TestListFragment = Me._testListFragment
				testListFragment.StateChanged = CType(Global.System.[Delegate].Combine(testListFragment.StateChanged, AddressOf Me.OnTestListStateChanged), EventHandler)
				Me._testListFragment.RetainInstance = True
			End If
			Dim fragmentTransaction As FragmentTransaction = Me.SupportFragmentManager.BeginTransaction()
			Dim fragment As Fragment = Me.SupportFragmentManager.FindFragmentById(2131296344)
			If fragment IsNot Nothing Then
				fragmentTransaction.Remove(fragment)
			End If
			fragmentTransaction.Add(2131296344, Me._testListFragment)
			fragmentTransaction.Commit()
		End Sub

		' Token: 0x0600000A RID: 10 RVA: 0x00002364 File Offset: 0x00000564
		Private Sub OnTestListStateChanged(sender As Object, eventArgs As EventArgs)
			Dim state As TestListState = Me._testListFragment.State
			If state = TestListState.[Default] Then
				Me.Subtitle = String.Empty
				Return
			End If
			If state <> TestListState.Selecting Then
				Me.Subtitle = String.Empty
				Return
			End If
			Me.Subtitle = "Select Test(s)"
		End Sub

		' Token: 0x0600000B RID: 11 RVA: 0x000023AC File Offset: 0x000005AC
		Public Sub ShowPlot(test As Test)
			MyBase.FindViewById(Of SearchView)(2131296386).Visibility = ViewStates.Gone
			Me.Subtitle = test.Name
			Dim testPlotFragment As TestPlotFragment = New TestPlotFragment(test) With { .RetainInstance = True }
			Dim fragmentTransaction As FragmentTransaction = Me.SupportFragmentManager.BeginTransaction()
			Dim fragment As Fragment = Me.SupportFragmentManager.FindFragmentById(2131296344)
			If fragment IsNot Nothing Then
				fragmentTransaction.Remove(fragment)
			End If
			fragmentTransaction.Add(2131296344, testPlotFragment)
			fragmentTransaction.Commit()
		End Sub

		' Token: 0x0600000C RID: 12 RVA: 0x00002420 File Offset: 0x00000620
		Public Sub ShowDiscover()
			MyBase.FindViewById(Of SearchView)(2131296386).Visibility = ViewStates.Gone
			Me.Subtitle = "Select Device"
			Dim discoverFragment As DiscoverFragment = New DiscoverFragment() With { .RetainInstance = True }
			Dim fragmentTransaction As FragmentTransaction = Me.SupportFragmentManager.BeginTransaction()
			Dim fragment As Fragment = Me.SupportFragmentManager.FindFragmentById(2131296344)
			If fragment IsNot Nothing Then
				fragmentTransaction.Remove(fragment)
			End If
			fragmentTransaction.Add(2131296344, discoverFragment)
			fragmentTransaction.Commit()
		End Sub

		' Token: 0x0600000D RID: 13 RVA: 0x00002492 File Offset: 0x00000692
		Public Sub OnException(sender As Object, exception As Exception)
			Me.OnShowDialog(exception.Message)
		End Sub

		' Token: 0x0600000E RID: 14 RVA: 0x000024A0 File Offset: 0x000006A0
		Public Sub OnShowDialog(message As String)
			MyBase.RunOnUiThread(Sub()
				If Me._alertDialog IsNot Nothing Then
					Me._alertDialog.Dismiss()
					Me._alertDialog = Nothing
				End If
				Dim <>4__this As MainActivity = Me
				Dim builder As AndroidX.AppCompat.App.AlertDialog.Builder = New AndroidX.AppCompat.App.AlertDialog.Builder(Me).SetTitle("Smart DCP").SetMessage(message)
				Dim text As String = "OK"
				Dim <>9__11_ As EventHandler(Of DialogClickEventArgs) = MainActivity.<>c.<>9__11_1
				Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__11_
				If <>9__11_ Is Nothing Then
					Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
					End Sub
					eventHandler = eventHandler2
					MainActivity.<>c.<>9__11_1 = eventHandler2
				End If
				<>4__this._alertDialog = builder.SetPositiveButton(text, eventHandler).Create()
				Me._alertDialog.Show()
			End Sub)
		End Sub

		' Token: 0x0600000F RID: 15 RVA: 0x000024D4 File Offset: 0x000006D4
		Public Overrides Sub OnBackPressed()
			If Me._alertDialog IsNot Nothing Then
				Me._alertDialog.Dismiss()
				Me._alertDialog = Nothing
			End If
			Dim builder As AndroidX.AppCompat.App.AlertDialog.Builder = New AndroidX.AppCompat.App.AlertDialog.Builder(Me).SetTitle("Smart DCP").SetMessage("Are you sure you want to exit?")
			Dim text As String = "Yes"
			Dim <>9__12_ As EventHandler(Of DialogClickEventArgs) = MainActivity.<>c.<>9__12_0
			Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__12_
			If <>9__12_ Is Nothing Then
				Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
					Process.KillProcess(Process.MyPid())
				End Sub
				eventHandler = eventHandler2
				MainActivity.<>c.<>9__12_0 = eventHandler2
			End If
			Me._alertDialog = builder.SetPositiveButton(text, eventHandler).SetNegativeButton("No", Sub(s As Object, <Nullable(1)> e As DialogClickEventArgs)
				Me._alertDialog.Dismiss()
				Me._alertDialog = Nothing
			End Sub).Create()
			Me._alertDialog.Show()
		End Sub

		' Token: 0x04000002 RID: 2
		Private _testListFragment As TestListFragment

		' Token: 0x04000003 RID: 3
		Private _alertDialog As AndroidX.AppCompat.App.AlertDialog
	End Class
End Namespace
