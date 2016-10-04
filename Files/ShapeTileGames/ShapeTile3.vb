Imports System.Drawing.Drawing2D

Namespace NewTile

    Public Enum ShapeType
        stSquare
        stCircle
        stDiamond
        stTriangle
    End Enum

    Public Class ColoredShape
        Inherits Control
        Implements IComparable

        Private FShape As ShapeType
        Property Shape() As ShapeType
            Get
                Return FShape
            End Get
            Set(ByVal Value As ShapeType)
                FShape = Value
                Invalidate()
            End Set
        End Property

        Public Sub ToggleShape()

            Select Case Shape
                Case ShapeType.stCircle
                    Shape = ShapeType.stDiamond
                Case ShapeType.stDiamond
                    Shape = ShapeType.stSquare
                Case ShapeType.stSquare
                    Shape = ShapeType.stTriangle
                Case ShapeType.stTriangle
                    Shape = ShapeType.stCircle
            End Select
        End Sub

        Public Sub ToggleColor()

            'can't use a case statement for colors
            If Me.Color.Equals(Color.Red) Then
                Me.Color = Color.Yellow
            ElseIf Me.Color.Equals(Color.Yellow) Then
                Me.Color = Color.Green
            ElseIf Me.Color.Equals(Color.Green) Then
                Me.Color = Color.Blue
            ElseIf Me.Color.Equals(Color.Blue) Then
                Me.Color = Color.Red
            End If

        End Sub

        Public Sub CopyFrom(ByVal c As ColoredShape)
            Me.Color = c.Color
            Me.Shape = c.Shape
        End Sub

        Public Sub New(ByVal iShape As Integer, ByVal iColor As Integer)

            MyBase.new()
            Select Case iShape
                Case 0
                    Me.Shape = ShapeType.stSquare
                Case 1
                    Me.Shape = ShapeType.stCircle
                Case 2
                    Me.Shape = ShapeType.stDiamond
                Case 3
                    Me.Shape = ShapeType.stTriangle
                Case Else
                    Throw New Exception("iShape index must be 0-3")
            End Select

            'odd, cannot do 'color.red' here
            Select Case iColor
                Case 0
                    Me.Color = Color.Red
                Case 1
                    Me.Color = Color.Yellow
                Case 2
                    Me.Color = Color.Blue
                Case 3
                    Me.Color = Color.Green
                Case Else
                    Throw New Exception("iColor index must be 0-3")
            End Select

        End Sub

        Private FBackwards As Boolean
        Property Backwards() As Boolean
            Get
                Return FBackwards
            End Get
            Set(ByVal Value As Boolean)
                FBackwards = Value
                Invalidate()
            End Set
        End Property

        Private FBorder As Integer = 2
        Property Border() As Integer
            Get
                Return FBorder
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Throw New Exception("Illegal value")
                Else
                    FBorder = Value
                    Invalidate()
                End If
            End Set
        End Property

        Private FColor As Color = Color.Red
        Property Color() As Color
            Get
                Return FColor
            End Get
            Set(ByVal Value As Color)

                Dim aC() As Color = {Color.Red, Color.Blue, Color.Green, Color.Yellow}

                If Array.IndexOf(aC, Value) = -1 Then
                    Throw New Exception("colors constrained to Red/Blue/Green/Yellow")
                Else
                    FColor = Value
                    Me.Invalidate()
                End If
            End Set
        End Property

        ReadOnly Property ColorWord() As String

            Get
                Dim s As String

                If Color.Equals(Color.Red) Then
                    s = "Red"
                ElseIf Color.Equals(Color.Blue) Then
                    s = "Blue"
                ElseIf Color.Equals(Color.Green) Then
                    s = "Green"
                ElseIf Color.Equals(Color.Yellow) Then
                    s = "Yellow"
                Else
                    'won't happen, but just in case
                    s = Color.ToString
                End If

                Return s
            End Get
        End Property

        ReadOnly Property ShapeWord() As String
            Get
                Dim s As String

                Select Case FShape
                    Case ShapeType.stCircle
                        s = "Circle"
                    Case ShapeType.stDiamond
                        s = "Diamond"
                    Case ShapeType.stSquare
                        s = "Square"
                    Case ShapeType.stTriangle
                        s = "Triangle"
                End Select
                Return s
            End Get
        End Property

        Overrides Function ToString() As String
            Return Me.ColorWord & " " & Me.ShapeWord
        End Function

        'this code draws the tile.
        Public Overridable Sub Draw(ByVal g As Graphics)

            Dim b As LinearGradientBrush

            Dim iRad As Integer = Width - (Me.Border * 2)
            Dim r As New Rectangle(Me.Border, Me.Border, iRad, iRad)

            b = New LinearGradientBrush(r, Color.White, Color.DarkGray, LinearGradientMode.Vertical)
            g.FillRectangle(b, r)
            g.DrawRectangle(Pens.White, r)
            If Backwards Then Exit Sub

            Select Case FShape
                Case ShapeType.stCircle
                    DrawCircle(g)
                Case ShapeType.stDiamond
                    DrawDiamond(g)
                Case ShapeType.stSquare
                    DrawSquare(g)
                Case ShapeType.stTriangle
                    DrawTriangle(g)
            End Select

        End Sub


        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            MyBase.OnPaint(e)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            Draw(e.Graphics)
        End Sub

        'cover the real width and height to force the shape to be square
        Shadows Property Width() As Integer
            Get
                Return MyBase.Width
            End Get

            Set(ByVal Value As Integer)
                MyBase.Width = Value
                MyBase.Height = Value
            End Set
        End Property

        Shadows Property Height() As Integer
            Get
                Return MyBase.Height
            End Get
            Set(ByVal Value As Integer)
                MyBase.Width = Value
                MyBase.Height = Value
            End Set
        End Property

        Public Overloads Function Equals(ByVal s As ColoredShape) As Boolean

            'two tiles equal if color and shape equal
            Return Me.Color.Equals(s.Color) _
                And s.Shape.Equals(Me.Shape)

        End Function

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo

            Dim o As ColoredShape = CType(obj, ColoredShape)

            Return Me.Top.CompareTo(o.Top)
        End Function

        Private Sub DrawSquare(ByVal g As Graphics)

            Dim b As New SolidBrush(Me.Color)
            Dim iRad As Integer = (Width - Me.Border) \ 2
            Dim ogc As GraphicsContainer
            Dim r As New Rectangle(-iRad \ 2, -iRad \ 2, iRad, iRad)

            ogc = g.BeginContainer
            Try
                With g
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .TranslateTransform(Width \ 2, Width \ 2)
                    .FillRectangle(b, r)
                    .DrawRectangle(Pens.Black, r)
                End With
            Finally
                g.EndContainer(ogc)
            End Try

        End Sub

        Private Sub DrawCircle(ByVal g As Graphics)

            Dim b As New SolidBrush(Me.Color)
            Dim iRad As Integer = (Width - Me.Border) \ 2
            Dim ogc As GraphicsContainer
            Dim r As New Rectangle(-iRad \ 2, -iRad \ 2, iRad, iRad)

            ogc = g.BeginContainer
            Try
                With g
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .TranslateTransform(Width \ 2, Width \ 2)
                    .FillEllipse(b, r)
                    .DrawEllipse(Pens.Black, r)
                End With
            Finally
                g.EndContainer(ogc)
            End Try

        End Sub

        Private Sub DrawDiamond(ByVal g As Graphics)

            Dim b As New SolidBrush(Me.Color)
            Dim iRad As Integer = (Width - Me.Border) \ 2
            Dim ogc As GraphicsContainer

            Dim r As New Rectangle(-iRad \ 2, -iRad \ 2, iRad, iRad)

            ogc = g.BeginContainer
            Try
                With g
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .TranslateTransform(Width \ 2, Width \ 2)
                    .RotateTransform(45)
                    .ScaleTransform(0.8, 0.8)
                    .FillRectangle(b, r)
                    .DrawRectangle(Pens.Black, r)
                End With
            Finally
                g.EndContainer(ogc)
            End Try

        End Sub


        Private Sub DrawTriangle(ByVal g As Graphics)

            Dim b As New SolidBrush(Me.Color)
            Dim iRad As Integer = (Width - Me.Border) \ 2
            Dim ogc As GraphicsContainer

            Dim pt(2) As Point
            pt(0) = New Point(-iRad \ 2, iRad \ 2)
            pt(1) = New Point(0, -iRad \ 2)
            pt(2) = New Point(iRad \ 2, iRad \ 2)
            Dim gp As New GraphicsPath
            gp.AddPolygon(pt)

            ogc = g.BeginContainer
            Try
                With g
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .TranslateTransform(Width \ 2, Width \ 2)
                    .FillPath(b, gp)
                    .DrawPath(Pens.Black, gp)
                End With
            Finally
                g.EndContainer(ogc)
            End Try

        End Sub

    End Class

End Namespace