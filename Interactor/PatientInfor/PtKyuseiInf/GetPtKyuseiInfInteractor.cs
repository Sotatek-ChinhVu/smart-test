using Domain.Models.PatientInfor.PtKyuseiInf;
using UseCase.PatientInfor.PtKyuseiInf.GetList;

namespace Interactor.PatientInfor.PtKyuseiInf
{
    public class GetPtKyuseiInfInteractor : IGetPtKyuseiInfInputPort
    {
        private readonly IPtKyuseiInfRepository _ptKyuseiInfRepository;

        public GetPtKyuseiInfInteractor(IPtKyuseiInfRepository ptKyuseiInfRepository)
        {
            _ptKyuseiInfRepository = ptKyuseiInfRepository;
        }

        public GetPtKyuseiInfOutputData Handle(GetPtKyuseiInfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.InValidHpId);

                if (inputData.PtId <= 0)
                    return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.InValidPtId);

                var listPtKyuseiInf = _ptKyuseiInfRepository.PtKyuseiInfModels(inputData.HpId, inputData.PtId, inputData.IsDeleted);

                if (!listPtKyuseiInf.Any())
                    return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.NoData);

                return new GetPtKyuseiInfOutputData(listPtKyuseiInf, GetPtKyuseiInfStatus.Success);
            }
            catch (Exception)
            {
                return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.Failed);
            }
        }
    }
}
