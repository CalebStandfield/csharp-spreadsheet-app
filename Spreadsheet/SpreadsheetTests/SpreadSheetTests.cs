// Testing by Caleb Standfield
// Date, 09/26/24

using CS3500.Formula;
using CS3500.Spreadsheet;

namespace SpreadsheetTests;

/// <summary>
///   <para>
///     Tester class for SpreadSheet, ensures all methods method work as intended
///   </para>
/// </summary>
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
        s.SetContentsOfCell("a1", "1");
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 1);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string> { "A1" }));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_OneElementA1_Count1AndListOfA1()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 1);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string> { "A1" }));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_MultipleElements_CountAndListOfElements()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        s.SetContentsOfCell("B1", "2");
        s.SetContentsOfCell("C1", "3");
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string> { "A1", "B1", "C1" }));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells_MultipleElementsOfDifferentContents_CountAndListOfElements()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        s.SetContentsOfCell("B1", "C");
        s.SetContentsOfCell("C1", new Formula("2 + 2").ToString());
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string> { "A1", "B1", "C1" }));
    }

    [TestMethod]
    public void GetNamesOfAllNonemptyCells__CountAndListOfElements()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        s.SetContentsOfCell("B1", "beans");
        s.SetContentsOfCell("C1", new Formula("2 + 2").ToString());
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.IsTrue(a1Set.SequenceEqual(new HashSet<string> { "A1", "B1", "C1" }));
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
        s.SetContentsOfCell("A1", "1");
        Assert.AreEqual(s.GetCellContents("A1"), 1.0);
    }

    [TestMethod]
    public void GetCellContents_CellWithTextContent_TextContent()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "C");
        Assert.AreEqual(s.GetCellContents("A1"), "C");
    }

    [TestMethod]
    public void GetCellContents_CellWithFormulaContent_FormulaContent()
    {
        var s = new Spreadsheet();
        var f = new Formula("2 + 2");
        s.SetContentsOfCell("A1", "=2 + 2");
        Assert.AreEqual(s.GetCellContents("A1"), f);
    }

    [TestMethod]
    public void GetCellContents_CellAccessNotNormalizedName_CellAccessNotNormalizedName()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("a1", "1");
        Assert.AreEqual(s.GetCellContents("A1"), 1.0);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContents_CellAccessWithInvalidName_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("C1", "1");
        Assert.AreEqual(s.GetCellContents("c"), 1.0);
    }

    [TestMethod]
    public void GetCellContents_ContentsDifferAfterSet_NewContentsPresentOldNotPresent()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("C1", "1");
        Assert.AreEqual(s.GetCellContents("C1"), 1.0);
        s.SetContentsOfCell("C1", "2");
        Assert.AreEqual(s.GetCellContents("C1"), 2.0);
        Assert.AreNotEqual(s.GetCellContents("C1"), 1.0);
    }

    #endregion

    // --- SetCellContents ---

    #region SetCellContents

    #region SetCellContentsDouble

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContents_CellAccessInvalidNameDouble_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("C", "1");
    }

    [TestMethod]
    public void SetCellContents_NoDependentsDouble_ListOfItself()
    {
        var s = new Spreadsheet();
        Assert.IsTrue(s.SetContentsOfCell("C1", "1").SequenceEqual(new List<string> { "C1" }));
    }

    [TestMethod]
    public void SetCellContents_XmlCommentForDoubleSetCell_ListIsAccurateAccordingToXmlComment()
    {
        var s = new Spreadsheet();
        const string b1 = "=A1 + 2";
        const string c1 = "=B1 + A1";
        s.SetContentsOfCell("B1", b1);
        s.SetContentsOfCell("C1", c1);
        Assert.IsTrue(s.SetContentsOfCell("A1", "1").SequenceEqual(new List<string> { "A1", "B1", "C1" }));
    }

    [TestMethod]
    public void SetCellContents_OneDependentOneDependee_ListOfItselfThenDependee()
    {
        var s = new Spreadsheet();
        const string a1 = "=B1 + 1";
        Assert.IsTrue(s.SetContentsOfCell("A1", a1).SequenceEqual(new List<string> { "A1" }));
        Assert.IsTrue(s.SetContentsOfCell("B1", "1").SequenceEqual(new List<string> { "B1", "A1" }));
    }

    #endregion

    #region SetCellContentsText

    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetCellContents_CellAccessInvalidNameText_ThrowsInvalidNameException()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("C", "C");
    }

    [TestMethod]
    public void SetCellContents_NoDependentsText_ListOfItself()
    {
        var s = new Spreadsheet();
        Assert.IsTrue(s.SetContentsOfCell("C1", "C").SequenceEqual(new List<string> { "C1" }));
    }

    [TestMethod]
    public void SetCellContents_SetToEmptyString_ContentsIsEmptyString()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("C1", "C");
        Assert.AreEqual(s.GetCellContents("C1"), "C");
        s.SetContentsOfCell("C1", string.Empty);
        Assert.AreEqual(s.GetCellContents("C1"), string.Empty);
    }

    [TestMethod]
    public void SetCellContents_SetToEmptyString_CellNoLongerAppearsInGetNonemptyCells()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("C1", "C");
        Assert.AreEqual(s.GetCellContents("C1"), "C");
        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().SequenceEqual(new List<string> { "C1" }));
        s.SetContentsOfCell("C1", string.Empty);
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
        s.SetContentsOfCell("C", new Formula("2 + 2").ToString());
    }

    [TestMethod]
    public void SetCellContents_NoDependentsFormula_ListOfItself()
    {
        var s = new Spreadsheet();
        var f = new Formula("2 + 2");
        Assert.IsTrue(s.SetContentsOfCell("C1", f.ToString()).SequenceEqual(new List<string> { "C1" }));
    }

    [TestMethod]
    public void SetCellContents_SingleDependentFormula_ListOfS1AndC1()
    {
        var s = new Spreadsheet();
        const string f = "=S1 + 2";
        Assert.IsTrue(s.SetContentsOfCell("C1", f).SequenceEqual(new List<string> { "C1" }));
        Assert.IsTrue(s.SetContentsOfCell("S1", "1").SequenceEqual(new List<string> { "S1", "C1" }));
    }

    [TestMethod]
    public void SetCellContents_MultiDependentFormula_ListDependees()
    {
        var s = new Spreadsheet();
        const string a1 = "=B1 + C1";
        const string b1 = "=1 + C1";
        Assert.IsTrue(s.SetContentsOfCell("A1", a1).SequenceEqual(new List<string> { "A1" }));
        Assert.IsTrue(s.SetContentsOfCell("B1", b1).SequenceEqual(new List<string> { "B1", "A1" }));
        Assert.IsTrue(s.SetContentsOfCell("C1", "1").SequenceEqual(new List<string> { "C1", "B1", "A1" }));
    }

    [TestMethod]
    public void SetCellContents_GetCellsToRecalculateMethodXmlExample_ListDependents()
    {
        var s = new Spreadsheet();
        const string f1 = "=5";
        s.SetContentsOfCell("A1", f1);
        const string f2 = "=A1 + 2";
        s.SetContentsOfCell("B1", f2);
        const string f3 = "=A1 + B1";
        s.SetContentsOfCell("C1", f3);
        const string f4 = "=A1 * 7";
        s.SetContentsOfCell("D1", f4);
        const string f5 = "=15";
        s.SetContentsOfCell("E1", f5);
        Assert.IsTrue(s.SetContentsOfCell("A1", f1).SequenceEqual(new List<string> { "A1", "D1", "B1", "C1" }));
    }

    [TestMethod]
    public void SetCellContents_A1IsRemoved_B1sListChangedToReflectThat()
    {
        var s = new Spreadsheet();
        const string f1 = "=B1 + 2";
        s.SetContentsOfCell("A1", f1);
        Assert.IsTrue(s.SetContentsOfCell("A1", f1).SequenceEqual(new List<string> { "A1" }));
        Assert.IsTrue(s.SetContentsOfCell("B1", "2").SequenceEqual(new List<string> { "B1", "A1" }));
        s.SetContentsOfCell("A1", string.Empty);
        Assert.IsTrue(s.SetContentsOfCell("B1", "2").SequenceEqual(new List<string> { "B1" }));
    }

    [TestMethod]
    public void SetCellContents_C1GetsChanged_ReturnListNoLongerContainsC1()
    {
        var s = new Spreadsheet();
        const string f1 = "=5";
        s.SetContentsOfCell("A1", f1);
        const string f2 = "=A1 + 2";
        s.SetContentsOfCell("B1", f2);
        const string f3 = "=A1 + B1";
        s.SetContentsOfCell("C1", f3);
        const string f4 = "=A1 * 7";
        s.SetContentsOfCell("D1", f4);
        const string f5 = "=15";
        s.SetContentsOfCell("E1", f5);
        Assert.IsTrue(s.SetContentsOfCell("A1", f1).SequenceEqual(new List<string> { "A1", "D1", "B1", "C1" }));
        s.SetContentsOfCell("C1", "Hi");
        Assert.IsTrue(s.SetContentsOfCell("A1", f1).SequenceEqual(new List<string> { "A1", "D1", "B1" }));
    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CycleAtStartOfFormula_ThrowsCircularException()
    {
        var s = new Spreadsheet();
        const string f1 = "=A1 + B1";
        s.SetContentsOfCell("A1", f1);
    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CycleAtEndOfFormula_ThrowsCircularException()
    {
        var s = new Spreadsheet();
        const string f1 = "=B1 + C1 + D1 + E1 + F1 + G1 + A1";
        s.SetContentsOfCell("A1", f1);
    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetCellContents_CycleAtEndOfFormulaTree_ThrowsCircularException()
    {
        var s = new Spreadsheet();
        const string a1 = "=1 + B1";
        const string b1 = "=2 + C1";
        const string c1 = "=3 + D1";
        const string d1 = "=4 + A1";
        s.SetContentsOfCell("A1", a1);
        s.SetContentsOfCell("B1", b1);
        s.SetContentsOfCell("C1", c1);
        s.SetContentsOfCell("D1", d1);
    }

    [TestMethod]
    public void SetCellContents_LongTreeOfConnectingFormulas_ListGoesInOrderBottomToTop()
    {
        var s = new Spreadsheet();
        const string a1 = "=1 + B1";
        const string b1 = "=2 + C1";
        const string c1 = "=3 + D1";
        const string d1 = "=4 + E1";
        s.SetContentsOfCell("A1", a1);
        s.SetContentsOfCell("B1", b1);
        s.SetContentsOfCell("C1", c1);
        s.SetContentsOfCell("D1", d1);
        Assert.IsTrue(s.SetContentsOfCell("E1", "1").SequenceEqual(new List<string> { "E1", "D1", "C1", "B1", "A1" }));
    }

    [TestMethod]
    public void SetCellContents_CircularExceptionDoesNotCauseChangeToGraphContents_NoChange()
    {
        var s = new Spreadsheet();
        const string a1 = "=A1 + 1";
        s.SetContentsOfCell("A1", "1");
        Assert.AreEqual(s.GetCellContents("A1"), 1.0);
        Assert.ThrowsException<CircularException>(() => s.SetContentsOfCell("A1", a1));
        Assert.AreEqual(s.GetCellContents("A1"), 1.0);
    }

    [TestMethod]
    public void SetCellContents_CircularExceptionDoesNotCauseChangeToGraphListOfDependencies_NoChange()
    {
        var s = new Spreadsheet();
        const string f1 = "=5";
        s.SetContentsOfCell("A1", f1);
        const string f2 = "=A1 + 2";
        s.SetContentsOfCell("B1", f2);
        const string f3 = "=A1 + B1";
        s.SetContentsOfCell("C1", f3);
        const string f4 = "=A1 * 7";
        s.SetContentsOfCell("D1", f4);
        const string f5 = "=15";
        s.SetContentsOfCell("E1", f5);
        Assert.IsTrue(s.SetContentsOfCell("A1", f1).SequenceEqual(new List<string> { "A1", "D1", "B1", "C1" }));
        Assert.ThrowsException<CircularException>(() => s.SetContentsOfCell("C1", "=C1"));
        Assert.IsTrue(s.SetContentsOfCell("A1", f1).SequenceEqual(new List<string> { "A1", "D1", "B1", "C1" }));
    }

    #endregion

    #endregion
    
    // --- GetCellValue ---
    
    #region GetCellValue

    #region GetCellValueMethod
    
    [TestMethod]
    public void GetCellValueMethod_UninitializedCell_ReturnsEmptyString()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        Assert.AreEqual(string.Empty, s.GetCellValue("B1").ToString());
    }

    [TestMethod]
    public void GetCellValueMethod_Double_ReturnsDouble()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        Assert.AreEqual("1", s.GetCellValue("A1").ToString());
    }
    
    [TestMethod]
    public void GetCellValueMethod_String_ReturnsString()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "Hello");
        Assert.AreEqual("Hello", s.GetCellValue("A1").ToString());
    }
    
    [TestMethod]
    public void GetCellValueMethod_CompleteCorrectFormula_ReturnsDoubleValue()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=2 + 2");
        Assert.AreEqual(4, s.GetCellValue("A1"));
    }
    
    #endregion

    #region GetCellValueDirrectAccess
    
    [TestMethod]
    public void GetCellValueAccess_Double_ReturnsDouble()
    {
        var s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1");
        Assert.AreEqual("1", s["A1"].ToString());
    }
    
    #endregion

    #endregion
}