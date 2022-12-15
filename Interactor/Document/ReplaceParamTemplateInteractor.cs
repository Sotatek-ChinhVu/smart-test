using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Interactor.Document.CommonGetListParam;
using Microsoft.Extensions.Options;
using System.Net;
using UseCase.Document.ReplaceParamTemplate;
using Text = DocumentFormat.OpenXml.Drawing.Text;

namespace Interactor.Document;

public class ReplaceParamTemplateInteractor : IReplaceParamTemplateInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;
    private readonly ICommonGetListParam _commonGetListParam;

    public ReplaceParamTemplateInteractor(IAmazonS3Service amazonS3Service, IOptions<AmazonS3Options> optionsAccessor, ICommonGetListParam commonGetListParam)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _commonGetListParam = commonGetListParam;
    }

    public ReplaceParamTemplateOutputData Handle(ReplaceParamTemplateInputData inputData)
    {
        try
        {
            if (!_amazonS3Service.ObjectExistsAsync(inputData.LinkFile.Replace(_options.BaseAccessUrl + "/", string.Empty)).Result)
            {
                return new ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus.TemplateLinkIsNotExists);
            }
            var listGroups = _commonGetListParam.GetListParam(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.HokenPId);

            using (WebClient client = new WebClient())
            {
                var content = client.DownloadData(inputData.LinkFile);
                using (var stream = new MemoryStream(content))
                {
                    using (var word = WordprocessingDocument.Open(stream, true))
                    {
                        if (word.MainDocumentPart != null && word.MainDocumentPart.Document.Body != null)
                        {
                            var element = word.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                                                .FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == "<<氏名>>");

                            element.Descendants<Text>().First().Text = "hello_vuquynh_anh患者番号";
                            element.Descendants<Text>().Skip(1).ToList().ForEach(t => t.Remove());


                            //var element = word.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                            //                .FirstOrDefault(sdt => sdt.SdtProperties != null && sdt.SdtProperties.GetFirstChild<Tag>()?.Val == "<<氏名>>");
                            //if (element != null)
                            //{
                            //    element.Descendants<Text>().First().Text = "hello_vuquynh_anh患者番号";
                            //    element.Descendants<Text>().Skip(1).ToList().ForEach(t => t.Remove());
                            //}
                        }
                        return new ReplaceParamTemplateOutputData(stream, ReplaceParamTemplateStatus.Successed);

                        //if (word.MainDocumentPart != null && word.MainDocumentPart.RootElement != null)
                        //{

                        //    foreach (var group in listGroups)
                        //    {
                        //        foreach (var param in group.ListParamModel)
                        //        {

                        //            //var name = word.MainDocumentPart.RootElement.Descendants<Text>().FirstOrDefault(c => c.Text.Contains(param.Parameter));
                        //            //if (name != null)
                        //            //{
                        //            //    name.Text = param.Value;
                        //            //}
                        //        }
                        //    }
                        //}
                    }
                }
            }
        }
        catch (Exception)
        {
            return new ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus.Failed);
        }
    }
}
