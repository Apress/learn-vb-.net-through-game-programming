Imports System.Drawing.Text
Imports System.Drawing.Drawing2D

Public Enum YhatzeeScoreType
    ystNumber = 0
    ystKind = 1
    ystFullHouse = 2
    ystSmallStraight = 3
    ystLargeStraight = 4
    ystYhatzee = 5
    ystChance = 6
End Enum

Public Class fYhatzee
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
    Friend WithEvents cbRoll As System.Windows.Forms.Button
    Friend WithEvents cbNewGame As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.cbRoll = New System.Windows.Forms.Button
        Me.cbNewGame = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cbRoll
        '
        Me.cbRoll.Location = New System.Drawing.Point(192, 8)
        Me.cbRoll.Name = "cbRoll"
        Me.cbRoll.TabIndex = 0
        Me.cbRoll.Text = "Roll #1"
        '
        'cbNewGame
        '
        Me.cbNewGame.Location = New System.Drawing.Point(8, 8)
        Me.cbNewGame.Name = "cbNewGame"
        Me.cbNewGame.TabIndex = 1
        Me.cbNewGame.Text = "Start Game"
        '
        'fYhatzee
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.ClientSize = New System.Drawing.Size(536, 437)
        Me.Controls.Add(Me.cbNewGame)
        Me.Controls.Add(Me.cbRoll)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "fYhatzee"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Yhatzee"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim p As DicePanelNew.DicePanelNew.DicePanelNew
    Dim oRand As Random
    Dim oWav As WavLibrary

    Private FRoll As Integer
    Private lbTotal As AALabel

    Private Sub FormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        p = New DicePanelNew.DicePanelNew.DicePanelNew
        p.Dock = DockStyle.Right
        p.Width = 260
        p.Visible = True
        AddHandler p.DieBounced, AddressOf DiePanelDieBounced
        AddHandler p.DieFrozen, AddressOf DiePanelDieFrozen
        Me.Controls.Add(p)

        Call SetupWavLibrary()
        oRand = New Random

        p.ClickToFreeze = True
        p.NumDice = 5

        Call MakeLabels()
    End Sub

    Private Sub fYhatActivated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles MyBase.Activated

        Static bDoneOnce As Boolean = False

        If Not bDoneOnce Then
            bDoneOnce = True
            StartGame()
        End If

    End Sub

    Private Sub SetupWavLibrary()

        oWav = New WavLibrary
        oWav.LoadFromResource("YhatzeeDotNet.die1.wav", "die1")
        oWav.LoadFromResource("YhatzeeDotNet.die2.wav", "die2")
        oWav.LoadFromResource("YhatzeeDotNet.ouch.wav", "ouch")
        oWav.LoadFromResource("YhatzeeDotNet.applause.wav", "applause")

    End Sub


    Private Sub StartGame()

        Dim o As Control

        cbNewGame.Visible = False
        For Each o In Me.Controls
            If TypeOf o Is AAYhatzeeScoreLabel Then
                With CType(o, AAYhatzeeScoreLabel)
                    .Text = ""
                    .ScoreAssigned = False
                End With
            End If
        Next
        p.ClearFreeze()
        Call CalcTotal()
        pRoll = 1
        DoTheRoll()
    End Sub

    Private Sub CalcTotal()

        Dim o As Control
        Dim iTot As Integer = 0

        For Each o In Me.Controls
            If TypeOf o Is AAYhatzeeScoreLabel Then
                With CType(o, AAYhatzeeScoreLabel)
                    If .ScoreAssigned Then
                        iTot += CInt(.Text)
                    End If
                End With
            End If
        Next

        lbTotal.Text = iTot
    End Sub

    ReadOnly Property AllBoxesFilled() As Boolean
        Get
            Dim o As Control

            For Each o In Me.Controls
                If TypeOf o Is AAYhatzeeScoreLabel Then
                    With CType(o, AAYhatzeeScoreLabel)
                        If Not .ScoreAssigned Then
                            Return False
                        End If
                    End With
                End If
            Next

            Return True
        End Get
    End Property

    Private Sub DoTheRoll()
        cbRoll.Visible = False
        Cursor = Cursors.WaitCursor
        Try
            p.RollDice()
        Finally
            cbRoll.Visible = True
            Cursor = Cursors.Default
        End Try
    End Sub

    Property pRoll() As Integer
        Get
            Return FRoll
        End Get
        Set(ByVal Value As Integer)
            If Value < 1 Or Value > 3 Then
                Throw New Exception("Valid Roll Values 1-3")
            End If
            FRoll = Value
            cbRoll.Visible = FRoll < 3
            cbRoll.Text = "Roll #" & FRoll + 1
        End Set
    End Property

    'creates the anti-aliased labels
    Private Sub MakeLabels()

        Dim lbA As AALabel
        Dim lbY As AAYhatzeeScoreLabel
        Dim f As Font
        Dim y As Integer = 56
        Dim i As Integer

        f = New Font("Tahoma", 12, FontStyle.Bold Or FontStyle.Italic, GraphicsUnit.Point)
        For i = 1 To 13
            lbA = New AALabel
            With lbA
                .BackColor = Color.Transparent
                .ForeColor = Color.White
                .Font = f
                .Location = New Point(8, y)
                .Size = New Size(148, 23)
                .TextAlign = ContentAlignment.TopCenter
                Select Case i
                    Case 1
                        .Text = "Ones"
                    Case 2
                        .Text = "Twos"
                    Case 3
                        .Text = "Threes"
                    Case 4
                        .Text = "Fours"
                    Case 5
                        .Text = "Fives"
                    Case 6
                        .Text = "Sixes"
                    Case 7
                        .Text = "Three of a Kind"
                    Case 8
                        .Text = "Four of a Kind"
                    Case 9
                        .Text = "Full House"
                    Case 10
                        .Text = "Small Straight"
                    Case 11
                        .Text = "Large Straight"
                    Case 12
                        .Text = "Yhatzee (!)"
                    Case 13
                        .Text = "Chance"
                End Select
            End With
            Me.Controls.Add(lbA)

            lbY = New AAYhatzeeScoreLabel
            With lbY
                .Font = f
                .Location = New Point(164, y)
                AddHandler lbY.MouseEnter, AddressOf MouseEnterLabel
                AddHandler lbY.MouseLeave, AddressOf MouseLeaveLabel
                AddHandler lbY.MouseDown, AddressOf MouseDownLabel

                Select Case i
                    Case 1, 2, 3, 4, 5, 6
                        .ScoreType = YhatzeeScoreType.ystNumber
                        .ScoreValue = i
                    Case 7
                        .ScoreType = YhatzeeScoreType.ystKind
                        .ScoreValue = 3
                    Case 8
                        .ScoreType = YhatzeeScoreType.ystKind
                        .ScoreValue = 4
                    Case 9
                        .ScoreType = YhatzeeScoreType.ystFullHouse
                    Case 10
                        .ScoreType = YhatzeeScoreType.ystSmallStraight
                    Case 11
                        .ScoreType = YhatzeeScoreType.ystLargeStraight
                    Case 12
                        .ScoreType = YhatzeeScoreType.ystYhatzee
                    Case 13
                        .ScoreType = YhatzeeScoreType.ystChance
                End Select
            End With
            Me.Controls.Add(lbY)

            y += 24
        Next

        lbTotal = New AALabel
        With lbTotal
            .Font = f
            .Location = New Point(164 - 36, y)
            .BackColor = Color.LightBlue
            .ForeColor = Color.Black
            .Size = New Size(72, 23)
            .TextAlign = ContentAlignment.TopRight
            .BorderStyle = BorderStyle.FixedSingle
        End With
        Me.Controls.Add(lbTotal)
    End Sub

    Private Sub cbRollClick(ByVal sender As System.Object, _
       ByVal e As System.EventArgs) Handles cbRoll.Click

        DoTheRoll()
        pRoll += 1
    End Sub

    Private Sub MouseEnterLabel(ByVal sender As Object, ByVal e As System.EventArgs)

        'do nothing if we're rolling
        If Not p.AllDiceStopped Then Exit Sub

        Dim lb As AAYhatzeeScoreLabel = CType(sender, Label)
        With lb
            If Not .ScoreAssigned Then
                Select Case .ScoreType
                    Case YhatzeeScoreType.ystNumber
                        .Text = p.YhatzeeNumberScore(CInt(.ScoreValue))
                    Case YhatzeeScoreType.ystKind
                        .Text = p.YhatzeeeOfAKindScore(.ScoreValue)
                    Case YhatzeeScoreType.ystFullHouse
                        .Text = p.YhatzeeeFullHouseScore
                    Case YhatzeeScoreType.ystSmallStraight
                        .Text = p.YhatzeeeSmallStraightScore
                    Case YhatzeeScoreType.ystLargeStraight
                        .Text = p.YhatzeeeLargeStraightScore
                    Case YhatzeeScoreType.ystChance
                        .Text = p.YhatzeeeChanceScore
                    Case YhatzeeScoreType.ystYhatzee
                        .Text = p.YhatzeeeFiveOfAKindScore
                End Select

            End If
        End With

    End Sub

    Private Sub MouseDownLabel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        'do nothing if we're rolling
        If Not p.AllDiceStopped Then Exit Sub

        Dim lb As AAYhatzeeScoreLabel = CType(sender, Label)
        With lb
            If Not .ScoreAssigned Then
                .ScoreAssigned = True

                If CInt(.Text) = 0 Then
                    oWav.PlaySynchronous("ouch")
                Else
                    oWav.PlaySynchronous("applause")
                End If

                Call CalcTotal()
                If AllBoxesFilled() Then
                    cbNewGame.Visible = True
                    cbRoll.Visible = False
                Else
                    p.ClearFreeze()
                    DoTheRoll()
                    pRoll = 1
                End If
            End If
        End With

    End Sub

    Private Sub MouseLeaveLabel(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lb As AAYhatzeeScoreLabel = CType(sender, Label)
        With lb
            If Not .ScoreAssigned Then
                .Text = ""
            End If
        End With

    End Sub

    Private Sub cbNewGame_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbNewGame.Click

        Call StartGame()
    End Sub

    Private Sub DiePanelDieBounced()

        If oRand.Next(100) Mod 2 = 0 Then
            oWav.Play("die1")
        Else
            oWav.Play("die2")
        End If

    End Sub

    Private Sub DiePanelDieFrozen(ByVal bUnfreeze As Boolean)

        If bUnfreeze Then
            oWav.Play("die1")
        Else
            oWav.Play("die2")
        End If

    End Sub


    Private Sub fYhatzee_Disposed(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles MyBase.Disposed

        oWav.Dispose()
    End Sub

    Private Sub fYhatzee_Paint(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Dim b As LinearGradientBrush

        b = New LinearGradientBrush(ClientRectangle, _
            Color.Red, Color.Black, LinearGradientMode.Vertical)

        e.Graphics.FillRectangle(b, ClientRectangle)
    End Sub
End Class


Public Class AALabel
    Inherits Label

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias
        MyBase.OnPaint(e)
    End Sub
End Class

'the label that controls score
Public Class AAYhatzeeScoreLabel
    Inherits AALabel

    Public Sub New()
        MyBase.New()

        'defaults
        BackColor = Color.LightBlue
        ForeColor = Color.Black
        Size = New Size(36, 23)
        TextAlign = ContentAlignment.TopCenter
        BorderStyle = BorderStyle.FixedSingle

    End Sub

    Private FScoreType As YhatzeeScoreType
    Property ScoreType() As YhatzeeScoreType
        Get
            Return FScoreType
        End Get
        Set(ByVal Value As YhatzeeScoreType)
            FScoreType = Value
        End Set
    End Property

    Private FScoreValue As Integer
    Property ScoreValue() As Integer
        Get
            Return FScoreValue
        End Get
        Set(ByVal Value As Integer)
            FScoreValue = Value
        End Set
    End Property

    Private FScoreAssigned As Boolean = False
    Property ScoreAssigned() As Boolean
        Get
            Return FScoreAssigned
        End Get
        Set(ByVal Value As Boolean)
            FScoreAssigned = Value
        End Set
    End Property

End Class