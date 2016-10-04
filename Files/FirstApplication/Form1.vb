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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(16, 72)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(104, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "operators"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(248, 48)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "This application shows simple basic syntax examples behind each button, which are" & _
        " explained in Appendix A"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(16, 104)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "loops and If-Then"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(16, 136)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(104, 23)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Functions"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim x As Integer
        Dim y As Integer
        Dim r As Integer

        x = 41
        y = 35

        r = x + y
        Debug.WriteLine("answer is " & r)

        r = x - y
        Debug.WriteLine("answer is " & r)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim iVal As Integer

        For iVal = 1 To 10
            If iVal Mod 2 = 0 Then
                Debug.WriteLine(iVal & " is even")
            Else
                Debug.WriteLine(iVal & " is odd")
            End If
        Next

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles Button3.Click

        Dim iVal As Integer

        For iVal = 1 To 10
            If IsAnEvenNumber(iVal) Then
                Call OutputResult(iVal, "even")
            Else
                Call OutputResult(iVal, "odd")
            End If
        Next

    End Sub

    Function IsAnEvenNumber(ByVal iVal As Integer) As Boolean

        If iVal Mod 2 = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Sub OutputResult(ByVal iVal As Integer, ByVal cResult As String)
        Debug.WriteLine(iVal & " is " & cResult)
    End Sub
End Class
