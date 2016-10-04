Imports Microsoft.Win32

'loads/saves/displays high score info
'uses the registry
Public Class HighScoreRecorder

    Private FScores As ArrayList            'array of HighScore objects
    Private FGameKey As String
    Private FLowerIsBetter As Boolean = False
    Private FNumberOfScores As Integer = 8
    Private FChanged As Boolean = False

    Const GAMESUBKEY As String = "Software\Apress\DotNetGames"
    Const NOKEY = "xxx"

    Public Sub New(ByVal cGameKey As String, ByVal bLowerIsBetter As Boolean)
        MyBase.New()

        FGameKey = GAMESUBKEY & "\" & cGameKey
        LowerIsBetter = bLowerIsBetter

        LoadScores()
    End Sub

    Protected Overrides Sub Finalize()
        Call SaveScores()
    End Sub


    Property NumberOfScores() As Integer
        Get
            Return FNumberOfScores
        End Get
        Set(ByVal Value As Integer)
            FNumberOfScores = Value
        End Set
    End Property

    Property LowerIsBetter() As Boolean
        Get
            Return FLowerIsBetter
        End Get
        Set(ByVal Value As Boolean)
            FLowerIsBetter = Value
        End Set
    End Property

    Private Sub LoadScores()

        Dim aKey As RegistryKey
        Dim cValue As String
        Dim iLoop As Integer = 0
        Dim bDone As Boolean
        Dim aHS As HighScore

        FScores = New ArrayList

        aKey = Registry.CurrentUser

        'create a subkey. trap any errors (security, etc)
        Try
            aKey = aKey.CreateSubKey(FGameKey)

            bDone = False
            Do While Not bDone
                cValue = aKey.GetValue("Score" & iLoop, NOKEY)
                If cValue.Equals(NOKEY) Then
                    bDone = True
                Else
                    aHS = New HighScore(Me)
                    If aHS.Parse(cValue) Then
                        FScores.Add(aHS)
                    End If

                    iLoop += 1

                    '1- highscores max
                    If iLoop > NumberOfScores Then
                        bDone = True
                    End If
                End If
            Loop

        Finally
            Call aKey.Close()
        End Try

    End Sub

    Private Sub SaveScores()

        Dim aKey As RegistryKey
        Dim hs As HighScore
        Dim cKey As String
        Dim iLoop As Integer = 0

        aKey = Registry.CurrentUser

        'create a subkey. trap any errors (security, etc)
        Try
            aKey = aKey.CreateSubKey(FGameKey)

            iLoop = 0
            For Each hs In FScores
                cKey = "Score" & iLoop
                aKey.SetValue(cKey, hs.Player & "|" & hs.Score)
                iLoop += 1
            Next

        Finally
            Call aKey.Close()
        End Try

    End Sub

    Public Function ScoreWorthyOfList(ByVal iScore As Integer) As Boolean

        Dim hs As HighScore

        'can always add if not to max slots yet
        If FScores.Count < NumberOfScores Then
            Return True
        Else
            hs = FScores.Item(FScores.Count - 1)

            If LowerIsBetter Then
                Return hs.Score >= iScore
            Else
                Return hs.Score <= iScore
            End If
        End If

    End Function

    Public Function AddScoreToList(ByVal cUser As String, ByVal iScore As Integer) As Integer

        If Not ScoreWorthyOflist(iScore) Then Exit Function

        'add new one, sort
        FScores.Add(New HighScore(cUser, iScore, Me))
        FScores.Sort()

        'drop last one if too many
        If FScores.Count > FNumberOfScores Then
            FScores.RemoveAt(FScores.Count - 1)
        End If

        FChanged = True

    End Function


    Private Class HighScore
        Implements IComparable

        Public Sub New(ByVal Reader As HighScoreRecorder)
            MyBase.New()
            FReader = Reader
        End Sub

        Public Sub New(ByVal cPlayer As String, ByVal iScore As Integer, ByVal Reader As HighScoreRecorder)
            MyBase.New()

            Score = iScore
            Player = cPlayer
            FReader = Reader
        End Sub

        Private FScore As Integer
        Private FPlayer As String
        Private FReader As HighScoreRecorder

        Property Score() As Integer
            Get
                Return FScore
            End Get
            Set(ByVal Value As Integer)
                FScore = Value
            End Set
        End Property

        Property Player() As String
            Get
                Return FPlayer
            End Get
            Set(ByVal Value As String)
                FPlayer = Value
            End Set
        End Property

        Public Function Parse(ByVal cScoreValue As String) As Boolean

            Dim iPos As Integer

            iPos = cScoreValue.IndexOf("|")
            If iPos < 0 Then
                Return False
            Else
                Try
                    FPlayer = cScoreValue.Substring(0, iPos)
                    FScore = CInt(cScoreValue.Substring(iPos + 1))
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End If

        End Function

        Public Overridable Function CompareTo(ByVal o As Object) As Integer _
            Implements IComparable.CompareTo

            If Not TypeOf o Is HighScore Then
                Return -1
            Else
                If FReader.FLowerIsBetter Then
                    Return Me.Score.CompareTo(CType(o, HighScore).Score)
                Else
                    Return -Me.Score.CompareTo(CType(o, HighScore).Score)
                End If
            End If

        End Function

    End Class



End Class
