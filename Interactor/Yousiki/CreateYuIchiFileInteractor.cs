using CalculateService.Interface;
using CalculateService.Receipt.ViewModels;
using Domain.Models.HpInf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.Yousiki;
using Helper.Common;
using Helper.Extension;
using Helper.Messaging;
using Infrastructure.Interfaces;
using System.Text;
using System.Text.RegularExpressions;
using UseCase.Yousiki.CreateYuIchiFile;
using CreateYuIchiFileProgressStatus = Helper.Messaging.Data.CreateYuIchiFileStatus;
using CreateYuIchiFileStatus = UseCase.Yousiki.CreateYuIchiFile.CreateYuIchiFileStatus;

namespace Interactor.Yousiki;

public class CreateYuIchiFileInteractor : ICreateYuIchiFileInputPort
{
    private readonly IYousikiRepository _yousikiRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;
    private const string mInp00010 = "mInp00010";
    private const string mFree00030 = "mFree00030";
    private const string mFree00040 = "mFree00040";
    private const string confirmMessage = "confirmMessage";
    private const string progress = "progress";
    private const string fileContent = "fileContent";
    public const string InvalidCreateYuIchiFileSinYm = "受診年月を入力してください。";
    public const string InvalidCreateYuIchiFileCheckedOption = "出力したいファイルにチェックを付けてください。";
    public const string CreateYuIchiFileSuccessed = "ファイルを保存しました。";
    public const string CreateYuIchiFileFailed = "出力対象が見つかりません。";
    private IMessenger? _messenger;
    private readonly ITenantProvider _tenantProvider;
    private readonly ISystemConfigProvider _systemConfigProvider;
    private readonly IEmrLogger _emrLogger;

