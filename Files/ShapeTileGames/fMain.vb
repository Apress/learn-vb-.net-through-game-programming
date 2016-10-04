Imports System.Drawing.Drawing2D

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
    Friend WithEvents cbCon As System.Windows.Forms.Button
    Friend WithEvents cbDeduc As System.Windows.Forms.Button
    Friend WithEvents cbLose As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cbCon = New System.Windows.Forms.Button
        Me.cbDeduc = New System.Windows.Forms.Button
        Me.cbLose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cbCon
        '
        Me.cbCon.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbCon.Location = New System.Drawing.Point(36, 24)
        Me.cbCon.Name = "cbCon"
        Me.cbCon.Size = New System.Drawing.Size(120, 23)
        Me.cbCon.TabIndex = 0
        Me.cbCon.Text = "Concentration"
        '
        'cbDeduc
        '
        Me.cbDeduc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbDeduc.Location = New System.Drawing.Point(36, 56)
        Me.cbDeduc.Name = "cbDeduc"
        Me.cbDeduc.Size = New System.Drawing.Size(120, 23)
        Me.cbDeduc.TabIndex = 1
        Me.cbDeduc.Text = "Deductile Reasoning"
        '
        'cbLose
        '
        Me.cbLose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbLose.Location = New System.Drawing.Point(36, 88)
        Me.cbLose.Name = "cbLose"
        Me.cbLose.Size = New System.Drawing.Size(120, 23)
        Me.cbLose.TabIndex = 2
        Me.cbLose.Text = "Lose Your Mind"
        '
        'fMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(192, 171)
        Me.Controls.Add(Me.cbLose)
        Me.Controls.Add(Me.cbDeduc)
        Me.Controls.Add(Me.cbCon)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ShapeTile Games"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub ConcenClick(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbCon.Click

        Dim f As New fConcentration
        f.ShowDialog()

    End Sub

    Private Sub DeducClick(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbDeduc.Click

        Dim f As New fDeductile
        f.ShowDialog()

    End Sub

    Private Sub cbLose_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbLose.Click

        Dim f As New fLoseYourMind
        f.ShowDialog()

    End Sub

    Private Sub fMain_Paint(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Dim b As LinearGradientBrush

        b = New LinearGradientBrush(ClientRectangle, _
            Color.Red, Color.Black, LinearGradientMode.Vertical)

        e.Graphics.FillRectangle(b, ClientRectangle)

    End Sub

End Class
