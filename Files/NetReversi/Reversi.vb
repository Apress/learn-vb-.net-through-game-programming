Imports System.Drawing.Drawing2D

'same basic code as the pcopponent version with the interface removed.
'we're moving the focus away from a game that has a computer opponent
'to that of a game that can be played by 1 or 2 players
'integer Value replaced with a color type
Public Class ReversiPiece

    Public Sub Draw(ByVal g As System.Drawing.Graphics)

        Dim r As New Rectangle(Location, Size)
        Dim p As New PointF(Location.X, Location.Y)
        Dim b As Brush

        r.Inflate(-2, -2)

        If Me.Value.Equals(Color.Empty) Then
            b = New LinearGradientBrush(r, Color.White, Color.DarkGray, LinearGradientMode.Vertical)
        Else
            b = New SolidBrush(Value)
        End If

        g.FillRectangle(b, r)
        g.DrawRectangle(Pens.White, r)

    End Sub

    Private FLocation As Point
    Public Property Location() As System.Drawing.Point
        Get
            Return FLocation
        End Get
        Set(ByVal Value As System.Drawing.Point)
            FLocation = Value
        End Set
    End Property

    Private FSize As Size
    Public Property Size() As System.Drawing.Size
        Get
            Return FSize
        End Get
        Set(ByVal Value As System.Drawing.Size)
            FSize = Value
        End Set
    End Property

    Public Function MouseIn(ByVal x As Integer, _
        ByVal y As Integer) As Boolean

        Dim r As New Rectangle(Location, Size)
        Return r.Contains(x, y)

    End Function

    Private FValue As Color = Color.Empty

    Public Property Value() As Color
        Get
            Return FValue
        End Get
        Set(ByVal i As Color)
            FValue = i
        End Set
    End Property

    'true if no color on this piece yet
    Public Function UnOccupied() As Boolean
        Return Me.Value.Equals(Color.Empty)
    End Function

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

    Private FSaveValue As Color

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

    Const PIECESIZE As Integer = 48

    Private aPieces(7, 7) As ReversiPiece

    Private FPlayer1 As ReversiPlayer
    Private FPlayer2 As ReversiPlayer

    Public Event BadMove()
    Public Event PlayerWon(ByVal oPlayer As ReversiPlayer)
    Public Event TieGame()
    Public Event RepeatingTurn(ByVal oPlayer As ReversiPlayer)
    Public Event UpdateScore(ByVal oPlayer1 As ReversiPlayer, ByVal oPlayer2 As ReversiPlayer)

    Public Sub New(ByVal f As System.Windows.Forms.Form, _
        ByVal p1 As ReversiPlayer, ByVal p2 As ReversiPlayer)

        MyBase.New()
        pForm = f

        FPlayer1 = p1
        FPlayer2 = p2

        If TypeOf FPlayer1 Is HumanReversiPlayer Then
            AddHandler CType(FPlayer1, HumanReversiPlayer).MyMoveLoc, _
                AddressOf MoveToLocation

        ElseIf TypeOf FPlayer1 Is NetworkReversiPlayer Then
            AddHandler CType(FPlayer1, NetworkReversiPlayer).MyMoveLoc, _
                AddressOf MoveToLocation

        ElseIf TypeOf FPlayer1 Is ComputerReversiPlayer Then
            AddHandler CType(FPlayer1, ComputerReversiPlayer).MyMakeBestMove, _
                AddressOf MakeBestMove

        End If

        If TypeOf FPlayer2 Is HumanReversiPlayer Then
            AddHandler CType(FPlayer2, HumanReversiPlayer).MyMoveLoc, _
                AddressOf MoveToLocation

        ElseIf TypeOf FPlayer2 Is NetworkReversiPlayer Then
            AddHandler CType(FPlayer2, NetworkReversiPlayer).MyMoveLoc, _
                AddressOf MoveToLocation

        ElseIf TypeOf FPlayer2 Is ComputerReversiPlayer Then
            AddHandler CType(FPlayer2, ComputerReversiPlayer).MyMakeBestMove, _
                AddressOf MakeBestMove

        End If

        StartGame()
    End Sub

    'the main "move a piece' sub. Called via event by human player. called by PC player after
    'determining best move to make.
    'this version simply calls the overloaded one after resolving the coordinates to a piece
    Private Overloads Sub MoveToLocation(ByVal oPlayer As ReversiPlayer, _
        ByVal x As Integer, ByVal y As Integer)

        Dim aP As ReversiPiece

        aP = PieceLandedOn(x, y)
        If aP Is Nothing Then Exit Sub 'didn't land on a square

        MoveToLocation(oPlayer, aP)
    End Sub

    'the main "move a piece' sub. Called via event by human player. called by PC player after
    'determining best move to make. Determines whose turn is next
    Private Overloads Sub MoveToLocation(ByVal oPlayer As ReversiPlayer, ByVal aP As ReversiPiece)

        If aP.UnOccupied Then

            If CanMoveHere(aP, oPlayer) Then
                MoveHere(aP, oPlayer)

                If TypeOf OtherPlayer(oPlayer) Is NetworkReversiPlayer Then
                    CType(OtherPlayer(oPlayer), NetworkReversiPlayer).SendMyTurnToOpponent(aP)
                End If

                pForm.Invalidate()
                Application.DoEvents()
                Call CalcScores()
            Else
                RaiseEvent BadMove()
                Exit Sub
            End If

            If Not CheckIfGameOver() Then
                If PlayerCantMoveAnywhere(OtherPlayer(oPlayer)) Then
                    RaiseEvent RepeatingTurn(oPlayer)
                    oPlayer.MyTurn = True               're-fires event
                Else
                    oPlayer.MyTurn = False
                    OtherPlayer(oPlayer).MyTurn = True
                End If
            Else
                oPlayer.MyTurn = False
                OtherPlayer(oPlayer).MyTurn = False
            End If

        Else
            RaiseEvent BadMove()
        End If

    End Sub

    Public Sub DrawBoard(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs)

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        e.Graphics.FillRectangle(Brushes.Black, pForm.ClientRectangle)

        Dim aP As ReversiPiece

        For Each aP In aPieces
            aP.Draw(e.Graphics)
        Next
        Application.DoEvents()

    End Sub

    Private Function CheckIfGameOver() As Boolean

        Dim bWon As Boolean
        Dim bTie As Boolean
        Dim oWinner As ReversiPlayer

        If (FPlayer1.Score + FPlayer2.Score) = 64 Then      'all squares filled
            Select Case FPlayer2.Score.CompareTo(FPlayer1.Score)
                Case 1
                    bWon = True
                    oWinner = FPlayer2
                Case -1
                    bWon = True
                    oWinner = FPlayer1
                Case 0
                    bTie = True
            End Select

        ElseIf FPlayer2.Score = 0 Then
            bWon = True
            oWinner = FPlayer1

        ElseIf FPlayer1.Score = 0 Then
            bWon = True
            oWinner = FPlayer2

        ElseIf PlayerCantMoveAnywhere(FPlayer2) And PlayerCantMoveAnywhere(FPlayer1) Then
            Select Case FPlayer2.Score.CompareTo(FPlayer1.Score)
                Case 1
                    bWon = True
                    oWinner = FPlayer2
                Case -1
                    bWon = True
                    oWinner = FPlayer1
                Case 0
                    bTie = True
            End Select
        End If

        If bWon Then
            RaiseEvent PlayerWon(oWinner)
            FPlayer1.MyTurn = False     'nobody else can play
            FPlayer2.MyTurn = False
            Return True
        ElseIf bTie Then
            RaiseEvent TieGame()
            FPlayer1.MyTurn = False     'nobody else can play
            FPlayer2.MyTurn = False
            Return True
        End If

    End Function

    Private Function PlayerCantMoveAnywhere(ByVal oPlayer As ReversiPlayer) As Boolean
        Dim aP As ReversiPiece

        For Each aP In aPieces
            If aP.UnOccupied Then
                If CanMoveHere(aP, oPlayer) Then
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Private Sub MakeBestMove(ByVal oPlayer As ReversiPlayer)

        Dim aP As ReversiPiece

        Dim iScore As Integer
        Dim iHigh As Integer = -1
        Dim aPHigh As ReversiPiece

        PushBoard()

        For Each aP In aPieces
            If aP.UnOccupied Then
                If CanMoveHere(aP, oPlayer) Then
                    MoveHere(aP, oPlayer)
                    iScore = BoardValue(oPlayer)
                    If iScore > iHigh Then
                        iHigh = iScore
                        aPHigh = aP
                    End If
                    PopBoard()
                End If
            End If
        Next

        System.Threading.Thread.Sleep(1000) 'wait a sec

        'I'm pretty sure a move will ALWAYS be available, or this would never get called
        If iHigh > 1 Then
            MoveToLocation(oPlayer, aPHigh)
        End If
    End Sub

    Private Function BoardValue(ByVal oPlayer As ReversiPlayer) As Integer

        Dim aP As ReversiPiece
        Dim r As Integer

        For Each aP In aPieces
            If aP.Value.Equals(oPlayer.Color) Then
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
           ByVal oPlayer As ReversiPlayer, ByVal iX As Integer, ByVal iY As Integer) As Boolean

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
        If Not aPieces(x, y).Value.Equals(oPlayer.OpponentColor) Then Exit Function

        'now, start looping. Looking for one of our pieces before the edge of the board or a blank
        x += iX
        y += iY
        bDone = (x < 0 Or x > 7 Or y < 0 Or y > 7)
        Do While Not (bDone Or bFound)
            If aPieces(x, y).Value.Equals(oPlayer.Color) Then
                bFound = True
            ElseIf aPieces(x, y).UnOccupied Then
                bDone = True
            Else
                x += iX
                y += iY
                bDone = (x < 0 Or x > 7 Or y < 0 Or y > 7)
            End If
        Loop

        Return bFound
    End Function

    Private Function CanMoveHere(ByVal aP As ReversiPiece, ByVal oPlayer As ReversiPlayer) As Boolean

        Dim x, y As Integer

        For x = -1 To 1
            For y = -1 To 1
                If Not (x = 0 And y = 0) Then
                    If CanMoveOnThisLine(aP, oPlayer, x, y) Then Return True
                End If
            Next
        Next

        Return False

    End Function

    Private Sub MoveHere(ByVal aP As ReversiPiece, ByVal oPlayer As ReversiPlayer)

        Dim x, y As Integer

        aP.Value = oPlayer.Color

        For x = -1 To 1
            For y = -1 To 1
                If Not (x = 0 And y = 0) Then
                    If CanMoveOnThisLine(aP, oPlayer, x, y) Then
                        MoveGuysOnThisLine(aP, oPlayer, x, y)
                    End If
                End If
            Next
        Next

    End Sub

    Private Sub CalcScores()

        Dim aP As ReversiPiece

        FPlayer2.Score = 0
        FPlayer1.Score = 0
        For Each aP In aPieces
            If aP.Value.Equals(FPlayer1.Color) Then FPlayer1.Score += 1
            If aP.Value.Equals(FPlayer2.Color) Then FPlayer2.Score += 1
        Next
        RaiseEvent UpdateScore(FPlayer1, FPlayer2)

    End Sub

    Private Sub MoveGuysOnThisLine(ByVal aP As ReversiPiece, _
           ByVal oPlayer As ReversiPlayer, ByVal iX As Integer, ByVal iY As Integer)

        Dim x, y As Integer
        Dim bDone As Boolean

        'travel 1 piece away in the proper direction
        'don't have to check that piece is right color or off board, already determined
        x = aP.xElt + iX
        y = aP.yElt + iY

        bDone = False
        Do While Not bDone
            If aPieces(x, y).Value.Equals(oPlayer.Color) Then
                bDone = True
            ElseIf aPieces(x, y).UnOccupied Then
                bDone = True
            Else
                aPieces(x, y).Value = oPlayer.Color
                x += iX
                y += iY
                bDone = (x < 0 Or x > 7 Or y < 0 Or y > 7)
            End If
        Loop

    End Sub

    Private Function PieceLandedOn(ByVal x As Integer, ByVal y As Integer) As ReversiPiece

        Dim aP As ReversiPiece

        For Each aP In aPieces
            If aP.MouseIn(x, y) Then
                Return aP
            End If
        Next
        Return Nothing

    End Function

    Private FForm As System.Windows.Forms.Form
    Public Property pForm() As System.Windows.Forms.Form
        Get
            Return FForm
        End Get
        Set(ByVal Value As System.Windows.Forms.Form)
            FForm = Value
            AddHandler FForm.Paint, AddressOf DrawBoard

            FForm.Width = 394
            'FForm.Height = 456
            FForm.Height = (PIECESIZE * 8) + SystemInformation.MenuHeight + SystemInformation.CaptionHeight + 34

        End Set
    End Property


    Private Sub StartGame()

        Dim x, y As Integer
        Dim oSiz As New Size(PIECESIZE, PIECESIZE)

        For x = 0 To 7
            For y = 0 To 7

                aPieces(x, y) = New ReversiPiece
                With aPieces(x, y)
                    .Location = New Point(x * PIECESIZE, y * PIECESIZE)
                    .Size = oSiz

                    Select Case (y * 8) + x
                        Case 27, 36
                            .Value = Color.Red
                        Case 28, 35
                            .Value = Color.Blue
                        Case Else
                            .Value = Color.Empty
                    End Select
                End With
            Next
        Next

        Call CalcScores()

    End Sub

    Private Function OtherPlayer(ByVal oPlayer As ReversiPlayer) As ReversiPlayer
        Return IIf(oPlayer Is FPlayer1, FPlayer2, FPlayer1)
    End Function

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