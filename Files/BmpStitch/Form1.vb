Imports System.IO
Imports System.Threading

Public Class fBMPStitch
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
    Friend WithEvents cbFile As System.Windows.Forms.Button
    Friend WithEvents oDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cbAnim As System.Windows.Forms.Button
    Friend WithEvents nudAcross As System.Windows.Forms.NumericUpDown
    Friend WithEvents lbAcross As System.Windows.Forms.Label
    Friend WithEvents lbDown As System.Windows.Forms.Label
    Friend WithEvents lbFrames As System.Windows.Forms.Label
    Friend WithEvents cbBuild As System.Windows.Forms.Button
    Friend WithEvents nudDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents lbClip As System.Windows.Forms.Label
    Friend WithEvents cbClipOff As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cbFile = New System.Windows.Forms.Button
        Me.oDialog = New System.Windows.Forms.OpenFileDialog
        Me.cbAnim = New System.Windows.Forms.Button
        Me.nudAcross = New System.Windows.Forms.NumericUpDown
        Me.lbAcross = New System.Windows.Forms.Label
        Me.nudDown = New System.Windows.Forms.NumericUpDown
        Me.lbDown = New System.Windows.Forms.Label
        Me.lbFrames = New System.Windows.Forms.Label
        Me.cbBuild = New System.Windows.Forms.Button
        Me.lbClip = New System.Windows.Forms.Label
        Me.cbClipOff = New System.Windows.Forms.CheckBox
        CType(Me.nudAcross, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbFile
        '
        Me.cbFile.Location = New System.Drawing.Point(16, 192)
        Me.cbFile.Name = "cbFile"
        Me.cbFile.TabIndex = 0
        Me.cbFile.Text = "Open..."
        '
        'oDialog
        '
        Me.oDialog.Filter = "*.bmp|*.bmp"
        Me.oDialog.InitialDirectory = "c:\vbNetGames\Art"
        Me.oDialog.Title = "open"
        '
        'cbAnim
        '
        Me.cbAnim.Location = New System.Drawing.Point(104, 192)
        Me.cbAnim.Name = "cbAnim"
        Me.cbAnim.TabIndex = 2
        Me.cbAnim.Text = "Anim"
        '
        'nudAcross
        '
        Me.nudAcross.Location = New System.Drawing.Point(160, 232)
        Me.nudAcross.Name = "nudAcross"
        Me.nudAcross.Size = New System.Drawing.Size(48, 21)
        Me.nudAcross.TabIndex = 4
        Me.nudAcross.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudAcross.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lbAcross
        '
        Me.lbAcross.Location = New System.Drawing.Point(104, 232)
        Me.lbAcross.Name = "lbAcross"
        Me.lbAcross.Size = New System.Drawing.Size(48, 23)
        Me.lbAcross.TabIndex = 5
        Me.lbAcross.Text = "Across"
        Me.lbAcross.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudDown
        '
        Me.nudDown.Location = New System.Drawing.Point(160, 256)
        Me.nudDown.Name = "nudDown"
        Me.nudDown.Size = New System.Drawing.Size(48, 21)
        Me.nudDown.TabIndex = 6
        Me.nudDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudDown.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lbDown
        '
        Me.lbDown.Location = New System.Drawing.Point(104, 256)
        Me.lbDown.Name = "lbDown"
        Me.lbDown.Size = New System.Drawing.Size(48, 23)
        Me.lbDown.TabIndex = 7
        Me.lbDown.Text = "Down"
        Me.lbDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lbFrames
        '
        Me.lbFrames.Location = New System.Drawing.Point(16, 232)
        Me.lbFrames.Name = "lbFrames"
        Me.lbFrames.Size = New System.Drawing.Size(56, 23)
        Me.lbFrames.TabIndex = 8
        Me.lbFrames.Text = "0 frames"
        '
        'cbBuild
        '
        Me.cbBuild.Location = New System.Drawing.Point(128, 336)
        Me.cbBuild.Name = "cbBuild"
        Me.cbBuild.TabIndex = 9
        Me.cbBuild.Text = "Build"
        '
        'lbClip
        '
        Me.lbClip.Location = New System.Drawing.Point(8, 288)
        Me.lbClip.Name = "lbClip"
        Me.lbClip.Size = New System.Drawing.Size(192, 40)
        Me.lbClip.TabIndex = 10
        '
        'cbClipOff
        '
        Me.cbClipOff.Location = New System.Drawing.Point(8, 248)
        Me.cbClipOff.Name = "cbClipOff"
        Me.cbClipOff.TabIndex = 11
        Me.cbClipOff.Text = "NoClip"
        '
        'fBMPStitch
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(208, 365)
        Me.Controls.Add(Me.lbClip)
        Me.Controls.Add(Me.cbBuild)
        Me.Controls.Add(Me.lbFrames)
        Me.Controls.Add(Me.lbDown)
        Me.Controls.Add(Me.nudDown)
        Me.Controls.Add(Me.lbAcross)
        Me.Controls.Add(Me.nudAcross)
        Me.Controls.Add(Me.cbAnim)
        Me.Controls.Add(Me.cbFile)
        Me.Controls.Add(Me.cbClipOff)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "fBMPStitch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BMPStitch"
        CType(Me.nudAcross, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FBaseFile As String
    Dim FFrames As Integer
    Dim pb As ClipPictureBox

    Private Sub cbFile_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbFile.Click

        If oDialog.ShowDialog Then
            FBaseFile = oDialog.FileName

            If pb Is Nothing Then
                pb = New ClipPictureBox
                pb.SizeMode = PictureBoxSizeMode.AutoSize
                pb.Location = New Point(10, 10)
                pb.Visible = True
                AddHandler pb.ClippingRectChanged, AddressOf pb_ClippingRectChanged
                Me.Controls.Add(pb)
            End If

            pb.Image = Image.FromFile(FBaseFile)

            Call CountFiles()
        End If

    End Sub

    Private Sub pb_ClippingRectChanged(ByVal r As Rectangle)
        lbClip.Text = r.ToString
    End Sub

    Private Sub CountFiles()

        Dim f As New FileInfo(FBaseFile)
        Dim d As New DirectoryInfo(f.DirectoryName)

        Dim cExt As String = f.Extension
        FFrames = 0
        For Each f In d.GetFiles("*" & cExt)
            FFrames += 1
        Next
        lbFrames.Text = FFrames & " frames"

        nudAcross.Value = Math.Sqrt(FFrames)
        nudDown.Value = nudAcross.Value
    End Sub

    Private Sub cbAnim_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbAnim.Click

        Dim f As New FileInfo(FBaseFile)
        Dim d As New DirectoryInfo(f.DirectoryName)

        Dim cExt As String = f.Extension
        For Each f In d.GetFiles("*" & cExt)
            pb.Image = Image.FromFile(f.FullName)
            pb.Refresh()
            Thread.CurrentThread.Sleep(100)
        Next

    End Sub

    Private Sub cbBuild_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbBuild.Click

        Dim b As Bitmap
        Dim f As New FileInfo(FBaseFile)
        Dim d As New DirectoryInfo(f.DirectoryName)

        Dim x, y As Integer
        Dim h, w As Integer
        Dim iA, iD As Integer
        Dim cExt As String

        iA = nudAcross.Value        'updowns store as decimal, putting in integer truncs value
        iD = nudDown.Value

        If iA * iD <> FFrames Then
            MsgBox("across * down must = frames", MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, "Error")
        Else
            w = pb.ClippingRect.Width
            h = pb.ClippingRect.Height

            b = New Bitmap(w * iA, h * iD, Graphics.FromImage(pb.Image))

            Dim g As Graphics

            g = Graphics.FromImage(b)

            Try
                x = 0
                y = 0

                cExt = f.Extension
                For Each f In d.GetFiles("*" & cExt)
                    pb.Image = Image.FromFile(f.FullName)
                    pb.Refresh()
                    Thread.CurrentThread.Sleep(100)

                    g.DrawImageUnscaled(pb.Image, x, y)

                    x += w

                    If x >= w * iA Then
                        x = 0
                        y += h
                    End If

                Next

            Finally
                g.Dispose()
            End Try

            b.Save("c:\BMPStitch.bmp", System.Drawing.Imaging.ImageFormat.Bmp)
            lbClip.Text = "c:\BMPStitch.bmp saved"

        End If

    End Sub

    Private Sub cbClipOff_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbClipOff.CheckedChanged

        pb.ClipDisabled = cbClipOff.Checked
    End Sub

End Class
