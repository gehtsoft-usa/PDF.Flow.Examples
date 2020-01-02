using System;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Events.Data;
using Gehtsoft.PDFFlow.Models.Content;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

namespace Gehtsoft.PDFFlow.LogBook
{
    public enum LinethroughMode
    {
        TextOnly,
        FullRow,
    }
    public class PDFSettings
    {
        public Box Padding { get; set; }
        public PaperSize PaperSize { get; set; } = PaperSize.A4;
        public PageOrientation Orientation { get; set; } = PageOrientation.Landscape;
        public string HeaderOdd { get; set; }
        public string HeaderEven { get; set; }
        //public string FflNo { get; set; }
        public string BookName { get; set; }
        public DateTime? DateOfPrint { get; set; }
        public DateTime? DateRangeStart { get; set; }
        public DateTime? DateRangeEnd { get; set; }
        public string CustomFilter { get; set; }
        public long NumberOfRecords { get; set; }
        public Action<NewPageCreated> PageChanged { get; set; }
        public Action<TableRowLayoutEventArgs> RowLayout { get; set; }
        public Stroke HeaderLineStyle { get; set; } = Stroke.Solid;
        public string DateFormat { get; set; } = "d/M/yyyy";
        public TextOverflowAction TextOverflowAction { get; set; } = TextOverflowAction.Ellipsis;

        public Stroke DiscardedLineStyle { get; set; } = Stroke.Solid;
        public Color DiscardedLineColor { get; set; } = Color.Black;
        public LinethroughMode DiscardedLineDrawMode { get; set; } = LinethroughMode.TextOnly;
        public uint StartingPage { get; set; }
        public bool DisplayDateRange { get; set; } = false;
    }
}
