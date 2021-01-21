using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LogBook.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

namespace LogBook
{
    internal static class ReflectionHelper
    {
        internal static IEnumerable<PropertyInfo> GetProperties(this object obj)
        {
            var propList = obj.GetType().GetProperties();
            return propList;
        }
    }
    internal class LogBookCoordinator : IStreamCoordinator
    {
        #region Fields
        private readonly DocumentBuilder _builder;
        private readonly PDFSettings _options;
        private readonly IStream _stream;
        private bool _disposed;
        private readonly bool _continuous;
        private TableBuilder _table;
        private readonly string DateFormat = @"MM\/dd\/yyyy";
        private FontBuilder exoRegular;
        private FontBuilder exoItalic;
        private FontBuilder exoBold;
        private readonly IEnumerable<SectionBuilder> _sections;
        #endregion Fields

        #region Constructors
        public LogBookCoordinator(IStream stream, bool continuous = false)
        {
            _stream = stream;
            _builder = stream.Builder;
            _options = stream.Options;
            DateFormat = _options.DateFormat ?? DateFormat;
            stream.Coordinator = this;
            _continuous = continuous;
            exoRegular = FontBuilder.New().FromFile(stream.exoRegularFile, 10);
            exoItalic = FontBuilder.New().FromFile(stream.exoItalicFile, 10);
            exoBold = FontBuilder.New().FromFile(stream.exoBoldFile, 10);
            _sections = CreateSections();
            if (continuous)
            {
                BuildSections();
            }
        }
        public LogBookCoordinator(string file, PDFSettings options, int startingPage = 0, bool continuous = false) : this(new DocumentStream(file, options) { StartingPage = startingPage }, continuous) { }
        public LogBookCoordinator(Stream stream, PDFSettings options, int startingPage = 0, bool continuous = false) : this(new DocumentStream(stream, options) { StartingPage = startingPage }, continuous) { }
        #endregion Constructors

