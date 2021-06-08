using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using InvestmentManagementReport.Model;

namespace InvestmentManagementReport.Widgets
{
    public sealed class GreenTableWidget
    {
        private readonly TableBuilder _tableBuilder;

        private static readonly Color GreenColor = Color.FromHtml("#3F7600");
        private static readonly FontBuilder FNT_9B = Fonts.Helvetica(9f).SetBold();
        private static readonly FontBuilder FNT_9 = Fonts.Helvetica(9f);
        private static readonly FontBuilder FNT_9I = Fonts.Helvetica(9f).SetOblique();
        private static readonly FontBuilder FNT_7_5_WHITE = Fonts.Helvetica(8f).SetColor(Color.White);
        
        private GreenTableWidget(TableBuilder tableBuilder)
        {
            _tableBuilder = tableBuilder;
        }

        public static GreenTableWidget AddTo(TableCellBuilder cellBuilder)
        {
            TableBuilder greenTable = cellBuilder.AddTable()
                .SetBorderStroke(Stroke.None);
            return new GreenTableWidget(greenTable);
        }

        public static GreenTableWidget AddTo(SectionBuilder sectionBuilder)
        {
            TableBuilder greenTable = sectionBuilder.AddTable()
                .SetBorderStroke(Stroke.None);
            return new GreenTableWidget(greenTable);
        }

        public void FillData(GreenTable tableInfo)
        {
            AddColumns(tableInfo.Columns);
            AddRows(tableInfo);
        }

        private void AddColumns(List<GreenTableColumn> columns)
        {
            int countOfColumns = columns.Sum(c => c.ColSpan > 1 ? c.ColSpan : 1);
            float percentsPerColumn = 100f / countOfColumns;
            foreach (var column in columns)
            {
                for (int i = 0; i < (column.ColSpan > 1 ? column.ColSpan : 1); i++)
                {
                    _tableBuilder.AddColumnPercent("", percentsPerColumn);
                }
            }
        }

        private void AddRows(GreenTable tableInfo)
        {
            GreenTableDataItem startItem = tableInfo.Data.FirstOrDefault(it => it.IsStart);
            GreenTableDataItem endItem = tableInfo.Data.FirstOrDefault(it => it.IsEnd);
            GreenTableDataItem[] contentItems = tableInfo.Data.Where(it => !it.IsEnd && !it.IsStart).ToArray();
            
            AddHeaderRow(tableInfo.Columns);
            if (startItem != null)
                AddRow(startItem, tableInfo.Columns);
            foreach (var contentItem in contentItems)
            {
                AddRow(contentItem, tableInfo.Columns);
            }
            if (endItem != null)
                AddRow(endItem, tableInfo.Columns);
        }

        private void AddHeaderRow(List<GreenTableColumn> columns)
        {
            TableRowBuilder headerRow = _tableBuilder
                .AddRow()
                .SetFont(FNT_7_5_WHITE)
                .SetBackColor(GreenColor);
            foreach (var column in columns)
            {
                TableCellBuilder headerRowCell = headerRow
                    .AddCell(column.Title ?? "")
                    .SetPadding(1f, 5f, 2f, 1f);
                if (column.ColSpan > 1)
                    headerRowCell.SetColSpan(column.ColSpan);
                if (column.IsRightAligned)
                    headerRowCell.SetHorizontalAlignment(HorizontalAlignment.Right);
            }
        }

        private void AddRow(GreenTableDataItem item, List<GreenTableColumn> columns)
        {
            TableRowBuilder row = _tableBuilder
                .AddRow()
                .SetFont(FNT_9);
            if (item.IsStart)
            {
                row.SetFont(FNT_9B);
            }
            else if (item.IsEnd)
            {
                row.SetFont(FNT_9B)
                    .SetBorderWidth(1f)
                    .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None);
            }
            else if (item.IsSubItem)
            {
                row.SetFont(FNT_9I);
            }
            int i = 0;
            foreach (var column in columns)
            {
                var text = item.Data.TryGetValue($"ValueColumn{i++}", out JsonElement el) ? el.GetString() : "";
                TableCellBuilder rowCell = row.AddCell(text);
                if (column.ColSpan > 1)
                    rowCell.SetColSpan(column.ColSpan);
                if (column.IsRightAligned)
                    rowCell.SetHorizontalAlignment(HorizontalAlignment.Right);
                if (item.IsStart)
                {
                    rowCell.SetPadding(0f, 0f, 0f, 5f);
                }
                if (item.IsSubItem)
                {
                    rowCell.SetPadding(9f, 0f, 0f, 0f);
                }
            }
        }
    }
}