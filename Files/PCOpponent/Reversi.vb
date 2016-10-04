Imports System.Drawing.Drawing2D


Public Class ReversiPiece
    Implements IPCOpponentGamePiece

    Public Sub Draw(ByVal g As System.Drawing.Graphics) Implements IPCOpponentGamePiece.Draw

        Dim r As New Rectangle(Location, Size)
        Dim p As New PointF(Location.X, Location.Y)
        Dim b As Brush

        r.Inflate(-2, -2)

        Select Case Value
            Case 0
                b = New LinearGradientBrush(r, Color.White, Color.DarkGray, LinearGradientMode.Vertical)
            Case -1
                b = Brushes.Blue
            Case 1
                b = Brushes.Red
        End Select

        g.FillRectangle(b, r)
        g.DrawRectangle(Pens.White, r)

    End Sub

    Private FLocation As Point
    Public Property Location() As System.Drawing.Point Implements IPCOpponentGamePiece.Location
        Get
            Return FLocation
        End Get
        Set(ByVal Value As System.Drawing.Point)
            FLocation = Value
        End Set
    End Property

    Private FSize As Size
    Public Property Size() As System.Drawing.Size Implements IPCOpponentGamePiece.Size
        Get
            Return FSize
        End Get
        Set(ByVal Value As System.Drawing.Size)
            FSize = Value
        End Set
    End Property

    Public Function MouseIn(ByVal x As Integer, _
        ByVal y As Integer) As Boolean Implements IPCOpponentGamePiece.MouseIn

        Dim r As New Rectangle(Location, Size)
        Return r.Contains(x, y)

    End Function

    Private FValue As Integer
    Public Property Value() As Integer Implements IPCOpponentGamePiece.Value
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

    ReadOnly Property xElt() As Integer
        Get
            Return Location.X \ Size.Width
        End Get
    End Property

    ReadOnly Property yElt() As Integer
        Get
            Return Location.Y \ Size.Height
        End Get
    End Property

    Private FSaveValue As Integer

    'store prior value
    Public Sub PushValue()
        FSaveValue = Value
    End Sub

    Public Sub PopValue()
        Value = FSaveValue
    End Sub

    Public Function IsEdge() As Boolean
        Return xElt = 0 OrElse yElt = 0 OrElse xElt = 7 OrElse yElt = 7
    End Function

    Public Function IsCorner() As Boolean

        Return (xElt = 0 And yElt = 0) OrElse _
               (xElt = 0 And yElt = 7) OrElse _
               (xElt = 7 And yElt = 0) OrElse _
               (xElt = 7 And yElt = 7)

    End Function
End Class

