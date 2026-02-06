Imports System

Namespace SmartDCP.[Shared]
	' Token: 0x0200001B RID: 27
	Public Interface ISettings
		' Token: 0x17000020 RID: 32
		' (get) Token: 0x060000D7 RID: 215
		ReadOnly Property SampleRate As Double

		' Token: 0x17000021 RID: 33
		' (get) Token: 0x060000D8 RID: 216
		ReadOnly Property SampleSettle As Double

		' Token: 0x17000022 RID: 34
		' (get) Token: 0x060000D9 RID: 217
		ReadOnly Property SampleThreshold As Double

		' Token: 0x17000023 RID: 35
		' (get) Token: 0x060000DA RID: 218
		ReadOnly Property DefaultWeight As Weight

		' Token: 0x17000024 RID: 36
		' (get) Token: 0x060000DB RID: 219
		' (set) Token: 0x060000DC RID: 220
		Property RangefinderName As String

		' Token: 0x17000025 RID: 37
		' (get) Token: 0x060000DD RID: 221
		' (set) Token: 0x060000DE RID: 222
		Property IsEULAAccepted As Boolean
	End Interface
End Namespace
