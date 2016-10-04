Imports ShapeTileGames.NewTile

Public Class fDeductile
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
    Friend WithEvents oMenu As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents mNew As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents mExit As System.Windows.Forms.MenuItem
    Friend WithEvents lbClues As System.Windows.Forms.ListBox
    Friend WithEvents lbInstructions As System.Windows.Forms.Label
    Friend WithEvents cbGuess As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.oMenu = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.mNew = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.mExit = New System.Windows.Forms.MenuItem
        Me.lbClues = New System.Windows.Forms.ListBox
        Me.lbInstructions = New System.Windows.Forms.Label
        Me.cbGuess = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'oMenu
        '
        Me.oMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mNew, Me.MenuItem2, Me.mExit})
        Me.MenuItem1.Text = "&File"
        '
        'mNew
        '
        Me.mNew.Index = 0
        Me.mNew.Text = "&New"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.Text = "-"
        '
        'mExit
        '
        Me.mExit.Index = 2
        Me.mExit.Text = "E&xit"
        '
        'lbClues
        '
        Me.lbClues.AllowDrop = True
        Me.lbClues.BackColor = System.Drawing.Color.Black
        Me.lbClues.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lbClues.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbClues.ForeColor = System.Drawing.Color.LightGray
        Me.lbClues.ItemHeight = 16
        Me.lbClues.Location = New System.Drawing.Point(144, 32)
        Me.lbClues.Name = "lbClues"
        Me.lbClues.Size = New System.Drawing.Size(296, 272)
        Me.lbClues.TabIndex = 0
        '
        'lbInstructions
        '
        Me.lbInstructions.ForeColor = System.Drawing.Color.LightGray
        Me.lbInstructions.Location = New System.Drawing.Point(176, 312)
        Me.lbInstructions.Name = "lbInstructions"
        Me.lbInstructions.Size = New System.Drawing.Size(224, 32)
        Me.lbInstructions.TabIndex = 1
        Me.lbInstructions.Text = "Drag and Drop tiles to switch positions. Right button drag swaps color only"
        Me.lbInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbGuess
        '
        Me.cbGuess.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbGuess.ForeColor = System.Drawing.SystemColors.Control
        Me.cbGuess.Location = New System.Drawing.Point(16, 320)
        Me.cbGuess.Name = "cbGuess"
        Me.cbGuess.Size = New System.Drawing.Size(64, 23)
        Me.cbGuess.TabIndex = 2
        Me.cbGuess.Text = "Guess"
        '
        'fDeductile
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(454, 359)
        Me.Controls.Add(Me.cbGuess)
        Me.Controls.Add(Me.lbInstructions)
        Me.Controls.Add(Me.lbClues)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Menu = Me.oMenu
        Me.Name = "fDeductile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DeducTile Reasoning"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private oGT As GameTimer
    Private FTiles As ArrayList
    Private FPuzGen As DeductileReasoning.PuzzleGenerator

    Private FDragRect As Rectangle
    Private FDragShape As ColoredShape
    Private FDidRight As Boolean

    Private Sub mExit_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles mExit.Click
        Me.Close()
    End Sub

    Private Sub StartGame()

        Dim oShape As ColoredShape

        Dim oRand As New Random
        Dim i As Integer

        Me.Cursor = Cursors.WaitCursor
        oGT.StopTimer()

        If Not FTiles Is Nothing Then
            For Each oShape In FTiles
                Me.Controls.Remove(oShape)
            Next
        End If

        FTiles = New ArrayList

        For i = 0 To 3
            oShape = New ColoredShape(i, i)
            With oShape
                .Backwards = False
                .Width = 64
                .Location = New Point(16, 32 + (i * 64))
                .AllowDrop = True
                AddHandler .MouseDown, AddressOf ShapeMouseDown
                AddHandler .MouseMove, AddressOf ShapeMouseMove
                AddHandler .MouseUp, AddressOf ShapeMouseUp
                AddHandler .DragOver, AddressOf ShapeDragOver
                AddHandler .DragDrop, AddressOf ShapeDragDrop
            End With

            'adding a reference to two places, the form and to an arraylist
            FTiles.Add(oShape)
            Me.Controls.Add(oShape)

        Next

        FPuzGen = New DeductileReasoning.PuzzleGenerator
        FPuzGen.PopulateListBox(lbClues)

        oGT.StartAt = New TimeSpan(0, 3, 0)
        oGT.StartTimer()
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub fDeductile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        oGT = New GameTimer
        With oGT
            .Dock = DockStyle.Top
            .Height = 32
            .Font = New Font("Tahoma", 16, FontStyle.Italic Or FontStyle.Bold)
            .ForeColor = Color.LightGray
            .StartAt = New TimeSpan(0, 3, 0)
            AddHandler .TimesUp, AddressOf TimerDone
            AddHandler .SecondsChanged, AddressOf TimerSeconds
        End With

        Me.Controls.Add(oGT)
        StartGame()
    End Sub

    Private Sub TimerDone(ByVal sender As Object)
        If MsgBox("You lose, play again?", _
            MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Try Again") = MsgBoxResult.Yes Then

            StartGame()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub TimerSeconds(ByVal sender As Object, ByVal t As TimeSpan)

        If t.TotalSeconds <= 10 Then
            CType(sender, GameTimer).ForeColor = Color.Red
        Else
            CType(sender, GameTimer).ForeColor = Color.LightGray
        End If

    End Sub

    Private Sub mNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mNew.Click

        If MsgBox("Restart Game?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, _
            "Restart") = MsgBoxResult.Yes Then

            Call StartGame()
        End If

    End Sub

    Private Sub ShapeMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim dragSize As Size = SystemInformation.DragSize

        FDragRect = New Rectangle(New Point(e.X - (dragSize.Width / 2), _
                                            e.Y - (dragSize.Height / 2)), dragSize)


    End Sub

    Private Sub ShapeMouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        FDragRect = Rectangle.Empty
        FDragShape = Nothing

    End Sub

    Private Sub ShapeMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If ((e.Button And MouseButtons.Left) = MouseButtons.Left) Or _
            ((e.Button And MouseButtons.Right) = MouseButtons.Right) Then

            ' If the mouse moves outside the rectangle, start the drag.
            If (Rectangle.op_Inequality(FDragRect, Rectangle.Empty) And _
                Not FDragRect.Contains(e.X, e.Y)) Then

                FDidRight = ((e.Button And MouseButtons.Right) = MouseButtons.Right)
                FDragShape = sender

                sender.DoDragDrop(sender, DragDropEffects.All)

            End If
        End If
    End Sub

    Private Sub ShapeDragOver(ByVal sender As Object, ByVal e As DragEventArgs)
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub ShapeDragDrop(ByVal sender As Object, ByVal e As DragEventArgs)

        Dim oDest As ColoredShape = CType(sender, ColoredShape)

        If oDest.Equals(FDragShape) Then Exit Sub

        If Not FDidRight Then
            'left drag, simply swap positions
            Dim p As Point

            p = FDragShape.Location
            FDragShape.Location = oDest.Location
            oDest.Location = p
        Else
            'right drag, swap colors
            Dim c As Color

            c = FDragShape.Color
            FDragShape.Color = oDest.Color
            oDest.Color = c

            oDest.Invalidate()
            FDragShape.Invalidate()
        End If
    End Sub

    Private Sub fDeductile_Closing(ByVal sender As Object, _
        ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If oGT.Enabled Then
            If MsgBox("End Game?", MsgBoxStyle.YesNo _
                Or MsgBoxStyle.Question, "Confirm?") = MsgBoxResult.No Then e.Cancel = True
        End If

    End Sub

    Private Sub cbGuess_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles cbGuess.Click

        Dim f As ColoredShape = FTiles.Item(0)

        'FTiles.Sort()           'calls compareTo
        FTiles.Sort()

        If FPuzGen.IsSolution(FTiles.Item(0), FTiles.Item(1), _
            FTiles.Item(2), FTiles.Item(3)) Then

            oGT.StopTimer()
            If MsgBox("You win, play again?", _
                MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Try Again") = MsgBoxResult.Yes Then
                StartGame()
            Else
                Me.Close()
            End If
        Else
            oGT.AddTime(New TimeSpan(0, 0, -5))
        End If
    End Sub

End Class
