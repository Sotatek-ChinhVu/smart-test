using Domain.Constant;
using Domain.Models.PatientInfor;
using Domain.Models.ReceSeikyu;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using UseCase.ReceSeikyu.ImportFile;

namespace Interactor.ReceSeikyu
{
    public class ImportFileReceSeikyuInteractor : IImportFileReceSeikyuInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly List<ReceSeikyuModel> receSeikyuImportList;

        public ImportFileReceSeikyuInteractor(IReceSeikyuRepository receptionRepository, IPatientInforRepository patientInforRepository, IAmazonS3Service amazonS3Service)
        {
            _receSeikyuRepository = receptionRepository;
            _patientInforRepository = patientInforRepository;
            _amazonS3Service = amazonS3Service;
            receSeikyuImportList = new();
        }

        public ImportFileReceSeikyuOutputData Handle(ImportFileReceSeikyuInputData inputData)
        {
            try
            {
                string content = ReadAsString(inputData.File);
                List<string> fileContent;

                // if the Operating System is linux, the file content has a "\n" character as a line break
                if (OperatingSystem.IsLinux())
                {
                    fileContent = content.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToList();
                }
                else
                {
                    fileContent = content.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).ToList();
                }
                string fileName = inputData.File.FileName;

                if (inputData.HpId <= 0)
                {
                    return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.InvalidHpId, string.Empty);
                }
                if (fileContent == null || fileContent.Count == 0)
                {
                    return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.InvalidContentFile, string.Empty);
                }

                return HandlerImportFileRece(fileName, fileContent, inputData.HpId, inputData.UserId, inputData.File.OpenReadStream());
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
            }
        }

        private ImportFileReceSeikyuOutputData HandlerImportFileRece(string fileName, List<string> fileContent, int hpId, int userId, Stream file)
        {
            string sRecKind = string.Empty;
            int areCnt = 0;
            int aSeikyuYm = 0;
            int sinYm = 0;
            int birthday = 0;
            long ptId = 0;
            int hokenId = 0;
            int seikyuYm = 0;
            string searchNo = string.Empty;
            string hosoku = string.Empty;
            string henreiJiyuu = string.Empty;
            string henreiJiyuuCd = string.Empty;
            List<string> rekiList = new List<string>();
            bool isSaveReki = false;

            bool IsReInvalid = false;
            if (fileName.ToUpper().Equals("RRECEC.HEN"))
            {
                for (int iRow = 0; iRow < fileContent.Count; iRow++)
                {
                    if (string.IsNullOrEmpty(fileContent[iRow])) continue;
                    List<string> slCol = fileContent[iRow].Split(new[] { "," }, StringSplitOptions.None).ToList();
                    if (slCol == null || slCol.Count == 0) continue;
                    if (slCol.Count < 21) continue;
                    if (IsReInvalid && slCol[0] != "RE") continue;
                    IsReInvalid = false;
                    // Get parameter from file
                    if (slCol[0] == "RE")
                    {
                        // Egg1
                        if (slCol[20].AsString().Length == 18)
                        {
                            ptId = _patientInforRepository.GetPtIdFromPtNum(hpId, CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 1, 9), 0)); // 1~9桁
                            birthday = StringToDateImport(slCol[6]);
                            sinYm = StringToSinYmImport(slCol[3]);
                            if (sinYm == 0)
                            {
                                sinYm = StringToSinYmImport(CIUtil.Copy(slCol[20], 10, 6)); // 11~16桁
                            }
                            if (!CheckIsValidImportData(hpId, ptId, sinYm, birthday))
                            {
                                IsReInvalid = true;
                                continue;
                            }
                            hokenId = CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 16, 3), 0); // 16~18桁目
                            hokenId *= 10; // ※保険番号、は、*10して保険IDに変換する
                        }
                        // Hayabusa
                        else
                        {
                            ptId = CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 1, 10), 0); // 1~10桁
                            sinYm = StringToSinYmImport(slCol[3]);
                            if (sinYm == 0)
                            {
                                sinYm = StringToSinYmImport(CIUtil.Copy(slCol[20], 11, 6)); // 11~16桁
                            }
                            birthday = StringToDateImport(slCol[6]);
                            if (!CheckIsValidImportData(hpId, ptId, sinYm, birthday))
                            {
                                IsReInvalid = true;
                                continue;
                            }
                            hokenId = CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 17, 5), 0); // 17~21桁目
                        }

                        // Insert RECE_SEIKYU
                        if (!InsertReceSeikyuProcess(hpId, ptId, sinYm, birthday, hokenId, userId)) continue;
                        areCnt++;
                        // Insert RECEDEN_RIREKI_INF_(レセオンライン返戻履歴管理)
                        _receSeikyuRepository.InsertSingleRerikiInf(hpId, ptId, sinYm, hokenId, slCol[18], fileContent[iRow], userId);
                    }
                }
            }
            else
            {
                for (int iRow = 0; iRow < fileContent.Count; iRow++)
                {
                    if (string.IsNullOrEmpty(fileContent[iRow])) continue;
                    List<string> slCol = fileContent[iRow].Split(new[] { "," }, StringSplitOptions.None).ToList();
                    if (IsReInvalid && slCol[0] != "RE") continue;
                    IsReInvalid = false;
                    if (iRow == 0)
                    {
                        //返戻医療機関レコード
                        if (slCol[0] != "HI")
                        {
                            string message = string.Format(ErrorMessage.MessageType_mEnt01030, new string[] { "返戻レセプト情報", "・返戻医療機関レコードが記録されていません。" });
                            return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Failed, message);
                        }
                        sRecKind = slCol[0];
                        aSeikyuYm = StringToSinYmImport(slCol[2]);
                    }
                    //レセプト共通レコード
                    else if (slCol[0] == "RE")
                    {
                        if (sRecKind != "HI" && sRecKind != "REKI")
                        {
                            string message = string.Format(ErrorMessage.MessageType_mEnt01030, new string[] { "返戻レセプト情報", "・履歴管理ブロックが記録されていません。" });
                            return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Failed, message);
                        }

                        if (sRecKind == "REKI")
                        {
                            if (InsertReceSeikyuProcess(hpId, ptId, sinYm, birthday, hokenId, userId, seikyuYm))
                            {
                                areCnt++;
                            }
                        }

                        // Egg1
                        if (slCol[20].AsString().Length == 11)
                        {
                            ptId = _patientInforRepository.GetPtIdFromPtNum(hpId, CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 1, 9), 0)); // 1~9桁
                            sinYm = StringToSinYmImport(slCol[3]);
                            birthday = StringToDateImport(slCol[6]);
                            if (!CheckIsValidImportData(hpId, ptId, sinYm, birthday))
                            {
                                IsReInvalid = true;
                                continue;
                            }
                            hokenId = CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 10, 2), 0); // 10~11桁目
                            hokenId *= 10; // ※保険番号、は、*10して保険IDに変換する
                        }
                        // Hayabusa
                        else
                        {
                            ptId = CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 1, 10), 0); // 1~10桁
                            sinYm = StringToSinYmImport(slCol[3]);
                            if (sinYm == 0)
                            {
                                sinYm = StringToSinYmImport(CIUtil.Copy(slCol[20], 11, 6)); // 11~16桁
                            }
                            birthday = StringToDateImport(slCol[6]);
                            if (!CheckIsValidImportData(hpId, ptId, sinYm, birthday))
                            {
                                IsReInvalid = true;
                                continue;
                            }
                            hokenId = CIUtil.StrToIntDef(CIUtil.Copy(slCol[20], 17, 5), 0); // 17~21桁目
                            for (int row = iRow + 1; row < fileContent.Count; row++)
                            {
                                if (string.IsNullOrEmpty(fileContent[row])) continue;
                                List<string> col = fileContent[row].Split(new[] { "," }, StringSplitOptions.None).ToList();
                                if (col.Count >= 4 && col[3] == "IR")
                                {
                                    seikyuYm = CIUtil.StrToIntDef(col[10], 0);
                                    break;
                                }
                            }
                        }

                        searchNo = slCol[18];
                        sRecKind = slCol[0];
                    }
                    //返戻理由レコード
                    else if (slCol[0] == "HR")
                    {
                        if (ptId == 0) continue;
                        if (sRecKind != "RE" && sRecKind != "HR")
                        {
                            string message = string.Format(ErrorMessage.MessageType_mEnt01030, new string[] { "返戻レセプト情報", "・レセプト共通レコードが記録されていません。" });
                            return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Failed, message);
                        }
                        sRecKind = slCol[0];
                        henreiJiyuuCd = slCol[4];
                        henreiJiyuu = slCol[5];
                        hosoku = slCol[6];
                        _receSeikyuRepository.InsertSingleHenJiyuu(hpId, ptId, sinYm, hokenId, hosoku, henreiJiyuuCd, henreiJiyuu, userId);
                    }
                    else if (CIUtil.StrToIntDef(slCol[0], 0) >= 1)
                    {
                        if (ptId == 0) continue;
                        if (sRecKind != "HR" && sRecKind != "REKI")
                        {
                            string message = string.Format(ErrorMessage.MessageType_mEnt01030, new string[] { "返戻レセプト情報", "・返戻理由レコードが記録されていません。" });
                            return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Failed, message);
                        }
                        sRecKind = "REKI";

                        // Save reki
                        rekiList.Add(fileContent[iRow]);
                        if (iRow + 1 < fileContent.Count)
                        {
                            var slColNext = fileContent[iRow + 1].Split(new[] { "," }, StringSplitOptions.None).ToList();
                            if (CIUtil.StrToIntDef(slColNext[0], 0) < 1)
                            {
                                isSaveReki = true;
                            }
                        }
                        else
                        {
                            isSaveReki = true;
                        }
                        if (isSaveReki)
                        {
                            string rekiContern = string.Join(Environment.NewLine, rekiList);
                            _receSeikyuRepository.InsertSingleRerikiInf(hpId, ptId, sinYm, hokenId, searchNo, rekiContern, userId);
                            isSaveReki = false;
                            rekiList = new List<string>();
                        }

                    }
                    //返戻合計レコード
                    else if (slCol[0] == "HG")
                    {
                        if (ptId == 0) continue;
                        if (sRecKind != "REKI")
                        {
                            string message = string.Format(ErrorMessage.MessageType_mEnt01030, new string[] { "返戻レセプト情報", "・履歴管理ブロックが記録されていません。" });
                            return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Failed, message);
                        }
                        sRecKind = "HG";
                        InsertReceSeikyuProcess(hpId, ptId, sinYm, birthday, hokenId, userId);
                        areCnt++;
                    }
                }
            }

            string path = $"{CommonConstants.Tempotary}/{CommonConstants.ReceiptcHen}/{aSeikyuYm}/";
            string fileNameUpload = $"{CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd_HHmmss_")}{fileName}";

            string idCloud = _amazonS3Service.UploadObjectAsync(path, fileNameUpload, file, true).Result;

            foreach (var receSeikyu in receSeikyuImportList)
            {
                receSeikyu.SetSeikyuYm(999999);
                _receSeikyuRepository.InsertSingleReceSeikyu(hpId, userId, receSeikyu);
            }

            if (_receSeikyuRepository.SaveChangeImportFileRececeikyus())
            {
                List<ReceInfo> receInfList = receSeikyuImportList.Select(item => new ReceInfo(item.PtId, item.HokenId, item.SinYm, item.SinYm)).Distinct().ToList();
                string message = string.Format(ErrorMessage.MessageType_mEnt02020, CIUtil.SMonthToShowSMonth(aSeikyuYm) + "請求分(" + areCnt + "件) の返戻レセプト");
                return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Successful, receInfList, message);
            }
            else
            {
                return new ImportFileReceSeikyuOutputData(ImportFileReceSeikyuStatus.Failed, string.Empty);
            }
        }

        private int StringToDateImport(string dateSource)
        {
            int result = 0;
            if (string.IsNullOrEmpty(dateSource)) return result;
            if (dateSource.Trim().Length == 7)
            {
                dateSource = string.Format("{0}.{1}.{2}.{3}",
                                            CIUtil.Copy(dateSource, 1, 1),
                                            CIUtil.Copy(dateSource, 2, 2),
                                            CIUtil.Copy(dateSource, 4, 2),
                                            CIUtil.Copy(dateSource, 6, 2));
                result = CIUtil.ShowWDateToSDate(dateSource);
            }
            else if (dateSource.Trim().Length == 8)
            {
                result = dateSource.AsInteger();
            }

            return result;
        }

        private int StringToSinYmImport(string dateSource)
        {
            int result = 0;
            if (string.IsNullOrEmpty(dateSource)) return result;
            if (dateSource.Trim().Length == 5)
            {
                dateSource = string.Format("{0}.{1}.{2}.01", CIUtil.Copy(dateSource, 1, 1), CIUtil.Copy(dateSource, 2, 2), CIUtil.Copy(dateSource, 4, 2));
                result = CIUtil.ShowWDateToSDate(dateSource) / 100;
            }
            else if (dateSource.Trim().Length == 6)
            {
                result = dateSource.AsInteger();
            }

            return result;
        }

        private bool CheckIsValidImportData(int hpId, long ptId, int sinYmFromFile, int birdthdayFromFile)
        {
            bool isPatientAlreadyCalculateInMonth = _patientInforRepository.GetCountRaiinAlreadyPaidOfPatientByDate(hpId, sinYmFromFile * 100 + 1,
                                                                                                          sinYmFromFile * 100 + 31, ptId,
                                                                                                          RaiinState.Calculate) > 0;
            bool isValidBirthday = _patientInforRepository.GetPtInf(hpId, ptId)?.Birthday == birdthdayFromFile;
            return ptId > 0 &&
                   ((sinYmFromFile > 0 && isPatientAlreadyCalculateInMonth) || (birdthdayFromFile > 0 && isValidBirthday));
        }


        private bool InsertReceSeikyuProcess(int hpId, long ptId, int sinYm, int birthDay, int hokenId, int userId, int seiKyuYm = 0)
        {
            if (!CheckIsValidImportData(hpId, ptId, sinYm, birthDay))
            {
                return false;
            }

            // RECE_SEIKYU 保存チェック
            if (_receSeikyuRepository.IsReceSeikyuExisted(hpId, ptId, sinYm, hokenId))
            {
                return false;
            }

            // PRE_HOKEN_IDを取得する
            int preHokenId = _receSeikyuRepository.GetReceSeikyuPreHoken(hpId, ptId, sinYm, hokenId);

            // 既存のRECE_SEIKYUレコードを削除する
            _receSeikyuRepository.DeleteReceSeikyu(hpId, userId, ptId, sinYm, hokenId);

            // PRE_HOKEN_IDに紐づくレセ電返戻事由を削除する
            _receSeikyuRepository.DeleteHenJiyuuRireki(hpId, userId, ptId, sinYm, hokenId);

            // RECE_SEIKYUにレコードを追加する
            var insertAfterReceSeikuu = new ReceSeikyuModel(hpId, ptId, 3, seiKyuYm, sinYm, hokenId);
            var insertBeforeReceSeikyu = receSeikyuImportList.FirstOrDefault(p => p.PtId == ptId && p.SinYm == sinYm && p.HokenId == hokenId && p.IsDeleted == 0);
            if (insertBeforeReceSeikyu != null)
            {
                if (insertAfterReceSeikuu.SeikyuYm > 0 && insertBeforeReceSeikyu.SeikyuYm > 0)
                {
                    if (insertAfterReceSeikuu.SeikyuYm >= insertBeforeReceSeikyu.SeikyuYm)
                    {
                        insertBeforeReceSeikyu.SetIsDeleted(1);
                    }
                    else
                    {
                        insertAfterReceSeikuu.SetIsDeleted(1);
                    }
                }
                else
                {
                    insertBeforeReceSeikyu.SetIsDeleted(1);
                }
            }
            receSeikyuImportList.Add(insertAfterReceSeikuu);
            return true;
        }


        public string ReadAsString(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString();
        }
    }
}
