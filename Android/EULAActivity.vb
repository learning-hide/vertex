Imports System
Imports System.Runtime.CompilerServices
Imports Android.App
Imports Android.Content
Imports Android.Content.PM
Imports Android.OS
Imports Android.Preferences
Imports Android.Widget

Namespace SmartDCP.Android
	' Token: 0x02000006 RID: 6
	<Activity(Label := "Smart DCP", NoHistory := True, Theme := "@style/MyTheme")>
	Public Class EULAActivity
		Inherits Activity

		' Token: 0x06000015 RID: 21 RVA: 0x00002598 File Offset: 0x00000798
		Protected Overrides Sub OnCreate(bundle As Bundle)
			MyBase.OnCreate(bundle)
			Me.RequestedOrientation = ScreenOrientation.Portrait
			Me.SetContentView(2131492895)
			AddHandler MyBase.FindViewById(Of Button)(2131296321).Click, Sub(s As Object, e As EventArgs)
				Dim defaultSharedPreferences As ISharedPreferences = PreferenceManager.GetDefaultSharedPreferences(MyBase.Application)
				If defaultSharedPreferences IsNot Nothing Then
					Dim sharedPreferencesEditor As ISharedPreferencesEditor = defaultSharedPreferences.Edit()
					sharedPreferencesEditor.PutBoolean("EULAAccepted", True)
					sharedPreferencesEditor.Apply()
				End If
				MyBase.StartActivity(GetType(MainActivity))
			End Sub
			AddHandler MyBase.FindViewById(Of Button)(2131296322).Click, Sub(s As Object, e As EventArgs)
				Dim builder As AlertDialog.Builder = New AlertDialog.Builder(Me).SetTitle("Smart DCP").SetMessage("Are you sure you want to exit?")
				Dim text As String = "Yes"
				Dim <>9__0_ As EventHandler(Of DialogClickEventArgs) = EULAActivity.<>c.<>9__0_2
				Dim eventHandler As EventHandler(Of DialogClickEventArgs) = <>9__0_
				If <>9__0_ Is Nothing Then
					Dim eventHandler2 As EventHandler(Of DialogClickEventArgs) = Sub(s2 As Object, <Nullable(1)> e2 As DialogClickEventArgs)
						Process.KillProcess(Process.MyPid())
					End Sub
					eventHandler = eventHandler2
					EULAActivity.<>c.<>9__0_2 = eventHandler2
				End If
				Me._alertDialog = builder.SetPositiveButton(text, eventHandler).SetNegativeButton("No", Sub(s2 As Object, <Nullable(1)> e2 As DialogClickEventArgs)
					Me._alertDialog.Dismiss()
					Me._alertDialog = Nothing
				End Sub).Create()
				Me._alertDialog.Show()
			End Sub
		End Sub

		' Token: 0x04000004 RID: 4
		Private _alertDialog As AlertDialog
	End Class
End Namespace
