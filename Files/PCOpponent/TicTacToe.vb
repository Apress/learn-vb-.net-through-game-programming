Imports System.Drawing.Drawing2D

Public Class TicTacToePiece
    Implements IPCOpponentGamePiece

    Public Sub Draw(ByVal g As System.Drawing.Graphics) _
        Implements IPCOpponentGamePiece.Draw

        Dim r As New Rectangle(Location, Size)
        Dim p As New PointF(Location.X, Location.Y)
        Dim f As New Font("Tahoma", 36, FontStyle.Bold)

        r.Inflate(-2, -2)
        g.DrawRectangle(Pens.White, r)

        p.X += 28
        p.Y += 20

        Select Case Value
            Case -1
                g.DrawString("O", f, Brushes.Blue, p)
            Case 1
                g.DrawString("X", f, Brushes.Red, p)
            Case Else
                Exit Sub
        End Select

    End Sub

    Private FLocation As Point
    Public Property Location() As System.Drawing.Point _
        Implements IPCOpponentGamePiece.Location

        Get
            Return FLocation
        End Get
        Set(ByVal Value As System.Drawing.Point)
            FLocation = Value
        End Set
    End Property

    Private FSize As Size
    Public Property Size() As System.Drawing.Size _
        Implements IPCOpponentGamePiece.Size

        Get
            Return FSize
        End Get
        Set(ByVal Value As System.Drawing.Size)
            FSize = Value
        End Set
    End Property

    Public Function MouseIn(ByVal x As Integer, _
        ByVal y As Integer) As Boolean _
        Implements IPCOpponentGamePiece.MouseIn

        Dim r As New Rectangle(Location, Size)
        Return r.Contains(x, y)

    End Function

    Private FValue As Integer
    Public Property Value() As Integer _
        Implements IPCOpponentGamePiece.Value

        Get
            Return FValue
        End Get
        Set(ByVal i As Integer)
            If i < -1 Or i > 1 Then
                Throw New Exception("Invalid Piece Value")
            Else
                FValue = i
            End If
        End Set
    End Property
End Class

