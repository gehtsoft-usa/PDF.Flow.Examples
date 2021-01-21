using BankAccountStatement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;
using System.Collections.Generic;
using System.Globalization;

namespace BankAccountStatement
{
    internal class BankAccountStatementBuilder
    { 
        internal static readonly CultureInfo DocumentLocale  = new CultureInfo("en-US");
        internal const PageOrientation Orientation = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));
        internal static readonly FontBuilder FNT4 = Fonts.Helvetica(4f);
        internal static readonly FontBuilder FNT5 = Fonts.Helvetica(5f);
        internal static readonly FontBuilder FNT5_6 = Fonts.Helvetica(5.6f);
        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT7B = Fonts.Helvetica(7f).SetBold();
        internal static readonly FontBuilder FNT7_2 = Fonts.Helvetica(7.2f);
        internal static readonly FontBuilder FNT7_2B = Fonts.Helvetica(7.2f).SetBold();
        internal static readonly FontBuilder FNT7_2URL = Fonts.Helvetica(7.2f)
            .SetUnderlineStroke(Stroke.Solid).SetUnderlineColor(Color.Blue);
        internal static readonly FontBuilder FNT7_5 = Fonts.Helvetica(7.5f);
        internal static readonly FontBuilder FNT7_5URL = Fonts.Helvetica(7.5f)
            .SetUnderlineStroke(Stroke.Solid).SetUnderlineColor(Color.Blue);
        internal static readonly FontBuilder FNT7_9 = Fonts.Helvetica(7.9f);
        internal static readonly FontBuilder FNT7_9B = Fonts.Helvetica(7.9f).SetBold();
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT8_9 = Fonts.Helvetica(8.9f);
        internal static readonly FontBuilder FNT9B = Fonts.Helvetica(9f).SetBold();
        internal static readonly FontBuilder FNT9_8B = Fonts.Helvetica(9.8f).SetBold();
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT10_5B = Fonts.Helvetica(10.5f).SetBold();
        internal static readonly FontBuilder FNT11_B = Fonts.Helvetica(11f).SetBold();
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT13B = Fonts.Helvetica(13f).SetBold();
        internal static readonly FontBuilder FNT16 = Fonts.Helvetica(16f);
        internal static readonly FontBuilder FNT18_3B = Fonts.Helvetica(18.3f).SetBold();
        internal static readonly FontBuilder FNTZ8 = FontBuilder.New()
            .SetName(FontNames.ZapfDingbats).SetSize(8);

        private readonly StatementInfo statementInfo;
        private readonly List<Statement> statements;

        public BankAccountStatementBuilder(StatementInfo statementInfo, List<Statement> statements)
        {
            this.statementInfo = statementInfo;
            this.statements = statements;
        }

        internal DocumentBuilder Build()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            new BankAccountStatementFirstPageBuilder(statementInfo)
                .Build(documentBuilder);
            new BankAccountStatementSecondPageBuilder(statementInfo, statements)
                .Build(documentBuilder);
            new BankAccountStatementLastPageBuilder(statementInfo)
                .Build(documentBuilder);
            return documentBuilder;
        }
    }
}