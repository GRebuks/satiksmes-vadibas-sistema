using System;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// Used to create output tables for printing out information
/// </summary>
namespace Transport_Management_System
{
    class TableBuilder
    {
        private const int TAB_SIZE = 8;
        private const int ADD_CHAR_SIZE = 2;

        private string _tableHeader;
        private List<List<dynamic>> _tableCells;
        private List<int> _columnWidths = new List<int>();
        //private List<string> _options;

        // Main method of building a table string
        public string BuildTable(string tableHeader, List<List<dynamic>> tableCells)
        {
            _tableHeader = tableHeader;
            _tableCells = tableCells;

            int width = TableWidth();
            StringBuilder sb = new StringBuilder();

            // Table header cell
            BuildLine(width, ref sb);
            BuildCell(_tableHeader, width, ref sb);
            sb.Append("|\n");
            BuildLine(width, ref sb);

            // Table cell output
            for (int i = 0; i < _tableCells.ToArray().GetLength(0); i++)
            {
                for (int j = 0; j < _tableCells[0].ToArray().GetLength(0); j++)
                {
                    BuildCell(_tableCells[i][j], _columnWidths[j], ref sb);
                }
                sb.Append("|\n");
            }
            BuildLine(width, ref sb);
            // Options go here
            BuildLine(width, ref sb);

            return sb.ToString();
        }

        // Calculates and returns the total width of a table
        private int TableWidth()
        {
            int width = 0;
            foreach (List<dynamic> columnCells in TransposeTable())
            {
                int columnSize = MaxCellSize(columnCells);
                width += columnSize;
                _columnWidths.Add(columnSize);
            }
            return width;
        }

        // Calculates the maximum cell size of each column
        // Iterates through the list to find the max value
        private int MaxCellSize(List<dynamic> cells)
        {
            int max = 0;
            foreach (dynamic cell in cells)
            {
                // Calculates the nearest tab size, using TAB_SIZE const for correct table output
                // ADD_CHAR_SIZE const used for the number of characters that surround the cell value
                int nearestTab = (int)Math.Ceiling((double)(cell.ToString().Length + ADD_CHAR_SIZE) / TAB_SIZE) * TAB_SIZE;

                if (nearestTab > max)
                {
                    max = nearestTab;
                }
            }
            return max;
        }

        // Builds a basic line with the specified width
        private void BuildLine(int width, ref StringBuilder sb)
        {
            for (int i = 1; i <= width; i++)
            {
                if (i == 1 || i == width)
                {
                    sb.Append("+");
                    continue;
                }
                sb.Append("-");
            }
            sb.Append("\n");
        }

        // Builds a complete cell in a table, adding necessary tabs according to width
        private void BuildCell(dynamic cell, int width, ref StringBuilder sb)
        {
            sb.Append($"|{cell}");

            int difference = width - cell.ToString().Length;

            // When these values are specific, the table breaks out of shape
            // These checks prevent that from happening, excluding ADD_CHAR_SIZE const from the difference
            if (cell.ToString().Length == 7 || cell.ToString().Length == 8 || difference == 9)
                FillEmpty(difference - ADD_CHAR_SIZE, ref sb);
            else
                FillEmpty(difference, ref sb);
        }

        // Calculates and fills the empty spaces with tabs according to the specified width
        private void FillEmpty(int width, ref StringBuilder sb)
        {
            for (int i = 0; i < Math.Ceiling((decimal)width / TAB_SIZE); i++)
            {
                sb.Append("\t");
            }
        }

        // Returns a transposed table for size calculations
        private List<List<dynamic>> TransposeTable()
        {
            List<List<dynamic>> transposedTable = new List<List<dynamic>>();
            for (int i = 0; i < _tableCells[0].Count; i++)
            {
                List<dynamic> col = new List<dynamic>();
                for (int j = 0; j < _tableCells.Count; j++)
                {
                    col.Add(_tableCells[j][i]);
                }
                transposedTable.Add(col);
            }
            return transposedTable;
        }
    }
}