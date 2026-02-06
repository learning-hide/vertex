Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports Android.App
Imports Android.Graphics
Imports Android.Graphics.Drawables
Imports Android.Runtime
Imports Android.Views
Imports Android.Widget
Imports Java.Interop
Imports Java.Lang
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x02000009 RID: 9
	Public Class TestListAdapter
		Inherits BaseAdapter(Of Test)
		Implements IFilterable, IJavaObject, IDisposable, IJavaPeerable

		' Token: 0x06000026 RID: 38 RVA: 0x00002A80 File Offset: 0x00000C80
		Public Sub New(context As Activity)
			Me._context = context
			Me.Filter = New TestListAdapter.TestListFilter(Me)
		End Sub

		' Token: 0x06000027 RID: 39 RVA: 0x00002ABC File Offset: 0x00000CBC
		Public Sub Load()
			Me._tests.Clear()
			Me._tests.AddRange(Test.LoadAll())
			Dim tests As List(Of Test) = Me._tests
			Dim <>9__2_ As Comparison(Of Test) = TestListAdapter.<>c.<>9__2_0
			Dim comparison As Comparison(Of Test) = <>9__2_
			If <>9__2_ Is Nothing Then
				Dim comparison2 As Comparison(Of Test) = Function(a As Test, b As Test) String.Compare(a.Name, b.Name, True, CultureInfo.InvariantCulture)
				comparison = comparison2
				TestListAdapter.<>c.<>9__2_0 = comparison2
			End If
			tests.Sort(comparison)
			Me._filtered.Clear()
			Me._filtered.AddRange(Me._tests)
			Me._selected.Clear()
			Me.NotifyDataSetChanged()
		End Sub

		' Token: 0x17000002 RID: 2
		' (get) Token: 0x06000028 RID: 40 RVA: 0x00002B3B File Offset: 0x00000D3B
		' (set) Token: 0x06000029 RID: 41 RVA: 0x00002B43 File Offset: 0x00000D43
		Public Property State As TestListState
			Get
				Return Me._state
			End Get
			Set(value As TestListState)
				If Me._state <> value Then
					Me._state = value
					If Me.StateChanged IsNot Nothing Then
						Me.StateChanged(Me, EventArgs.Empty)
					End If
				End If
			End Set
		End Property

		' Token: 0x17000003 RID: 3
		' (get) Token: 0x0600002A RID: 42 RVA: 0x00002B6E File Offset: 0x00000D6E
		' (set) Token: 0x0600002B RID: 43 RVA: 0x00002B76 File Offset: 0x00000D76
		Public Property Filter As Filter Implements Android.Widget.IFilterable.Filter

		' Token: 0x17000004 RID: 4
		' (get) Token: 0x0600002C RID: 44 RVA: 0x00002B7F File Offset: 0x00000D7F
		Public Overrides ReadOnly Property Count As Integer
			Get
				Return Me._filtered.Count
			End Get
		End Property

		' Token: 0x17000005 RID: 5
		Public Overrides ReadOnly Default Property Item(position As Integer) As Test
			Get
				Return Me._filtered(position)
			End Get
		End Property

		' Token: 0x0600002E RID: 46 RVA: 0x00002B9A File Offset: 0x00000D9A
		Public Overrides Function GetItemId(position As Integer) As Long
			Return CLng(position)
		End Function

		' Token: 0x0600002F RID: 47 RVA: 0x00002BA0 File Offset: 0x00000DA0
		Public Overrides Function GetView(position As Integer, convertView As View, parent As ViewGroup) As View
			Dim view As View = If(convertView, Me._context.LayoutInflater.Inflate(17367044, Nothing))
			Dim test As Test = Me(position)
			If view IsNot Nothing Then
				If Me.IsSelected(test) Then
					view.Background = New ColorDrawable(Color.LightGray)
				Else
					view.Background = Nothing
				End If
				view.FindViewById(Of TextView)(16908308).Text = test.Name
				view.FindViewById(Of TextView)(16908309).Text = test.DateTime
			End If
			Return view
		End Function

		' Token: 0x17000006 RID: 6
		' (get) Token: 0x06000030 RID: 48 RVA: 0x00002C23 File Offset: 0x00000E23
		Public ReadOnly Property Selected As IReadOnlyList(Of String)
			Get
				Return Me._selected
			End Get
		End Property

		' Token: 0x06000031 RID: 49 RVA: 0x00002C2B File Offset: 0x00000E2B
		Public Function IsSelected(test As Test) As Boolean
			Return Me._selected.Contains(test.Path)
		End Function

		' Token: 0x06000032 RID: 50 RVA: 0x00002C3E File Offset: 0x00000E3E
		Public Sub [Select](test As Test)
			If Not Me.IsSelected(test) Then
				Me._selected.Add(test.Path)
				Me.NotifyDataSetChanged()
			End If
		End Sub

		' Token: 0x06000033 RID: 51 RVA: 0x00002C60 File Offset: 0x00000E60
		Public Sub UnSelect(test As Test)
			If Me.IsSelected(test) Then
				Me._selected.Remove(test.Path)
				Me.NotifyDataSetChanged()
			End If
		End Sub

		' Token: 0x06000034 RID: 52 RVA: 0x00002C83 File Offset: 0x00000E83
		Public Sub ClearSelect()
			Me._selected.Clear()
			Me.NotifyDataSetChanged()
		End Sub

		' Token: 0x04000008 RID: 8
		Public StateChanged As EventHandler

		' Token: 0x0400000A RID: 10
		Private _context As Activity

		' Token: 0x0400000B RID: 11
		Private _tests As List(Of Test) = New List(Of Test)()

		' Token: 0x0400000C RID: 12
		Private _filtered As List(Of Test) = New List(Of Test)()

		' Token: 0x0400000D RID: 13
		Private _selected As List(Of String) = New List(Of String)()

		' Token: 0x0400000E RID: 14
		Private _state As TestListState

		' Token: 0x0200003A RID: 58
		Private Class TestListFilter
			Inherits Filter

			' Token: 0x06000141 RID: 321 RVA: 0x00006FAF File Offset: 0x000051AF
			Public Sub New(adapter As TestListAdapter)
				Me._adapter = adapter
			End Sub

			' Token: 0x06000142 RID: 322 RVA: 0x00006FC0 File Offset: 0x000051C0
			Protected Overrides Function PerformFiltering(constraint As ICharSequence) As Filter.FilterResults
				Dim filterResults As Filter.FilterResults = New Filter.FilterResults()
				Me._adapter._filtered.Clear()
				If constraint Is Nothing Then
					Me._adapter._filtered.AddRange(Me._adapter._tests)
				Else
					Dim list As List(Of Test) = New List(Of Test)()
					Dim text As String = constraint.ToString()
					For Each test As Test In Me._adapter._tests
						If Not String.IsNullOrEmpty(test.Name) AndAlso (test.Name.ToUpperInvariant().Contains(text.ToUpperInvariant()) OrElse test.DateTime.ToUpperInvariant().Contains(text.ToUpperInvariant())) Then
							list.Add(test)
						End If
					Next
					Me._adapter._filtered.AddRange(list)
					Dim array As Java.Lang.[Object]() = New Java.Lang.[Object](list.Count - 1) {}
					For i As Integer = 0 To list.Count - 1
						array(i) = New Java.Lang.[String](list(i).Name)
					Next
					filterResults.Values = array
					filterResults.Count = list.Count
					Try
						constraint.Dispose()
					Catch ex As Global.System.Exception
					End Try
				End If
				Return filterResults
			End Function

			' Token: 0x06000143 RID: 323 RVA: 0x0000711C File Offset: 0x0000531C
			Protected Overrides Sub PublishResults(constraint As ICharSequence, results As Filter.FilterResults)
				Me._adapter.NotifyDataSetChanged()
				Try
					constraint.Dispose()
					results.Dispose()
				Catch ex As Global.System.Exception
				End Try
			End Sub

			' Token: 0x040007C2 RID: 1986
			Private _adapter As TestListAdapter
		End Class
	End Class
End Namespace
