﻿using Domain.Models.HistoryOrder;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.Ka;
using Domain.Models.KarteFilterMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.OrdInfs;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Domain.Models.User;
using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.GetListTrees;

namespace Interactor.MedicalExamination
{
    public class GetMedicalExaminationHistoryInteractor : IGetMedicalExaminationHistoryInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IKarteInfRepository _karteInfRepository;
        private readonly IKarteKbnMstRepository _karteKbnRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IKaRepository _kaRepository;
        private readonly IKarteFilterMstRepository _karteFilterMstRepository;
        private readonly IRaiinListTagRepository _rainListTagRepository;
        private readonly IHistoryOrderRepository _historyOrderRepository;
        public GetMedicalExaminationHistoryInteractor(IOrdInfRepository ordInfRepository, IKarteInfRepository karteInfRepository, IKarteKbnMstRepository karteKbnRepository, IReceptionRepository receptionRepository, IInsuranceRepository insuranceRepository, IUserRepository userRepository, IKaRepository kaRepository, IKarteFilterMstRepository karteFilterMstRepository, IRaiinListTagRepository rainListTagRepository, IHistoryOrderRepository historyOrderRepository)
        {
            _ordInfRepository = ordInfRepository;
            _karteInfRepository = karteInfRepository;
            _karteKbnRepository = karteKbnRepository;
            _receptionRepository = receptionRepository;
            _insuranceRepository = insuranceRepository;
            _userRepository = userRepository;
            _kaRepository = kaRepository;
            _karteFilterMstRepository = karteFilterMstRepository;
            _rainListTagRepository = rainListTagRepository;
            _historyOrderRepository = historyOrderRepository;
        }

        public GetMedicalExaminationHistoryOutputData Handle(GetMedicalExaminationHistoryInputData inputData)
        {
            try
            {
                var validate = Validate(inputData);
                if (validate != GetMedicalExaminationHistoryStatus.Successed)
                {
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), validate, 0);
                }
                var historyKarteOdrRaiins = new List<HistoryKarteOdrRaiinItem>();

                (int, List<HistoryOrderModel>) historyList = _historyOrderRepository.GetList(
                    inputData.HpId, 
                    inputData.UserId, 
                    inputData.PtId, 
                    inputData.SinDate, 
                    inputData.StartPage, 
                    inputData.PageSize,
                    (int)inputData.FilterId, 
                    inputData.DeleteConditon);

                var insuranceModelList = _insuranceRepository.GetInsuranceList(inputData.HpId, inputData.PtId, inputData.SinDate, true);

                foreach (HistoryOrderModel history in historyList.Item2)
                {
                    KarteInfModel karteInf = history.KarteInfModel;
                    KarteInfHistoryItem karteInfHistoryItem = new KarteInfHistoryItem(karteInf.HpId, karteInf.RaiinNo, karteInf.KarteKbn, karteInf.SeqNo, karteInf.PtId, karteInf.SinDate, karteInf.Text, karteInf.UpdateDate, karteInf.CreateDate, karteInf.IsDeleted, karteInf.RichText, karteInf.CreateName);
                    List<GrpKarteHistoryItem> karteHistoryList = new List<GrpKarteHistoryItem> {
                        new GrpKarteHistoryItem(
                        karteInf.KarteKbn,
                        string.Empty,
                        string.Empty,
                        1,
                        0,
                        new List<KarteInfHistoryItem> { karteInfHistoryItem }) 
                    };

                    var historyKarteOdrRaiin = new HistoryKarteOdrRaiinItem
                        (
                            history.RaiinNo,
                            history.SinDate,
                            history.HokenPid,
                            history.HokenTitle,
                            history.HokenRate,
                            history.SyosaisinKbn,
                            history.JikanKbn,
                            history.KaId,
                            history.KaName,
                            history.TantoId,
                            history.TantoName,
                            history.SanteiKbn,
                            history.TagNo,
                            history.SinryoTitle,
                            history.HokenType, 
                            new List<HokenGroupHistoryItem>(),
                            karteHistoryList
                        );

                    //Excute order
                    ExcuteOrder(insuranceModelList, history.OrderInfList, historyKarteOdrRaiin, historyKarteOdrRaiins);
                }
                var result = new GetMedicalExaminationHistoryOutputData(historyList.Item1, historyKarteOdrRaiins.OrderByDescending(x => x.SinDate).ToList(), GetMedicalExaminationHistoryStatus.Successed, 0);

