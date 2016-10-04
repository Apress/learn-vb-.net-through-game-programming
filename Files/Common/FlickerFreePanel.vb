Imports System.Windows.Forms

Public Class FlickerFreePanel
    Inherits System.Windows.Forms.Panel

    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        Me.BackColor = Color.Black

    End Sub

End Class
