Imports System
Imports Android.Bluetooth

Namespace SmartDCP.Android.Platform
	' Token: 0x02000012 RID: 18
	Public Class BluetoothLeGattCallback
		Inherits BluetoothGattCallback

		' Token: 0x14000006 RID: 6
		' (add) Token: 0x06000097 RID: 151 RVA: 0x00004D10 File Offset: 0x00002F10
		' (remove) Token: 0x06000098 RID: 152 RVA: 0x00004D48 File Offset: 0x00002F48
		Public Event Connected As EventHandler(Of BluetoothGatt)

		' Token: 0x14000007 RID: 7
		' (add) Token: 0x06000099 RID: 153 RVA: 0x00004D80 File Offset: 0x00002F80
		' (remove) Token: 0x0600009A RID: 154 RVA: 0x00004DB8 File Offset: 0x00002FB8
		Public Event Disconnected As EventHandler(Of BluetoothGatt)

		' Token: 0x14000008 RID: 8
		' (add) Token: 0x0600009B RID: 155 RVA: 0x00004DF0 File Offset: 0x00002FF0
		' (remove) Token: 0x0600009C RID: 156 RVA: 0x00004E28 File Offset: 0x00003028
		Public Event ServicesDiscovered As EventHandler(Of BluetoothGatt)

		' Token: 0x14000009 RID: 9
		' (add) Token: 0x0600009D RID: 157 RVA: 0x00004E60 File Offset: 0x00003060
		' (remove) Token: 0x0600009E RID: 158 RVA: 0x00004E98 File Offset: 0x00003098
		Public Event CharacteristicChanged As EventHandler(Of BluetoothGattCharacteristic)

		' Token: 0x0600009F RID: 159 RVA: 0x00004ECD File Offset: 0x000030CD
		Public Overrides Overloads Sub OnCharacteristicChanged(gatt As BluetoothGatt, characteristic As BluetoothGattCharacteristic)
			MyBase.OnCharacteristicChanged(gatt, characteristic)
			Me.OnCharacteristicChanged(Me, characteristic)
		End Sub

		' Token: 0x060000A0 RID: 160 RVA: 0x00004EDF File Offset: 0x000030DF
		Public Overrides Overloads Sub OnServicesDiscovered(gatt As BluetoothGatt, status As GattStatus)
			MyBase.OnServicesDiscovered(gatt, status)
			If status = GattStatus.Success Then
				Me.OnServicesDiscovered(gatt)
			End If
		End Sub

		' Token: 0x060000A1 RID: 161 RVA: 0x00004EF3 File Offset: 0x000030F3
		Public Overrides Sub OnConnectionStateChange(gatt As BluetoothGatt, status As GattStatus, newState As ProfileState)
			MyBase.OnConnectionStateChange(gatt, status, newState)
			Select Case newState
				Case ProfileState.Disconnected
					Me.OnDisconnected(gatt)
					Return
				Case ProfileState.Connecting, ProfileState.Disconnecting
				Case ProfileState.Connected
					Me.OnConnected(gatt)
				Case Else
					Return
			End Select
		End Sub

		' Token: 0x060000A2 RID: 162 RVA: 0x00004F24 File Offset: 0x00003124
		Private Sub OnConnected(gatt As BluetoothGatt)
			If Me.Connected IsNot Nothing Then
				Me.Connected(Me, gatt)
			End If
		End Sub

		' Token: 0x060000A3 RID: 163 RVA: 0x00004F3B File Offset: 0x0000313B
		Private Sub OnDisconnected(gatt As BluetoothGatt)
			If Me.Disconnected IsNot Nothing Then
				Me.Disconnected(Me, gatt)
			End If
		End Sub

		' Token: 0x060000A4 RID: 164 RVA: 0x00004F52 File Offset: 0x00003152
		Private Overloads Sub OnServicesDiscovered(gatt As BluetoothGatt)
			If Me.ServicesDiscovered IsNot Nothing Then
				Me.ServicesDiscovered(Me, gatt)
			End If
		End Sub

		' Token: 0x060000A5 RID: 165 RVA: 0x00004F69 File Offset: 0x00003169
		Private Overloads Sub OnCharacteristicChanged(sender As Object, characteristic As BluetoothGattCharacteristic)
			If Me.CharacteristicChanged IsNot Nothing Then
				Me.CharacteristicChanged(Me, characteristic)
			End If
		End Sub
	End Class
End Namespace
