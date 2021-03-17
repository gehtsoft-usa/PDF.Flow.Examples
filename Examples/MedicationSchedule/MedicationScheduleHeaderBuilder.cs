using Gehtsoft.Barcodes.Enums;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using MedicationSchedule.Model;
using Newtonsoft.Json;
using System.IO;
using static MedicationSchedule.MedicationScheduleBuilder;

namespace MedicationSchedule
{
    internal class MedicationScheduleHeaderBuilder
    {
        internal readonly string logoPath = Path.Combine("images", "MedicationSchedule_Logo.png");

        PatientInfoData patientInfoData = JsonConvert.DeserializeObject<PatientInfoData>
            (File.ReadAllText(Path.Combine("Content", "patient-info.json")));
        HeaderInfoData headerInfoData = JsonConvert.DeserializeObject<HeaderInfoData>
            (File.ReadAllText(Path.Combine("Content", "header-info.json")));

        internal void Build(SectionBuilder sectionBuilder)
        {
            var table = sectionBuilder.AddTable()
                                          .SetBorderStroke(Stroke.None)
                                          .AddColumnToTable("", 89)
                                          .AddColumnToTable("", 355)
                                          .AddColumnToTable("", 11)
                                          .AddColumnToTable("", 264);

            var row1 = table.AddRow();
            row1.AddCell()
                    .SetPadding(0, 3, 0, 0)
                    .SetBorderStroke(Stroke.None)
                    .AddImage(logoPath)
                        .SetScale(ScalingMode.UserDefined)
                        .SetWidth(100);
            AddPatientInfoTable(row1.AddCell());

            row1.AddCellToRow();
            AddContactsTable(row1.AddCell());

            var row2 = table.AddRow();
            AddDateAndSignaturesTable(row2.AddCell());

            row2.AddCellToRow()
                .AddCellToRow();

            sectionBuilder.AddParagraph("* Best Possible Medication History")
                              .SetMarginTop(5)
                              .SetFont(FNT8)
                              .AddTextToParagraph("\n** Verification of steroids medication " +
                                            "that are part of the patients therapy treatment");
        }

        internal void AddPatientInfoTable(TableCellBuilder cell)
        {
            var paragraphStyle = StyleBuilder.New().SetFont(FNT12).SetLineSpacing(0.85f);

            cell.AddTable()
                    .SetBorderWidth(0, 0, 0, 0.2f)
                    .SetContentRowStyleFont(FNT8)
                    .AddColumnPercentToTable("", 40)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 30)
                    .AddRow()
                        .AddCell("Medication Reconcilliation/BPMH* for:")
                            .AddParagraph(patientInfoData.PatientName)
                                .ApplyStyle(paragraphStyle)
                    .ToRow()
                        .AddCell("Social Security Number")
                            .AddParagraph(patientInfoData.SSN)
                                .ApplyStyle(paragraphStyle)
                    .ToRow()
                        .AddCell("DOB")
                            .AddParagraph(patientInfoData.DOB)
                                .ApplyStyle(paragraphStyle)
                .ToTable()
                    .AddRow()
                        .AddCell("Two Week Period From:")
                            .SetPadding(0, 2, 0, 0)
                            .AddParagraph(patientInfoData.From)
                                .ApplyStyle(paragraphStyle)
                    .ToRow()
                        .AddCell("To:")
                            .SetPadding(0, 2, 0, 0)
                            .AddParagraph(patientInfoData.To)
                                .ApplyStyle(paragraphStyle)
                    .ToRow()
                        .AddCell()
                        .SetBorderStroke(Stroke.None);
        }

        internal void AddDateAndSignaturesTable(TableCellBuilder cell)
        {
            cell.SetColSpan(2)
                .AddTable()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetContentRowStyleFont(FNT8)
                    .AddColumnPercentToTable("", 20)
                    .AddColumnPercentToTable("", 42)
                    .AddColumnPercentToTable("", 38)
                    .AddRow()
                        .AddCell("Date (mm/dd/yyyy)")
                            .SetPadding(0, 3, 0, 22)
                    .ToRow()
                        .AddCell("Prepared by (Signature/Printed Name)")
                            .SetPadding(0, 3, 0, 22)
                    .ToRow()
                        .AddCell("Verified by PhC (Signature/Printed Name)")
                            .SetPadding(0, 3, 0, 22)
                .ToTable()
                    .AddRow()
                        .AddCell("Date (mm/dd/yyyy)")
                            .SetPadding(0, 2, 0, 22)
                    .ToRow()
                        .AddCell("Verified by RN (Signature/Printed Name)**")
                            .SetPadding(0, 2, 0, 22)
                    .ToRow()
                        .AddCell("Counselled by (Signature/Printed Name)")
                            .SetPadding(0, 2, 0, 22)
                .ToTable()
                    .AddRow()
                        .AddCell("Date (mm/dd/yyyy)")
                            .SetPadding(0, 2, 0, 22)
                    .ToRow()
                        .AddCell("Parent/Legal Guardian (Signature/Printed Name)")
                            .SetPadding(0, 2, 0, 22)
                    .ToRow()
                        .AddCell();
        }

        internal void AddContactsTable(TableCellBuilder cell)
        {
            TableBuilder contactsTable = cell
                .SetRowSpan(2)
                .SetBorderStroke(Stroke.Solid)
                .SetBorderWidth(1, 0, 0, 1)
                .AddTable()
                    .AddColumnPercentToTable("", 100f)
                    .AddColumnToTable("", 37f);
            contactsTable
                .AddRow()
                    .AddCell()
                        .AddTable()
                            .SetBorderStroke(Stroke.None)
                            .AddColumnPercentToTable("", 35)
                            .AddColumnPercentToTable("", 65)
                            .AddRow()
                                .AddCell()
                                    .SetColSpan(2)
                                    .SetPadding(8, 0, 0, 7)
                                    .AddParagraph("K00")
                                        .SetFont(FNT10B)
                        .ToTable()
                            .AddRow()
                                .AddCell()
                                    .SetColSpan(2)
                                    .SetPadding(4, 0, 0, 8)
                                    .AddParagraph("PLEASE NOTE:")
                                        .SetFont(FNT10B)
                                        .AddText(headerInfoData.Note)
                                            .SetFont(FNT10)
                        .ToTable()
                            .AddRow()
                                .SetFont(FNT10)
                                .AddCell()
                                    .SetPadding(4, 0, 0, 0)
                                    .AddParagraph("Information:")
                                        .SetLineSpacing(1)
                                .ToCell()
                                    .AddParagraph("Emergency:")
                                        .SetBold()
                                        .SetLineSpacing(1)
                                .ToCell()
                                    .AddParagraphToCell("Website:")
                            .ToRow()
                                .AddCell()
                                    .AddParagraph(headerInfoData.Information)
                                        .SetLineSpacing(1)
                                .ToCell()
                                    .AddParagraph(headerInfoData.Emergency)
                                        .SetLineSpacing(1)
                                        .SetBold()
                                .ToCell()
                                    .AddParagraph(headerInfoData.URL)
                        .ToTable()
                    .ToCell()
                .ToRow()
                    .AddCell()
                        .SetBorderWidth(0f, 0f, 0f, 1f)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .AddBarcode("(01)01234567890123", BarcodeType.GS1_128A, 225f, 0,
                            barcodeRotation: BarcodeRotation.Clockwise_90,
                            hasQuiteZones: false)
                            .SetPaddingBottom(10f);
        }
    }
}