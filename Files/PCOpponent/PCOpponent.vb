
Public Interface IPCOpponentGamePiece

    Property Value() As Integer
    Property Location() As Point
    Property Size() As Size
    Function MouseIn(ByVal x As Integer, ByVal y As Integer) As Boolean
    Sub Draw(ByVal g As Graphics)

End Interface

Public Interface IPCOpponentGame

    Property pForm() As Form

    Event BadMove()
    Event PlayerWon()
    Event ComputerWon()
    Event NobodyWon()
    Event CurrentScore(ByVal iPlayer As Integer, ByVal iComputer As Integer)

    Sub StartGame()
    Sub DrawBoard(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
    Sub OnMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    Sub MakeMove()

End Interface

