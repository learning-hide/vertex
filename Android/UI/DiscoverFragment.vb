Imports System
Imports System.Linq
Imports Android.Content.PM
Imports Android.OS
Imports Android.Views
Imports Android.Widget
Imports AndroidX.Fragment.App
Imports SmartDCP.Android.Platform
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x02000008 RID: 8
	Friend Class DiscoverFragment
		Inherits ListFragment

		' Token: 0x0600001C RID: 28 RVA: 0x0000273F File Offset: 0x0000093F
		Public Overrides Sub OnCreate(savedState As Bundle)
			MyBase.OnCreate(savedState)
			MyBase.HasOptionsMenu = True
		End Sub

		' Token: 0x0600001D RID: 29 RVA: 0x00002750 File Offset: 0x00000950
		Public Overrides Sub OnActivityCreated(savedInstanceState As Bundle)
			MyBase.OnActivityCreated(savedInstanceState)
			Me._discover = New BluetoothLeDiscover(MyBase.Activity)
			AddHandler Me._discover.Discovered, AddressOf Me.OnDiscovered
			Me._discover.Open()
			If PermissionsHelper.CheckAndRequestBluetoothPermissions(Me) Then
				Me._discover.StartScan()
			End If
			Me._adapter = New DiscoverAdapter(MyBase.Activity)
			Me.ListAdapter = Me._adapter
			Me._settings = New Settings(MyBase.Activity)
			Me.Refresh()
		End Sub

		' Token: 0x0600001E RID: 30 RVA: 0x000027DE File Offset: 0x000009DE
		Public Overrides Sub OnPrepareOptionsMenu(menu As IMenu)
			MyBase.OnPrepareOptionsMenu(menu)
			menu.Clear()
			MyBase.Activity.MenuInflater.Inflate(2131558400, menu)
		End Sub

		' Token: 0x0600001F RID: 31 RVA: 0x00002804 File Offset: 0x00000A04
		Public Overrides Function OnOptionsItemSelected(item As IMenuItem) As Boolean
			Dim itemId As Integer = item.ItemId
			If itemId = 2131296324 Then
				If Me._discover IsNot Nothing Then
					Me._discover.Close()
					Me._discover = Nothing
				End If
				Me._settings.RangefinderName = Me._adapter.Selected
				Dim mainActivity As MainActivity = TryCast(MyBase.Activity, MainActivity)
				If mainActivity IsNot Nothing Then
					mainActivity.ShowTestList()
				End If
				Return True
			End If
			If itemId <> 2131296377 Then
				Return MyBase.OnOptionsItemSelected(item)
			End If
			Me.Refresh()
			Return True
		End Function

		' Token: 0x06000020 RID: 32 RVA: 0x00002884 File Offset: 0x00000A84
		Protected Sub OnDiscovered(sender As Object, e As DiscoveredRangefinder)
			Try
				Dim activity As FragmentActivity = MyBase.Activity
				If activity IsNot Nothing Then
					activity.RunOnUiThread(Sub()
						Me._adapter.Add(e)
					End Sub)
				End If
			Catch ex As Exception
				Dim mainActivity As MainActivity = TryCast(MyBase.Activity, MainActivity)
				If mainActivity IsNot Nothing Then
					mainActivity.OnException(Me, ex)
				End If
			End Try
		End Sub

		' Token: 0x06000021 RID: 33 RVA: 0x000028F0 File Offset: 0x00000AF0
		Public Sub Refresh()
			Me._adapter.Clear()
			If Not String.IsNullOrEmpty(Me._settings.RangefinderName) Then
				Dim discoveredRangefinder As DiscoveredRangefinder = New DiscoveredRangefinder() With { .Name = Me._settings.RangefinderName }
				Me._adapter.Add(discoveredRangefinder)
				Me._adapter.[Select](discoveredRangefinder)
			End If
			If PermissionsHelper.CheckAndRequestBluetoothPermissions(Me) Then
				Me._discover.StartScan()
			End If
		End Sub

		' Token: 0x06000022 RID: 34 RVA: 0x0000295C File Offset: 0x00000B5C
		Public Overrides Sub OnListItemClick(listView As ListView, view As View, index As Integer, id As Long)
			For i As Integer = 0 To Me._adapter.Count - 1
				Dim discoveredRangefinder As DiscoveredRangefinder = Me._adapter(i)
				If index = i Then
					If Me._adapter.IsSelected(discoveredRangefinder) Then
						Me._adapter.UnSelect(discoveredRangefinder)
					Else
						Me._adapter.[Select](discoveredRangefinder)
					End If
				Else
					Me._adapter.UnSelect(discoveredRangefinder)
				End If
			Next
		End Sub

		' Token: 0x06000023 RID: 35 RVA: 0x000029C6 File Offset: 0x00000BC6
		Private Sub OnException(sender As Object, ex As Exception)
			Dim mainActivity As MainActivity = TryCast(MyBase.Activity, MainActivity)
			If mainActivity Is Nothing Then
				Return
			End If
			mainActivity.OnException(Me, ex)
		End Sub

		' Token: 0x06000024 RID: 36 RVA: 0x000029E0 File Offset: 0x00000BE0
		Public Overrides Sub OnRequestPermissionsResult(requestCode As Integer, permissions As String(), grantResults As Permission())
			If requestCode = 99 Then
				PermissionsHelper.Clear()
				If grantResults.Length <> 0 Then
					For i As Integer = 0 To permissions.Length - 1
						Console.WriteLine("Permission " + permissions(i) + " is " + If((grantResults(i) = Permission.Granted), "granted", "denied"))
					Next
					Dim <>9__8_ As Func(Of Permission, Boolean) = DiscoverFragment.<>c.<>9__8_0
					Dim func As Func(Of Permission, Boolean) = <>9__8_
					If <>9__8_ Is Nothing Then
						Dim func2 As Func(Of Permission, Boolean) = Function(p As Permission) p = Permission.Granted
						func = func2
						DiscoverFragment.<>c.<>9__8_0 = func2
					End If
					If grantResults.All(func) AndAlso PermissionsHelper.CheckBluetoothPermissions(Me.Context) Then
						Dim discover As BluetoothLeDiscover = Me._discover
						If discover Is Nothing Then
							Return
						End If
						discover.StartScan()
					End If
				End If
			End If
		End Sub

		' Token: 0x04000005 RID: 5
		Private _settings As Settings

		' Token: 0x04000006 RID: 6
		Private _discover As BluetoothLeDiscover

		' Token: 0x04000007 RID: 7
		Private _adapter As DiscoverAdapter
	End Class
End Namespace
