using Domain.Models.Reception;
using UseCase.Reception.GetListRaiinInf;

namespace Interactor.Reception;

public class GetListRaiinInfInteractor : IGetListRaiinInfInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;

    public GetListRaiinInfInteractor(IReceptionRepository raiinInfRepository)
    {
        _raiinInfRepository = raiinInfRepository;
    }

    public GetListRaiinInfOutputData Handle(GetListRaiinInfInputData inputData)
    {
        try
        {
            if (inputData.HpId < 0)
            {
                return new GetListRaiinInfOutputData(GetListRaiinInfStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetListRaiinInfOutputData(GetListRaiinInfStatus.InvalidPtId);
            }
            if (inputData.PageIndex < 1)
            {
                return new GetListRaiinInfOutputData(GetListRaiinInfStatus.InvalidPageIndex);
            }
            if (inputData.PageSize < 0)
            {
                return new GetListRaiinInfOutputData(GetListRaiinInfStatus.InvalidPageSize);
            }

            var listRaiinInfos = GetListRaiinInfos(inputData.HpId, inputData.PtId, inputData.PageIndex, inputData.PageSize, inputData.IsDeleted, inputData.IsAll)
                                .Select(item => new GetListRaiinInfOutputItem(
                                                    item.HpId,
                                                    item.PtId,
                                                    item.SinDate,
                                                    item.UketukeNo,
                                                    item.Status,
                                                    item.KaSname,
                                                    item.SName,
                                                    item.Houbetu,
                                                    item.HokensyaNo,
                                                    item.HokenKbn,
                                                    item.HokenId,
                                                    item.HokenPid,
                                                    item.RaiinNo,
                                                    item.IsDeleted))
                                .ToList();
            return new GetListRaiinInfOutputData(listRaiinInfos, GetListRaiinInfStatus.Success);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }

    private List<GetListRaiinInfOutputItem> GetListRaiinInfos(int hpId, long ptId, int pageIndex, int pageSize, int isDeleted, bool isAll)
    {
        List<GetListRaiinInfOutputItem> result = new(_raiinInfRepository.GetListRaiinInf(hpId, ptId, pageIndex, pageSize, isDeleted, isAll)
                                                                        .Select(x => new GetListRaiinInfOutputItem(
                                                                               x.HpId,
                                                                               x.PtId,
                                                                               x.SinDate,
                                                                               x.UketukeNo,
                                                                               x.Status,
                                                                               x.KaSname,
                                                                               x.SName,
                                                                               x.Houbetu,
                                                                               x.HokensyaNo,
                                                                               x.HokenKbn,
                                                                               x.HokenId,
                                                                               x.HokenPid,
                                                                               x.RaiinNo,
                                                                               x.IsDeleted)));
        return result;
    }
}