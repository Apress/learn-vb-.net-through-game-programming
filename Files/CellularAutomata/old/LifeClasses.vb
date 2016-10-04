Imports System.Drawing.Drawing2D

Public MustInherit Class CellularAutomataPanel
    Inherits System.Windows.Forms.Panel

    Protected FCells As ArrayList
    Private FRows As Integer
    Private FCols As Integer

    Private oRand As Random

    Property Rows()
        Get
            Return FRows
        End Get
        Set(ByVal Value)
            FRows = Value
        End Set
    End Property


    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        oRand = New Random
        CreateData()
    End Sub

    Function HalfTheTime() As Boolean
        Return oRand.Next(0, Int16.MaxValue) Mod 2 = 0
    End Function

    Private FCellRadius As Integer = 8
    Property CellRadius() As Integer
        Get
            Return FCellRadius
        End Get
        Set(ByVal Value As Integer)
            FCellRadius = Value
            CreateData()
            Me.Invalidate()
        End Set
    End Property

    Private Sub CreateData()

        Dim oC As LifeCell
        Dim iRow As Integer = 0
        Dim iCol As Integer

        FCells = New ArrayList
        Do
            iCol = 0
            Do
                oC = New LifeCell(Me)
                With oC
                    .Position = New Point(iCol * CellRadius, iRow * CellRadius)
                    .Alive = oRand.Next(0, 1000) Mod 5 = 0
                End With
                FCells.Add(oC)

                iCol += 1
            Loop Until iCol * CellRadius > Me.Width
            iRow += 1

        Loop Until (iRow * CellRadius) > Me.Height

        FRows = iRow
        FCols = iCol

        Debug.Assert(FRows * FCols = FCells.Count)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Draw(e.Graphics)
    End Sub

    Protected Overrides Sub OnResize(ByVal eventargs As System.EventArgs)
        MyBase.OnResize(eventargs)
        Call CreateData()
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        MyBase.OnMouseDown(e)

        Dim oC As LifeCell

        For Each oC In FCells
            If oC.ClientRectangle.Contains(e.X, e.Y) Then
                If e.Button = MouseButtons.Left Then
                    oC.Alive = Not oC.Alive
                    Me.Invalidate()
                    Exit Sub
                End If
                If e.Button = MouseButtons.Right Then
                    MsgBox("neighbors:" & oC.Neighbors)
                End If
            End If
        Next

    End Sub


    Private Sub Draw(ByVal g As Graphics)

        Dim oC As LifeCell
        For Each oC In FCells
            oC.Draw(g)
        Next

    End Sub

    Private Sub IndextoRowCol(ByVal i As Integer, ByRef iRow As Integer, ByRef iCol As Integer)
        iRow = i \ FCols
        iCol = i Mod FCols
    End Sub

    Private Function CellAt(ByVal iRow As Integer, ByVal iCol As Integer) As LifeCell

        If iRow < 0 Then Return Nothing
        If iRow >= FRows Then Return Nothing
        If iCol < 0 Then Return Nothing
        If iCol >= FCols Then Return Nothing

        Return FCells((iRow * FCols) + iCol)
    End Function

    Public Sub NextGeneration()

        Dim i As Integer
        Dim iRow, iCol As Integer
        Dim iLRow, iLCol As Integer     'loop variables
        Dim oC, oCn As LifeCell
        Dim oHRT As HighResTimer

        oHRT = New HighResTimer
        oHRT.StartTimer()

        For i = 0 To FCells.Count - 1
            oC = FCells.Item(i)
            oC.Neighbors = 0
            IndextoRowCol(i, iRow, iCol)

            For iLRow = -1 To 1
                For iLCol = -1 To 1
                    If iLRow = 0 And iLCol = 0 Then
                        'nothing
                    Else
                        oCn = CellAt(iRow + iLRow, iCol + iLCol)
                        If Not (oCn Is Nothing) Then
                            If oCn.Alive Then
                                oC.Neighbors += 1
                            End If
                        End If
                    End If
                Next
            Next
        Next

        'have to do this in two passes, or changing a cell mid-pass would affect neighbors
        For Each oC In FCells
            oC.LiveToSeeAnotherDay()
        Next

        FTicksElapsed = oHRT.EndTimer()
        Me.Invalidate()
    End Sub

    Public Sub NextGeneration2()

        Dim i As Integer
        Dim iRow, iCol As Integer
        Dim iLRow, iLCol As Integer     'loop variables
        Dim oC, oCn As LifeCell
        Dim oHRT As HighResTimer

        oHRT = New HighResTimer
        oHRT.StartTimer()

        'clear
        For Each oC In FCells
            oC.Neighbors = 0
        Next

        For i = 0 To FCells.Count - 1
            oC = FCells.Item(i)
            IndextoRowCol(i, iRow, iCol)

            If oC.Alive Then
                For iLRow = -1 To 1
                    For iLCol = -1 To 1
                        If iLRow = 0 And iLCol = 0 Then
                            'nothing
                        Else
                            oCn = CellAt(iRow + iLRow, iCol + iLCol)
                            If Not (oCn Is Nothing) Then
                                oCn.Neighbors += 1
                            End If
                        End If
                    Next
                Next

            End If
        Next

        'have to do this in two passes, or changing a cell mid-pass would affect neighbors
        For Each oC In FCells
            oC.LiveToSeeAnotherDay()
        Next

        FTicksElapsed = oHRT.EndTimer()
        Me.Invalidate()
    End Sub

    Dim FTicksElapsed As Long
    ReadOnly Property TicksElapsed() As Long
        Get
            Return FTicksElapsed
        End Get
    End Property

    Private Class LifeCell

        Private FPan As LifePanel

        Public Sub New(ByVal lp As LifePanel)
            MyBase.new()
            FPan = lp
        End Sub

        Private FAlive As Boolean = False
        Property Alive() As Boolean
            Get
                Return FAlive
            End Get
            Set(ByVal Value As Boolean)
                FAlive = Value
            End Set
        End Property

        Private FNeighbors As Integer
        Property Neighbors() As Integer
            Get
                Return FNeighbors
            End Get
            Set(ByVal Value As Integer)
                FNeighbors = Value
            End Set
        End Property

        Private fPos As Point
        Property Position() As Point
            Get
                Return fPos
            End Get
            Set(ByVal Value As Point)
                fPos = Value
            End Set
        End Property

        Public Overridable Sub Draw(ByVal g As Graphics)

            Dim ogc As GraphicsContainer
            Dim iRad As Integer
            Dim r As Rectangle
            Dim b As Brush

            iRad = FPan.CellRadius - 2
            r = New Rectangle(1, 1, iRad, iRad)
            If Me.Alive Then
                b = Brushes.Yellow
            Else
                b = Brushes.Black
            End If

            ogc = g.BeginContainer
            Try
                g.TranslateTransform(fPos.X, fPos.Y)
                g.FillRectangle(b, r)
            Finally
                g.EndContainer(ogc)
            End Try

        End Sub

        Public Sub LiveToSeeAnotherDay()
            If Not Alive Then
                'not alive now, comes to life if 3 neighbors
                Alive = (Neighbors = 3)
            Else
                'alive now, stays alive w/ 2 or 3 neighbors
                Alive = (Neighbors = 2 Or Neighbors = 3)
            End If
        End Sub

        ReadOnly Property ClientRectangle() As Rectangle
            Get
                Return New Rectangle(fPos.X, fPos.Y, FPan.CellRadius, FPan.CellRadius)
            End Get
        End Property

    End Class

End Class

