Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Public Class fServerConnect
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
        Me.cbCancel = New System.Windows.Forms.Button
        Me.lbOut = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cbCancel
        '
        Me.cbCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cbCancel.Location = New System.Drawing.Point(190, 62)
        Me.cbCancel.Name = "cbCancel"
        Me.cbCancel.TabIndex = 0
        Me.cbCancel.Text = "Cancel"
        '
        'lbOut
        '
        Me.lbOut.Location = New System.Drawing.Point(24, 32)
        Me.lbOut.Name = "lbOut"
        Me.lbOut.Size = New System.Drawing.Size(160, 23)
        Me.lbOut.TabIndex = 1
        Me.lbOut.Text = "Hit Cancel to Stop Waiting"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(24, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(160, 23)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Waiting for Client to Connect"
        '
        'fServerConnect
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(278, 99)
        Me.Controls.Add(Me.cbCancel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbOut)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "fServerConnect"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Waiting"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private THEPORT As Integer = 8878
    Private FPlayerName As String
    Private FOpponentName As String

    Private FThread As Thread
    Private FListen As TcpListener
    Private FClient As TcpClient

    Sub New(ByVal cName As String)
        InitializeComponent()           'note - had to copy from normal constructor

        FPlayerName = cName

        Dim oIPA As IPAddress = Dns.Resolve("localhost").AddressList(0)

        Try
            FListen = New TcpListener(oIPA, THEPORT)
            FListen.Start()
        Catch oEx As Exception
            MsgBox(oEx.ToString)
        End Try

        FThread = New Thread(AddressOf LookForIt)
        FThread.Start()

    End Sub

    Private Sub LookForIt()

        Dim oStream As NetworkStream
        Dim oRead As StreamReader
        Dim oByte() As Byte
        Dim cSend As String

        FClient = FListen.AcceptTcpClient

        lbOut.Text = "Connecting..."
        Application.DoEvents()

        Me.Cursor = Cursors.WaitCursor
        Try

            'open the stream, read the name of the opponent. Send your name back
            oStream = FClient.GetStream
            oRead = New StreamReader(oStream)

            FOpponentName = oRead.ReadLine
            FThread.Sleep(500)

            cSend = FPlayerName & Microsoft.VisualBasic.vbCrLf         'add crlf so READLINE works
            oByte = System.Text.Encoding.ASCII.GetBytes(cSend.ToCharArray())
            oStream.Write(oByte, 0, oByte.Length)

            FListen.Stop()

        Finally
            Me.Cursor = Cursors.Default
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Try
    End Sub

    Private Sub cbCancel_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbCancel.Click

        FThread.Abort()
        FListen.Stop()

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
