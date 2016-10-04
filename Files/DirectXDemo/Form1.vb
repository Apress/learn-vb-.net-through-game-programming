Imports Microsoft.DirectX
Imports Microsoft.DirectX.DirectDraw

Public Class Form1
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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form1"
        Me.Text = "Form1"

    End Sub

#End Region
    Private Const WID As Integer = 1024
    Private Const HGT As Integer = 768
    Private Const NUMDICE As Integer = 250

    Private FNeedToRestore As Boolean
    Private FDraw As Microsoft.DirectX.DirectDraw.Device
    Private FFront As Microsoft.DirectX.DirectDraw.Surface
    Private FBack As Microsoft.DirectX.DirectDraw.Surface
    Private FDieSurf As Microsoft.DirectX.DirectDraw.Surface

    Private FDice As ArrayList

    Private Sub Form1_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Cursor.Dispose()     'byebye cursor
        InitializeDirectDraw()
        SetupDice()

        While Created
            DrawFrame()
        End While
    End Sub

    Private Sub InitializeDirectDraw()

        Dim oSurfaceDesc As New SurfaceDescription
        Dim oSurfaceCaps As New SurfaceCaps
        Dim i As Integer

        FDraw = New Microsoft.DirectX.DirectDraw.Device

        FDraw.SetCooperativeLevel(Me, _
            Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.FullscreenExclusive)

        FDraw.SetDisplayMode(WID, HGT, 16, 0, False)

        With oSurfaceDesc
            .SurfaceCaps.PrimarySurface = True
            .SurfaceCaps.Flip = True
            .SurfaceCaps.Complex = True
            .BackBufferCount = 1
            FFront = New Surface(oSurfaceDesc, FDraw)
            oSurfaceCaps.BackBuffer = True
            FBack = FFront.GetAttachedSurface(oSurfaceCaps)
            FBack.ForeColor = Color.White
            .Clear()
        End With

        FNeedToRestore = True

    End Sub

    Public Sub RestoreSurfaces()

        Dim oCK As New ColorKey

        FDraw.RestoreAllSurfaces()

        Dim a As Reflection.Assembly = _
            System.Reflection.Assembly.GetExecutingAssembly()

        If Not FDieSurf Is Nothing Then
            FDieSurf.Dispose()
            FDieSurf = Nothing
        End If

        FDieSurf = New Surface(a.GetManifestResourceStream( _
           "DirectXDemo.dicexrot.bmp"), New SurfaceDescription, FDraw)

        FDieSurf.SetColorKey(ColorKeyFlags.SourceDraw, oCK)

    End Sub

    Private Sub SetupDice()

        Dim d As SimpleDie
        Dim r As New Random

        FDice = New ArrayList
        Do While FDice.Count < NUMDICE
            d = New SimpleDie(New Point(r.Next(0, WID - 72), r.Next(0, HGT - 72)))
            FDice.Add(d)
        Loop

    End Sub

    Private Sub DrawFrame()

        Dim d As SimpleDie

        If FFront Is Nothing Then Exit Sub

        'can't draw now, device not ready
        If Not FDraw.TestCooperativeLevel() Then
            FNeedToRestore = True
            Exit Sub
        End If

        If FNeedToRestore Then
            RestoreSurfaces()
            FNeedToRestore = False
        End If

        FBack.ColorFill(0)
        For Each d In FDice
            d.Draw(FBack, FDieSurf)
        Next

        Try
            FBack.ForeColor = Color.White
            FBack.DrawText(10, 10, "Press escape to exit", False)
            FFront.Flip(FBack, FlipFlags.DoNotWait)
        Catch oEX As Exception
            Debug.WriteLine(oEX.Message)
        Finally
            Application.DoEvents()
        End Try

    End Sub

    Private Sub Form1_KeyUp(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp

        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class

Public Class SimpleDie
    Private FLocation As Point
    Private FFrame As Integer

    Public Sub New(ByVal p As Point)
        FLocation = p
    End Sub

    ReadOnly Property pLocation() As Point
        Get
            Return FLocation
        End Get
    End Property

    Public Sub Draw(ByVal FDest As Surface, ByVal FSource As Surface)

        Dim oRect As Rectangle

        oRect = New Rectangle((FFrame Mod 6) * 72, (FFrame \ 6) * 72, 72, 72)

        FDest.DrawFast(FLocation.X, FLocation.Y, FSource, oRect, _
            DrawFastFlags.DoNotWait Or DrawFastFlags.SourceColorKey)

        FFrame = (FFrame + 1) Mod 36
    End Sub
End Class