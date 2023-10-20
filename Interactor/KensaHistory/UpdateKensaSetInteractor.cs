using Domain.Models.KensaSet;
using UseCase.KensaHistory.UpdateKensaSet;

namespace Interactor.KensaHistory
{
    public class UpdateKensaSetInteractor : IUpdateKensaSetInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;

        public UpdateKensaSetInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }

        public UpdateKensaSetOuputData Handle(UpdateKensaSetInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new UpdateKensaSetOuputData(false, UpdateKensaSetStatus.InValidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new UpdateKensaSetOuputData(false, UpdateKensaSetStatus.InValidUserId);
            }

            try
            {
                var data = _kensaSetRepository.UpdateKensaSet(inputData.HpId, inputData.UserId, inputData.SetId, inputData.SetName, inputData.SortNo, inputData.IsDeleted, inputData.KensaSetDetails);
                return new UpdateKensaSetOuputData(data, UpdateKensaSetStatus.Successed);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
