using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Gehtsoft.PDFFlow.LogBook.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Core.Utility;
using Gehtsoft.PDFFlow.Models.Content;
using Gehtsoft.PDFFlow.Models.Document;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

namespace Gehtsoft.PDFFlow.LogBook
{
    public class LogBookCoordinator : IStreamCoordinator
    {
        #region Fields
        private readonly DocumentBuilder _builder;
        private readonly PDFSettings _options;
        private readonly IStream _stream;
        private bool _disposed;
        private readonly bool _continuous;
        private Table _table;        
        private readonly string DateFormat = "MM/dd/yyyy";
        private readonly IEnumerable<Section> _sections;
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

                Input((IDictionary<string, object>)properties);
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
            var row = _table.AddRow();

            
            row.CellsStyle.SetBackColor(Color.White);
            row.SetBackColor(Color.White);

            row.AddTag(book);
            row.CellsStyle.SetMinHeight(0);

            // Received                              
            if (book.Discarded)
            {
                row.SetFont(Font.Exo(10));
                row.SetItalic();
                row.SetStrikethrough(_options.DiscardedLineDrawMode == LinethroughMode.FullRow, _options.DiscardedLineColor, _options.DiscardedLineStyle);
            }
            row.AddCell(book.Code).AddTag("id");
            row.AddCellToRow(book.Manufacturer).AddCellToRow(book.Model).AddCellToRow(book.VIN)
                .AddCell(book.Received.HasValue ? book.Received.Value.ToString(DateFormat) : "");
            row.AddCell(book.Source);
            
            // Sent
            row.AddCell(book.Code).AddTag("id");
            row.AddCell(book.Sent.HasValue ? book.Sent.Value.ToString(DateFormat) : "");                
            row.AddCell(book.Buyer_Name);              
            row.AddCell(book.FullAddress);
           
            
            if (book.Discarded && book.DiscardReason != null && book.DiscardReason.Length > 0)
            {
                row = _table.AddRow();                
                row.AddTag(book).SetFont(new Font { Name = FontNames.Exo, Size = 8, Italic = true, Color = Color.Gray })
                       .SetHorizontalAlignment(HorizontalAlignment.Left).SetBackColor(Color.FromRgba(1.0, 1.0, 0.9))
                       .AddCellToRow("")
                       .AddCellToRow(book.DiscardReason)
                       .AddCellToRow("")
                       .AddCellToRow("")
                       .AddCellToRow("")
                       .AddCellToRow("")
                       .AddCellToRow("")
                       .AddCellToRow("")
                       .AddCellToRow("")
                       .AddCellToRow("");
            }
        }

