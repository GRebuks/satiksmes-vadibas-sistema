using System;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// Used to create output tables for printing out information
/// </summary>
class TableBuilder
{
    private string _tableHeader;
    private List<List<string>> _tableCells;
    private List<int> _columnWidths;
    private List<string> _options;
    
    public string BuildTable(string tableHeader, List<List<string>> _tableCells)
    {
        _tableHeader = tableHeader;
        _tableCells = _tableCells;

        int width = TableWidth();
        StringBuilder sb = new StringBuilder();

        // Table header cell
        BuildLine(width, ref sb);
        BuildCell(_tableHeader, width, ref sb);
        BuildLine(width, ref sb);

        // Table column width size calculation
        for (int i = 0; i < _tableCells.Count; i++) {
            _columnWidths[i] = MaxCellSize(_tableCells[i]);
        }

        // Table cell output
        // i - row
        for (int i = 0; i < _tableCells[0].Count; i++) 
        {
            // j - column
            for (int j = 0; j < _tableCells.Count; j++)
            {
                BuildCell(_tableCells[j][i], _columnWidths[j], ref sb);
            }
            sb.Append("\n");
        }

        return sb.ToString();
    }

    private int TableWidth() {
        int width;
        foreach (List<string> columnCells in _tableCells)
        {
            width += MaxCellSize(columnCells);
        }
        return width;
    }

    private int MaxCellSize(List<string> cells)
    {
        int max = 0;
        foreach (string cell in cells)
        {
            if (cell.Length > max)
                max = cell.Length;
        }
        return max;
    }

    private void BuildLine(int width, ref StringBuilder sb) {
        for (int i = 0; i < width; i++) {
            if (i == 0 || i == width) 
            {
                sb.Append("+");
                continue;
            }
            sb.Append("-");
        }
        sb.Append("\n");
    }

    private void BuildCell(string cell, int width, ref StringBuilder sb) 
    {
        string cellStart = $"| {cell}";
        sb.Append($"{cellStart}|");
        FillEmpty(width - cellStart.Length, ref sb);
    }

    private void FillEmpty(int width, ref StringBuilder sb)
    {
        for (int i = 0; i < (width / 4).Ceiling(); i++)
        {
            sb.Append("\t");
        }
    }
}