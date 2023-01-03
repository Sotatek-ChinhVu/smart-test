using FindAndReplace;
using Interactor.Document.CommonGetListParam;
using UseCase.Document;
using UseCase.Document.DownloadDocumentTemplate;

namespace Interactor.Document;

public class DownloadDocumentTemplateInteractor : IDownloadDocumentTemplateInputPort
{
    private readonly ICommonGetListParam _commonGetListParam;

    public DownloadDocumentTemplateInteractor(ICommonGetListParam commonGetListParam)
    {
        _commonGetListParam = commonGetListParam;
    }

    public DownloadDocumentTemplateOutputData Handle(DownloadDocumentTemplateInputData inputData)
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
                        return new DownloadDocumentTemplateOutputData(ReplaceParamsFileDocx(stream, listGroupParams, inputData.ListReplaceComments), DownloadDocumentTemplateStatus.Successed);
                    }
                }
                return new DownloadDocumentTemplateOutputData(DownloadDocumentTemplateStatus.Failed);
            }
        }
    }

    private MemoryStream ReplaceParamsFileDocx(MemoryStream streamOutput, List<ItemGroupParamModel> listGroupParams, List<ReplaceCommentInputItem> listReplaceComments)
    {
        var flatDocument = new FlatDocument(streamOutput);
        foreach (var group in listGroupParams)
        {
            foreach (var param in group.ListParamModel)
            {
                flatDocument.FindAndReplace("《" + param.Parameter + "》", param.Value);
            }
        }
        foreach (var comment in listReplaceComments)
        {
            flatDocument.FindAndReplace("@" + comment.ReplaceKey + "@", comment.ReplaceValue);
        }
        flatDocument.Close();
        return streamOutput;
    }
}
