Public Class PaintPanel
    Inherits Panel

    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

    End Sub

End Class
