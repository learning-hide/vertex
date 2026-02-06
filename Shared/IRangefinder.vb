Imports System

Namespace SmartDCP.[Shared]
	' Token: 0x02000018 RID: 24
	Public Interface IRangefinder
		' Token: 0x1400000B RID: 11
		' (add) Token: 0x060000C8 RID: 200
		' (remove) Token: 0x060000C9 RID: 201
		Event MeasurementChanged As EventHandler

		' Token: 0x1700001C RID: 28
		' (get) Token: 0x060000CA RID: 202
		ReadOnly Property Measurement As Single

		' Token: 0x060000CB RID: 203
		Sub Measure()

		' Token: 0x060000CC RID: 204
		Sub Scan()

		' Token: 0x060000CD RID: 205
		Sub Open(name As String)

		' Token: 0x1700001D RID: 29
		' (get) Token: 0x060000CE RID: 206
		ReadOnly Property IsOpen As Boolean

		' Token: 0x060000CF RID: 207
		Sub Close()

		' Token: 0x1700001E RID: 30
		' (get) Token: 0x060000D0 RID: 208
		ReadOnly Property IsConnected As Boolean

		' Token: 0x1700001F RID: 31
		' (get) Token: 0x060000D1 RID: 209
		ReadOnly Property IsSupported As Boolean
	End Interface
End Namespace
