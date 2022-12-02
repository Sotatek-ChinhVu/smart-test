using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Schema.GetListImageTemplates;

namespace Interactor.Schema;

public class GetListImageTemplatesInteractor : IGetListImageTemplatesInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;

    public GetListImageTemplatesInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
    }

    public GetListImageTemplatesOutputData Handle(GetListImageTemplatesInputData inputData)
    {
        List<GetListImageTemplatesOutputItem> listFolders = new();
        List<string> listImageItems = new();
        List<string> listFolderItems = new();
        var listFolderPath = new List<string>(){
                                                    CommonConstants.Reference,
                                                    CommonConstants.Schema
                                                };
        string path = _amazonS3Service.GetFolderUploadOther(listFolderPath);
        var response = _amazonS3Service.GetListObjectAsync(path);
        response.Wait();
        var listDatas = response.Result;
        foreach (var item in listDatas)
        {
            var start = item.IndexOf(path) + path.Length;
            var end = item.IndexOf("/", path.Length + 1);
            var imageName = _options.BaseAccessUrl + "/" + item;
            imageName = imageName.Replace(" ", "%20").Replace("+", "%2B");
            listImageItems.Add(imageName);

            if (end > start)
            {
                var folderName = item.Substring(start, end - start);
                if (!listFolderItems.Contains(folderName))
                {
                    listFolderItems.Add(folderName);
                }
            }
            else
            {
                var folderName = item.Substring(start);
                if (!listFolderItems.Contains(folderName))
                {
                    listFolderItems.Add(folderName);
                }
            }
        }

        foreach (var item in listFolderItems)
        {
            listFolders.Add(new GetListImageTemplatesOutputItem(item, listImageItems.Where(img => img.Contains(item)).ToList()));
        }

        return new GetListImageTemplatesOutputData(listFolders, GetListImageTemplatesStatus.Successed);
    }
}
