using Domain.Models.MedicalExamination;
using Domain.Models.MstItem;
using UseCase.UpdateKensaMst;
using static Helper.Constants.TenMstConst;

namespace Interactor.MedicalExamination
{
    public class UpdateKensaMstInteractor : IUpdateKensaMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public UpdateKensaMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public UpdateKensaMstOutputData Handle(UpdateKensaMstInputData inputData)
        {
            try
            {
                foreach (var data in inputData.TenMsts)
                {
                    var status = data.Validation();
                    if (status != ValidationStatus.Valid)
                    {
                        return new UpdateKensaMstOutputData(ConvertStatusTenMst(status));
                    }
                }

                if(_mstItemRepository.UpdateKensaMst(inputData.HpId, inputData.UserId, inputData.KensaMsts, inputData.TenMsts, inputData.ChildKensaMsts))
                {
                    return new UpdateKensaMstOutputData(UpdateKensaMstStatus.Success);
                }
                else
                {
                    return new UpdateKensaMstOutputData(UpdateKensaMstStatus.Failed);
                }
                
            }
            finally
            {
                _mstItemRepository.ReleaseResource(); 
            }
        }
        private static UpdateKensaMstStatus ConvertStatusTenMst(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidMasterSbt)
                return UpdateKensaMstStatus.InvalidMasterSbt;
            if (status == ValidationStatus.InvalidItemCd)
                return UpdateKensaMstStatus.InvalidItemCd;
            if (status == ValidationStatus.InvalidMinAge)
                return UpdateKensaMstStatus.InvalidMinAge;
            if (status == ValidationStatus.InvalidMaxAge)
                return UpdateKensaMstStatus.InvalidMaxAge;
            if (status == ValidationStatus.InvalidCdKbn)
                return UpdateKensaMstStatus.InvalidCdKbn;
            if (status == ValidationStatus.InvalidKokuji1)
                return UpdateKensaMstStatus.InvalidKokuji1;
            if (status == ValidationStatus.InvalidKokuji2)
                return UpdateKensaMstStatus.InvalidKokuji2;

            return UpdateKensaMstStatus.Success;
        }
    }
}
