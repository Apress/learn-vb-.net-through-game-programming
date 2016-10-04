Imports Microsoft.DirectX
Imports Microsoft.DirectX.DirectDraw

Namespace dxWorld

    Public Enum ShipStatus
        ssAlive = 0
        ssDying = 1
        ssDead = 2
    End Enum

    Public Class dxShipSprite
        Inherits dxSprite

        Private FShipSurfaceOff As Microsoft.DirectX.DirectDraw.Surface
        Private FShipSurfaceOn As Microsoft.DirectX.DirectDraw.Surface
        Private FShipSurfaceBoom As Microsoft.DirectX.DirectDraw.Surface

        Private FSkipFrame As Integer
        Private FAngle As Integer
        Private FLivesLeft As Integer = 3
        Private oRand As New Random

        Const MAXA As Single = 3.5
        Const MAXV As Single = 4.0

        Public Sub New()
            MyBase.new()
            FBoundingBox = New Rectangle(27, 28, 42, 42)
            AddHandler Me.GetSurfaceData, AddressOf GetShipSurfaceData
        End Sub

        Private FIsTurningLeft As Boolean
        Property IsTurningLeft() As Boolean
            Get
                Return FIsTurningLeft
            End Get
            Set(ByVal Value As Boolean)
                FIsTurningLeft = Value
            End Set
        End Property

        Private FIsTurningRight As Boolean
        Property IsTurningRight() As Boolean
            Get
                Return FIsTurningRight
            End Get
            Set(ByVal Value As Boolean)
                FIsTurningRight = Value
            End Set
        End Property

        Private FThrustersOn As Boolean
        Property ThrustersOn() As Boolean
            Get
                Return FThrustersOn
            End Get
            Set(ByVal Value As Boolean)
                FThrustersOn = Value
            End Set
        End Property

        Private FStatus As ShipStatus
        ReadOnly Property Status() As ShipStatus
            Get
                Return FStatus
            End Get
        End Property

        Public Sub KillMe()
            If FStatus = ShipStatus.ssAlive Then
                Frame = -1       'reset frame to 0 for explosion
                FStatus = ShipStatus.ssDying
                FLivesLeft -= 1
            End If
        End Sub

        Public Sub BringMeToLife(ByVal x As Integer, ByVal y As Integer)
            If FLivesLeft > 0 Then
                Me.Frame = 0
                Me.Location = New PointF(x, y)
                Me.Velocity = New PointF(0, 0)
                FStatus = ShipStatus.ssAlive
            End If
        End Sub

        ReadOnly Property LivesLeft() As Integer
            Get
                Return FLivesLeft
            End Get
        End Property

        Private FVelocity As PointF
        Property Velocity() As PointF
            Get
                Return FVelocity
            End Get
            Set(ByVal Value As PointF)
                FVelocity = Value

                If FVelocity.X < -MAXV Then FVelocity.X = -MAXV
                If FVelocity.X > MAXV Then FVelocity.X = MAXV

                If FVelocity.Y < -MAXV Then FVelocity.Y = -MAXV
                If FVelocity.Y > MAXV Then FVelocity.Y = MAXV

            End Set
        End Property

        Private FAcceleration As Single = 0
        Property Acceleration() As Single
            Get
                Return FAcceleration
            End Get
            Set(ByVal Value As Single)
                FAcceleration = Value

                If FAcceleration < 0 Then FAcceleration = 0
                If FAcceleration > MAXA Then FAcceleration = MAXA
            End Set
        End Property

        Private Sub Turn()
            Dim iTemp As Integer = Me.Frame

            If Not Me.Status = ShipStatus.ssAlive Then Exit Sub

            'not moving!
            If IsTurningLeft And IsTurningRight Then
                Exit Sub
            End If

            If IsTurningRight Then
                iTemp -= 1
                If iTemp < 0 Then iTemp += 24
            ElseIf IsTurningLeft Then
                iTemp = (iTemp + 1) Mod 24
            End If

            Frame = iTemp

        End Sub

        Public Overrides Sub Move()

            Dim dx, dy As Single

            'we're only moving every x frames
            FSkipFrame = (FSkipFrame + 1) Mod 1000
            If FSkipFrame Mod 3 = 0 Then

                Select Case Me.Status
                    Case ShipStatus.ssAlive
                        Turn()

                        If ThrustersOn Then
                            Acceleration += 1

                            dy = -Math.Sin(FAngle * Math.PI / 180) * Acceleration
                            dx = Math.Cos(FAngle * Math.PI / 180) * Acceleration

                            Velocity = New PointF(Velocity.X + dx, Velocity.Y + dy)
                        Else
                            Acceleration = 0
                        End If
                    Case ShipStatus.ssDying
                        Frame += 1

                        Velocity = New PointF(0, 0)
                        Acceleration = 0

                        'we're done drawing the boom
                        If Frame >= 6 Then
                            FStatus = ShipStatus.ssDead
                        End If

                    Case ShipStatus.ssDead
                        'nothing
                End Select

            End If

            Location = New PointF(Location.X + Velocity.X, Location.Y + Velocity.Y)

        End Sub

        ReadOnly Property Angle() As Integer
            Get
                Return FAngle
            End Get
        End Property

        'frame tied to Angle
        Overrides Property Frame() As Integer
            Get
                Return MyBase.Frame
            End Get
            Set(ByVal Value As Integer)
                MyBase.Frame = Value
                FAngle = Frame * 15
            End Set
        End Property

        'we can keep surfaces in the ship sprite class because there's only one of them
        Public Sub RestoreSurfaces(ByVal oDraw As Microsoft.DirectX.DirectDraw.Device)

            Dim oCK As New ColorKey

            Dim a As Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()

            If Not FShipSurfaceOff Is Nothing Then
                FShipSurfaceOff.Dispose()
                FShipSurfaceOff = Nothing
            End If

            FShipSurfaceOff = New Surface(a.GetManifestResourceStream( _
               "SpaceRocks.ShipNoFire.bmp"), New SurfaceDescription, oDraw)
            FShipSurfaceOff.SetColorKey(ColorKeyFlags.SourceDraw, oCK)

            If Not FShipSurfaceOn Is Nothing Then
                FShipSurfaceOn.Dispose()
                FShipSurfaceOn = Nothing
            End If

            FShipSurfaceOn = New Surface(a.GetManifestResourceStream( _
               "SpaceRocks.ShipFire.bmp"), New SurfaceDescription, oDraw)
            FShipSurfaceOn.SetColorKey(ColorKeyFlags.SourceDraw, oCK)

            If Not FShipSurfaceBoom Is Nothing Then
                FShipSurfaceBoom.Dispose()
                FShipSurfaceBoom = Nothing
            End If

            FShipSurfaceBoom = New Surface(a.GetManifestResourceStream( _
               "SpaceRocks.Boom.bmp"), New SurfaceDescription, oDraw)
            FShipSurfaceBoom.SetColorKey(ColorKeyFlags.SourceDraw, oCK)

        End Sub

        Private Sub GetShipSurfaceData(ByVal aSprite As dxSprite, _
            ByRef oSurf As Surface, ByRef oRect As Rectangle)

            Dim aShip As dxShipSprite = CType(aSprite, dxShipSprite)

            Select Case aShip.Status
                Case ShipStatus.ssDead
                    oSurf = Nothing

                Case ShipStatus.ssDying
                    oSurf = FShipSurfaceBoom

                Case ShipStatus.ssAlive

                    If aShip.ThrustersOn AndAlso _
                        oRand.Next(0, Integer.MaxValue) Mod 10 <> 0 Then

                        oSurf = FShipSurfaceOn
                    Else
                        oSurf = FShipSurfaceOff
                    End If

            End Select

            oRect = New Rectangle((aShip.Frame Mod 6) * 96, (aShip.Frame \ 6) * 96, 96, 96)

        End Sub

    End Class

End Namespace