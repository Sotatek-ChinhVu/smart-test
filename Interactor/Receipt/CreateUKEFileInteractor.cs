using Domain.Constant;
using Domain.Models.Receipt;
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

                int seikyuKbnMode = 0;
                if (inputData.ChkTogetsu)
                    seikyuKbnMode = 0;
                if (inputData.ChkHenreisai)
                    seikyuKbnMode = 1;
                if (inputData.ChkHenreisai && inputData.ChkTogetsu)
                    seikyuKbnMode = 2;

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
            {
                using FileStream fs = File.Create(filePath);
            }
                
            List<ArgumentModel> arguments = new List<ArgumentModel>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            for (int i = 0; i < receData.Count; i++)
            {
                RecedenFileInfo fileInfo = GetFileInfo(i + 1, modeType, raisoRadio);

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

        private RecedenFileInfo GetFileInfo(int index, ModeTypeCreateUKE mode, int raisoRadio)
        {
            string prefix = string.Empty;
            int createDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
            string time = CIUtil.GetJapanDateTimeNow().ToString("HH:mm:ss");
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
            return new RecedenFileInfo(fileName, createDate, createTime, prefix);
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
    }

    internal class RecedenFileInfo
    {
        public string Prefix { get; private set; }

        public string FileName { get; private set; }

        public int CreateDate { get; private set; }

        public int CreateTime { get; private set; }

        internal RecedenFileInfo(string filename, int createDate, int createTime, string prefix = "")
        {
            FileName = filename;
            CreateDate = createDate;
            CreateTime = createTime;
            Prefix = prefix;
        }
    }
}
