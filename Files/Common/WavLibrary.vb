'loads WAV files from executable or from disk into memory and plays them.
'each WAV file is referenced by a short name given it during load
Public Class WavLibrary

    Private FSounds As Hashtable

    Public Sub New()
        MyBase.New()
        FSounds = New Hashtable
    End Sub

    Public Sub Dispose()

        Dim w As WavFile

        For Each w In FSounds.Values
            w.Dispose()
        Next
    End Sub

    Public Function LoadFromResource(ByVal cResName As String, ByVal cName As String) As Boolean

        Dim w As WavFile

        w = New WavFile(cName)
        If w.LoadFromResource(cResName) Then
            FSounds.Add(w.Name, w)
        End If

    End Function

    Public Overloads Sub Play(ByVal cName As String, Optional ByVal bSync As Boolean = False)

        Dim w As WavFile

        w = FSounds.Item(cName)
        If w Is Nothing Then
            Throw New Exception("Sound name " & cName & " not found")
        Else
            w.Play(bSync)
        End If
    End Sub

    'play and pause
    Public Overloads Sub Play(ByVal cName As String, _
        ByVal mSec As Integer, _
        Optional ByVal bSync As Boolean = False)

        Dim w As WavFile

        w = FSounds.Item(cName)
        If w Is Nothing Then Exit Sub

        w.Play(mSec, bSync)

    End Sub

    Private Class WavFile

        Private Declare Function sndPlaySound Lib "winmm.dll" _
            Alias "sndPlaySoundA" (ByVal szSound As Byte(), ByVal UFlags As Int32) As Int32

        Private Const SND_ASYNC As Int32 = &H1
        Private Const SND_MEMORY As Int32 = &H4

        Private bstr() As Byte
        Private FName As String

        Public Sub New(ByVal cName As String)
            MyBase.New()
            FName = cName
        End Sub

        Public Sub Dispose()
            bstr = Nothing
        End Sub

        Property Name() As String
            Get
                Return FName
            End Get
            Set(ByVal Value As String)
                FName = Value
            End Set
        End Property

        Public Function LoadFromResource(ByVal cResName As String) As Boolean

            Dim a As System.IO.Stream
            Dim bRet As Boolean = False

            a = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(cResName)
            If a Is Nothing Then Exit Function

            ReDim bstr(a.Length)

            Try
                a.Read(bstr, 0, Int(a.Length))
                bRet = True
            Catch oEX As Exception
                bRet = False
            Finally
                a = Nothing
            End Try

            Return bRet

        End Function

        'always pause at least a little, or sound often can't be heard
        Public Overloads Sub Play(ByVal bSync As Boolean)
            Play(mSec:=10, bSync:=bSync)
        End Sub

        Public Overloads Sub Play(ByVal mSec As Integer, ByVal bSync As Boolean)

            If bstr Is Nothing Then Exit Sub
            Dim iFlags As Integer

            Try
                iFlags = SND_MEMORY
                If Not bSync Then iFlags = iFlags Or SND_ASYNC
                sndPlaySound(bstr, iFlags)
                System.Threading.Thread.Sleep(mSec)
            Catch oEX As Exception
                'nothing
            End Try

        End Sub

    End Class
End Class

