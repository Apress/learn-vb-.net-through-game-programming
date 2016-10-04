Imports System.io

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Form1"
        Me.Text = "Finalize Demo"

    End Sub

#End Region

    Private oHog As ResourceHog

    Private Sub Form1_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        oHog = New ResourceHog(Application.ExecutablePath)
    End Sub
End Class


Class ResourceHog

    Private f As FileStream
    Private oRead As BinaryReader

    Public Sub New(ByVal cFilename As String)
        MyBase.New()

        f = New FileStream(cFilename, FileMode.Open, FileAccess.Read)
        oRead = New BinaryReader(f)
        Debug.WriteLine("file opened " & DateTime.Now)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()

        oRead.Close()
        f.Close()
        Debug.WriteLine("file closed " & DateTime.Now)
    End Sub

End Class

