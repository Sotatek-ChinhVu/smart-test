using Domain.Constant;
using Domain.Models.ChartApproval;
using UseCase.ChartApproval.GetApprovalInfList;

namespace Interactor.ChartApproval
{
    public class GetApprovalInfListInteractor : IGetApprovalInfListInputPort
    {
        private readonly IApprovalInfRepository _approvalInfRepository;

        public GetApprovalInfListInteractor(IApprovalInfRepository approvalInfRepository) => _approvalInfRepository = approvalInfRepository;

        public GetApprovalInfListOutputData Handle(GetApprovalInfListInputData input)
        {
            try
            {
                if (input.StartDate < 0)
                {
                    string msg = string.Format(ErrorMessage.MessageType_mInp00110, "診察日(To)", "診察日(From)");
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidStartDate, msg, TypeMessage.TypeMessageError, new List<ApprovalInfModel>());
                }

                if (input.EndDate <= 0)
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidEndDate, string.Empty, TypeMessage.TypeMessageError, new List<ApprovalInfModel>());

                if (input.StartDate > input.EndDate)
                {
                    string msg = string.Format(ErrorMessage.MessageType_mInp00110, "診察日(To)", "診察日(From)");
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidStartDateMoreThanEndDate, msg, TypeMessage.TypeMessageError, new List<ApprovalInfModel>());
                }

                if ((input.EndDate - input.StartDate) > 100 && !input.ConfirmStartDateEndDate)
                {
                    string msg = string.Format(ErrorMessage.MessageType_mDo00050, "期間の指定が1ヶ月を超えている");
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.ConfirmStartDateEndDate, msg, TypeMessage.TypeMessageWarning, new List<ApprovalInfModel>());
                }

                if (input.KaId < 0)
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidKaId, string.Empty, TypeMessage.TypeMessageError, new List<ApprovalInfModel>());

                if (input.TantoId < 0)
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.InvalidTantoId, string.Empty, TypeMessage.TypeMessageError, new List<ApprovalInfModel>());

                var approvalInf = _approvalInfRepository.GetList(input.HpId, input.StartDate, input.EndDate, input.KaId, input.TantoId);

                if(approvalInf.Any())
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.Success, string.Empty, TypeMessage.TypeMessageSuccess, approvalInf);
                else
                    return new GetApprovalInfListOutputData(GetApprovalInfListStatus.NoData, string.Empty, TypeMessage.TypeMessageSuccess, approvalInf);
            }
            finally
            {
                _approvalInfRepository.ReleaseResource();
            }
        }
    }
}