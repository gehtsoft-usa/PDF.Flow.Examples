using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using InvestmentManagementReport.Model;
using System.Collections.Generic;

namespace InvestmentManagementReport.Widgets
{
    public sealed class NotesWidget
    {
        private readonly TableBuilder _tableBuilder;
        private static readonly FontBuilder FNT_7_5I = Fonts.Helvetica(7.5f).SetOblique();
        private NotesWidget(TableBuilder tableBuilder)
        {
            _tableBuilder = tableBuilder;
        }
        public void FillData(List<NameValuePair> data)
        {
            foreach (var note in data)
            {
                _tableBuilder.AddRow()
                    .AddCellToRow(note.Name)
                    .AddCellToRow(note.Value);
            }
        }
        public static NotesWidget AddTo(TableCellBuilder cellBuilder)
        {
            TableBuilder notesTable = 
                cellBuilder.AddTable()
                    .SetMarginTop(5f)
                    .SetBorderStroke(Stroke.None)
                    .AddColumnPercentToTable("", 4f)
                    .AddColumnPercentToTable("", 96f)
                    .SetContentRowStyleFont(FNT_7_5I);
            return new NotesWidget(notesTable);
        }
        public static NotesWidget AddTo(SectionBuilder sectionBuilder)
        {
            TableBuilder notesTable = 
                sectionBuilder.AddTable()
                    .SetWidth(XUnit.FromPercent(45))
                    .SetMarginTop(5f)
                    .SetBorderStroke(Stroke.None)
                    .AddColumnPercentToTable("", 4f)
                    .AddColumnPercentToTable("", 96f)
                    .SetContentRowStyleFont(FNT_7_5I);
            return new NotesWidget(notesTable);
        }
    }
}
