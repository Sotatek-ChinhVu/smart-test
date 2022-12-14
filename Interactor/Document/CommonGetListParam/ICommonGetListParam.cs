using UseCase.Document;

namespace Interactor.Document.CommonGetListParam;

public interface ICommonGetListParam
{
    List<ItemDisplayParamModel> GetListParam(int hpId, int userId, long ptId, int sinDate, long raiinNo, int hokenPId);
}
