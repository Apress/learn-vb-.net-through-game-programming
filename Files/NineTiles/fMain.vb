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
    Friend WithEvents oDice As DicePanel.DicePanel.DicePanel
    Friend WithEvents lbHelp As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.oDice = New DicePanel.DicePanel.DicePanel
        Me.lbHelp = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'oDice
        '
        Me.oDice.BackColor = System.Drawing.Color.Black
        Me.oDice.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.oDice.Location = New System.Drawing.Point(0, 362)
        Me.oDice.Name = "oDice"
        Me.oDice.Size = New System.Drawing.Size(334, 176)
        Me.oDice.TabIndex = 0
        '
        'lbHelp
        '
        Me.lbHelp.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbHelp.Location = New System.Drawing.Point(0, 330)
        Me.lbHelp.Name = "lbHelp"
        Me.lbHelp.Size = New System.Drawing.Size(334, 32)
        Me.lbHelp.TabIndex = 1
        Me.lbHelp.Text = "click on tiles until they equal the number on the dice, then right click to take " & _
        "away tiles"
        Me.lbHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(334, 538)
        Me.Controls.Add(Me.lbHelp)
        Me.Controls.Add(Me.oDice)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "NineTiles"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private oNumPanel As NumberPanel
    Private oRand As Random
    Private oWav As WavLibrary

    Private Sub fMain_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        oNumPanel = New NumberPanel
        oNumPanel.Dock = DockStyle.Top
        AddHandler oNumPanel.MouseDown, AddressOf NumPanelMouseDown
        AddHandler oNumPanel.TileMoving, AddressOf NumPanelTileMoving
        Me.Controls.Add(oNumPanel)

        SetupWavLibrary()
        oRand = New Random

    End Sub

    Private Sub SetupWavLibrary()

        oWav = New WavLibrary
        oWav.LoadFromResource("NineTiles.die1.wav", "die1")
        oWav.LoadFromResource("NineTiles.die2.wav", "die2")
        oWav.LoadFromResource("NineTiles.squeak1.wav", "squeak1")
        oWav.LoadFromResource("NineTiles.squeak2.wav", "squeak2")
        oWav.LoadFromResource("NineTiles.thud.wav", "thud")
        oWav.LoadFromResource("NineTiles.applause.wav", "applause")
        oWav.LoadFromResource("NineTiles.laughs.wav", "laughs")
        oWav.LoadFromResource("NineTiles.dischord.wav", "dischord")

    End Sub


    Private Sub fMain_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles MyBase.Activated

        Static bDoneOnce As Boolean = False

        If Not bDoneOnce Then
            bDoneOnce = True
            StartGame()
        End If

    End Sub

    Private Sub StartGame()
        oNumPanel.ResetTiles()
        oDice.RollDice()
    End Sub

    Private Sub NumPanelMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If oNumPanel.bInAnimation Then Exit Sub

        If e.Button = MouseButtons.Right Then
            If oDice.Result = oNumPanel.Result Then

                lbHelp.Visible = False

                oNumPanel.HideBackward()
                oWav.Play("thud", 400)

                If oNumPanel.TilesVisible = 0 Then
                    oWav.Play("applause", bSync:=True)
                    StartGame()
                Else
                    oDice.RollDice()
                    If Not oNumPanel.ResultAvailable(oDice.Result) Then
                        oWav.Play("laughs", bSync:=True)
                        StartGame()
                    End If
                End If
            Else
                oWav.Play("dischord")
            End If
        End If

    End Sub

    Private Sub NumPanelTileMoving(ByVal bMovingBackward As Boolean)

        Dim cWav As String
        If bMovingBackward Then
            cWav = "squeak1"
        Else
            cWav = "squeak2"
        End If
        oWav.Play(cWav)

    End Sub

    Private Sub oDice_DieBounced() Handles oDice.DieBounced

        Dim cWav As String
        If oRand.Next(0, 1000) Mod 2 = 0 Then
            cWav = "die1"
        Else
            cWav = "die2"
        End If
        oWav.Play(cWav)

    End Sub

End Class
