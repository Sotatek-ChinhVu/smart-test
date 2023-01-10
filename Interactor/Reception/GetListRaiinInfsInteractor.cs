using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetListRaiinInfs;

namespace Interactor.RaiinFilterMst;

public class GetListRaiinInfsInteractor : IGetListRaiinInfsInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;

    public GetListRaiinInfsInteractor(IReceptionRepository raiinInfRepository)
    {
        _raiinInfRepository = raiinInfRepository;
    }

    public GetListRaiinInfsOutputData Handle(GetListRaiinInfsInputData inputData)
    {
        try
        {
            var data = _raiinInfRepository.GetListRaiinInf(inputData.HpId, inputData.PtId);
            if (inputData.HpId < 0)
            {
                return new GetListRaiinInfsOutputData(GetListRaiinInfsStatus.InValidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetListRaiinInfsOutputData(GetListRaiinInfsStatus.InValidPtId);
            }

            

            return new GetListRaiinInfsOutputData(data.ToList(), GetListRaiinInfsStatus.Success);
        }
        catch
        {
            return new GetListRaiinInfsOutputData(GetListRaiinInfsStatus.Failed);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }
}