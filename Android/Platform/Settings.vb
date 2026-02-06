Imports System
Imports Android.App
Imports Android.Content
Imports Android.Preferences
Imports SmartDCP.[Shared]

Namespace SmartDCP.Android.Platform
	' Token: 0x02000015 RID: 21
	Friend Class Settings
		Implements ISettings

		' Token: 0x060000BE RID: 190 RVA: 0x00005738 File Offset: 0x00003938
		Public Sub New(activity As Activity)
			Me._preferences = PreferenceManager.GetDefaultSharedPreferences(activity)
		End Sub

		' Token: 0x17000016 RID: 22
		' (get) Token: 0x060000BF RID: 191 RVA: 0x0000574C File Offset: 0x0000394C
		Public ReadOnly Property SampleRate As Double Implements SmartDCP.[Shared].ISettings.SampleRate
			Get
				Return 3.0
			End Get
		End Property

		' Token: 0x17000017 RID: 23
		' (get) Token: 0x060000C0 RID: 192 RVA: 0x00005757 File Offset: 0x00003957
		Public ReadOnly Property SampleSettle As Double Implements SmartDCP.[Shared].ISettings.SampleSettle
			Get
				Return 1500.0
			End Get
		End Property

		' Token: 0x17000018 RID: 24
		' (get) Token: 0x060000C1 RID: 193 RVA: 0x00005762 File Offset: 0x00003962
		Public ReadOnly Property SampleThreshold As Double Implements SmartDCP.[Shared].ISettings.SampleThreshold
			Get
				Return 4.0
			End Get
		End Property

		' Token: 0x17000019 RID: 25
		' (get) Token: 0x060000C2 RID: 194 RVA: 0x0000576D File Offset: 0x0000396D
		' (set) Token: 0x060000C3 RID: 195 RVA: 0x00005785 File Offset: 0x00003985
		Public Property DefaultWeight As Weight Implements SmartDCP.[Shared].ISettings.DefaultWeight
			Get
				If Me._preferences.GetInt("DefaultWeight", 0) <> 0 Then
					Return Weight.Small101Lbs
				End If
				Return Weight.Large176Lbs
			End Get
			Set(value As Weight)
				Dim sharedPreferencesEditor As ISharedPreferencesEditor = Me._preferences.Edit()
				sharedPreferencesEditor.PutInt("DefaultWeight", CInt(value))
				sharedPreferencesEditor.Apply()
			End Set
		End Property

		' Token: 0x1700001A RID: 26
		' (get) Token: 0x060000C4 RID: 196 RVA: 0x000057A4 File Offset: 0x000039A4
		' (set) Token: 0x060000C5 RID: 197 RVA: 0x000057BB File Offset: 0x000039BB
		Public Property RangefinderName As String Implements SmartDCP.[Shared].ISettings.RangefinderName
			Get
				Return Me._preferences.GetString("RangefinderName", String.Empty)
			End Get
			Set(value As String)
				Dim sharedPreferencesEditor As ISharedPreferencesEditor = Me._preferences.Edit()
				sharedPreferencesEditor.PutString("RangefinderName", If(value, String.Empty))
				sharedPreferencesEditor.Apply()
			End Set
		End Property

		' Token: 0x1700001B RID: 27
		' (get) Token: 0x060000C6 RID: 198 RVA: 0x000057E3 File Offset: 0x000039E3
		' (set) Token: 0x060000C7 RID: 199 RVA: 0x000057E3 File Offset: 0x000039E3
		Public Property IsEULAAccepted As Boolean Implements SmartDCP.[Shared].ISettings.IsEULAAccepted
			Get
				Throw New NotImplementedException()
			End Get
			Set(value As Boolean)
				Throw New NotImplementedException()
			End Set
		End Property

		' Token: 0x04000051 RID: 81
		Private _preferences As ISharedPreferences
	End Class
End Namespace
