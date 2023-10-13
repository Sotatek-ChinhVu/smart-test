using Domain.Models.KensaIrai;
using System.Text;
using UseCase.MainMenu.CreateDataKensaIraiRenkei;
using UseCase.MainMenu.ImportKensaIrai;

namespace Interactor.MainMenu;

public class ImportKensaIraiInteractor : IImportKensaIraiInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;

    public ImportKensaIraiInteractor(IKensaIraiRepository kensaIraiRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
    }

    public ImportKensaIraiOutputData Handle(ImportKensaIraiInputData inputData)
    {
        string contentString = string.Empty;
        try
        {
            var streamReader = new StreamReader(inputData.DatFile, Encoding.ASCII);
            contentString = streamReader.ReadToEnd();
        }
        catch
        {
            return new ImportKensaIraiOutputData(ImportKensaIraiStatus.InvalidInputFile);
        }
        var kensaInfDetailList = GenKensaInfDetailList(contentString);
        var resultValidate = ValidateInputData(inputData.HpId, kensaInfDetailList);
        if (resultValidate != ImportKensaIraiStatus.ValidateSuccessed)
        {
            return new ImportKensaIraiOutputData(resultValidate);
        }
        //var result = _kensaIraiRepository.SaveKensaIraiImport(inputData.HpId, inputData.UserId, inputData.Messenger, kensaInfDetailList);
        List<KensaInfMessageModel> result = new();
        return new ImportKensaIraiOutputData(result, ImportKensaIraiStatus.Successed);
    }

    private ImportKensaIraiStatus ValidateInputData(int hpId, List<KensaInfDetailModel> kensaInfDetailList)
    {
        var centerCdList = kensaInfDetailList.Select(item => item.CenterCd).Distinct().ToList();
        var iraiCdList = kensaInfDetailList.Select(item => item.IraiCd).Distinct().ToList();
        if (kensaInfDetailList.Any(item => string.IsNullOrEmpty(item.CenterCd)) || !_kensaIraiRepository.CheckExistCenterCd(hpId, centerCdList))
        {
            return ImportKensaIraiStatus.InvalidCenterCd;
        }
        else if (kensaInfDetailList.Any(item => item.IraiCd == 0 || !_kensaIraiRepository.CheckExistIraiCd(hpId, iraiCdList)))
        {
            return ImportKensaIraiStatus.InvalidIraiCd;
        }
        else if (kensaInfDetailList.Any(item => string.IsNullOrEmpty(item.ResultType) && !new List<string> { "E", "U", "L", "B" }.Contains(item.ResultType)))
        {
            return ImportKensaIraiStatus.InvalidResultType;
        }
        return ImportKensaIraiStatus.ValidateSuccessed;
    }

    private List<KensaInfDetailModel> GenKensaInfDetailList(string contentString)
    {
        List<KensaInfDetailModel> result = new();
        List<string> contenItemList = contentString.Split("\r\n").ToList();
        foreach (var item in contenItemList)
        {
            if (string.IsNullOrEmpty(item) && item.Length != 256)
            {
                continue;
            }
            string centerCd = SubString(item, 3, 6);
            long iraiCd = long.Parse(SubString(item, 9, 20));
            string nyubi = SubString(item, 70, 3);
            string yoketu = SubString(item, 73, 3);
            string bilirubin = SubString(item, 76, 3);
            string kensaItemCd = SubString(item, 79, 5);
            string abnormalKbn = SubString(item, 84, 4);
            string resultVal = SubString(item, 96, 8);
            string resultType = SubString(item, 104, 1);
            string cmtCd1 = SubString(item, 105, 3);
            string cmtCd2 = SubString(item, 108, 3);
            result.Add(new KensaInfDetailModel(centerCd, iraiCd, nyubi, yoketu, bilirubin, kensaItemCd, abnormalKbn, resultVal, resultType, cmtCd1, cmtCd2));
        }
        return result;
    }

    private string SubString(string input, int startIndex, int endIndex)
    {
        return input.Substring(startIndex - 1, endIndex).Trim();
    }
}
