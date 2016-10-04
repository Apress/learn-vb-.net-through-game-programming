Public Class WavPlayer

    Private Declare Function sndPlaySound Lib "winmm.dll" _
        Alias "sndPlaySoundA" (ByVal szSound As Byte(), ByVal UFlags As Int32) As Int32

    Private Const SND_ASYNC As Integer = 1
    Private Const SND_MEMORY As Integer = 4


    Public Shared Sub PlayWav(ByVal cResName As String)
        Dim a As System.IO.Stream

        a = System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(cResName)

        'couldn't find the sound
        If a Is Nothing Then Exit Sub
        Dim bstr(a.Length) As Byte

        Try
            a.Read(bstr, 0, Int(a.Length))
            sndPlaySound(bstr, SND_ASYNC Or SND_MEMORY)
        Finally
            bstr = Nothing
            a = Nothing
        End Try

    End Sub


End Class
