Imports System
Imports System.Collections.Generic
Imports System.Threading
Imports Android.App
Imports Android.Bluetooth
Imports Android.Runtime
Imports Java.Interop
Imports Java.Lang
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.Platform
	' Token: 0x02000010 RID: 16
	Public Class BluetoothLeDiscover
		Inherits Java.Lang.[Object]
		Implements BluetoothAdapter.ILeScanCallback, IJavaObject, IDisposable, IJavaPeerable

		' Token: 0x14000001 RID: 1
		' (add) Token: 0x06000075 RID: 117 RVA: 0x000042E4 File Offset: 0x000024E4
		' (remove) Token: 0x06000076 RID: 118 RVA: 0x0000431C File Offset: 0x0000251C
		Public Event Discovered As EventHandler(Of DiscoveredRangefinder)

		' Token: 0x06000077 RID: 119 RVA: 0x00004351 File Offset: 0x00002551
		Public Sub New(activity As Activity)
			Me._activity = activity
		End Sub

		' Token: 0x1700000E RID: 14
		' (get) Token: 0x06000078 RID: 120 RVA: 0x00004377 File Offset: 0x00002577
		Public ReadOnly Property IsOpen As Boolean
			Get
				Return Me._isOpen
			End Get
		End Property

		' Token: 0x06000079 RID: 121 RVA: 0x0000437F File Offset: 0x0000257F
		Public Sub Open()
			Me._manager = CType(Me._activity.GetSystemService("bluetooth"), BluetoothManager)
			Me._adapter = Me._manager.Adapter
			Me._isOpen = True
		End Sub

		' Token: 0x0600007A RID: 122 RVA: 0x000043B4 File Offset: 0x000025B4
		Public Sub Close()
			Me._isOpen = False
			Me.StopScan(True)
			Try
				If Me._adapter IsNot Nothing Then
					Me._adapter.CancelDiscovery()
					Me._adapter.Dispose()
					Me._adapter = Nothing
				End If
			Catch ex As Global.System.Exception
			End Try
			Try
				If Me._manager IsNot Nothing Then
					Me._manager.Dispose()
					Me._manager = Nothing
				End If
			Catch ex2 As Global.System.Exception
			End Try
		End Sub

		' Token: 0x0600007B RID: 123 RVA: 0x00004438 File Offset: 0x00002638
		Public Sub StartScan()
			If Me._isOpen AndAlso Me._adapter IsNot Nothing AndAlso Not Me._scanning Then
				Me._scanning = True
				Dim scanned As List(Of String) = Me._scanned
				SyncLock scanned
					Me._scanned.Clear()
				End SyncLock
				Me._adapter.StartLeScan(Me)
				Me._event.Reset()
				If Me._scanThread Is Nothing Then
					Me._scanThread = AddressOf Me.ScanTimeoutThread
					Me._scanThread.Start()
				End If
			End If
		End Sub

		' Token: 0x0600007C RID: 124 RVA: 0x000044E4 File Offset: 0x000026E4
		Private Sub ScanTimeoutThread()
			If Not Me._event.WaitOne(5000) Then
				Me.StopScan(False)
			End If
		End Sub

		' Token: 0x0600007D RID: 125 RVA: 0x00004500 File Offset: 0x00002700
		Public Sub StopScan(Optional join As Boolean = True)
			Me._event.[Set]()
			If Me._scanThread IsNot Nothing Then
				If join AndAlso Me._scanThread.IsAlive Then
					Me._scanThread.Join(10000)
				End If
				Me._scanThread = Nothing
			End If
			Dim adapter As BluetoothAdapter = Me._adapter
			If adapter IsNot Nothing Then
				adapter.StopLeScan(Me)
			End If
			If PermissionsHelper.CheckBluetoothPermissions(Me._activity) Then
				Dim adapter2 As BluetoothAdapter = Me._adapter
				If adapter2 IsNot Nothing Then
					adapter2.CancelDiscovery()
				End If
			End If
			Me._scanning = False
		End Sub

		' Token: 0x0600007E RID: 126 RVA: 0x00004584 File Offset: 0x00002784
		Public Sub OnLeScan(device As BluetoothDevice, rssi As Integer, scanRecord As Byte()) Implements Android.Bluetooth.BluetoothAdapter.ILeScanCallback.OnLeScan
			If Me._isOpen AndAlso device IsNot Nothing Then
				Dim flag As Boolean = False
				Dim scanned As List(Of String) = Me._scanned
				SyncLock scanned
					Dim name As String = device.Name
					If Not String.IsNullOrEmpty(name) Then
						flag = Not Me._scanned.Contains(name)
						If flag Then
							Me._scanned.Add(name)
						End If
					End If
				End SyncLock
				If flag Then
					Me.OnDiscovered(New DiscoveredRangefinder() With { .Name = device.Name })
				End If
				device.Dispose()
				device = Nothing
			End If
		End Sub

		' Token: 0x0600007F RID: 127 RVA: 0x0000461C File Offset: 0x0000281C
		Protected Sub OnDiscovered(discovered As DiscoveredRangefinder)
			If Me.Discovered IsNot Nothing Then
				Me.Discovered(Me, discovered)
			End If
		End Sub

		' Token: 0x04000028 RID: 40
		Private Const MaxScanDuration As Integer = 5000

		' Token: 0x0400002A RID: 42
		Private _activity As Activity

		' Token: 0x0400002B RID: 43
		Private _adapter As BluetoothAdapter

		' Token: 0x0400002C RID: 44
		Private _isOpen As Boolean

		' Token: 0x0400002D RID: 45
		Private _manager As BluetoothManager

		' Token: 0x0400002E RID: 46
		Private _scanned As List(Of String) = New List(Of String)()

		' Token: 0x0400002F RID: 47
		Private _scanning As Boolean

		' Token: 0x04000030 RID: 48
		Private _scanThread As Global.System.Threading.Thread

		' Token: 0x04000031 RID: 49
		Private _event As ManualResetEvent = New ManualResetEvent(False)
	End Class
End Namespace
