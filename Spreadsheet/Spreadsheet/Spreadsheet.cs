// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain, Fall 2021, Fall 2024
//     - Updated return types
//     - Updated documentation
// Implementation by Caleb Standfield
// Date, 09/26/24

using System.Text.RegularExpressions;

namespace CS3500.Spreadsheet;

using CS3500.Formula;
using CS3500.DependencyGraph;
using System.Text.Json;

/// <summary>
///   <para>
///     A Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet represents an infinite number of named cells.
///   </para>
/// <para>
///     Valid Cell Names: A string is a valid cell name if and only if it is one or
///     more letters followed by one or more numbers, e.g., A5, BC27.
/// </para>
/// <para>
///    Cell names are case-insensitive, so "x1" and "X1" are the same cell name.
///    Your code should normalize (uppercased) any stored name but accept either.
/// </para>
/// <para>
///     A spreadsheet represents a cell corresponding to every possible cell name.  (This
///     means that a spreadsheet contains an infinite number of cells.)  In addition to
///     a name, each cell has a contents and a value.  The distinction is important.
/// </para>
/// <para>
///     The <b>contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
///     If the contents of a cell is set to the empty string, the cell is considered empty.
/// </para>
/// <para>
///     By analogy, the contents of a cell in Excel is what is displayed on
///     the editing line when the cell is selected.
/// </para>
/// <para>
///     In a new spreadsheet, the contents of every cell is the empty string. Note:
///     this is by definition (it is IMPLIED, not stored).
/// </para>
/// <para>
///     The <b>value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
///     in the grid.) We are not concerned with cell values yet, only with their contents,
///     but for context:
/// </para>
/// <list type="number">
///   <item>If a cell's contents is a string, its value is that string.</item>
///   <item>If a cell's contents is a double, its value is that double.</item>
///   <item>
///     <para>
///       If a cell's contents is a Formula, its value is either a double or a FormulaError,
///       as reported by the Evaluate method of the Formula class.  For this assignment,
///       you are not dealing with values yet.
///     </para>
///   </item>
/// </list>
/// <para>
///     Spreadsheets are never allowed to contain a combination of Formulas that establish
///     a circular dependency.  A circular dependency exists when a cell depends on itself,
///     either directly or indirectly.
///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
///     dependency.
/// </para>
/// </summary>
public class Spreadsheet
{
    /// <summary>
    ///   <para>
    ///     A Dictionary that hold all non-empty cells that represent the spreadsheet.
    ///   </para>
    /// </summary>
    private readonly Dictionary<string, Cell> _spreadsheet = new();

    /// <summary>
    ///   <para>
    ///     The dependencyGraph that will keep track dependencies of the cells.
    ///   </para>
    /// </summary>
    private readonly DependencyGraph _dependencyGraph = new();

    /// <summary>
    ///   <para>
    ///     Regex to help with determining if a name is valid.
    ///   </para>
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    /// True if this spreadsheet has been changed since it was 
    /// created or saved (whichever happened most recently),
    /// False otherwise.
    /// </summary>
    public bool Changed { get; private set; }

    /// <summary>
    ///   <para>
    ///     The default zero argument constructor that creates a new
    ///     spreadsheet in a default blank state.
    ///   </para>
    /// </summary>
    public Spreadsheet()
    {
    }

