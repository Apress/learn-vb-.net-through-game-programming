Imports ShapeTileGames.NewTile

Public Class fLoseYourMind
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
    Friend WithEvents pnTop As System.Windows.Forms.Panel
    Friend WithEvents cbGuess As System.Windows.Forms.Button
    Friend WithEvents pnBottom As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbInstructions As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.pnTop = New System.Windows.Forms.Panel
        Me.cbGuess = New System.Windows.Forms.Button
        Me.pnBottom = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.lbInstructions = New System.Windows.Forms.Label
        Me.pnBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnTop
        '
        Me.pnTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnTop.Location = New System.Drawing.Point(0, 0)
        Me.pnTop.Name = "pnTop"
        Me.pnTop.Size = New System.Drawing.Size(292, 48)
        Me.pnTop.TabIndex = 0
        '
        'cbGuess
        '
        Me.cbGuess.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbGuess.ForeColor = System.Drawing.SystemColors.Control
        Me.cbGuess.Location = New System.Drawing.Point(216, 128)
        Me.cbGuess.Name = "cbGuess"
        Me.cbGuess.Size = New System.Drawing.Size(48, 23)
        Me.cbGuess.TabIndex = 4
        Me.cbGuess.Text = "Guess"
        '
        'pnBottom
        '
        Me.pnBottom.Controls.Add(Me.lbInstructions)
        Me.pnBottom.Controls.Add(Me.Label1)
        Me.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnBottom.Location = New System.Drawing.Point(0, 579)
        Me.pnBottom.Name = "pnBottom"
        Me.pnBottom.Size = New System.Drawing.Size(292, 40)
        Me.pnBottom.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label1.ForeColor = System.Drawing.Color.LightGray
        Me.Label1.Location = New System.Drawing.Point(0, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(292, 16)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Top row indicates Shape/Bottom indicates Color"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbInstructions
        '
        Me.lbInstructions.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbInstructions.ForeColor = System.Drawing.Color.LightGray
        Me.lbInstructions.Location = New System.Drawing.Point(0, 8)
        Me.lbInstructions.Name = "lbInstructions"
        Me.lbInstructions.Size = New System.Drawing.Size(292, 16)
        Me.lbInstructions.TabIndex = 9
        Me.lbInstructions.Text = "Left/Right click to change Shape/Color"
        Me.lbInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fLoseYourMind
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(292, 619)
        Me.Controls.Add(Me.pnBottom)
        Me.Controls.Add(Me.cbGuess)
        Me.Controls.Add(Me.pnTop)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "fLoseYourMind"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lose Your Mind"
        Me.pnBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FSolution As LoseYourMind.TileCollection
    Private FCurrentGuess As LoseYourMind.TileCollectionGuess
    Private FGuesses As ArrayList
    Private oWav As WavLibrary

    Private Const NUMTILES As Integer = 4
    Private Const TILESIZE As Integer = 48

    Private Sub fLoseYourMind_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Dim lb As Label
        Dim i As Integer

        For i = 0 To 9
            lb = New Label
            lb.Text = (i + 1) & "."
            lb.TextAlign = ContentAlignment.MiddleRight
            lb.Width = 24
            lb.Location = New Point(4, HeightFromTurnNumber(i) + TILESIZE - lb.Height)
            lb.ForeColor = Color.LightGray
            lb.BackColor = Color.Transparent
            Me.Controls.Add(lb)
        Next

        SetupWavLibrary()
        StartGame()
    End Sub

    Private Sub SetupWavLibrary()

        oWav = New WavLibrary
        oWav.LoadFromResource("ShapeTileGames.die1.wav", "die1")
        oWav.LoadFromResource("ShapeTileGames.8ping.wav", "8ping")
        oWav.LoadFromResource("ShapeTileGames.ouch.wav", "ouch")
        oWav.LoadFromResource("ShapeTileGames.ovation.wav", "ovation")

    End Sub


    Private Function HeightFromTurnNumber(ByVal iTurn As Integer) As Integer
        Return Me.Height - (iTurn * TILESIZE) - (TILESIZE * 2) - pnBottom.Height
    End Function

    Private Sub ShapeMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim oShape As ColoredShape = sender

        If ((e.Button And MouseButtons.Left) = MouseButtons.Left) Then
            oShape.ToggleShape()
            oWav.Play("die1", 100)
        End If

        If ((e.Button And MouseButtons.Right) = MouseButtons.Right) Then
            oShape.ToggleColor()
            oWav.Play("die1", 100)
        End If
    End Sub

    Private Sub cbGuess_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbGuess.Click

        Dim oGH As LoseYourMind.GuessHintRenderer
        Dim cMsg As String

        FGuesses.Add(FCurrentGuess)
        FCurrentGuess.CheckAgainst(FSolution)

        oGH = New LoseYourMind.GuessHintRenderer(FCurrentGuess)
        oGH.Location = cbGuess.Location
        oGH.Size = New Size(54, 32)
        Me.Controls.Add(oGH)

        If FCurrentGuess.Wins Then
            oWav.Play("ovation", 100)
            ShowSolution()

            cMsg = "Winnah, Winnah, Chicken Dinnah!" & Environment.NewLine
            cMsg &= "Play again?"
            If MsgBox(cMsg, MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "You win") = MsgBoxResult.Yes Then
                Call StartGame()
                Exit Sub
            Else
                Me.Close()
            End If

        Else
            If FGuesses.Count = 10 Then
                oWav.Play("ouch", 100)
                ShowSolution()
                cMsg = "You Lose!" & Environment.NewLine
                cMsg &= "Play again?"
                If MsgBox(cMsg, MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "You lose") = MsgBoxResult.Yes Then
                    Call StartGame()
                    Exit Sub
                Else
                    Me.Close()
                End If

            Else
                oWav.Play("8ping", 100)
            End If
            cbGuess.Top -= TILESIZE
            Call SetupGuess()
        End If

    End Sub

    Private Sub ShowSolution()
        Dim oShape As ColoredShape

        For Each oShape In FSolution
            oShape.Backwards = False
        Next
        cbGuess.Visible = False

    End Sub


    Private Sub DeleteOldGameControls()

        Dim o As Control
        Dim i, iTop As Integer

        'can't use a 'for each' with a remove
        i = 0
        Do While i < Me.Controls.Count
            o = Me.Controls.Item(i)
            If TypeOf o Is LoseYourMind.GuessHintRenderer Then
                Me.Controls.Remove(o)
            ElseIf TypeOf o Is ColoredShape Then
                Me.Controls.Remove(o)
            Else
                i += 1
            End If
        Loop

        i = 0
        Do While i < pnTop.Controls.Count
            o = pnTop.Controls.Item(i)
            If TypeOf o Is ColoredShape Then
                pnTop.Controls.Remove(o)
            Else
                i += 1
            End If
        Loop


    End Sub

    Private Sub StartGame()

        Dim oRand As New Random
        Dim oShape As ColoredShape
        Dim i, iTop As Integer

        DeleteOldGameControls()

        FGuesses = New ArrayList
        cbGuess.Visible = True

        FSolution = New LoseYourMind.TileCollection
        i = 0
        Do
            oShape = New ColoredShape(oRand.Next(0, 4), oRand.Next(0, 4))
            oShape.Backwards = True
            oShape.Width = TILESIZE
            oShape.Left = 32 + (i * oShape.Width)

            FSolution.Add(oShape)
            pnTop.Controls.Add(oShape)
            i += 1
        Loop Until FSolution.Count = NUMTILES

        iTop = HeightFromTurnNumber(0)
        cbGuess.Location = New Point((TILESIZE * NUMTILES) + 40, iTop + 8)

        Call SetupGuess()
    End Sub

    Private Sub SetupGuess()

        Dim oShape As ColoredShape
        Dim oTC As New LoseYourMind.TileCollectionGuess
        Dim o As Control
        Dim i, iTop As Integer

        'create 4 tiles for guessing, put in a guess object.

        'remove clicking ability on all shapes
        For Each o In Me.Controls
            If TypeOf o Is ColoredShape Then
                RemoveHandler o.MouseDown, AddressOf ShapeMouseDown
            End If
        Next

        FCurrentGuess = New LoseYourMind.TileCollectionGuess

        iTop = HeightFromTurnNumber(FGuesses.Count)
        Do
            oShape = New ColoredShape(i, i)
            With oShape
                .Location = New Point(32 + (i * TILESIZE), iTop)
                .Width = TILESIZE
                .Backwards = False

                'copy from guess before
                If FGuesses.Count > 0 Then
                    oTC = FGuesses.Item(FGuesses.Count - 1)
                    .CopyFrom(oTC.Item(i))          'copies shape and color
                End If
                AddHandler .MouseDown, AddressOf ShapeMouseDown
            End With
            Me.Controls.Add(oShape)
            FCurrentGuess.Add(oShape)

            i += 1
        Loop Until i >= NUMTILES

    End Sub
End Class
