using Domain.Models.KensaIrai;
using Helper.Messaging;
using Helper.Messaging.Data;
using System.Text;
using UseCase.MainMenu.ImportKensaIrai;

namespace Interactor.MainMenu;

public class ImportKensaIraiInteractor : IImportKensaIraiInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private IMessenger? _messenger;

    public ImportKensaIraiInteractor(IKensaIraiRepository kensaIraiRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
    }

    public ImportKensaIraiOutputData Handle(ImportKensaIraiInputData inputData)
    {
        _messenger = inputData.Messenger;
        string contentString = string.Empty;
        try
        {
            try
            {
                var streamReader = new StreamReader(inputData.DatFile, Encoding.ASCII);
                contentString = streamReader.ReadToEnd();
            }
            catch
            {
                return new ImportKensaIraiOutputData(ImportKensaIraiStatus.InvalidInputFile);
            }
            _kensaIraiRepository.SaveKensaResultLog(inputData.HpId, inputData.UserId, contentString);
            var kensaInfDetailResult = GenKensaInfDetailList(inputData.HpId, contentString);
            if (kensaInfDetailResult.successed)
            {
                var result = _kensaIraiRepository.SaveKensaIraiImport(inputData.HpId, inputData.UserId, inputData.Messenger, kensaInfDetailResult.kensaInfDetailList);
                return new ImportKensaIraiOutputData(result, ImportKensaIraiStatus.Successed);
            }
            return new ImportKensaIraiOutputData(ImportKensaIraiStatus.Failed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
        }
    }

    private (List<KensaInfDetailModel> kensaInfDetailList, bool successed) GenKensaInfDetailList(int hpId, string contentString)
    {
        List<KensaInfDetailModel> result = new();
        List<string> contenItemList = contentString.Split("\r\n").ToList();
        bool successed = true;
        foreach (var item in contenItemList)
        {
            if (string.IsNullOrEmpty(item) || item.Length != 256)
            {
                continue;
            }
            string type = SubString(item, 1, 2);
            string centerCd = SubString(item, 3, 6);
            string iraiCdInput = SubString(item, 9, 20);
            long iraiCd = 0;
            long.TryParse(iraiCdInput, out iraiCd);
            string nyubi = SubString(item, 70, 3);
            string yoketu = SubString(item, 73, 3);
            string bilirubin = SubString(item, 76, 3);
            string kensaItemCd = SubString(item, 79, 5);
            string abnormalKbn = SubString(item, 84, 4);
            string resultVal = SubString(item, 96, 8);
            string resultType = SubString(item, 104, 1);
            string cmtCd1 = SubString(item, 105, 3);
            string cmtCd2 = SubString(item, 108, 3);
            result.Add(new KensaInfDetailModel(type, centerCd, iraiCd, nyubi, yoketu, bilirubin, kensaItemCd, abnormalKbn, resultVal, resultType, cmtCd1, cmtCd2));
        }
        var iraiCdList = result.Where(item => item.IraiCd > 0).Select(item => item.IraiCd).Distinct().ToList();
        var iraiCdNotExitList = _kensaIraiRepository.GetIraiCdNotExistList(hpId, iraiCdList);
        if (iraiCdNotExitList.Any())
        {
            StringBuilder iraiCdNotExistString = new();
            foreach (var item in iraiCdNotExitList)
            {
                if (string.IsNullOrEmpty(iraiCdNotExistString.ToString()))
                {
                    iraiCdNotExistString.Append(item);
                    continue;
                }
                iraiCdNotExistString.Append(", " + item);
            }
            SendMessager(new KensaInfMessageStatus(true, false, string.Empty, "登録のない依頼キーです。{" + iraiCdNotExistString.ToString() + "}"));
            successed = false;
        }
        return (result, successed);
    }

    private string SubString(string input, int startIndex, int endIndex)
    {
        return input.Substring(startIndex - 1, endIndex).Trim();
    }

    private void SendMessager(KensaInfMessageStatus status)
    {
        _messenger!.Send(status);
    }
}
