Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization.Formatters.Soap

Namespace CellularAutomata

    Public MustInherit Class CellularAutomataGame

        Private oHRT As HighResTimer
        Private FGenerationCount As Integer

        Protected FCtl As Control
        Protected FCells As ArrayList
        Protected oRand As Random
        Protected FRows As Integer = 16
        Protected FCols As Integer = 16

        Public Sub New(ByVal oCtl As Control)
            MyBase.New()
            FCtl = oCtl

            AddHandler FCtl.Paint, AddressOf ControlPaint
            AddHandler FCtl.Resize, AddressOf ControlResize
            AddHandler FCtl.MouseDown, AddressOf ControlMouseDown

            oRand = New Random
            oHRT = New HighResTimer

            CreateData()
            FCtl.Invalidate()
        End Sub

        Private FCellRadius As Integer = 8
        Property CellRadius() As Integer
            Get
                Return FCellRadius
            End Get
            Set(ByVal Value As Integer)
                FCellRadius = Value
                CreateData()
            End Set
        End Property

        ReadOnly Property GenerationCount() As Integer
            Get
                Return FGenerationCount
            End Get
        End Property

        Protected Function HalfTheTime() As Boolean
            Return oRand.Next(0, Int16.MaxValue) Mod 2 = 0
        End Function

        Private Sub CreateData()

            Dim oC As CellularAutomataCell
            Dim iRow As Integer = 0
            Dim iCol As Integer
            Dim oPt As Point

            FCells = New ArrayList
            Do
                iCol = 0
                Do
                    oPt = New Point(iCol * CellRadius, iRow * CellRadius)
                    oC = Me.CreateOneCell(oPt)
                    FCells.Add(oC)

                    iCol += 1
                Loop Until iCol * CellRadius > FCtl.Width
                iRow += 1

            Loop Until (iRow * CellRadius) > FCtl.Height

            FRows = iRow
            FCols = iCol
            FGenerationCount = 0

            Debug.Assert(FRows * FCols = FCells.Count)
        End Sub

        Protected MustOverride Function CreateOneCell(ByVal FPos As Point) As CellularAutomataCell

        Private Sub ControlPaint(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.PaintEventArgs)

            Dim oC As CellularAutomataCell
            For Each oC In FCells
                oC.Draw(e.Graphics)
            Next
        End Sub

        Private Sub ControlMouseDown(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.MouseEventArgs)

            Dim oC As CellularAutomataCell

            For Each oC In FCells
                If oC.ClientRectangle.Contains(e.X, e.Y) Then
                    If e.Button = MouseButtons.Left Then
                        oC.OnMouseDown(e)
                        FCtl.Invalidate()
                        Exit Sub
                    End If
                End If
            Next


        End Sub

        Private Sub ControlResize(ByVal sender As Object, _
            ByVal eventargs As System.EventArgs)

            CreateData()
            FCtl.Invalidate()
        End Sub

        Dim FTimerTicksElapsed As Long
        ReadOnly Property TimerTicksElapsed() As Long
            Get
                Return FTimerTicksElapsed
            End Get
        End Property

        Public Sub Tick()

            Try
                oHRT.StartTimer()
                RunAGeneration()
                FGenerationCount += 1
            Finally
                FTimerTicksElapsed = oHRT.EndTimer()
                FCtl.Invalidate()           'don't want paint time included in timings, just processing time
            End Try

        End Sub

        Protected MustOverride Sub RunAGeneration()

        Protected Sub IndexToRowCol(ByVal i As Integer, ByRef iRow As Integer, ByRef iCol As Integer)
            iRow = i \ FCols
            iCol = i Mod FCols
        End Sub

        Protected Function RowColToCell(ByVal iRow As Integer, ByVal iCol As Integer, _
        Optional ByVal bWrap As Boolean = False) As CellularAutomataCell

            If Not bWrap Then
                If iRow < 0 Then Return Nothing
                If iRow >= FRows Then Return Nothing
                If iCol < 0 Then Return Nothing
                If iCol >= FCols Then Return Nothing
            Else
                If iRow < 0 Then iRow = FRows
                If iRow >= FRows Then iRow = 0
                If iCol < 0 Then iCol = FCols
                If iCol >= FCols Then iCol = 0
            End If

            Return FCells((iRow * FCols) + iCol)
        End Function

        Public Sub SaveToFile(ByVal cFilename As String)

            Dim oFS As New FileStream(cFilename, FileMode.Create)
            Dim oBF As New SoapFormatter

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Try
                oBF.Serialize(oFS, FCells)
            Catch oEX As Exception
                MsgBox(oEX.Message)
            Finally
                oFS.Close()
                System.Windows.Forms.Cursor.Current = Cursors.Default
            End Try

        End Sub

        Public Sub LoadFromFile(ByVal cFilename As String)

            Dim oFS As New FileStream(cFilename, FileMode.Open)
            Dim oBF As New SoapFormatter

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Try
                FCells = CType(oBF.Deserialize(oFS), ArrayList)
            Catch oEX As Exception
                MsgBox(oEX.Message)
            Finally
                oFS.Close()
                System.Windows.Forms.Cursor.Current = Cursors.Default
            End Try

        End Sub

    End Class

    <Serializable()> _
       Public MustInherit Class CellularAutomataCell

        Private FRadius As Integer
        Private FDrawRad As Integer

        Public Sub New(ByVal oPos As Point, ByVal r As Integer)
            MyBase.New()
            FPos = oPos
            FRadius = r

            FDrawRad = FRadius - 2
        End Sub

        Private FPos As Point
        ReadOnly Property Position() As Point
            Get
                Return FPos
            End Get
        End Property

        ReadOnly Property Radius() As Integer
            Get
                Return FRadius
            End Get
        End Property

        Public Sub Draw(ByVal g As Graphics)

            Dim r As Rectangle
            Dim b As Brush

            r = New Rectangle(FPos.X, FPos.Y, FDrawRad, FDrawRad)
            b = New SolidBrush(Me.GetColor)
            g.FillRectangle(b, r)

        End Sub

        ReadOnly Property ClientRectangle() As Rectangle
            Get
                Return New Rectangle(FPos.X, FPos.Y, FRadius, FRadius)
            End Get
        End Property

        Public MustOverride Function GetColor() As Color
        Public MustOverride Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

    End Class

End Namespace