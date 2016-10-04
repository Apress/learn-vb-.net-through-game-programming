Imports System.IO

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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(24, 64)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private oHogLong As DisposableResourceHog

    Private Sub Form1_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        oHogLong = New DisposableResourceHog(Application.ExecutablePath, "Long")

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles Button1.Click

        Dim oHog As DisposableResourceHog

        oHog = New DisposableResourceHog(Application.ExecutablePath, "Short")
        oHog.Dispose()
    End Sub
End Class

Public Class DisposableResourceHog
    Implements IDisposable

    Private f As FileStream
    Private oRead As BinaryReader
    Private FName As String

    Public Sub New(ByVal cFilename As String, ByVal cName As String)
        MyBase.New()

        FName = cName
        f = New FileStream(cFilename, FileMode.Open, FileAccess.Read)
        oRead = New BinaryReader(f)
        Debug.WriteLine(FName & " file opened " & DateTime.Now)

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Dispose(False)
    End Sub

    Public Overloads Sub Dispose() Implements System.IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Private Disposed As Boolean = False

    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not Me.Disposed Then

            If disposing Then
                oRead.Close()
                f.Close()
                Debug.WriteLine(FName & " file closed " & DateTime.Now)
            End If
        End If

        Me.Disposed = True
    End Sub

End Class
