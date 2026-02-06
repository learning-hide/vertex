Imports System
Imports System.Collections.Generic
Imports Android.App
Imports Android.Bluetooth
Imports Java.Util
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.Platform
	' Token: 0x02000013 RID: 19
	Public Class LeicaDistro
		Implements IRangefinder

		' Token: 0x1400000A RID: 10
		' (add) Token: 0x060000A7 RID: 167 RVA: 0x00004F88 File Offset: 0x00003188
		' (remove) Token: 0x060000A8 RID: 168 RVA: 0x00004FC0 File Offset: 0x000031C0
		Public Event MeasurementChanged As EventHandler Implements SmartDCP.[Shared].IRangefinder.MeasurementChanged

		' Token: 0x060000A9 RID: 169 RVA: 0x00004FF5 File Offset: 0x000031F5
		Public Sub New(activity As Activity)
			Me._activity = activity
		End Sub

		' Token: 0x17000012 RID: 18
		' (get) Token: 0x060000AA RID: 170 RVA: 0x0000500F File Offset: 0x0000320F
		Public ReadOnly Property IsOpen As Boolean Implements SmartDCP.[Shared].IRangefinder.IsOpen
			Get
				Return Me._bluetooth IsNot Nothing AndAlso Me._bluetooth.IsOpen
			End Get
		End Property

		' Token: 0x060000AB RID: 171 RVA: 0x00005028 File Offset: 0x00003228
		Public Sub Open(selected As String) Implements SmartDCP.[Shared].IRangefinder.Open
			Me._bluetooth = New BluetoothLeManager(Me._activity)
			AddHandler Me._bluetooth.Connected, AddressOf Me.OnDeviceConnected
			AddHandler Me._bluetooth.Disconnected, AddressOf Me.OnDeviceDisconnected
			AddHandler Me._bluetooth.ServicesDiscovered, AddressOf Me.OnServicesDiscovered
			AddHandler Me._bluetooth.CharacteristicChanged, AddressOf Me.OnCharacteristicChanged
			Me._bluetooth.Selected = selected
			Me._bluetooth.Open()
		End Sub

		' Token: 0x060000AC RID: 172 RVA: 0x000050BC File Offset: 0x000032BC
		Public Sub Close() Implements SmartDCP.[Shared].IRangefinder.Close
			Try
				Me._devices.Clear()
				If Me._device IsNot Nothing Then
					Try
						Me._device.Disconnect()
					Catch ex As Exception
					End Try
					Try
						Me._device.Close()
					Catch ex2 As Exception
					End Try
					Try
						Me._device.Dispose()
					Catch ex3 As Exception
					End Try
					Me._device = Nothing
				End If
				Try
					If Me._service IsNot Nothing Then
						Me._service.Dispose()
						Me._service = Nothing
					End If
				Catch ex4 As Exception
				End Try
				Me._measurement = 0F
			Catch ex5 As Exception
			End Try
			Try
				Me._bluetooth.Close()
				Me._bluetooth = Nothing
			Catch ex6 As Exception
			End Try
		End Sub

		' Token: 0x17000013 RID: 19
		' (get) Token: 0x060000AD RID: 173 RVA: 0x000051AC File Offset: 0x000033AC
		Public ReadOnly Property IsSupported As Boolean Implements SmartDCP.[Shared].IRangefinder.IsSupported
			Get
				Return Me._bluetooth IsNot Nothing AndAlso Me._bluetooth.IsSupported
			End Get
		End Property

		' Token: 0x17000014 RID: 20
		' (get) Token: 0x060000AE RID: 174 RVA: 0x000051C3 File Offset: 0x000033C3
		Public ReadOnly Property IsConnected As Boolean Implements SmartDCP.[Shared].IRangefinder.IsConnected
			Get
				Return Me._device IsNot Nothing AndAlso Me._service IsNot Nothing
			End Get
		End Property

		' Token: 0x17000015 RID: 21
		' (get) Token: 0x060000AF RID: 175 RVA: 0x000051D8 File Offset: 0x000033D8
		' (set) Token: 0x060000B0 RID: 176 RVA: 0x000051E0 File Offset: 0x000033E0
		Public Property Measurement As Single Implements SmartDCP.[Shared].IRangefinder.Measurement
			Get
				Return Me._measurement
			End Get
			Private Set(value As Single)
				Me._measurement = value
				Me.OnMeasurementChanged()
			End Set
		End Property

		' Token: 0x060000B1 RID: 177 RVA: 0x000051F0 File Offset: 0x000033F0
		Public Sub Measure() Implements SmartDCP.[Shared].IRangefinder.Measure
			If Me._device IsNot Nothing AndAlso Me._service IsNot Nothing Then
				Dim characteristic As BluetoothGattCharacteristic = Me._service.GetCharacteristic(UUID.FromString("3ab10109-f831-4395-b29d-570977d5bf94"))
				characteristic.SetValue(New Byte() { 103 })
				Me._device.WriteCharacteristic(characteristic)
				characteristic.Dispose()
			End If
		End Sub

		' Token: 0x060000B2 RID: 178 RVA: 0x0000524C File Offset: 0x0000344C
		Public Sub LaserOn()
			If Me._device IsNot Nothing AndAlso Me._service IsNot Nothing Then
				Dim characteristic As BluetoothGattCharacteristic = Me._service.GetCharacteristic(UUID.FromString("3ab10109-f831-4395-b29d-570977d5bf94"))
				characteristic.SetValue(New Byte() { 111 })
				Me._device.WriteCharacteristic(characteristic)
				characteristic.Dispose()
			End If
		End Sub

		' Token: 0x060000B3 RID: 179 RVA: 0x000052A8 File Offset: 0x000034A8
		Public Sub LaserOff()
			If Me._device IsNot Nothing AndAlso Me._service IsNot Nothing Then
				Dim characteristic As BluetoothGattCharacteristic = Me._service.GetCharacteristic(UUID.FromString("3ab10109-f831-4395-b29d-570977d5bf94"))
				characteristic.SetValue(New Byte() { 112 })
				Me._device.WriteCharacteristic(characteristic)
				characteristic.Dispose()
			End If
		End Sub

		' Token: 0x060000B4 RID: 180 RVA: 0x00005302 File Offset: 0x00003502
		Public Sub Scan() Implements SmartDCP.[Shared].IRangefinder.Scan
			If Me._bluetooth IsNot Nothing Then
				Me._bluetooth.StartScan()
			End If
		End Sub

		' Token: 0x060000B5 RID: 181 RVA: 0x00005318 File Offset: 0x00003518
		Private Sub OnCharacteristicChanged(sender As Object, characteristic As BluetoothGattCharacteristic)
			Dim value As Byte() = characteristic.GetValue()
			Me.Measurement = BitConverter.ToSingle(value, 0)
		End Sub

		' Token: 0x060000B6 RID: 182 RVA: 0x00005339 File Offset: 0x00003539
		Private Sub OnMeasurementChanged()
			If Me.MeasurementChanged IsNot Nothing Then
				Me.MeasurementChanged(Me, EventArgs.Empty)
			End If
		End Sub

		' Token: 0x060000B7 RID: 183 RVA: 0x00005354 File Offset: 0x00003554
		Private Sub OnServicesDiscovered(sender As Object, gatt As BluetoothGatt)
			Me._device = gatt
			Me._service = gatt.GetService(UUID.FromString("3ab10100-f831-4395-b29d-570977d5bf94"))
			If Me._service IsNot Nothing Then
				Dim characteristic As BluetoothGattCharacteristic = Me._service.GetCharacteristic(UUID.FromString("3ab10101-f831-4395-b29d-570977d5bf94"))
				If characteristic IsNot Nothing Then
					Dim descriptor As BluetoothGattDescriptor = characteristic.GetDescriptor(UUID.FromString("00002902-0000-1000-8000-00805f9b34fb"))
					If descriptor IsNot Nothing Then
						descriptor.SetValue(BitConverter.GetBytes(2US))
						If gatt.WriteDescriptor(descriptor) Then
							gatt.SetCharacteristicNotification(characteristic, True)
						End If
						descriptor.Dispose()
					End If
					characteristic.Dispose()
				End If
			End If
		End Sub

		' Token: 0x060000B8 RID: 184 RVA: 0x000053E4 File Offset: 0x000035E4
		Private Sub OnDeviceConnected(sender As Object, gatt As BluetoothGatt)
			If gatt IsNot Nothing AndAlso gatt.Device IsNot Nothing Then
				Dim address As String = gatt.Device.Address
				Dim flag As Boolean = False
				Dim devices As List(Of String) = Me._devices
				SyncLock devices
					If Not String.IsNullOrEmpty(address) Then
						flag = Not Me._devices.Contains(address)
						If flag Then
							Me._devices.Add(address)
						End If
					End If
				End SyncLock
				If flag Then
					gatt.DiscoverServices()
				End If
			End If
		End Sub

		' Token: 0x060000B9 RID: 185 RVA: 0x00005468 File Offset: 0x00003668
		Private Sub OnDeviceDisconnected(sender As Object, gatt As BluetoothGatt)
			If gatt IsNot Nothing AndAlso gatt.Device IsNot Nothing Then
				Dim address As String = gatt.Device.Address
				Dim devices As List(Of String) = Me._devices
				SyncLock devices
					If Not String.IsNullOrEmpty(address) AndAlso Me._devices.Contains(address) Then
						Me._devices.Remove(address)
					End If
				End SyncLock
			End If
		End Sub

		' Token: 0x04000047 RID: 71
		Private _activity As Activity

		' Token: 0x04000048 RID: 72
		Private _devices As List(Of String) = New List(Of String)()

		' Token: 0x04000049 RID: 73
		Private _bluetooth As BluetoothLeManager

		' Token: 0x0400004A RID: 74
		Private _service As BluetoothGattService

		' Token: 0x0400004B RID: 75
		Private _device As BluetoothGatt

		' Token: 0x0400004C RID: 76
		Private _measurement As Single
	End Class
End Namespace
