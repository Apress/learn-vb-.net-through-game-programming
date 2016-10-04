Imports System.Drawing.Text

Public Class GameTimer
    Inherits Control

    Private FTimer As Timer
    Private FStartTime As DateTime
    Private FStartAt As TimeSpan = New TimeSpan(0, 5, 0)
    Private FLastSeconds As Integer = -1           'used to raise event

    Public Event SecondsChanged(ByVal sender As Object, ByVal t As TimeSpan)
    Public Event TimesUp(ByVal sender As Object)

    Property StartAt() As TimeSpan
        Get
            Return FStartAt
        End Get

        Set(ByVal Value As TimeSpan)
            FStartAt = Value
        End Set
    End Property

    Public Sub StartTimer()

        If FTimer Is Nothing Then
            FTimer = New Timer
            FTimer.Interval = 100
            AddHandler FTimer.Tick, AddressOf TimerTick
        End If

        FStartTime = DateTime.Now
        FTimer.Enabled = True

    End Sub

    Public Sub StopTimer()
        If Not FTimer Is Nothing Then
            FTimer.Enabled = False
        End If
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias

        Dim t As TimeSpan

        Dim h As String
        Dim m As String
        Dim s As String
        Dim cTime As String

        'time since started
        t = DateTime.Now.Subtract(FStartTime)
        t = FStartAt.Subtract(t)        'time left
        If t.Milliseconds < 0 Then
            If FTimer.Enabled Then
                StopTimer()
                RaiseEvent TimesUp(Me)
            End If
            t = TimeSpan.Zero           'nothing left
        End If

        h = t.Hours
        h = h.PadLeft(2, "0")

        m = t.Minutes
        m = m.PadLeft(2, "0")

        s = t.Seconds
        s = s.PadLeft(2, "0")

        If h > 0 Then
            cTime = h & ":" & m & ":" & s
        Else
            cTime = m & ":" & s
        End If

        e.Graphics.DrawString(cTime, Me.Font, New SolidBrush(Me.ForeColor), 0, 0)
        MyBase.OnPaint(e)

        If s <> FLastSeconds Then
            RaiseEvent SecondsChanged(Me, t)
            FLastSeconds = s
        End If

        Application.DoEvents()
    End Sub

    Private Sub TimerTick(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Invalidate()
    End Sub

    'used to add or remove time on the clock
    Public Sub AddTime(ByVal t As TimeSpan)
        'to add time, we actually subtract from the start time

        FStartAt = FStartAt.Add(t)

    End Sub

    'hide ability to enable/disable through this property
    Shadows ReadOnly Property Enabled() As Boolean
        Get
            Return FTimer.Enabled
        End Get
    End Property
End Class
