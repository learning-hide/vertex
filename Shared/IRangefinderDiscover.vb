Imports System

Namespace SmartDCP.[Shared]
	' Token: 0x0200001A RID: 26
	Public Interface IRangefinderDiscover
		' Token: 0x1400000C RID: 12
		' (add) Token: 0x060000D3 RID: 211
		' (remove) Token: 0x060000D4 RID: 212
		Event Discovered As EventHandler(Of DiscoveredRangefinder)

		' Token: 0x060000D5 RID: 213
		Sub Start()

		' Token: 0x060000D6 RID: 214
		Sub [Stop]()
	End Interface
End Namespace
