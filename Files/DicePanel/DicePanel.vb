Imports System.Math
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace DicePanel

    <ToolboxItem(True)> _
    Public Class DicePanel
        Inherits System.Windows.Forms.Panel

        Private aDice As ArrayList

        Protected FbmStop As Bitmap
        Protected FbmxRot As Bitmap
        Protected FbmyRot As Bitmap
        Protected FRand As New Random

        Private bmBack As Bitmap                 'background bitmap
        Public Event DieBounced()

        Public Sub New()
            MyBase.New()

            Me.SetStyle(ControlStyles.UserPaint, True)
            Me.SetStyle(ControlStyles.DoubleBuffer, True)
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

            Me.BackColor = Color.Black

            Dim a As Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            FbmxRot = New Bitmap(a.GetManifestResourceStream("DicePanel.dicexrot.bmp"))
            FbmyRot = New Bitmap(a.GetManifestResourceStream("DicePanel.diceyrot.bmp"))
            FbmStop = New Bitmap(a.GetManifestResourceStream("DicePanel.dicedone.bmp"))

            'NEW. this used to be a major pain in VB6
            FbmxRot.MakeTransparent(Color.Black)
            FbmyRot.MakeTransparent(Color.Black)
            FbmStop.MakeTransparent(Color.Black)

        End Sub

        'called from form dispose
        Public Overloads Sub Dispose()
            FbmxRot.Dispose()
            FbmyRot.Dispose()
            FbmStop.Dispose()
            bmBack.Dispose()
            MyBase.Dispose()
        End Sub

        Private FNumDice As Integer = 2

        <Description("Number of Dice in the Panel"), _
            Category("Dice"), _
            DefaultValue(2)> _
        Property NumDice() As Integer
            Get
                Return FNumDice
            End Get
            Set(ByVal Value As Integer)
                FNumDice = Value

                'regen dice, but only if done once before, or else dbl init
                If DiceGenerated() Then
                    Dim d As Die
                    GenerateDice()
                    Clear()
                    For Each d In aDice
                        d.DrawDie(bmBack)
                    Next
                    Me.Invalidate()
                End If
            End Set
        End Property

        Private FDebugDrawMode As Boolean = False

        <Description("Draws a Box Around the Die for Collision Debugging"), _
        Category("Dice"), DefaultValue(False)> _
        Property DebugDrawMode() As Boolean
            Get
                Return FDebugDrawMode
            End Get
            Set(ByVal Value As Boolean)
                FDebugDrawMode = Value
            End Set
        End Property

        Private Sub GenerateDice()

            Dim d As Die
            Dim dOld As Die
            Dim bDone As Boolean
            Dim iTry As Integer

            aDice = New ArrayList

            Do While aDice.Count < NumDice
                d = New Die(Me)
                iTry = 0

                Do
                    iTry += 1
                    bDone = True
                    d.InitializeLocation()
                    For Each dOld In aDice
                        If d.Overlapping(dOld) Then
                            bDone = False
                        End If
                    Next
                Loop Until bDone Or iTry > 1000
                aDice.Add(d)
            Loop

        End Sub

        <Description("Summed Result of All the Dice"), Category("Dice")> _
        ReadOnly Property Result() As Integer
            Get
                Dim d As Die
                Dim i As Integer = 0

                For Each d In aDice
                    i += d.Result
                Next
                Return i
            End Get
        End Property

        Private ReadOnly Property AllDiceStopped() As Boolean
            Get
                Dim d As Die
                Dim r As Boolean

                r = True
                For Each d In aDice
                    If d.IsRolling Then
                        r = False
                    End If
                Next

                Return r
            End Get
        End Property

        Public Sub RollDice()

            Dim d As Die

            For Each d In aDice
                d.InitializeRoll()
            Next

            Do
                Clear()     'NEW - clear only once per frame

                For Each d In aDice
                    d.UpdateDiePosition()
                    'HandleCollisions()
                    d.DrawDie(bmBack)
                Next
                HandleCollisions()
                Me.Invalidate()
                Application.DoEvents()
            Loop Until AllDiceStopped

        End Sub

        Private Sub HandleCollisions()

            Dim di As Die
            Dim dj As Die
            Dim i As Integer
            Dim j As Integer

            If NumDice = 1 Then Exit Sub

            'can't use foreach loops here, want to start j loop index AFTER first loop
            For i = 0 To aDice.Count - 2
                For j = i + 1 To aDice.Count - 1
                    di = aDice.Item(i)
                    dj = aDice.Item(j)
                    di.HandleCollision(dj)
                Next
            Next

        End Sub

        Private Sub SetupBackgroundAndDice()
            MakeBackgroundBitmap()

            Dim d As Die
            If Not DiceGenerated() Then
                GenerateDice()
            End If
            For Each d In aDice
                d.DrawDie(bmBack)
            Next

        End Sub

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            MyBase.OnPaint(e)

            'happens in design mode
            If bmBack Is Nothing Then
                Call SetupBackgroundAndDice()
            End If
            e.Graphics.DrawImageUnscaled(bmBack, 0, 0)
        End Sub

        Protected Overrides Sub OnResize(ByVal eventargs As System.EventArgs)
            MyBase.OnResize(eventargs)
            Call SetupBackgroundAndDice()
        End Sub

        Private Sub MakeBackgroundBitmap()
            If Not bmBack Is Nothing Then bmBack.Dispose()
            bmBack = New Bitmap(Me.Width, Me.Height, PixelFormat.Format32bppPArgb)
            Clear()
        End Sub

        Private Sub Clear()

            Dim gr As Graphics

            gr = Graphics.FromImage(bmBack)
            Try
                gr.Clear(Color.Black)
            Finally
                gr.Dispose()
            End Try
        End Sub

        Private Function DiceGenerated() As Boolean
            Return Not (aDice Is Nothing)
        End Function

        Protected Sub OnDieBounced()
            RaiseEvent DieBounced()
        End Sub

        Protected Friend Class Die

            Private Const MAXMOVE As Integer = 5
            Private Enum DieStatus
                dsStopped = 0
                dsRolling = 1
                dsLanding = 2
            End Enum

            Private FRollLoop As Integer

            Private h As Integer = 72
            Private w As Integer = 72
            Private FxPos As Integer
            Private FyPos As Integer

            Private dxDir As Integer         '-MAXMOVE to MAXMOVE
            Private dyDir As Integer         '-MAXMOVE to MAXMOVE, indicates direction moving

            Private FPanel As DicePanel
            Private FStatus As DieStatus = DieStatus.dsLanding

            Public Sub New(ByVal pn As DicePanel)
                MyBase.New()
                FPanel = pn
            End Sub

            Private FFrame As Integer
            Private Property Frame() As Integer
                Get
                    Return FFrame
                End Get
                Set(ByVal Value As Integer)
                    FFrame = Value

                    If FFrame < 0 Then FFrame += 36
                    If FFrame > 35 Then FFrame -= 36
                End Set
            End Property


            Private FResult As Integer       'result of the die, 1-6
            Property Result() As Integer
                Get
                    Return FResult
                End Get
                Set(ByVal Value As Integer)
                    If Value < 1 Or Value > 6 Then
                        Throw New Exception("Invalid Die Value")
                    Else
                        FResult = Value
                    End If
                End Set
            End Property

            Private Property xPos() As Integer
                Get
                    Return FxPos
                End Get
                Set(ByVal Value As Integer)
                    FxPos = Value

                    If FxPos < 0 Then
                        FxPos = 0
                        Call BounceX()
                    End If
                    If FxPos > FPanel.Width - w Then
                        FxPos = FPanel.Width - w
                        Call BounceX()
                    End If
                End Set
            End Property

            Private Property yPos() As Integer
                Get
                    Return FyPos
                End Get
                Set(ByVal Value As Integer)
                    FyPos = Value

                    If FyPos < 0 Then
                        FyPos = 0
                        Call BounceY()
                    End If
                    If FyPos > FPanel.Height - h Then
                        FyPos = FPanel.Height - h
                        Call BounceY()
                    End If
                End Set
            End Property

            Public Sub InitializeLocation()
                Try
                    xPos = FPanel.FRand.Next(0, FPanel.Width - w)
                    yPos = FPanel.FRand.Next(0, FPanel.Height - h)
                Catch oEX As Exception
                    xPos = 0
                    yPos = 0
                End Try
            End Sub

            Public Sub UpdateDiePosition()

                Select Case FStatus
                    Case DieStatus.dsLanding
                        'if landing reduce the frame by 1, regardless of direction
                        Frame -= 1
                    Case DieStatus.dsRolling
                        'frame goes up or down based on y direction
                        Frame += (1 * Sign(dyDir))
                        'NEW - need because other dice might be moving if i've stopped
                    Case DieStatus.dsStopped
                        Exit Sub            'don't move if stopped
                End Select

                'update the position
                xPos += dxDir
                yPos += dyDir

                FRollLoop += 1

                Select Case FStatus
                    Case DieStatus.dsRolling
                        'after 100 frames, check for a small chance that the die will stop rolling
                        If FRollLoop > 100 And FPanel.FRand.Next(1, 100) < 10 Then
                            FStatus = DieStatus.dsLanding
                            FRollLoop = 0

                            Frame = Result * 6
                        End If

                    Case DieStatus.dsLanding
                        'die lands for 6 frames and stops
                        If FRollLoop > 5 Then
                            FStatus = DieStatus.dsStopped
                        End If
                End Select

            End Sub

            Public Sub InitializeRoll()

                Do
                    'initialize the directions, 0/1 no good
                    dxDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1)
                Loop Until Abs(dxDir) > 2
                Do
                    dyDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1)
                Loop Until Abs(dyDir) > 2
                Result = FPanel.FRand.Next(1, 7)      'decide what the result will be

                FRollLoop = 0
                FStatus = DieStatus.dsRolling

            End Sub

            Public Sub DrawDie(ByVal bDest As Bitmap)

                Dim gr As Graphics
                Dim b As Bitmap

                Dim x As Integer = (Frame Mod 6) * w
                Dim y As Integer = (Frame \ 6) * h
                Dim r As New System.Drawing.Rectangle(x, y, w, h)

                'select the correct bitmap based on what the die is doing, and what direction it's going
                If FStatus = DieStatus.dsRolling Then
                    'check quandrant rolling towards based on sign of xdir*ydir
                    If (dxDir * dyDir) > 0 Then
                        b = FPanel.FbmyRot
                    Else
                        b = FPanel.FbmxRot
                    End If
                Else
                    b = FPanel.FbmStop
                End If

                gr = Graphics.FromImage(bDest)
                Try
                    gr.DrawImage(b, xPos, yPos, r, GraphicsUnit.Pixel)
                    If FPanel.DebugDrawMode Then
                        Dim p As New Pen(Color.Yellow)
                        Dim xc, yc As Single

                        xc = xPos + (w \ 2)
                        yc = yPos + (h \ 2)

                        gr.DrawRectangle(p, Me.Rect)
                        gr.DrawLine(p, xc, yc, xc + Sign(dxDir) * (w \ 2), yc + Sign(dyDir) * (h \ 2))
                    End If
                Finally
                    gr.Dispose()
                End Try

            End Sub

            ReadOnly Property IsNotRolling() As Boolean
                Get
                    Return FStatus = DieStatus.dsStopped
                End Get
            End Property

            ReadOnly Property IsRolling() As Boolean
                Get
                    Return Not IsNotRolling
                End Get
            End Property

            ReadOnly Property Rect() As Rectangle
                Get
                    Return New Rectangle(xPos, yPos, w, h)
                End Get
            End Property

            Public Function Overlapping(ByVal d As Die) As Boolean
                Return d.Rect.IntersectsWith(Me.Rect)
            End Function

            Public Sub HandleCollision(ByVal d As Die)
                If Me.Overlapping(d) Then
                    If Abs(d.yPos - Me.yPos) <= Abs(d.xPos - Me.xPos) Then
                        HandleBounceX(d)
                    Else
                        HandleBounceY(d)
                    End If
                End If
            End Sub

            Private Sub HandleBounceX(ByVal d As Die)

                Dim dLeft As Die
                Dim dRight As Die

                If Me.xPos < d.xPos Then
                    dLeft = Me
                    dRight = d
                Else
                    dLeft = d
                    dRight = Me
                End If

                'moving toward each other
                If dLeft.dxDir > 0 And dRight.dxDir < 0 Then
                    Me.BounceX()
                    d.BounceX()
                    Exit Sub
                End If

                'moving right, left one caught up to right one
                If dLeft.dxDir > 0 And dRight.dxDir > 0 Then
                    dLeft.BounceX()
                    Exit Sub
                End If

                'moving left, right one caught up to left one
                If dLeft.dxDir < 0 And dRight.dxDir < 0 Then
                    dRight.BounceX()
                End If

            End Sub

            Private Sub HandleBounceY(ByVal d As Die)

                Dim dTop As Die
                Dim dBot As Die

                If Me.yPos < d.yPos Then
                    dTop = Me
                    dBot = d
                Else
                    dTop = d
                    dBot = Me
                End If

                If dTop.dyDir > 0 And dBot.dyDir < 0 Then
                    Me.BounceY()
                    d.BounceY()
                    Exit Sub
                End If

                'moving down, top one caught up to bottom one
                If dTop.dyDir > 0 And dBot.dyDir > 0 Then
                    dTop.BounceY()
                    Exit Sub
                End If

                'moving left, bottom one caught up to top one
                If dTop.dyDir < 0 And dBot.dyDir < 0 Then
                    dBot.BounceY()
                End If

            End Sub

            Private Sub BounceX()
                dxDir = -dxDir
                FPanel.OnDieBounced()
            End Sub

            Private Sub BounceY()
                dyDir = -dyDir
                FPanel.OnDieBounced()
            End Sub

        End Class

    End Class
End Namespace