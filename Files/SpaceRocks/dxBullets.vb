Imports Microsoft.DirectX
Imports Microsoft.DirectX.DirectDraw

Namespace dxWorld

    Public Class dxBulletSprite
        Inherits dxSprite

        Private FFrameAliveCount As Integer

        Sub New()
            MyBase.New()
            FBoundingBox = New Rectangle(10, 10, 12, 12)
        End Sub

        Private FVelocity As PointF
        Property Velocity() As PointF
            Get
                Return FVelocity
            End Get
            Set(ByVal Value As PointF)
                FVelocity = Value
            End Set
        End Property

        Public Overrides Sub Move()
            Location = New PointF(Location.X + Velocity.X, Location.Y + Velocity.Y)
            FFrameAliveCount += 1
        End Sub

        ReadOnly Property pFrameAliveCount() As Integer
            Get
                Return FFrameAliveCount
            End Get
        End Property
    End Class

    Class dxBulletCollection

        Private FBullets As ArrayList
        Private FSurface As Surface
        Private FShowBoundingBox As Boolean
        Private FRand As New Random

        Sub New()
            MyBase.New()
            FBullets = New ArrayList
        End Sub

        ReadOnly Property pCount() As Integer
            Get
                Try
                    Return FBullets.Count
                Catch
                    Return 0
                End Try
            End Get
        End Property

        Property pShowBoundingBox() As Boolean
            Get
                Return FShowBoundingBox
            End Get
            Set(ByVal Value As Boolean)

                Dim aBullet As dxBulletSprite

                FShowBoundingBox = Value

                For Each aBullet In FBullets
                    aBullet.pShowBoundingBox = Value
                Next

            End Set
        End Property

        Public Sub RestoreSurfaces(ByVal oDraw As Microsoft.DirectX.DirectDraw.Device)

            Dim oCK As New ColorKey
            oCK.ColorSpaceLowValue = RGB(255, 255, 255)
            oCK.ColorSpaceHighValue = RGB(255, 255, 255)

            Dim oSurf As Microsoft.DirectX.DirectDraw.Surface

            Dim a As Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()

            If Not FSurface Is Nothing Then
                FSurface.Dispose()
                FSurface = Nothing
            End If

            FSurface = New Surface(a.GetManifestResourceStream("SpaceRocks.Bullet.bmp"), _
                New SurfaceDescription, oDraw)

            FSurface.SetColorKey(ColorKeyFlags.SourceDraw, oCK)

        End Sub

        Public Sub Shoot(ByVal p As PointF, ByVal iAngle As Integer)

            If FBullets.Count >= 4 Then Exit Sub

            Dim dx, dy As Single
            Dim aBullet As dxBulletSprite

            aBullet = New dxBulletSprite
            With aBullet
                .pShowBoundingBox = Me.pShowBoundingBox
                .Location = p

                dy = -Math.Sin(iAngle * Math.PI / 180) * 6
                dx = Math.Cos(iAngle * Math.PI / 180) * 6

                .Velocity = New PointF(dx, dy)
                .Move()

                AddHandler .GetSurfaceData, AddressOf GetBulletSurfaceData
            End With
            FBullets.Add(aBullet)

        End Sub

        Private Sub GetBulletSurfaceData(ByVal aSprite As dxSprite, _
            ByRef oSurf As Surface, ByRef oRect As Rectangle)

            oSurf = FSurface
            oRect = New Rectangle(0, 0, 31, 31)

        End Sub

        Public Sub Move()

            Dim aBullet As dxBulletSprite
            Dim i As Integer

            For Each aBullet In FBullets
                aBullet.Move()
            Next

            'remove bullets on the screen too long. 
            'have to use a loop so you don't skip over when deleting
            i = 0
            Do While i < FBullets.Count
                aBullet = FBullets.Item(i)
                If aBullet.pFrameAliveCount >= 100 Then
                    FBullets.Remove(aBullet)
                Else
                    i = i + 1
                End If
            Loop

        End Sub

        Public Sub Draw(ByVal oSurf As Microsoft.DirectX.DirectDraw.Surface)

            Dim aBullet As dxBulletSprite

            For Each aBullet In FBullets
                aBullet.Draw(oSurf)
            Next

        End Sub

        Public Sub BreakRocks(ByVal FRocks As dxRockCollection)

            Dim aBullet As dxBulletSprite
            Dim i As Integer

            'check each bullet to see if it hits a rock
            'have to use a loop so you don't skip over when deleting
            i = 0
            Do While i < FBullets.Count
                aBullet = FBullets.Item(i)
                If FRocks.CollidingWith(aBullet.WorldBoundingBox, bBreakRock:=True) Then
                    FBullets.Remove(aBullet)
                Else
                    i = i + 1
                End If
            Loop

        End Sub

    End Class

End Namespace
