using Interactor.Document.CommonGetListParam;
using UseCase.Document.GetListParamTemplate;

namespace Interactor.Document;

public class GetListParamTemplateInteractor : IGetListParamTemplateInputPort
{
    private readonly ICommonGetListParam _commonGetListParam;

    public GetListParamTemplateInteractor(ICommonGetListParam commonGetListParam)
    {
        _commonGetListParam = commonGetListParam;
    }

    public GetListParamTemplateOutputData Handle(GetListParamTemplateInputData inputData)
    {
        try
        {
            var result = _commonGetListParam.GetListParam(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.HokenPId);
            return new GetListParamTemplateOutputData(result, GetListParamTemplateStatus.Successed);
        }
        catch (Exception)
        {
            return new GetListParamTemplateOutputData(GetListParamTemplateStatus.Failed);
        }
    }
}
