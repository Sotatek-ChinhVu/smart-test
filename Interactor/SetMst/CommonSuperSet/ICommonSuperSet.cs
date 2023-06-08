using Domain.Models.SetMst;
using UseCase.SetMst.GetList;

namespace Interactor.SetMst.CommonSuperSet;

public interface ICommonSuperSet
{
    List<GetSetMstListOutputItem> BuildTreeSetKbn(List<SetMstModel>? datas);
}
