Imports ShapeTileGames.OldTile

Public Class fConcentration
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
    Friend WithEvents mFile As System.Windows.Forms.MenuItem
    Friend WithEvents oNew As System.Windows.Forms.MenuItem
    Friend WithEvents oDash0 As System.Windows.Forms.MenuItem
    Friend WithEvents oExit As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.oMenu = New System.Windows.Forms.MainMenu
        Me.mFile = New System.Windows.Forms.MenuItem
        Me.oNew = New System.Windows.Forms.MenuItem
        Me.oDash0 = New System.Windows.Forms.MenuItem
        Me.oExit = New System.Windows.Forms.MenuItem
        '
        'oMenu
        '
        Me.oMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mFile})
        '
        'mFile
        '
        Me.mFile.Index = 0
        Me.mFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.oNew, Me.oDash0, Me.oExit})
        Me.mFile.Text = "&File"
        '
        'oNew
        '
        Me.oNew.Index = 0
        Me.oNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.oNew.Text = "&New"
        '
        'oDash0
        '
        Me.oDash0.Index = 1
        Me.oDash0.Text = "-"
        '
        'oExit
        '
        Me.oExit.Index = 2
        Me.oExit.Text = "E&xit"
        '
        'fConcentration
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(386, 221)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Menu = Me.oMenu
        Me.MinimizeBox = False
        Me.Name = "fConcentration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Brain Drain Concentration"

    End Sub

#End Region

    Dim FTiles As ArrayList
    Dim FFirstOneFlipped As ColoredShape
    Private oWav As WavLibrary
    Dim oGT As GameTimer

    Private Sub fConcentration_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        oGT = New GameTimer
        With oGT
            .Dock = DockStyle.Bottom
            .Height = 32
            .Font = New Font("Tahoma", 16, FontStyle.Italic Or FontStyle.Bold)
            .ForeColor = Color.LightGray
            AddHandler .TimesUp, AddressOf TimerDone
            AddHandler .SecondsChanged, AddressOf TimerSeconds
        End With

        Me.Controls.Add(oGT)

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

    Private Sub StartGame()

        Dim oShape As ColoredShape

        Dim oRand As New Random
        Dim i, j, k As Integer
        Dim iCtr As Integer
        Dim p As Point

        Me.Cursor = Cursors.WaitCursor
        oGT.StopTimer()

        'note how two references pointing to the same object
        'me.controls and FTiles.
        If Not FTiles Is Nothing Then
            For Each oShape In FTiles
                Me.Controls.Remove(oShape)
            Next
        End If

        FTiles = New ArrayList

        For i = 0 To 3                          '4 shapes...
            For j = 0 To 3                      '4 colors...
                For k = 0 To 1                  '2 copies
                    'oShape = New ColoredShape(i, j)
                    oShape = ColoredShape.CreateByIndex(i, j)
                    oShape.Backwards = True
                    oShape.Width = 48
                    oShape.Left = (iCtr Mod 8) * oShape.Width
                    oShape.Top = (iCtr \ 8) * oShape.Height
                    AddHandler oShape.Click, AddressOf ShapeClick

                    'adding a reference to two places, the form and to an arraylist
                    FTiles.Add(oShape)
                    Me.Controls.Add(oShape)

                    iCtr += 1
                Next
            Next
        Next

        'swap positions randomly
        For k = 0 To 999
            i = oRand.Next(0, FTiles.Count)
            j = oRand.Next(0, FTiles.Count)
            If i <> j Then
                p = CType(FTiles.Item(i), ColoredShape).Location

                CType(FTiles.Item(i), ColoredShape).Location = _
                    CType(FTiles.Item(j), ColoredShape).Location

                CType(FTiles.Item(j), ColoredShape).Location = p
            End If
        Next

        FFirstOneFlipped = Nothing
        Me.Cursor = Cursors.Default
        oGT.StartTimer()

    End Sub

    Private Sub ShapeClick(ByVal sender As Object, _
      ByVal e As System.EventArgs)

        Dim s As ColoredShape = sender

        s.Backwards = False
        oWav.Play("die1", 100)
        Application.DoEvents()

        If FFirstOneFlipped Is Nothing Then
            FFirstOneFlipped = s
        Else
            If s.Equals(FFirstOneFlipped) Then
                System.Threading.Thread.Sleep(100)          'pause just a sec
                Me.Controls.Remove(s)
                Me.Controls.Remove(FFirstOneFlipped)

                FTiles.Remove(s)
                FTiles.Remove(FFirstOneFlipped)
                oWav.Play("die1", 100)

                If FTiles.Count = 0 Then
                    oGT.StopTimer()
                    oWav.Play("ovation", 100)
                    MsgBox("you win")
                End If
            Else
                System.Threading.Thread.Sleep(500)
                s.Backwards = True
                FFirstOneFlipped.Backwards = True
                oWav.Play("ouch", 100)
            End If
            FFirstOneFlipped = Nothing
        End If

    End Sub

    Private Sub oExit_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles oExit.Click

        Me.Close()
    End Sub

    Private Sub TimerDone(ByVal sender As Object)
        MsgBox("you lose")
    End Sub

    Private Sub TimerSeconds(ByVal sender As Object, ByVal t As TimeSpan)

        If t.TotalSeconds < 60 Then
            CType(sender, GameTimer).ForeColor = Color.Red
        Else
            CType(sender, GameTimer).ForeColor = Color.LightGray
        End If

    End Sub

    Private Sub oNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oNew.Click
        If MsgBox("Restart Game?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, _
            "Restart") = MsgBoxResult.Yes Then

            Call StartGame()
        End If
    End Sub

    Private Sub fConcentration_Closing(ByVal sender As Object, _
        ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If MsgBox("End Game?", MsgBoxStyle.YesNo _
            Or MsgBoxStyle.Question, "Confirm?") = MsgBoxResult.No Then e.Cancel = True

    End Sub
End Class
