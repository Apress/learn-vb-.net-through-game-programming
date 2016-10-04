Public Class fNewGame
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
    Friend WithEvents gbNetwork As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lbPlayer1 As System.Windows.Forms.Label
    Friend WithEvents cbOk As System.Windows.Forms.Button
    Friend WithEvents cbCancel As System.Windows.Forms.Button
    Friend WithEvents rbSrv0 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents tbIPAddress As System.Windows.Forms.TextBox
    Friend WithEvents lbAddr As System.Windows.Forms.Label
    Friend WithEvents lbOpp As System.Windows.Forms.Label
    Friend WithEvents tbPlayerName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbPlayer2Name As System.Windows.Forms.TextBox
    Friend WithEvents rbNetwork As System.Windows.Forms.RadioButton
    Friend WithEvents rbHuman As System.Windows.Forms.RadioButton
    Friend WithEvents rbComputer As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.gbNetwork = New System.Windows.Forms.GroupBox
        Me.tbIPAddress = New System.Windows.Forms.TextBox
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.rbSrv0 = New System.Windows.Forms.RadioButton
        Me.lbAddr = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.tbPlayer2Name = New System.Windows.Forms.TextBox
        Me.rbNetwork = New System.Windows.Forms.RadioButton
        Me.rbHuman = New System.Windows.Forms.RadioButton
        Me.rbComputer = New System.Windows.Forms.RadioButton
        Me.tbPlayerName = New System.Windows.Forms.TextBox
        Me.lbPlayer1 = New System.Windows.Forms.Label
        Me.lbOpp = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cbOk = New System.Windows.Forms.Button
        Me.cbCancel = New System.Windows.Forms.Button
        Me.gbNetwork.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbNetwork
        '
        Me.gbNetwork.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbNetwork.Controls.Add(Me.tbIPAddress)
        Me.gbNetwork.Controls.Add(Me.RadioButton2)
        Me.gbNetwork.Controls.Add(Me.rbSrv0)
        Me.gbNetwork.Controls.Add(Me.lbAddr)
        Me.gbNetwork.Enabled = False
        Me.gbNetwork.Location = New System.Drawing.Point(8, 192)
        Me.gbNetwork.Name = "gbNetwork"
        Me.gbNetwork.Size = New System.Drawing.Size(264, 128)
        Me.gbNetwork.TabIndex = 2
        Me.gbNetwork.TabStop = False
        Me.gbNetwork.Text = "Connection Info"
        '
        'tbIPAddress
        '
        Me.tbIPAddress.Location = New System.Drawing.Point(120, 88)
        Me.tbIPAddress.Name = "tbIPAddress"
        Me.tbIPAddress.TabIndex = 2
        Me.tbIPAddress.Text = "127.0.0.1"
        '
        'RadioButton2
        '
        Me.RadioButton2.Location = New System.Drawing.Point(16, 56)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(192, 24)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "Connect to Someone else"
        '
        'rbSrv0
        '
        Me.rbSrv0.Checked = True
        Me.rbSrv0.Location = New System.Drawing.Point(16, 24)
        Me.rbSrv0.Name = "rbSrv0"
        Me.rbSrv0.Size = New System.Drawing.Size(208, 24)
        Me.rbSrv0.TabIndex = 0
        Me.rbSrv0.TabStop = True
        Me.rbSrv0.Text = "Wait for Someone to connect to me"
        '
        'lbAddr
        '
        Me.lbAddr.Location = New System.Drawing.Point(56, 88)
        Me.lbAddr.Name = "lbAddr"
        Me.lbAddr.TabIndex = 3
        Me.lbAddr.Text = "IP Address"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tbPlayer2Name)
        Me.GroupBox2.Controls.Add(Me.rbNetwork)
        Me.GroupBox2.Controls.Add(Me.rbHuman)
        Me.GroupBox2.Controls.Add(Me.rbComputer)
        Me.GroupBox2.Controls.Add(Me.tbPlayerName)
        Me.GroupBox2.Controls.Add(Me.lbPlayer1)
        Me.GroupBox2.Controls.Add(Me.lbOpp)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(264, 168)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "My Info"
        '
        'tbPlayer2Name
        '
        Me.tbPlayer2Name.Location = New System.Drawing.Point(128, 104)
        Me.tbPlayer2Name.Name = "tbPlayer2Name"
        Me.tbPlayer2Name.Size = New System.Drawing.Size(104, 21)
        Me.tbPlayer2Name.TabIndex = 10
        Me.tbPlayer2Name.Text = "Player2"
        '
        'rbNetwork
        '
        Me.rbNetwork.Location = New System.Drawing.Point(80, 136)
        Me.rbNetwork.Name = "rbNetwork"
        Me.rbNetwork.Size = New System.Drawing.Size(176, 24)
        Me.rbNetwork.TabIndex = 8
        Me.rbNetwork.Text = "other player, different PC"
        '
        'rbHuman
        '
        Me.rbHuman.Location = New System.Drawing.Point(82, 80)
        Me.rbHuman.Name = "rbHuman"
        Me.rbHuman.Size = New System.Drawing.Size(176, 24)
        Me.rbHuman.TabIndex = 7
        Me.rbHuman.Text = "other player, using same PC"
        '
        'rbComputer
        '
        Me.rbComputer.Checked = True
        Me.rbComputer.Location = New System.Drawing.Point(82, 56)
        Me.rbComputer.Name = "rbComputer"
        Me.rbComputer.TabIndex = 6
        Me.rbComputer.TabStop = True
        Me.rbComputer.Text = "the computer"
        '
        'tbPlayerName
        '
        Me.tbPlayerName.Location = New System.Drawing.Point(72, 24)
        Me.tbPlayerName.Name = "tbPlayerName"
        Me.tbPlayerName.Size = New System.Drawing.Size(104, 21)
        Me.tbPlayerName.TabIndex = 4
        Me.tbPlayerName.Text = "Player1"
        '
        'lbPlayer1
        '
        Me.lbPlayer1.Location = New System.Drawing.Point(16, 24)
        Me.lbPlayer1.Name = "lbPlayer1"
        Me.lbPlayer1.TabIndex = 5
        Me.lbPlayer1.Text = "Name:"
        '
        'lbOpp
        '
        Me.lbOpp.Location = New System.Drawing.Point(16, 80)
        Me.lbOpp.Name = "lbOpp"
        Me.lbOpp.TabIndex = 9
        Me.lbOpp.Text = "Opponent"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(88, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Name:"
        '
        'cbOk
        '
        Me.cbOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cbOk.Location = New System.Drawing.Point(280, 264)
        Me.cbOk.Name = "cbOk"
        Me.cbOk.TabIndex = 5
        Me.cbOk.Text = "Ok"
        '
        'cbCancel
        '
        Me.cbCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cbCancel.Location = New System.Drawing.Point(280, 296)
        Me.cbCancel.Name = "cbCancel"
        Me.cbCancel.TabIndex = 6
        Me.cbCancel.Text = "Cancel"
        '
        'fNewGame
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(360, 333)
        Me.Controls.Add(Me.cbCancel)
        Me.Controls.Add(Me.cbOk)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.gbNetwork)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "fNewGame"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Start New Game"
        Me.gbNetwork.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub tbPlayer_GotFocus(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles tbPlayerName.GotFocus, tbPlayer2Name.GotFocus

        Dim tb As TextBox = sender

        tb.SelectAll()
    End Sub

    Private Sub rbOpp2_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles rbNetwork.CheckedChanged, _
        rbHuman.CheckedChanged, rbComputer.CheckedChanged

        gbNetwork.Enabled = rbNetwork.Checked
    End Sub

End Class
