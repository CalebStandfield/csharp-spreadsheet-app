// <copyright file="SpreadsheetPage.razor.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

using CS3500.Formula;
using CS3500.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Client.Pages;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;

/// <summary>
///   <para>
///     This class is the controller between the backend and front end of the spreadsheet
///   </para>
/// </summary>
public partial class SpreadsheetPage
{
    /// <summary>
    /// Based on your computer, you could shrink/grow this value based on performance.
    /// </summary>
    private const int Rows = 50;

    /// <summary>
    /// Number of columns, which will be labeled A-Z.
    /// </summary>
    private const int Cols = 26;

    /// <summary>
    ///   <para>
    ///     The backing spreadsheet for the spreadsheet GUI.
    ///   </para>
    /// </summary>
    private Spreadsheet _spreadsheet = new();

    /// <summary>
    ///   <para>
    ///     The current selected cell of the spreadsheet.
    ///     Default value of "A1".
    ///   </para>
    /// </summary>
    private string _selectedCell = "A1";

    /// <summary>
    ///   <para>
    ///     The current inputted string of the selected cell in the spreadsheet.
    ///     Default value of an empty string.
    ///   </para>
    /// </summary>
    private string _selectedCellInput = string.Empty;
    
    /// <summary>
    ///   <para>
    ///     The current value of the selected cell of the spreadsheet.
    ///     Default value of an empty string.
    ///   </para>
    /// </summary>
    private string _selectedCellValue = string.Empty;
    
    /// <summary>
    ///   <para>
    ///     The old input of the selected cell.
    ///     Default value of an empty string.
    ///   </para>
    /// </summary>
    private string _oldCellInput = string.Empty;
    
    /// <summary>
    ///   <para>
    ///     The old value of the selected cell.
    ///     Default value of an empty string.
    ///   </para>
    /// </summary>
    private string _oldCellValue = string.Empty;

    /// <summary>
    ///   <para>
    ///     Provides an easy way to convert from an index to a letter (0 -> A).
    ///   </para>
    /// </summary>
    private char[] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    
    /// <summary>
    ///   <para>
    ///     Gets or sets the name of the file to be saved.
    ///   </para>
    /// </summary>
    private string FileSaveName { get; set; } = "Spreadsheet.sprd";
    
    /// <summary>
    ///   <para>
    ///     Gets or sets the data for all the cells in the spreadsheet GUI.
    ///   </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore { get; set; } = new string[Rows, Cols];

    /// <summary>
    ///   <para>
    ///     Holds each state of the spreadsheet in a stack.
    ///     In the order from first state to last.
    ///   </para>
    /// </summary>
    private readonly Stack<CellInfoChanged> _back = new();

    /// <summary>
    ///   <para>
    ///     Holds each state of the spreadsheet in a stack.
    ///     In the order from last state to first.
    ///   </para>
    /// </summary>
    private readonly Stack<CellInfoChanged> _forward = new();

    /// <summary>
    ///   <para>
    ///     An ElementReference to the input field in the spreadsheet.
    ///     Used to automatically select the input field.
    ///   </para>
    /// </summary>
    private ElementReference _inputElement;
    
    /// <summary>
    ///   <para>
    ///     The exception message to be displayed.
    ///     Default value of string.Empty.
    ///   </para>
    /// </summary>
    private string _exceptionMessage = string.Empty;
    
    /// <summary>
    ///   <para>
    ///     A boolean to activate if the message popup is being displayed or not.
    ///     True if displayed, false if not.
    ///   </para>
    /// </summary>
    private bool _showPopup;

    /// <summary>
    ///   <para>
    ///     The current file name to load this spreadsheet to.
    ///   </para>
    /// </summary>
    private string _loadedFileName = string.Empty;
    
    /// <summary>
    /// Handler for when a cell is clicked
    /// </summary>
    /// <param name="row">The row component of the cell's coordinates</param>
    /// <param name="col">The column component of the cell's coordinates</param>
    private async void CellClicked(int row, int col)
    {
        // Get the name of the row, col equivalent 
        _selectedCell = GetCellName(row, col);
        
        // Get the input -- contents, and the value of this cell
        _selectedCellInput = ContentsOfCell(_selectedCell);
        _selectedCellValue = ValueOfCell(_selectedCell);
        
        // Get the old input and value of the cell
        _oldCellInput = _selectedCellInput;
        _oldCellValue = _selectedCellValue;
        
        // Select input area
        await SelectInput();
    }

