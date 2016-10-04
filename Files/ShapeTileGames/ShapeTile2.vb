Imports System.Drawing.Drawing2D

'this was the first pass at creating the colored shape class. 
'it uses inheritence to create the four different
'types of tiles. This scheme breaks down when you wish to 
'add functionality to the base class via inheritence,
'as you would need to inherit all subclasses. It also does not 
'work well for games where a tile has to change shape on the fly

Namespace OldTile

    Public MustInherit Class ColoredShape
        Inherits Control
        Implements IComparable

        Public Shared Function CreateByIndex(ByVal iShape As Integer, _
            ByVal iColor As Integer) As ColoredShape

            Dim o As ColoredShape
            Dim oClr As New Color

            Select Case iShape
                Case 0
                    o = New SquareColoredShape
                Case 1
                    o = New CircleColoredShape
                Case 2
                    o = New DiamondColoredShape
                Case 3
                    o = New TriangleColoredShape
                Case Else
                    Throw New Exception("iShape index must be 0-3")
            End Select

            'odd, cannot do 'color.red' here
            Select Case iColor
                Case 0
                    o.Color = oClr.Red
                Case 1
                    o.Color = oClr.Yellow
                Case 2
                    o.Color = oClr.Blue
                Case 3
                    o.Color = oClr.Green
                Case Else
                    Throw New Exception("iColor index must be 0-3")
            End Select

            Return o
        End Function

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
                    Invalidate()
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

        MustOverride ReadOnly Property ShapeWord() As String

        Overrides Function ToString() As String
            Return Me.ColorWord & " " & Me.ShapeWord
        End Function

        'this code draws the background. Override to draw the shape itself
        Protected Overridable Sub Draw(ByVal g As Graphics)

            Dim b As LinearGradientBrush

            Dim iRad As Integer = Width - (Me.Border * 2)
            Dim r As New Rectangle(Me.Border, Me.Border, iRad, iRad)

            b = New LinearGradientBrush(r, Color.White, Color.DarkGray, LinearGradientMode.Vertical)
            g.FillRectangle(b, r)

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

            'two tiles equal if color and class equal
            Return Me.Color.Equals(s.Color) _
                And s.GetType.Equals(Me.GetType)

        End Function

        'there's only one compareto used here, used when sorting tiles by their position in Deductile Reasoning.
        'if we wanted different CompareTos, an inheritance scheme might be more usable, but causes
        'combinatorial class problems
        'example = try inheriting ColoredShape into OrderedColoredShape to implement the CompareTo,
        'you would have to inherit all the subclasses as well.
        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo

            Dim o As ColoredShape = CType(obj, ColoredShape)

            Return Me.Top.CompareTo(o.Top)
        End Function

    End Class

    Public Class SquareColoredShape
        Inherits ColoredShape

        Protected Overrides Sub Draw(ByVal g As Graphics)

            MyBase.Draw(g)      'draw the background
            If Backwards Then Exit Sub

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

        Overrides ReadOnly Property ShapeWord() As String
            Get
                Return "Square"
            End Get
        End Property

    End Class

    Public Class CircleColoredShape
        Inherits ColoredShape

        Protected Overrides Sub Draw(ByVal g As Graphics)

            MyBase.Draw(g)      'draw the background
            If Backwards Then Exit Sub

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

        Overrides ReadOnly Property ShapeWord() As String
            Get
                Return "Circle"
            End Get
        End Property

    End Class

    Public Class DiamondColoredShape
        Inherits ColoredShape

        Protected Overrides Sub Draw(ByVal g As Graphics)

            MyBase.Draw(g)      'draw the background
            If Backwards Then Exit Sub

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

        Overrides ReadOnly Property ShapeWord() As String
            Get
                Return "Diamond"
            End Get
        End Property

    End Class

    Public Class TriangleColoredShape
        Inherits ColoredShape

        Protected Overrides Sub Draw(ByVal g As Graphics)

            MyBase.Draw(g)      'draw the background
            If Backwards Then Exit Sub

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

        Overrides ReadOnly Property ShapeWord() As String
            Get
                Return "Triangle"
            End Get
        End Property

    End Class

End Namespace