        private IEnumerable<Section> CreateSections()
        {
            var sections = new List<Section>();
            var s1 = new Section();
            s1.SetMargins(50).SetSize(() => _options.PaperSize).SetOrientation(() => _options.Orientation).SetNumerationStyle(NumerationStyle.Arabic)
                .AddParagraph("Operations Log").SetAlignment(HorizontalAlignment.Center).SetFontName(FontNames.Exo).SetFontSize(46).SetMargins(50, 0, 0, 0);
            s1.AddParagraph("Book").SetAlignment(HorizontalAlignment.Center).SetFontName(FontNames.Exo).SetFontSize(56).SetMargins(10, 0, 0, 0);            
            s1.AddParagraph("Powered by PDFFlow").SetFont(new Font{Name = FontNames.Exo,Size = 10,Color = Color.Black}).SetBold().SetAlignment(HorizontalAlignment.Right).Margins.Top = 100;            
            sections.Add(s1);

            var s2 = new Section();
            s2.SetMargins(_options.Padding).SetSize(() => _options.PaperSize).SetOrientation(() => _options.Orientation).SetNumerationStyle(NumerationStyle.Arabic).Page.PageNumberStart = _options.StartingPage - 1;

            AddLayouts(s2);

            _table = s2.AddTable(table =>
            {
                table.SetRepeatHeaders(true).RowLayout = _options.RowLayout;
                var titleFont = new Font
                {
                    Name = FontNames.Exo,
                    Size = 10,
                    Bold = true
                };
                table.ContentRowStyle.Font = titleFont.Clone().SetBold(false);
                table.ContentRowStyle.SetBackColor(Color.White);
                table.SetBackColor(Color.White);
                

                var headerStyle = table.HeaderRowStyle.Clone().SetFont(titleFont).SetBackColor(Color.FromRgba(0.8, 0.8, 0.8))
                .SetHorizontalAlignment(HorizontalAlignment.Left).SetBorderStyle(Stroke.None, Stroke.None);                     
                table.HeaderRowStyle = headerStyle;
                string id = "";
                int rowIndex = 0;
                void FormatRow(TableRow r, string tag)
                {
                    if (r.RowType != TableRowType.Title)
                    {
                        if (r.Tag is Operation book)
                        {
                            foreach (var cell in r.Cells.Where(x => x.Tag != null && x.Tag.ToString() == tag))
                            {
                                var index = r.Cells.IndexOf(cell);
                                var col = table.Columns[index];
                                cell.SetHorizontalAlignment(HorizontalAlignment.Center);                                
                                if (book.Code == id && r.Index != rowIndex)
                                {
                                    cell.Content.Clear();
                                    cell.Style.Border.Top.Style = Stroke.None;
                                }
                            }

                            rowIndex = r.Index;
                            id = book.Code;
                        }
                    }
                }
                table.RowAdded = row => { FormatRow(row, "id"); };                
                table.SetPartialBorders(false).SetRepeatHeaders(true).ContentRowStyle.SetBorderStyle(Stroke.None, Stroke.None).SetOverflowAction(_options.TextOverflowAction);
                table.AddTitleRow(r =>
                {
                    var style = r.CellsStyle.Clone().SetHorizontalAlignment(HorizontalAlignment.Left).SetVerticalAlignment(VerticalAlignment.Bottom).SetBackColor(Color.White)
                                                    .SetMinHeight(25)
                                                    .SetFont(new Font { Name = FontNames.Exo, Size = 13 });

                    style.Border = new Border();
                    style.SetBorderStyle(Stroke.None);
                    style.Border.Top.Style = Stroke.Solid;

                    r.AddCell("").SetStyle(style);

                    r.AddCell("Description of model").SetStyle(style);

                    r.AddCell("", 5).SetStyle(style);

                    r.AddCell("Reciept").SetStyle(style);

                    r.AddCell("", 3).SetStyle(style);
                });

                var headerRowStyle = table.HeaderRowStyle.Clone();                

                table.AddColumnPercent("#", 5).SetStyle(headerRowStyle);

                table.AddColumnPercent("Manufacturer", 24).SetStyle(headerRowStyle);               

                table.AddColumnPercent("Model", 16).SetStyle(headerRowStyle);

                table.AddColumnPercent("VIN", 24).SetStyle(headerRowStyle);
                                
                table.AddColumnPercent("Date Received", 16).SetStyle(headerRowStyle);

                table.AddColumnPercent("Source", 15).SetStyle(headerRowStyle);

                table.AddColumnPercent("#", 5).SetStyle(headerRowStyle);

                table.AddColumnPercent("Date Sent", 16).SetStyle(headerRowStyle);

                table.AddColumnPercent("Buyer Name", 30).SetStyle(headerRowStyle);

                table.AddColumnPercent("Buyer Address", 49).SetStyle(headerRowStyle);

            });
            sections.Add(s2);

            var s3 = new Section();
            string dateFormat = DateFormat;

            var font = new Font
            {
                Name = FontNames.Exo,
                Size = 11f
            };

            s3.SetMargins(60).SetSize(() => _options.PaperSize).SetOrientation(() => _options.Orientation).SetNumerationStyle(NumerationStyle.Arabic)
              .AddParagraph("Book Name").SetAlignment(HorizontalAlignment.Center).SetMargins(250, 0, 0, 0).SetFont(font);

            s3.AddParagraph(_options.BookName).SetAlignment(HorizontalAlignment.Center).SetFont(font).SetBold();
          
            s3.AddParagraph("Date of Print").SetAlignment(HorizontalAlignment.Center).SetMargins(10, 0, 0, 0).SetFont(font);            

            s3.AddParagraph(_options.DateOfPrint.Value.ToString(dateFormat)).SetAlignment(HorizontalAlignment.Center).SetFont(font).SetBold();

            s3.AddParagraph("Date range").SetAlignment(HorizontalAlignment.Center).SetMargins(10, 0, 0, 0).SetFont(font);

            s3.AddParagraph(string.Concat(_options.DateRangeStart.Value.ToString(dateFormat), " - ", _options.DateRangeEnd.Value.ToString(dateFormat))).SetAlignment(HorizontalAlignment.Center).SetFont(font).SetBold();

            s3.AddParagraph("Number of Records").SetAlignment(HorizontalAlignment.Center).SetMargins(10, 0, 0, 0).SetFont(font);

            s3.AddParagraph(_options.NumberOfRecords.ToString()).SetAlignment(HorizontalAlignment.Center).SetFont(font).SetBold();

            sections.Add(s3);
            return sections;
        }

