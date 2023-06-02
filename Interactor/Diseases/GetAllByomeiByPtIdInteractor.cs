using Domain.Models.Diseases;
using UseCase.Diseases.GetAllByomeiByPtId;

namespace Interactor.Diseases;

public class GetAllByomeiByPtIdInteractor : IGetAllByomeiByPtIdInputPort
{
    private readonly IPtDiseaseRepository _diseaseRepository;
    public GetAllByomeiByPtIdInteractor(IPtDiseaseRepository diseaseRepository)
    {
        _diseaseRepository = diseaseRepository;
    }

    public GetAllByomeiByPtIdOutputData Handle(GetAllByomeiByPtIdInputData inputData)
    {
        try
        {
            int pageIndex = inputData.PageIndex <= 0 ? 1 : inputData.PageIndex;
            int pageSize = inputData.PageSize <= 0 ? 1 : inputData.PageSize;
            var ptDiseaseListModel = _diseaseRepository.GetAllByomeiByPtId(inputData.HpId,
                                                                           inputData.PtId,
                                                                           pageIndex,
                                                                           pageSize);
            return new GetAllByomeiByPtIdOutputData(ptDiseaseListModel, GetAllByomeiByPtIdStatus.Success);
        }
        finally
        {
            _diseaseRepository.ReleaseResource();
        }
    }
}
