using CS3500.Formula;
using CS3500.Spreadsheet;

namespace SpreadsheetTests;

[TestClass]
public class SpreadSheetTests
{
    // --- GetNamesOfAllNonemptyCells Tests ---

    #region GetNamesOfAllNonemptyCells

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_NewSpreadSheet_Count0AndEmptyList()
    {
        var s = new Spreadsheet();
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 0);
        var emptySet = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(emptySet.SequenceEqual(new HashSet<string>()));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_OneElementA1NameIsNormalized_Count1AndListOfA1()
    {
        var s = new Spreadsheet();
        s.SetCellContents("a1", 1);
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 1);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string>{"A1"}));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_OneElementA1_Count1AndListOfA1()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", 1);
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 1);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string>{"A1"}));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_MultipleElements_CountAndListOfElements()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", 1);
        s.SetCellContents("B1", 2);
        s.SetCellContents("C1", 3);
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string>{"A1", "B1", "C1"}));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_MultipleElementsOfDifferentContents_CountAndListOfElements()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", 1);
        s.SetCellContents("B1", "C");
        s.SetCellContents("C1", new Formula("2 + 2"));
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string>{"A1", "B1", "C1"}));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells__CountAndListOfElements()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", 1);
        s.SetCellContents("B1", "beans");
        s.SetCellContents("C1", new Formula("2 + 2"));
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string>{"A1", "B1", "C1"}));
    }

    #endregion

    // --- GetCellContents Tests ---

    #region GetCellContents

    [TestMethod]
    public void GetCellContents_CellWithNoContents_EmptyString()
    {
        var s = new Spreadsheet();
        Assert.AreEqual(s.GetCellContents("A1"), string.Empty);
    }

    [TestMethod]
    public void GetCellContents_CellWithDoubleContent_DoubleContent()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", 1);
        Assert.AreEqual(s.GetCellContents("A1"), 1.0);
    }

    [TestMethod]
    public void GetCellContents_CellWithTextContent_TextContent()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", "C");
        Assert.AreEqual(s.GetCellContents("A1"), "C");
    }

    [TestMethod]
    public void GetCellContents_CellWithFormulaContent_FormulaContent()
    {
        var s = new Spreadsheet();
        var f = new Formula("2 + 2");
        s.SetCellContents("A1", f);
        Assert.AreEqual(s.GetCellContents("A1"), f);
    }

    [TestMethod]
    public void GetCellContents_CellAccessNotNormalizedName_CellAccessNotNormalizedName()
    {
        var s = new Spreadsheet();
        s.SetCellContents("a1", 1);
        Assert.AreEqual(s.GetCellContents("A1"), 1.0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContents_CellAccessWithInvalidName_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C1", 1);
        Assert.AreEqual(s.GetCellContents("c"), 1.0);
    }

    [TestMethod]
    public void GetCellContents_ContentsDifferAfterSet_NewContentsPresentOldNotPresent()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C1", 1);
        Assert.AreEqual(s.GetCellContents("C1"), 1.0);
        s.SetCellContents("C1", 2);
        Assert.AreEqual(s.GetCellContents("C1"), 2.0);
        Assert.AreNotEqual(s.GetCellContents("C1"), 1.0);
    }

    #endregion

    // --- SetCellContents

    #region SetCellContents

    #region SetCellContentsDouble

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContents_CellAccessInvalidNameDouble_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C", 1);
    }

    [TestMethod]
    public void SetCellContents_NoDependentsDouble_ListOfItself()
    {
        var s = new Spreadsheet();
        Assert.IsTrue(s.SetCellContents("C1", 1).SequenceEqual(new List<string>{"C1"}));
    }
    
    [TestMethod]
    public void SetCellContents_XmlCommentForDoubleSetCell_ListIsAccurateAccordingToXmlComment()
    {
        var s = new Spreadsheet();
        var b1 = new Formula("A1 * 2");
        var c1 = new Formula("B1 + A1");
        s.SetCellContents("B1", b1);
        s.SetCellContents("C1", c1);
        Assert.IsTrue(s.SetCellContents("A1", 1).SequenceEqual(new List<string>{"A1", "B1", "C1"}));
        
    }
    
    [TestMethod]
    public void SetCellContents_OneDependentOneDependee_ListOfItselfThenDependee()
    {
        var s = new Spreadsheet();
        var a1 = new Formula("B1 + 1");
        Assert.IsTrue(s.SetCellContents("A1", a1).SequenceEqual(new List<string>{"A1"}));
        Assert.IsTrue(s.SetCellContents("B1", 1).SequenceEqual(new List<string>{"B1", "A1"}));
    }

    #endregion

    #region SetCellContentsText

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContents_CellAccessInvalidNameText_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C", "C");
    }

    [TestMethod]
    public void SetCellContents_NoDependentsText_ListOfItself()
    {
        var s = new Spreadsheet();
        Assert.IsTrue(s.SetCellContents("C1", "C").SequenceEqual(new List<string>{"C1"}));
    }
    
    [TestMethod]
    public void SetCellContents_SetToEmptyString_ContentsIsEmptyString()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C1", "C");
        Assert.AreEqual(s.GetCellContents("C1"), "C");
        s.SetCellContents("C1", string.Empty);
        Assert.AreEqual(s.GetCellContents("C1"), string.Empty);
    }
    
    [TestMethod]
    public void SetCellContents_SetToEmptyString_CellNoLongerAppearsInGetNonemptyCells()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C1", "C");
        Assert.AreEqual(s.GetCellContents("C1"), "C");
        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().SequenceEqual(new List<string>{"C1"}));
        s.SetCellContents("C1", string.Empty);
        Assert.AreEqual(s.GetCellContents("C1"), string.Empty);
        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().SequenceEqual(new List<string>()));
    }

    #endregion

    #region SetCellContentsFormula

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContents_CellAccessInvalidNameFormula_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetCellContents("C", new Formula("2 + 2"));
    }

    [TestMethod]
    public void SetCellContents_NoDependentsFormula_EmptyList()
    {
        var s = new Spreadsheet();
        var f = new Formula("2 + 2");
        Assert.AreEqual(s.SetCellContents("C1", f).ToString(), new List<string>().ToString());
    }
    
    [TestMethod]
    public void SetCellContents_SingleDependentFormula_ListOfS1()
    {
        var s = new Spreadsheet();
        var f = new Formula("S1 + 2");
        Assert.AreEqual(s.SetCellContents("C1", f).ToString(), new List<string>{"S1"}.ToString());
    }
    
    [TestMethod]
    public void SetCellContents_MultiDependentFormula_ListDependents()
    {
        var s = new Spreadsheet();
        var f = new Formula("S1 + S2");
        Assert.AreEqual(s.SetCellContents("C1", f).ToString(), new List<string>{"S1", "S2"}.ToString());
    }
    
    [TestMethod]
    public void SetCellContents_ChainDependentFormula_ListDependents()
    {
        var s = new Spreadsheet();
        var f1 = new Formula("S1 + S2");
        var f2 = new Formula("B1 + Z3");
        s.SetCellContents("C1", f1);
        s.SetCellContents("S2", f2);
        Assert.AreEqual(s.SetCellContents("C1", f1).ToString(), new List<string>{"S1", "S2", "B1", "Z3"}.ToString());
    }
    
    [TestMethod]
    public void SetCellContents_GetCellsToRecalculateMethodXmlExample_ListDependents()
    {
        var s = new Spreadsheet();
        var f1 = new Formula("5");
        s.SetCellContents("A1", f1);
        var f2 = new Formula("A1 + 2");
        s.SetCellContents("B1", f2);
        var f3 = new Formula("A1 + B1");
        s.SetCellContents("C1", f3);
        var f4 = new Formula("A1 * 7");
        s.SetCellContents("D1", f4);
        var f5 = new Formula("15");
        s.SetCellContents("E1", f5);
        Assert.AreEqual(s.SetCellContents("A1", f1).ToString(), new List<string>{"A1", "B1", "C1", "D1"}.ToString());
    }
    
    [TestMethod]
    public void SetCellContents_A1sListIsEmptyAfterChange_ListDependents()
    {
        var s = new Spreadsheet();
        var f1 = new Formula("B1 + C1");
        var f2 = new Formula("C1 + D1");
        s.SetCellContents("A1", f1);
        s.SetCellContents("B1", f2);
        Assert.AreEqual(s.SetCellContents("A1", f1).ToString(), new List<string>{"A1", "B1", "C1", "D1"}.ToString());
        Assert.AreEqual(s.SetCellContents("A1", new Formula("5")).ToString(), new List<string>().ToString());
    }
    
    [TestMethod]
    public void SetCellContents_C1GetsChanged_ReturnListNoLongerUpdatesC1()
    {
        var s = new Spreadsheet();
        var f1 = new Formula("5");
        s.SetCellContents("A1", f1);
        var f2 = new Formula("A1 + 2");
        s.SetCellContents("B1", f2);
        var f3 = new Formula("A1 + B1");
        s.SetCellContents("C1", f3);
        var f4 = new Formula("A1 * 7");
        s.SetCellContents("D1", f4);
        var f5 = new Formula("15");
        s.SetCellContents("E1", f5);
        Assert.AreEqual(s.SetCellContents("A1", f1).ToString(), new List<string>{"A1", "B1", "C1", "D1"}.ToString());
        s.SetCellContents("C1", "Hi");
        Assert.AreEqual(s.SetCellContents("A1", f1).ToString(), new List<string>{"A1", "B1", "D1"}.ToString());
    }
    
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CycleAtStartOfFormula_ThrowsCircularException()
    {
        var s = new Spreadsheet();
        var f1 = new Formula("A1 + B1");
        s.SetCellContents("A1", f1);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CycleAtEndOfFormula_ThrowsCircularException()
    {
        var s = new Spreadsheet();
        var f1 = new Formula("B1 + C1 + D1 + E1 + F1 + G1 + A1");
        s.SetCellContents("A1", f1);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CycleAtEndOfFormulaTree_ThrowsCircularException()
    {
        var s = new Spreadsheet();
        var a1 = new Formula("1 + B1");
        var b1 = new Formula("2 + C1");
        var c1 = new Formula("3 + D1");
        var d1 = new Formula("4 + A1");
        s.SetCellContents("A1", a1);
        s.SetCellContents("B1", b1);
        s.SetCellContents("C1", c1);
        s.SetCellContents("D1", d1);
    }
    
    [TestMethod]
    public void SetCellContents_LongTreeOfConnectingFormulas_ListAllDependents()
    {
        var s = new Spreadsheet();
        var a1 = new Formula("1 + B1");
        var b1 = new Formula("2 + C1");
        var c1 = new Formula("3 + D1");
        var d1 = new Formula("4 + E1");
        s.SetCellContents("A1", a1);
        s.SetCellContents("B1", b1);
        s.SetCellContents("C1", c1);
        s.SetCellContents("D1", d1);
        Assert.AreEqual(s.SetCellContents("E1", 1).ToString(), new List<string>{"A1", "B1", "C1", "D1", "E1"}.ToString());
    }

    #endregion

    #endregion
}