    public CreateYuIchiFileInteractor(IYousikiRepository yousikiRepository, IHpInfRepository hpInfRepository, ISystemGenerationConfRepository systemGenerationConfRepository, ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
    {
        _yousikiRepository = yousikiRepository;
        _hpInfRepository = hpInfRepository;
        _systemGenerationConfRepository = systemGenerationConfRepository;
        _tenantProvider = tenantProvider;
        _systemConfigProvider = systemConfigProvider;
        _emrLogger = emrLogger;
    }

    public CreateYuIchiFileOutputData Handle(CreateYuIchiFileInputData inputData)
    {
        _messenger = inputData.Messenger;
        try
        {
            bool isExported = false;
            var validateData = ValidateData(inputData);
            if (validateData.Status != CreateYuIchiFileStatus.ValidateSuccessed)
            {
                // send invalid message progress to FE
                SendMessager(true, validateData.MessageType, GetMessage(validateData.Status));
                return validateData;
            }
            var yousiki1InfList = _yousikiRepository.GetListYousiki1Inf(inputData.HpId, inputData.SinYm);

            if (yousiki1InfList.Any() && !inputData.ReactCreateYuIchiFile.ConfirmPatientList)
            {
                var unregistedPatients = yousiki1InfList.FindAll(p => p.Status == 0 || p.Status == 1)
                                                        .GroupBy(item => item.PtId).Select(item => item.FirstOrDefault())
                                                        .ToList();
                if (unregistedPatients.Count > 0)
                {
                    StringBuilder patientStringBuilder = new();
                    foreach (var unregistedPatient in unregistedPatients)
                    {
                        patientStringBuilder.Append($"{unregistedPatient?.SinYm / 100}/{unregistedPatient?.SinYm % 100} ID:{unregistedPatient?.PtNum.AsString(),-10} {unregistedPatient?.Name ?? string.Empty}" + Environment.NewLine);
                    }
                    var patientList = patientStringBuilder.ToString().TrimEnd(Environment.NewLine.ToCharArray());

                    // Send message confirm to FE
                    SendMessager(true, confirmMessage, patientList);
                    return new CreateYuIchiFileOutputData(confirmMessage, patientList, CreateYuIchiFileStatus.Failed);
                }
            }

            if (inputData.IsCreateForm1File)
            {
                isExported = ExportOutpatientForm1(inputData.HpId, inputData.SinYm, inputData.IsCheckedTestPatient) || isExported;
            }
            if (inputData.IsCreateEFFile || inputData.IsCreateEFile || inputData.IsCreateFFile)
            {
                isExported = ExportEFFile(inputData.HpId, inputData.SinYm, inputData.IsCreateEFFile, inputData.IsCreateEFile, inputData.IsCreateFFile, inputData.IsCheckedTestPatient) || isExported;
            }
            if (inputData.IsCreateKData)
            {
                isExported = ExportForeignKCsvFile(inputData.HpId, inputData.SinYm, inputData.IsCheckedTestPatient) || isExported;
            }
            if (isExported)
            {
                // send done message progress to FE
                SendMessager(true, mFree00040, GetMessage(CreateYuIchiFileStatus.CreateYuIchiFileSuccessed));
                return new CreateYuIchiFileOutputData(mFree00040, string.Empty, CreateYuIchiFileStatus.CreateYuIchiFileSuccessed);
            }
            // send faill message progress to FE
            SendMessager(true, mFree00040, GetMessage(CreateYuIchiFileStatus.CreateYuIchiFileFailed));
            return new CreateYuIchiFileOutputData(mFree00040, string.Empty, CreateYuIchiFileStatus.CreateYuIchiFileFailed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }

    #region export data
    #region ExportForeignKCsvFile
    private bool ExportForeignKCsvFile(int hpId, int sinYm, bool isCheckedTestPatient)
    {
        // send message progress to export data
        SendMessager(false, progress, $"Kファイル{sinYm}月分　作成中・・・");

        var raiinInfs = _yousikiRepository.GetRaiinInfsInMonth(hpId, sinYm);
        if (!raiinInfs.Any())
        {
            return false;
        }
        string facilityCode = GetFacilityCode(hpId, sinYm);
        string fileName = Path.Combine($"Kg_{facilityCode}_{sinYm}.csv");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var encoding = Encoding.GetEncoding(932);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            byte[] bytes;
            using (StreamWriter writer = new StreamWriter(memoryStream, encoding))
            {
                foreach (var raiinInf in raiinInfs)
                {
                    if (isCheckedTestPatient || !raiinInf.IsTester)
                    {
                        writer.Write($"{facilityCode},{raiinInf.PtNum.AsString().PadLeft(10, '0')},{raiinInf.SinDate}" +
                        $",{HenkanJ.Instance.ToFullsize(Regex.Replace(raiinInf.KanaName, @"\s+", string.Empty))},{raiinInf.Sex},{raiinInf.Birthday.AsString().PadLeft(8, '0')}\r\n");
                    }
                }
            }
            bytes = memoryStream.ToArray();

            string base64Data = Convert.ToBase64String(bytes);

            // send contentData to export data
            SendMessager(false, fileContent, $"Kファイル{sinYm}月分　作成中・・・", base64Data, fileName);
        }

        return true;
    }
    #endregion

    #region ExportEFFile
    private bool ExportEFFile(int hpId, int sinYm, bool isCreateEFFile, bool isCreateEFile, bool isCreateFFile, bool isCheckedTestPatient)
    {
        bool result = false;
        string facilityCode = GetFacilityCode(hpId, sinYm);
        string fileData;
        string fileName;
        var _fileViewModel = new EFFileViewModel(_tenantProvider, _systemConfigProvider, _emrLogger);
        try
        {
            if (isCreateEFFile)
            {
                // send progress to export data
                SendMessager(false, progress, $"EFファイル{sinYm}月分　作成中・・・");

                fileData = _fileViewModel.GetEFFileData(hpId, sinYm, isCheckedTestPatient);
                if (!string.IsNullOrEmpty(fileData))
                {
                    result = true;
                    fileName = $"EFg_{facilityCode}_{sinYm}.txt";

                    // Export txt file
                    ExportTxtFile(fileName, $"EFファイル{sinYm}月分　作成中・・・", string.Empty, fileData, false);
                }
            }
            if (isCreateEFile)
            {
                // send progress to export data
                SendMessager(false, progress, $"Eファイル{sinYm}月分　作成中・・・");

                fileData = _fileViewModel.GetEFileData(hpId, sinYm, isCheckedTestPatient);
                if (!string.IsNullOrEmpty(fileData))
                {
                    result = true;
                    fileName = $"Eg_{facilityCode}_{sinYm}.txt";

                    // Export txt file
                    ExportTxtFile(fileName, $"Eファイル{sinYm}月分　作成中・・・", string.Empty, fileData, false);
                }
            }
            if (isCreateFFile)
            {
                // send progress to export data
                SendMessager(false, progress, $"Fファイル{sinYm}月分　作成中・・・");
                fileData = _fileViewModel.GetFFileData(hpId, sinYm, isCheckedTestPatient);
                if (!string.IsNullOrEmpty(fileData))
                {
                    result = true;
                    fileName = $"Fg_{facilityCode}_{sinYm}.txt";

                    // Export txt file
                    ExportTxtFile(fileName, $"Fファイル{sinYm}月分　作成中・・・", string.Empty, fileData, false);
                }
            }
        }
        finally
        {
            _fileViewModel.Dispose();
        }
        return result;
    }
    #endregion

    #region Export Outpatient Form 1 (FF1)
    /// <summary>
    /// Export OutPatient Form1
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="isCheckedTestPatient"></param>
    /// <returns></returns>
    private bool ExportOutpatientForm1(int hpId, int sinYm, bool isCheckedTestPatient)
    {
        // send progress to export data
        SendMessager(false, progress, $"様式１{sinYm}月分　作成中・・・");

        var listYousiki1Inf = _yousikiRepository.GetListYousiki1Inf(hpId, sinYm, 2);
        if (!listYousiki1Inf.Any())
        {
            return false;
        }
        listYousiki1Inf = listYousiki1Inf.OrderBy(item => item.PtNum)
                                         .ThenBy(item => item.DataType)
                                         .ThenBy(item => item.SeqNo)
                                         .ToList();

        string facilityCode = GetFacilityCode(hpId, sinYm);
        var fileFF1Content = GenerateFF1FileContent(sinYm.ToString(), facilityCode, listYousiki1Inf, isCheckedTestPatient);
        var headerContent = $"施設コード\tデータ識別番号\t受診年月\tコード\t連番\t" +
                            $"ペイロード1\tペイロード2\tペイロード3\tペイロード4\tペイロード5\tペイロード6\tペイロード7\tペイロード8\tペイロード9\r\n";
        var fileName = $"FF1_{facilityCode}_{sinYm}.txt";

        // Export txt file
        ExportTxtFile(fileName, $"様式１{sinYm}月分　作成中・・・", headerContent, fileFF1Content, true);
        return true;
    }

    /// <summary>
    /// Generate FF1 file content
    /// </summary>
    /// <param name="sSinYm"></param>
    /// <param name="facilityCode"></param>
    /// <param name="listYousiki1Inf"></param>
    /// <param name="isCheckedTestPatient"></param>
    /// <returns></returns>
    private string GenerateFF1FileContent(string sSinYm, string facilityCode, List<Yousiki1InfModel> listYousiki1Inf, bool isCheckedTestPatient)
    {
        StringBuilder result = new();
        foreach (var yousiki1Inf in listYousiki1Inf)
        {
            if (!isCheckedTestPatient && yousiki1Inf.IsTester)
            {
                continue;
            }
            var content = GenerateFF1FileContentFromYousiki1InfDetail(yousiki1Inf.PtNum, sSinYm, facilityCode, yousiki1Inf.FilterYousiki1InfDetailList);

            if (string.IsNullOrEmpty(content))
            {
                continue;
            }

            if (!string.IsNullOrEmpty(result.ToString()))
            {
                result.Append(Environment.NewLine);
                result.Append(content);
            }
            else
            {
                result.Append(content);
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Generate FF1 file content from Yousiki1InfDetail
    /// </summary>
    /// <param name="ptNum"></param>
    /// <param name="sSinYm"></param>
    /// <param name="facilityCode"></param>
    /// <param name="listYousiki1InfDetail"></param>
    /// <returns></returns>
    private string GenerateFF1FileContentFromYousiki1InfDetail(long ptNum, string sSinYm, string facilityCode, List<Yousiki1InfDetailModel> listYousiki1InfDetail)
    {
        StringBuilder result = new();
        var listCodeNo = listYousiki1InfDetail.Select(item => item.CodeNo).Distinct().ToList();
        foreach (var codeNo in listCodeNo)
        {
            var listDetail = listYousiki1InfDetail.Where(item => item.CodeNo == codeNo).OrderBy(x => x.RowNo).ToList();
            var content = GenerateFF1FileContentFromCodeNo(ptNum, sSinYm, facilityCode, codeNo, listDetail);
            if (string.IsNullOrEmpty(content))
            {
                continue;
            }
            if (!string.IsNullOrEmpty(result.ToString()))
            {
                result.Append(Environment.NewLine);
                result.Append(content);
            }
            else
            {
                result.Append(content);
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Generate FF1 file content from codeNo
    /// </summary>
    /// <param name="ptNum"></param>
    /// <param name="sSinYm"></param>
    /// <param name="facilityCode"></param>
    /// <param name="codeNo"></param>
    /// <param name="listYousiki1InfDetail"></param>
    /// <returns></returns>
    private string GenerateFF1FileContentFromCodeNo(long ptNum, string sSinYm, string facilityCode, string codeNo, List<Yousiki1InfDetailModel> listYousiki1InfDetail)
    {
        StringBuilder result = new();
        var listRowNo = listYousiki1InfDetail.Select(item => item.RowNo).Distinct().ToList();
        var header = GenerateFF1FileHeader(sSinYm, facilityCode, ptNum);
        foreach (var rowNo in listRowNo)
        {
            var listDetail = listYousiki1InfDetail.Where(item => item.RowNo == rowNo).ToList();
            var body = GenerateFF1FileBody(codeNo, rowNo, listDetail);
            if (string.IsNullOrEmpty(body))
            {
                continue;
            }
            var newLine = $"{header}\t{body}";
            if (!string.IsNullOrEmpty(result.ToString()))
            {
                result.Append(Environment.NewLine);
                result.Append(newLine);
            }
            else
            {
                result.Append(newLine);
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Generate FF1 file header
    /// </summary>
    /// <param name="sSinYm"></param>
    /// <param name="facilityCode"></param>
    /// <param name="ptNum"></param>
    /// <returns></returns>
    private string GenerateFF1FileHeader(string sSinYm, string facilityCode, long ptNum)
    {
        return $"{facilityCode}\t{ptNum.AsString().PadLeft(10, '0')}\t{sSinYm}";
    }

    /// <summary>
    /// Generate FF1 file body
    /// </summary>
    /// <param name="codeNo"></param>
    /// <param name="rowNo"></param>
    /// <param name="listYousiki1InfDetail"></param>
    /// <returns></returns>
    private string GenerateFF1FileBody(string codeNo, int rowNo, List<Yousiki1InfDetailModel> listYousiki1InfDetail)
    {
        if (listYousiki1InfDetail == null || listYousiki1InfDetail.Count <= 0)
            return string.Empty;

        var payload1Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 1)?.Value;
        var payload2Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 2)?.Value;
        var payload3Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 3)?.Value;
        var payload4Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 4)?.Value;
        var payload5Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 5)?.Value;

        var payload6Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 6)?.Value;
        var payload7Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 7)?.Value;
        var payload8Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 8)?.Value;
        var payload9Value = listYousiki1InfDetail.FirstOrDefault(item => item.Payload == 9)?.Value;

        if (string.IsNullOrEmpty(payload1Value) &&
            string.IsNullOrEmpty(payload2Value) &&
            string.IsNullOrEmpty(payload3Value) &&
            string.IsNullOrEmpty(payload4Value) &&
            string.IsNullOrEmpty(payload5Value) &&
            string.IsNullOrEmpty(payload6Value) &&
            string.IsNullOrEmpty(payload7Value) &&
            string.IsNullOrEmpty(payload8Value) &&
            string.IsNullOrEmpty(payload9Value))
        {
            return string.Empty;
        }
        else
        {
            var prefixBody = $"{codeNo}\t{rowNo.AsString()}";
            var sufixBody = $"{payload1Value}\t{payload2Value}\t{payload3Value}\t{payload4Value}\t{payload5Value}\t{payload6Value}\t{payload7Value}\t{payload8Value}\t{payload9Value}";
            return $"{prefixBody}\t{sufixBody}";
        }
    }
    #endregion
    #endregion

    #region common function
    /// <summary>
    /// validate data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private CreateYuIchiFileOutputData ValidateData(CreateYuIchiFileInputData data)
    {
        if (data.SinYm <= 0 || data.SinYm.ToString().Length != 6)
        {
            return new CreateYuIchiFileOutputData(mInp00010, string.Empty, CreateYuIchiFileStatus.InvalidCreateYuIchiFileSinYm);
        }
        if (!data.IsCreateForm1File
            && !data.IsCreateEFFile
            && !data.IsCreateEFile
            && !data.IsCreateFFile
            && !data.IsCreateKData)
        {
            return new CreateYuIchiFileOutputData(mFree00030, string.Empty, CreateYuIchiFileStatus.InvalidCreateYuIchiFileSinYm);
        }
        return new CreateYuIchiFileOutputData(string.Empty, string.Empty, CreateYuIchiFileStatus.ValidateSuccessed);
    }

    /// <summary>
    /// Get FacilityCode
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <returns></returns>
    private string GetFacilityCode(int hpId, int sinYm)
    {
        int prefNo = 0;
        int medicalCode = _systemGenerationConfRepository.GetSettingValue(hpId, 100001, 0, sinYm).Item1;
        var hpInf = _hpInfRepository.GetListHpInf(hpId).OrderByDescending(item => item.StartDate).FirstOrDefault();
        if (hpInf != null)
        {
            prefNo = hpInf.PrefNo;
            if (medicalCode == 0)
            {
                medicalCode = hpInf.HpCd.AsInteger();
            }
        }
        return prefNo.AsString().PadLeft(2, '0') + medicalCode.AsString().PadLeft(7, '0');
    }

    /// <summary>
    /// Send message to FE
    /// </summary>
    /// <param name="doneProgress"></param>
    /// <param name="messageType"></param>
    /// <param name="message"></param>
    private void SendMessager(bool doneProgress, string messageType, string message)
    {
        var status = new CreateYuIchiFileProgressStatus(doneProgress, messageType, message, string.Empty, string.Empty);
        _messenger!.Send(status);
    }

    /// <summary>
    /// Send message to FE
    /// </summary>
    /// <param name="doneProgress"></param>
    /// <param name="messageType"></param>
    /// <param name="message"></param>
    /// <param name="dataContent"></param>
    /// <param name="fileName"></param>
    private void SendMessager(bool doneProgress, string messageType, string message, string dataContent, string fileName)
    {
        var status = new CreateYuIchiFileProgressStatus(doneProgress, messageType, message, dataContent, fileName);
        _messenger!.Send(status);
    }

    /// <summary>
    /// Get response message to send
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private string GetMessage(CreateYuIchiFileStatus status) => status switch
    {
        CreateYuIchiFileStatus.InvalidCreateYuIchiFileSinYm => InvalidCreateYuIchiFileSinYm,
        CreateYuIchiFileStatus.InvalidCreateYuIchiFileCheckedOption => InvalidCreateYuIchiFileCheckedOption,
        CreateYuIchiFileStatus.CreateYuIchiFileSuccessed => CreateYuIchiFileSuccessed,
        CreateYuIchiFileStatus.CreateYuIchiFileFailed => CreateYuIchiFileFailed,
        _ => string.Empty
    };

    private void ExportTxtFile(string fileName, string progressMessage, string headerContent, string contentData, bool writeEndLine = false)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var encoding = Encoding.GetEncoding(932);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            byte[] bytes;
            using (StreamWriter writer = new StreamWriter(memoryStream, encoding))
            {
                if (!string.IsNullOrEmpty(headerContent))
                {
                    writer.Write(headerContent);
                }
                if (!string.IsNullOrEmpty(contentData))
                {
                    writer.Write(contentData);
                }
                if (writeEndLine)
                {
                    // 改行がなければ支援ツールで最終行が読み込まれない
                    writer.WriteLine();
                }
            }
            bytes = memoryStream.ToArray();

            string base64Data = Convert.ToBase64String(bytes);

            // send contentData to export data
            SendMessager(false, fileContent, progressMessage, base64Data, fileName);
        }
    }
    #endregion 
}
