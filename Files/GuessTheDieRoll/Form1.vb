
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
        Me.lbOne.Location = New System.Drawing.Point(26, 24)
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
        Me.lbTwo.Location = New System.Drawing.Point(94, 24)
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
        Me.lbThree.Location = New System.Drawing.Point(162, 24)
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
        Me.lbSix.Location = New System.Drawing.Point(366, 24)
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
        Me.lbFive.Location = New System.Drawing.Point(298, 24)
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
        Me.lbFour.Location = New System.Drawing.Point(230, 24)
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
        Me.lbResult.Location = New System.Drawing.Point(24, 80)
        Me.lbResult.Name = "lbResult"
        Me.lbResult.Size = New System.Drawing.Size(392, 16)
        Me.lbResult.TabIndex = 13
        Me.lbResult.Text = "Correct!"
        Me.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fGuess
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(448, 381)
        Me.Controls.Add(Me.lbResult)
        Me.Controls.Add(Me.lbSix)
        Me.Controls.Add(Me.lbFive)
        Me.Controls.Add(Me.lbFour)
        Me.Controls.Add(Me.lbThree)
        Me.Controls.Add(Me.lbTwo)
        Me.Controls.Add(Me.lbOne)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "fGuess"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Guess the Die Roll"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pnLower As PaintPanel
    Private bmBack As Bitmap                 'background bitmap

    Private bmStop As Bitmap
    Private bmxRot As Bitmap
    Private bmyRot As Bitmap

    Private oRand As Random

    Const HGT As Integer = 144
    Const WID As Integer = 144

    Private diexPos As Integer
    Private dieyPos As Integer
    Private diexDir As Integer         '-8 to 8
    Private dieyDir As Integer         '-8 to 8, indicates direction moving
    Private dieResult As Integer      'result of the die, 1-6
    Private dieFrame As Integer

    Private Enum DieMovementStatus
        dmsStopped = 0
        dmsRolling = 1
        dmsLanding = 2
    End Enum
    Private dieStatus As DieMovementStatus = DieMovementStatus.dmsLanding

    Private FGuess As Integer
    Property Guess() As Integer
        Get
            Return FGuess
        End Get

        Set(ByVal Value As Integer)
            FGuess = Value
        End Set
    End Property

    Private Sub fGuess_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        pnLower = New PaintPanel
        pnLower.BackColor = Color.Black
        pnLower.Dock = DockStyle.Bottom
        pnLower.Visible = True
        AddHandler pnLower.Paint, AddressOf pnLower_Paint
        Me.Controls.Add(pnLower)
        pnLower.Height = Me.Height - lbResult.Height - lbResult.Top - 48

        bmBack = New Bitmap(pnLower.Width, pnLower.Height)

        'initialize the random number generator
        oRand = New Random

        Guess = oRand.Next(1, 7)
        Call UpdateGuessButtons()
        dieFrame = (Guess - 1) * 6

        Dim a As Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        bmxRot = New Bitmap(a.GetManifestResourceStream("GuessTheDieRoll.dicexrot.bmp"))
        bmyRot = New Bitmap(a.GetManifestResourceStream("GuessTheDieRoll.diceyrot.bmp"))
        bmStop = New Bitmap(a.GetManifestResourceStream("GuessTheDieRoll.dicedone.bmp"))

        'initialize the location of the die
        diexPos = oRand.Next(0, pnLower.Width - WID)
        dieyPos = oRand.Next(0, pnLower.Height - HGT)
        DrawDie()

        lbResult.Text = ""
    End Sub

    Private Sub cbButtonClick(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles lbOne.Click, _
        lbTwo.Click, lbThree.Click, lbFour.Click, lbFive.Click, lbSix.Click

        Guess = CInt(CType(sender, Label).Text)
        Call UpdateGuessButtons()
        Call RollTheDie()

        If dieResult = Guess Then
            lbResult.Text = "Correct!"
        Else
            lbResult.Text = "Try Again"
        End If
    End Sub

    Private Sub pnLower_Paint(ByVal sender As System.Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs)

        e.Graphics.DrawImage(bmBack, 0, 0)
    End Sub

    Private Sub RollTheDie()

        Dim iLoop As Integer = 0

        lbResult.Text = ""

        'initialize the directions, 0/1 no good
        Do
            diexDir = oRand.Next(-8, 9)
        Loop Until Math.Abs(diexDir) > 3
        Do
            dieyDir = oRand.Next(-8, 9)
        Loop Until Math.Abs(dieyDir) > 3

        'decide what the result will be
        dieResult = oRand.Next(1, 7)

        Application.DoEvents()
        Me.Cursor = Cursors.WaitCursor

        dieStatus = DieMovementStatus.dmsRolling
        Do
            UpdateDiePosition()
            DrawDie()

            iLoop += 1

            Select Case dieStatus
                Case DieMovementStatus.dmsRolling
                    'after 100 frames, have a 15% chance 
                    'that the die will stop rolling
                    If iLoop > 100 And oRand.Next(1, 100) < 10 Then
                        dieStatus = DieMovementStatus.dmsLanding
                        iLoop = 0

                        dieFrame = dieResult * 6
                    End If

                Case DieMovementStatus.dmsLanding
                    'die lands for 6 frames and stops
                    If iLoop > 5 Then
                        dieStatus = DieMovementStatus.dmsStopped
                    End If
            End Select

        Loop Until dieStatus = DieMovementStatus.dmsStopped
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub UpdateDiePosition()

        Select Case dieStatus
            Case DieMovementStatus.dmsLanding
                'if landing reduce the frame by 1, regardless of direction
                dieFrame -= 1
            Case DieMovementStatus.dmsRolling
                'frame goes up or down based on y direction
                dieFrame += Math.Sign(dieyDir)
        End Select
        If dieFrame < 0 Then dieFrame += 36
        If dieFrame > 35 Then dieFrame -= 36

        'update the position

        diexPos += diexDir

        'bounce for x
        If diexPos < 0 Then
            diexPos = 0
            diexDir = -diexDir
        End If
        If diexPos > pnLower.Width - WID Then
            diexPos = pnLower.Width - WID
            diexDir = -diexDir
        End If

        dieyPos += dieyDir
        'bounce for y
        If dieyPos < 0 Then
            dieyPos = 0
            dieyDir = -dieyDir
        End If
        If dieyPos > pnLower.Height - HGT Then
            dieyPos = pnLower.Height - HGT
            dieyDir = -dieyDir
        End If

    End Sub

    Private Sub DrawDie()

        Dim gr As Graphics
        Dim oBitmap As Bitmap

        Dim x As Integer = (dieFrame Mod 6) * WID
        Dim y As Integer = (dieFrame \ 6) * HGT
        Dim r As New System.Drawing.Rectangle(x, y, WID, HGT)

        'select the correct bitmap based on what the die is doing, and what direction it's going
        If dieStatus = DieMovementStatus.dmsRolling Then
            'check quandrant rolling towards based on sign of xdir*ydir
            If (diexDir * dieyDir) > 0 Then
                oBitmap = bmyRot
            Else
                oBitmap = bmxRot
            End If
        Else
            oBitmap = bmStop
        End If

        gr = Graphics.FromImage(bmBack)
        Try
            gr.Clear(Color.Black)
            gr.DrawImage(oBitmap, diexPos, dieyPos, r, GraphicsUnit.Pixel)
        Finally
            gr.Dispose()
        End Try

        pnLower.Invalidate()
        Application.DoEvents()

    End Sub

    Private Sub UpdateGuessButtons()

        Dim ctl As Control
        Dim clrBack As Color = Color.Blue
        Dim clrFore As Color = Color.White

        For Each ctl In Me.Controls
            If TypeOf ctl Is Label Then
                With ctl
                    If .Tag = "Yes" Then
                        If CInt(.Text) = Guess Then
                            .BackColor = clrBack
                            .ForeColor = clrFore
                        Else
                            .BackColor = Color.FromKnownColor(KnownColor.Control)
                            .ForeColor = Color.Black
                        End If
                    End If
                End With
            End If
        Next

    End Sub

End Class
