using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

namespace InvestmentManagementReport.Widgets
{
    internal sealed class TwoColumnLayoutWidget
    {
        public TableCellBuilder LeftColumn { get; }
        public TableCellBuilder RightColumn { get; }
        
        private TwoColumnLayoutWidget(TableCellBuilder leftColumn, TableCellBuilder rightColumn)
        {
            LeftColumn = leftColumn;
            RightColumn = rightColumn;
        }
        
        public static TwoColumnLayoutWidget AddTo(SectionBuilder sectionBuilder)
        {
            TableBuilder tableBuilder = sectionBuilder
                .AddTable(XUnit.FromPercent(50f), 10f, XUnit.FromPercent(50f))
                .SetWidth(sectionBuilder.PageSize.Width - sectionBuilder.Margins.Horizontal)
                .SetBorder(Stroke.None);
            TableRowBuilder rowBuilder = tableBuilder
                .AddRow();
            TableCellBuilder leftColumnCell = rowBuilder.AddCell();
            TableCellBuilder rightColumnCell = rowBuilder.AddCellToRow().AddCell();
            
            return new TwoColumnLayoutWidget(leftColumnCell, rightColumnCell);
        }
    }
}