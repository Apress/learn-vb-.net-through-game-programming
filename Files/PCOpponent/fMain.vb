Imports System.Reflection

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
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents mNew As System.Windows.Forms.MenuItem
    Friend WithEvents mTic As System.Windows.Forms.MenuItem
    Friend WithEvents mReversi As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents mExit As System.Windows.Forms.MenuItem
    Friend WithEvents sbMain As System.Windows.Forms.StatusBar
    Friend WithEvents sb1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents sb2 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.oMenu = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.mNew = New System.Windows.Forms.MenuItem
        Me.mTic = New System.Windows.Forms.MenuItem
        Me.mReversi = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.mExit = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.sbMain = New System.Windows.Forms.StatusBar
        Me.sb1 = New System.Windows.Forms.StatusBarPanel
        Me.sb2 = New System.Windows.Forms.StatusBarPanel
        CType(Me.sb1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sb2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'oMenu
        '
        Me.oMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem2})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mNew, Me.MenuItem6, Me.mExit})
        Me.MenuItem1.Text = "&File"
        '
        'mNew
        '
        Me.mNew.Index = 0
        Me.mNew.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mTic, Me.mReversi})
        Me.mNew.Text = "&New"
        '
        'mTic
        '
        Me.mTic.Index = 0
        Me.mTic.Text = "Tic Tac Toe"
        '
        'mReversi
        '
        Me.mReversi.Index = 1
        Me.mReversi.Text = "Reversi"
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 1
        Me.MenuItem6.Text = "-"
        '
        'mExit
        '
        Me.mExit.Index = 2
        Me.mExit.Text = "E&xit"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.Text = ""
        '
        'sbMain
        '
        Me.sbMain.Location = New System.Drawing.Point(0, 251)
        Me.sbMain.Name = "sbMain"
        Me.sbMain.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.sb1, Me.sb2})
        Me.sbMain.ShowPanels = True
        Me.sbMain.Size = New System.Drawing.Size(292, 22)
        Me.sbMain.SizingGrip = False
        Me.sbMain.TabIndex = 0
        '
        'sb1
        '
        Me.sb1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.sb1.Width = 228
        '
        'sb2
        '
        Me.sb2.Width = 64
        '
        'fMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.sbMain)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Menu = Me.oMenu
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PC Opponents"
        CType(Me.sb1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sb2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private aGame As IPCOpponentGame

    Private Sub mTic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mTic.Click
        aGame = New TicTacToeGame(Me)
        SetupGame()
    End Sub
    Private Sub SetupGame()

        With aGame
            AddHandler .PlayerWon, AddressOf PlayerWon
            AddHandler .ComputerWon, AddressOf ComputerWon
            AddHandler .NobodyWon, AddressOf NobodyWon
            AddHandler .BadMove, AddressOf BadMove
            AddHandler .CurrentScore, AddressOf UpdateScores

            .StartGame()
        End With
        Me.Invalidate()

    End Sub

    Private Sub PlayerWon()
        AskToPlayAgain("You Win")
    End Sub

    Private Sub ComputerWon()
        AskToPlayAgain("I Win")
    End Sub

    Private Sub NobodyWon()
        AskToPlayAgain("It's a Draw")
    End Sub

    Private Sub AskToPlayAgain(ByVal cMsg As String)

        If MsgBox(cMsg & ", Play Again?", _
            MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Continue") = MsgBoxResult.Yes Then

            aGame.StartGame()
            Me.Invalidate()

        Else
            Me.Close()
        End If

    End Sub

    Private Sub BadMove()
        MsgBox("Can't move there", MsgBoxStyle.Critical Or MsgBoxStyle.OKOnly, "Error")
    End Sub

    Private Sub UpdateScores(ByVal iPlayer As Integer, ByVal iComputer As Integer)
        sb1.Text = "Player: " & iPlayer & ", PC:" & iComputer
    End Sub

    Private Sub mReversi_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles mReversi.Click

        aGame = New ReversiGame(Me)
        SetupGame()
    End Sub

    Private Sub mExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mExit.Click
        Me.Close()
    End Sub
End Class
