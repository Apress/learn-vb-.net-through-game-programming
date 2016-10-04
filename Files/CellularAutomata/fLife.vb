Public Class fLife
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
    Friend WithEvents aTimer As System.Windows.Forms.Timer
    Friend WithEvents pnBottom As System.Windows.Forms.Panel
    Friend WithEvents cbGo As System.Windows.Forms.Button
    Friend WithEvents lbTime As System.Windows.Forms.Label
    Friend WithEvents cbSingle As System.Windows.Forms.Button
    Friend WithEvents lbGen As System.Windows.Forms.Label
    Friend WithEvents lbT As System.Windows.Forms.Label
    Friend WithEvents lbG As System.Windows.Forms.Label
    Friend WithEvents rbGame0 As System.Windows.Forms.RadioButton
    Friend WithEvents rbGame1 As System.Windows.Forms.RadioButton
    Friend WithEvents rbGame2 As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.aTimer = New System.Windows.Forms.Timer(Me.components)
        Me.pnBottom = New System.Windows.Forms.Panel
        Me.rbGame2 = New System.Windows.Forms.RadioButton
        Me.rbGame1 = New System.Windows.Forms.RadioButton
        Me.rbGame0 = New System.Windows.Forms.RadioButton
        Me.lbG = New System.Windows.Forms.Label
        Me.lbT = New System.Windows.Forms.Label
        Me.lbGen = New System.Windows.Forms.Label
        Me.cbGo = New System.Windows.Forms.Button
        Me.lbTime = New System.Windows.Forms.Label
        Me.cbSingle = New System.Windows.Forms.Button
        Me.pnBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'aTimer
        '
        Me.aTimer.Interval = 10
        '
        'pnBottom
        '
        Me.pnBottom.Controls.Add(Me.rbGame2)
        Me.pnBottom.Controls.Add(Me.rbGame1)
        Me.pnBottom.Controls.Add(Me.rbGame0)
        Me.pnBottom.Controls.Add(Me.lbG)
        Me.pnBottom.Controls.Add(Me.lbT)
        Me.pnBottom.Controls.Add(Me.lbGen)
        Me.pnBottom.Controls.Add(Me.cbGo)
        Me.pnBottom.Controls.Add(Me.lbTime)
        Me.pnBottom.Controls.Add(Me.cbSingle)
        Me.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnBottom.Location = New System.Drawing.Point(0, 253)
        Me.pnBottom.Name = "pnBottom"
        Me.pnBottom.Size = New System.Drawing.Size(536, 72)
        Me.pnBottom.TabIndex = 5
        '
        'rbGame2
        '
        Me.rbGame2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbGame2.Location = New System.Drawing.Point(408, 48)
        Me.rbGame2.Name = "rbGame2"
        Me.rbGame2.Size = New System.Drawing.Size(120, 16)
        Me.rbGame2.TabIndex = 13
        Me.rbGame2.Text = "The Voting Game"
        '
        'rbGame1
        '
        Me.rbGame1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbGame1.Location = New System.Drawing.Point(408, 32)
        Me.rbGame1.Name = "rbGame1"
        Me.rbGame1.Size = New System.Drawing.Size(120, 16)
        Me.rbGame1.TabIndex = 12
        Me.rbGame1.Text = "Color Life"
        '
        'rbGame0
        '
        Me.rbGame0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbGame0.Checked = True
        Me.rbGame0.Location = New System.Drawing.Point(408, 16)
        Me.rbGame0.Name = "rbGame0"
        Me.rbGame0.Size = New System.Drawing.Size(120, 16)
        Me.rbGame0.TabIndex = 11
        Me.rbGame0.TabStop = True
        Me.rbGame0.Text = "Conway's Life"
        '
        'lbG
        '
        Me.lbG.Location = New System.Drawing.Point(96, 32)
        Me.lbG.Name = "lbG"
        Me.lbG.Size = New System.Drawing.Size(72, 16)
        Me.lbG.TabIndex = 9
        Me.lbG.Text = "Generations:"
        Me.lbG.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lbT
        '
        Me.lbT.Location = New System.Drawing.Point(128, 16)
        Me.lbT.Name = "lbT"
        Me.lbT.Size = New System.Drawing.Size(40, 16)
        Me.lbT.TabIndex = 8
        Me.lbT.Text = "Ticks:"
        Me.lbT.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lbGen
        '
        Me.lbGen.Location = New System.Drawing.Point(168, 32)
        Me.lbGen.Name = "lbGen"
        Me.lbGen.Size = New System.Drawing.Size(136, 16)
        Me.lbGen.TabIndex = 7
        '
        'cbGo
        '
        Me.cbGo.Location = New System.Drawing.Point(8, 8)
        Me.cbGo.Name = "cbGo"
        Me.cbGo.TabIndex = 0
        Me.cbGo.Text = "Go"
        '
        'lbTime
        '
        Me.lbTime.Location = New System.Drawing.Point(168, 16)
        Me.lbTime.Name = "lbTime"
        Me.lbTime.Size = New System.Drawing.Size(136, 16)
        Me.lbTime.TabIndex = 6
        '
        'cbSingle
        '
        Me.cbSingle.Location = New System.Drawing.Point(8, 40)
        Me.cbSingle.Name = "cbSingle"
        Me.cbSingle.TabIndex = 1
        Me.cbSingle.Text = "Single Step"
        '
        'fLife
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(536, 325)
        Me.Controls.Add(Me.pnBottom)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "fLife"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cellular Automata"
        Me.pnBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private oP As FlickerFreePanel
    Private oCell As CellularAutomata.CellularAutomataGame

    Private Sub Form1_Load(ByVal sender As System.Object, _
      ByVal e As System.EventArgs) Handles MyBase.Load

        oP = New FlickerFreePanel

        oP.Dock = DockStyle.Fill
        Me.Controls.Add(oP)

        CreateGame()

    End Sub

    Private Sub RunOne(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles aTimer.Tick, cbSingle.Click

        oCell.Tick()
        lbTime.Text = oCell.TimerTicksElapsed
        lbGen.Text = oCell.GenerationCount

    End Sub

    Private Sub cbGo_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbGo.Click

        If aTimer.Enabled Then
            aTimer.Enabled = False
            cbGo.Text = "Go"
            rbGame0.Enabled = True
            rbGame1.Enabled = True
            rbGame2.Enabled = True
        Else
            aTimer.Enabled = True
            cbGo.Text = "Stop"
            rbGame0.Enabled = False
            rbGame1.Enabled = False
            rbGame2.Enabled = False
        End If

    End Sub

    Private Sub CreateGame()

        If rbGame0.Checked Then
            oCell = New CellularAutomata.ConwaysLife(oP)
            oCell.CellRadius = 8
        ElseIf rbGame1.Checked Then
            oCell = New CellularAutomata.RainbowLife(oP)
            oCell.CellRadius = 8
        Else
            oCell = New CellularAutomata.TheVotingGame(oP)
            oCell.CellRadius = 32
        End If

    End Sub

    Private Sub rbGame_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles rbGame0.CheckedChanged, _
        rbGame1.CheckedChanged, rbGame2.CheckedChanged

        If oP Is Nothing Then Exit Sub

        CreateGame()
    End Sub

End Class

