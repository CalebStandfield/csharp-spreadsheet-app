// <authors> [Ethan Perkins] </authors>
// <date> [10/18/2024] </date>
namespace CS3500.Spreadsheet
{
    using CS3500.Formula;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Cell used to store information in a Spreadsheet.
    /// Stores the contens of a cell, alongside its value (has not been implemented yet).
    /// </summary>
    internal class Cell
    {
        /// <summary>
        /// Value of a cell, either as a double or a string.
        /// </summary>
        internal object Value
        {
            get; set;
        }

        /// <summary>
        /// Contents of a cell, either as a Formula, double, or a string.
        /// </summary>
        internal object Contents {
            get;
        }

        /// <summary>
        /// String form of the contents of this cell.
        /// </summary>
        [JsonInclude]
        internal string StringForm
        {
            get; private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        [JsonConstructor]
        internal Cell()
        {
            Contents = 0;
            Value = 0;
            StringForm = string.Empty;
        }

        /// <summary>
        /// Constructs the cell with Formula contents.
        /// </summary>
        /// <param name="contents">Formula to store in the cell</param>
        internal Cell(Formula content)
        {
            Contents = content;
            Value = 0;
            StringForm = "=" + content.ToString();
        }
        /// <summary>
        /// Constructs the cell with string contents.
        /// </summary>
        /// <param name="contents">String to store in the cell</param>
        internal Cell(string content)
        {
            Contents = content;
            Value = content;
            StringForm = content;
        }
        /// <summary>
        /// Constructs the cell with double contents.
        /// </summary>
        /// <param name="contents">Double to store in the cell</param>
        internal Cell(double content)
        {
            Contents = content;
            Value = content;
            StringForm = content.ToString();
        }
    }
}
