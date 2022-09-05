using EmrCalculateApi.Constants;
using Infrastructure.Interfaces;
using UseCase.Schema.GetListImageTemplates;

namespace Interactor.Schema;

public class GetListImageTemplatesInteractor : IGetListImageTemplatesInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;

    public GetListImageTemplatesInteractor(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    public GetListImageTemplatesOutputData Handle(GetListImageTemplatesInputData inputData)
    {
        var listDatas = _amazonS3Service.GetListObjectAsync(SchemaConst.Schema);
        listDatas.Start();
        var datas = listDatas.Result;
        List<GetListImageTemplatesOutputItem> listFolders = new();
        return new GetListImageTemplatesOutputData(listFolders, GetListImageTemplatesStatus.Successed);
    }
}