Public Class TicTacToeGame
    Implements IPCOpponentGame

    Const WID As Integer = 104

    Private aPieces As ArrayList
    Private FComputerWon As Integer
    Private FPlayerWon As Integer

    Public Event BadMove() Implements IPCOpponentGame.BadMove
    Public Event PlayerWon() Implements IPCOpponentGame.PlayerWon
    Public Event ComputerWon() Implements IPCOpponentGame.ComputerWon
    Public Event NobodyWon() Implements IPCOpponentGame.NobodyWon
    Public Event CurrentScore(ByVal iPlayer As Integer, _
      ByVal iComputer As Integer) _
      Implements IPCOpponentGame.CurrentScore

    Public Sub New(ByVal f As System.Windows.Forms.Form)
        MyBase.New()
        pForm = f
    End Sub

    Public Sub DrawBoard(ByVal sender As Object, _
      ByVal e As System.windows.forms.PaintEventArgs) _
      Implements IPCOpponentGame.DrawBoard

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.FillRectangle(Brushes.Black, pForm.ClientRectangle)

        Dim aP As TicTacToePiece

        For Each aP In aPieces
            aP.Draw(e.Graphics)
        Next

    End Sub


    Public Sub OnMouseDown(ByVal sender As Object, _
      ByVal e As System.Windows.Forms.MouseEventArgs) _
      Implements IPCOpponentGame.OnMouseDown

        Dim aP As TicTacToePiece

        Try
            'don't let him click again
            RemoveHandler fForm.MouseDown, AddressOf OnMouseDown

            For Each aP In aPieces
                If aP.MouseIn(e.X, e.Y) Then

                    If aP.Value = 0 Then

                        aP.Value = 1
                        pForm.Invalidate()

                        If HasPlayerWon() Then
                            FPlayerWon += 1
                            RaiseEvent PlayerWon()
                            RaiseEvent CurrentScore(FPlayerWon, FComputerWon)
                            Exit For
                        Else
                            If EmptySpots() = 0 Then
                                RaiseEvent NobodyWon()
                                RaiseEvent CurrentScore(FPlayerWon, FComputerWon)
                                Exit For
                            Else

                                MakeMove()
                                pForm.Invalidate()
                                If HasComputerWon() Then
                                    FComputerWon += 1
                                    RaiseEvent ComputerWon()
                                    RaiseEvent CurrentScore(FPlayerWon, FComputerWon)
                                End If
                                Exit For
                            End If
                        End If

                    Else
                        RaiseEvent BadMove()
                        Exit For
                    End If

                End If
            Next

        Finally
            AddHandler fForm.MouseDown, AddressOf OnMouseDown
        End Try

    End Sub

    Private Function HasPlayerWon() As Boolean
        Return RowScoreExists(3)
    End Function

    Private Function HasComputerWon() As Boolean
        Return RowScoreExists(-3)
    End Function

    Private Function EmptySpots() As Integer

        Dim aP As TicTacToePiece
        Dim r As Integer = 0

        For Each aP In aPieces
            If aP.Value = 0 Then
                r += 1
            End If
        Next

        Return r
    End Function

    'short circut eval
    Public Function RowScoreExists(ByVal iScore) As Boolean

        Return RowScore(0, 1, 2) = iScore OrElse _
                RowScore(3, 4, 5) = iScore OrElse _
                RowScore(6, 7, 8) = iScore OrElse _
                RowScore(0, 3, 6) = iScore OrElse _
                RowScore(1, 4, 7) = iScore OrElse _
                RowScore(2, 5, 8) = iScore OrElse _
                RowScore(0, 4, 8) = iScore OrElse _
                RowScore(2, 4, 6) = iScore

    End Function

    Public Sub MakeMove() Implements IPCOpponentGame.MakeMove

        Dim i As Integer
        Dim aP As TicTacToePiece

        'try every blank spot with me. See if I would win there
        For Each aP In aPieces
            If aP.Value = 0 Then
                aP.Value = -1
                If HasComputerWon() Then        'i win
                    Exit Sub
                End If
                aP.Value = 0
            End If
        Next

        'try every blank spot with him. See if HE would win there
        For Each aP In aPieces
            If aP.Value = 0 Then
                aP.Value = 1
                If HasPlayerWon() Then        'player would win here. move there
                    aP.Value = -1
                    Exit Sub
                End If
                aP.Value = 0
            End If
        Next

        'try every blank spot with him. See if he would win there in 2 moves
        'try the center first (spot 4), though

        For i = 4 To aPieces.Count - 1
            aP = aPieces.Item(i)
            If aP.Value = 0 Then
                aP.Value = 1
                If RowScoreExists(2) Then        'player wins in 2 moves. move there
                    aP.Value = -1
                    Exit Sub
                End If
                aP.Value = 0
            End If
        Next

        For i = 0 To 3
            aP = aPieces.Item(i)
            If aP.Value = 0 Then
                aP.Value = 1
                If RowScoreExists(2) Then        'player wins in 2 moves. move there
                    aP.Value = -1
                    Exit Sub
                End If
                aP.Value = 0
            End If
        Next

        Debug.Assert(False, "Beep")

    End Sub

    Private Function RowScore(ByVal i As Integer, _
     ByVal j As Integer, ByVal k As Integer) As Integer

        Dim aPi, aPj, aPk As TicTacToePiece

        aPi = aPieces.Item(i)
        aPj = aPieces.Item(j)
        aPk = aPieces.Item(k)

        Return aPi.Value + aPj.Value + aPk.Value
    End Function

    Public Sub StartGame() Implements IPCOpponentGame.StartGame

        Dim i As Integer
        Dim aP As TicTacToePiece

        aPieces = New ArrayList

        For i = 0 To 8
            aP = New TicTacToePiece
            aP.Location = New Point((i Mod 3) * WID, (i \ 3) * WID)
            aP.Size = New Size(WID, WID)
            aP.Value = 0
            aPieces.Add(aP)
        Next

    End Sub

    Private fForm As Form
    Property pForm() As Form _
     Implements IPCOpponentGame.pForm

        Get
            Return fForm
        End Get
        Set(ByVal Value As Form)

            fForm = Value
            fForm.Width = 324
            fForm.Height = (WID * 3) + SystemInformation.MenuHeight + SystemInformation.CaptionHeight + 34
            AddHandler fForm.Paint, AddressOf DrawBoard
            AddHandler fForm.MouseDown, AddressOf OnMouseDown

        End Set
    End Property


End Class