Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports Android.Content
Imports Android.Content.PM
Imports Android.OS
Imports AndroidX.AppCompat.App
Imports AndroidX.Core.Content
Imports AndroidX.Fragment.App
Imports Java.Util.Concurrent.Atomic

Namespace SmartDCP.Android.Platform
	' Token: 0x02000014 RID: 20
	Public Module PermissionsHelper
		' Token: 0x060000BA RID: 186 RVA: 0x000054DC File Offset: 0x000036DC
		Shared Sub New()
			If Build.VERSION.SdkInt < BuildVersionCodes.S Then
				PermissionsHelper.BluetoothPermissions.Remove("android.permission.BLUETOOTH_CONNECT")
				PermissionsHelper.BluetoothPermissions.Remove("android.permission.BLUETOOTH_SCAN")
				PermissionsHelper.Rationales.Remove("android.permission.BLUETOOTH_CONNECT")
				PermissionsHelper.Rationales.Remove("android.permission.BLUETOOTH_SCAN")
			End If
		End Sub

		' Token: 0x060000BB RID: 187 RVA: 0x000055A2 File Offset: 0x000037A2
		Public Sub Clear()
			PermissionsHelper.InFlight.[Set](False)
		End Sub

		' Token: 0x060000BC RID: 188 RVA: 0x000055B0 File Offset: 0x000037B0
		Public Function CheckBluetoothPermissions(context As Context) As Boolean
			Return PermissionsHelper.BluetoothPermissions.All(Function(p As String) ContextCompat.CheckSelfPermission(context, p) = Permission.Granted)
		End Function

		' Token: 0x060000BD RID: 189 RVA: 0x000055E0 File Offset: 0x000037E0
		Public Function CheckAndRequestBluetoothPermissions(fragment As Fragment) As Boolean
			Dim enumerable As IEnumerable(Of String) = PermissionsHelper.BluetoothPermissions.Where(Function(p As String) fragment.Activity.CheckSelfPermission(p) = Permission.Denied)
			If enumerable.Count() = 0 Then
				Return True
			End If
			If PermissionsHelper.InFlight.[Get]() Then
				Return False
			End If
			PermissionsHelper.InFlight.[Set](True)
			Dim permissionsToGrant As List(Of String) = New List(Of String)()
			Using enumerator As IEnumerator(Of String) = enumerable.GetEnumerator()
				While enumerator.MoveNext()
					Dim p As String = enumerator.Current
					If fragment.ShouldShowRequestPermissionRationale(p) Then
						New AlertDialog.Builder(fragment.Activity).SetTitle("Grant Permission").SetMessage(PermissionsHelper.Rationales(p)).SetPositiveButton(17039370, Sub(_sender As Object, <Nullable(1)> _args As DialogClickEventArgs)
							permissionsToGrant.Add(p)
						End Sub).Create().Show()
					Else
						permissionsToGrant.Add(p)
					End If
				End While
			End Using
			If permissionsToGrant.Count > 0 Then
				fragment.RequestPermissions(permissionsToGrant.ToArray(), 99)
			End If
			Return False
		End Function

		' Token: 0x0400004D RID: 77
		Public Const RequestCodeBluetoothPermissions As Integer = 99

		' Token: 0x0400004E RID: 78
		Private InFlight As AtomicBoolean = New AtomicBoolean(False)

		' Token: 0x0400004F RID: 79
		Public BluetoothPermissions As List(Of String) = New List(Of String)() From { "android.permission.ACCESS_FINE_LOCATION", "android.permission.BLUETOOTH_CONNECT", "android.permission.BLUETOOTH_SCAN" }

		' Token: 0x04000050 RID: 80
		Private Rationales As Dictionary(Of String, String) = New Dictionary(Of String, String)() From { { "android.permission.ACCESS_FINE_LOCATION", "SmartDCP requires location permission to discover BLE devices." }, { "android.permission.BLUETOOTH_CONNECT", "SmartDCP requires Bluetooth Connect permission to connect to devices." }, { "android.permission.BLUETOOTH_SCAN", "SmartDCP requires Bluetooth Scan permission to scan for devices." } }
	End Module
End Namespace