    /// <summary>
    ///   <para>
    ///     Private method to be called for when clicking a new cell.
    ///     This then selects the input area to allow for typing directly after selecting a cell.
    ///   </para>
    /// </summary>
    private async Task SelectInput()
    {
        await _inputElement.FocusAsync();
    }

    /// <summary>
    ///   <para>
    ///     Gets the StringForm of the contents of this cell.
    ///   </para>
    /// </summary>
    /// <param name="name">The name of the cell to retrieve from</param>
    /// <returns>The StringForm of the contents</returns>
    private string ContentsOfCell(string name)
    {
        var contents = _spreadsheet.GetCellContents(name);
        if (contents is Formula)
        {
            return "=" + contents;
        }

        return contents.ToString() ?? string.Empty;
    }

    /// <summary>
    ///   <para>
    ///     Gets the value of the cell
    ///   </para>
    /// </summary>
    /// <param name="name">The name of the cell to retrieve from</param>
    /// <returns>The string version of the value</returns>
    private string ValueOfCell(string name)
    {
        var value = _spreadsheet.GetCellValue(name);
        
        // Check that the value is an error to later display
        if (value is FormulaError error)
        {
            return error.Reason;
        }

        return value.ToString() ?? string.Empty;
    }

    /// <summary>
    ///   <para>
    ///     Changes the contents of the cell selected in the UI.
    ///     This Method only triggers once the user presses enter or clicks away from the cell.
    ///     Once the user has done this a value will be calculated and any possible exceptions will be thrown.
    ///   </para>
    /// </summary>
    /// <param name="args">The supplied information about a change that has occured</param>
    private void ChangeCellContents(ChangeEventArgs args)
    {
        // Get the value of args which is the contents of the input field
        var contents = args.Value!.ToString() ?? string.Empty;
        
        ChangeCellContents(_selectedCell,contents);
        
        // Update the member variables to reflect this selected cell
        _selectedCellInput = contents;
        _selectedCellValue = ValueOfCell(_selectedCell);
        
        // Push this information into the _back stack for later use of undo
        _back.Push(new CellInfoChanged(_selectedCell, _oldCellInput, _oldCellValue));
        
        // The forward stack must be cleared, similar to excel or a webpage
        _forward.Clear();
    }
    
    /// <summary>
    ///   <para>
    ///     Updates the Ui of the spreadsheet without actually calculating the values.
    ///     This then allows for the display to be updates when writing a formula but without,
    ///     the formula throwing many exceptions, as when writing a formula many rules are broken
    ///     before finishing a valid formula.
    ///   </para>
    ///   <para>
    ///     The values and contents of the spreadsheet will be immediately calculated correctly
    ///     once the user presses enter or clicks away from the cell (as this calls ChangeCellContents), thus overriding any partially incorrect
    ///     data that this method might temporarily place into the spreadsheet
    ///   </para>
    /// </summary>
    /// <param name="args">The supplied information about a change that has occured</param>
    private void UpdateUiWithoutCalculation(ChangeEventArgs args)
    {
        // Get the value of args which is the contents of the input field
        var contents = args.Value!.ToString() ?? string.Empty;
        
        // Update the member variables to reflect this selected cell
        _selectedCellInput = contents;
        _selectedCellValue = contents;
        
        // Get coords fo this selected cell
        var coords = GetCellCoord(_selectedCell);
        
        // This will update the backing store to the contents without having to calculate the value
        CellsBackingStore[coords[0], coords[1]] = contents;
        
    }

