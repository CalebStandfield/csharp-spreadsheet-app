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
    // A Dictionary that hold all non-empty cells that represent the spreadsheet
    private readonly Dictionary<string, Cell> _spreadsheet = new();

    // The dependencyGraph that will keep track dependencies of the cells
    private readonly DependencyGraph _dependencyGraph = new();

    // Regex to help with determining if a name is valid
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

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
        // If TryGetValue successes access the cell and return the contents of cell
        // If TryGetValue fails the cell does not exist return string.Empty as default
        return _spreadsheet.TryGetValue(NormalizedName(name), out var cell)
            ? cell.Contents
            : string.Empty;
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
    public IList<string> SetCellContents(string name, double number)
    {
        // Return call to helper method
        return SetCellContentsHelper(NormalizedName(name), number, []);
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
    public IList<string> SetCellContents(string name, string text)
    {
        var normalizedCellName = NormalizedName(name);

        // If text is not the empty string then call helper method to replace cell with new contents
        if (!text.Equals(string.Empty)) return SetCellContentsHelper(normalizedCellName, text, []);

        // Text was the empty string, thus remove the key associated to the name
        _spreadsheet.Remove(normalizedCellName);

        // Update the dependency graph to remove possible dependents
        _dependencyGraph.ReplaceDependents(normalizedCellName, []);

        // Return call to helper method
        return GetCellsToRecalculate(normalizedCellName).ToList();
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
    public IList<string> SetCellContents(string name, Formula formula)
    {
        var normalizedCellName = NormalizedName(name);

        // Keep track of the current state of the cell and dependency graph
        var originalCellContents = GetCellContents(normalizedCellName);
        var originalDependents = _dependencyGraph.GetDependents(normalizedCellName).ToList();

        try
        {
            var dependents = formula.GetVariables();
            // Return call to helper method
            return SetCellContentsHelper(normalizedCellName, formula, dependents.ToHashSet());
        }
        catch (CircularException)
        {
            // Reset the spreadsheet to its original state
            // Reset contents of cell
            _spreadsheet[normalizedCellName] = new Cell(originalCellContents);
            
            // Check if originalCellContents is the empty string
            if (originalCellContents is string contents && originalCellContents.Equals(string.Empty))
            {
                // Call to SetCellContents to remove cell as it is empty
                _ = SetCellContents(normalizedCellName, contents);
            }
            // Reset the dependents of cell
            _dependencyGraph.ReplaceDependents(normalizedCellName, originalDependents);
            throw new CircularException();
        }
    }

    /// <summary>
    ///   <para>
    ///     Set the contents of the named cell to the given contents.
    ///     If the passed in contents are of type formula a possible CircularException may happen.
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
        // Make a new cell with passed in contents
        _spreadsheet[name] = new Cell(contents);

        // Create the dependencies that this new cell is associated with
        _dependencyGraph.ReplaceDependents(name, dependents);

        // Return call to GetCellsToRecalculate
        return GetCellsToRecalculate(name).ToList();
    }

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
    ///     A nested class that represents a cell inside a spreadsheet.
    ///     Each cell is tied to a name via the spreadsheetDictionary.
    ///     Cells hold their contents. 
    ///   </para>
    /// </summary>
    /// <param name="contents">The contents of which this cell holds</param>
    private class Cell(object contents)
    {
        /// <summary>
        ///   <para>
        ///     Contains getter and setter for the use of easy access in the spreadSheet class.
        ///     Represents the contents of a cell.
        ///   </para>
        /// </summary>
        public object Contents { get; } = contents;
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