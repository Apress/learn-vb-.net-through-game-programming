
Imports System.Threading
Imports System.net.sockets

Public Class fMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents oMenu As System.Windows.Forms.MainMenu
    Friend WithEvents mFile As System.Windows.Forms.MenuItem
    Friend WithEvents mNew As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents mExit As System.Windows.Forms.MenuItem
    Friend WithEvents sbLower As System.Windows.Forms.StatusBar
    Friend WithEvents sb0 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents sb1 As System.Windows.Forms.StatusBarPanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.oMenu = New System.Windows.Forms.MainMenu
        Me.mFile = New System.Windows.Forms.MenuItem
        Me.mNew = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.mExit = New System.Windows.Forms.MenuItem
        Me.sbLower = New System.Windows.Forms.StatusBar
        Me.sb0 = New System.Windows.Forms.StatusBarPanel
        Me.sb1 = New System.Windows.Forms.StatusBarPanel
        CType(Me.sb0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sb1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'oMenu
        '
        Me.oMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mFile})
        '
        'mFile
        '
        Me.mFile.Index = 0
        Me.mFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mNew, Me.MenuItem3, Me.mExit})
        Me.mFile.Text = "&File"
        '
        'mNew
        '
        Me.mNew.Index = 0
        Me.mNew.Text = "&New"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.Text = "-"
        '
        'mExit
        '
        Me.mExit.Index = 2
        Me.mExit.Text = "E&xit"
        '
        'sbLower
        '
        Me.sbLower.Location = New System.Drawing.Point(0, 339)
        Me.sbLower.Name = "sbLower"
        Me.sbLower.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.sb0, Me.sb1})
        Me.sbLower.ShowPanels = True
        Me.sbLower.Size = New System.Drawing.Size(400, 22)
        Me.sbLower.TabIndex = 0
        '
        'sb0
        '
        Me.sb0.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.sb0.Width = 192
        '
        'sb1
        '
        Me.sb1.Width = 192
        '
        'fMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(400, 361)
        Me.Controls.Add(Me.sbLower)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Menu = Me.oMenu
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Network Reversi"
        CType(Me.sb0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sb1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim Player1 As ReversiPlayer
    Dim Player2 As ReversiPlayer
    Dim Game As ReversiGame

    Dim oThread As Threading.Thread
    Dim oClient As TCPClient

    Private Sub mNew_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles mNew.Click

        ShutStuffDown()     'shut down old stuff if second game

        Dim f As New fNewGame
        If f.ShowDialog <> DialogResult.Cancel Then

            Dim bGameOn As Boolean = True

            If f.rbComputer.Checked Then
                Player1 = New HumanReversiPlayer(f.tbPlayerName.Text, Color.Red, Me)
                Player2 = New ComputerReversiPlayer("BorgBlue", Color.Blue)
            ElseIf f.rbHuman.Checked Then
                Player1 = New HumanReversiPlayer(f.tbPlayerName.Text, Color.Red, Me)
                Player2 = New HumanReversiPlayer(f.tbPlayer2Name.Text, Color.Blue, Me)
            Else            'network game
                If f.rbSrv0.Checked Then

                    Dim fSrv As New fServerConnect(f.tbPlayerName.Text)
                    If fSrv.ShowDialog = DialogResult.OK Then

                        oClient = fSrv.pClient      'save the client so we can close it

                        Player1 = New HumanReversiPlayer(f.tbPlayerName.Text, Color.Red, Me)
                        Player2 = New NetworkReversiPlayer(fSrv.pOpponentName, _
                            Color.Blue, oClient.GetStream)

                        oThread = New Thread(New ThreadStart( _
                            AddressOf CType(Player2, NetworkReversiPlayer).LookForTurn))
                        oThread.Start()

                    Else
                        bGameOn = False
                    End If

                Else
                    Dim fCl As New fClientConnect(f.tbPlayerName.Text, f.tbIPAddress.Text)
                    If fCl.ShowDialog = DialogResult.OK Then

                        oClient = fCl.pClient      'save the client so we can close it

                        Player1 = New NetworkReversiPlayer(fCl.pOpponentName, _
                            Color.Red, oClient.GetStream)

                        oThread = New Thread(New ThreadStart( _
                            AddressOf CType(Player1, NetworkReversiPlayer).LookForTurn))
                        oThread.Start()

                        Player2 = New HumanReversiPlayer(f.tbPlayerName.Text, Color.Blue, Me)

                    End If
                End If

            End If

            If bGameOn Then
                AddHandler Player1.IsMyTurn, AddressOf TurnNotify
                AddHandler Player2.IsMyTurn, AddressOf TurnNotify

                Game = New ReversiGame(Me, Player1, Player2)
                AddHandler Game.BadMove, AddressOf BadMoveNotify
                AddHandler Game.PlayerWon, AddressOf PlayerWonNotify
                AddHandler Game.TieGame, AddressOf TieGameNotify
                AddHandler Game.RepeatingTurn, AddressOf RepeatTurnNotify
                AddHandler Game.UpdateScore, AddressOf ScoreUpdateNotify

                Player1.MyTurn = True
                Me.Invalidate()
            Else
                sb0.Text = "game cancelled"
            End If
        End If

    End Sub

    Private Sub ShutStuffDown()
        If Not (oThread Is Nothing) Then
            Try
                oThread.Abort()
            Catch
                'don't care
            End Try
        End If
        If Not (oClient Is Nothing) Then
            Try
                oClient.Close()
            Catch
                'don't care
            End Try
        End If
    End Sub

    Private Sub TurnNotify(ByVal oPlayer As ReversiPlayer)
        sb0.Text = oPlayer.Name & "'s turn (" & oPlayer.Color.ToString & ")"
    End Sub

    Private Sub BadMoveNotify()
        sb0.Text = "Illegal move, try again"
    End Sub

    Private Sub PlayerWonNotify(ByVal oPlayer As ReversiPlayer)
        sb0.Text = oPlayer.Name & " won the game!"
    End Sub

    Private Sub TieGameNotify()
        sb0.Text = "this game ends in a tie"
    End Sub

    Private Sub RepeatTurnNotify(ByVal oPlayer As ReversiPlayer)
        sb0.Text = oPlayer.Name & " gets to move again"
    End Sub

    Private Sub ScoreUpdateNotify(ByVal oPlayer1 As ReversiPlayer, ByVal oPlayer2 As ReversiPlayer)
        sb1.Text = oPlayer1.Name & ": " & oPlayer1.Score & ", " _
            & oPlayer2.Name & ": " & oPlayer2.Score
    End Sub

    Private Sub fMain_Closing(ByVal sender As Object, _
        ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        ShutStuffDown()
    End Sub

    Private Sub fMain_Load(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

    End Sub

    Private Sub mExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mExit.Click
        If MsgBox("End Game?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Quit") = MsgBoxResult.Yes Then
            Me.Close()
        End If
    End Sub
End Class
