Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Android.App

Namespace SmartDCP.[Shared]
	' Token: 0x0200001F RID: 31
	Public Class Test
		' Token: 0x1700002C RID: 44
		' (get) Token: 0x060000E8 RID: 232 RVA: 0x00005C7B File Offset: 0x00003E7B
		' (set) Token: 0x060000E9 RID: 233 RVA: 0x00005C8C File Offset: 0x00003E8C
		Public Property Name As String
			Get
				Return If(Me._name, String.Empty)
			End Get
			Set(value As String)
				Me._name = value
			End Set
		End Property

		' Token: 0x1700002D RID: 45
		' (get) Token: 0x060000EA RID: 234 RVA: 0x00005C98 File Offset: 0x00003E98
		Public ReadOnly Property DateTime As String
			Get
				Dim fileName As String = Global.System.IO.Path.GetFileName(Me.Path)
				Try
					Dim num As Integer
					Dim num2 As Integer
					Dim num3 As Integer
					Dim num4 As Integer
					Dim num5 As Integer
					Dim num6 As Integer
					If Not String.IsNullOrEmpty(fileName) AndAlso Integer.TryParse(fileName.Substring(0, 4), num) AndAlso Integer.TryParse(fileName.Substring(4, 2), num2) AndAlso Integer.TryParse(fileName.Substring(6, 2), num3) AndAlso Integer.TryParse(fileName.Substring(8, 2), num4) AndAlso Integer.TryParse(fileName.Substring(10, 2), num5) AndAlso Integer.TryParse(fileName.Substring(12, 2), num6) Then
						Dim dateTime As DateTime = New DateTime(num, num2, num3, num4, num5, num6)
						Return String.Format("{0} {1}", dateTime.ToLongDateString(), dateTime.ToLongTimeString())
					End If
				Catch ex As Exception
				End Try
				Return fileName
			End Get
		End Property

		' Token: 0x1700002E RID: 46
		' (get) Token: 0x060000EB RID: 235 RVA: 0x00005D6C File Offset: 0x00003F6C
		Public Shared ReadOnly Property Directory As String
			Get
				Return Global.System.IO.Path.Combine(Application.Context.GetExternalFilesDir("").AbsolutePath, "SmartDCP")
			End Get
		End Property

		' Token: 0x060000EC RID: 236 RVA: 0x00005D8C File Offset: 0x00003F8C
		Public Shared Sub Write(samples As IReadOnlyList(Of Sample))
			If samples.Count > 0 Then
				File.AppendAllText(samples(0).Path, "Time (ms), Name, Depth (mm), Delta (mm), CBR (%)" & vbLf)
				For Each sample As Sample In samples
					Dim text As String = If(sample.Name, String.Empty)
					File.AppendAllText(sample.Path, String.Format("{0},{1},{2},{3},{4:F2}" & vbLf, New Object() { CLng(sample.Time), text.Replace(",", "_"), CLng(sample.Depth), CLng(sample.Delta), sample.CBR }))
				Next
			End If
		End Sub

		' Token: 0x060000ED RID: 237 RVA: 0x00005E70 File Offset: 0x00004070
		Public Shared Function LoadAll() As List(Of Test)
			Dim list As List(Of Test) = New List(Of Test)()
			If Not Global.System.IO.Directory.Exists(Test.Directory) Then
				Global.System.IO.Directory.CreateDirectory(Test.Directory)
			End If
			Dim files As String() = Global.System.IO.Directory.GetFiles(Test.Directory, "*.csv", SearchOption.TopDirectoryOnly)
			If files IsNot Nothing Then
				Dim array As String() = files
				For i As Integer = 0 To array.Length - 1
					Dim test As Test = Test.Load(array(i))
					If test IsNot Nothing AndAlso test.Samples.Count > 0 Then
						list.Add(test)
					End If
				Next
			End If
			Return list
		End Function

		' Token: 0x060000EE RID: 238 RVA: 0x00005EE4 File Offset: 0x000040E4
		Public Shared Function Load(path As String) As Test
			Dim test As Test = Nothing
			If Not String.IsNullOrEmpty(path) AndAlso File.Exists(path) Then
				test = New Test() With { .Path = path }
				Dim array As String() = File.ReadAllLines(path)
				If array IsNot Nothing Then
					Dim array2 As String() = array
					For i As Integer = 0 To array2.Length - 1
						Dim array3 As String() = array2(i).Split(","c, StringSplitOptions.None)
						If array3.Length >= 5 Then
							Dim sample As Sample = New Sample()
							sample.Path = path
							If Double.TryParse(array3(0), sample.Time) AndAlso Double.TryParse(array3(2), sample.Depth) AndAlso Double.TryParse(array3(3), sample.Delta) AndAlso Double.TryParse(array3(4), sample.CBR) Then
								Dim text As String = array3(1).Trim()
								If String.IsNullOrEmpty(test.Name) AndAlso Not String.IsNullOrEmpty(text) Then
									test.Name = text
								End If
								test.Samples.Add(sample)
							End If
						End If
					Next
				End If
			End If
			Return test
		End Function

		' Token: 0x04000066 RID: 102
		Public Path As String = String.Empty

		' Token: 0x04000067 RID: 103
		Public Weight As Weight

		' Token: 0x04000068 RID: 104
		Public Samples As List(Of Sample) = New List(Of Sample)()

		' Token: 0x04000069 RID: 105
		Private _name As String
	End Class
End Namespace
