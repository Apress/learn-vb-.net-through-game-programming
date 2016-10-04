Imports System.Math
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace DicePanelNew

    <ToolboxItem(True)> _
    Public Class DicePanelNew
        Inherits System.Windows.Forms.Panel

        Private aDice As ArrayList

        Protected FbStop As Bitmap
        Protected FbxRot As Bitmap
        Protected FbyRot As Bitmap
        Protected FbHalo As Bitmap
        Protected FRand As New Random

        Private bBack As Bitmap                 'background bitmap
        Public Event DieBounced()
        Public Event DieFrozen(ByVal bUnFreeze As Boolean)

        Public Sub New()
            MyBase.New()

            Me.SetStyle(ControlStyles.UserPaint, True)
            Me.SetStyle(ControlStyles.DoubleBuffer, True)
            Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

            Me.BackColor = Color.Black

            Dim a As Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            FbxRot = New Bitmap(a.GetManifestResourceStream("DicePanelNew.dicexrot.bmp"))
            FbyRot = New Bitmap(a.GetManifestResourceStream("DicePanelNew.diceyrot.bmp"))
            FbStop = New Bitmap(a.GetManifestResourceStream("DicePanelNew.dicedone.bmp"))
            'new
            FbHalo = New Bitmap(a.GetManifestResourceStream("DicePanelNew.diehalo.bmp"))

            FbxRot.MakeTransparent(Color.Black)
            FbyRot.MakeTransparent(Color.Black)
            FbStop.MakeTransparent(Color.Black)
            'new
            FbHalo.MakeTransparent(Color.Black)

        End Sub

        Public Overloads Sub Dispose()
            FbxRot.Dispose()
            FbyRot.Dispose()
            FbStop.Dispose()
            bBack.Dispose()
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
                        d.DrawDie(bBack)
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

        'new
        Private FClickToFreeze As Boolean

        'new
        <Description("Allows user to click dice to lock their movement"), _
        Category("Dice"), DefaultValue(False)> _
        Property ClickToFreeze() As Boolean
            Get
                Return FClickToFreeze
            End Get
            Set(ByVal Value As Boolean)
                FClickToFreeze = Value
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

        'new, changed visibility
        Public ReadOnly Property AllDiceStopped() As Boolean
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

            'don't roll if all frozen
            If AllDiceFrozen() Then Exit Sub

            For Each d In aDice
                d.InitializeRoll()
            Next

            Do
                Clear()
                For Each d In aDice
                    d.UpdateDiePosition()
                    d.DrawDie(bBack)
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
                d.DrawDie(bBack)
            Next

        End Sub

        Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
            MyBase.OnPaint(e)

            'happens in design mode
            If bBack Is Nothing Then
                Call SetupBackgroundAndDice()
            End If
            e.Graphics.DrawImageUnscaled(bBack, 0, 0)
        End Sub

        Protected Overrides Sub OnResize(ByVal eventargs As System.EventArgs)
            MyBase.OnResize(eventargs)
            Call SetupBackgroundAndDice()
        End Sub

        'new
        Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

            Dim d As Die
            Dim bFound As Boolean = False

            If e.Button = MouseButtons.Left Then
                If ClickToFreeze Then

                    For Each d In aDice
                        If d.ClickedOn(e.X, e.Y) Then
                            d.Frozen = Not d.Frozen
                            bFound = True
                            Exit For
                        End If
                    Next

                    If bFound Then
                        Clear()
                        For Each d In aDice
                            d.DrawDie(bBack)
                        Next
                        Invalidate()
                    End If

                    Exit Sub    'don't run mybase if clicktofrezze
                End If
            End If

            MyBase.OnMouseDown(e)
        End Sub

        Private Sub MakeBackgroundBitmap()
            If Not bBack Is Nothing Then bBack.Dispose()
            bBack = New Bitmap(Me.Width, Me.Height, PixelFormat.Format32bppPArgb)
            Clear()
        End Sub

        Private Sub Clear()

            Dim gr As Graphics

            gr = Graphics.FromImage(bBack)
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

        Protected Sub OnDieFrozen(ByVal bUnFreeze As Boolean)
            RaiseEvent DieFrozen(bUnFreeze)
        End Sub

        'new
        'don't roll if all frozen
        Private Function AllDiceFrozen() As Boolean

            Dim d As Die
            If ClickToFreeze Then

                For Each d In aDice
                    If Not d.Frozen Then
                        Return False
                    End If
                Next
                Return True

            End If

        End Function

        'new
        Public Sub ClearFreeze()
            Dim d As Die

            If ClickToFreeze Then

                For Each d In aDice
                    If d.Frozen Then
                        d.Frozen = False
                    End If
                Next
            End If

        End Sub

        'the score for the numeric 1-6 categories in Y
        Public Function YhatzeeNumberScore(ByVal iNum As Integer) As Integer

            Dim d As Die
            Dim iTot As Integer = 0

            For Each d In aDice
                If d.Result = iNum Then
                    iTot += iNum
                End If
            Next
            Return iTot

        End Function

        Public Function YhatzeeeOfAKindScore(ByVal NumofAKind As Integer) As Integer

            Dim d As Die
            Dim iOccur(6) As Integer
            Dim i As Integer

            For Each d In aDice
                iOccur(d.Result) += 1
            Next

            For i = 0 To 6
                If iOccur(i) >= NumofAKind Then
                    Return Me.Result
                End If
            Next

        End Function

        Public Function YhatzeeeFiveOfAKindScore() As Integer

            Const SCORE As Integer = 50

            Dim d As Die
            Dim iOccur(6) As Integer
            Dim i As Integer

            For Each d In aDice
                iOccur(d.Result) += 1
            Next

            For i = 0 To 6
                If iOccur(i) >= 5 Then
                    Return SCORE
                End If
            Next

        End Function

        Public Function YhatzeeeChanceScore() As Integer
            Return Me.Result
        End Function

        Public Function YhatzeeeSmallStraightScore() As Integer

            Const SCORE As Integer = 30
            Dim d As Die
            Dim iOccur(6) As Integer
            Dim i As Integer

            For Each d In aDice
                iOccur(d.Result) += 1
            Next

            If iOccur(1) >= 1 And iOccur(2) >= 1 And iOccur(3) >= 1 And iOccur(4) >= 1 Then
                Return SCORE
            End If

            If iOccur(2) >= 1 And iOccur(3) >= 1 And iOccur(4) >= 1 And iOccur(5) >= 1 Then
                Return SCORE
            End If

        End Function

        Public Function YhatzeeeLargeStraightScore() As Integer

            Const SCORE As Integer = 40
            Dim d As Die
            Dim iOccur(6) As Integer
            Dim i As Integer

            For Each d In aDice
                iOccur(d.Result) += 1
            Next

            If iOccur(1) = 1 And iOccur(2) = 1 And iOccur(3) = 1 And iOccur(4) = 1 And iOccur(5) = 1 Then
                Return SCORE
            End If

            If iOccur(2) = 1 And iOccur(3) = 1 And iOccur(4) = 1 And iOccur(5) = 1 And iOccur(6) = 1 Then
                Return SCORE
            End If

        End Function

        Public Function YhatzeeeFullHouseScore() As Integer

            Const SCORE As Integer = 25
            Dim d As Die
            Dim iOccur(6) As Integer
            Dim i As Integer
            Dim bPair As Boolean
            Dim bTrip As Boolean


            For Each d In aDice
                iOccur(d.Result) += 1
            Next

            For i = 0 To 6
                If iOccur(i) = 2 Then
                    bPair = True
                ElseIf iOccur(i) = 3 Then
                    bTrip = True
                End If
            Next

            If bPair And bTrip Then
                Return SCORE
            End If

        End Function

        Private Class Die

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

            Private FPanel As DicePanelNew
            Private FStatus As DieStatus = DieStatus.dsLanding

            Public Sub New(ByVal pn As DicePanelNew)
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

            'NEW - determines if die is 'locked'
            Private FFrozen As Boolean
            Property Frozen() As Boolean
                Get
                    Return FFrozen
                End Get
                Set(ByVal Value As Boolean)
                    FFrozen = Value
                    FPanel.OnDieFrozen(FFrozen)
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

                Select Case pStatus
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

                Select Case pStatus
                    Case DieStatus.dsRolling
                        'after 75 frames, check for a small chance that the die will stop rolling
                        'reduced to 75 frames from 100 for yhatzee, could be a property for user-customization
                        If FRollLoop > 75 And FPanel.FRand.Next(1, 100) < 10 Then
                            pStatus = DieStatus.dsLanding
                            FRollLoop = 0

                            Frame = Result * 6
                        End If

                    Case DieStatus.dsLanding
                        'die lands for 6 frames and stops
                        If FRollLoop > 5 Then
                            pStatus = DieStatus.dsStopped
                        End If
                End Select

            End Sub

            'new, done for directional changing, collision but
            Private Property pStatus() As DieStatus
                Get
                    Return FStatus
                End Get
                Set(ByVal Value As DieStatus)
                    FStatus = Value
                    If Value = DieStatus.dsStopped Then
                        dxDir = 0       'stop direction
                        dyDir = 0

                    End If
                End Set
            End Property

            Public Sub InitializeRoll()

                'new
                If Not Frozen Then
                    Do
                        dxDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1)
                    Loop Until Abs(dxDir) > 2
                    Do
                        dyDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1)
                    Loop Until Abs(dyDir) > 2
                    Result = FPanel.FRand.Next(1, 7)      'decide what the result will be

                    FRollLoop = 0
                    pStatus = DieStatus.dsRolling
                Else
                    pStatus = DieStatus.dsStopped
                End If
            End Sub

            Public Sub DrawDie(ByVal bDest As Bitmap)

                Dim gr As Graphics
                Dim b As Bitmap

                Dim x As Integer = (Frame Mod 6) * w
                Dim y As Integer = (Frame \ 6) * h
                Dim r As New System.Drawing.Rectangle(x, y, w, h)

                'select the correct bitmap based on what the die is doing, and what direction it's going
                If pStatus = DieStatus.dsRolling Then
                    'check quandrant rolling towards based on sign of xdir*ydir
                    If (dxDir * dyDir) > 0 Then
                        b = FPanel.FbyRot
                    Else
                        b = FPanel.FbxRot
                    End If
                Else
                    b = FPanel.FbStop
                End If

                gr = Graphics.FromImage(bDest)
                Try
                    'new
                    If Frozen Then
                        gr.DrawImage(FPanel.FbHalo, xPos, yPos, New Rectangle(0, 0, w, h), GraphicsUnit.Pixel)
                    End If

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
                    Return pStatus = DieStatus.dsStopped
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
                If dLeft.dxDir >= 0 And dRight.dxDir <= 0 Then
                    Me.BounceX()
                    d.BounceX()
                    Exit Sub
                End If

                'moving right, left one caught up to right one
                If dLeft.dxDir > 0 And dRight.dxDir >= 0 Then
                    dLeft.BounceX()
                    Exit Sub
                End If

                'moving left, right one caught up to left one
                If dLeft.dxDir <= 0 And dRight.dxDir < 0 Then
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

                If dTop.dyDir >= 0 And dBot.dyDir <= 0 Then
                    Me.BounceY()
                    d.BounceY()
                    Exit Sub
                End If

                'moving down, top one caught up to bottom one
                If dTop.dyDir > 0 And dBot.dyDir >= 0 Then
                    dTop.BounceY()
                    Exit Sub
                End If

                'moving left, bottom one caught up to top one
                If dTop.dyDir <= 0 And dBot.dyDir < 0 Then
                    dBot.BounceY()
                End If

            End Sub

            Private Sub BounceX()
                dxDir = -dxDir

                'no sound if not moving
                If pStatus <> DieStatus.dsStopped Then
                    FPanel.OnDieBounced()
                End If
            End Sub

            Private Sub BounceY()
                dyDir = -dyDir

                'no sound if not moving
                If pStatus <> DieStatus.dsStopped Then
                    FPanel.OnDieBounced()
                End If
            End Sub

            'new
            Public Function ClickedOn(ByVal x As Integer, ByVal y As Integer) As Boolean
                Return Me.Rect.Contains(x, y)
            End Function

        End Class

    End Class
End Namespace