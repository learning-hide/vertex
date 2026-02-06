Imports System
Imports System.Collections.Generic
Imports System.Threading
Imports Android.App
Imports Android.Bluetooth
Imports Android.Runtime
Imports Java.Interop
Imports Java.Lang

Namespace SmartDCP.Android.Platform
	' Token: 0x02000011 RID: 17
	Public Class BluetoothLeManager
		Inherits Java.Lang.[Object]
		Implements BluetoothAdapter.ILeScanCallback, IJavaObject, IDisposable, IJavaPeerable

		' Token: 0x14000002 RID: 2
		' (add) Token: 0x06000080 RID: 128 RVA: 0x00004634 File Offset: 0x00002834
		' (remove) Token: 0x06000081 RID: 129 RVA: 0x0000466C File Offset: 0x0000286C
		Public Event Connected As EventHandler(Of BluetoothGatt)

		' Token: 0x14000003 RID: 3
		' (add) Token: 0x06000082 RID: 130 RVA: 0x000046A4 File Offset: 0x000028A4
		' (remove) Token: 0x06000083 RID: 131 RVA: 0x000046DC File Offset: 0x000028DC
		Public Event Disconnected As EventHandler(Of BluetoothGatt)

		' Token: 0x14000004 RID: 4
		' (add) Token: 0x06000084 RID: 132 RVA: 0x00004714 File Offset: 0x00002914
		' (remove) Token: 0x06000085 RID: 133 RVA: 0x0000474C File Offset: 0x0000294C
		Public Event ServicesDiscovered As EventHandler(Of BluetoothGatt)

		' Token: 0x14000005 RID: 5
		' (add) Token: 0x06000086 RID: 134 RVA: 0x00004784 File Offset: 0x00002984
		' (remove) Token: 0x06000087 RID: 135 RVA: 0x000047BC File Offset: 0x000029BC
		Public Event CharacteristicChanged As EventHandler(Of BluetoothGattCharacteristic)

		' Token: 0x06000088 RID: 136 RVA: 0x000047F1 File Offset: 0x000029F1
		Public Sub New(activity As Activity)
			Me._activity = activity
		End Sub

		' Token: 0x1700000F RID: 15
		' (get) Token: 0x06000089 RID: 137 RVA: 0x0000482D File Offset: 0x00002A2D
		Public ReadOnly Property IsOpen As Boolean
			Get
				Return Me._isOpen
			End Get
		End Property

		' Token: 0x17000010 RID: 16
		' (get) Token: 0x0600008A RID: 138 RVA: 0x00004835 File Offset: 0x00002A35
		Public ReadOnly Property IsSupported As Boolean
			Get
				Return Me._adapter Is Nothing OrElse Me._adapter.IsEnabled
			End Get
		End Property

		' Token: 0x17000011 RID: 17
		' (get) Token: 0x0600008B RID: 139 RVA: 0x0000484C File Offset: 0x00002A4C
		' (set) Token: 0x0600008C RID: 140 RVA: 0x00004854 File Offset: 0x00002A54
		Public Property Selected As String

		' Token: 0x0600008D RID: 141 RVA: 0x0000485D File Offset: 0x00002A5D
		Public Sub Open()
			Me._manager = CType(Me._activity.GetSystemService("bluetooth"), BluetoothManager)
			Me._adapter = Me._manager.Adapter
			Me._isOpen = True
		End Sub

		' Token: 0x0600008E RID: 142 RVA: 0x00004894 File Offset: 0x00002A94
		Public Sub Close()
			Me._isOpen = False
			Me.StopScan(True)
			Try
				For Each bluetoothDevice As BluetoothDevice In Me._devices
					Try
						bluetoothDevice.Dispose()
					Catch ex As Global.System.Exception
					End Try
				Next
			Catch ex2 As Global.System.Exception
			End Try
			Try
				For Each bluetoothLeGattCallback As BluetoothLeGattCallback In Me._callbacks
					Try
						bluetoothLeGattCallback.Dispose()
					Catch ex3 As Global.System.Exception
					End Try
				Next
			Catch ex4 As Global.System.Exception
			End Try
			Try
				If Me._adapter IsNot Nothing Then
					Me._adapter.CancelDiscovery()
					Me._adapter.Dispose()
					Me._adapter = Nothing
				End If
			Catch ex5 As Global.System.Exception
			End Try
			Try
				If Me._manager IsNot Nothing Then
					Me._manager.Dispose()
					Me._manager = Nothing
				End If
			Catch ex6 As Global.System.Exception
			End Try
			Me._scanned.Clear()
			Me._devices.Clear()
			Me._callbacks.Clear()
		End Sub

		' Token: 0x0600008F RID: 143 RVA: 0x00004A00 File Offset: 0x00002C00
		Public Sub StartScan()
			If Me._isOpen AndAlso Me._adapter IsNot Nothing AndAlso Not Me._scanning Then
				Me._scanning = True
				Me._adapter.StartLeScan(Me)
				Me._event.Reset()
				If Me._scanThread Is Nothing Then
					Me._scanThread = AddressOf Me.ScanTimeoutThread
					Me._scanThread.Start()
				End If
			End If
		End Sub

		' Token: 0x06000090 RID: 144 RVA: 0x00004A70 File Offset: 0x00002C70
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
			Dim adapter2 As BluetoothAdapter = Me._adapter
			If adapter2 IsNot Nothing Then
				adapter2.CancelDiscovery()
			End If
			Me._scanning = False
		End Sub

		' Token: 0x06000091 RID: 145 RVA: 0x00004AE4 File Offset: 0x00002CE4
		Private Sub ScanTimeoutThread()
			If Not Me._event.WaitOne(5000) Then
				Me.StopScan(False)
			End If
		End Sub

		' Token: 0x06000092 RID: 146 RVA: 0x00004B00 File Offset: 0x00002D00
		Public Sub OnLeScan(device As BluetoothDevice, rssi As Integer, scanRecord As Byte()) Implements Android.Bluetooth.BluetoothAdapter.ILeScanCallback.OnLeScan
			If device IsNot Nothing Then
				Dim flag As Boolean = False
				If Me._isOpen Then
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
				End If
				If flag Then
					If String.IsNullOrEmpty(Me.Selected) OrElse String.Equals(Me.Selected, device.Name, StringComparison.OrdinalIgnoreCase) Then
						Dim bluetoothLeGattCallback As BluetoothLeGattCallback = New BluetoothLeGattCallback()
						AddHandler bluetoothLeGattCallback.Connected, AddressOf Me.OnGattConnected
						AddHandler bluetoothLeGattCallback.Disconnected, AddressOf Me.OnGattDisconnected
						AddHandler bluetoothLeGattCallback.ServicesDiscovered, AddressOf Me.OnGattServicesDiscovered
						AddHandler bluetoothLeGattCallback.CharacteristicChanged, AddressOf Me.OnGattCharacteristicChanged
						Try
							Me._callbacks.Add(bluetoothLeGattCallback)
						Catch ex As Global.System.Exception
						End Try
						Try
							Me._devices.Add(device)
						Catch ex2 As Global.System.Exception
						End Try
						device.ConnectGatt(Me._activity, False, bluetoothLeGattCallback)
						Return
					End If
				Else
					device.Dispose()
					device = Nothing
				End If
			End If
		End Sub

		' Token: 0x06000093 RID: 147 RVA: 0x00004C4C File Offset: 0x00002E4C
		Private Sub OnGattConnected(sender As Object, gatt As BluetoothGatt)
			If Me._isOpen AndAlso Me.Connected IsNot Nothing Then
				Me.Connected(Me, gatt)
			End If
		End Sub

		' Token: 0x06000094 RID: 148 RVA: 0x00004C6C File Offset: 0x00002E6C
		Private Sub OnGattDisconnected(sender As Object, gatt As BluetoothGatt)
			Dim scanned As List(Of String) = Me._scanned
			SyncLock scanned
				Me._scanned.Clear()
			End SyncLock
			If Me._isOpen AndAlso Me.Disconnected IsNot Nothing Then
				Me.Disconnected(Me, gatt)
			End If
		End Sub

		' Token: 0x06000095 RID: 149 RVA: 0x00004CD0 File Offset: 0x00002ED0
		Private Sub OnGattServicesDiscovered(sender As Object, gatt As BluetoothGatt)
			If Me._isOpen AndAlso Me.ServicesDiscovered IsNot Nothing Then
				Me.ServicesDiscovered(Me, gatt)
			End If
		End Sub

		' Token: 0x06000096 RID: 150 RVA: 0x00004CEF File Offset: 0x00002EEF
		Private Sub OnGattCharacteristicChanged(sender As Object, characteristic As BluetoothGattCharacteristic)
			If Me._isOpen AndAlso Me.CharacteristicChanged IsNot Nothing Then
				Me.CharacteristicChanged(Me, characteristic)
			End If
		End Sub

		' Token: 0x04000032 RID: 50
		Private Const MaxScanDuration As Integer = 5000

		' Token: 0x04000038 RID: 56
		Private _activity As Activity

		' Token: 0x04000039 RID: 57
		Private _adapter As BluetoothAdapter

		' Token: 0x0400003A RID: 58
		Private _manager As BluetoothManager

		' Token: 0x0400003B RID: 59
		Private _scanned As List(Of String) = New List(Of String)()

		' Token: 0x0400003C RID: 60
		Private _devices As List(Of BluetoothDevice) = New List(Of BluetoothDevice)()

		' Token: 0x0400003D RID: 61
		Private _callbacks As List(Of BluetoothLeGattCallback) = New List(Of BluetoothLeGattCallback)()

		' Token: 0x0400003E RID: 62
		Private _isOpen As Boolean

		' Token: 0x0400003F RID: 63
		Private _scanning As Boolean

		' Token: 0x04000040 RID: 64
		Private _scanThread As Global.System.Threading.Thread

		' Token: 0x04000041 RID: 65
		Private _event As ManualResetEvent = New ManualResetEvent(False)
	End Class
End Namespace
