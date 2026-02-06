Imports System
Imports System.Collections.Generic
Imports Android.App
Imports Android.Views
Imports Android.Widget
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.UI
	' Token: 0x0200000F RID: 15
	Public Class TestSampleAdapter
		Inherits BaseAdapter(Of Sample)

		' Token: 0x0600006D RID: 109 RVA: 0x000041C4 File Offset: 0x000023C4
		Public Sub New(context As Activity)
			Me._context = context
		End Sub

		' Token: 0x1700000B RID: 11
		' (get) Token: 0x0600006E RID: 110 RVA: 0x000041DE File Offset: 0x000023DE
		Public ReadOnly Property Samples As IReadOnlyList(Of Sample)
			Get
				Return Me._samples
			End Get
		End Property

		' Token: 0x0600006F RID: 111 RVA: 0x00002B9A File Offset: 0x00000D9A
		Public Overrides Function GetItemId(position As Integer) As Long
			Return CLng(position)
		End Function

		' Token: 0x1700000C RID: 12
		Public Overrides ReadOnly Default Property Item(position As Integer) As Sample
			Get
				Return Me._samples(position)
			End Get
		End Property

		' Token: 0x1700000D RID: 13
		' (get) Token: 0x06000071 RID: 113 RVA: 0x000041F4 File Offset: 0x000023F4
		Public Overrides ReadOnly Property Count As Integer
			Get
				Return Me._samples.Count
			End Get
		End Property

		' Token: 0x06000072 RID: 114 RVA: 0x00004204 File Offset: 0x00002404
		Public Overrides Function GetView(position As Integer, convertView As View, parent As ViewGroup) As View
			Dim view As View = If(convertView, Me._context.LayoutInflater.Inflate(17367044, Nothing))
			view.FindViewById(Of TextView)(16908308).Text = String.Format("CBR {0:F0}%", Me._samples(position).CBR)
			view.FindViewById(Of TextView)(16908309).Text = String.Format("{0} mm, {1} mm", Me._samples(position).Delta.ToString("F1"), Me._samples(position).Depth.ToString("F1"))
			Return view
		End Function

		' Token: 0x06000073 RID: 115 RVA: 0x000042AC File Offset: 0x000024AC
		Public Function Add(sample As Sample) As Integer
			Me._samples.Add(sample)
			Me.NotifyDataSetChanged()
			Return Me._samples.Count - 1
		End Function

		' Token: 0x06000074 RID: 116 RVA: 0x000042CD File Offset: 0x000024CD
		Public Sub Remove(position As Integer)
			Me._samples.RemoveAt(position)
			Me.NotifyDataSetChanged()
		End Sub

		' Token: 0x04000026 RID: 38
		Private _context As Activity

		' Token: 0x04000027 RID: 39
		Private _samples As List(Of Sample) = New List(Of Sample)()
	End Class
End Namespace
