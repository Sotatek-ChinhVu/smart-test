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
            var ptDiseaseListModel = _diseaseRepository.GetAllByomeiByPtId(inputData.HpId,
                                                                           inputData.PtId,
                                                                           inputData.PageIndex,
                                                                           inputData.PageSize);
            return new GetAllByomeiByPtIdOutputData(ptDiseaseListModel, GetAllByomeiByPtIdStatus.Success);
        }
        finally
        {
            _diseaseRepository.ReleaseResource();
        }
    }
}
