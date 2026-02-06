Imports System
Imports Android.App
Imports Android.Content
Imports Android.Content.PM
Imports Android.OS
Imports Android.Preferences

Namespace SmartDCP.Android
	' Token: 0x02000007 RID: 7
	<Activity(Label := "Smart DCP", Theme := "@style/Theme.Splash", MainLauncher := True, NoHistory := True)>
	Public Class SplashActivity
		Inherits Activity

		' Token: 0x0600001A RID: 26 RVA: 0x000026D4 File Offset: 0x000008D4
		Protected Overrides Sub OnCreate(bundle As Bundle)
			MyBase.OnCreate(bundle)
			Me.RequestedOrientation = ScreenOrientation.Portrait
			Me.SetContentView(2131492892)
			Dim flag As Boolean = False
			Dim defaultSharedPreferences As ISharedPreferences = PreferenceManager.GetDefaultSharedPreferences(MyBase.Application)
			If defaultSharedPreferences IsNot Nothing AndAlso defaultSharedPreferences.GetBoolean("EULAAccepted", False) Then
				flag = True
			End If
			If flag Then
				MyBase.StartActivity(GetType(MainActivity))
				Return
			End If
			MyBase.StartActivity(GetType(EULAActivity))
		End Sub
	End Class
End Namespace
