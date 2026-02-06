Imports System
Imports System.Collections.Generic
Imports Android.App
Imports Android.Graphics
Imports Android.Graphics.Drawables
Imports Android.Views
Imports Android.Widget
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x0200000E RID: 14
	Public Class DiscoverAdapter
		Inherits BaseAdapter(Of DiscoveredRangefinder)

		' Token: 0x06000062 RID: 98 RVA: 0x00003F83 File Offset: 0x00002183
		Public Sub New(context As Activity)
			Me._context = context
		End Sub

		' Token: 0x17000008 RID: 8
		' (get) Token: 0x06000063 RID: 99 RVA: 0x00003FA8 File Offset: 0x000021A8
		Public ReadOnly Property Selected As String
			Get
				For Each text As String In Me._selected
					For Each discoveredRangefinder As DiscoveredRangefinder In Me._discovered
						If String.Equals(discoveredRangefinder.Name, text, StringComparison.OrdinalIgnoreCase) Then
							Return discoveredRangefinder.Name
						End If
					Next
				Next
				Return String.Empty
			End Get
		End Property

		' Token: 0x06000064 RID: 100 RVA: 0x00002B9A File Offset: 0x00000D9A
		Public Overrides Function GetItemId(position As Integer) As Long
			Return CLng(position)
		End Function

		' Token: 0x17000009 RID: 9
		Public Overrides ReadOnly Default Property Item(position As Integer) As DiscoveredRangefinder
			Get
				Return Me._discovered(position)
			End Get
		End Property

		' Token: 0x1700000A RID: 10
		' (get) Token: 0x06000066 RID: 102 RVA: 0x0000405E File Offset: 0x0000225E
		Public Overrides ReadOnly Property Count As Integer
			Get
				Return Me._discovered.Count
			End Get
		End Property

		' Token: 0x06000067 RID: 103 RVA: 0x0000406C File Offset: 0x0000226C
		Public Overrides Function GetView(position As Integer, convertView As View, parent As ViewGroup) As View
			Dim view As View = If(convertView, Me._context.LayoutInflater.Inflate(17367043, Nothing))
			Dim discoveredRangefinder As DiscoveredRangefinder = Me(position)
			If view IsNot Nothing Then
				If Me.IsSelected(discoveredRangefinder) Then
					view.Background = New ColorDrawable(Color.LightGray)
				Else
					view.Background = Nothing
				End If
				view.FindViewById(Of TextView)(16908308).Text = discoveredRangefinder.Name
			End If
			Return view
		End Function

		' Token: 0x06000068 RID: 104 RVA: 0x000040D9 File Offset: 0x000022D9
		Public Function IsSelected(item As DiscoveredRangefinder) As Boolean
			Return Me._selected.Contains(item.Name)
		End Function

		' Token: 0x06000069 RID: 105 RVA: 0x000040EC File Offset: 0x000022EC
		Public Sub [Select](item As DiscoveredRangefinder)
			If Not Me.IsSelected(item) Then
				Me._selected.Add(item.Name)
				Me.NotifyDataSetChanged()
			End If
		End Sub

		' Token: 0x0600006A RID: 106 RVA: 0x0000410E File Offset: 0x0000230E
		Public Sub UnSelect(item As DiscoveredRangefinder)
			If Me.IsSelected(item) Then
				Me._selected.Remove(item.Name)
				Me.NotifyDataSetChanged()
			End If
		End Sub

		' Token: 0x0600006B RID: 107 RVA: 0x00004131 File Offset: 0x00002331
		Public Sub Clear()
			Me._selected.Clear()
			Me._discovered.Clear()
			Me.NotifyDataSetChanged()
		End Sub

		' Token: 0x0600006C RID: 108 RVA: 0x00004150 File Offset: 0x00002350
		Public Sub Add(item As DiscoveredRangefinder)
			For Each discoveredRangefinder As DiscoveredRangefinder In Me._discovered
				If String.Equals(item.Name, discoveredRangefinder.Name, StringComparison.OrdinalIgnoreCase) Then
					Return
				End If
			Next
			Me._discovered.Add(item)
			Me.NotifyDataSetChanged()
		End Sub

		' Token: 0x04000023 RID: 35
		Private _context As Activity

		' Token: 0x04000024 RID: 36
		Private _discovered As List(Of DiscoveredRangefinder) = New List(Of DiscoveredRangefinder)()

		' Token: 0x04000025 RID: 37
		Private _selected As List(Of String) = New List(Of String)()
	End Class
End Namespace
