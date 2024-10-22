// <authors> [Ethan Perkins] </authors>
// <date> [10/18/2024] </date>

using CS3500.Formula;
using CS3500.Spreadsheet;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SpreadsheetTests;

/// <summary>
/// This is a test class for Spreadsheet and is intended
/// to contain all Spreadsheet Unit Tests
/// </summary>
[TestClass]
public class SpreadsheetTests
{
    #region --- GetNamesOfAllNonemptyCells Tests ---

    /// <summary>
    /// Tests if a spreadsheet is constructed empty.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_AllEmpty_ReturnsEmpty()
    {
        Spreadsheet a = new();
        Assert.AreEqual(a.GetNamesOfAllNonemptyCells().Count, 0);
    }

    /// <summary>
    /// Tests if one cell is returned from adding in one cell.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_OneFilled_ReturnsOne()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        Assert.IsTrue(a.GetNamesOfAllNonemptyCells().SequenceEqual(new HashSet<string> { "A1" }));
    }

    /// <summary>
    /// Tests if multiple cells are returned from adding in multiple cells.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_TwoFilled_ReturnsBoth()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "2");
        Assert.IsTrue(a.GetNamesOfAllNonemptyCells().SequenceEqual(new HashSet<string> { "A1", "B2" }));
    }

    /// <summary>
    /// Tests if cells set to an empty string are read as empty.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_SetToEmptyString_ReturnsEmpty()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", string.Empty);
        a.SetContentsOfCell("B2", "");
        Assert.AreEqual(a.GetNamesOfAllNonemptyCells().Count, 0);
    }

    /// <summary>
    /// Tests if the removal of a cell (set to empty) is read as empty.
    /// </summary>
    [TestMethod]
    public void GetNamesOfAllNonemptyCells_Remove_ReturnsEmpty()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("A1", "");
        Assert.AreEqual(a.GetNamesOfAllNonemptyCells().Count, 0);
    }

    #endregion

    #region --- General SetContentsOfCell + GetCellContents Tests ---

    /// <summary>
    /// Tests if doubles can be set in and got from cells.
    /// </summary>
    [TestMethod]
    public void SetGetCellContents_TryDouble_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "8e1");
        Assert.AreEqual(a.GetCellContents("A1"), 8.0);
        Assert.AreEqual(a.GetCellContents("B2"), 80.0);
    }

    /// <summary>
    /// Tests if strings can be set in and got from cells.
    /// </summary>
    [TestMethod]
    public void SetGetCellContents_TryString_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "Hello!");
        a.SetContentsOfCell("B2", string.Empty);
        Assert.AreEqual(a.GetCellContents("A1"), "Hello!");
        Assert.AreEqual(a.GetCellContents("B2"), "");
    }

    /// <summary>
    /// Tests if formulas can be set in and got from cells.
    /// </summary>
    [TestMethod]
    public void SetGetCellContents_TryFormula_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=1+2");
        a.SetContentsOfCell("B2", "=A1");
        Assert.IsTrue(a.GetCellContents("A1").Equals(new Formula("1+2")));
        Assert.IsTrue(a.GetCellContents("B2").Equals(new Formula("A1")));
    }

    #endregion

    #region --- SetContentsOfCell Tests ---

    /// <summary>
    /// Tests if SetContentsOfCell throws when given an invalid variable name, setting using a double.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCell_InvalidNameDouble_Throws()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("ddd", "1");
    }

    /// <summary>
    /// Tests if SetContentsOfCell throws when given an invalid variable name, setting using a formula.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCell_InvalidNameFormula_Throws()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("fff", "=1");
    }

    /// <summary>
    /// Tests if SetContentsOfCell throws when given an invalid variable name, setting using a string.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void SetContentsOfCell_InvalidNameString_Throws()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("sss", "1");
    }

    /// <summary>
    /// Tests if SetContentsOfCell does not throw when using a lowercase variable.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_LowercaseVariable_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("a1", "8");
        Assert.AreEqual(a.GetCellContents("A1"), (double)8);
    }

    /// <summary>
    /// Tests if SetContentsOfCell returns a list with the current cell even if the cell is set to empty.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_EmptySet_ReturnsItself()
    {
        Spreadsheet a = new();
        Assert.IsTrue(a.SetContentsOfCell("A1", string.Empty).SequenceEqual(new List<string> { "A1" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell returns a list with the current cell.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_NoDependencies_ReturnsOne()
    {
        Spreadsheet a = new();
        Assert.IsTrue(a.SetContentsOfCell("A1", "8.0").SequenceEqual(new List<string> { "A1" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell returns a list with one dependency in the correct order.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_OneDependency_ReturnsTwo()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        Assert.IsTrue(a.SetContentsOfCell("B2", "=A1+2").SequenceEqual(new List<string> { "B2" }));
        Assert.IsTrue(a.SetContentsOfCell("A1", "6").SequenceEqual(new List<string> { "A1", "B2" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell returns a list with one dependency in the correct order, even if the current cell is set to empty.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_SetToEmpty_ReturnsTwo()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+2");
        Assert.IsTrue(a.SetContentsOfCell("A1", string.Empty).SequenceEqual(new List<string> { "A1", "B2" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell returns a list with dependencies in the correct order, using doubles.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_MultipleDependenciesWithDoubles_ReturnsCorrectOrder()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+2");
        Assert.IsTrue(a.SetContentsOfCell("C3", "=A1+B2").SequenceEqual(new List<string> { "C3" }));
        Assert.IsTrue(a.SetContentsOfCell("B2", "=A1+3").SequenceEqual(new List<string> { "B2", "C3" }));
        Assert.IsTrue(a.SetContentsOfCell("A1", "8").SequenceEqual(new List<string> { "A1", "B2", "C3" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell returns a list with dependencies in the correct order, using strings.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_MultipleDependenciesWithStrings_ReturnsCorrectOrder()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "Good morning");
        a.SetContentsOfCell("B2", "=A1+2");
        Assert.IsTrue(a.SetContentsOfCell("C3", "=A1+B2").SequenceEqual(new List<string> { "C3" }));
        Assert.IsTrue(a.SetContentsOfCell("B2", "=A1+3").SequenceEqual(new List<string> { "B2", "C3" }));
        Assert.IsTrue(a.SetContentsOfCell("A1", "Hello there!").SequenceEqual(new List<string> { "A1", "B2", "C3" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell throws if it detects a loop.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_Circular3Loop_Throws()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=B2");
        a.SetContentsOfCell("B2", "=C3");
        a.SetContentsOfCell("C3", "=A1");
    }

    /// <summary>
    /// Tests if SetContentsOfCell throws if it detects a loop of one variable (to itself).
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void SetContentsOfCell_SelfLoop_Throws()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=A1");
    }

    /// <summary>
    /// Tests if SetContentsOfCell keeps the Spreadsheet the same in case of a loop.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_Circular3Loop_DoesNotChangeSpreadsheet()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "15");
        a.SetContentsOfCell("B2", "=C3");
        a.SetContentsOfCell("C3", "=A1");
        Assert.ThrowsException<CircularException>(() => a.SetContentsOfCell("A1", "=B2"));
        Assert.AreEqual(a.GetCellContents("A1"), (double)15);
    }

    /// <summary>
    /// Tests if the example given in GetCellsToRecalculate is done properly.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_XMLExample_ReturnsCorrectOrder()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "5");
        a.SetContentsOfCell("B1", "=A1+2");
        a.SetContentsOfCell("C1", "=A1+B1");
        a.SetContentsOfCell("D1", "=A1*7");
        a.SetContentsOfCell("E1", "=15");
        bool bl = false;
        bool c1seen = false;
        //This checks if the order of the cells is correct (D1 can appear anywhere in the list after A1, but B1 has to be between A1 and C1)
        foreach (string name in a.SetContentsOfCell("A1", "5"))
        {
            if (name.Equals("B1")&&!c1seen){
                bl = true; break;
            }
            if (name.Equals("C1"))
            {
                c1seen = true;
            }
        }
        Assert.IsTrue(bl);
        Assert.IsTrue(a.SetContentsOfCell("A1", "5")[0]=="A1");

    }

    #endregion

    #region --- SetContentsOfCell Updating Value Tests ---

    /// <summary>
    /// Tests if SetContentsOfCell updates values when the Formula is changed.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_ChangeFormula_Returns()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+3");
        Assert.AreEqual((double)11, a.GetCellValue("B2"));
        a.SetContentsOfCell("B2", "=A1+A1");
        Assert.AreEqual((double)16, a.GetCellValue("B2"));
    }

    /// <summary>
    /// Tests if SetContentsOfCell updates values when a new Formula is introduced.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_NewFormula_Returns()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+3");
        a.SetContentsOfCell("C3", "=15");
        a.SetContentsOfCell("A1", "=5+C3");
        Assert.AreEqual((double)20, a.GetCellValue("A1"));
        Assert.AreEqual((double)23, a.GetCellValue("B2"));
    }

    /// <summary>
    /// Tests if SetContentsOfCell updates values when a double is changed to a string.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_ChangeDoubleToString_Returns()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+3");
        Assert.AreEqual((double)11, a.GetCellValue("B2"));
        a.SetContentsOfCell("A1", "hello!");
        Assert.IsTrue(a.GetCellValue("B2") is FormulaError);
    }

    /// <summary>
    /// Tests if SetContentsOfCell updates values when a Formula is changed to a string.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_ChangeFormulaToString_Returns()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+3");
        a.SetContentsOfCell("C3", "=B2+3");
        Assert.AreEqual((double)14, a.GetCellValue("C3"));
        a.SetContentsOfCell("B2", "hello!");
        Assert.IsTrue(a.GetCellValue("C3") is FormulaError);
    }

    /// <summary>
    /// Tests if SetContentsOfCell updates values when a Formula is changed to a double.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_ChangeFormulaToDouble_Returns()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+3");
        a.SetContentsOfCell("C3", "=B2+3");
        Assert.AreEqual((double)14, a.GetCellValue("C3"));
        a.SetContentsOfCell("B2", "7");
        Assert.AreEqual((double)10, a.GetCellValue("C3"));
    }

    /// <summary>
    /// Tests if SetContentsOfCell can revert a FormulaError in value.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_RevertFormulaError_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=B2");
        Assert.IsTrue(a.GetCellValue("A1") is FormulaError);
        a.SetContentsOfCell("A1", "=5+3");
        Assert.AreEqual((double)8, a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests if SetContentsOfCell updates cells that are related to cells removed from the spreadsheet.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_RemoveCell_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=5+3");
        a.SetContentsOfCell("B2", "=A1");
        Assert.AreEqual((double)8, a.GetCellValue("B2"));
        a.SetContentsOfCell("A1", "");
        Assert.IsTrue(a.GetCellValue("B2") is FormulaError);
        Assert.IsTrue(a.GetNamesOfAllNonemptyCells().SequenceEqual(new HashSet<string> { "B2" }));
    }

    /// <summary>
    /// Tests if SetContentsOfCell updates cells that are related to cells removed from the spreadsheet.
    /// </summary>
    [TestMethod]
    public void SetContentsOfCell_CircularDependency_ValueDoesNotChange()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1");
        Assert.AreEqual((double)8, a.GetCellValue("A1"));
        Assert.ThrowsException<CircularException>(() => a.SetContentsOfCell("A1", "=B2"));
        Assert.AreEqual((double)8, a.GetCellValue("A1"));
    }

    #endregion

    #region --- GetCellContents Tests ---

    /// <summary>
    /// Tests if GetCellContents throws if it detects an invalid variable name.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellContents_InvalidName_Throws()
    {
        Spreadsheet a = new();
        a.GetCellContents("ddd");
    }

    /// <summary>
    /// Tests if GetCellContents does not throw if it detects a lowercase variable name.
    /// </summary>
    [TestMethod]
    public void GetCellContents_LowercaseVariable_Valid()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        Assert.AreEqual(a.GetCellContents("a1"), (double)8);
    }

    /// <summary>
    /// Tests if GetCellContents does not throw if it is given a variable name that has no cell associated with it.
    /// </summary>
    [TestMethod]
    public void GetCellContents_EmptyCell_Valid()
    {
        Spreadsheet a = new();
        Assert.AreEqual(a.GetCellContents("A1"), string.Empty);
    }

    #endregion

    #region --- GetCellValue, this[] Tests ---

    /// <summary>
    /// Tests GetCellValue with string values
    /// </summary>
    [TestMethod]
    public void GetCellValue_Empty_ReturnsEmptyString()
    {
        Spreadsheet a = new();
        Assert.AreEqual("", a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests GetCellValue with string values
    /// </summary>
    [TestMethod]
    public void GetCellValue_String_ReturnsString()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        Assert.AreEqual("hello!", a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests GetCellValue with double values
    /// </summary>
    [TestMethod]
    public void GetCellValue_Double_ReturnsDouble()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "82");
        Assert.AreEqual((double)82, a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests GetCellValue with Formula (double) values
    /// </summary>
    [TestMethod]
    public void GetCellValue_Formula_ReturnsDouble()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=5");
        Assert.AreEqual((double)5, a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests GetCellValue (this[]) with string values
    /// </summary>
    [TestMethod]
    public void GetCellValueBrackets_String_ReturnsString()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        Assert.AreEqual("hello!", a["A1"]);
    }

    /// <summary>
    /// Tests GetCellValue (this[]) with double values
    /// </summary>
    [TestMethod]
    public void GetCellValueBrackets_Double_ReturnsDouble()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "82");
        Assert.AreEqual((double)82, a["A1"]);
    }

    /// <summary>
    /// Tests GetCellValue (this[]) with Formula (double) values
    /// </summary>
    [TestMethod]
    public void GetCellValueBrackets_Formula_ReturnsDouble()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "=5");
        Assert.AreEqual((double)5, a["A1"]);
    }

    /// <summary>
    /// Tests GetCellValue with invalid variable names
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValue_InvalidName_Throws()
    {
        Spreadsheet a = new();
        a.GetCellValue("1A1");
    }

    /// <summary>
    /// Tests GetCellValue with invalid variable names
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void GetCellValue_InvalidName2_Throws()
    {
        Spreadsheet a = new();
        a.GetCellValue("A1A");
    }

    /// <summary>
    /// Tests GetCellValue with Formula (double) values that have variable names not defined
    /// </summary>
    [TestMethod]
    public void GetCellValue_FormulaNotEvaluatable_ReturnsFormulaError()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("B2", "=A1");
        Assert.IsTrue(a.GetCellValue("B2") is FormulaError);
    }

    /// <summary>
    /// Tests GetCellValue with Formula (double) values that have variable names with string values
    /// </summary>
    [TestMethod]
    public void GetCellValue_FormulaEvaluatesToString_ReturnsFormulaError()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.SetContentsOfCell("B2", "=A1");
        Assert.IsTrue(a.GetCellValue("B2") is FormulaError);
    }

    /// <summary>
    /// Tests GetCellValue with Formulas requiring other cells
    /// </summary>
    [TestMethod]
    public void GetCellValue_FormulaMultipleVars_ReturnsDouble()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "8");
        a.SetContentsOfCell("B2", "=A1+5");
        Assert.AreEqual((double)13, a.GetCellValue("B2"));
    }

    /// <summary>
    /// Tests GetCellValue with Formulas added in "incorrect" order
    /// </summary>
    [TestMethod]
    public void GetCellValue_FormulaOrdering_ReturnsDouble()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("B2", "=A1+5");
        Assert.IsTrue(a.GetCellValue("B2") is FormulaError);
        a.SetContentsOfCell("A1", "8");
        Assert.AreEqual((double)13, a.GetCellValue("B2"));
    }

    /// <summary>
    /// Tests GetCellValue with several Formulas added in a random "incorrect" order
    /// </summary>
    [TestMethod]
    public void GetCellValue_FormulaRandomOrdering_ReturnsCorrectly()
    {
        Spreadsheet a = new();
        Random r = new();
        string[][] list = [["A1", "8"],["B2", "=A1+4"], ["C3", "=B2+8"], ["D4", "=A1+C3"], ["E5", "=D4+B2"], ["F6", "Hello"]];
        r.Shuffle(list);
        foreach (string[] s in list)
        {
            a.SetContentsOfCell(s[0], s[1]);
        }
        Assert.AreEqual((double)8, a.GetCellValue("A1"));
        Assert.AreEqual((double)12, a.GetCellValue("B2"));
        Assert.AreEqual((double)20, a.GetCellValue("C3"));
        Assert.AreEqual((double)28, a.GetCellValue("D4"));
        Assert.AreEqual((double)40, a.GetCellValue("E5"));
        Assert.AreEqual("Hello", a.GetCellValue("F6"));
    }

    #endregion

    #region --- Changed Tests ---

    /// <summary>
    /// Tests if Changed is false on construction.
    /// </summary>
    [TestMethod]
    public void Changed_Constructed_False()
    {
        Spreadsheet a = new();
        Assert.IsFalse(a.Changed);
    }

    /// <summary>
    /// Tests if Changed is false on construction from a file.
    /// </summary>
    [TestMethod]
    public void Changed_ConstructedFromFile_False()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "hello!"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.IsFalse(a.Changed);
    }

    /// <summary>
    /// Tests if Changed is changed to true on SetContentsOfCell.
    /// </summary>
    [TestMethod]
    public void Changed_OneSet_True()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        Assert.IsTrue(a.Changed);
    }

    /// <summary>
    /// Tests if Changed does not revert after "reverting" a change.
    /// </summary>
    [TestMethod]
    public void Changed_OneSetRevert_True()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.SetContentsOfCell("A1", "");
        Assert.IsTrue(a.Changed);
    }

    /// <summary>
    /// Tests if Changed does not change from true after a circular exception.
    /// </summary>
    [TestMethod]
    public void Changed_BeforeCircularExceptionTrue_True()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "1");
        a.SetContentsOfCell("B2", "=A1");
        Assert.IsTrue(a.Changed);
        Assert.ThrowsException<CircularException>(() => a.SetContentsOfCell("A1", "=B2"));
        Assert.IsTrue(a.Changed);
    }

    /// <summary>
    /// Tests if Changed does not change from false after a circular exception.
    /// </summary>
    [TestMethod]
    public void Changed_BeforeCircularExceptionFalse_False()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "1"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.IsFalse(a.Changed);
        Assert.ThrowsException<CircularException>(() => a.SetContentsOfCell("A1", "=A1"));
        Assert.IsFalse(a.Changed);
    }

    /// <summary>
    /// Tests if Changed is set to false after a save.
    /// </summary>
    [TestMethod]
    public void Changed_Save_False()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "1");
        a.SetContentsOfCell("B2", "=A1");
        a.Save("a.txt");
        Assert.IsFalse(a.Changed);
    }

    /// <summary>
    /// Tests if Changed is set to false after two saves (its not toggled).
    /// </summary>
    [TestMethod]
    public void Changed_SaveTwice_False()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "1");
        a.SetContentsOfCell("B2", "=A1");
        a.Save("a.txt");
        a.Save("b.txt");
        Assert.IsFalse(a.Changed);
    }

    /// <summary>
    /// Tests if Changed can be set to true after a save.
    /// </summary>
    [TestMethod]
    public void Changed_SaveThenChange_True()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "1");
        a.SetContentsOfCell("B2", "=A1");
        a.Save("a.txt");
        a.SetContentsOfCell("A1", "2");
        Assert.IsTrue(a.Changed);
    }

    /// <summary>
    /// Tests if Changed changes to true even if no contents/values are changing.
    /// </summary>
    [TestMethod]
    public void Changed_ChangeToItself_True()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "1");
        a.Save("a.txt");
        a.SetContentsOfCell("A1", "1");
        Assert.IsTrue(a.Changed);
    }

    /// <summary>
    /// Tests if Changed changes to true if setting an empty cell to an empty cell.
    /// </summary>
    [TestMethod]
    public void Changed_ChangeToEmpty_True()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "");
        Assert.IsTrue(a.Changed);
    }

    #endregion

    #region --- Save Tests ---

    /// <summary>
    /// Tests if an empty save file does not contain any variables.
    /// </summary>
    [TestMethod]
    public void Save_EmptySpreadsheet_Valid()
    {
        Spreadsheet a = new();
        a.Save("a.txt");
        string s = File.ReadAllText("a.txt");
        Assert.IsFalse(Regex.IsMatch(s, @$"^[a-zA-Z]+\d+$"));
    }

    /// <summary>
    /// Tests if a save file contains all the elements in the spreadsheet.
    /// </summary>
    [TestMethod]
    public void Save_CellList_Printed()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.SetContentsOfCell("B2", "1");
        a.SetContentsOfCell("C3", "=1+B2");
        a.SetContentsOfCell("A2", "1111!");
        a.Save("a.txt");
        string s = File.ReadAllText("a.txt");
        Assert.IsTrue(Regex.IsMatch(s, "A1"));
        Assert.IsTrue(Regex.IsMatch(s, "A2"));
        Assert.IsTrue(Regex.IsMatch(s, "B2"));
        Assert.IsTrue(Regex.IsMatch(s, "C3"));
        Assert.IsTrue(Regex.IsMatch(s, "hello!"));
        Assert.IsTrue(Regex.IsMatch(s, "1"));
        Assert.IsTrue(Regex.IsMatch(s, @"=1\+B2"));
        Assert.IsTrue(Regex.IsMatch(s, "1111!"));
    }

    /// <summary>
    /// Tests if a save file contains the fields "StringForm" and "Cells".
    /// </summary>
    [TestMethod]
    public void Save_Default_PrintsCorrectNames()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.Save("a.txt");
        string s = File.ReadAllText("a.txt");
        Assert.IsTrue(Regex.IsMatch(s, "StringForm"));
        Assert.IsTrue(Regex.IsMatch(s, "Cells"));
    }

    /// <summary>
    /// Tests if cells set to an empty string do not appear in the save file.
    /// </summary>
    [TestMethod]
    public void Save_Empty_NotPrinted()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.SetContentsOfCell("B2", "");
        a.Save("a.txt");
        string s = File.ReadAllText("a.txt");
        Assert.IsTrue(Regex.IsMatch(s, "A1"));
        Assert.IsFalse(Regex.IsMatch(s, "B2"));
    }

    /// <summary>
    /// Tests that changes after a save are not in the save file.
    /// </summary>
    [TestMethod]
    public void Save_ChangesAfterSave_NotPrinted()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.Save("a.txt");
        a.SetContentsOfCell("B2", "1");
        string s = File.ReadAllText("a.txt");
        Assert.IsTrue(Regex.IsMatch(s, "A1"));
        Assert.IsFalse(Regex.IsMatch(s, "B2"));
    }

    /// <summary>
    /// Tests the save file cannot be saved to a random non-existing folder.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Save_InvalidFolderLocation_Throws()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.Save("/nonsense/folder/a.txt");
    }

    /// <summary>
    /// Tests Changed does not change from true after a failed save.
    /// </summary>
    [TestMethod]
    public void Save_ChangedTrueAfterThrownSave_RemainsTrue()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        Assert.IsTrue(a.Changed);
        Assert.ThrowsException<SpreadsheetReadWriteException>(() => a.Save("C:/nonsense/folder/a.txt"));
        Assert.IsTrue(a.Changed);
    }

    /// <summary>
    /// Tests Changed does not change from false after a failed save.
    /// </summary>
    [TestMethod]
    public void Save_ChangedFalseAfterThrownSave_RemainsFalse()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "hello!");
        a.Save("a.txt");
        Assert.IsFalse(a.Changed);
        Assert.ThrowsException<SpreadsheetReadWriteException>(() => a.Save("C:/nonsense/folder/a.txt"));
        Assert.IsFalse(a.Changed);
    }

    #endregion

    #region --- Spreadsheet From File Tests ---

    /// <summary>
    /// Tests loading a basic spreadsheet from a file.
    /// </summary>
    [TestMethod]
    public void Load_OneCell_ExistsInSpreadsheet()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "hello!"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.AreEqual("hello!", a.GetCellContents("A1"));
    }

    /// <summary>
    /// Tests loading a spreadsheet with no cells.
    /// </summary>
    [TestMethod]
    public void Load_NoCells_NoneExistsInSpreadsheet()
    {
        string sheet = """
            {
                "Cells": 
                {
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.AreEqual(0, a.GetNamesOfAllNonemptyCells().Count);
    }

    /// <summary>
    /// Tests loading a spreadsheet with no cells, taken from the save of another empty spreadsheet.
    /// </summary>
    [TestMethod]
    public void Load_SaveFromEmptySpreadsheet_NoneExistsInSpreadsheet()
    {
        Spreadsheet a = new();
        a.Save("a.txt");
        Spreadsheet b = new("a.txt");
        Assert.AreEqual(0, b.GetNamesOfAllNonemptyCells().Count);
    }

    /// <summary>
    /// Tests if loading throws if a file does not exist.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_FileDoesNotExist_Throws()
    {
        Spreadsheet a = new("newFile.txt");
    }

    /// <summary>
    /// Tests if loading throws if a file name is not valid.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_InvalidFileName_Throws()
    {
        Spreadsheet a = new("banana!");
    }

    /// <summary>
    /// Tests if loading throws if Cells is not named properly.
    /// </summary>
    [TestMethod]
    public void Load_ImproperCellName_NotLoaded()
    {
        string sheet = """
            {
                "A": 
                {
                    "A1": 
                    {
                        "StringForm" : "hello!"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.AreEqual(string.Empty, a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests if loading throws if Cells is missing.
    /// </summary>
    [TestMethod]
    public void Load_MissingCells_Throws()
    {
        string sheet = """
            {
                "Nothing" : "here"
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.AreEqual(0, a.GetNamesOfAllNonemptyCells().Count);
    }

    /// <summary>
    /// Tests if loading throws if variables are not named properly.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_InvalidCellName_Throws()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "1243153": 
                    {
                        "StringForm" : "hello!"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
    }

    /// <summary>
    /// Tests if loading throws if StringForm is not named properly.
    /// </summary>
    [TestMethod]
    public void Load_InvalidStringName_Throws()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "string" : "hello!"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.AreEqual(string.Empty, a.GetCellValue("A1"));
    }

    /// <summary>
    /// Tests if loading throws if a CircularException is in the file.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_CircularException_Throws()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "=A1"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
    }

    /// <summary>
    /// Tests if loading throws if an invalid formula is in the file.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_FormulaFormatException_Throws()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "="
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
    }

    /// <summary>
    /// Tests if loading does not throw if a FormulaError is found.
    /// </summary>
    [TestMethod]
    public void Load_InvalidNameException_ReturnsFormulaError()
    {
        string sheet = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "=B2"
                    }
                }
            }
            """;
        File.WriteAllText("save.txt", sheet);
        Spreadsheet a = new("save.txt");
        Assert.IsTrue(a.GetCellValue("A1") is FormulaError);
    }

    /// <summary>
    /// Tests is loading two variables in either order is valid.
    /// </summary>
    [TestMethod]
    public void Load_Ordering_ValidBothWays()
    {
        string sheet1 = """
            {
                "Cells": 
                {
                    "A1": 
                    {
                        "StringForm" : "=1"
                    },
                    "B2": 
                    {
                        "StringForm" : "=A1"
                    }
                }
            }
            """;
        File.WriteAllText("save1.txt", sheet1);
        Spreadsheet a = new("save1.txt");
        Assert.AreEqual(1.0, a.GetCellValue("B2"));
        string sheet2 = """
            {
                "Cells": 
                {
                    "B2": 
                    {
                        "StringForm" : "=A1"
                    },
                    "A1": 
                    {
                        "StringForm" : "=1"
                    }
                }
            }
            """;
        File.WriteAllText("save2.txt", sheet2);
        Spreadsheet b = new("save2.txt");
        Assert.AreEqual(1.0, b.GetCellValue("B2"));
    }

    /// <summary>
    /// Tests is loading an empty file throws.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_DeserializeEmptyFile_Throws()
    {
        File.WriteAllText("empty.txt", string.Empty);
        _ = new Spreadsheet("empty.txt");
    }

    /// <summary>
    /// Tests is loading a null file throws.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_DeserializeNullFile_Throws()
    {
        File.WriteAllText("empty.txt", null);
        _ = new Spreadsheet("empty.txt");
    }

    [TestMethod]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Load_ThrowsSpreadsheetReadWriteException()
    {
        File.WriteAllText("emptySpreadsheet.txt", "null");

        _ = new Spreadsheet("emptySpreadsheet.txt");
    }

    /// <summary>
    /// Tests if saving and loading result in the same
    /// </summary>
    [TestMethod]
    public void SaveLoad_SampleSpreadsheet_SameSpreadsheet()
    {
        Spreadsheet a = new();
        a.SetContentsOfCell("A1", "12");
        a.SetContentsOfCell("B2", "=A1");
        a.SetContentsOfCell("C3", "hello");
        a.SetContentsOfCell("D4", "=B2+A1");
        a.SetContentsOfCell("E5", "EEEEE");
        a.SetContentsOfCell("F6", "333333");
        a.Save("sample.txt");
        Spreadsheet b = new("sample.txt");
        Assert.AreEqual(12.0, b.GetCellValue("A1"));
        Assert.AreEqual(12.0, b.GetCellValue("B2"));
        Assert.AreEqual("hello", b.GetCellValue("C3"));
        Assert.AreEqual(24.0, b.GetCellValue("D4"));
        Assert.AreEqual("EEEEE", b.GetCellValue("E5"));
        Assert.AreEqual(333333.0, b.GetCellValue("F6"));
    }

    #endregion
}
