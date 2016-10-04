Imports ShapeTileGames.NewTile

Namespace DeductileReasoning

    Public Class PuzzleGenerator

        Private oPL As TileComboPossiblesList
        Private FClueClassNames As ArrayList
        Private FClues As ArrayList
        Private FAsm As Reflection.Assembly

        Public Sub New()
            MyBase.New()

            oPL = New TileComboPossiblesList
            GetClueClasses()
            GenerateClues()
        End Sub

        'use reflection to find all the clue subclasses and load them up
        Private Sub GetClueClasses()

            FAsm = System.Reflection.Assembly.GetExecutingAssembly()
            Dim t As Type

            FClueClassNames = New ArrayList

            'GetType used when not instantiating (can't instantiate b/c of MustInherit)
            Dim tParent As Type = GetType(TileComboClue)

            For Each t In FAsm.GetTypes
                If t.IsSubclassOf(tParent) Then
                    FClueClassNames.Add(t)
                End If
            Next

        End Sub

        Private Sub GenerateClues()

            Dim c As TileComboClue
            Dim oTyp As Type

            Dim oRand As New Random
            Dim oArgs() As Object = {oPL.Solution}

            Debug.WriteLine("-------------------------------------------")
            Debug.WriteLine(oPL.Solution.ToString)
            Debug.WriteLine("-------------------------------------------")

            FClues = New ArrayList
            Do
                oTyp = FClueClassNames.Item(oRand.Next(0, FClueClassNames.Count))
                c = Activator.CreateInstance(oTyp, oArgs)
                If oPL.NumberClueWouldEliminate(c) > 0 Then
                    oPL.EliminateBasedOnClue(c)
                    FClues.Add(c)
                    'Debug.WriteLine(c.ClueText & " (" & oPL.SolutionsLeft & ")")
                End If

            Loop Until oPL.AllNonSolutionsEliminated
        End Sub

        Public Sub PopulateListBox(ByVal lb As ListBox)

            Dim c As TileComboClue

            lb.Items.Clear()
            For Each c In FClues
                lb.Items.Add(c.ClueText)
            Next
        End Sub

        Public Function IsSolution(ByVal a As ColoredShape, _
            ByVal b As ColoredShape, ByVal c As ColoredShape, _
            ByVal d As ColoredShape) As Boolean

            Dim t As New TileCombo(a, b, c, d)

            Return t.Equals(oPL.Solution)

        End Function

    End Class

    'holds 4 ColoredShapes
    Public Class TileCombo
        Private Fcs As ArrayList        'holds array of 4 ColoredShapes

        Public Sub New(ByVal a As ColoredShape, ByVal b As ColoredShape, _
            ByVal c As ColoredShape, ByVal d As ColoredShape)

            Fcs = New ArrayList
            Fcs.Add(a)
            Fcs.Add(b)
            Fcs.Add(c)
            Fcs.Add(d)
        End Sub

        Public Overloads Function Equals(ByVal t As TileCombo) As Boolean

            Dim oMe As ColoredShape
            Dim oHim As ColoredShape
            Dim i As Integer
            Dim r As Boolean

            r = True
            For i = 0 To Fcs.Count - 1
                oMe = Me.ColoredShape(i)
                oHim = t.ColoredShape(i)
                If Not oMe.Equals(oHim) Then
                    r = False
                End If
            Next

            Return r

        End Function

        ReadOnly Property ColoredShape(ByVal i As Integer) As ColoredShape
            Get
                Return Fcs.Item(i)
            End Get
        End Property

        'has this combo been eliminated using the available clues
        Private FEliminated As Boolean = False
        Property Eliminated() As Boolean
            Get
                Return FEliminated
            End Get
            Set(ByVal Value As Boolean)
                FEliminated = Value
            End Set
        End Property

        Public Overrides Function ToString() As String

            Dim o As ColoredShape
            Dim s As String

            For Each o In Fcs
                s &= o.ToString & ";"
            Next

            Return s.Substring(0, s.Length - 1)
        End Function

    End Class

    Public Class TileComboPossiblesList

        Private FPossibles As ArrayList
        Private iSolution As Integer

        Public Sub New()
            GeneratePossibles()
            SelectSolution()
        End Sub

        Public Function Item(ByVal i As Integer) As TileCombo
            Return FPossibles.Item(i)
        End Function

        Public Function Solution() As TileCombo
            Return FPossibles.Item(iSolution)
        End Function

        Private Sub GeneratePossibles()

            Dim FTuples As ArrayList
            Dim i, j As Integer

            Dim oTi, oTj As FourTuple

            'these are the 24 ordered possibilities for integers 0,1,2,3
            FTuples = New ArrayList
            With FTuples
                .Add(New FourTuple(0, 1, 2, 3))
                .Add(New FourTuple(0, 1, 3, 2))
                .Add(New FourTuple(0, 2, 1, 3))
                .Add(New FourTuple(0, 2, 3, 1))
                .Add(New FourTuple(0, 3, 2, 1))
                .Add(New FourTuple(0, 3, 1, 2))
                .Add(New FourTuple(1, 0, 2, 3))
                .Add(New FourTuple(1, 0, 3, 2))
                .Add(New FourTuple(2, 0, 1, 3))
                .Add(New FourTuple(2, 0, 3, 1))
                .Add(New FourTuple(3, 0, 1, 2))
                .Add(New FourTuple(3, 0, 2, 1))
                .Add(New FourTuple(1, 2, 0, 3))
                .Add(New FourTuple(1, 3, 0, 2))
                .Add(New FourTuple(2, 1, 0, 3))
                .Add(New FourTuple(2, 3, 0, 1))
                .Add(New FourTuple(3, 2, 0, 1))
                .Add(New FourTuple(3, 1, 0, 2))
                .Add(New FourTuple(1, 2, 3, 0))
                .Add(New FourTuple(1, 3, 2, 0))
                .Add(New FourTuple(2, 3, 1, 0))
                .Add(New FourTuple(2, 1, 3, 0))
                .Add(New FourTuple(3, 2, 1, 0))
                .Add(New FourTuple(3, 1, 2, 0))
            End With

            'we need every permuation of every permutation. this gives us 576 combinations
            FPossibles = New ArrayList
            For Each oTi In FTuples
                For Each oTj In FTuples
                    FPossibles.Add(New TileCombo( _
                        New ColoredShape(oTi.a, oTj.a), _
                        New ColoredShape(oTi.b, oTj.b), _
                        New ColoredShape(oTi.c, oTj.c), _
                        New ColoredShape(oTi.d, oTj.d)))

                Next
            Next

        End Sub
        Private Sub SelectSolution()
            'FPossibles contains 576 possible combinations of tile color/shapes. pick one

            Dim oRand As New Random
            iSolution = oRand.Next(0, FPossibles.Count)
        End Sub

        Public Function SolutionsLeft() As Integer

            Dim t As TileCombo
            Dim iCtr As Integer = 0

            For Each t In FPossibles
                If Not t.Eliminated Then
                    iCtr += 1
                End If
            Next

            Return iCtr
        End Function

        Public Function AllNonSolutionsEliminated() As Boolean
            Return (SolutionsLeft() = 1)
        End Function

        'holds 4 integers in a class
        Private Class FourTuple
            Public a As Integer
            Public b As Integer
            Public c As Integer
            Public d As Integer

            Public Sub New(ByVal ia As Integer, ByVal ib As Integer, _
                ByVal ic As Integer, ByVal id As Integer)

                a = ia
                b = ib
                c = ic
                d = id
            End Sub
        End Class

        Public Function NumberClueWouldEliminate(ByVal c As TileComboClue) As Integer

            Dim t As TileCombo
            Dim r As Integer

            r = 0
            For Each t In FPossibles
                If Not c.CluePertainsTo(t) Then
                    If Not t.Eliminated Then
                        r += 1
                    End If
                End If
            Next

            Return r

        End Function

        'eliminates tiles that don't fit a clue
        'count NEWLY eliminated clues
        Public Sub EliminateBasedOnClue(ByVal c As TileComboClue)

            Dim t As TileCombo
            Dim iCount As Integer

            For Each t In FPossibles
                If Not c.CluePertainsTo(t) Then
                    If Not t.Eliminated Then
                        iCount += 1
                        t.Eliminated = True
                    End If
                End If
            Next

            Debug.Assert(iCount > 0)        'make sure it eliminated at least one remaining solution
        End Sub

        Public Sub EnumerateRemaining()

            Dim t As TileCombo

            For Each t In FPossibles
                If Not t.Eliminated Then
                    Debug.WriteLine(t.ToString)
                End If
            Next

        End Sub
    End Class


    Public MustInherit Class TileComboClue

        Protected FTC As TileCombo
        Protected oRand As Random

        Public Sub New(ByVal t As TileCombo)
            FTC = t
            oRand = New Random
        End Sub

        MustOverride Function ClueText() As String
        MustOverride Function CluePertainsTo(ByVal t As TileCombo) As Boolean

        Function HalfTheTime() As Boolean
            Return oRand.Next(0, Int16.MaxValue) Mod 2 = 0
        End Function

        Public Function PositionalText(ByVal iPos As Integer) As String

            Dim iRand As Integer
            Dim s As String

            Select Case iPos
                Case 0
                    iRand = oRand.Next(0, Int16.MaxValue) Mod 3
                    Select Case iRand
                        Case 0
                            s &= "first"
                        Case 1
                            s &= "in the first position"
                        Case 2
                            s &= "in the top position"
                    End Select

                Case 1
                    If HalfTheTime() Then
                        s &= "second"
                    Else
                        s &= "in the second position"
                    End If

                Case 2
                    iRand = oRand.Next(0, Int16.MaxValue) Mod 3
                    Select Case iRand
                        Case 0
                            s &= "third"
                        Case 1
                            s &= "in the third position"
                        Case 2
                            s &= "in the second last position"
                    End Select

                Case 3
                    iRand = oRand.Next(0, Int16.MaxValue) Mod 5
                    Select Case iRand
                        Case 0
                            s &= "last"
                        Case 1
                            s &= "in the last position"
                        Case 2
                            s &= "fourth"
                        Case 3
                            s &= "in the fourth position"
                        Case 4
                            s &= "in the bottom position"
                    End Select
            End Select
            Return s

        End Function

    End Class

    'the s is c 
    'the c tile is a s
    Public Class ComboClueTheShapeIsColor
        Inherits TileComboClue

        Private FTile As ColoredShape

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)

            Dim oRand As New Random
            FTile = t.ColoredShape(oRand.Next(0, 4))

        End Sub

        Overrides Function ClueText() As String
            If HalfTheTime() Then
                Return "The " & FTile.ColorWord & " tile is a " & FTile.ShapeWord
            Else
                Return "The " & FTile.ShapeWord & " is " & FTile.ColorWord
            End If
        End Function

        'return true if the 
        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer

            For i = 0 To 3
                ocs = t.ColoredShape(i)
                If ocs.Color.Equals(FTile.Color) Then
                    Return ocs.ShapeWord.Equals(FTile.ShapeWord)
                End If
            Next

        End Function

    End Class

    'the s is not c
    'the c tile is not a s
    Public Class ComboClueTheShapeIsNOTColor
        Inherits TileComboClue

        Private FTileColor As ColoredShape  'contains the color to use
        Private FTileShape As ColoredShape  'contains the shape to use (2 must be different)

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)

            Dim oRand As New Random
            Dim i, j As Integer

            i = oRand.Next(0, 4)
            Do
                j = oRand.Next(0, 4)
            Loop Until i <> j

            FTileColor = t.ColoredShape(i)
            FTileShape = t.ColoredShape(j)
        End Sub

        Overrides Function ClueText() As String
            If HalfTheTime() Then
                Return "The " & FTileColor.ColorWord & " tile is not a " & FTileShape.ShapeWord
            Else
                Return "The " & FTileShape.ShapeWord & " is not " & FTileColor.ColorWord
            End If
        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer

            For i = 0 To 3
                ocs = t.ColoredShape(i)
                If ocs.Color.Equals(FTileColor.Color) Then
                    Return Not ocs.ShapeWord.Equals(FTileShape.ShapeWord)
                End If
            Next

        End Function

    End Class

    'the s is neither c1 nor c2
    'the c1 and c2 tiles are not a s
    Public Class ComboClueTheShapeIsNeitherColor
        Inherits TileComboClue

        Private FTileColorA As ColoredShape  'contains the first color to use
        Private FTileColorB As ColoredShape  'contains the second color to use
        Private FTileShape As ColoredShape  'contains the shape to use 

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)
            Dim i, j, k As Integer
            Dim oRand As New Random

            i = oRand.Next(0, 4)
            Do
                j = oRand.Next(0, 4)
            Loop Until i <> j

            Do
                k = oRand.Next(0, 4)
            Loop Until k <> i And k <> j

            FTileColorA = t.ColoredShape(i)
            FTileColorB = t.ColoredShape(j)
            FTileShape = t.ColoredShape(k)
        End Sub

        Overrides Function ClueText() As String

            Dim s As String

            If HalfTheTime() Then
                s = "The " & FTileColorA.ColorWord
                s &= " and " & FTileColorB.ColorWord & " tiles"
                s &= " are not the " & FTileShape.ShapeWord
            Else
                s = "The " & FTileShape.ShapeWord & " is neither "
                s &= FTileColorA.ColorWord & " nor "
                s &= FTileColorB.ColorWord
            End If

            Return s
        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer

            For i = 0 To 3
                ocs = t.ColoredShape(i)

                If ocs.ShapeWord.Equals(FTileShape.ShapeWord) Then
                    Return Not (ocs.Color.Equals(FTileColorA.Color) Or _
                            ocs.Color.Equals(FTileColorB.Color))
                End If
            Next

        End Function

    End Class

    'the c tile is neither a s1 nor a s2
    'the s1 and s2 tiles are not c
    Public Class ComboClueTheColorIsNeitherShape
        Inherits TileComboClue

        Private FTileShapeA As ColoredShape  'contains the first shape to use
        Private FTileShapeB As ColoredShape  'contains the second shape to use
        Private FTileColor As ColoredShape   'contains the color to use 

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)
            Dim i, j, k As Integer
            Dim oRand As New Random

            i = oRand.Next(0, 4)
            Do
                j = oRand.Next(0, 4)
            Loop Until i <> j

            Do
                k = oRand.Next(0, 4)
            Loop Until k <> i And k <> j

            FTileShapeA = t.ColoredShape(i)
            FTileShapeB = t.ColoredShape(j)
            FTileColor = t.ColoredShape(k)
        End Sub

        Overrides Function ClueText() As String

            Dim s As String

            If HalfTheTime() Then
                s = "The " & FTileShapeA.ShapeWord
                s &= " and " & FTileShapeB.ShapeWord & " tiles"
                s &= " are not " & FTileColor.ColorWord
            Else
                s = "The " & FTileColor.ColorWord & " tile is neither a "
                s &= FTileShapeA.ShapeWord & " nor a "
                s &= FTileShapeB.ShapeWord
            End If

            Return s
        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer

            For i = 0 To 3
                ocs = t.ColoredShape(i)

                If ocs.ColorWord.Equals(FTileColor.ColorWord) Then
                    Return Not (ocs.ShapeWord.Equals(FTileShapeA.ShapeWord) Or _
                            ocs.ShapeWord.Equals(FTileShapeB.ShapeWord))
                End If
            Next

        End Function

    End Class

    'the x is above y
    'the y is below x
    Public Class ComboClueXAboveY
        Inherits TileComboClue

        Private FTileTop As ColoredShape
        Private FTileBottom As ColoredShape

        Private FTopIsShape As Boolean
        Private FBottomIsShape As Boolean

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)
            Dim i, j, tmp As Integer
            Dim oRand As New Random

            i = oRand.Next(0, 4)
            Do
                j = oRand.Next(0, 4)
            Loop Until i <> j
            If j < i Then
                tmp = i
                i = j
                j = tmp
            End If

            FTileTop = t.ColoredShape(i)
            FTileBottom = t.ColoredShape(j)

            FTopIsShape = HalfTheTime()
            FBottomIsShape = HalfTheTime()
        End Sub

        Overrides Function ClueText() As String

            Dim s As String

            If HalfTheTime() Then
                s = "The " & IIf(FTopIsShape, FTileTop.ShapeWord, FTileTop.ColorWord & " tile")
                s &= " is above the "
                s &= IIf(FBottomIsShape, FTileBottom.ShapeWord, FTileBottom.ColorWord & " tile")
            Else
                s = "The " & IIf(FBottomIsShape, FTileBottom.ShapeWord, FTileBottom.ColorWord & " tile")
                s &= " is below the "
                s &= IIf(FTopIsShape, FTileTop.ShapeWord, FTileTop.ColorWord & " tile")
            End If

            Return s
        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer
            Dim iPos, jPos As Integer

            For i = 0 To 3
                ocs = t.ColoredShape(i)
                If FTopIsShape Then
                    If ocs.ShapeWord.Equals(FTileTop.ShapeWord) Then iPos = i
                Else
                    If ocs.ColorWord.Equals(FTileTop.ColorWord) Then iPos = i
                End If

                If FBottomIsShape Then
                    If ocs.ShapeWord.Equals(FTileBottom.ShapeWord) Then jPos = i
                Else
                    If ocs.ColorWord.Equals(FTileBottom.ColorWord) Then jPos = i
                End If
            Next

            Return iPos < jPos
        End Function

    End Class

    'the s/c is in the x position
    Public Class ComboClueInAPosition
        Inherits TileComboClue

        Private FTile As ColoredShape
        Private FIndex As Integer
        Private FIsShape As Boolean

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)
            Dim oRand As New Random

            FIsShape = HalfTheTime()
            FIndex = oRand.Next(0, 4)
            FTile = t.ColoredShape(FIndex)

        End Sub

        Overrides Function ClueText() As String

            Dim s As String
            Dim iRand As Integer

            s = "The "
            s &= IIf(FIsShape, FTile.ShapeWord, FTile.ColorWord & " tile")
            s &= " is " & Me.PositionalText(FIndex)

            Return s
        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer
            Dim iPos, jPos As Integer
            Dim r As Boolean

            ocs = t.ColoredShape(FIndex)
            If FIsShape Then
                r = ocs.ShapeWord.Equals(FTile.ShapeWord)
            Else
                r = ocs.ColorWord.Equals(FTile.ColorWord)
            End If

            Return r
        End Function

    End Class

    'the s/c is NOT in the x position
    Public Class ComboClueNOTInAPosition
        Inherits TileComboClue

        Private FTile As ColoredShape
        Private FIndex As Integer
        Private FIndexNot As Integer
        Private FIsShape As Boolean

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)
            Dim oRand As New Random

            FIsShape = HalfTheTime()
            FIndex = oRand.Next(0, 4)
            Do
                FIndexNot = oRand.Next(0, 4)
            Loop Until FIndex <> FIndexNot

            FTile = t.ColoredShape(FIndex)

        End Sub

        Overrides Function ClueText() As String

            Dim s As String

            s = "The "
            s &= IIf(FIsShape, FTile.ShapeWord, FTile.ColorWord & " tile")
            s &= " is not " & PositionalText(FIndexNot)

            Return s
        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer
            Dim iPos, jPos As Integer
            Dim r As Boolean

            ocs = t.ColoredShape(FIndexNot)
            If FIsShape Then
                r = Not ocs.ShapeWord.Equals(FTile.ShapeWord)
            Else
                r = Not ocs.ColorWord.Equals(FTile.ColorWord)
            End If

            Return r
        End Function

    End Class

    'the s/c has at least n tiles above/below
    'n = 1 or 2
    Public Class ComboClueTilesToTopBottom
        Inherits TileComboClue

        Private FTile As ColoredShape
        Private FIndex As Integer               'which tile to describe
        Private FIsShape As Boolean             'select color or shape
        Private FIsTop As Boolean               'top or bottom
        Private FUseTwo As Boolean              '1 or 2 

        Public Sub New(ByVal t As TileCombo)
            MyBase.New(t)
            Dim oRand As New Random

            FIsShape = HalfTheTime()
            FIsTop = HalfTheTime()

            If FIsTop Then
                FIndex = oRand.Next(1, 4)           'can't be leftmost

                Select Case FIndex
                    Case 1
                        FUseTwo = False
                    Case 2, 3
                        FUseTwo = HalfTheTime()
                    Case Else
                        Throw New ApplicationException("bad random index in " & Me.GetType.Name)
                End Select
            Else

                FIndex = oRand.Next(0, 3)           'can't be rightmost

                Select Case FIndex
                    Case 0, 1
                        FUseTwo = HalfTheTime()
                    Case 2, 3
                        FUseTwo = False
                    Case Else
                        Throw New ApplicationException("bad random index in " & Me.GetType.Name)
                End Select
            End If

            FTile = t.ColoredShape(FIndex)
        End Sub

        Overrides Function ClueText() As String

            Dim s As String
            Dim iRand As Integer
            Dim iNumToUse As Integer

            s = "The "
            s &= IIf(FIsShape, FTile.ShapeWord, FTile.ColorWord & " tile")
            s &= " has at least "
            s &= IIf(FUseTwo, "two tiles", "one tile")
            s &= IIf(FIsTop, " above", " below")
            s &= " it"

            Return s

        End Function

        Overrides Function CluePertainsTo(ByVal t As TileCombo) As Boolean

            Dim ocs As ColoredShape
            Dim i As Integer
            Dim iPos As Integer
            Dim r As Boolean

            For i = 0 To 3
                ocs = t.ColoredShape(i)
                If FIsShape Then
                    If ocs.ShapeWord.Equals(FTile.ShapeWord) Then iPos = i
                Else
                    If ocs.ColorWord.Equals(FTile.ColorWord) Then iPos = i
                End If
            Next

            If FIsTop Then
                Select Case iPos
                    Case 0
                        r = False
                    Case 1
                        r = Not FUseTwo
                    Case Else
                        r = True
                End Select
            Else
                Select Case iPos
                    Case 3
                        r = False
                    Case 2
                        r = Not FUseTwo
                    Case Else
                        r = True
                End Select
            End If

            Return r
        End Function

    End Class

End Namespace

