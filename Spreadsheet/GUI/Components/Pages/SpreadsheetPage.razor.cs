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
    ///     The backing spreadsheet for the spreadsheet gui
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
    ///     The current selected cell of the spreadsheet as a duo of numbers.
    ///     Default value of [0,0].
    ///   </para>
    /// </summary>
    private int[] _selectedCellCoords = [0, 0];

    /// <summary>
    ///   <para>
    ///     The current inputted string of the selected cell of the spreadsheet.
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
    ///     The current inputted string of the selected cell of the spreadsheet.
    ///     Default value of an empty string.
    ///   </para>
    /// </summary>
    private string _oldCellInput = string.Empty;
    
    /// <summary>
    ///   <para>
    ///     The current value of the selected cell of the spreadsheet.
    ///     Default value of an empty string.
    ///   </para>
    /// </summary>
    private string _oldCellValue = string.Empty;

    /// <summary>
    /// Provides an easy way to convert from an index to a letter (0 -> A)
    /// </summary>
    private char[] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    
    /// <summary>
    ///   Gets or sets the name of the file to be saved
    /// </summary>
    private string FileSaveName { get; set; } = "Spreadsheet.sprd";
    
    /// <summary>
    ///   <para> Gets or sets the data for all the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore { get; set; } = new string[Rows, Cols];

    private Stack<CellInfoChanged> _back = new();

    private Stack<CellInfoChanged> _forward = new();

    private ElementReference _inputElement;
    
    private string _exceptionMessage = string.Empty;
    
    private bool _showPopup = false;

    private string _loadedFileName = string.Empty;
    
    /// <summary>
    /// Handler for when a cell is clicked
    /// </summary>
    /// <param name="row">The row component of the cell's coordinates</param>
    /// <param name="col">The column component of the cell's coordinates</param>
    private async void CellClicked(int row, int col)
    {
        
        _selectedCell = GetCellName(row, col);
        _selectedCellCoords = [row, col];
        _selectedCellInput = ContentsOfCell(_selectedCell);
        _selectedCellValue = ValueOfCell(_selectedCell);
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
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string ContentsOfCell(string input)
    {
        var contents = _spreadsheet.GetCellContents(input);
        if (contents is Formula)
        {
            return "=" + contents;
        }

        return contents.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private string ValueOfCell(string cell)
    {
        var value = _spreadsheet.GetCellValue(cell);
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
    /// <param name="args"></param>
    private void ChangeCellContents(ChangeEventArgs args)
    {
        var contents = args.Value!.ToString() ?? string.Empty;
        ChangeCellContents(contents, _selectedCell);
        _selectedCellInput = contents;
        _selectedCellValue = ValueOfCell(_selectedCell);
        _back.Push(new CellInfoChanged(_selectedCell, _oldCellInput, _oldCellValue));
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
    /// <param name="args"></param>
    private void UpdateUiWithoutCalculation(ChangeEventArgs args)
    {
        var contents = args.Value!.ToString() ?? string.Empty;
        _selectedCellInput = contents;
        var coords = GetCellCoord(_selectedCell);
        _selectedCellValue = contents;
        CellsBackingStore[coords[0], coords[1]] = contents;
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="name"></param>
    private void ChangeCellContents(string content, string name)
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
    
    // This method handles the exception and shows the popup
    private void HandleException(Exception e)
    {
        _exceptionMessage = e.Message;
        _showPopup = true;
        StateHasChanged();
    }

    // Close the popup
    private void ClosePopup()
    {
        _showPopup = false;
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
            ChangeCellContents(ContentsOfCell(cell), cell);
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
        ChangeCellContents(change.Input, change.Name);  
    }
}