        #region Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_stream != null)
                {
                    if (!_continuous)
                        BuildSections();
                    _stream.Dispose();
                }
            }
            _disposed = true;
        }

        private void BuildSections()
        {
            foreach (SectionBuilder section in _sections)
            {
                _builder.AddSection(section);
            }
        }

        public void Input<T>(T data) where T : IEntity
        {
            if (data is Operation entity)
            {
                Input(entity);
            }
            else
            {
                Dictionary<string, object> properties = new Dictionary<string, object>();
                foreach (PropertyInfo info in data.GetProperties())
                {
                    properties.Add(info.Name, info.GetValue(data));
                }

                Input(properties);
            }

        }

        public void Input<T>(IEnumerable<T> data) where T : IEntity
        {
            foreach (var item in data)
            {
                Input(item);
            }
        }

        public void Input(IDictionary<string, object> data)
        {
            var entity = new Operation();
            foreach (var pair in data)
            {
                PropertyInfo property = typeof(Operation).GetProperty(pair.Key);
                if (property.CanWrite)
                    property.SetValue(entity, pair.Value);
            }
            Input(entity);
        }

        public void Input(Operation book)
        {
            TableRowBuilder row = _table.AddRow();

            // Received                              
            row
                .AddCellToRow(book.Code)
                .AddCellToRow(book.Manufacturer)
                .AddCellToRow(book.Model)
                .AddCellToRow(book.VIN)
                .AddCellToRow(book.Received.HasValue ? book.Received.Value.ToString(DateFormat) : "")
                .AddCell(book.Source);

            // Sent
            row
                .AddCellToRow(book.Code)
                .AddCellToRow(book.Sent.HasValue ? book.Sent.Value.ToString(DateFormat) : "")
                .AddCellToRow(book.Buyer_Name)
                .AddCell(book.FullAddress);

            if (book.Discarded)
            {
                row
                    .SetFont(exoItalic)
                    .SetStrikethrough(_options.DiscardedStrikethroughStroke, _options.DiscardedStrikethroughColor, _options.DiscardedStrikethroughMode == StrikethroughMode.FullRow);
            }

            if (book.Discarded && !string.IsNullOrEmpty(book.DiscardReason))
            {
                row = _table.AddRow();
                row
                   .SetFont(exoItalic).SetFontSize(8).SetFontColor(Color.Gray)
                   .SetBackColor(Color.FromRgba(1.0, 1.0, 0.9))
                   .AddCellToRow("")
                   .AddCellToRow(book.DiscardReason, 2)
                   .AddCellToRow("")
                   .AddCellToRow("")
                   .AddCellToRow("")
                   .AddCellToRow("")
                   .AddCellToRow("")
                   .AddCellToRow("")
                   .AddCellToRow("");
            }
        }

        private IEnumerable<SectionBuilder> CreateSections()
        {
            // Section 1

            var s1 = SectionBuilder.New()
                .SetStyleFont(exoRegular)
                .SetMargins(50)
                .SetSize(_options.PaperSize)
                .SetOrientation(_options.Orientation)
                .AddParagraph("Operations Log")
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetFontSize(46)
                    .SetMarginTop(50)
            .ToSection()
                .AddParagraph("Book")
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetFontSize(56)
                    .SetMarginTop(10)
            .ToSection()
                .AddParagraph("Powered by PDFFlow")
                    .SetFont(exoBold)
                    .SetAlignment(HorizontalAlignment.Right)
                    .SetMarginTop(100)
            .ToSection();

            // Section 2

            var s2 = SectionBuilder.New()
                .SetStyleFont(exoRegular)
                .SetMargins(_options.SectionMargins)
                .SetSize(_options.PaperSize)
                .SetOrientation(_options.Orientation)
                .SetPageNumberStart(_options.StartingPage - 1);

            AddRepeatingAreas(s2);

            StyleBuilder style = StyleBuilder.New()
                    .SetHorizontalAlignment(HorizontalAlignment.Left)
                    .SetVerticalAlignment(VerticalAlignment.Bottom)
                    .SetBackColor(Color.White)
                    .SetFont(exoRegular)
                    .SetFontSize(13);

            _table = s2.AddTable();

            _table.SetMultipageSpread(6, 4);

            _table
                .SetRepeatHeaders(true)
                .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.Solid)
                .SetContentRowStyleTextOverflowAction(_options.TextOverflowAction);

            _table
                .AddColumnPercentToTable("#", 5)
                .AddColumnPercentToTable("Manufacturer", 24)
                .AddColumnPercentToTable("Model", 16)
                .AddColumnPercentToTable("VIN", 24)
                .AddColumnPercentToTable("Date Received", 16)
                .AddColumnPercentToTable("Source", 15)
                .AddColumnPercentToTable("#", 5)
                .AddColumnPercentToTable("Date Sent", 16)
                .AddColumnPercentToTable("Buyer Name", 30)
                .AddColumnPercentToTable("Buyer Address", 49);

            _table
                .SetHeaderRowStyleFont(exoBold)
                .SetHeaderRowStyleBackColor(Color.FromRgba(.8f, .8f, .8f))
                .SetHeaderRowStyleHorizontalAlignment(HorizontalAlignment.Left)
                .AddHeaderRow()
                .ApplyStyle(style)                        
                .SetMinHeight(25)
                .AddCellToRow("")
                .AddCellToRow("Description of model", 5)
                .AddCellToRow("")
                .AddCellToRow("Reciept", 3);

            // Section 3

            var s3 = SectionBuilder.New().SetStyleFont(exoRegular.SetSize(11));

            string dateFormat = DateFormat;

            s3
                .SetMargins(_options.SectionMargins)
                .SetSize(_options.PaperSize)
                .SetOrientation(_options.Orientation)
                .AddParagraph("Book Name")
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetMargins(0, 200, 0, 0)
            .ToSection()    
                .AddParagraph(_options.BookName)
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetFont(exoBold)
            .ToSection()
                .AddParagraph("Date of Print")
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetMargins(0, 10, 0, 0)
            .ToSection()
                .AddParagraph(_options.DateOfPrint.Value.ToString(dateFormat))
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetFont(exoBold)
            .ToSection()
                .AddParagraph("Date range")
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetMargins(0, 10, 0, 0)
            .ToSection()
                .AddParagraph(string.Concat(_options.DateRangeStart.Value.ToString(dateFormat), " - ", _options.DateRangeEnd.Value.ToString(dateFormat)))
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(exoBold)
            .ToSection()
                .AddParagraph("Number of Records")
                .SetAlignment(HorizontalAlignment.Center)
                .SetMargins(0, 10, 0, 0)
            .ToSection()
                .AddParagraph(_options.NumberOfRecords.ToString())
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(exoBold);

            return new List<SectionBuilder> {s1, s2, s3};
        }

        private void AddRepeatingAreas(SectionBuilder section)
        {
            AddHeader(true);
            AddHeader(false);
            AddFooter(true);
            AddFooter(false);

            void AddHeader(bool isOddPage)
            {
                string SentOrReceived = isOddPage ? "SENT" : "RECEIVED";
                string HeaderText = isOddPage ? _options.HeaderOdd : _options.HeaderEven;
                RepeatingAreaBuilder header; 
                if (isOddPage)
                    header = section.AddHeaderToOddPage(50);
                else
                    header = section.AddHeaderToEvenPage(50);
                header
                    .AddTable()
                        .SetBorderStroke(Stroke.None)
                        .AddColumnToTable()
                        .AddColumnToTable()
                        .AddRow()
                        .AddCell(SentOrReceived)
                                .SetFontSize(24)
                                .SetVerticalAlignment(VerticalAlignment.Bottom)
                        .ToRow()
                            .AddCell(HeaderText)
                                .SetPadding(0, 0, 10, 0)
                                .SetHorizontalAlignment(HorizontalAlignment.Right)
                                .SetVerticalAlignment(VerticalAlignment.Bottom);
            }

            void AddFooter(bool isOddPage)
            {
                string dateRangeStr1 = "";
                string dateRangeStr2 = "";
                if (_options.DisplayDateRange && _options.DateRangeStart.HasValue && _options.DateRangeEnd.HasValue)
                {
                    dateRangeStr1 = " | Date Range ";
                    dateRangeStr2 = $"{_options.DateRangeStart.Value.ToString(DateFormat)} - {_options.DateRangeEnd.Value.ToString(DateFormat)}";
                }
                RepeatingAreaBuilder footer;
                if (isOddPage)
                    footer = section.AddFooterToOddPage(30);
                else
                    footer = section.AddFooterToEvenPage(30);
                footer
                    .AddTable()
                        .SetBorderStroke(Stroke.None)
                        .SetContentRowStroke(Stroke.None)
                        .AddColumnToTable()
                        .AddColumnToTable()
                        .AddRow()
                            .AddCell()
                                   .AddParagraph()
                                   .AddTextToParagraph("Page ")
                                   .AddPageNumberToParagraph()
                                   .AddTextToParagraph(" | ")
                                   .AddText(_options.BookName)
                                   .SetFont(exoBold)
                            .ToRow()
                                .AddCell()
                                   .SetHorizontalAlignment(HorizontalAlignment.Right)
                                   .AddParagraph()
                                        .AddTextToParagraph("Date Of Print ")
                                            .AddText(_options.DateOfPrint.Value.ToString(DateFormat))
                                                .SetFont(exoBold)
                                    .ToParagraph()
                                        .AddText(dateRangeStr1)
                                    .ToParagraph()
                                        .AddText(dateRangeStr2)
                                            .SetFont(exoBold);
            }
        }
        #endregion Methods
    }
}
