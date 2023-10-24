using Domain.Models.KensaSet;
using UseCase.KensaHistory.UpdateKensaInfDetail;
using UseCase.KensaHistory.UpdateKensaSet;

namespace Interactor.KensaHistory
{
    public class UpdateKensaInfDetailInteractor : IUpdateKensaInfDetailInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;

        public UpdateKensaInfDetailInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }

        public UpdateKensaInfDetailOutputData Handle(UpdateKensaInfDetailInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new UpdateKensaInfDetailOutputData(false, UpdateKensaInfDetailStatus.InValidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new UpdateKensaInfDetailOutputData(false, UpdateKensaInfDetailStatus.InValidUserId);
            }

            if (inputData.KensaInfDetails.Count <= 0)
            {
                return new UpdateKensaInfDetailOutputData(false, UpdateKensaInfDetailStatus.InvalidDataUpdate);
            }
            try
            {
                var data = _kensaSetRepository.UpdateKensaInfDetail(inputData.HpId, inputData.UserId, inputData.PtId, inputData.IraiCd, inputData.IraiDate, inputData.KensaInfDetails);
                return new UpdateKensaInfDetailOutputData(data, UpdateKensaInfDetailStatus.Successed);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
