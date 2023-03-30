using Domain.Constant;
using Domain.Models.Receipt;
using Domain.Models.Receipt.ReceiptCreation;
using EventProcessor.Interfaces;
using EventProcessor.Model;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Common;
using Interactor.CalculateService;
using System.Text;
using UseCase.Receipt.CreateUKEFile;

namespace Interactor.Receipt
{
    public class CreateUKEFileInteractor : ICreateUKEFileInputPort
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly ICalcultateCustomerService _calcultateCustomerService;
        private readonly IEventProcessorService _eventProcessorService;

        public CreateUKEFileInteractor(IReceiptRepository receiptRepository, ICalcultateCustomerService calcultateCustomerService, IEventProcessorService eventProcessorService)
        {
            _receiptRepository = receiptRepository;
            _calcultateCustomerService = calcultateCustomerService;
            _eventProcessorService = eventProcessorService;
        }

        public CreateUKEFileOutputData Handle(CreateUKEFileInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.InvalidHpId, string.Empty, TypeMessage.TypeMessageError, new List<UKEFileOutputData>());

                if (inputData.SeikyuYm <= 0)
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.InvaliSeikyuYm, string.Empty, TypeMessage.TypeMessageError, new List<UKEFileOutputData>());

                if (inputData.ModeType == ModeTypeCreateUKE.Rosai)
                {
                    string errorInfs = ValidateData(inputData.HpId, inputData.SeikyuYm);
                    if (!string.IsNullOrEmpty(errorInfs))
                    {
                        return new CreateUKEFileOutputData(CreateUKEFileStatus.ErrorValidateRosai, errorInfs, TypeMessage.TypeMessageError, new List<UKEFileOutputData>());
                    }
                }

                // Aftercare
                if (inputData.ModeType == ModeTypeCreateUKE.Aftercare)
                {
                    string errorInfs = ValidateAftercare(inputData.HpId, inputData.SeikyuYm);
                    if (!string.IsNullOrEmpty(errorInfs))
                    {
                        return new CreateUKEFileOutputData(CreateUKEFileStatus.ErrorValidateAftercare, errorInfs, TypeMessage.TypeMessageError, new List<UKEFileOutputData>());
                    }
                }

                if (!inputData.ChkHenreisai && !inputData.ChkTogetsu)
                {
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.ErrorInputData, "出力対象が見つかりません。", TypeMessage.TypeMessageError, new List<UKEFileOutputData>());
                }
                if (inputData.IncludeOutDrug && !inputData.SkipWarningIncludeOutDrug)
                {
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.WarningInputData, "出力条件に '院外処方薬を記録する' が含まれています。" + Environment.NewLine + "本請求には使用できませんが、実行しますか？", TypeMessage.TypeMessageWarning, new List<UKEFileOutputData>());
                }
                else if (inputData.IncludeTester && !inputData.SkipWarningIncludeTester)
                {
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.WarningInputData, "出力条件に 'テスト患者を記録する' が含まれています。" + Environment.NewLine + "本請求には使用できませんが、実行しますか？", TypeMessage.TypeMessageWarning, new List<UKEFileOutputData>());
                }
                else if (inputData.KaId != 0 && !inputData.SkipWarningKaId)
                {
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.WarningInputData, "出力条件に '診療科' が含まれています。" + Environment.NewLine + "本請求には使用できませんが、実行しますか？", TypeMessage.TypeMessageWarning, new List<UKEFileOutputData>());
                }

                else if (inputData.DoctorId != 0 && !inputData.SkipWarningDoctorId)
                {
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.WarningInputData, "出力条件に '担当医' が含まれています。" + Environment.NewLine + "本請求には使用できませんが、実行しますか？", TypeMessage.TypeMessageWarning, new List<UKEFileOutputData>());
                }

                int seikyuKbnMode = 0;
                if (inputData.ChkTogetsu)
                    seikyuKbnMode = 0;
                if (inputData.ChkHenreisai)
                    seikyuKbnMode = 1;
                if (inputData.ChkHenreisai && inputData.ChkTogetsu)
                    seikyuKbnMode = 2;

                if (!inputData.ConfirmCreateUKEFile)
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.ConfirmCreateUKEFile, "磁気レセプトの作成には時間がかかる場合があります。実行しますか？", TypeMessage.TypeMessageConfirmation, new List<UKEFileOutputData>());

                var responseAPI = _calcultateCustomerService.RunCaculationPostAsync<List<string>>(TypeCalculate.GetRecedenData, new
                {
                    Mode = (int)inputData.ModeType,
                    inputData.Sort,
                    inputData.HpId,
                    SeikyuYM = inputData.SeikyuYm,
                    OutputYM = inputData.SeikyuYmOutput,
                    SeikyuKbnMode = seikyuKbnMode,
                    inputData.KaId,
                    TantoId = inputData.DoctorId,
                    inputData.IncludeTester,
                    inputData.IncludeOutDrug
                }).Result;

                if (!responseAPI.IsSuccess || responseAPI.Data.Count == 0)
                {
                    return new CreateUKEFileOutputData(CreateUKEFileStatus.NoData, "出力対象が見つかりません。" + Environment.NewLine + "・出力条件を確認し、再実行してください。", TypeMessage.TypeMessageError, new List<UKEFileOutputData>());
                }
                else
                {
                    var ukeFiles = HandlerFileUKE(responseAPI.Data, inputData.ModeType, inputData.HpId, inputData.UserId, inputData.SeikyuYm);

                    if (ukeFiles.Any())
                        return new CreateUKEFileOutputData(CreateUKEFileStatus.Successful, "ファイルを保存しました。", TypeMessage.TypeMessageSuccess, ukeFiles);
                    else
                        return new CreateUKEFileOutputData(CreateUKEFileStatus.Failed, string.Empty, TypeMessage.TypeMessageError, ukeFiles);
                }
            }
            finally
            {
                _receiptRepository.ReleaseResource();
            }
        }

        private List<UKEFileOutputData> HandlerFileUKE(List<string> receData, ModeTypeCreateUKE modeType, int hpId, int UserId, int seikyuYm)
        {
            List<UKEFileOutputData> ukeFiles = new List<UKEFileOutputData>();
            int raisoRadio = 0;
            switch (modeType)
            {
                case ModeTypeCreateUKE.Shaho:
                    raisoRadio = 3;
                    break;
                case ModeTypeCreateUKE.Kokuho:
                    raisoRadio = 3;
                    break;
                case ModeTypeCreateUKE.Rosai:
                    raisoRadio = 1;
                    break;
                case ModeTypeCreateUKE.Aftercare:
                    raisoRadio = 1;
                    break;
                default:
                    break;
            }

            string filePath = Path.GetFullPath("TempFiles\\Receiptc\\RECEIPTC.UKE");

            FileInfo fileTemp = new FileInfo(filePath);
            if (!fileTemp.Exists)
                File.Create(filePath);

            List<ArgumentModel> arguments = new List<ArgumentModel>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            for (int i = 0; i < receData.Count; i++)
            {
                RecedenFileInfoModel fileInfo = GetFileInfo(i + 1, modeType, raisoRadio);

                // Write temp file
                string fileName = $"{fileInfo.Prefix}{fileInfo.CreateDate}_{fileInfo.CreateTime}_{fileInfo.FileName}";

                File.WriteAllText(filePath, receData.ElementAt(i), Encoding.GetEncoding("shift_jis"));

                using (FileStream st = File.OpenRead(filePath))
                {
                    ukeFiles.Add(new UKEFileOutputData(st.ToMemoryStream(), fileName));
                }
                // Event
                arguments.Add(new ArgumentModel(hpId, UserId, EventCode.ReportReceden, 0, seikyuYm, 0, $"mode:{modeType} file:{fileInfo.FileName}"));
            }

            if (arguments.Count > 0)
            {
                _eventProcessorService.DoEvent(arguments);
            }

            return ukeFiles;
        }

        private RecedenFileInfoModel GetFileInfo(int index, ModeTypeCreateUKE mode, int raisoRadio)
        {
            string prefix = string.Empty;
            int createDate = CIUtil.DateTimeToInt(DateTime.Now);
            string time = DateTime.Now.ToString("HH:mm:ss");
            int createTime = time.Replace(":", "").AsInteger();
            string fileName = GetFileName(mode);
            switch (mode)
            {
                case ModeTypeCreateUKE.Shaho:
                    prefix = "S";
                    break;
                case ModeTypeCreateUKE.Kokuho:
                    prefix = "K";
                    break;
                case ModeTypeCreateUKE.Rosai:
                    prefix = "R";
                    if (raisoRadio == 1)
                    {
                        //当月請求分
                        fileName = "RREC0" + index + "00.UKE";
                    }
                    else if (raisoRadio == 2)
                    {
                        //返戻請求分
                        fileName = "RREC0" + index + "00.UKS";
                    }
                    break;
                case ModeTypeCreateUKE.Aftercare:
                    prefix = "R";
                    break;
                default:
                    break;
            }
            return new RecedenFileInfoModel(fileName, createDate, createTime, prefix);
        }

        public string GetFileName(ModeTypeCreateUKE mode)
        {
            string fileName = "RECEIPTC.UKE";
            if (mode == ModeTypeCreateUKE.Aftercare)
            {
                fileName = "AREC0100.UKE";
            }
            return fileName;
        }

        private string ValidateData(int hpId, int seikyuYm)
        {
            string errorSyobyo = string.Empty;
            string errorSyobyoKeika = string.Empty;
            string errorRousaiSaigai = string.Empty;

            List<ReceInfValidateModel> receInfModels = _receiptRepository.GetReceValidateReceiptCreation(hpId, new List<long>(), seikyuYm);
            foreach (var receInfItem in receInfModels)
            {
                if (receInfItem.IsTester == 1) continue;
                // check using Rosai Receden 
                if ((receInfItem.HokenKbn == 11 || receInfItem.HokenKbn == 12) &&
                    receInfItem.IsPaperRece == 0)
                {
                    // check error Rousai Saigai
                    if (receInfItem.RousaiSaigaiKbn != 1 &&
                        receInfItem.RousaiSaigaiKbn != 2)
                    {
                        errorRousaiSaigai += Environment.NewLine + string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId);
                    }
                    // check error Syobyo
                    if (receInfItem.RousaiSyobyoDate <= 0)
                    {
                        errorSyobyo += Environment.NewLine + string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId);
                    }
                    // check error SyobyoKeika
                    if (!_receiptRepository.ExistSyobyoKeikaData(hpId, receInfItem.PtId, receInfItem.SinYm, receInfItem.HokenId))
                    {
                        errorSyobyoKeika += Environment.NewLine + string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId);
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorRousaiSaigai))
            {
                errorRousaiSaigai = errorRousaiSaigai.Insert(0, "■災害区分が設定されていません。");
                errorRousaiSaigai = errorRousaiSaigai.Insert(0, Environment.NewLine);
                errorRousaiSaigai += Environment.NewLine;
                errorRousaiSaigai += Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(errorSyobyo))
            {
                errorSyobyo = errorSyobyo.Insert(0, "■傷病開始年月日が設定されていません。");
                errorSyobyo += Environment.NewLine;
                errorSyobyo += Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(errorSyobyoKeika))
            {
                errorSyobyoKeika = errorSyobyoKeika.Insert(0, "■傷病の経過が設定されていません。");
                errorSyobyoKeika += Environment.NewLine;
            }
            return errorRousaiSaigai + errorSyobyo + errorSyobyoKeika;
        }

        private string ValidateAftercare(int hpId, int seikyuYm)
        {
            string errorSyobyoKeika = string.Empty;
            List<ReceInfValidateModel> receInfModels = _receiptRepository.GetReceValidateReceiptCreation(hpId, new List<long>(), seikyuYm).Where(item => item.HokenKbn == 13).ToList();
            foreach (var receInfItem in receInfModels)
            {
                if (receInfItem.IsTester == 1) continue;
                // check using Aftercare Receden 
                if (receInfItem.HokenKbn == 13 && receInfItem.IsPaperRece == 0)
                {
                    // Check error SyobyoKeika 
                    if (!_receiptRepository.ExistSyobyoKeikaData(hpId, receInfItem.PtId, receInfItem.SinYm, receInfItem.HokenId))
                    {
                        errorSyobyoKeika += Environment.NewLine + string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId);
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorSyobyoKeika))
            {
                errorSyobyoKeika = errorSyobyoKeika.Insert(0, "■傷病の経過が設定されていません。");
                errorSyobyoKeika += Environment.NewLine;
            }
            return errorSyobyoKeika;
        }

    }

    internal class RecedenFileInfoModel
    {
        public string Prefix { get; private set; }

        public string FileName { get; private set; }

        public int CreateDate { get; private set; }

        public int CreateTime { get; private set; }

        internal RecedenFileInfoModel(string filename, int createDate, int createTime, string prefix = "")
        {
            FileName = filename;
            CreateDate = createDate;
            CreateTime = createTime;
            Prefix = prefix;
        }
    }
}
