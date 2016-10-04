Public Class HighResTimer

    Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Long) As Short
    Declare Function QueryPerformanceFrequency Lib "Kernel32" (ByRef X As Long) As Short

    Private i1 As Long
    Private i2 As Long
    Private iFreq As Long

    Public Sub New()

        If QueryPerformanceCounter(i1) Then   ' Begin timing.
            QueryPerformanceFrequency(iFreq)
        Else
            Throw New ApplicationException("High Res timing not supported")
        End If

    End Sub

    Public Sub StartTimer()
        QueryPerformanceCounter(i1) 'begin timing
    End Sub

    Public Function EndTimer() As Long
        QueryPerformanceCounter(i2)
        Return i2 - i1
    End Function

End Class
