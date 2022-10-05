using DevExpress.Interface;
using DevExpress.Models;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.Karte2Print;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class Karte2ExportInteractor : IKarte2ExportInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IKarteInfRepository _karteInfRepository;
        private readonly IKarteKbnMstRepository _karteKbnRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IKaRepository _kaRepository;
        private readonly IRaiinListTagRepository _rainListTagRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly IKarte2Export _karte2Export;

        public Karte2ExportInteractor(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository, IReceptionRepository receptionRepository, IInsuranceRepository insuranceRepository, IUserRepository userRepository, IKaRepository kaRepository, IRaiinListTagRepository rainListTagRepository, IPatientInforRepository patientInforRepository, IAmazonS3Service amazonS3Service, IKarte2Export karte2Export)
        {
            _ordInfRepository = ordInfRepository;
            _karteInfRepository = karteInfRepository;
            _karteKbnRepository = karteKbnRepository;
            _receptionRepository = receptionRepository;
            _insuranceRepository = insuranceRepository;
            _userRepository = userRepository;
            _kaRepository = kaRepository;
            _rainListTagRepository = rainListTagRepository;
            _patientInforRepository = patientInforRepository;
            _amazonS3Service = amazonS3Service;
            _karte2Export = karte2Export;
        }
        public Karte2ExportOutputData Handle(Karte2ExportInputData inputData)
        {
            try
            {
                var patientInfo = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, inputData.SinDate, (int)inputData.RaiinNo);
                if (patientInfo == null || patientInfo.PtId == 0)
                {
                    return new Karte2ExportOutputData(Karte2PrintStatus.InvalidUser);
                }
                var historyKarteOdrRaiinItemWithStatus = GetHistoryKarteOdrRaiinItem(inputData);
                if (historyKarteOdrRaiinItemWithStatus.Item2 != Karte2PrintStatus.Successed)
                {
                    return new Karte2ExportOutputData(historyKarteOdrRaiinItemWithStatus.Item2);
                }
                var historyKarteOdrRaiinItem = historyKarteOdrRaiinItemWithStatus.Item1;
                var karte2ExportModel = new Karte2ExportModel()
                {
                    HpId = patientInfo.HpId,
                    PtId = patientInfo.PtId,
                    SinDate = historyKarteOdrRaiinItemWithStatus.Item1.First().SinDate,
                    RaiinNo = historyKarteOdrRaiinItemWithStatus.Item1.First().RaiinNo,
                    KanaName = patientInfo.KanaName,
                    Name = patientInfo.Name,
                    Sex = patientInfo.Sex.ToString(),
                    Birthday = patientInfo.Birthday.ToString(),
                    CurrentTime = DateTime.UtcNow.ToString(),
                    StartDate = inputData.StartDate.ToString(),
                    EndDate = inputData.EndDate.ToString(),
                    RichTextKarte2Models = MapToDevExpressModel(historyKarteOdrRaiinItem),
                    FileName = inputData.HpId + "_" + inputData.PtId + "_" + inputData.UserId
                };
                MemoryStream stream = new MemoryStream();
                _karte2Export.ExportToPdf(karte2ExportModel,stream);
                _karte2Export.ExportToPdf(karte2ExportModel);
                var url = UploadAmazonS3(karte2ExportModel, stream);
                if (string.IsNullOrEmpty(url))
                {
                    return new Karte2ExportOutputData(Karte2PrintStatus.InvalidUrl);
                }
                return new Karte2ExportOutputData(url, Karte2PrintStatus.Successed);
            }
            catch (Exception)
            {
                return new Karte2ExportOutputData(Karte2PrintStatus.Failed);
            }
        }
        private List<RichTextKarte2Model> MapToDevExpressModel(List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems)
        {
            var hokenKarte2s = historyKarteOdrRaiinItems.SelectMany(x => x.HokenGroups)
               .SelectMany(x => x.GroupOdrItems).ToList();

            var richTextKarte2s = historyKarteOdrRaiinItems.SelectMany(x => x.KarteHistories).SelectMany(x => x.KarteData)
                .Select(x => new RichTextKarte2Model()
                {
                    HpId = x.HpId,
                    PtId = x.PtId,
                    SinDate = x.SinDate,
                    RaiinNo = x.RaiinNo,
                    RichText = x.RichText,
                    GroupNameKarte2Models = hokenKarte2s.Where(y=> y.OdrInfs.Any(z => z.HpId == x.HpId &&
                                                                                      z.PtId == x.PtId &&
                                                                                      z.SinDate == x.SinDate&&
                                                                                      z.RaiinNo == x.RaiinNo))
                                                                                           .Select( k => 
                                                                                              new GroupNameKarte2Model()
                                                                                              {
                                                                                                  GroupName = k.GroupName,
                                                                                                  HpId = x.HpId,
                                                                                                  PtId = x.PtId,
                                                                                                  RaiinNo = x.RaiinNo,
                                                                                                  SinDate = x.SinDate,
                                                                                                  RpNameKarte2Models = k.OdrInfs.Select(m=> new RpNameKarte2Model()
                                                                                                  {
                                                                                                      RpName = m.RpName,
                                                                                                      ItemNameKarte2Models = m.OdrDetails.Select(n => new ItemNameKarte2Model()
                                                                                                      {
                                                                                                          ItemName = n.ItemName
                                                                                                      }).ToList(),

                                                                                                  }).ToList(),
                                                                                              }
                                                                                            ).ToList()
                }).ToList();

            return richTextKarte2s;
        }
        private string UploadAmazonS3(Karte2ExportModel karte2ExportModel, MemoryStream stream)
        {
            string fileName = CommonConstants.SubFolderKarte2Print + "/" + karte2ExportModel.FileName.Replace(" ","") + ".pdf";
            if (_amazonS3Service.ObjectExistsAsync(fileName).Result)
            {
                var isDelete = _amazonS3Service.DeleteObjectAsync(fileName).Result;
                if (!isDelete)
                {
                    return String.Empty;
                }
            }

            // Insert new file
            var subFolder = CommonConstants.SubFolderKarte2Print;

            if (stream.Length <= 0)
            {
                return String.Empty;
            }

            var responseUpload = _amazonS3Service.UploadPdfAsync(true,subFolder, fileName, stream);
            var url = responseUpload.Result;
            return url;
        }
        private (List<HistoryKarteOdrRaiinItem>, Karte2PrintStatus) GetHistoryKarteOdrRaiinItem(Karte2ExportInputData inputData)
        {
            var validate = Validate(inputData);
            if (validate != Karte2PrintStatus.Successed)
            {
                return new(new List<HistoryKarteOdrRaiinItem>(), validate);
            }

            var query = from raiinInf in _receptionRepository.GetList(inputData.HpId, inputData.PtId, inputData.DeletedOdrVisibilitySetting)
                        join ptHokenPattern in _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, inputData.DeletedOdrVisibilitySetting == 1)
                        on raiinInf.HokenPid equals ptHokenPattern.HokenPid
                        where inputData.StartDate <= raiinInf.SinDate &&
                        raiinInf.SinDate <= inputData.EndDate
                        select raiinInf;

            var allRaiinInf = query?.ToList();



            var raiinNos = allRaiinInf?.Select(q => q.RaiinNo)?.ToList();
            var tantoIds = allRaiinInf?.Select(r => r.TantoId).ToList();
            var kaIds = allRaiinInf?.Select(r => r.TantoId).ToList();
            var sinDates = allRaiinInf?.Select(r => r.SinDate).ToList();
            var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

            #region karte
            var allkarteKbns = _karteKbnRepository.GetList(inputData.HpId, true);
            var allkarteInfs = raiinNos == null ? new List<KarteInfModel>() : _karteInfRepository.GetList(inputData.PtId, inputData.HpId, inputData.DeletedOdrVisibilitySetting, raiinNos).OrderBy(c => c.KarteKbn).ToList();
            #endregion

            #region Odr
            var allOdrInfs = raiinNos == null ? new List<OrdInfModel>() : _ordInfRepository
           .GetList(inputData.PtId, inputData.HpId, inputData.UserId, inputData.DeletedOdrVisibilitySetting, raiinNos)
                                                  .Where(x => inputData.IsCheckedSyosai || x.OdrKouiKbn != 10)
                                                 .OrderBy(odr => odr.OdrKouiKbn)
                                                 .ThenBy(odr => odr.RpNo)
            .ThenBy(odr => odr.RpEdaNo)
            .ThenBy(odr => odr.SortNo)
                                                 .ToList();

            var insuranceData = _insuranceRepository.GetInsuranceListById(inputData.HpId, inputData.PtId, inputData.SinDate);
            var hokenFirst = insuranceData?.ListInsurance.FirstOrDefault();
            var doctors = tantoIds == null ? new List<UserMstModel>() : _userRepository.GetDoctorsList(tantoIds)?.ToList();
            var kaMsts = kaIds == null ? new List<KaMstModel>() : _kaRepository.GetByKaIds(kaIds)?.ToList();
            var raiinListTags = (sinDates == null || raiinNos == null) ? new List<RaiinListTagModel>() : _rainListTagRepository.GetList(inputData.HpId, inputData.PtId, false, sinDates, raiinNos)?.ToList();
            IEnumerable<ApproveInfModel>? approveInfs = null;

            if (allRaiinInf == null || !allRaiinInf.Any())
            {
                return new(new List<HistoryKarteOdrRaiinItem>(), Karte2PrintStatus.NoData);
            }

            Parallel.ForEach(allRaiinInf, raiinInf =>
            {
                //Infor relation
                var doctorFirst = doctors?.FirstOrDefault(d => d.UserId == raiinInf.TantoId);
                var kaMst = kaMsts?.FirstOrDefault(k => k.KaId == raiinInf.KaId);
                var raiinTag = raiinListTags?.FirstOrDefault(r => r.RaiinNo == raiinInf.RaiinNo && r.SinDate == raiinInf.SinDate);
                var approveInf = approveInfs?.FirstOrDefault(a => a.RaiinNo == raiinInf.RaiinNo);

                //Composite karte and order
                var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem(raiinInf.RaiinNo, raiinInf.SinDate, raiinInf.HokenPid, hokenFirst == null ? string.Empty : hokenFirst.HokenName, hokenFirst == null ? string.Empty : hokenFirst.DisplayRateOnly, raiinInf.SyosaisinKbn, raiinInf.JikanKbn, raiinInf.KaId, kaMst == null ? String.Empty : kaMst.KaName, raiinInf.TantoId, doctorFirst == null ? String.Empty : doctorFirst.Sname, raiinInf.SanteiKbn, raiinTag?.TagNo ?? 0, approveInf?.DisplayApprovalInfo ?? string.Empty, GetHokenPatternType(hokenFirst?.HokenKbn ?? 0), raiinInf.Status, new List<HokenGroupHistoryItem>(), new List<GrpKarteHistoryItem>());

                //Excute karte
                List<KarteInfModel> karteInfByRaiinNo = allkarteInfs.Where(odr => odr.RaiinNo == historyKarteOdrRaiin.RaiinNo).OrderBy(c => c.KarteKbn).ThenByDescending(c => c.IsDeleted).ThenBy(c => c.CreateDate).ThenBy(c => c.UpdateDate).ToList();

                historyKarteOdrRaiin.KarteHistories.AddRange(from karteKbn in allkarteKbns
                                                             where karteInfByRaiinNo.Any(c => c.KarteKbn == karteKbn.KarteKbn)
                                                             let karteGrp = new GrpKarteHistoryItem(karteKbn == null ? 0 : karteKbn.KarteKbn, string.IsNullOrEmpty(karteKbn?.KbnName) ? String.Empty : karteKbn.KbnName, string.IsNullOrEmpty(karteKbn?.KbnShortName) ? String.Empty : karteKbn.KbnShortName, karteKbn == null ? 0 : karteKbn.CanImg, karteKbn == null ? 0 : karteKbn.SortNo, karteInfByRaiinNo.Where(c => c.KarteKbn == karteKbn?.KarteKbn).OrderByDescending(c => c.IsDeleted)
                .Select(c => new KarteInfHistoryItem(
                                    c.HpId,
                                    c.RaiinNo,
                                    c.KarteKbn,
                                    c.SeqNo,
                                    c.PtId,
                                    c.SinDate,
                                    c.Text,
                                    c.CreateDate,
                                    c.UpdateDate,
                                    c.IsDeleted,
                                    c.RichText,
                                    c.CreateName
                                    )
                ).ToList())
                                                             select karteGrp);
                //Excute order
                ExcuteOrder(insuranceData, allOdrInfs, historyKarteOdrRaiin, historyKarteOdrRaiins);
            });

            var result = historyKarteOdrRaiins.OrderByDescending(x => x.SinDate).ToList();
            FilterData(ref result, inputData);


            #endregion
            if (historyKarteOdrRaiins?.Count > 0)
            {
                return (historyKarteOdrRaiins, Karte2PrintStatus.Successed);
            }
            else
                return new(new List<HistoryKarteOdrRaiinItem>(), Karte2PrintStatus.NoData);

        }
        private static int GetHokenPatternType(int hokenKbn)
        {
            switch (hokenKbn)
            {
                case 0:
                    return 3;
                case 1:
                case 2:
                    return 1;
                case 11:
                case 12:
                case 13:
                case 14:
                    return 2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private static Karte2PrintStatus Validate(Karte2ExportInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return Karte2PrintStatus.InvalidHpId;
            }

            if (inputData.PtId <= 0)
            {
                return Karte2PrintStatus.InvalidPtId;
            }

            if (inputData.SinDate <= 0)
            {
                return Karte2PrintStatus.InvalidSinDate;
            }

            if (inputData.StartDate <= 0)
            {
                return Karte2PrintStatus.InvalidStartDate;
            }
            if (inputData.EndDate <= 0)
            {
                return Karte2PrintStatus.InvalidEndDate;
            }

            if (!(inputData.DeletedOdrVisibilitySetting >= 0 && inputData.DeletedOdrVisibilitySetting <= 2))
            {
                return Karte2PrintStatus.InvalidDeleteCondition;
            }

            if (inputData.UserId <= 0)
            {
                return Karte2PrintStatus.InvalidUser;
            }

            return Karte2PrintStatus.Successed;
        }

        private void ExcuteOrder(InsuranceDataModel? insuranceData, List<OrdInfModel> allOdrInfs, HistoryKarteOdrRaiinItem historyKarteOdrRaiin, List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiins)
        {
            var odrInfListByRaiinNo = allOdrInfs
         .Where(o => o.RaiinNo == historyKarteOdrRaiin.RaiinNo).Select(
                o => o.ChangeOdrDetail(o.OrdInfDetails.Where(od =>od != null && od.RaiinNo == historyKarteOdrRaiin.RaiinNo).
         ToList()));
            odrInfListByRaiinNo = odrInfListByRaiinNo.OrderBy(odr => odr.OdrKouiKbn)
                                      .ThenBy(odr => odr.RpNo)
                                      .ThenBy(odr => odr.RpEdaNo)
                                      .ThenBy(odr => odr.SortNo)
                                      .ToList();

            // Find By Hoken
            List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

            Parallel.ForEach(hokenPidList, hokenPid =>
            {
                var hoken = insuranceData?.ListInsurance.FirstOrDefault(c => c.HokenId == hokenPid);
                var hokenGrp = new HokenGroupHistoryItem(hokenPid, hoken == null ? string.Empty : hoken.HokenName, new List<GroupOdrGHistoryItem>());

                var groupOdrInfList = odrInfListByRaiinNo.Where(odr => odr.HokenPid == hokenPid)
                    .GroupBy(odr => new
                    {
                        odr.HokenPid,
                        odr.GroupKoui,
                        odr.InoutKbn,
                        odr.SyohoSbt,
                        odr.SikyuKbn,
                        odr.TosekiKbn,
                        odr.SanteiKbn
                    })
                    .Select(grp => grp.FirstOrDefault())
                    .ToList();

                Parallel.ForEach(groupOdrInfList, groupOdrInf =>
                {
                    var group = new GroupOdrGHistoryItem(hokenPid, string.Empty, new List<OdrInfHistoryItem>());

                    var rpOdrInfs = odrInfListByRaiinNo.Where(odrInf => odrInf.HokenPid == hokenPid
                                                && odrInf.GroupKoui.Value == groupOdrInf?.GroupKoui.Value
                                                && odrInf.InoutKbn == groupOdrInf?.InoutKbn
                                                && odrInf.SyohoSbt == groupOdrInf?.SyohoSbt
                                                && odrInf.SikyuKbn == groupOdrInf?.SikyuKbn
                                                && odrInf.TosekiKbn == groupOdrInf?.TosekiKbn
                                                && odrInf.SanteiKbn == groupOdrInf?.SanteiKbn)
                                            .ToList();

                    //_mapper.Map<OdrInfModel>(c)
                    Parallel.ForEach(rpOdrInfs.OrderBy(c => c.IsDeleted), rpOdrInf =>
                    {

                        var odrModel = new OdrInfHistoryItem(
                                                        rpOdrInf.HpId,
                                                        rpOdrInf.RaiinNo,
                                                        rpOdrInf.RpNo,
                                                        rpOdrInf.RpEdaNo,
                                                        rpOdrInf.PtId,
                                                        rpOdrInf.SinDate,
                                                        rpOdrInf.HokenPid,
                                                        rpOdrInf.OdrKouiKbn,
                                                        rpOdrInf.RpName,
                                                        rpOdrInf.InoutKbn,
                                                        rpOdrInf.SikyuKbn,
                                                        rpOdrInf.SyohoSbt,
                                                        rpOdrInf.SanteiKbn,
                                                        rpOdrInf.TosekiKbn,
                                                        rpOdrInf.DaysCnt,
                                                        rpOdrInf.SortNo,
                                                        rpOdrInf.Id,
                                                        rpOdrInf.GroupKoui.Value,
                                                        rpOdrInf.OrdInfDetails.Select(od =>
                                                            new OdrInfDetailItem(
                                                                od.HpId,
                                                                od.RaiinNo,
                                                                od.RpNo,
                                                                od.RpEdaNo,
                                                                od.RowNo,
                                                                od.PtId,
                                                                od.SinDate,
                                                                od.SinKouiKbn,
                                                                od.ItemCd,
                                                                od.ItemName,
                                                                od.Suryo,
                                                                od.UnitName,
                                                                od.UnitSbt,
                                                                od.TermVal,
                                                                od.KohatuKbn,
                                                                od.SyohoKbn,
                                                                od.SyohoLimitKbn,
                                                                od.DrugKbn,
                                                                od.YohoKbn,
                                                                od.Kokuji1,
                                                                od.Kokuji2,
                                                                od.IsNodspRece,
                                                                od.IpnCd,
                                                                od.IpnName,
                                                                od.JissiKbn,
                                                                od.JissiDate,
                                                                od.JissiId,
                                                                od.JissiMachine,
                                                                od.ReqCd,
                                                                od.Bunkatu,
                                                                od.CmtName,
                                                                od.CmtName,
                                                                od.FontColor,
                                                                od.CommentNewline,
                                                                od.Yakka,
                                                                od.IsGetPriceInYakka,
                                                                od.Ten,
                                                                od.BunkatuKoui,
                                                                od.AlternationIndex,
                                                                od.KensaGaichu,
                                                                od.OdrTermVal,
                                                                od.CnvTermVal,
                                                                od.YjCd,
                                                                od.MasterSbt,
                                                                od.YohoSets,
                                                                od.Kasan1,
                                                                od.Kasan2
                                                        )
                                                        ).ToList(),
                                                        rpOdrInf.CreateDate,
                                                        rpOdrInf.CreateId,
                                                        rpOdrInf.CreateName,
                                                        rpOdrInf.UpdateDate,
                                                        rpOdrInf.IsDeleted
                                                     );

                        group.OdrInfs.Add(odrModel);
                    });
                    hokenGrp.GroupOdrItems.Add(group);
                });

                historyKarteOdrRaiin.HokenGroups.Add(hokenGrp);
            });
            historyKarteOdrRaiins.Add(historyKarteOdrRaiin);
        }
        private void FilterData(ref List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, Karte2ExportInputData inputData)
        {
            List<OrderHokenType> GetListAcceptedHokenType()
            {
                List<OrderHokenType> result = new List<OrderHokenType>();
                if (inputData.IsCheckedHoken)
                {
                    result.Add(OrderHokenType.Hoken);
                }
                if (inputData.IsCheckedJihi)
                {
                    result.Add(OrderHokenType.Jihi);
                }
                if (inputData.IsCheckedHokenJihi)
                {
                    result.Add(OrderHokenType.HokenJihi);
                }
                if (inputData.IsCheckedJihiRece)
                {
                    result.Add(OrderHokenType.JihiRece);
                }
                if (inputData.IsCheckedHokenRousai)
                {
                    result.Add(OrderHokenType.Rousai);
                }
                if (inputData.IsCheckedHokenJibai)
                {
                    result.Add(OrderHokenType.Jibai);
                }
                return result;
            }
            if (!inputData.IsIncludeTempSave)
            {
                historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.Status != 3).ToList();
            }

            List<OrderHokenType> listAcceptedHokenType = GetListAcceptedHokenType();

            //Filter raiin as hoken setting
            List<HistoryKarteOdrRaiinItem> filteredKaruteList = new List<HistoryKarteOdrRaiinItem>();
            foreach (var history in historyKarteOdrRaiinItems)
            {
                if (listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                {
                    filteredKaruteList.Add(history);
                    continue;
                }

                if (history.HokenGroups == null ||
                    !history.HokenGroups.Any())
                {
                    continue;
                }

                if (inputData.DeletedOdrVisibilitySetting == 1)
                {
                    if (listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                    {
                        filteredKaruteList.Add(history);
                        continue;
                    }
                }
                else if (inputData.DeletedOdrVisibilitySetting == 0)
                {

                    foreach (var hokenGroup in history.HokenGroups)
                    {
                        bool isDataExisted = false;
                        foreach (var group in hokenGroup.GroupOdrItems)
                        {
                            isDataExisted = group.OdrInfs.Any(o => o.IsDeleted == 0);
                            if (isDataExisted)
                            {
                                break;
                            }
                        }

                        if (isDataExisted &&
                            listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                        {
                            filteredKaruteList.Add(history);
                            break;
                        }
                    }
                }
                else if (inputData.DeletedOdrVisibilitySetting == 2)
                {
                    foreach (var hokenGroup in history.HokenGroups)
                    {
                        bool isDataExisted = false;
                        foreach (var group in hokenGroup.GroupOdrItems)
                        {
                            isDataExisted = group.OdrInfs.Any(o => o.IsDeleted != 2);
                            if (isDataExisted)
                            {
                                break;
                            }
                        }

                        if (isDataExisted &&
                            listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                        {
                            filteredKaruteList.Add(history);
                            break;
                        }
                    }
                }
            }

            historyKarteOdrRaiinItems = filteredKaruteList;

            historyKarteOdrRaiinItems.ForEach((karute) =>
            {

                //Filter order as hoken setting
                if (karute.HokenGroups != null && karute.HokenGroups.Any())
                {
                    var listHoken = karute.HokenGroups.Where(h => listAcceptedHokenType.Contains((OrderHokenType)karute.HokenType)).ToList();
                    listHoken.ForEach((hoken) =>
                    {
                        if (!inputData.IsCheckedJihi)
                        {
                            {
                                hoken = new HokenGroupHistoryItem(hoken.HokenPid, hoken.HokenTitle, hoken.GroupOdrItems.Where(o => o.SanteiKbn == 0).ToList());
                            }
                        }
                        hoken.GroupOdrItems.ToList().ForEach((group) =>
                        {
                            if (!inputData.IsCheckedJihi)
                            {
                                if (inputData.DeletedOdrVisibilitySetting == 0)
                                {
                                    group = new GroupOdrGHistoryItem(group.HokenPid, group.SinkyuName, group.OdrInfs
                                        .Where(o => o.IsDeleted == 0)
                                        .OrderBy(o => o.SortNo)
                                        .ToList());
                                }
                                else if (inputData.DeletedOdrVisibilitySetting == 2)
                                {
                                    group = new GroupOdrGHistoryItem(group.HokenPid, group.SinkyuName, group.OdrInfs
                                        .Where(o => o.IsDeleted != 2)
                                        .OrderBy(o => o.SortNo)
                                        .ToList());
                                }
                            }
                        });
                    });

                    karute = new HistoryKarteOdrRaiinItem(karute.RaiinNo, karute.SinDate, karute.HokenPid, karute.HokenTitle, karute.HokenRate, karute.SyosaisinKbn, karute.JikanKbn, karute.KaId, karute.KaName, karute.TantoId, karute.TantoName, karute.SanteiKbn, karute.TagNo, karute.SinryoTitle, karute.HokenType, karute.Status, listHoken, karute.KarteHistories);
                }
            });

            //Filter karte and order empty
            historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.HokenGroups != null && k.HokenGroups.Any() && k.KarteHistories != null && k.KarteHistories.Any()).ToList();
        }



    }
}
