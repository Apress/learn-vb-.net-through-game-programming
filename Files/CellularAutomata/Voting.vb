Namespace CellularAutomata

    Public Class TheVotingGame
        Inherits CellularAutomataGame

        Private fSetupNeighbors As Boolean

        Public Sub New(ByVal oCtl As Control)
            MyBase.New(oCtl)
        End Sub

        Protected Overrides Function CreateOneCell(ByVal oPos As Point) As CellularAutomataCell

            Dim oC As VotingCell

            fSetupNeighbors = False     'if we're creating new cells, then neighbors not setup

            oC = New VotingCell(oPos, Me.CellRadius)
            oC.IsDemocrat = HalfTheTime()
            Return oC

        End Function

        Protected Overrides Sub RunAGeneration()

            Dim oC As VotingCell

            If Not fSetupNeighbors Then
                SetupNeighbors()
            End If

            'choose a random neighbor
            oC = fCells.Item(oRand.Next(0, fcells.Count))
            oC.ChangePartyAffiliation()

        End Sub

        'each cell has an arraylist that points to neighboring cells. Set this up now.
        Private Sub SetupNeighbors()

            Dim i As Integer
            Dim iRow, iCol As Integer     'loop variables
            Dim iLRow, iLCol As Integer     'loop variables
            Dim oCn As VotingCell
            Dim oC As VotingCell

            For i = 0 To FCells.Count - 1
                oC = FCells.Item(i)
                IndexToRowCol(i, iRow, iCol)

                For iLRow = -1 To 1
                    For iLCol = -1 To 1
                        If iLRow = 0 And iLCol = 0 Then
                            'nothing, same cell
                        Else
                            'wrap around to other side
                            oCn = RowColToCell(iRow + iLRow, iCol + iLCol, bWrap:=True)
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

    Public Class VotingCell
        Inherits CellularAutomataCell

        Private Shared oRand As New Random

        'pointers to eight neighbors
        Protected FNeighbors As ArrayList         'up to 8 neighbors, doesn't matter where they are

        Public Sub New(ByVal oPos As Point, ByVal r As Integer)
            MyBase.New(oPos, r)
        End Sub

        Overrides Function GetColor() As Color
            Return IIf(Not FDemocrat, Color.Blue, Color.Red)
        End Function

        Private FDemocrat As Boolean = False
        Property IsDemocrat() As Boolean
            Get
                Return FDemocrat
            End Get
            Set(ByVal Value As Boolean)
                FDemocrat = Value
            End Set
        End Property

        Public Sub AddNeighbor(ByVal oC As VotingCell)
            If FNeighbors Is Nothing Then
                FNeighbors = New ArrayList
            End If

            FNeighbors.Add(oC)      'pointer
        End Sub

        Public Sub ChangePartyAffiliation()

            Dim oC As VotingCell

            oC = FNeighbors.Item(oRand.Next(0, FNeighbors.Count))
            Me.IsDemocrat = oC.IsDemocrat

        End Sub

        Public Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
            Me.IsDemocrat = Not Me.IsDemocrat
        End Sub

    End Class

End Namespace