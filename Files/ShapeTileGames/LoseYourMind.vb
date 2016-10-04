Imports System.Drawing.Drawing2D
Imports ShapeTileGames.NewTile

Namespace LoseYourMind

    Public Class TileCollection
        Inherits System.Collections.CollectionBase

        Public Sub Add(ByVal o As ColoredShape)
            Me.List.Add(o)
        End Sub

        Public ReadOnly Property Item(ByVal iIndex As Integer) As ColoredShape
            Get
                Return Me.List(iIndex)
            End Get
        End Property

        Public Sub Remove(ByVal o As ColoredShape)
            Me.List.Remove(o)
        End Sub

        'this is a shallow copy
        Public Function Clone() As TileCollection

            Dim r As New TileCollection

            Dim o As ColoredShape

            For Each o In Me.List
                r.Add(o)
            Next

            Return r
        End Function

        Public Overrides Function ToString() As String

            Dim s As String
            Dim o As ColoredShape

            For Each o In Me.List
                s &= o.ToString & ";"
            Next

            Return s.Substring(0, s.Length - 1)
        End Function
    End Class

    'represents one guess in the game.
    'includes the tiles and the comparison
    Public Class TileCollectionGuess
        Inherits TileCollection

        Private FNumShapeCorrect As Integer       'shapes in correct place
        Private FNumColorCorrect As Integer       'colors in correct place
        Private FNumShapeWrongSpot As Integer       'correct shapes in wrong place
        Private FNumColorWrongSpot As Integer       'correct colors in wrong place

        ReadOnly Property NumShapeCorrect() As Integer
            Get
                Return FNumShapeCorrect
            End Get
        End Property

        ReadOnly Property NumShapeWrongSpot() As Integer
            Get
                Return FNumShapeWrongSpot
            End Get
        End Property

        ReadOnly Property NumColorCorrect() As Integer
            Get
                Return FNumColorCorrect
            End Get
        End Property

        ReadOnly Property NumColorWrongSpot() As Integer
            Get
                Return FNumColorWrongSpot
            End Get
        End Property


        Public Sub CheckAgainst(ByVal oSolution As TileCollection)
            CheckAgainstShape(oSolution)
            CheckAgainstColor(oSolution)
        End Sub

        Private Sub CheckAgainstShape(ByVal oSolution As TileCollection)

            Dim oSol As TileCollection = oSolution.Clone
            Dim oMe As TileCollection = Me.Clone
            Dim oCSMe As ColoredShape
            Dim oCSSol As ColoredShape
            Dim i As Integer = 0

            'first count those in same position, and remove as we go
            'can't use 'for each' since removing
            Do While i < oMe.Count
                If oMe.Item(i).ShapeWord.Equals(oSol.Item(i).ShapeWord) Then
                    oMe.Remove(oMe.Item(i))
                    oSol.Remove(oSol.Item(i))
                    FNumShapeCorrect += 1
                Else
                    i = i + 1
                End If
            Loop

            'now count in different position. remove only from solution
            For Each oCSMe In oMe
                For Each oCSSol In oSol
                    If oCSMe.ShapeWord.Equals(oCSSol.ShapeWord) Then
                        FNumShapeWrongSpot += 1
                        oSol.Remove(oCSSol)     'only remove in solution
                        Exit For
                    End If
                Next
            Next

        End Sub

        Private Sub CheckAgainstColor(ByVal oSolution As TileCollection)

            Dim oSol As TileCollection = oSolution.Clone
            Dim oMe As TileCollection = Me.Clone
            Dim oCSMe As ColoredShape
            Dim oCSSol As ColoredShape
            Dim i As Integer = 0

            'first count those in same position, and remove as we go
            'can't use 'for each' since removing
            Do While i < oMe.Count
                If oMe.Item(i).Color.Equals(oSol.Item(i).Color) Then
                    oMe.Remove(oMe.Item(i))
                    oSol.Remove(oSol.Item(i))
                    FNumColorCorrect += 1
                Else
                    i = i + 1
                End If
            Loop

            'now count in different position. remove only from solution
            For Each oCSMe In oMe
                For Each oCSSol In oSol
                    If oCSMe.Color.Equals(oCSSol.Color) Then
                        FNumColorWrongSpot += 1
                        oSol.Remove(oCSSol)     'only remove in solution
                        Exit For
                    End If
                Next
            Next
        End Sub

        Public Overrides Function ToString() As String

            Dim s As String = MyBase.ToString

            s &= "(shape="
            s &= FNumShapeCorrect & ";" & FNumShapeWrongSpot & ") "
            s &= "(color="
            s &= FNumColorCorrect & ";" & FNumColorWrongSpot & ")"

            Return s

        End Function

        Public Function Wins() As Boolean
            Return FNumShapeCorrect = Me.List.Count And _
                FNumColorCorrect = Me.List.Count
        End Function
    End Class

    Public Class GuessHintRenderer
        Inherits Control

        Private FTC As TileCollectionGuess
        Public Sub New(ByVal oTC As TileCollectionGuess)
            MyBase.new()
            FTC = oTC
        End Sub

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            Draw(e.Graphics)
        End Sub

        Private Sub Draw(ByVal g As Graphics)

            Const CSIZE = 8

            Dim iLeft As Integer
            Dim iTop As Integer
            Dim i As Integer

            Dim r As Rectangle

            r = Me.ClientRectangle
            r.Inflate(-1, -1)
            g.DrawRectangle(Pens.White, r)
            g.FillRectangle(Brushes.LightGray, r)

            iTop = 6
            iLeft = 4
            For i = 0 To FTC.NumShapeCorrect - 1
                r = New Rectangle(iLeft, iTop, CSIZE, CSIZE)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.Black, r)
                iLeft += CSIZE + 4
            Next

            For i = 0 To FTC.NumShapeWrongSpot - 1
                r = New Rectangle(iLeft, iTop, CSIZE, CSIZE)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.White, r)
                iLeft += CSIZE + 4
            Next

            iTop += CSIZE + (CSIZE \ 2)
            iLeft = 4
            For i = 0 To FTC.NumColorCorrect - 1
                r = New Rectangle(iLeft, iTop, CSIZE, CSIZE)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.Black, r)
                iLeft += CSIZE + 4
            Next

            For i = 0 To FTC.NumColorWrongSpot - 1
                r = New Rectangle(iLeft, iTop, CSIZE, CSIZE)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.White, r)
                iLeft += CSIZE + 4
            Next

        End Sub

    End Class
End Namespace

