Imports System.Windows.Forms
Imports System.Drawing.Imaging
Imports System.Math

Public Class NumberPanel
    Inherits Panel

    Private Const IRAD As Integer = 112

    Private fbNum As Bitmap
    Private bBack As Bitmap
    Private FInAnimation As Boolean

    Private oTiles As ArrayList

    Public Event TileMoving(ByVal FBackwards As Boolean)

    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        Dim a As Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        fbNum = New Bitmap(a.GetManifestResourceStream("NineTiles.numbertiles.bmp"))

        Call CreateTiles()

        Me.Height = IRAD * 3
        Me.Width = IRAD * 3

    End Sub

    Public Overloads Sub Dispose()
        fbNum.Dispose()
        MyBase.Dispose()
    End Sub

    Private Sub CreateTiles()

        Dim i As Integer
        Dim t As TileData

        oTiles = New ArrayList()
        For i = 1 To 9
            t = New TileData(Me, i)
            oTiles.Add(t)
        Next

    End Sub

    Protected Sub OnTileMoving(ByVal FBackwards As Boolean)
        RaiseEvent TileMoving(FBackwards)
    End Sub

    Protected Overrides Sub OnResize(ByVal eventargs As System.EventArgs)
        MyBase.OnResize(EventArgs)
        Call SetupBackground()
    End Sub

    Private Sub SetupBackground()
        If Not bBack Is Nothing Then bBack.Dispose()
        'bBack = New Bitmap(Me.Width, Me.Height, PixelFormat.Format32bppPArgb)
        bBack = New Bitmap(Me.Width, Me.Height)
    End Sub

    Public Function TilesVisible() As Boolean

        Dim aTile As TileData
        Dim i As Integer = 0

        For Each aTile In oTiles
            If aTile.pVisible Then
                i += 1
            End If
        Next
        Return i

    End Function

    ReadOnly Property bInAnimation()
        Get
            Return FInAnimation
        End Get
    End Property

    ReadOnly Property Result() As Integer
        Get
            Dim aTile As TileData
            Dim i As Integer = 0

            For Each aTile In oTiles
                If aTile.pVisible And aTile.pBackwards Then
                    i += aTile.pTileNum
                End If
            Next
            Return i
        End Get
    End Property

    Public Sub HideBackward()
        Dim aTile As TileData

        For Each aTile In oTiles
            If aTile.pVisible And aTile.pBackwards Then
                aTile.pVisible = False
            End If
        Next
        Me.Invalidate()
        Application.DoEvents()
    End Sub

    'checks all combinations of 1, 2, 3, and 4 visible tiles 
    Public Function ResultAvailable(ByVal iDesired As Integer) As Boolean

        Dim i, j, k, l As Integer
        Dim aTilei, aTilej, aTilek, aTilel As TileData

        'one-bangers
        For i = 0 To oTiles.Count - 1
            aTilei = oTiles(i)
            If aTilei.pVisible Then
                If aTilei.pTileNum = iDesired Then
                    Return True
                End If
            End If
        Next

        '2-bangers
        For i = 0 To oTiles.Count - 2
            For j = i + 1 To oTiles.Count - 1
                aTilei = oTiles(i)
                aTilej = oTiles(j)
                If aTilei.pVisible And _
                    aTilej.pVisible Then

                    If aTilei.pTileNum + aTilej.pTileNum = iDesired Then
                        Return True
                    End If

                End If
            Next
        Next

        '3-bangers
        For i = 0 To oTiles.Count - 3
            For j = i + 1 To oTiles.Count - 2
                For k = j + 1 To oTiles.Count - 1
                    aTilei = oTiles(i)
                    aTilej = oTiles(j)
                    aTilek = oTiles(k)
                    If aTilei.pVisible And _
                        aTilej.pVisible And _
                        aTilek.pVisible Then

                        If aTilei.pTileNum + aTilej.pTileNum + aTilek.pTileNum = iDesired Then
                            Return True
                        End If

                    End If
                Next
            Next
        Next

        '4-bangers
        For i = 0 To oTiles.Count - 4
            For j = i + 1 To oTiles.Count - 3
                For k = j + 1 To oTiles.Count - 2
                    For l = k + 1 To oTiles.Count - 1

                        aTilei = oTiles(i)
                        aTilej = oTiles(j)
                        aTilek = oTiles(k)
                        aTilel = oTiles(l)
                        If aTilei.pVisible And _
                            aTilej.pVisible And _
                            aTilek.pVisible And _
                            aTilel.pVisible Then

                            If aTilei.pTileNum + _
                                aTilej.pTileNum + _
                                aTilek.pTileNum + _
                                aTilel.pTileNum = iDesired Then
                                Return True
                            End If
                        End If
                    Next
                Next
            Next
        Next

    End Function

    Public Sub ResetTiles()

        Dim t As TileData

        For Each t In oTiles
            t.Reset()
        Next

        Me.Invalidate()
        Application.DoEvents()

    End Sub

    Private Function TileFromPoint(ByVal x As Long, ByVal y As Long) As Integer
        TileFromPoint = ((y \ IRAD) * 3) + (x \ IRAD)
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        MyBase.OnMouseDown(e)

        If e.Button = MouseButtons.Left Then
            If Not FInAnimation Then
                FInAnimation = True

                Try
                    Dim aTile As TileData
                    Dim iTile As Integer = TileFromPoint(e.X, e.Y)

                    aTile = oTiles(iTile)
                    aTile.ToggleFacing()
                Catch oEX As Exception
                    Throw oEX
                Finally
                    FInAnimation = False
                End Try

            End If
        End If
    End Sub


    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        'happens in design mode
        If bBack Is Nothing Then
            Call SetupBackground()
        End If
        e.Graphics.DrawImageUnscaled(bBack, 0, 0)
    End Sub

    Private Class TileData

        Private FPanel As NumberPanel

        'don't need separate height/width, sprite is square
        Private Const IRAD As Integer = 112

        'tiles are always drawn in the same place on the panel
        Private FxPos, FyPos As Integer
        Private FySrc As Integer

        Public Sub New(ByVal oPanel As NumberPanel, ByVal iTileNum As Integer)
            MyBase.New()

            Dim iTilePos As Integer

            FPanel = oPanel
            FTileNum = iTileNum

            iTilePos = iTileNum - 1         '0-8, one less than tile number

            'coordinates to draw tile on panel is fixed
            FxPos = (iTilePos Mod 3) * IRAD
            FyPos = (iTilePos \ 3) * IRAD

            'y coord in source bitmap fixed (see bitmap)
            FySrc = iTilePos * IRAD
        End Sub

        Private FTileNum As Integer
        ReadOnly Property pTileNum() As Integer
            Get
                Return FTileNum
            End Get
        End Property

        Private FVisible As Boolean = True
        Property pVisible() As Boolean
            Get
                Return FVisible
            End Get

            Set(ByVal Value As Boolean)
                FVisible = Value
                Me.Draw()
            End Set
        End Property

        Private FBackwards As Boolean = False

        Property pBackwards() As Boolean
            Get
                Return FBackwards
            End Get
            Set(ByVal Value As Boolean)
                FBackwards = Value
                pFrame = IIf(Value, 9, 0)
            End Set
        End Property

        Private FFrame As Integer = 0
        Private Property pFrame() As Integer
            Get
                Return FFrame
            End Get
            Set(ByVal Value As Integer)

                If FFrame < 0 Or FFrame > 9 Then
                    Throw New Exception("Frame out of range")
                End If

                FFrame = Value

            End Set
        End Property

        Public Sub Reset()
            pBackwards = False
            pVisible = True
        End Sub

        Private Sub Draw()

            Dim gr As Graphics
            Dim r As System.Drawing.Rectangle
            Dim xSrc As Integer

            Dim bDest As Bitmap = FPanel.bBack

            gr = Graphics.FromImage(bDest)
            Try
                If FVisible Then
                    xSrc = FFrame * IRAD
                    r = New System.Drawing.Rectangle(xSrc, FySrc, IRAD + 1, IRAD + 1)
                    gr.DrawImage(FPanel.fbNum, FxPos, FyPos, r, GraphicsUnit.Pixel)
                Else
                    'draw a black square
                    r = New System.Drawing.Rectangle(FxPos, FyPos, IRAD, IRAD)
                    gr.FillRectangle(New SolidBrush(Color.Black), r)
                End If
            Finally
                gr.Dispose()
            End Try

        End Sub

        'animates backward or forward
        Public Sub ToggleFacing()

            Dim iStart, iEnd As Integer
            Dim iDir, iLoop As Integer
            If Not FVisible Then Exit Sub

            If pBackwards Then
                iStart = 9
                iEnd = 0
                iDir = -1
            Else
                iStart = 0
                iEnd = 9
                iDir = 1
            End If

            FPanel.OnTileMoving(pBackwards)
            For iLoop = iStart To iEnd Step iDir
                pFrame = iLoop
                Me.Draw()
                FPanel.Invalidate()
                Application.DoEvents()
            Next
            pBackwards = Not pBackwards

        End Sub
    End Class

End Class
