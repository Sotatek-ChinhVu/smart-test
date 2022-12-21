using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using FindAndReplace;
using Interactor.Document.CommonGetListParam;
using System.Net;
using UseCase.Document;
using UseCase.Document.DowloadDocumentTemplate;

namespace Interactor.Document;

public class DowloadDocumentTemplateInteractor : IDowloadDocumentTemplateInputPort
{
    private readonly ICommonGetListParam _commonGetListParam;

    public DowloadDocumentTemplateInteractor(ICommonGetListParam commonGetListParam)
    {
        _commonGetListParam = commonGetListParam;
    }

    public DowloadDocumentTemplateOutputData Handle(DowloadDocumentTemplateInputData inputData)
    {
        try
        {
            var extension = System.IO.Path.GetExtension(inputData.LinkFile).ToLower();
            using (var httpClient = new HttpClient())
            {
                var responseStream = httpClient.GetStreamAsync(inputData.LinkFile).Result;
                using (var stream = new MemoryStream())
                {
                    responseStream.CopyTo(stream);
                    if (stream.Length > 0)
                    {
                        var listGroupParams = _commonGetListParam.GetListParam(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.HokenPId);
                        if (extension.Equals(".docx"))
                        {
                            return new DowloadDocumentTemplateOutputData(ReplaceParamsFileDocx(stream, listGroupParams), DowloadDocumentTemplateStatus.Successed);
                        }
                    }
                    return new DowloadDocumentTemplateOutputData(DowloadDocumentTemplateStatus.Failed);
                }
            }
        }
        catch (Exception)
        {
            return new DowloadDocumentTemplateOutputData(DowloadDocumentTemplateStatus.Failed);
        }
    }

    private MemoryStream ReplaceParamsFileDocx(MemoryStream streamOutput, List<ItemGroupParamModel> listGroupParams)
    {
        var flatDocument = new FlatDocument(streamOutput);
        foreach (var group in listGroupParams)
        {
            foreach (var param in group.ListParamModel)
            {
                flatDocument.FindAndReplace("<<" + param.Parameter + ">>", param.Value);
            }
        }
        flatDocument.Close();
        return streamOutput;
    }
}
