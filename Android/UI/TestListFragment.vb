Imports System
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Android.Content
Imports Android.Net
Imports Android.OS
Imports Android.Views
Imports Android.Widget
Imports AndroidX.Core.Content
Imports AndroidX.Fragment.App
Imports Java.IO
Imports Java.Util.Zip
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x0200000D RID: 13
	Public Class TestListFragment
		Inherits ListFragment

		' Token: 0x17000007 RID: 7
		' (get) Token: 0x06000056 RID: 86 RVA: 0x000038F0 File Offset: 0x00001AF0
		Public ReadOnly Property State As TestListState
			Get
				If Me._adapter IsNot Nothing Then
					Return Me._adapter.State
				End If
				Return TestListState.[Default]
			End Get
		End Property

		' Token: 0x06000057 RID: 87 RVA: 0x0000273F File Offset: 0x0000093F
		Public Overrides Sub OnCreate(savedState As Bundle)
			MyBase.OnCreate(savedState)
			MyBase.HasOptionsMenu = True
		End Sub

		' Token: 0x06000058 RID: 88 RVA: 0x00003908 File Offset: 0x00001B08
		Public Overrides Sub OnActivityCreated(savedInstanceState As Bundle)
			MyBase.OnActivityCreated(savedInstanceState)
			Me._adapter = New TestListAdapter(MyBase.Activity)
			Dim adapter As TestListAdapter = Me._adapter
			adapter.StateChanged = CType([Delegate].Combine(adapter.StateChanged, New EventHandler(Sub(sender As Object, args As EventArgs)
				Dim stateChanged As EventHandler = Me.StateChanged
				If stateChanged Is Nothing Then
					Return
				End If
				stateChanged(Me, EventArgs.Empty)
			End Sub)), EventHandler)
			Me._adapter.Load()
			Me.ListAdapter = Me._adapter
			Me._searchView = MyBase.Activity.FindViewById(Of SearchView)(2131296386)
			AddHandler Me._searchView.QueryTextChange, Sub(s As Object, <Nullable(1)> e As SearchView.QueryTextChangeEventArgs)
				Me._adapter.Filter.InvokeFilter(e.NewText)
			End Sub
			Me.RegisterForContextMenu(Me.ListView)
		End Sub

		' Token: 0x06000059 RID: 89 RVA: 0x000039A4 File Offset: 0x00001BA4
		Public Overrides Sub OnCreateContextMenu(menu As IContextMenu, v As View, menuInfo As IContextMenuContextMenuInfo)
			MyBase.OnCreateContextMenu(menu, v, menuInfo)
			Dim adapterContextMenuInfo As AdapterView.AdapterContextMenuInfo = TryCast(menuInfo, AdapterView.AdapterContextMenuInfo)
			If adapterContextMenuInfo IsNot Nothing Then
				Dim test As Test = Me._adapter(adapterContextMenuInfo.Position)
				If test IsNot Nothing Then
					menu.SetHeaderTitle(test.Name)
				End If
			End If
			menu.Add(0, 0, 0, "Delete")
		End Sub

		' Token: 0x0600005A RID: 90 RVA: 0x000039F8 File Offset: 0x00001BF8
		Public Overrides Function OnContextItemSelected(item As IMenuItem) As Boolean
			Dim adapterContextMenuInfo As AdapterView.AdapterContextMenuInfo = TryCast(item.MenuInfo, AdapterView.AdapterContextMenuInfo)
			If adapterContextMenuInfo IsNot Nothing AndAlso item.ItemId = 0 Then
				Dim test As Test = Me._adapter(adapterContextMenuInfo.Position)
				If Global.System.IO.File.Exists(test.Path) Then
					Global.System.IO.File.Delete(test.Path)
				End If
				Me._adapter.Load()
				If Not String.IsNullOrEmpty(Me._searchView.Query) Then
					Me._adapter.Filter.InvokeFilter(Me._searchView.Query)
				End If
				Return True
			End If
			Return MyBase.OnContextItemSelected(item)
		End Function

		' Token: 0x0600005B RID: 91 RVA: 0x00003A88 File Offset: 0x00001C88
		Public Overrides Sub OnPrepareOptionsMenu(menu As IMenu)
			MyBase.OnPrepareOptionsMenu(menu)
			menu.Clear()
			If Me._adapter.State = TestListState.[Default] Then
				MyBase.Activity.MenuInflater.Inflate(2131558401, menu)
				Return
			End If
			If TestListState.Selecting = Me._adapter.State Then
				MyBase.Activity.MenuInflater.Inflate(2131558402, menu)
			End If
		End Sub

		' Token: 0x0600005C RID: 92 RVA: 0x00003AEC File Offset: 0x00001CEC
		Public Overrides Function OnOptionsItemSelected(item As IMenuItem) As Boolean
			Dim itemId As Integer = item.ItemId
			If itemId <= 2131296339 Then
				If itemId = 2131296324 Then
					Me._adapter.State = TestListState.[Default]
					MyBase.Activity.InvalidateOptionsMenu()
					Me._adapter.ClearSelect()
					Return True
				End If
				If itemId = 2131296339 Then
					Me.SendEmail()
					Me._adapter.State = TestListState.[Default]
					MyBase.Activity.InvalidateOptionsMenu()
					Me._adapter.ClearSelect()
					Return True
				End If
			Else
				If itemId = 2131296397 Then
					Me._adapter.State = TestListState.Selecting
					MyBase.Activity.InvalidateOptionsMenu()
					Return True
				End If
				If itemId = 2131296399 Then
					If TypeOf MyBase.Activity Is MainActivity Then
						TryCast(MyBase.Activity, MainActivity).ShowDiscover()
					End If
					Return True
				End If
				If itemId = 2131296434 Then
					If TypeOf MyBase.Activity Is MainActivity Then
						TryCast(MyBase.Activity, MainActivity).ShowTest()
					End If
					Return True
				End If
			End If
			Return MyBase.OnOptionsItemSelected(item)
		End Function

		' Token: 0x0600005D RID: 93 RVA: 0x00003BEC File Offset: 0x00001DEC
		Public Sub SendEmail()
			Try
				If Me._adapter.Selected.Count > 0 Then
					Dim directoryName As String = Path.GetDirectoryName(Me._adapter.Selected(0))
					If Not String.IsNullOrEmpty(directoryName) Then
						Try
							Dim files As String() = Directory.GetFiles(directoryName, "*.zip", SearchOption.TopDirectoryOnly)
							For i As Integer = 0 To files.Length - 1
								Global.System.IO.File.Delete(files(i))
							Next
						Catch ex As Exception
						End Try
						Dim intent As Intent = New Intent("android.intent.action.SEND")
						intent.SetType("text/plain")
						intent.PutExtra("android.intent.extra.SUBJECT", "Smart DCP")
						Dim text As String = String.Format("SmartDCP_{0}_{1}.zip", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString())
						For Each c As Char In Path.GetInvalidFileNameChars()
							text = text.Replace(c, "_"c)
						Next
						For Each c2 As Char In Path.GetInvalidPathChars()
							text = text.Replace(c2, "_"c)
						Next
						For Each c3 As Char In New Char() { " "c, ":"c }
							text = text.Replace(c3, "_"c)
						Next
						Dim text2 As String = Path.Combine(directoryName, text)
						Using fileStream As FileStream = Global.System.IO.File.Create(text2)
							Using zipOutputStream As ZipOutputStream = New ZipOutputStream(fileStream)
								For Each text3 As String In Me._adapter.Selected
									Dim zipEntry As ZipEntry = New ZipEntry(Path.GetFileName(text3))
									zipOutputStream.PutNextEntry(zipEntry)
									Dim array2 As Byte() = Global.System.IO.File.ReadAllBytes(text3)
									zipOutputStream.Write(array2)
									zipOutputStream.CloseEntry()
								Next
								zipOutputStream.Close()
							End Using
							fileStream.Close()
						End Using
						Dim file As Java.IO.File = New Java.IO.File(text2)
						file.SetReadable(True, False)
						Dim uriForFile As Uri = FileProvider.GetUriForFile(MyBase.Activity, "com.ara.vertek.smartdcp.provider", file)
						intent.PutExtra("android.intent.extra.STREAM", uriForFile)
						intent.AddFlags(ActivityFlags.GrantReadUriPermission)
						MyBase.Activity.StartActivity(Intent.CreateChooser(intent, "Send mail..."))
					End If
				End If
			Catch ex2 As Exception
				If TypeOf MyBase.Activity Is MainActivity Then
					TryCast(MyBase.Activity, MainActivity).OnException(Me, ex2)
				End If
			End Try
		End Sub

		' Token: 0x0600005E RID: 94 RVA: 0x00003ED8 File Offset: 0x000020D8
		Public Overrides Sub OnListItemClick(listView As ListView, view As View, index As Integer, id As Long)
			Dim test As Test = Me._adapter(index)
			If Me._adapter.State = TestListState.[Default] Then
				If TypeOf MyBase.Activity Is MainActivity Then
					TryCast(MyBase.Activity, MainActivity).ShowPlot(test)
					Return
				End If
			ElseIf TestListState.Selecting = Me._adapter.State Then
				If Not Me._adapter.IsSelected(test) Then
					Me._adapter.[Select](test)
					Return
				End If
				Me._adapter.UnSelect(test)
			End If
		End Sub

		' Token: 0x04000020 RID: 32
		Public StateChanged As EventHandler

		' Token: 0x04000021 RID: 33
		Private _adapter As TestListAdapter

		' Token: 0x04000022 RID: 34
		Private _searchView As SearchView
	End Class
End Namespace
