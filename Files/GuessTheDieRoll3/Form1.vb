Imports System.Math
Imports System.Threading

Public Class fGuess
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Dim oRand As New Random()

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
    Friend WithEvents lbTwo As System.Windows.Forms.Label
    Friend WithEvents lbThree As System.Windows.Forms.Label
    Friend WithEvents lbSix As System.Windows.Forms.Label
    Friend WithEvents lbFive As System.Windows.Forms.Label
    Friend WithEvents lbFour As System.Windows.Forms.Label
    Friend WithEvents lbResult As System.Windows.Forms.Label
    Friend WithEvents lbTwelve As System.Windows.Forms.Label
    Friend WithEvents lbEleven As System.Windows.Forms.Label
    Friend WithEvents lbTen As System.Windows.Forms.Label
    Friend WithEvents lbNine As System.Windows.Forms.Label
    Friend WithEvents lbEight As System.Windows.Forms.Label
    Friend WithEvents lbSeven As System.Windows.Forms.Label
    Friend WithEvents DicePanel1 As DicePanel.DicePanel.DicePanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lbTwo = New System.Windows.Forms.Label
        Me.lbThree = New System.Windows.Forms.Label
        Me.lbSix = New System.Windows.Forms.Label
        Me.lbFive = New System.Windows.Forms.Label
        Me.lbFour = New System.Windows.Forms.Label
        Me.lbResult = New System.Windows.Forms.Label
        Me.lbTwelve = New System.Windows.Forms.Label
        Me.lbEleven = New System.Windows.Forms.Label
        Me.lbTen = New System.Windows.Forms.Label
        Me.lbNine = New System.Windows.Forms.Label
        Me.lbEight = New System.Windows.Forms.Label
        Me.lbSeven = New System.Windows.Forms.Label
        Me.DicePanel1 = New DicePanel.DicePanel.DicePanel
        Me.SuspendLayout()
        '
        'lbTwo
        '
        Me.lbTwo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbTwo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbTwo.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbTwo.Location = New System.Drawing.Point(56, 8)
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
        Me.lbThree.Location = New System.Drawing.Point(124, 8)
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
        Me.lbSix.Location = New System.Drawing.Point(328, 8)
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
        Me.lbFive.Location = New System.Drawing.Point(260, 8)
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
        Me.lbFour.Location = New System.Drawing.Point(192, 8)
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
        Me.lbResult.Location = New System.Drawing.Point(136, 128)
        Me.lbResult.Name = "lbResult"
        Me.lbResult.Size = New System.Drawing.Size(184, 16)
        Me.lbResult.TabIndex = 13
        Me.lbResult.Text = "Correct!"
        Me.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbTwelve
        '
        Me.lbTwelve.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbTwelve.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbTwelve.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbTwelve.Location = New System.Drawing.Point(359, 64)
        Me.lbTwelve.Name = "lbTwelve"
        Me.lbTwelve.Size = New System.Drawing.Size(56, 48)
        Me.lbTwelve.TabIndex = 19
        Me.lbTwelve.Tag = "Yes"
        Me.lbTwelve.Text = "12"
        Me.lbTwelve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbEleven
        '
        Me.lbEleven.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbEleven.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbEleven.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbEleven.Location = New System.Drawing.Point(292, 64)
        Me.lbEleven.Name = "lbEleven"
        Me.lbEleven.Size = New System.Drawing.Size(56, 48)
        Me.lbEleven.TabIndex = 18
        Me.lbEleven.Tag = "Yes"
        Me.lbEleven.Text = "11"
        Me.lbEleven.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbTen
        '
        Me.lbTen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbTen.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbTen.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbTen.Location = New System.Drawing.Point(225, 64)
        Me.lbTen.Name = "lbTen"
        Me.lbTen.Size = New System.Drawing.Size(56, 48)
        Me.lbTen.TabIndex = 17
        Me.lbTen.Tag = "Yes"
        Me.lbTen.Text = "10"
        Me.lbTen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbNine
        '
        Me.lbNine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbNine.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbNine.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbNine.Location = New System.Drawing.Point(158, 64)
        Me.lbNine.Name = "lbNine"
        Me.lbNine.Size = New System.Drawing.Size(56, 48)
        Me.lbNine.TabIndex = 16
        Me.lbNine.Tag = "Yes"
        Me.lbNine.Text = "9"
        Me.lbNine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbEight
        '
        Me.lbEight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbEight.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbEight.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbEight.Location = New System.Drawing.Point(91, 64)
        Me.lbEight.Name = "lbEight"
        Me.lbEight.Size = New System.Drawing.Size(56, 48)
        Me.lbEight.TabIndex = 15
        Me.lbEight.Tag = "Yes"
        Me.lbEight.Text = "8"
        Me.lbEight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbSeven
        '
        Me.lbSeven.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lbSeven.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lbSeven.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lbSeven.Location = New System.Drawing.Point(24, 64)
        Me.lbSeven.Name = "lbSeven"
        Me.lbSeven.Size = New System.Drawing.Size(56, 48)
        Me.lbSeven.TabIndex = 14
        Me.lbSeven.Tag = "Yes"
        Me.lbSeven.Text = "7"
        Me.lbSeven.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DicePanel1
        '
        Me.DicePanel1.BackColor = System.Drawing.Color.Black
        Me.DicePanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DicePanel1.Location = New System.Drawing.Point(0, 165)
        Me.DicePanel1.Name = "DicePanel1"
        Me.DicePanel1.Size = New System.Drawing.Size(448, 216)
        Me.DicePanel1.TabIndex = 20
        '
        'fGuess
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(448, 381)
        Me.Controls.Add(Me.DicePanel1)
        Me.Controls.Add(Me.lbTwelve)
        Me.Controls.Add(Me.lbEleven)
        Me.Controls.Add(Me.lbTen)
        Me.Controls.Add(Me.lbNine)
        Me.Controls.Add(Me.lbEight)
        Me.Controls.Add(Me.lbSeven)
        Me.Controls.Add(Me.lbResult)
        Me.Controls.Add(Me.lbSix)
        Me.Controls.Add(Me.lbFive)
        Me.Controls.Add(Me.lbFour)
        Me.Controls.Add(Me.lbThree)
        Me.Controls.Add(Me.lbTwo)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "fGuess"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Guess the Die Roll"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FBackColor As Color = Color.Blue
    Private FForeColor As Color = Color.White

    Property Guess() As Integer
        Get
            Dim c As Control

            For Each c In Me.Controls
                If TypeOf c Is Label Then
                    With c
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
                Throw New Exception("Guess must be a number from 2 to 12")
            End If
        End Set
    End Property


    Private Sub fGuess_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Guess = 2
        lbResult.Text = ""
    End Sub

    Private Sub cbButtonClick(ByVal sender As System.Object, _
       ByVal e As System.EventArgs) Handles lbTwo.Click, lbThree.Click, _
       lbFour.Click, lbFive.Click, lbSix.Click, lbSeven.Click, lbEight.Click, _
       lbNine.Click, lbTen.Click, lbEleven.Click, lbTwelve.Click

        Guess = CInt(CType(sender, Label).Text)
        Call RollTheDie()

    End Sub

    Private Sub RollTheDie()

        lbResult.Text = ""
        Application.DoEvents()

        Me.Cursor = Cursors.WaitCursor
        DicePanel1.RollDice()
        If DicePanel1.Result = Guess Then
            lbResult.Text = "Correct!"
        Else
            lbResult.Text = "Try Again"
        End If
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub DicePanel1_DieBounced() Handles DicePanel1.DieBounced
        If oRand.Next(0, 99) Mod 2 = 0 Then
            Call WavPlayer.PlayWav("GuessTheDieRoll3.DIE2.WAV")
        Else
            Call WavPlayer.PlayWav("GuessTheDieRoll3.DIE1.WAV")
        End If
    End Sub
End Class
