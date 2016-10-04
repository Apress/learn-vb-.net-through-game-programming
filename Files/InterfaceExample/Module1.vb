Module ShowASequence

    Sub Main()

        Dim oInt As IIntegerSequencer
        Dim oRand As New Random
        Dim i As Integer

        If oRand.Next(0, 1000) Mod 2 = 0 Then
            oInt = New IntegerCounter
        Else
            oInt = New FibonacciCounter
        End If

        Console.WriteLine("couting started ")
        Do
            i = oInt.GetNext
            Console.Write(i & ",")
        Loop Until i > 100
        Console.ReadLine()
    End Sub

End Module

Interface IIntegerSequencer
    Function GetNext() As Integer
End Interface

Public Class IntegerCounter
    Implements IIntegerSequencer

    Private FLast As Integer = 1

    Public Function GetNext() As Integer _
        Implements IIntegerSequencer.GetNext

        FLast += 1
        Return FLast
    End Function
End Class

Public Class FibonacciCounter
    Implements IIntegerSequencer

    Dim iTurn As Integer = 0
    Private FLast1 As Integer = 1
    Private FLast2 As Integer = 1

    Public Function GetNext() As Integer _
        Implements IIntegerSequencer.GetNext

        Dim iTemp As Integer

        iTurn += 1
        If iTurn < 3 Then
            Return 1
        Else

            iTemp = FLast1 + FLast2

            FLast1 = FLast2
            FLast2 = iTemp
            Return iTemp
        End If
    End Function

End Class


