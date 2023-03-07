using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using UseCase.Receipt.GetReceCheckOptionList;

namespace Interactor.Receipt;

public class GetReceCheckOptionListInteractor : IGetReceCheckOptionListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetReceCheckOptionListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetReceCheckOptionListOutputData Handle(GetReceCheckOptionListInputData inputData)
    {
        try
        {
            var errCdList = new List<string>
                                { "E1001", "E1003",
                                  "E2001", "E2003", "E2005", "E2006", "E2007", "E2008", "E2009",
                                  "E2010", "E2011", "E2012", "E2013",
                                  "E3001", "E3003", "E3004", "E3005", "E3007", "E3008",
                                  "E4001", "E4002", "E5013"
                                };
            var receCheckOptionList = _receiptRepository.GetReceCheckOptList(inputData.HpId, errCdList);
            return new GetReceCheckOptionListOutputData(ConvertToResult(receCheckOptionList), GetReceCheckOptionListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private Dictionary<string, ReceCheckOptModel> ConvertToResult(List<ReceCheckOptModel> receCheckOptionList)
    {
        Dictionary<string, ReceCheckOptModel> result = new();
        result.Add("HokenYukoKigenChekku", GetReceCheckOptModel("E1001", receCheckOptionList));
        result.Add("HokenMiKakuninChekku", GetReceCheckOptModel("E1003", receCheckOptionList));
        result.Add("ByomeiMiNyuryokuChekku", GetReceCheckOptModel("E2001", receCheckOptionList));
        result.Add("ByomeiShoshinryoToByomeiChekku", GetReceCheckOptModel("E2003", receCheckOptionList));
        result.Add("ByomeiShuByomeiChekku", GetReceCheckOptModel("E2005", receCheckOptionList));
        result.Add("ByomeiHaishiByomeiChekku", GetReceCheckOptModel("E2006", receCheckOptionList));
        result.Add("ByomeiMiKodoKaByomeiChekku", GetReceCheckOptModel("E2007", receCheckOptionList));
        result.Add("ByomeiUtagaiByomeiChekku", GetReceCheckOptModel("E2008", receCheckOptionList));
        result.Add("ByomeiTekioByomeiChekku", GetReceCheckOptModel("E2009", receCheckOptionList));
        result.Add("ByomeiBuiOrderByomeiChekku", GetReceCheckOptModel("E2010", receCheckOptionList));
        result.Add("ByomeiBuiMedicalByomeiChekku", GetReceCheckOptModel("E2011", receCheckOptionList));
        result.Add("DuplicatedByomeiCheckku", GetReceCheckOptModel("E2012", receCheckOptionList));
        result.Add("SyusyokuByomeiCheckku", GetReceCheckOptModel("E2013", receCheckOptionList));
        result.Add("ShinryoYukoKigenChekku", GetReceCheckOptModel("E3001", receCheckOptionList));
        result.Add("ShinryoShoshinryoChekku", GetReceCheckOptModel("E3003", receCheckOptionList));
        result.Add("ShinryoSanteiKaisuChekku", GetReceCheckOptModel("E3004", receCheckOptionList));
        result.Add("ShinryoMiKodoKaTokuZaiChekku", GetReceCheckOptModel("E3005", receCheckOptionList));
        result.Add("ShinryoNenreiChekku", GetReceCheckOptModel("E3007", receCheckOptionList));
        result.Add("KomentoChekku", GetReceCheckOptModel("E3008", receCheckOptionList));
        result.Add("AdditionItemChekku", GetReceCheckOptModel("E5013", receCheckOptionList));
        result.Add("ShohoToyoNissuChekku", GetReceCheckOptModel("E4001", receCheckOptionList));
        result.Add("ShohoDoshuDoKoChekku", GetReceCheckOptModel("E4002", receCheckOptionList));

        return result;
    }

    private ReceCheckOptModel GetReceCheckOptModel(string errCd, List<ReceCheckOptModel> receCheckOptionList)
    {
        var receCheckItem = receCheckOptionList.FirstOrDefault(item => item.ErrCd == errCd);
        if (receCheckItem != null)
        {
            return receCheckItem;
        }
        if (errCd == "E2008")
        {
            return new ReceCheckOptModel(errCd, 3);
        }
        return new ReceCheckOptModel(errCd);
    }
}
