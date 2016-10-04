Imports System.Math

Public Class ClipPictureBox
    Inherits PictureBox

    Private Const TSIZE As Integer = 4

    Private FClipDisabled As Boolean

    Private FClipInitialized As Boolean = False
    Private FClipTop As Integer
    Private FClipBottom As Integer
    Private FClipLeft As Integer
    Private FClipRight As Integer

    Private FDraggingTop As Boolean = False
    Private FDraggingBottom As Boolean = False
    Private FDraggingLeft As Boolean = False
    Private FDraggingRight As Boolean = False

    Private FClipColor As Color = Color.Yellow
    Property ClipColor() As Color
        Get
            Return FClipColor
        End Get
        Set(ByVal Value As Color)
            FClipColor = Value
            Me.Invalidate()
        End Set
    End Property

    Property ClipDisabled() As Boolean
        Get
            Return FClipDisabled
        End Get
        Set(ByVal Value As Boolean)
            FClipDisabled = Value
            If FClipDisabled Then
                FClipTop = 0
                FClipBottom = Me.Height
                FClipLeft = 0
                FClipRight = Me.Width
                Me.Invalidate()
                RaiseEvent ClippingRectChanged(Me.ClippingRect)
            End If
        End Set
    End Property

    Public Event ClippingRectChanged(ByVal r As Rectangle)

    ReadOnly Property ClippingRect() As Rectangle
        Get
            Return New Rectangle(FClipLeft, FClipTop, FClipRight - FClipLeft, FClipBottom - FClipTop)
        End Get
    End Property

    Protected Overrides Sub OnPaint(ByVal pe As System.Windows.Forms.PaintEventArgs)

        MyBase.OnPaint(pe)

        Call DrawYMarker(pe.Graphics, FClipTop)
        Call DrawYMarker(pe.Graphics, FClipBottom)

        Call DrawXMarker(pe.Graphics, FClipLeft)
        Call DrawXMarker(pe.Graphics, FClipRight)
    End Sub

    Private Sub DrawYMarker(ByVal g As Graphics, ByVal y As Integer)

        Dim pt(2) As Point
        Dim b As New SolidBrush(ClipColor)

        pt(0) = New Point
        With pt(0)
            .X = 0
            .Y = y - TSIZE
        End With

        pt(1) = New Point
        With pt(1)
            .X = TSIZE
            .Y = y
        End With

        pt(2) = New Point
        With pt(2)
            .X = 0
            .Y = y + TSIZE
        End With

        Call g.FillPolygon(b, pt)

        Dim p As New Pen(ClipColor)
        p.DashStyle = Drawing.Drawing2D.DashStyle.Dot
        g.DrawLine(p, 0, y, Width, y)

    End Sub
    Private Sub DrawXMarker(ByVal g As Graphics, ByVal x As Integer)

        Dim pt(2) As Point
        Dim b As New SolidBrush(ClipColor)

        pt(0) = New Point
        With pt(0)
            .X = x - TSIZE
            .Y = 0
        End With

        pt(1) = New Point
        With pt(1)
            .X = x
            .Y = TSIZE
        End With

        pt(2) = New Point
        With pt(2)
            .X = x + TSIZE
            .Y = 0
        End With

        Call g.FillPolygon(b, pt)
        Dim p As New Pen(ClipColor)
        p.DashStyle = Drawing.Drawing2D.DashStyle.Dot
        g.DrawLine(p, x, 0, x, Height)

    End Sub

    Shadows Property Image() As Image
        Get
            Return MyBase.Image
        End Get
        Set(ByVal Value As Image)
            MyBase.Image = Value

            If Not FClipInitialized Then
                FClipTop = Height \ 4
                FClipBottom = (Height \ 4) * 3

                FClipLeft = Width \ 4
                FClipRight = (Width \ 4) * 3
                FClipInitialized = True
            End If
        End Set
    End Property


    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        If e.Button = MouseButtons.Left Then
            If e.Y <= TSIZE Then
                If Abs(e.X - FClipLeft) <= TSIZE Then
                    FDraggingLeft = True
                    Exit Sub
                End If
                If Abs(e.X - FClipRight) <= TSIZE Then
                    FDraggingRight = True
                    Exit Sub
                End If
            End If

            If e.X <= TSIZE Then
                If Abs(e.Y - FClipTop) <= TSIZE Then
                    FDraggingTop = True
                    Exit Sub
                End If
                If Abs(e.Y - FClipBottom) <= TSIZE Then
                    FDraggingBottom = True
                    Exit Sub
                End If
            End If
        End If
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)

        If FDraggingTop Then
            If e.Y >= 0 And e.Y <= Me.Height \ 2 Then
                FClipTop = e.Y
                Me.Invalidate()
                RaiseEvent ClippingRectChanged(Me.ClippingRect)
            End If
        End If

        If FDraggingBottom Then
            If e.Y > Me.Height \ 2 And e.Y < Me.Height Then
                FClipBottom = e.Y
                Me.Invalidate()
                RaiseEvent ClippingRectChanged(Me.ClippingRect)
            End If
        End If

        If FDraggingLeft Then
            If e.X >= 0 And e.X <= Me.Width \ 2 Then
                FClipLeft = e.X
                Me.Invalidate()
                RaiseEvent ClippingRectChanged(Me.ClippingRect)
            End If
        End If

        If FDraggingRight Then
            If e.X > Me.Width \ 2 And e.X < Me.Width Then
                FClipRight = e.X
                Me.Invalidate()
                RaiseEvent ClippingRectChanged(Me.ClippingRect)
            End If
        End If

        MyBase.OnMouseMove(e)

    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        FDraggingTop = False
        FDraggingBottom = False
        FDraggingLeft = False
        FDraggingRight = False

        MyBase.OnMouseUp(e)
    End Sub
End Class
