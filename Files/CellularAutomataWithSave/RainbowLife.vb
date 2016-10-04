Namespace CellularAutomata

    Public Class RainbowLife
        Inherits ConwaysLife

        Public Sub New(ByVal oCtl As Control)
            MyBase.New(oCtl)
        End Sub

        Protected Overrides Function CreateOneCell(ByVal oPos As Point) As CellularAutomataCell

            Dim oC As RainbowLifeCell

            oC = New RainbowLifeCell(oPos, Me.CellRadius)
            If oRand.Next(0, Int32.MaxValue) Mod 5 = 0 Then
                oC.Alive = True
                oC.SetRandomColor()
            End If

            Return oC

        End Function

    End Class

    <Serializable()> _
        Public Class RainbowLifeCell
        Inherits ConwaysLifeCell

        Private Shared oRand As New Random

        Private FNeighborRTot As Integer
        Private FNeighborGTot As Integer
        Private FNeighborBTot As Integer
        Private FColor As Color

        Public Sub New(ByVal oPos As Point, ByVal r As Integer)
            MyBase.New(oPos, r)
        End Sub

        Overrides Function GetColor() As Color
            Return IIf(Not Alive, Color.Black, FColor)
        End Function

        Private Sub SetColor(ByVal c As Color)
            FColor = c
        End Sub

        Public Overrides Sub PreCountReset()
            MyBase.PreCountReset()
            FNeighborRTot = 0
            FNeighborGTot = 0
            FNeighborBTot = 0
        End Sub

        Public Overrides Sub UpdateNeighborsBasedOnMe()

            Dim oC As RainbowLifeCell

            If Me.Alive Then
                For Each oC In FNeighbors
                    oC.NeighborCount += 1

                    oC.FNeighborRTot += Me.GetColor.R
                    oC.FNeighborGTot += Me.GetColor.G
                    oC.FNeighborBTot += Me.GetColor.B
                Next
            End If
        End Sub


        Public Overrides Sub UpdateAliveBasedonNeighbors()

            Dim oC As ConwaysLifeCell

            If Not Alive Then
                If NeighborCount = 3 Then
                    Alive = True
                    Me.SetColor(Color.FromArgb(FNeighborRTot \ 3, FNeighborGTot \ 3, FNeighborBTot \ 3))
                End If
            Else
                'alive now, stays alive w/ 2 or 3 neighbors
                Alive = (NeighborCount = 2 Or NeighborCount = 3)
            End If
        End Sub

        Public Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
            If Me.Alive Then
                Me.Alive = False
            Else
                Me.Alive = True
                SetRandomColor()
            End If
        End Sub

        'totally random rgbs gives muddy colors
        Public Sub SetRandomColor()

            Dim c As Color

            Select Case oRand.Next(0, Int32.MaxValue) Mod 10
                Case 0 : c = Color.Yellow
                Case 1 : c = Color.Green
                Case 2 : c = Color.Blue
                Case 3 : c = Color.Red
                Case 4, 5 : c = Color.White
                Case 6 : c = Color.Orange
                Case 7 : c = Color.Violet
                Case 8 : c = Color.DarkBlue
                Case 9 : c = Color.Magenta
            End Select
            Me.SetColor(c)

        End Sub

    End Class

End Namespace