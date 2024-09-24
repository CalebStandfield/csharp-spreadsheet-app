using CS3500.Formula;
using CS3500.Spreadsheet;

namespace SpreadsheetTests;

[TestClass]
public class SpreadSheetTests
{

    #region GetNamesOfAllNoneemptyCells
    
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
        s.SetCellContents("B1", "beans");
        s.SetCellContents("C1", new Formula("2 + 2"));
        Assert.AreEqual(s.GetNamesOfAllNonemptyCells().Count, 3);
        var a1Set = s.GetNamesOfAllNonemptyCells();
        Assert.AreEqual(a1Set.ToString(), new HashSet<string>{"A1", "B1", "C1"}.ToString());
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
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
}