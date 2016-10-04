Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Public Class fClientConnect
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
    Friend WithEvents cbCancel As System.Windows.Forms.Button
    Friend WithEvents lbOut As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lbOut = New System.Windows.Forms.Label
        Me.cbCancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lbOut
        '
        Me.lbOut.Location = New System.Drawing.Point(24, 32)
        Me.lbOut.Name = "lbOut"
        Me.lbOut.Size = New System.Drawing.Size(160, 23)
        Me.lbOut.TabIndex = 3
        Me.lbOut.Text = "Hit Cancel to Stop Waiting"
        '
        'cbCancel
        '
        Me.cbCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cbCancel.Location = New System.Drawing.Point(189, 61)
        Me.cbCancel.Name = "cbCancel"
        Me.cbCancel.TabIndex = 2
        Me.cbCancel.Text = "Cancel"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(24, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(160, 23)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Looking for Server"
        '
        'fClientConnect
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(278, 99)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbOut)
        Me.Controls.Add(Me.cbCancel)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "fClientConnect"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Waiting to Connect..."
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private THEPORT As Integer = 8878

    Private FPlayerName As String
    Private FServerName As String
    Private FOpponentName As String

    Private FThread As Thread
    Private FClient As TcpClient

    Sub New(ByVal cName As String, ByVal cServer As String)

        InitializeComponent()

        FPlayerName = cName
        FServerName = cServer

        FThread = New Thread(AddressOf LookForIt)
        FThread.Start()
    End Sub

    Private Sub LookForIt()

        Dim oStream As NetworkStream
        Dim oRead As StreamReader
        Dim oByte() As Byte
        Dim cSend As String

        FClient = New TcpClient(FServerName, THEPORT)

        lbOut.Text = "Connecting..."
        Application.DoEvents()

        Me.Cursor = Cursors.WaitCursor
        Try

            'open the stream, Send your name to the server. read his name back
            'this is the opposite order of the server
            oStream = FClient.GetStream

            cSend = FPlayerName & Microsoft.VisualBasic.vbCrLf       'add crlf so READLINE works
            oByte = System.Text.Encoding.ASCII.GetBytes(cSend.ToCharArray())
            oStream.Write(oByte, 0, oByte.Length)

            FThread.Sleep(500)

            oRead = New StreamReader(oStream)
            FOpponentName = oRead.ReadLine

        Finally
            Me.Cursor = Cursors.Default
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub cbCancel_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbCancel.Click

        FThread.Abort()

    End Sub

    ReadOnly Property pOpponentName() As String
        Get
            Return FOpponentName
        End Get
    End Property

    ReadOnly Property pClient() As TcpClient
        Get
            Return FClient
        End Get
    End Property
End Class

