using Domain.Models.Reception;
using UseCase.Reception;
using UseCase.Reception.GetListRaiinInfs;

namespace Interactor.RaiinFilterMst;

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
            var data = _raiinInfRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.PageIndex, inputData.PageSize);

            if (inputData.HpId < 0)
            {
                return new GetListRaiinInfOutputData(new(), GetListRaiinInfStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetListRaiinInfOutputData(new(), GetListRaiinInfStatus.InvalidPtId);
            }
            if(inputData.PageIndex < 1)
            {
                return new GetListRaiinInfOutputData(new(), GetListRaiinInfStatus.InvalidPageIndex);
            }    
            if(inputData.PageSize < 0)
            {
                return new GetListRaiinInfOutputData(new(), GetListRaiinInfStatus.InvalidPageSize);
            }

            var listRaiinInfos = GetListRaiinInfos(inputData.HpId, inputData.PtId, inputData.PageIndex, inputData.PageSize).Select(item => new ReceptionGetDto(
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
                                                            item.RaiinNo))
                                                            .ToList();
            return new GetListRaiinInfOutputData(listRaiinInfos, GetListRaiinInfStatus.Success);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }

    private List<ReceptionModel> GetListRaiinInfos(int hpId, long ptId, int pageIndex, int pageSize)
    {
        List<ReceptionModel> result = new(_raiinInfRepository.GetListRaiinInf(hpId, ptId, pageIndex, pageSize).Select(x => new ReceptionModel(
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
                                                      x.RaiinNo)));
        return result;
    }
}