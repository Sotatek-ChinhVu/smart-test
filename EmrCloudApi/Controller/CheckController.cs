using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using EmrCloudApi.Constants;
using FindAndReplace;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase
{
    [HttpGet(ApiPath.DowloadDocumentTemplate)]
    public IActionResult ReplaceParamTemplate()
    {
        var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.docx";
        using (var client = new WebClient())
        {
            var content = client.DownloadData(link);
            using (var stream = new MemoryStream(content))
            {
                string inputFile = @"D:\SourceCode\SmartKarte_1\EmrCloudApi\FindAndReplace\Sample Input.docx";
                string outputFile = @"D:\SourceCode\SmartKarte_1\EmrCloudApi\FindAndReplace\Sample Output.docx";

                // Copy Word document.
                System.IO.File.Copy(inputFile, outputFile, true);

                // Open copied document.
                using (var flatDocument = new FlatDocument(outputFile))
                {
                    // Search and replace document's text content.
                    flatDocument.FindAndReplace("[TITLE]", "Lorem Ipsum");
                    flatDocument.FindAndReplace("[SUBTITLE]", "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet...");
                    flatDocument.FindAndReplace("[NAME]", "John Doe");
                    flatDocument.FindAndReplace("[EMAIL]", "john.doe@email.com");
                    flatDocument.FindAndReplace("[PHONE]", "(000)-111-222");
                    flatDocument.FindAndReplace("<<氏名>>", "(000)-111-222");
                    flatDocument.FindAndReplace("<<保険/保険者番号>>", "Check text jkalsfhjashjfkl保険/asfasf保険者番号");
                    flatDocument.FindAndReplace("<<保険/特記事項１>>", "check text 2 asg保険/保険者番号fasas保険/特記事項１asg");
                    // Save document on Dispose.
                }


                //using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
                //{
                //    var body = doc.MainDocumentPart.Document.Body;

                //    int check = 0;

                //    var paras = body.Elements<Paragraph>();
                //    var root = doc.MainDocumentPart;
                //    var root2 = doc.MainDocumentPart.RootElement.Descendants<Text>();

                //    var param = doc.MainDocumentPart.RootElement.Descendants<Text>().FirstOrDefault(c => c.Text.Contains("<<保険/特記事項１>>"));
                //    param = doc.MainDocumentPart.RootElement.Descendants<Text>().FirstOrDefault(c => c.Text.Contains("<<check>>"));

                //    foreach (var text in body.Descendants<Text>())
                //    {
                //        if (text.Text.Contains("<<保険/特記事項１>>"))
                //        {
                //            text.Text = text.Text.Replace("<<保険/特記事項１>>", "NewText");
                //        }
                //    }

                //    //var check = body.InnerText.FirstOrDefault(item => item.Contains("<<患者番号>>"));
                //    //var paras = body.Elements<Paragraph>();

                //    //foreach (var para in paras)
                //    //{
                //    //    foreach (var run in para.Elements<Run>())
                //    //    {
                //    //        foreach (var text in run.Elements<Text>())
                //    //        {
                //    //            if (text.Text.Contains("<<check>>"))
                //    //            {
                //    //                text.Text = text.Text.Replace("<<check>>", "replaced-text");
                //    //            }
                //    //            if (text.Text.Contains("<<患者番号>>"))
                //    //            {
                //    //                text.Text = text.Text.Replace("<<患者番号>>", "replaced-text");
                //    //            }
                //    //        }
                //    //    }
                //    //}
                //}
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.docx");
            }
        }
    }


    //public IActionResult ReplaceParamTemplate()
    //{
    //    var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.xlsx";
    //    using (var client = new WebClient())
    //    {
    //        var content = client.DownloadData(link);
    //        using (var stream = new MemoryStream(content))
    //        {
    //            var streamOutput = new MemoryStream();
    //            return File(streamOutput.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.xlsx");
    //        }
    //    }
    //}

    //[HttpGet(ApiPath.DowloadDocumentTemplate)]
    //public IActionResult ExportEmployee()
    //{
    //    //var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.docx";
    //    var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.xlsx";
    //    using (var client = new WebClient())
    //    {
    //        var content = client.DownloadData(link);
    //        using (var stream = new MemoryStream(content))
    //        {
    //            // Open the document for editing.
    //            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(stream, true))
    //            {
    //                // Get the SharedStringTablePart. If it does not exist, create a new one.
    //                SharedStringTablePart shareStringPart;
    //                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
    //                {
    //                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
    //                }
    //                else
    //                {
    //                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
    //                }

    //                // Insert the text into the SharedStringTablePart.
    //                int index = InsertSharedStringItem("check", shareStringPart);

    //                // Insert a new worksheet.
    //                WorksheetPart worksheetPart = InsertWorksheet(spreadSheet.WorkbookPart);

    //                // Insert cell A1 into the new worksheet.
    //                Cell cell = InsertCellInWorksheet("A", 1, worksheetPart);

    //                // Set the value of cell A1.
    //                cell.CellValue = new CellValue(index.ToString());
    //                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

    //                // Save the new worksheet.
    //                worksheetPart.Worksheet.Save();
    //            }
    //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.xlsx");
    //            //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.docx");
    //        }
    //    }
    //}
    //private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    //{
    //    Worksheet worksheet = worksheetPart.Worksheet;
    //    SheetData sheetData = worksheet.GetFirstChild<SheetData>();
    //    string cellReference = columnName + rowIndex;

    //    // If the worksheet does not contain a row with the specified row index, insert one.
    //    Row row;
    //    if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
    //    {
    //        row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
    //    }
    //    else
    //    {
    //        row = new Row() { RowIndex = rowIndex };
    //        sheetData.Append(row);
    //    }

    //    // If there is not a cell with the specified column name, insert one.  
    //    if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
    //    {
    //        return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
    //    }
    //    else
    //    {
    //        // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
    //        Cell refCell = null;
    //        foreach (Cell cell in row.Elements<Cell>())
    //        {
    //            if (cell.CellReference.Value.Length == cellReference.Length)
    //            {
    //                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
    //                {
    //                    refCell = cell;
    //                    break;
    //                }
    //            }
    //        }

    //        Cell newCell = new Cell() { CellReference = cellReference };
    //        row.InsertBefore(newCell, refCell);

    //        worksheet.Save();
    //        return newCell;
    //    }
    //}

    //private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
    //{
    //    // Add a new worksheet part to the workbook.
    //    WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
    //    newWorksheetPart.Worksheet = new Worksheet(new SheetData());
    //    newWorksheetPart.Worksheet.Save();

    //    Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
    //    string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

    //    // Get a unique ID for the new sheet.
    //    uint sheetId = 1;
    //    if (sheets.Elements<Sheet>().Count() > 0)
    //    {
    //        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
    //    }

    //    string sheetName = "Sheet" + sheetId;

    //    // Append the new worksheet and associate it with the workbook.
    //    Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
    //    sheets.Append(sheet);
    //    workbookPart.Workbook.Save();

    //    return newWorksheetPart;
    //}
    //private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
    //{
    //    // If the part does not contain a SharedStringTable, create one.
    //    if (shareStringPart.SharedStringTable == null)
    //    {
    //        shareStringPart.SharedStringTable = new SharedStringTable();
    //    }

    //    int i = 0;

    //    // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
    //    foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
    //    {
    //        if (item.InnerText == text)
    //        {
    //            return i;
    //        }

    //        i++;
    //    }

    //    // The text does not exist in the part. Create the SharedStringItem and return its index.
    //    shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
    //    shareStringPart.SharedStringTable.Save();

    //    return i;
    //}
}