        private void BuildSections()
        {
            foreach (Section section in _sections)
            {
                _builder.AddSection(section);
            }
        }

        private void AddLayouts(Section section)
        {
            AddHeader(true);  
            AddHeader(false); 
            AddFooter(true);  
            AddFooter(false);            

            void AddHeader(bool isOddPage)
            {
                string SentOrReceived = isOddPage ? "SENT" : "RECEIVED";
                string DescriptionOrReceipt = isOddPage ? "Receipt" : "Description of Model";
                string HeaderText = isOddPage ? _options.HeaderOdd : _options.HeaderEven;
                LayoutPage LayoutPage = isOddPage ? section.Layout.AddOddPage() : section.Layout.AddEvenPage();
                LayoutPage.AddRepeatingArea(section.Page, 50, areaConfig: area =>
                {
                    area.AddTable(table =>
                    {
                        table.SetBorderStyle(Stroke.None);
                        table.Border.SetWidth(0.0f);
                        
                        table.Width = 0;
                        table.AddRow(row =>
                        {
                            row.CellsStyle.Border = new Border();
                            row.CellsStyle.Border.SetWidth(0.0f);
                            row.SetBorderStyle(Stroke.None);
                            row.AddCell(SentOrReceived)                            
                            .SetHorizontalAlignment(HorizontalAlignment.Left).SetFont(Font.Exo(24));                        
                            row.AddCell(HeaderText).SetPadding(0, 0, 10, 0).SetHorizontalAlignment(HorizontalAlignment.Right)                            
                            .SetFont(Font.Exo(10));
                        });
                    });
                });
            }

            void AddFooter(bool isOddPage)
            {
                LayoutPage LayoutPage = isOddPage ? section.Layout.AddOddPage() : section.Layout.AddEvenPage();
                LayoutPage.AddRepeatingArea(section.Page, 30, true, areaConfig: area =>
                {
                    area.AddItem<Table>(table =>
                    {
                        table.SetBorderStyle(Stroke.None);
                        table.Border.SetWidth(0.0f);
                        table.ContentRowStyle.SetBorderStyle(Stroke.None).Border.SetWidth(0.0f);
                        table.AddRow(row =>
                        {
                            row.CellsStyle.Border = new Border();
                            row.CellsStyle.Border.SetWidth(0.0f);
                            row.SetBorderStyle(Stroke.None);                                                       
                            row.AddCell().SetFont(default)
                                .AddParagraph().SetFont(new Font { Name = FontNames.Exo, Size = 10 })
                                .AddTextToParagraph("Page ").AddPageNumber().AddTextToParagraph(" | ").AddText(_options.BookName).SetBold();

                            var parDate = row.AddCell().SetFont(default).SetHorizontalAlignment(HorizontalAlignment.Right).AddParagraph().SetFont(new Font { Name = FontNames.Exo, Size = 10 });
                            parDate.AddTextToParagraph("Date Of Print ").AddText(_options.DateOfPrint.Value.ToString("MM/dd/yyyy")).SetBold();
                            if (_options.DisplayDateRange && _options.DateRangeStart.HasValue && _options.DateRangeEnd.HasValue)
                            {
                                parDate.AddTextToParagraph(" | Date Range ").AddText($"{_options.DateRangeStart.Value:MM/dd/yyyy} - {_options.DateRangeEnd.Value:MM/dd/yyyy}").SetBold();
                            }                           
                        });
                    });

                });
            }            
        }
        #endregion Methods
    }
}