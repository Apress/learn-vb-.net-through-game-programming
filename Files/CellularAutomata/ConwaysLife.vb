
Namespace CellularAutomata

    Public Class ConwaysLife
        Inherits CellularAutomataGame

        Private fSetupNeighbors As Boolean

        Public Sub New(ByVal oCtl As Control)
            MyBase.New(oCtl)
        End Sub

        Protected Overrides Function CreateOneCell(ByVal oPos As Point) As CellularAutomataCell

            Dim oC As ConwaysLifeCell

            fSetupNeighbors = False     'if we're creating new cells, then neighbors not setup

            oC = New ConwaysLifeCell(oPos, Me.CellRadius)
            oC.Alive = oRand.Next(0, Int32.MaxValue) Mod 5 = 0
            Return oC

        End Function

        Protected Overrides Sub RunAGeneration()

            Dim oC As ConwaysLifeCell

            If Not fSetupNeighbors Then
                SetupNeighbors()
            End If

            'must be done in 3 separate passes, as one action could affect the other
            'clear neighbor count
            For Each oC In FCells
                oC.PreCountReset()
            Next

            'if I'm alive, updated neighbor count on all 8 neighbors
            For Each oC In FCells
                oC.UpdateNeighborsBasedOnMe()
            Next

            For Each oC In FCells
                oC.UpdateAliveBasedonNeighbors()
            Next
        End Sub

        'each cell has an arraylist that points to neighboring cells. Set this up now.
        Private Sub SetupNeighbors()

            Dim i As Integer
            Dim iRow, iCol As Integer     'loop variables
            Dim iLRow, iLCol As Integer     'loop variables
            Dim oCn As ConwaysLifeCell

            Dim oC As ConwaysLifeCell
            For i = 0 To FCells.Count - 1
                oC = FCells.Item(i)
                oC.ClearNeighbors()
                IndexToRowCol(i, iRow, iCol)

                For iLRow = -1 To 1
                    For iLCol = -1 To 1
                        If iLRow = 0 And iLCol = 0 Then
                            'nothing, same cell
                        Else
                            oCn = RowColToCell(iRow + iLRow, iCol + iLCol)
                            If Not (oCn Is Nothing) Then
                                oC.AddNeighbor(oCn)
                            End If
                        End If
                    Next
                Next
            Next

            fSetupNeighbors = True

        End Sub


    End Class

    Public Class ConwaysLifeCell
        Inherits CellularAutomataCell

        'pointers to eight neighbors
        Protected FNeighbors As ArrayList         'up to 8 neighbors, doesn't matter where they are

        Public Sub New(ByVal oPos As Point, ByVal r As Integer)
            MyBase.New(oPos, r)
        End Sub

        Overrides Function GetColor() As Color
            Return IIf(Not Alive, Color.Black, Color.Yellow)
        End Function

        Private FAlive As Boolean = False
        Property Alive() As Boolean
            Get
                Return FAlive
            End Get
            Set(ByVal Value As Boolean)
                FAlive = Value
            End Set
        End Property

        Private FNeighborCount As Integer
        Property NeighborCount() As Integer
            Get
                Return FNeighborCount
            End Get
            Set(ByVal Value As Integer)
                FNeighborCount = Value
            End Set
        End Property

        Public Sub ClearNeighbors()
            FNeighbors = New ArrayList
        End Sub

        Public Sub AddNeighbor(ByVal oC As ConwaysLifeCell)
            If FNeighbors Is Nothing Then
                FNeighbors = New ArrayList
            End If

            FNeighbors.Add(oC)      'pointer
        End Sub

        Public Overridable Sub PreCountReset()
            NeighborCount = 0
        End Sub

        Public Overridable Sub UpdateAliveBasedonNeighbors()
            If Not Alive Then
                'not alive now, comes to life if 3 neighbors
                Alive = (NeighborCount = 3)
            Else
                'alive now, stays alive w/ 2 or 3 neighbors
                Alive = (NeighborCount = 2 Or NeighborCount = 3)
            End If
        End Sub

        Public Overridable Sub UpdateNeighborsBasedOnMe()

            Dim oC As ConwaysLifeCell

            If Me.Alive Then
                For Each oC In FNeighbors
                    oC.NeighborCount += 1
                Next
            End If
        End Sub

        Public Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
            Me.Alive = Not Me.Alive
        End Sub

    End Class

End Namespace
