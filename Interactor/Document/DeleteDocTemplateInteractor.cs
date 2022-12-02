using Helper.Constants;
using Infrastructure.Interfaces;
using UseCase.Document.DeleteDocTemplate;

namespace Interactor.Document;

public class DeleteDocTemplateInteractor : IDeleteDocTemplateInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;

    public DeleteDocTemplateInteractor(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    public DeleteDocTemplateOutputData Handle(DeleteDocTemplateInputData inputData)
    {
        try
        {
            var listFolderPath = new List<string>(){
                                                   CommonConstants.Reference,
                                                   CommonConstants.Files,
                                                   inputData.CategoryCd.ToString()
                                                };
            string path = _amazonS3Service.GetFolderUploadOther(listFolderPath);
            var checkExist = _amazonS3Service.ObjectExistsAsync(path + inputData.FileTemplateName);
            checkExist.Wait();
            if (!checkExist.Result)
            {
                return new DeleteDocTemplateOutputData(DeleteDocTemplateStatus.TemplateNotFount);
            }
            var response = _amazonS3Service.DeleteObjectAsync(path + inputData.FileTemplateName);
            response.Wait();
            if (response.Result)
            {
                return new DeleteDocTemplateOutputData(DeleteDocTemplateStatus.Successed);
            }
            return new DeleteDocTemplateOutputData(DeleteDocTemplateStatus.Failed);
        }
        catch (Exception)
        {
            return new DeleteDocTemplateOutputData(DeleteDocTemplateStatus.Failed);
        }
    }
}