                if (historyKarteOdrRaiins?.Count > 0)
                    return result;
                else
                    return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.NoData, 0);
            }
            catch
            {
                return new GetMedicalExaminationHistoryOutputData(0, new List<HistoryKarteOdrRaiinItem>(), GetMedicalExaminationHistoryStatus.Failed, 0);
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        private static GetMedicalExaminationHistoryStatus Validate(GetMedicalExaminationHistoryInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidHpId;
            }
            if (inputData.StartPage < 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidStartPage;
            }
            if (inputData.PtId <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidPtId;
            }
            if (inputData.SinDate <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidSinDate;
            }
            if (inputData.PageSize <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidPageSize;
            }

            if (!(inputData.DeleteConditon >= 0 && inputData.DeleteConditon <= 2))
            {
                return GetMedicalExaminationHistoryStatus.InvalidDeleteCondition;
            }

            if (inputData.UserId <= 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidUserId;
            }

            if (inputData.FilterId < 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidFilterId;
            }

            if (!(inputData.SearchType >= 0 && inputData.SearchType <= 2))
            {
                return GetMedicalExaminationHistoryStatus.InvalidSearchType;
            }

            if ((inputData.SearchType != 0 && !(inputData.SearchCategory >= 1 && inputData.SearchType <= 3)) || (inputData.SearchType == 0 && inputData.SearchCategory != 0))
            {
                return GetMedicalExaminationHistoryStatus.InvalidSearchCategory;
            }
            if (string.IsNullOrEmpty(inputData.SearchText.Trim()) && inputData.SearchType != 0)
            {
                return GetMedicalExaminationHistoryStatus.InvalidSearchText;
            }

            return GetMedicalExaminationHistoryStatus.Successed;
        }

        /// <summary>
        /// Excute Order
        /// </summary>
        /// <param name="insuranceData"></param>
        /// <param name="allOdrInfs"></param>
        /// <param name="historyKarteOdrRaiin"></param>
        /// <param name="historyKarteOdrRaiins"></param>
        private static void ExcuteOrder(List<InsuranceModel> insuranceData, List<OrdInfModel> orderInfList, HistoryKarteOdrRaiinItem historyKarteOdrRaiin, List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiins)
        {
            var odrInfListByRaiinNo = orderInfList.OrderBy(odr => odr.OdrKouiKbn)
                                      .ThenBy(odr => odr.RpNo)
                                      .ThenBy(odr => odr.RpEdaNo)
                                      .ThenBy(odr => odr.SortNo)
                                      .ToList();

            // Find By Hoken
            List<int> hokenPidList = odrInfListByRaiinNo.GroupBy(odr => odr.HokenPid).Select(grp => grp.Key).ToList();

            foreach (var hokenPid in hokenPidList)
            {
                var hoken = insuranceData?.FirstOrDefault(c => c.HokenInf.HokenId == hokenPid);
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

                foreach (var groupOdrInf in groupOdrInfList)
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
                    foreach (var rpOdrInf in rpOdrInfs.OrderBy(c => c.IsDeleted))
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
                                                            rpOdrInf.OrdInfDetails.OrderBy(o => o.RowNo).Select(od =>
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
                                                                    od.Kasan2,
                                                                    od.CnvUnitName,
                                                                    od.OdrUnitName
                                                            )
                                                            ).ToList(),
                                                            rpOdrInf.CreateDate,
                                                            rpOdrInf.CreateId,
                                                            rpOdrInf.CreateName,
                                                            rpOdrInf.UpdateDate,
                                                            rpOdrInf.IsDeleted
                                                         );

                        group.OdrInfs.Add(odrModel);
                    }
                    hokenGrp.GroupOdrItems.Add(group);
                }
            }
            historyKarteOdrRaiins.Add(historyKarteOdrRaiin);
        }
    }
}