    /// <summary>
    ///   <para>
    ///     Change the cell contents of the 
    ///   </para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="content"></param>
    private void ChangeCellContents(string name, string content)
    {
        try
        {
            var list = _spreadsheet.SetContentsOfCell(name, content);
            foreach (var cell in list)
            {
                var coords = GetCellCoord(cell);
                CellsBackingStore[coords[0], coords[1]] = ValueOfCell(cell);
            }
        }
        catch (Exception e)
        {
            var coords = GetCellCoord(_selectedCell);
            _spreadsheet.SetContentsOfCell(name, _oldCellInput);
            CellsBackingStore[coords[0], coords[1]] = _oldCellValue;
            HandleException(e);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    private void HandleException(Exception e)
    {
        _exceptionMessage = e.Message;
        if (e is CircularException)
        {
            _exceptionMessage = "Circular Exception detected at Spreadsheet Cell: " + _selectedCell;
        }
        _showPopup = true;
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    private async void ClosePopup()
    {
        _showPopup = false;
        await SelectInput();
    }

    /// <summary>
    ///   <para>
    ///     Get the name of the clicked cell.
    ///   </para>
    /// </summary>
    /// <param name="row">The row component of the cell's coordinates</param>
    /// <param name="col">The column component of the cell's coordinates</param>
    private string GetCellName(int row, int col)
    {
        return Alphabet[col].ToString() + (row + 1);
    }

    /// <summary>
    ///   <para>
    ///     Get the coordinates of the clicked cell from name.
    ///   </para>
    /// </summary>
    /// <param name="cell">The name of the cell.</param>
    private int[] GetCellCoord(string cell)
    {
        int.TryParse(cell[1..], out var num);
        return [num - 1, Array.IndexOf(Alphabet, cell[0])];
    }

    #region SaveClearLoad

    /// <summary>
    /// Saves the current spreadsheet, by providing a download of a file
    /// containing the json representation of the spreadsheet.
    /// </summary>
    private async void SaveFile()
    {
        await JSRuntime.InvokeVoidAsync("downloadFile", FileSaveName, _spreadsheet.JsonString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    private void SaveFileAs(ChangeEventArgs args)
    {
        var name = args.Value!.ToString() ?? string.Empty;
        if ((string)args.Value! != string.Empty)
        {
            FileSaveName = name + ".sprd";
            return;
        }
        if (_loadedFileName != string.Empty)
        {
            FileSaveName = _loadedFileName;
            return;
        }
        FileSaveName = "spreadsheet.sprd"; ;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearSpreadsheet()
    {
        CellsBackingStore = new string[Rows, Cols];
        _spreadsheet = new Spreadsheet();
        _back.Clear();
        _forward.Clear();
        CellClicked(0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadFromSpreadsheet()
    {
        var nonemptyCells = _spreadsheet.GetNamesOfAllNonemptyCells();
        foreach (var cell in nonemptyCells)
        {
            ChangeCellContents(cell, ContentsOfCell(cell));
        }
    }

    /// <summary>
    /// This method will run when the file chooser is used, for loading a file.
    /// Uploads a file containing a json representation of a spreadsheet, and 
    /// replaces the current sheet with the loaded one.
    /// </summary>
    /// <param name="args">The event arguments, which contains the selected file name</param>
    private async void HandleFileChooser(EventArgs args)
    {
        try
        {
            var fileContent = string.Empty;

            var eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("unable to get file name");
            if (eventArgs.FileCount != 1) return;
            var file = eventArgs.File;
            if (file is null)
            {
                return;
            }
            _loadedFileName = file.Name;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            // fileContent will contain the contents of the loaded file
            fileContent = await reader.ReadToEndAsync();

            // Clear First
            ClearSpreadsheet();
            _spreadsheet.CreateSpreadSheet(fileContent);
            LoadFromSpreadsheet();
            StateHasChanged();
        }
        catch (Exception e)
        {
            Debug.WriteLine("an error occurred while loading the file..." + e);
        }
    }

    #endregion

    private class CellInfoChanged(string name, string input, string value)
    {
        public readonly string Name = name;
        public readonly string Input = input;
        public readonly string Value = value;
    }

    private void Undo()
    {
        UpdateStack(_back, _forward);
    }

    private void Redo()
    {
        UpdateStack(_forward, _back);
    }

    private void UpdateStack(Stack<CellInfoChanged> changes, Stack<CellInfoChanged> reverseChanges)
    {
        CellInfoChanged change;
        try
        {
            change = changes.Pop();
        }        
        catch (Exception e)
        {
            return;
        }
        reverseChanges.Push(new CellInfoChanged(change.Name, ContentsOfCell(change.Name), ValueOfCell(change.Name)));
        _selectedCell = change.Name;
        _selectedCellInput = change.Input;
        _selectedCellValue = change.Value;
        ChangeCellContents(change.Name, change.Input);  
    }
}