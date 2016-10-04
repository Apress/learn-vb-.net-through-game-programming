
Public Class fGuess
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
    Friend WithEvents lbOne As System.Windows.Forms.Label
    Friend WithEvents lbTwo As System.Windows.Forms.Label
    Friend WithEvents lbThree As System.Windows.Forms.Label
    Friend WithEvents lbSix As System.Windows.Forms.Label
    Friend WithEvents lbFive As System.Windows.Forms.Label
    Friend WithEvents lbFour As System.Windows.Forms.Label
    Friend WithEvents lbResult As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lbOne = New System.Windows.Forms.Label
        Me.lbTwo = New System.Windows.Forms.Label
        Me.lbThree = New System.Windows.Forms.Label
        Me.lbSix = New System.Windows.Forms.Label
        Me.lbFive = New System.Windows.Forms.Label
        Me.lbFour = New System.Windows.Forms.Label
        Me.lbResult = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lbOne
        '
        Me.lbOne.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbOne.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbOne.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbOne.Location = New System.Drawing.Point(30, 24)
        Me.lbOne.Name = "lbOne"
        Me.lbOne.Size = New System.Drawing.Size(56, 48)
        Me.lbOne.TabIndex = 7
        Me.lbOne.Tag = "Yes"
        Me.lbOne.Text = "1"
        Me.lbOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbTwo
        '
        Me.lbTwo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbTwo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbTwo.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbTwo.Location = New System.Drawing.Point(98, 24)
        Me.lbTwo.Name = "lbTwo"
        Me.lbTwo.Size = New System.Drawing.Size(56, 48)
        Me.lbTwo.TabIndex = 8
        Me.lbTwo.Tag = "Yes"
        Me.lbTwo.Text = "2"
        Me.lbTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbThree
        '
        Me.lbThree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbThree.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbThree.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbThree.Location = New System.Drawing.Point(166, 24)
        Me.lbThree.Name = "lbThree"
        Me.lbThree.Size = New System.Drawing.Size(56, 48)
        Me.lbThree.TabIndex = 9
        Me.lbThree.Tag = "Yes"
        Me.lbThree.Text = "3"
        Me.lbThree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbSix
        '
        Me.lbSix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbSix.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbSix.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbSix.Location = New System.Drawing.Point(370, 24)
        Me.lbSix.Name = "lbSix"
        Me.lbSix.Size = New System.Drawing.Size(56, 48)
        Me.lbSix.TabIndex = 12
        Me.lbSix.Tag = "Yes"
        Me.lbSix.Text = "6"
        Me.lbSix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbFive
        '
        Me.lbFive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbFive.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbFive.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbFive.Location = New System.Drawing.Point(302, 24)
        Me.lbFive.Name = "lbFive"
        Me.lbFive.Size = New System.Drawing.Size(56, 48)
        Me.lbFive.TabIndex = 11
        Me.lbFive.Tag = "Yes"
        Me.lbFive.Text = "5"
        Me.lbFive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbFour
        '
        Me.lbFour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbFour.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbFour.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbFour.Location = New System.Drawing.Point(234, 24)
        Me.lbFour.Name = "lbFour"
        Me.lbFour.Size = New System.Drawing.Size(56, 48)
        Me.lbFour.TabIndex = 10
        Me.lbFour.Tag = "Yes"
        Me.lbFour.Text = "4"
        Me.lbFour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbResult
        '
        Me.lbResult.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbResult.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbResult.Location = New System.Drawing.Point(136, 80)
        Me.lbResult.Name = "lbResult"
        Me.lbResult.Size = New System.Drawing.Size(184, 16)
        Me.lbResult.TabIndex = 13
        Me.lbResult.Text = "Correct!"
        Me.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fGuess
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(450, 549)
        Me.Controls.Add(Me.lbResult)
        Me.Controls.Add(Me.lbSix)
        Me.Controls.Add(Me.lbFive)
        Me.Controls.Add(Me.lbFour)
        Me.Controls.Add(Me.lbThree)
        Me.Controls.Add(Me.lbTwo)
        Me.Controls.Add(Me.lbOne)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(458, 558)
        Me.Name = "fGuess"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Guess the Die Roll"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pnLower As PaintPanel

    Private FBackColor As Color = Color.Blue
    Private FForeColor As Color = Color.White

    Dim d As Die

    Property Guess() As Integer
        Get
            Dim c As Control

            For Each c In Me.Controls
                If TypeOf c Is Label Then
                    With CType(c, Label)
                        If .BackColor.Equals(FBackColor) Then
                            Return CInt(.Text)
                        End If
                    End With
                End If
            Next
        End Get

        Set(ByVal Value As Integer)

            Dim c As Control
            Dim bFound As Boolean = False

            For Each c In Me.Controls
                If TypeOf c Is Label Then
                    With c
                        If .Tag = "Yes" AndAlso CInt(.Text) = Value Then
                            .BackColor = FBackColor
                            .ForeColor = FForeColor
                            bFound = True
                        Else
                            .BackColor = Color.FromKnownColor(KnownColor.Control)
                            .ForeColor = Color.Black
                        End If
                    End With
                End If
            Next

            If Not bFound Then
                Throw New Exception("Guess must be a number from 1 to 6")
            End If
        End Set
    End Property


    Private Sub fGuess_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        pnLower = New PaintPanel()
        pnLower.BackColor = Color.Black
        pnLower.Dock = DockStyle.Bottom
        pnLower.Visible = True

        AddHandler pnLower.Paint, AddressOf pnLower_Paint
        Me.Controls.Add(pnLower)
        pnLower.Height = Me.Height - lbResult.Height - lbResult.Top - 48

        Guess = 1

        'start the die on whatever the initial guess is
        d = New Die(pnLower)
        d.Frame = (Guess - 1) * 6

        'initialize the location of the die
        d.InitializeLocation()
        d.DrawDie()

        lbResult.Text = ""
    End Sub

    Private Sub cbButtonClick(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles lbOne.Click, _
        lbTwo.Click, lbThree.Click, lbFour.Click, lbFive.Click, lbSix.Click

        Guess = CInt(CType(sender, Label).Text)
        Call RollTheDie()

    End Sub

    Private Sub pnLower_Paint(ByVal sender As System.Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs)

        e.Graphics.DrawImage(d.BackgroundPic, 0, 0)
    End Sub


    Private Sub RollTheDie()

        Dim iLoop As Integer = 0

        lbResult.Text = ""

        Application.DoEvents()
        Me.Cursor = Cursors.WaitCursor

        d.InitializeRoll()
        Do
            d.UpdateDiePosition()
            d.DrawDie()
            pnLower.Invalidate()
            Application.DoEvents()
        Loop Until d.IsNotRolling

        If d.Result = Guess Then
            lbResult.Text = "Correct!"
        Else
            lbResult.Text = "Try Again"
        End If
        Me.Cursor = Cursors.Default

    End Sub

End Class