    /// <summary>
    /// Constructs a spreadsheet using the saved data in the file referred to by
    /// the given filename. 
    /// <see cref="Save(string)"/>
    /// </summary>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   Thrown if the file can not be loaded into a spreadsheet for any reason
    /// </exception>
    /// <param name="filename">The path to the file containing the spreadsheet to load</param>
    public Spreadsheet(string filename)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///   <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it.
    ///   </para>
    ///   <para>
    ///     The output JSON should look like the following.
    ///   </para>
    ///   <para>
    ///     For example, consider a spreadsheet that contains a cell "A1" 
    ///     with contents being the double 5.0, and a cell "B3" with contents 
    ///     being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    ///   </para>
    ///   <para>
    ///      This method would produce the following JSON string:
    ///   </para>
    ///   <code>
    ///   {
    ///     "Cells": {
    ///       "A1": {
    ///         "StringForm": "5"
    ///       },
    ///       "B3": {
    ///         "StringForm": "=A1+2"
    ///       },
    ///       "C4": {
    ///         "StringForm": "hello"
    ///       }
    ///     }
    ///   }
    ///   </code>
    ///   <para>
    ///     You can achieve this by making sure your data structure is a dictionary 
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName 
    ///     attribute).
    ///   </para>
    ///   <para>
    ///     There can be 0 cells in the dictionary, resulting in { "Cells" : {} } 
    ///   </para>
    ///   <para>
    ///     Further, when writing the value of each cell...
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       If the contents is a string, the value of StringForm is that string
    ///     </item>
    ///     <item>
    ///       If the contents is a double d, the value of StringForm is d.ToString()
    ///     </item>
    ///     <item>
    ///       If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file, 
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        if (Changed)
        {
            try
            {
                var json = JsonSerializer.Serialize(_spreadsheet);
            }
            catch (SpreadsheetReadWriteException)
            {

            }
            
        }
        Changed = false;
    }

    /// <summary>
    ///   Provides a copy of the normalized names of all the cells in the spreadsheet
    ///   that contain information (i.e., non-empty cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        var nonEmptyCellNames = new HashSet<string>();
        foreach (var name in _spreadsheet.Where(name => !string.IsNullOrEmpty(name.Value.ToString())))
        {
            nonEmptyCellNames.Add(name.Key);
        }

        return nonEmptyCellNames;
    }

    /// <summary>
    ///   Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        return _spreadsheet.TryGetValue(NormalizedName(name), out var cell) 
            ? cell.Contents // Cell existed, return contents
            : string.Empty; // Cell did not exist, default return of the empty string
    }

    /// <summary>
    ///   <para>
    ///     Get the ContentType of a passed in string.
    ///     Can either be of type double, string, or Formula.
    ///   </para>
    /// </summary>
    /// <param name="content">The string form of contents</param>
    /// <returns>The corresponding ContentType of the parsed string</returns>
    private static CellContentsType GetContentType(string content)
    {
        // Attempt to parse as double
        if (double.TryParse(content, out _))
        {
            return CellContentsType.Double;
        }

        // If first char is '=' then is of type Formula, else string
        return content.StartsWith('=')
            ? CellContentsType.Formula
            : CellContentsType.String;
    }
    
    /// <summary>
    ///   <para>
    ///     Get the ContentType of the passed object representing the contents.
    ///     Can either be of type double, string, or formula
    ///   </para>
    /// </summary>
    /// <param name="contents">The object representing the contents of a cell</param>
    /// <returns>The corresponding ContentType of the parsed object</returns>
    private static CellContentsType GetContentType(object contents)
    {
        return contents switch
        {
            // Use pattern to determine what ContentType to return
            double => CellContentsType.Double,
            Formula => CellContentsType.Formula,
            // Default to string
            _ => CellContentsType.String,
        };
    }

    #region SetContentsOfCellMethods

    /// <summary>
    ///   <para>
    ///     Set the contents of the named cell to be the provided string
    ///     which will either represent (1) a string, (2) a number, or 
    ///     (3) a formula (based on the prepended '=' character).
    ///   </para>
    ///   <para>
    ///     Rules of parsing the input string:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       <para>
    ///         If 'content' parses as a double, the contents of the named
    ///         cell becomes that double.
    ///       </para>
    ///     </item>
    ///     <item>
    ///         If the string does not begin with an '=', the contents of the 
    ///         named cell becomes 'content'.
    ///     </item>
    ///     <item>
    ///       <para>
    ///         If 'content' begins with the character '=', an attempt is made
    ///         to parse the remainder of content into a Formula f using the Formula
    ///         constructor.  There are then three possibilities:
    ///       </para>
    ///       <list type="number">
    ///         <item>
    ///           If the remainder of content cannot be parsed into a Formula, a 
    ///           CS3500.Formula.FormulaFormatException is thrown.
    ///         </item>
    ///         <item>
    ///           Otherwise, if changing the contents of the named cell to be f
    ///           would cause a circular dependency, a CircularException is thrown,
    ///           and no change is made to the spreadsheet.
    ///         </item>
    ///         <item>
    ///           Otherwise, the contents of the named cell becomes f.
    ///         </item>
    ///       </list>
    ///     </item>
    ///   </list>
    /// </summary>
    /// <returns>
    ///   <para>
    ///     The method returns a list consisting of the name plus the names 
    ///     of all other cells whose value depends, directly or indirectly, 
    ///     on the named cell. The order of the list should be any order 
    ///     such that if cells are re-evaluated in that order, their dependencies 
    ///     are satisfied by the time they are evaluated.
    ///   </para>
    ///   <example>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list {A1, B1, C1} is returned.
    ///   </example>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If name is invalid, throws an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///     If a formula will result in a circular dependency, throws CircularException.
    /// </exception>
    public IList<string> SetContentsOfCell(string name, string content)
    {
        // Normalize name to pass into subsequent methods
        var normalizedName = NormalizedName(name);

        // Retrieve content type to determine which private helper method to call
        var contentType = GetContentType(content);

        var retList = contentType switch
        {
            CellContentsType.Double => SetCellContents(normalizedName, double.Parse(content)),
            CellContentsType.String => SetCellContents(normalizedName, content),
            // First char is '=', pass the remaining string as the contents
            _ => SetCellContents(normalizedName, new Formula(content[1..]))
        };

        // Using the ordered return list calculate each value
        SetValueOfListOfCells(retList);

        return retList;
    }

    /// <summary>
    ///   Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new contents of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    ///   <para>
    ///     The order must correspond to a valid dependency ordering for recomputing
    ///     all the cells, i.e., if you re-evaluate each cell in the order of the list,
    ///     the overall spreadsheet will be correctly updated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///     evaluated, followed by B1, followed by C1.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        // Return call to helper method
        return SetCellContentsHelper(name, number, []);
    }

    /// <summary>
    ///   The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new contents of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        // If text is not the empty string then call helper method to replace cell with new contents
        if (!text.Equals(string.Empty)) return SetCellContentsHelper(name, text, []);

        // Text was the empty string, thus remove the key associated to the name
        _spreadsheet.Remove(name);

        // Update the dependency graph to remove possible dependents
        _dependencyGraph.ReplaceDependents(name, []);

        // State of the spreadsheet has changed
        Changed = true;

        // Return call to helper method
        return GetCellsToRecalculate(name).ToList();
    }

    /// <summary>
    ///   Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException, and no
    ///     change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new contents of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        // Keep track of the current state of the cell and dependency graph
        var originalCellContents = GetCellContents(name);
        var originalDependents = _dependencyGraph.GetDependents(name).ToHashSet();

        try
        {
            var dependents = formula.GetVariables();
            // Return call to helper method
            return SetCellContentsHelper(name, formula, dependents.ToHashSet());
        }
        catch (CircularException)
        {
            // Check if originalCellContents is the empty string
            if (originalCellContents is string contents && originalCellContents.Equals(string.Empty))
            {
                // Call to SetCellContents to remove cell as it is empty
                _ = SetCellContents(name, contents);
            }

            // Passing in original cell data to restore the cell, output not needed
            _ = SetCellContentsHelper(name, originalCellContents, originalDependents);

            throw new CircularException();
        }
    }

    /// <summary>
    ///   <para>
    ///     Set the contents of the named cell to the given contents.
    ///     If the contents passed in are of type formula a possible CircularException may happen.
    ///   </para>
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///  
    ///   <para>
    ///     If changing the contents of the named cell to be a formula that would
    ///     cause a circular dependency, throw a CircularException, and no
    ///     change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell </param>
    /// <param name="contents"> The contents of to which set the cells contents to be</param>
    /// <param name="dependents"> The list of dependents to pass into to update the dependencyGraph</param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContentsHelper(string name, object contents, HashSet<string> dependents)
    {
        // Create a new Cell
        var cell = new Cell(contents);

        // Add the cell to the spreadsheet
        _spreadsheet[name] = cell;

        // Get the content type to assign to cell
        cell.ContentType = GetContentType(contents);
        
        // Sets the StringForm of the cell
        cell.StringForm = cell.ContentType switch
        {
            // If type Formula prepend an equals sign
            CellContentsType.Formula => "=" + contents,
            // Default of double or string
            _ => contents.ToString()!
        };

        // Create the dependencies that this new cell is associated with
        _dependencyGraph.ReplaceDependents(name, dependents);

        // State of the spreadsheet has changed
        Changed = true;

        // Return call to GetCellsToRecalculate
        return GetCellsToRecalculate(name).ToList();
    }

    #endregion
    
    #region GetAndSetValueMethods

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell, as defined by
    ///     <see cref="GetCellValue(string)"/>.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   <see cref="GetCellValue(string)"/>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object this[string name]
    {
        get { return GetCellValue(name); }
    }

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value should be either a string, a double, or a CS3500.Formula.FormulaError.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string name)
    {
        return _spreadsheet.TryGetValue(NormalizedName(name), out var cell)
            ? cell.Value // Cell existed, return the value
            : string.Empty; // Cell does not exist, default return of the empty string
    }

    /// <summary>
    ///   <param>
    ///     Takes in a cell and determins based of the cells contents what type of value can be parsed
    ///     from the contents, then assigns the parsed value to the cells value field.
    ///   </param>
    /// </summary>
    /// <param name="cell">The cell to be parsed and assigned a value to</param>
    private void SetValueOfCell(Cell cell)
    {
        // Get contents as a string
        var contents = cell.Contents.ToString();
        // From the given cell grab its content type
        var contentsType = cell.ContentType;
        switch (contentsType)
        {
            case CellContentsType.Formula:
                // Type formula, call method to begin Evaluation of contents
                cell.Value = EvaluateContentFormula(contents!);
                break;
            case CellContentsType.Double:
            {
                // Type double, parse the contents to a double
                cell.Value = double.Parse(contents!);
                break;
            }
            // For string fall to the default case
            case CellContentsType.String:
            default:
                // Assign value to the string form
                cell.Value = contents!;
                break;
        }
    }

    /// <summary>
    ///   <para>
    ///     Sets the value of all the cells in the provided list.
    ///     List must be ordered of which cells to be evaluated first.
    ///   </para>
    /// </summary>
    /// <param name="cells">The ordered list of cells to evaluate</param>
    private void SetValueOfListOfCells(IList<string> cells)
    {
        // Iterate through each cell and set the value
        foreach (var cellName in cells)
        {
            if (_spreadsheet.TryGetValue(NormalizedName(cellName), out var cell))
            {
                SetValueOfCell(cell);
            }
        }
    }

    /// <summary>
    ///   <para>
    ///     Calls the Evaluate method on the passed in contents.
    ///   </para>
    /// </summary>
    /// <param name="contents">The contents to be evaluated</param>
    /// <returns>A double of the contents, or a FormulaError if the contents could not be parsed into a double</returns>
    private object EvaluateContentFormula(string contents)
    {
        return _ = new Formula(contents).Evaluate(LookupValueOfCellsDelegate);
    }

    /// <summary>
    ///   <para>
    ///     Returns the value of the cell if it is a double, otherwise this method will throw an exception
    ///     to be cought by the Formula class to return a FromulaError.
    ///   </para>
    /// </summary>
    /// <param name="name">The cell to retrieve the value from</param>
    /// <returns>A double of which is held inside the cell as a value</returns>
    /// <exception cref="ArgumentException">Throws an exception when the cell passed in doesn't contain a number as its value</exception>
    private double LookupValueOfCellsDelegate(string name)
    {
        // Get the cell from the spreadsheet and check if it is a double
        if (_spreadsheet.TryGetValue(NormalizedName(name), out var cell) && cell.Value is double number)
        {
            // Return the number
            return number;
        }
        // Was not a number or the cell did not exist
        throw new ArgumentException("Cell doesn't exist or does not contain a numerical value");
    }

    #endregion

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all cells whose
    ///   values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    ///   <para>For example, suppose that: </para>
    ///   <list type="bullet">
    ///      <item>A1 contains 3</item>
    ///      <item>B1 contains the formula A1 * A1</item>
    ///      <item>C1 contains the formula B1 + A1</item>
    ///      <item>D1 contains the formula B1 - C1</item>
    ///   </list>
    ///   <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return _dependencyGraph.GetDependees(NormalizedName(name));
    }

    /// <summary>
    ///   <para>
    ///     This method is implemented for you, but makes use of your GetDirectDependents.
    ///   </para>
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    ///   <para>
    ///     For example, suppose that:
    ///   </para>
    ///   <list type="number">
    ///     <item>
    ///       A1 contains 5
    ///     </item>
    ///     <item>
    ///       B1 contains the formula A1 + 2.
    ///     </item>
    ///     <item>
    ///       C1 contains the formula A1 + B1.
    ///     </item>
    ///     <item>
    ///       D1 contains the formula A1 * 7.
    ///     </item>
    ///     <item>
    ///       E1 contains 15
    ///     </item>
    ///   </list>
    ///   <para>
    ///     If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///     and they must be recalculated in an order which has A1 first, and B1 before C1
    ///     (there are multiple such valid orders).
    ///     The method will produce one of those enumerations.
    ///   </para>
    ///   <para>
    ///      PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///      IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///   </para>
    /// </summary>
    /// <param name="name"> The name of the cell.  Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    ///   <para>
    ///     A helper for the GetCellsToRecalculate method.
    ///     This method performs a topological sort on the name passed in; in which it gets the
    ///     direct dependents of the name (cell) and starts with adding the name as the first cell that is visited.
    ///     This signifies that this cell should be the first cell that will be updated when the return list is used
    ///     calculate in what order to recalculate cells.
    ///   </para>
    /// </summary>
    /// <param name="start">The first node that will be visited</param>
    /// <param name="name">The name of the nodes to grab direct dependents from</param>
    /// <param name="visited">A HashSet that indicated what has already been visited</param>
    /// <param name="changed">A Linked list that retains the order in which cells should be updated</param>
    /// <exception cref="CircularException"></exception>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        // Add the current node to visited 
        visited.Add(name);
        foreach (var n in GetDirectDependents(name))
        {
            // For any dependency n that is equal to the start that creates a cycle
            if (n.Equals(start))
            {
                throw new CircularException();
            }

            // If the visited has not traversed into this dependency 
            // Call itself to recursively travel through dependencies
            if (!visited.Contains(n))
            {
                // Recursive call with n being new name
                Visit(start, n, visited, changed);
            }
        }

        // Add name to return list
        changed.AddFirst(name);
    }

    /// <summary>
    ///   <para>
    ///     Normalizes the given possible cell name.
    ///   </para>
    /// </summary>
    /// <param name="token">The name to be normalized.</param>
    /// <returns>
    ///   A normalized string representation of the name, if the name is valid and can be normalized.
    /// </returns>
    /// <exception cref="InvalidNameException">Will be thrown when an invalid name is passed through</exception>
    private static string NormalizedName(string token)
    {
        const string standaloneVarPattern = $"^{VariableRegExPattern}$";
        if (Regex.IsMatch(token, standaloneVarPattern))
        {
            // If a token is a letter then make uppercase
            return new string(token.Select(c => char.IsLetter(c) ? char.ToUpper(c) : c).ToArray());
        }

        throw new InvalidNameException();
    }

    /// <summary>
    ///   <para>
    ///     An enum to signify the type of contents to base value calculation off of
    ///   </para>
    /// </summary>
    private enum CellContentsType
    {
        Double,
        String,
        Formula
    }

    /// <summary>
    ///   <para>
    ///     A nested class that represents a cell inside a spreadsheet.
    ///     Each cell is tied to a name via the spreadsheetDictionary.
    ///     Cells hold their contents. 
    ///   </para>
    /// </summary>
    private class Cell(object contents)
    {
        /// <summary>
        ///   <para>
        ///     Contains getter and setter for the use of easy access in the spreadSheet class.
        ///     Represents the contents of a cell.
        ///   </para>
        /// </summary>
        public object Contents { get; } = contents;

        /// <summary>
        ///   <para>
        ///     Contains getter and setter for the use of easy access in the spreadSheet class.
        ///     Represents the contents of a cell.
        ///   </para>
        /// </summary>
        public string StringForm { get; set; } = null!;

        /// <summary>
        ///   <para>
        ///     Represents the value of the cell, is assigned value by the spreadsheet class
        ///     at a later time.
        ///   </para>
        /// </summary>
        public object Value { get; set; } = null!;

        /// <summary>
        ///   <para>
        ///     Represents what type of contents it holds can be of type double, string, formula, or formula error,
        ///     is assigned by the spreadsheet class at a later time 
        ///   </para>
        /// </summary>
        public CellContentsType ContentType { get; set; }
    }
}

/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception;

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception;

/// <summary>
/// <para>
///   Thrown to indicate that a read or write attempt has failed with
///   an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    ///   <para>
    ///     Creates the exception with a message defining what went wrong.
    ///   </para>
    /// </summary>
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException(string msg)
        : base(msg)
    {
    }
}