using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Document;
using Interactor.Document.CommonGetListParam;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class DownloadDocTemplateController : ControllerBase
{
    private readonly ICommonGetListParam _commonGetListParam;

    public DownloadDocTemplateController(ICommonGetListParam commonGetListParam)
    {
        _commonGetListParam = commonGetListParam;
    }

    [HttpGet(ApiPath.ReplaceParamTemplate)]
    public IActionResult ExportEmployee([FromQuery] ReplaceParamTemplateRequest inputData)
    {
        using (var client = new WebClient())
        {
            var content = client.DownloadData(inputData.LinkFile);
            using (var stream = new MemoryStream(content))
            {
                using (var word = WordprocessingDocument.Open(stream, true))
                {
                    if (word.MainDocumentPart != null && word.MainDocumentPart.Document.Body != null)
                    {
                        var listGroups = _commonGetListParam.GetListParam(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.HokenPId);
                        foreach (var group in listGroups)
                        {
                            foreach (var param in group.ListParamModel)
                            {
                                var element = word.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                                                 .FirstOrDefault(sdt => sdt.SdtProperties != null && sdt.SdtProperties.GetFirstChild<Tag>()?.Val == "<<" + param.Parameter + ">>");
                                if (element != null)
                                {
                                    element.Descendants<Text>().First().Text = param.Value;
                                    element.Descendants<Text>().Skip(1).ToList().ForEach(t => t.Remove());
                                }
                            }
                        }
                    }
                }
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.docx");
            }
        }
    }
}