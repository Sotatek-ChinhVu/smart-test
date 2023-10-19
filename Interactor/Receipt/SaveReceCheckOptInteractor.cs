using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using UseCase.Receipt.SaveReceCheckOpt;

namespace Interactor.Receipt;

public class SaveReceCheckOptInteractor : ISaveReceCheckOptInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public SaveReceCheckOptInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public SaveReceCheckOptOutputData Handle(SaveReceCheckOptInputData inputData)
    {
        try
        {
            var validateResult = ValidateInputData(inputData);
            if (validateResult != SaveReceCheckOptStatus.ValidateSuccess)
            {
                return new SaveReceCheckOptOutputData(validateResult);
            }
            var receCheckOptList = inputData.ReceCheckOptList.Select(item => new ReceCheckOptModel(
                                                                                item.ErrCd,
                                                                                item.ErrCd == "E2008" ? item.CheckOpt : 0,
                                                                                string.Empty,
                                                                                item.IsInvalid ? 0 : 1))
                                                             .ToList();
            _receiptRepository.SaveReceCheckOpt(inputData.HpId, inputData.UserId, receCheckOptList);
            return new SaveReceCheckOptOutputData(SaveReceCheckOptStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private SaveReceCheckOptStatus ValidateInputData(SaveReceCheckOptInputData inputData)
    {
        var errCdList = new List<string>
                                { "E1001", "E1003",
                                  "E2001", "E2003", "E2005", "E2006", "E2007", "E2008", "E2009",
                                  "E2010", "E2011", "E2012", "E2013",
                                  "E3001", "E3003", "E3004", "E3005", "E3007", "E3008",
                                  "E4001", "E4002", "E5013"
                                };
        if (inputData.ReceCheckOptList.Any(item => item.ErrCd == "E2008" && item.CheckOpt < 0))
        {
            return SaveReceCheckOptStatus.InvalidCheckOpt;
        }
        else if (inputData.ReceCheckOptList.Any(item => !errCdList.Contains(item.ErrCd)))
        {
            return SaveReceCheckOptStatus.InvalidErrCd;
        }
        return SaveReceCheckOptStatus.ValidateSuccess;
    }
}
