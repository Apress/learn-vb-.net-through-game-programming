Imports Microsoft.DirectX
Imports Microsoft.DirectX.DirectDraw
Imports Microsoft.DirectX.DirectInput

Namespace dxWorld

    'contains all the directx surface objects, and all logic for drawing
    'draws a black screen with text 'hit esc to end'
    Public MustInherit Class dxWorld

        Private FFrm As Form

        Private FNeedToRestore As Boolean = False

        Protected oRand As New Random
        Protected oDraw As Microsoft.DirectX.DirectDraw.Device
        Protected oFront As Microsoft.DirectX.DirectDraw.Surface
        Protected oBack As Microsoft.DirectX.DirectDraw.Surface
        Protected oJoystick As Microsoft.DirectX.DirectInput.Device

        Public Sub New(ByVal f As Form)
            MyBase.New()

            FFrm = f
            FFrm.Cursor.Dispose()     'byebye cursor
            AddHandler FFrm.KeyDown, AddressOf FormKeyDown
            AddHandler FFrm.KeyUp, AddressOf FormKeyUp
            AddHandler FFrm.Disposed, AddressOf FormDispose

            InitializeDirectDraw()
            InitializeJoystick()
            InitializeWorld()

            Do While FFrm.Created
                DrawFrame()
            Loop
        End Sub

        Protected Overridable Sub FormDispose(ByVal sender As Object, _
            ByVal e As System.EventArgs)

            If Not (oJoystick Is Nothing) Then
                oJoystick.Unacquire()
            End If

        End Sub

        ReadOnly Property WorldRectangle() As Rectangle
            Get
                Return New Rectangle(0, 0, WID, HGT)
            End Get
        End Property

        'override for better keyboard handling
        Protected MustOverride Sub FormKeyDown(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.KeyEventArgs)

        'override for better keyboard handling
        Protected Overridable Sub FormKeyUp(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.KeyEventArgs)

            If e.KeyCode = Keys.Escape Then
                FFrm.Close()
            End If
        End Sub

        Private Sub InitializeDirectDraw()

            Dim oSurfaceDesc As New SurfaceDescription
            Dim oSurfaceCaps As New SurfaceCaps
            Dim i As Integer

            oDraw = New Microsoft.DirectX.DirectDraw.Device

            oDraw.SetCooperativeLevel(FFrm, _
                Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.FullscreenExclusive)
            oDraw.SetDisplayMode(WID, HGT, 16, 0, False)

            With oSurfaceDesc
                .SurfaceCaps.PrimarySurface = True
                .SurfaceCaps.Flip = True
                .SurfaceCaps.Complex = True
                .BackBufferCount = 1
                oFront = New Surface(oSurfaceDesc, oDraw)
                oSurfaceCaps.BackBuffer = True
                oBack = oFront.GetAttachedSurface(oSurfaceCaps)
                oBack.ForeColor = Color.White
                .Clear()
            End With

            FNeedToRestore = True

        End Sub

        Private Sub InitializeJoystick()

            Dim oInst As DeviceInstance
            Dim oDOInst As DeviceObjectInstance

            'get the first attached joystick
            For Each oInst In Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly)
                oJoystick = New Microsoft.DirectX.DirectInput.Device(oInst.InstanceGuid)
                Exit For
            Next

            If Not (oJoystick Is Nothing) Then

                oJoystick.SetDataFormat(DeviceDataFormat.Joystick)
                oJoystick.SetCooperativeLevel(FFrm, _
                    Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Exclusive Or _
                    Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground)

                ' Set the numeric range for each axis to +/- 256. 
                For Each oDOInst In oJoystick.Objects
                    If 0 <> (oDOInst.ObjectId And CInt(DeviceObjectTypeFlags.Axis)) Then

                        oJoystick.Properties.SetRange(ParameterHow.ById, _
                            oDOInst.ObjectId, New InputRange(-256, +256))
                    End If
                Next
            End If
        End Sub

        'override to set up your world objects
        Protected MustOverride Sub InitializeWorld()

        'override when bitmaps have to be reloaded
        Protected Overridable Sub RestoreSurfaces()
            oDraw.RestoreAllSurfaces()
        End Sub

        'clears back buffer to black. calls protected sub. flips back to front
        Private Sub DrawFrame()

            If oFront Is Nothing Then Exit Sub

            If Not oDraw.TestCooperativeLevel() Then
                FNeedToRestore = True
                Return
            End If

            If FNeedToRestore Then
                RestoreSurfaces()
                FNeedToRestore = False
            End If

            oBack.ColorFill(0)

            DrawWorldWithinFrame()

            Try
                oFront.Flip(oBack, FlipFlags.DoNotWait)
            Catch oEX As Exception
                Debug.WriteLine(oEX.Message)
            Finally
                Application.DoEvents()
            End Try

        End Sub

        'override. put all your drawing in here.
        Protected Overridable Sub DrawWorldWithinFrame()

            Try
                oBack.ForeColor = Color.White
                oBack.DrawText(10, 10, "Press escape to exit", False)
            Catch oEX As Exception
                Debug.WriteLine(oEX.Message)
            End Try

        End Sub
    End Class

    Public Class dxSpaceRocks
        Inherits dxWorld

        Private FShip As dxShipSprite
        Private FRocks As dxRockCollection
        Private FBullets As dxBulletCollection

        Private FLeftPressed As Boolean = False
        Private FRightPressed As Boolean = False
        Private FUpPressed As Boolean = False
        Private FSpacePressed As Boolean = False

        Public Sub New(ByVal f As Form)
            MyBase.New(f)
        End Sub

        Protected Overrides Sub InitializeWorld()

            Dim oRand As New Random

            FShip = New dxShipSprite
            FShip.Location = New PointF(100, 100)
            FShip.Size = New Size(96, 96)
            FShip.pShowBoundingBox = False

            FRocks = New dxRockCollection
            FRocks.pShowBoundingBox = False

            FBullets = New dxBulletCollection
            FBullets.pShowBoundingBox = False

        End Sub

        Protected Overrides Sub RestoreSurfaces()
            MyBase.RestoreSurfaces()

            FShip.RestoreSurfaces(oDraw)
            FRocks.RestoreSurfaces(oDraw)
            FBullets.RestoreSurfaces(odraw)

        End Sub

        Protected Overrides Sub DrawWorldWithinFrame()

            Dim p As New Point((WID / 2) - 40, 10)

            MyBase.DrawWorldWithinFrame()

            'joysticks don't generate events, so we update the ship
            'based on joystick state each turn
            UpdateShipState()

            FShip.Move()
            FRocks.Move()
            FBullets.Move()

            FBullets.Draw(oBack)
            FShip.Draw(oBack)
            FRocks.Draw(oBack)

            FBullets.BreakRocks(FRocks)

            oBack.ForeColor = Color.White
            Select Case FShip.Status
                Case ShipStatus.ssAlive
                    oBack.DrawText(p.X, p.Y, "Lives Left: " & FShip.LivesLeft, False)
                    If FRocks.CollidingWith(FShip.WorldBoundingBox, bBreakRock:=False) Then
                        FShip.KillMe()
                    End If

                Case ShipStatus.ssDying
                    oBack.DrawText(p.X, p.Y, "Oops.", False)

                Case ShipStatus.ssDead
                    If FShip.LivesLeft = 0 Then
                        oBack.DrawText(p.X, p.Y, "Game Over", False)
                    Else
                        oBack.DrawText(p.X, p.Y, _
                            "Hit SpaceBar to make ship appear in middle of screen", False)
                    End If
            End Select

        End Sub

        Protected Overrides Sub FormKeyDown(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.KeyEventArgs)

            Select Case e.KeyCode
                Case Keys.Left
                    FLeftPressed = True
                Case Keys.Right
                    FRightPressed = True
                Case Keys.Up
                    FUpPressed = True
                Case Keys.Space
                    FSpacePressed = True
                Case Keys.B
                    FShip.pShowBoundingBox = Not FShip.pShowBoundingBox
                    FRocks.pShowBoundingBox = Not FRocks.pShowBoundingBox
                    FBullets.pShowBoundingBox = Not FBullets.pShowBoundingBox
            End Select
        End Sub

        Protected Overrides Sub FormKeyUp(ByVal sender As Object, _
            ByVal e As System.Windows.Forms.KeyEventArgs)

            MyBase.FormKeyUp(sender, e)

            Select Case e.KeyCode
                Case Keys.Left
                    FLeftPressed = False
                Case Keys.Right
                    FRightPressed = False
                Case Keys.Up
                    FUpPressed = False
            End Select
        End Sub

        Private Sub UpdateShipState()

            Dim oState As New JoystickState
            Dim bButtons As Byte()
            Dim b As Byte

            Dim p As PointF

            If Not oJoystick Is Nothing Then

                Try
                    oJoystick.Poll()

                Catch oEX As InputException
                    If TypeOf oEX Is NotAcquiredException Or _
                        TypeOf oEX Is InputLostException Then

                        Try
                            ' Acquire the device.
                            oJoystick.Acquire()
                        Catch
                            Exit Sub
                        End Try
                    End If
                End Try

                Try
                    oState = oJoystick.CurrentJoystickState
                Catch
                    Exit Sub
                End Try

                'ship is turning if x axis movement
                FShip.IsTurningRight = (oState.X > 100) Or FRightPressed
                FShip.IsTurningLeft = (oState.X < -100) Or FLeftPressed
                FShip.ThrustersOn = (oState.Y < -100) Or FUpPressed

                'any button pushed on the joystick will work
                bButtons = oState.GetButtons()
                For Each b In bButtons
                    If (b And &H80) <> 0 Then
                        FSpacePressed = True
                        Exit For
                    End If
                Next

            Else
                FShip.IsTurningRight = FRightPressed
                FShip.IsTurningLeft = FLeftPressed
                FShip.ThrustersOn = FUpPressed
            End If

            If FSpacePressed Then
                Select Case FShip.Status
                    Case ShipStatus.ssDead
                        'center screen
                        FShip.BringMeToLife(WID \ 2 - FShip.Size.Width \ 2, _
                                            HGT \ 2 - FShip.Size.Height \ 2)
                    Case ShipStatus.ssAlive
                        p = FShip.Center
                        p.X = p.X - 16
                        p.Y = p.Y - 16

                        FBullets.Shoot(p, FShip.Angle)
                End Select
                FSpacePressed = False
            End If
        End Sub
    End Class
End Namespace