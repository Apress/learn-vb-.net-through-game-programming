Imports System.Threading

Public Class Form1
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
    Friend WithEvents lbOut As System.Windows.Forms.ListBox
    Friend WithEvents cbGo As System.Windows.Forms.Button
    Friend WithEvents tbNum As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbStatus As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lbOut = New System.Windows.Forms.ListBox
        Me.cbGo = New System.Windows.Forms.Button
        Me.tbNum = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lbStatus = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'lbOut
        '
        Me.lbOut.Location = New System.Drawing.Point(8, 64)
        Me.lbOut.Name = "lbOut"
        Me.lbOut.Size = New System.Drawing.Size(192, 199)
        Me.lbOut.TabIndex = 0
        '
        'cbGo
        '
        Me.cbGo.Location = New System.Drawing.Point(96, 32)
        Me.cbGo.Name = "cbGo"
        Me.cbGo.Size = New System.Drawing.Size(64, 23)
        Me.cbGo.TabIndex = 1
        Me.cbGo.Text = "Go"
        '
        'tbNum
        '
        Me.tbNum.Location = New System.Drawing.Point(16, 32)
        Me.tbNum.Name = "tbNum"
        Me.tbNum.Size = New System.Drawing.Size(72, 21)
        Me.tbNum.TabIndex = 2
        Me.tbNum.Text = "10"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 23)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "number of threads to generate"
        '
        'lbStatus
        '
        Me.lbStatus.Location = New System.Drawing.Point(208, 64)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(160, 199)
        Me.lbStatus.TabIndex = 4
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(376, 273)
        Me.Controls.Add(Me.lbStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbNum)
        Me.Controls.Add(Me.cbGo)
        Me.Controls.Add(Me.lbOut)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Form1"
        Me.Text = "Threads"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cbGo_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbGo.Click

        Dim iNumOf As Integer
        Dim iLoop As Integer
        Dim FThreads As ArrayList
        Dim oExec As MyThreadExecutor
        Dim bDone As Boolean

        Cursor = Cursors.WaitCursor
        lbOut.Items.Clear()
        lbStatus.Items.Clear()
        Application.DoEvents()

        Try

            Try
                iNumOf = CInt(tbNum.Text)
            Catch
                iNumOf = 10      'default 10 
            End Try

            FThreads = New ArrayList
            For iLoop = 1 To iNumOf
                oExec = New MyThreadExecutor("Thread" & iLoop, iLoop * 2, iLoop * 3)
                AddHandler oExec.NotifyDone, AddressOf DoneNotification
                FThreads.Add(oExec)
            Next

            For Each oExec In FThreads
                oExec.pThread.Start()
            Next

            'wait for each thread to finish. can't use JOIN
            bDone = False
            Do While Not bDone
                For Each oExec In FThreads
                    bDone = True
                    If oExec.pThread.IsAlive Then
                        bDone = False
                    End If
                    Application.DoEvents()
                Next
            Loop

            'output the results
            For Each oExec In FThreads
                lbOut.Items.Add(oExec.pThread.Name & " generated value  " & oExec.pReturnVal)
            Next

        Finally
            Cursor = Cursors.Default
        End Try

    End Sub

    Public Delegate Sub AddListBoxDelegate(ByVal cThreadName As String)

    'thread safe (but doesn't work in this program)
    Public Sub DoneNotification(ByVal sender As Object)

        Dim oExec As MyThreadExecutor = CType(sender, MyThreadExecutor)

        If lbStatus.InvokeRequired Then
            Dim oDel As New AddListBoxDelegate(AddressOf Me.AddListBoxValue)
            lbStatus.Invoke(oDel, New Object(0) {oExec.pThread.Name})
        Else
            AddListBoxValue(oExec.pThread.Name)
        End If
    End Sub

    Public Sub AddListBoxValue(ByVal cThreadName As String)
        lbStatus.Items.Add(cThreadName & " is done")
    End Sub


End Class

Public Class MyThreadExecutor

    Private FThread As Thread
    Private FMin As Integer
    Private FMax As Integer
    Private FReturnVal As Integer

    Public Event NotifyDone(ByVal sender As Object)

    Public Sub New(ByVal n As String, ByVal iMin As Integer, ByVal iMax As Integer)
        MyBase.New()
        FMin = iMin
        FMax = iMax

        FThread = New Thread(AddressOf RunMe)
        FThread.Name = n
        FThread.IsBackground = True
    End Sub

    ReadOnly Property pThread() As Thread
        Get
            Return FThread
        End Get
    End Property

    ReadOnly Property pReturnVal() As Integer
        Get
            Return FReturnVal
        End Get
    End Property

    Private Sub RunMe()

        Dim oRand As New Random

        'pause a random amount of time to simulate doing actual work
        FThread.Sleep(oRand.Next(1, 4) * 1000)

        FReturnVal = oRand.Next(FMin, FMax + 1)
        RaiseEvent NotifyDone(Me)
    End Sub

End Class