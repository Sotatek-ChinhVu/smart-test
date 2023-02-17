using Domain.Models.Receipt;
using System.Text;
using UseCase.Receipt.GetReceHenReason;

namespace Interactor.Receipt;

public class GetReceHenReasonInteractor : IGetReceHenReasonInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetReceHenReasonInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetReceHenReasonOutputData Handle(GetReceHenReasonInputData inputData)
    {
        try
        {
            StringBuilder receReasonCmt = new();
            var receReasonList = _receiptRepository.GetReceReasonList(inputData.HpId, inputData.SeikyuYm, inputData.SinDate, inputData.PtId, inputData.HokenId);
            foreach (var reason in receReasonList)
            {
                if (!string.IsNullOrEmpty(reason.HenreiJiyuu) && !string.IsNullOrEmpty(reason.Hosoku))
                {
                    receReasonCmt.AppendLine(reason.HenreiJiyuu + " (" + reason.Hosoku + ")");
                }
                else if (!string.IsNullOrEmpty(reason.HenreiJiyuu))
                {
                    receReasonCmt.AppendLine(reason.HenreiJiyuu);
                }
            }
            return new GetReceHenReasonOutputData(GetReceHenReasonStatus.Successed, receReasonCmt.ToString());
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }
}
