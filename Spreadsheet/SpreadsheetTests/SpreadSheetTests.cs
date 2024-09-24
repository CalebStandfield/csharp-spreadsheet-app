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
        Assert.AreEqual(emptySet.ToString(), new HashSet<string>().ToString());
    }
    
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_OneElementA1NameIsNormalized_Count1AndListOfA1()
    {
        var s = new Spreadsheet();
        s.SetCellContents("a1", 1);
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 1);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.AreEqual(a1Set.ToString(), new HashSet<string>{"A1"}.ToString());
    }
    
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_OneElementA1_Count1AndListOfA1()
    {
        var s = new Spreadsheet();
        s.SetCellContents("A1", 1);
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 1);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.AreEqual(a1Set.ToString(), new HashSet<string>{"A1"}.ToString());
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
        Assert.AreEqual(a1Set.ToString(), new HashSet<string>{"A1", "B1", "C1"}.ToString());
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
        Assert.AreEqual(a1Set.ToString(), new HashSet<string>{"A1", "B1", "C1"}.ToString());
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
        Assert.AreEqual(a1Set.ToString(), new HashSet<string>{"A1", "B1", "C1"}.ToString());
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
    
    #endregion
}