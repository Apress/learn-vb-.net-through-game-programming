Imports Microsoft.DirectX
Imports Microsoft.DirectX.DirectDraw

Namespace dxWorld

    Public Enum dxRockSize
        rsLarge = 0
        rsMed = 1
        rsSmall = 2
    End Enum

    Public Class dxRockSprite
        Inherits dxSprite

        Public Event RockBroken(ByVal aRock As dxRockSprite)

        Const MAXV As Single = 7.0

        Private FFrameCount As Integer
        Private FRockSize As dxRockSize
        Private FRotSpeed As Integer        '0,1 or 2
        Private FAlternateModel As Boolean          'first or second graphic
        Private FSpinReverse As Boolean
        Private FDir As Integer = 1

        Property pAlternateModel() As Boolean
            Get
                Return FAlternateModel
            End Get
            Set(ByVal Value As Boolean)
                FAlternateModel = Value
            End Set
        End Property

        Property pSpinReverse() As Boolean
            Get
                Return FSpinReverse
            End Get
            Set(ByVal Value As Boolean)
                FSpinReverse = Value
                FDir = IIf(Value, -1, 1)
            End Set
        End Property

        Property pRockSize() As dxRockSize
            Get
                Return FRockSize
            End Get
            Set(ByVal Value As dxRockSize)
                FRockSize = Value

                Select Case FRockSize
                    Case dxRockSize.rsLarge
                        FBoundingBox = New Rectangle(8, 21, 78, 60)
                    Case dxRockSize.rsMed
                        FBoundingBox = New Rectangle(5, 17, 52, 40)
                    Case dxRockSize.rsSmall
                        FBoundingBox = New Rectangle(3, 7, 26, 20)
                End Select
            End Set
        End Property

        Property pRotSpeed() As Integer
            Get
                Return FRotSpeed
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Or Value > 2 Then
                    Throw New Exception("invalid rock rotation speed (0-2)")
                Else
                    FRotSpeed = Value
                End If
            End Set
        End Property

        Public Overrides Sub Move()

            FFrameCount += 1

            If FFrameCount Mod 3 <= pRotSpeed Then
                Frame = (Frame + FDir)
                If Frame < 0 Then Frame += 36
                If Frame > 35 Then Frame -= 36
            End If

            Location = New PointF(Location.X + Velocity.X, Location.Y + Velocity.Y)
        End Sub

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

        'runs when rock shot
        Public Sub Break()
            'notify the collection so he can split me into 2
            RaiseEvent RockBroken(Me)
        End Sub

    End Class

    Class dxRockCollection

        Private FRocks As ArrayList
        Private FRockSurfaces As ArrayList
        Private FShowBoundingBox As Boolean
        Private FRand As New Random
        Private FRocksLastTime As Integer = 3

        Sub New()
            MyBase.New()
            FRocks = New ArrayList
            FRockSurfaces = New ArrayList
        End Sub

        ReadOnly Property pCount() As Integer
            Get
                Try
                    Return FRocks.Count
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

                Dim aRock As dxRockSprite

                FShowBoundingBox = Value

                For Each aRock In FRocks
                    aRock.pShowBoundingBox = Value
                Next

            End Set
        End Property

        Public Sub RestoreSurfaces(ByVal oDraw As Microsoft.DirectX.DirectDraw.Device)

            Dim oCK As New ColorKey
            oCK.ColorSpaceLowValue = RGB(255, 255, 255)
            oCK.ColorSpaceHighValue = RGB(255, 255, 255)

            Dim i As Integer
            Dim cName As String
            Dim oSurf As Microsoft.DirectX.DirectDraw.Surface

            Dim a As Reflection.Assembly = _
                System.Reflection.Assembly.GetExecutingAssembly()

            Do While FRockSurfaces.Count > 1
                oSurf = FRockSurfaces.Item(0)
                oSurf.Dispose()
                oSurf = Nothing
            Loop

            'load the bitmaps
            For i = 0 To 5

                cName = "SpaceRocks.Rock"
                Select Case i
                    Case 0 : cName &= "ABig"
                    Case 1 : cName &= "AMed"
                    Case 2 : cName &= "ASmall"
                    Case 3 : cName &= "BBig"
                    Case 4 : cName &= "BMed"
                    Case 5 : cName &= "BSmall"
                End Select
                cName &= ".bmp"

                oSurf = New Surface(a.GetManifestResourceStream(cName), _
                    New SurfaceDescription, oDraw)
                oSurf.SetColorKey(ColorKeyFlags.SourceDraw, oCK)
                FRockSurfaces.Add(oSurf)

            Next


        End Sub

        Private Sub AddRocks(ByVal iNum As Integer)

            Dim i As Integer
            For i = 1 To iNum
                AddRock()
            Next

        End Sub

        'adds a large rock to a random edge
        Private Overloads Function AddRock()

            Dim oPt As PointF
            'start location along the edges

            Select Case FRand.Next(0, Integer.MaxValue) Mod 4
                Case 0
                    oPt = New PointF(0, FRand.Next(0, Integer.MaxValue) Mod HGT)
                Case 1
                    oPt = New PointF(WID, FRand.Next(0, Integer.MaxValue) Mod HGT)
                Case 2
                    oPt = New PointF(FRand.Next(0, Integer.MaxValue) Mod WID, 0)
                Case 3
                    oPt = New PointF(FRand.Next(0, Integer.MaxValue) Mod WID, HGT)
            End Select

            Return AddRock(dxRockSize.rsLarge, oPt)
        End Function

        Private Overloads Function AddRock(ByVal pSize As dxRockSize, _
            ByVal p As PointF) As dxRockSprite

            Dim aRock As dxRockSprite

            aRock = New dxRockSprite
            With aRock
                .pShowBoundingBox = Me.pShowBoundingBox
                .pAlternateModel = FRand.Next(0, Integer.MaxValue) Mod 2 = 0
                .pSpinReverse = FRand.Next(0, Integer.MaxValue) Mod 2 = 0
                .pRotSpeed = FRand.Next(0, Integer.MaxValue) Mod 3
                .pRockSize = pSize
                Select Case pSize
                    Case dxRockSize.rsLarge
                        .Size = New Size(96, 96)
                    Case dxRockSize.rsMed
                        .Size = New Size(64, 64)
                    Case dxRockSize.rsSmall
                        .Size = New Size(32, 32)
                End Select

                .Location = p

                Do  'no straight up/down or left/right
                    .Velocity = New PointF(FRand.Next(-3, 3), FRand.Next(-3, 3))
                Loop Until .Velocity.X <> 0 And .Velocity.Y <> 0

                .Move() 'the first move makes sure they're off the edge

                AddHandler .GetSurfaceData, AddressOf GetRockSurfaceData
                AddHandler .RockBroken, AddressOf RockBroken
            End With
            FRocks.Add(aRock)
        End Function

        Private Sub GetRockSurfaceData(ByVal aSprite As dxSprite, _
            ByRef oSurf As Surface, ByRef oRect As Rectangle)

            Dim aRock As dxRockSprite = CType(aSprite, dxRockSprite)
            Dim iIndex As Integer

            iIndex = IIf(aRock.pAlternateModel, 3, 0)
            Select Case aRock.pRockSize
                Case dxRockSize.rsLarge
                    'nothing
                Case dxRockSize.rsMed
                    iIndex += 1
                Case dxRockSize.rsSmall
                    iIndex += 2
            End Select

            oSurf = CType(FRockSurfaces.Item(iIndex), Surface)
            oRect = New Rectangle((aRock.Frame Mod 6) * aRock.Size.Width, _
                (aRock.Frame \ 6) * aRock.Size.Height, aRock.Size.Width, aRock.Size.Height)

        End Sub

        Public Sub Move()

            Dim aRock As dxRockSprite

            If FRocks.Count = 0 Then
                FRocksLastTime += 1
                AddRocks(FRocksLastTime)
            End If

            For Each aRock In FRocks
                aRock.Move()
            Next


        End Sub

        Public Sub Draw(ByVal oSurf As Microsoft.DirectX.DirectDraw.Surface)

            Dim aRock As dxRockSprite

            For Each aRock In FRocks
                aRock.Draw(oSurf)
            Next

        End Sub

        Private Sub RockBroken(ByVal aRock As dxRockSprite)

            Select Case aRock.pRockSize
                Case dxRockSize.rsLarge
                    AddRock(dxRockSize.rsMed, aRock.Location)
                    AddRock(dxRockSize.rsMed, aRock.Location)

                Case dxRockSize.rsMed
                    AddRock(dxRockSize.rsSmall, aRock.Location)
                    AddRock(dxRockSize.rsSmall, aRock.Location)

                Case dxRockSize.rsSmall
                    'nothing
            End Select
            FRocks.Remove(aRock)

        End Sub

        Public Function CollidingWith(ByVal aRect As Rectangle, ByVal bBreakRock As Boolean) As Boolean

            Dim aRock As dxRockSprite

            For Each aRock In FRocks
                If aRock.WorldBoundingBox.IntersectsWith(aRect) Then
                    If bBreakRock Then
                        aRock.Break()
                    End If
                    Return True
                End If
            Next

            Return False
        End Function
    End Class

End Namespace
