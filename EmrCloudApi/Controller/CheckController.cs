using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EmrCloudApi.Constants;
using Interactor.Document.CommonGetListParam;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UseCase.Document;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase
{
    private readonly ICommonGetListParam _commonGetListParam;

    public CheckController(ICommonGetListParam commonGetListParam)
    {
        _commonGetListParam = commonGetListParam;
    }

    [HttpGet(ApiPath.DowloadDocumentTemplate)]
    public IActionResult ReplaceParamTemplate()
    {
        var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.xlsx";


        return File(ExportTemplateXlsx(link).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(link));

        //using (var client = new WebClient())
        //{
        //    var content = client.DownloadData(link);

        //    using (var streamOutput = new MemoryStream(content))
        //    {
        //        using (var workbook = SpreadsheetDocument.Open(streamOutput, true, new OpenSettings { AutoSave = true }))
        //        {
        //            // Replace shared strings
        //            if (workbook.WorkbookPart != null)
        //            {
        //                var sharedStringsPart = workbook.WorkbookPart.SharedStringTablePart;
        //                if (sharedStringsPart != null)
        //                {
        //                    var sharedStringTextElements = sharedStringsPart.SharedStringTable.Descendants<Text>();
        //                    foreach (var group in listGroupParams)
        //                    {
        //                        foreach (var param in group.ListParamModel)
        //                        {
        //                            DoReplaceFileXlsx(sharedStringTextElements, param);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return File(streamOutput.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Path.GetFileName(link));
        //    }
        //}
    }
    private MemoryStream ExportTemplateXlsx(string link)
    {
        var listGroupParams = _commonGetListParam.GetListParam(1, 2, 883, 20221214, 901140931, 10);
        using (var client = new WebClient())
        {
            var content = client.DownloadData(link);
            using (var streamOutput = new MemoryStream(content))
            {
                using (var workbook = SpreadsheetDocument.Open(streamOutput, true, new OpenSettings { AutoSave = true }))
                {
                    // Replace shared strings
                    if (workbook.WorkbookPart != null)
                    {
                        var sharedStringsPart = workbook.WorkbookPart.SharedStringTablePart;
                        if (sharedStringsPart != null)
                        {
                            var sharedStringTextElements = sharedStringsPart.SharedStringTable.Descendants<Text>();
                            foreach (var group in listGroupParams)
                            {
                                foreach (var param in group.ListParamModel)
                                {
                                    DoReplaceFileXlsx(sharedStringTextElements, param);
                                }
                            }
                        }
                    }
                    return streamOutput;
                }
            }
        }
    }

    private static void DoReplaceFileXlsx(IEnumerable<Text> textElements, ItemDisplayParamModel param)
    {
        var listData = textElements.ToList();
        for (int i = 0; i < listData.Count; i++)
        {
            if (i > 0)
            {
                if (listData[i].Text.Contains("<<" + param.Parameter + ">>"))
                {
                    listData[i].Text = listData[i].Text.Replace("<<", string.Empty);
                    listData[i].Text = listData[i].Text.Replace(">>", string.Empty);
                }
                if (listData[i - 1].Text.Contains("<<")
                && listData[i].Text.Contains(param.Parameter)
                && listData[i + 1].Text.Contains(">>"))
                {
                    listData[i - 1].Text = listData[i - 1].Text.Replace("<<", string.Empty);
                    listData[i + 1].Text = listData[i + 1].Text.Replace(">>", string.Empty);
                }
                if (listData[i].Text.Contains(param.Parameter + ">>")
                    && listData[i - 1].Text.Contains("<<"))
                {
                    listData[i - 1].Text = listData[i - 1].Text.Replace("<<", string.Empty);
                }
                if (listData[i].Text.Contains("<<" + param.Parameter)
                    && listData[i + 1].Text.Contains(">>"))
                {
                    listData[i + 1].Text = listData[i + 1].Text.Replace(">>", string.Empty);
                }
            }
            else
            {
                if (listData[i].Text.Contains("<<" + param.Parameter + ">>"))
                {
                    listData[i].Text = listData[i].Text.Replace("<<", string.Empty);
                    listData[i].Text = listData[i].Text.Replace(">>", string.Empty);
                }
                if (listData[i].Text.Contains("<<" + param.Parameter)
                    && listData[i + 1].Text.Contains(">>"))
                {
                    listData[i + 1].Text = listData[i + 1].Text.Replace(">>", string.Empty);
                }
            }
            listData[i].Text = listData[i].Text.Replace(param.Parameter, param.Value);
        }
    }
}