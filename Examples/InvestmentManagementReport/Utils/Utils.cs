using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentManagementReport.Utils
{
    internal static class Utils
    {
        internal static ParagraphBuilder SetHelveticaOfSize(this ParagraphBuilder paragraphBuilder, string fontSize, bool isBold = false)
        {
            return paragraphBuilder
                .SetFont(Fonts.Helvetica(float.Parse(fontSize)))
                .SetBold(isBold);
        }

        internal static ParagraphBuilder SetHelveticaOfSize(this ParagraphBuilder paragraphBuilder, float fontSize, bool isBold = false)
        {
            return paragraphBuilder
                .SetFont(Fonts.Helvetica(fontSize))
                .SetBold(isBold);
        }

        internal static TableCellBuilder SetHelveticaOfSize(this TableCellBuilder cellBuilder, string fontSize, bool isBold = false)
        {
            return cellBuilder
                .SetFont(Fonts.Helvetica(float.Parse(fontSize)))
                .SetBold(isBold);
        }

        internal static TableCellBuilder SetHelveticaOfSize(this TableCellBuilder cellBuilder, float fontSize, bool isBold = false)
        {
            return cellBuilder
                .SetFont(Fonts.Helvetica(fontSize))
                .SetBold(isBold);
        }
    }
}
