using Domain.Models.SpecialNote.PatientInfo;
using UseCase.SpecialNote.GetPtWeight;

namespace Interactor.SpecialNote
{
    public class GetPtWeightInteractor : IGetPtWeightInputPort
    {
        private readonly IPatientInfoRepository _patientInfoRepository;

        public GetPtWeightInteractor(IPatientInfoRepository patientInfoRepository)
        {
            _patientInfoRepository = patientInfoRepository;
        }

        public GetPtWeightOutputData Handle(GetPtWeightInputData inputData)
        {
            try
            {
                if (inputData.PtId <= 0)
                {
                    return new GetPtWeightOutputData(GetPtWeightStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetPtWeightOutputData(GetPtWeightStatus.InvalidSinDate);
                }

                var result = _patientInfoRepository.GetPtWeight(inputData.PtId, inputData.SinDate);

                return new GetPtWeightOutputData(result.HpId, result.PtId, result.IraiCd, result.SeqNo, result.IraiDate, result.RaiinNo, result.KensaItemCd, result.ResultVal, result.ResultType, result.AbnormalKbn, result.IsDeleted, result.CmtCd1, result.CmtCd2, result.UpdateDate, GetPtWeightStatus.Successed);
            }
            finally
            {
                _patientInfoRepository.ReleaseResource();
            }
        }
    }
}
