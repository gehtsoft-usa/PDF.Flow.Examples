using System;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

namespace LogBook
{
    public enum StrikethroughMode
    {
        TextOnly,
        FullRow,
    }
    public class PDFSettings
    {
        public Box SectionMargins { get; set; }
        public PaperSize PaperSize { get; set; } = PaperSize.A4;
        public PageOrientation Orientation { get; set; } = PageOrientation.Landscape;
        public string HeaderOdd { get; set; }
        public string HeaderEven { get; set; }
        public string BookName { get; set; }
        public DateTime? DateOfPrint { get; set; }
        public DateTime? DateRangeStart { get; set; }
        public DateTime? DateRangeEnd { get; set; }
        public long NumberOfRecords { get; set; }
        public string DateFormat { get; set; } = "d/M/yyyy";
        public TextOverflowAction TextOverflowAction { get; set; } = TextOverflowAction.Ellipsis;
        public Stroke DiscardedStrikethroughStroke { get; set; } = Stroke.Solid;
        public Color DiscardedStrikethroughColor { get; set; } = Color.Black;
        public StrikethroughMode DiscardedStrikethroughMode { get; set; } = StrikethroughMode.TextOnly;
        public uint StartingPage { get; set; }
        public bool DisplayDateRange { get; set; } = false;
    }
}
