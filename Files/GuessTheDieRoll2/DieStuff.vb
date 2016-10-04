Imports System.Math

Public Class PaintPanel
    Inherits Panel

    Public Sub New()
        MyBase.New()

        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

    End Sub

End Class

'why is it better?
'encapsulation - outside programmer no longer has to deal with:
'DieStatus, rollloop,h,w,diexPos, etc (all the privates)
'
'better grouping of code
'see Frame and Result property, w/ error checking, and 
'you "just know" where code is

'look at the guess property (form)
'rollthedie more self-explanatory, with object used.

'putting down 2 die should be easier

Public Class Die

    Private Enum DieStatus
        dsStopped = 0
        dsRolling = 1
        dsLanding = 2
    End Enum

    Private bmStop As Bitmap
    Private bmxRot As Bitmap
    Private bmyRot As Bitmap
    Private bmBack As Bitmap                 'background bitmap

    Private oRand As New Random(Now.Ticks Mod 100)
    Private FRollLoop As Integer

    Private h As Integer = 144
    Private w As Integer = 144

    Private diexPos As Integer
    Private dieyPos As Integer
    Private diexDir As Integer         '-8 to 8
    Private dieyDir As Integer         '-8 to 8, indicates direction moving

    Private FStatus As DieStatus = DieStatus.dsLanding
    Private FPanel As PaintPanel

    Public Sub New(ByVal pn As PaintPanel)
        MyBase.New()

        FPanel = pn
        bmBack = New Bitmap(FPanel.Width, FPanel.Height)

        Dim a As Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        bmxRot = New Bitmap(a.GetManifestResourceStream("GuessTheDieRoll2.dicexrot.bmp"))
        bmyRot = New Bitmap(a.GetManifestResourceStream("GuessTheDieRoll2.diceyrot.bmp"))
        bmStop = New Bitmap(a.GetManifestResourceStream("GuessTheDieRoll2.dicedone.bmp"))
    End Sub


    Private FFrame As Integer
    Property Frame() As Integer
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

    Public Sub InitializeLocation()
        diexPos = oRand.Next(0, FPanel.Width - w)
        dieyPos = oRand.Next(0, FPanel.Height - h)
    End Sub

    Public Sub UpdateDiePosition()

        Select Case FStatus
            Case DieStatus.dsLanding
                'if landing reduce the frame by 1, regardless of direction
                Frame -= 1
            Case DieStatus.dsRolling
                'frame goes up or down based on x direction
                Frame += (1 * Sign(dieyDir))
        End Select

        'update the position
        diexPos += diexDir

        'bounce for x
        If diexPos < 0 Then
            diexPos = 0
            diexDir = -diexDir
            Call WavPlayer.PlayWav("GuessTheDieRoll2.DIE1.WAV")
        End If
        If diexPos > FPanel.Width - w Then
            diexPos = FPanel.Width - w
            diexDir = -diexDir
            Call WavPlayer.PlayWav("GuessTheDieRoll2.DIE1.WAV")
        End If

        dieyPos += dieyDir
        'bounce for y
        If dieyPos < 0 Then
            dieyPos = 0
            dieyDir = -dieyDir
            Call WavPlayer.PlayWav("GuessTheDieRoll2.DIE2.WAV")
        End If
        If dieyPos > FPanel.Height - h Then
            dieyPos = FPanel.Height - h
            dieyDir = -dieyDir
            Call WavPlayer.PlayWav("GuessTheDieRoll2.DIE2.WAV")
        End If

        FRollLoop += 1

        Select Case FStatus
            Case DieStatus.dsRolling
                'after 100 frames, check for a small chance that the die will stop rolling
                If FRollLoop > 100 And oRand.Next(1, 100) < 10 Then
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
            diexDir = oRand.Next(-8, 9)       'initialize the directions, 0/1 no good
        Loop Until Abs(diexDir) > 3
        Do
            dieyDir = oRand.Next(-8, 9)
        Loop Until Abs(dieyDir) > 3
        Result = oRand.Next(1, 7)      'decide what the result will be

        FRollLoop = 0
        FStatus = DieStatus.dsRolling

    End Sub

    Public Sub DrawDie()

        Dim gr As Graphics
        Dim b As Bitmap

        Dim x As Integer = (Frame Mod 6) * w
        Dim y As Integer = (Frame \ 6) * h
        Dim r As New System.Drawing.Rectangle(x, y, w, h)

        'select the correct bitmap based on what the die is doing, and what direction it's going
        If FStatus = DieStatus.dsRolling Then
            'check quandrant rolling towards based on sign of xdir*ydir
            If (diexDir * dieyDir) > 0 Then
                b = bmyRot
            Else
                b = bmxRot
            End If
        Else
            b = bmStop
        End If

        gr = Graphics.FromImage(bmBack)
        Try
            gr.Clear(Color.Black)
            gr.DrawImage(b, diexPos, dieyPos, r, GraphicsUnit.Pixel)
        Finally
            gr.Dispose()
        End Try

    End Sub

    ReadOnly Property IsNotRolling() As Boolean
        Get
            Return FStatus = DieStatus.dsStopped
        End Get
    End Property

    ReadOnly Property BackgroundPic() As Bitmap
        Get
            Return bmBack
        End Get
    End Property

End Class
