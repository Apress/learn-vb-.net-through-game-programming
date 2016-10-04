Imports System.Net.Sockets
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Delegate Sub MoveToLocationDef(ByVal sender As ReversiPlayer, _
    ByVal x As Integer, ByVal y As Integer)

Public Delegate Sub MakeBestMoveDef(ByVal sender As ReversiPlayer)

Public MustInherit Class ReversiPlayer

    Private FName As String
    Private FColor As Color
    Private FScore As Integer
    Private FMyTurn As Boolean

    Public Event IsMyTurn(ByVal oPlayer As ReversiPlayer)

    Sub New(ByVal cNm As String, ByVal cClr As Color)
    MyBase.New()

    FName = cNm

    'only two colors allowed
    Debug.Assert(cClr.Equals(Color.Red) Or cClr.Equals(Color.Blue))
    FColor = cClr
End Sub

ReadOnly Property Name() As String
    Get
        Return FName
    End Get
End Property

ReadOnly Property Color() As Color
    Get
        Return FColor
    End Get
End Property

ReadOnly Property OpponentColor() As Color
    Get
        Return IIf(FColor.Equals(Color.Red), Color.Blue, Color.Red)
    End Get
End Property

Overridable Property MyTurn() As Boolean
    Get
        Return FMyTurn
    End Get
    Set(ByVal Value As Boolean)
        FMyTurn = Value
        If Value Then
            RaiseEvent IsMyTurn(Me)
        End If
    End Set
End Property

Property Score() As Integer
    Get
        Return FScore
    End Get
    Set(ByVal Value As Integer)
        FScore = Value
    End Set
End Property

End Class

Public Class HumanReversiPlayer
    Inherits ReversiPlayer

    Private FForm As Form
    Public Event MyMoveLoc As MoveToLocationDef

    Sub New(ByVal cNm As String, ByVal cClr As Color, ByVal f As Form)
        MyBase.New(cNm, cClr)
        FForm = f
    End Sub

    Overrides Property MyTurn() As Boolean
        Get
            Return MyBase.MyTurn
        End Get
        Set(ByVal Value As Boolean)
            If Value Then
                AddHandler FForm.MouseDown, AddressOf OnMouseDown
            Else
                RemoveHandler FForm.MouseDown, AddressOf OnMouseDown
            End If

            MyBase.MyTurn = Value
        End Set
    End Property

    Private Sub OnMouseDown(ByVal sender As Object, _
      ByVal e As System.Windows.Forms.MouseEventArgs)

        If Not MyTurn Then Exit Sub
        If Not e.Button = MouseButtons.Left Then Exit Sub

        RaiseEvent MyMoveLoc(Me, e.X, e.Y)
    End Sub
End Class

Public Class ComputerReversiPlayer
    Inherits ReversiPlayer

    Public Event MyMakeBestMove As MakeBestMoveDef

    Sub New(ByVal cNm As String, ByVal cClr As Color)
        MyBase.New(cNm, cClr)
    End Sub

    Overrides Property MyTurn() As Boolean
        Get
            Return MyBase.MyTurn
        End Get
        Set(ByVal Value As Boolean)

            MyBase.MyTurn = Value
            If Value Then
                RaiseEvent MyMakeBestMove(Me)
            End If
        End Set
    End Property

End Class

Public Class NetworkReversiPlayer
    Inherits ReversiPlayer

    Private FStream As NetworkStream
    Public Event MyMoveLoc As MoveToLocationDef

    Sub New(ByVal cNm As String, ByVal cClr As Color, ByVal oStream As NetworkStream)
        MyBase.New(cNm, cClr)
        FStream = oStream
    End Sub

    Public Sub LookForTurn()

        Do
            If FStream.DataAvailable Then

                Dim cPiece As String
                Dim oPiece As New ReversiPiece
                Dim oSer As New XmlSerializer(oPiece.GetType)
                Dim oRead As StreamReader

                oRead = New StreamReader(FStream)
                cPiece = oRead.ReadLine
                oPiece = oSer.Deserialize(New StringReader(cPiece))

                RaiseEvent MyMoveLoc(Me, oPiece.Location.X, oPiece.Location.Y)
            End If
            System.Threading.Thread.Sleep(250)
        Loop Until False

    End Sub

    Public Sub SendMyTurnToOpponent(ByVal aP As ReversiPiece)

        Dim oSer As New XmlSerializer(aP.GetType)
        Dim oSW As New StringWriter
        Dim oWriter As New XmlTextWriter(oSW)
        Dim oByte() As Byte
        Dim cSend As String

        oSer.Serialize(oSW, aP)
        oWriter.Close()
        cSend = oSW.ToString
        cSend = cSend.Replace(Chr(10), "")
        cSend = cSend.Replace(Chr(13), "")
        cSend &= Microsoft.VisualBasic.vbCrLf        'add crlf so READLINE works
        Try
            oByte = System.Text.Encoding.ASCII.GetBytes(cSend.ToCharArray())
            FStream.Write(oByte, 0, oByte.Length)
        Catch oEX As SocketException
            MsgBox(oEX.Message)
        End Try

    End Sub

End Class