Public Class ReversiGame
    Implements IPCOpponentGame

    Private aPieces(7, 7) As ReversiPiece
    Private FPlayer, FComputer As Integer

    Public Event NobodyWon() Implements IPCOpponentGame.NobodyWon
    Public Event PlayerWon() Implements IPCOpponentGame.PlayerWon
    Public Event BadMove() Implements IPCOpponentGame.BadMove
    Public Event ComputerWon() Implements IPCOpponentGame.ComputerWon
    Public Event CurrentScore(ByVal iPlayer As Integer, ByVal iComputer As Integer) _
        Implements IPCOpponentGame.CurrentScore

    Public Sub New(ByVal f As System.Windows.Forms.Form)
        MyBase.New()
        pForm = f
    End Sub

    Public Sub DrawBoard(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs) Implements IPCOpponentGame.DrawBoard

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.FillRectangle(Brushes.Black, pForm.ClientRectangle)

        Dim aP As ReversiPiece

        For Each aP In aPieces
            aP.Draw(e.Graphics)
        Next

    End Sub

    Private Function HasComputerWon() As Boolean

        If (FComputer + FPlayer) = 64 Then
            Return FComputer > FPlayer
        ElseIf FPlayer = 0 Then
            Return True
        ElseIf PlayerCantMoveAnywhere(-1) And PlayerCantMoveAnywhere(1) Then
            Return FComputer > FPlayer
        End If
    End Function

    Private Function HasPlayerWon() As Boolean

        If (FComputer + FPlayer) = 64 Then
            Return FComputer < FPlayer
        ElseIf FComputer = 0 Then
            Return True
        ElseIf PlayerCantMoveAnywhere(-1) And PlayerCantMoveAnywhere(1) Then
            Return FComputer < FPlayer
        End If

    End Function

    Private Function PlayerCantMoveAnywhere(ByVal Player As Integer) As Boolean
        Dim aP As ReversiPiece

        For Each aP In aPieces
            If aP.Value = 0 Then
                If CanMoveHere(aP, Player) Then
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Public Sub MakeMove() Implements IPCOpponentGame.MakeMove

        Dim aP As ReversiPiece

        Dim iScore As Integer
        Dim iHigh As Integer = -1
        Dim aPHigh As ReversiPiece

        PushBoard()

        For Each aP In aPieces
            If aP.Value = 0 Then
                If CanMoveHere(aP, -1) Then
                    MoveHere(aP, -1)
                    iScore = BoardScore(-1)
                    If iScore > iHigh Then
                        iHigh = iScore
                        aPHigh = aP
                    End If
                    PopBoard()
                End If
            End If
        Next

        If iHigh > 1 Then
            MoveHere(aPHigh, -1)
            pForm.Invalidate()
        Else
            MsgBox("computer has to pass")
        End If
    End Sub

    Private Function BoardScore(ByVal Player As Integer) As Integer

        Dim aP As ReversiPiece
        Dim r As Integer

        For Each aP In aPieces
            If aP.Value = Player Then
                If aP.IsCorner Then
                    r += 20
                ElseIf aP.IsEdge Then
                    r += 5
                Else
                    r += 1
                End If
            End If
        Next
        Return r

    End Function

    Private Function CanMoveOnThisLine(ByVal aP As ReversiPiece, _
           ByVal Player As Integer, ByVal iX As Integer, ByVal iY As Integer) As Boolean

        Dim x, y As Integer
        Dim bDone As Boolean
        Dim bFound As Boolean = False

        'travel 1 piece away in the proper direction
        x = aP.xElt + iX
        y = aP.yElt + iY

        'if off board, exit
        If x < 0 Or x > 7 Then Exit Function
        If y < 0 Or y > 7 Then Exit Function

        'make sure piece one away is opposite color
        If aPieces(x, y).Value <> -Player Then Exit Function

        'now, start looping. Looking for one of our pieces before the edge of the board or a blank
        x += iX
        y += iY
        bDone = (x < 0 Or x > 7 Or y < 0 Or y > 7)
        Do While Not (bDone Or bFound)
            If aPieces(x, y).Value = Player Then
                bFound = True
            ElseIf aPieces(x, y).Value = 0 Then
                bDone = True
            Else
                x += iX
                y += iY
                bDone = (x < 0 Or x > 7 Or y < 0 Or y > 7)
            End If
        Loop

        Return bFound
    End Function

    Private Function CanMoveHere(ByVal aP As ReversiPiece, ByVal Player As Integer) As Boolean

        Dim x, y As Integer

        For x = -1 To 1
            For y = -1 To 1
                If Not (x = 0 And y = 0) Then
                    If CanMoveOnThisLine(aP, Player, x, y) Then Return True
                End If
            Next
        Next

        Return False

    End Function

    Private Sub MoveHere(ByVal aP As ReversiPiece, ByVal Player As Integer)

        Dim x, y As Integer

        aP.Value = Player

        For x = -1 To 1
            For y = -1 To 1
                If Not (x = 0 And y = 0) Then
                    If CanMoveOnThisLine(aP, Player, x, y) Then
                        MoveGuysOnThisLine(aP, Player, x, y)
                    End If
                End If
            Next
        Next

        Call CalcScores()

    End Sub

    Private Sub CalcScores()


        Dim aP As ReversiPiece

        FComputer = 0
        FPlayer = 0
        For Each aP In aPieces
            Select Case aP.Value
                Case 1
                    FPlayer += 1
                Case -1
                    FComputer += 1
            End Select
        Next

        RaiseEvent CurrentScore(FPlayer, FComputer)

    End Sub
    Private Sub MoveGuysOnThisLine(ByVal aP As ReversiPiece, _
           ByVal Player As Integer, ByVal iX As Integer, ByVal iY As Integer)

        Dim x, y As Integer
        Dim bDone As Boolean

        'travel 1 piece away in the proper direction
        'don't have to check that piece is right color or off board, already determined
        x = aP.xElt + iX
        y = aP.yElt + iY

        bDone = False
        Do While Not bDone
            If aPieces(x, y).Value = Player Then
                bDone = True
            ElseIf aPieces(x, y).Value = 0 Then
                bDone = True
            Else
                aPieces(x, y).Value = Player
                x += iX
                y += iY
                bDone = (x < 0 Or x > 7 Or y < 0 Or y > 7)
            End If
        Loop

    End Sub

    Public Sub OnMouseDown(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.MouseEventArgs) Implements IPCOpponentGame.onMouseDown

        Dim aP As ReversiPiece
        Dim bDone As Boolean

        Try
            'don't let him click again
            RemoveHandler FForm.MouseDown, AddressOf OnMouseDown

            For Each aP In aPieces
                If aP.MouseIn(e.X, e.Y) Then

                    If aP.Value = 0 Then

                        If CanMoveHere(aP, 1) Then
                            MoveHere(aP, 1)
                            pForm.Invalidate()

                            FForm.Cursor = Cursors.WaitCursor
                            Application.DoEvents()
                            System.Threading.Thread.Sleep(1000)
                            FForm.Cursor = Cursors.Default
                        Else
                            RaiseEvent BadMove()
                            Exit For
                        End If

                        If HasPlayerWon() Then
                            RaiseEvent PlayerWon()
                            Exit For
                        ElseIf HasComputerWon() Then
                            RaiseEvent ComputerWon()
                            Exit For
                        Else
                            If FPlayer + FComputer = 64 Then
                                RaiseEvent NobodyWon()
                                Exit For
                            Else

                                Do
                                    MakeMove()
                                    pForm.Invalidate()

                                    If HasComputerWon() Then
                                        RaiseEvent ComputerWon()
                                        Exit For
                                    ElseIf HasPlayerWon() Then
                                        RaiseEvent PlayerWon()
                                        Exit For
                                    ElseIf FPlayer + FComputer = 64 Then
                                        RaiseEvent NobodyWon()
                                        Exit For
                                    End If

                                    If PlayerCantMoveAnywhere(1) Then
                                        MsgBox("you have to pass")
                                    Else
                                        bdone = True
                                    End If
                                Loop Until bDone

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
            AddHandler FForm.MouseDown, AddressOf OnMouseDown
        End Try

    End Sub

    Private FForm As System.Windows.Forms.Form
    Public Property pForm() As System.Windows.Forms.Form Implements IPCOpponentGame.pForm
        Get
            Return FForm
        End Get
        Set(ByVal Value As System.Windows.Forms.Form)
            FForm = Value
            AddHandler FForm.Paint, AddressOf DrawBoard
            AddHandler FForm.MouseDown, AddressOf OnMouseDown

            FForm.Width = 268
            FForm.Height = 328
            FForm.Height = (32 * 8) + SystemInformation.MenuHeight + SystemInformation.CaptionHeight + 34

        End Set
    End Property


    Public Sub StartGame() Implements IPCOpponentGame.StartGame

        Dim x, y As Integer

        For x = 0 To 7
            For y = 0 To 7

                aPieces(x, y) = New ReversiPiece
                With aPieces(x, y)
                    .Location = New Point(x * 32, y * 32)
                    .Size = New Size(32, 32)

                    Select Case (y * 8) + x
                        Case 27, 36
                            .Value = 1
                        Case 28, 35
                            .Value = -1
                        Case Else
                            .Value = 0
                    End Select
                End With
            Next
        Next

        Call CalcScores()

    End Sub

    Private Sub PushBoard()

        Dim aP As ReversiPiece

        For Each aP In aPieces
            aP.PushValue()
        Next

    End Sub

    Private Sub PopBoard()

        Dim aP As ReversiPiece

        For Each aP In aPieces
            aP.PopValue()
        Next

    End Sub
End Class