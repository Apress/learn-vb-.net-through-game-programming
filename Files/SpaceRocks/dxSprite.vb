Imports Microsoft.DirectX
Imports Microsoft.DirectX.DirectDraw
Imports System.Math

Namespace dxWorld

    'the base sprite class. contains information about position 
    'and state
    Public MustInherit Class dxSprite

        'get the correct surface and drawing rectangle to use
        Public Event GetSurfaceData(ByVal sender As dxSprite, _
            ByRef oSource As Microsoft.DirectX.DirectDraw.Surface, _
            ByRef oRect As Rectangle)

        Public MustOverride Sub Move()

        Private FLocation As PointF          'upper left corner
        Property Location() As PointF
            Get
                Return FLocation
            End Get
            Set(ByVal Value As PointF)
                FLocation = Value

                If FLocation.X <= -Size.Width Then
                    FLocation.X += WID
                ElseIf FLocation.X >= WID Then
                    FLocation.X -= (WID + Size.Width - 1)
                End If

                If FLocation.Y <= -Size.Height Then
                    FLocation.Y += (HGT + Size.Height)
                ElseIf FLocation.Y >= HGT Then
                    FLocation.Y -= (HGT + Size.Height - 1)
                End If

            End Set
        End Property

        Private FSize As Size 'size of one frame
        Property Size() As Size
            Get
                Return FSize
            End Get
            Set(ByVal Value As Size)
                FSize = Value
            End Set
        End Property

        Private FFrame As Integer
        Overridable Property Frame() As Integer
            Get
                Return FFrame
            End Get
            Set(ByVal Value As Integer)
                FFrame = Value
            End Set
        End Property

        'object-space, 0,0 = upper left corner of object
        Protected FBoundingBox As Rectangle
        ReadOnly Property BoundingBox() As Rectangle
            Get
                Return FBoundingBox
            End Get
        End Property

        'translated to world coordinates and clipped
        Protected FWBB As Rectangle
        ReadOnly Property WorldBoundingBox() As Rectangle
            Get
                Return FWBB
            End Get
        End Property


        Private FShowBoundingBox As Boolean = False
        Property pShowBoundingBox() As Boolean
            Get
                Return FShowBoundingBox
            End Get
            Set(ByVal Value As Boolean)
                FShowBoundingBox = Value
            End Set
        End Property

        ReadOnly Property Center() As PointF
            Get
                Return New PointF(Location.X + (Size.Width \ 2), _
                                Location.Y + (Size.Height \ 2))
            End Get
        End Property

        Public Sub Draw(ByVal oSurf As Microsoft.DirectX.DirectDraw.Surface)

            Dim oSource As Microsoft.DirectX.DirectDraw.Surface
            Dim oRect As Rectangle
            Dim oPt As Point
            Dim iDiff As Integer

            RaiseEvent GetSurfaceData(Me, oSource, oRect)

            If oSource Is Nothing Then
                Exit Sub
            Else
                Try
                    FWBB = Me.BoundingBox        'start w/ normal bbox

                    'start at the location
                    oPt = New Point(System.Math.Floor(Location.X), _
                                    System.Math.Floor(Location.Y))

                    If oPt.X < 0 Then
                        'draw partial on left side
                        oRect = New Rectangle(oRect.Left - oPt.X, oRect.Top, _
                            oRect.Width + oPt.X, oRect.Height)

                        If oPt.X + FWBB.Left < 0 Then

                            FWBB = New Rectangle(0, FWBB.Top, _
                                FWBB.Width + (oPt.X + FWBB.Left), FWBB.Height)

                        Else

                            FWBB = New Rectangle(FWBB.Left + oPt.X, FWBB.Top, _
                                FWBB.Width, FWBB.Height)

                        End If
                        oPt.X = 0
                    End If

                    If oPt.Y < 0 Then
                        'draw partial on top side
                        oRect = New Rectangle(oRect.Left, oRect.Top - oPt.Y, _
                            oRect.Width, oRect.Height + oPt.Y)

                        If oPt.Y + FWBB.Top < 0 Then

                            FWBB = New Rectangle(FWBB.Left, 0, _
                                FWBB.Width, FWBB.Height + (oPt.Y + FWBB.Top))

                        Else

                            FWBB = New Rectangle(FWBB.Left, FWBB.Top + oPt.Y, _
                                FWBB.Width, FWBB.Height)

                        End If


                        oPt.Y = 0
                    End If

                    If oPt.X + oRect.Width > WID Then
                        'draw partial on right side
                        oRect = New Rectangle(oRect.Left, oRect.Top, _
                            WID - oPt.X, oRect.Height)

                        If oPt.X + FWBB.Left + FWBB.Width > WID Then

                            FWBB = New Rectangle(FWBB.Left, FWBB.Top, _
                                WID - (oPt.X + FWBB.Left), FWBB.Height)

                        End If

                    End If

                    If oPt.Y + oRect.Height > HGT Then
                        'draw partial on bottom
                        oRect = New Rectangle(oRect.Left, oRect.Top, _
                            oRect.Width, HGT - oPt.Y)

                        If oPt.Y + FWBB.Top + FWBB.Height > HGT Then

                            FWBB = New Rectangle(FWBB.Left, FWBB.Top, _
                                FWBB.Width, HGT - (oPt.Y + FWBB.Top))

                        End If

                    End If

                    'should never happen, just in case
                    If oRect.Width <= 0 Or oRect.Height <= 0 Then Return

                    'offset the bounding box by the world coordinates
                    FWBB.Offset(oPt.X, oPt.Y)

                    'draw the sprite
                    oSurf.DrawFast(oPt.X, oPt.Y, oSource, oRect, _
                        DrawFastFlags.DoNotWait Or DrawFastFlags.SourceColorKey)

                    'draw the bounding box
                    If Me.pShowBoundingBox Then
                        oSurf.ForeColor = Color.Red

                        oSurf.DrawBox(FWBB.Left, FWBB.Top, FWBB.Right, FWBB.Bottom)

                    End If

                Catch oEx As Exception
                    Debug.WriteLine("--------------------------------------")
                    Debug.WriteLine(oEx.Message)
                End Try
            End If

        End Sub
    End Class

End Namespace