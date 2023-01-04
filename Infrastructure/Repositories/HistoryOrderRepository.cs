using Domain.Models.HistoryOrder;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.KarteFilterMst;
using Domain.Models.KarteInf;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Converter;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Repositories
{
    public class HistoryOrderRepository : RepositoryBase, IHistoryOrderRepository
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IKaService _kaService;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IRaiinListTagRepository _raiinListTagRepository;
        private readonly IKarteInfRepository _karteInfRepository;

        public HistoryOrderRepository(ITenantProvider tenantProvider, IUserInfoService userInfoService, IKaService kaService, IInsuranceRepository insuranceRepository, IRaiinListTagRepository raiinListTagRepository, IKarteInfRepository karteInfRepository) : base(tenantProvider)
        {
            _userInfoService = userInfoService;
            _insuranceRepository = insuranceRepository;
            _raiinListTagRepository = raiinListTagRepository;
            _kaService = kaService;
            _karteInfRepository = karteInfRepository;
        }

        public KarteFilterMstModel GetFilter(int hpId, int userId, int filterId)
        {
            var filterMstData = NoTrackingDataContext.KarteFilterMsts.FirstOrDefault(u => u.HpId == hpId && u.UserId == userId && u.FilterId == filterId && u.IsDeleted == 0);
            if (filterMstData == null)
            {
                return new KarteFilterMstModel(hpId, userId);
            }

            var filterDetailList = NoTrackingDataContext.KarteFilterDetails
            .Where(item => item.HpId == hpId && item.UserId == userId && item.FilterId == filterId)
            .ToList();

            var isBookMarkChecked = filterDetailList.FirstOrDefault(detail => detail.FilterId == filterId && detail.FilterItemCd == 1) != null;
            var listHokenId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 3).Select(item => item.FilterEdaNo).ToList();
            var listKaId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 4).Select(item => item.FilterEdaNo).ToList();
            var listUserId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 2).Select(item => item.FilterEdaNo).ToList();

            var detailModel = new KarteFilterDetailModel(hpId, userId, filterId, isBookMarkChecked, listHokenId, listKaId, listUserId);

            var result = new KarteFilterMstModel(
                hpId,
                userId,
                filterId,
                filterMstData.FilterName ?? string.Empty,
                filterMstData.SortNo,
                filterMstData.AutoApply,
                filterMstData.IsDeleted,
                detailModel);

            return result;
        }

        private IEnumerable<RaiinInf> GenerateRaiinListQuery(int hpId, int userId, long ptId, int filterId, int isDeleted)
        {
            KarteFilterMstModel karteFilter = GetFilter(hpId, userId, filterId);
            List<int> hokenPidListByCondition = GetHokenPidListByCondition(hpId, ptId, isDeleted, karteFilter);

            //Filter RaiinInf by condition.
            IQueryable<RaiinInf> raiinInfListQueryable = NoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId &&
                            r.PtId == ptId &&
                            r.Status >= 3 &&
                            (r.IsDeleted == DeleteTypes.None || isDeleted == 1 || (r.IsDeleted != DeleteTypes.Confirm && isDeleted == 2)) &&
                            hokenPidListByCondition.Contains(r.HokenPid) &&
                            (karteFilter.IsAllDepartment || karteFilter.ListDepartmentCode.Contains(r.KaId)) &&
                            (karteFilter.IsAllDoctor || karteFilter.ListDoctorCode.Contains(r.TantoId)));

            IEnumerable<RaiinInf> raiinInfEnumerable;
            if (karteFilter.OnlyBookmark)
            {
                raiinInfEnumerable = from raiinInf in raiinInfListQueryable
                                     join raiinTag in NoTrackingDataContext.RaiinListTags.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && r.TagNo != 0)
                                      on raiinInf.RaiinNo equals raiinTag.RaiinNo
                                     select raiinInf;
            }
            else
            {
                raiinInfEnumerable = raiinInfListQueryable.Select(r => r);
            }
            return raiinInfEnumerable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="userId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <param name="currentIndex"></param>
        /// <param name="filterId"></param>
        /// <param name="keyWord"></param>
        /// <param name="isDeleted"></param>
        /// <param name="searchType"></param>
        /// 0: order and karte
        /// 1: only karte
        /// 2: only order
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public (int, ReceptionModel) Search(int hpId, int userId, long ptId, int sinDate, int currentIndex, int filterId, int isDeleted, string keyWord, int searchType, bool isNext)
        {
            IEnumerable<RaiinInf> raiinInfEnumerable = GenerateRaiinListQuery(hpId, userId, ptId, filterId, isDeleted);
            List<long> raiinNoList;

            if (isNext)
            {
                raiinNoList = raiinInfEnumerable.OrderByDescending(r => r.SinDate)
                                                .OrderByDescending(r => r.RaiinNo)
                                                .Skip(currentIndex + 1)
                                                .Select(r => r.RaiinNo)
                                                .ToList();
            }
            else
            {
                raiinNoList = raiinInfEnumerable.OrderByDescending(r => r.SinDate)
                                                .OrderByDescending(r => r.RaiinNo)
                                                .Take(currentIndex)
                                                .Select(r => r.RaiinNo)
                                                .ToList();
            }

            (int, ReceptionModel) GenerateResult(long raiinNo)
            {
                RaiinInf? raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo);

                if (raiinInf == null)
                {
                    return (0, new ReceptionModel());
                }

                int index = 0;

                if (isNext)
                {
                    index = currentIndex + raiinNoList.IndexOf(raiinNo) + 1;
                }
                else
                {
                    index = Math.Max(0, currentIndex - (raiinNoList.Count - raiinNoList.IndexOf(raiinNo)));
                }
                return (index, Reception.FromRaiinInf(raiinInf));
            }

            long raiinNoByKarte = 0;
            long raiinNoByOrder = 0;
            if (searchType == 0 || searchType == 1)
            {
                raiinNoByKarte = SearchKarte(hpId, ptId, isDeleted, raiinNoList, keyWord, isNext);
            }

            if (searchType == 0 || searchType == 2)
            {
                raiinNoByOrder = SearchOrder(hpId, ptId, isDeleted, raiinNoList, keyWord, isNext);
            }

            if (raiinNoByKarte == 0 && raiinNoByOrder == 0)
            {
                return (0, new ReceptionModel());
            }

            if (raiinNoByKarte == 0)
            {
                return GenerateResult(raiinNoByOrder);
            }

            if (raiinNoByOrder == 0)
            {
                return GenerateResult(raiinNoByKarte);
            }

            long foundRaiinNo = isNext ? Math.Max(raiinNoByKarte, raiinNoByOrder) : Math.Min(raiinNoByKarte, raiinNoByOrder);

            return GenerateResult(foundRaiinNo);
        }

        public (int, List<HistoryOrderModel>) GetList(int hpId, int userId, long ptId, int sinDate, int offset, int limit, int filterId, int isDeleted)
        {
            IEnumerable<RaiinInf> raiinInfEnumerable = GenerateRaiinListQuery(hpId, userId, ptId, filterId, isDeleted);
            int totalCount = raiinInfEnumerable.Count();
            List<RaiinInf> raiinInfList = raiinInfEnumerable.OrderByDescending(r => r.SinDate).Skip(offset).Take(limit).ToList();

            if (!raiinInfList.Any())
            {
                return (0, new List<HistoryOrderModel>());
            }

            List<long> raiinNoList = raiinInfList.Select(r => r.RaiinNo).ToList();

            List<KarteInfModel> allKarteInfList = GetKarteInfList(hpId, ptId, isDeleted, raiinNoList);
            Dictionary<long, List<OrdInfModel>> allOrderInfList = GetOrderInfList(hpId, ptId, isDeleted, raiinNoList);
            List<InsuranceModel> insuranceModelList = _insuranceRepository.GetInsuranceList(hpId, ptId, sinDate, true);
            List<RaiinListTagModel> tagModelList = _raiinListTagRepository.GetList(hpId, ptId, raiinNoList);
            List<FileInfModel> listKarteFile = _karteInfRepository.GetListKarteFile(hpId, ptId, raiinNoList, isDeleted != 0);
            //List<ApproveInfModel> approveInfModelList = _ordInfRepository.GetApproveInf(hpId, ptId, true, raiinNoList).ToList();

            List<HistoryOrderModel> historyOrderModelList = new List<HistoryOrderModel>();
            foreach (long raiinNo in raiinNoList)
            {
                RaiinInf? raiinInf = raiinInfList.FirstOrDefault(r => r.RaiinNo == raiinNo);
                if (raiinInf == null)
                {
                    continue;
                }

                ReceptionModel receptionModel = Reception.FromRaiinInf(raiinInf);
                KarteInfModel karteInfModel = allKarteInfList.FirstOrDefault(r => r.RaiinNo == raiinNo) ?? new KarteInfModel(hpId, raiinNo);
                List<OrdInfModel> orderInfList = allOrderInfList[raiinNo];
                InsuranceModel insuranceModel = insuranceModelList.FirstOrDefault(i => i.HokenPid == raiinInf.HokenPid) ?? new InsuranceModel();
                RaiinListTagModel tagModel = tagModelList.FirstOrDefault(t => t.RaiinNo == raiinNo) ?? new RaiinListTagModel();
                List<FileInfModel> listKarteFileModel = listKarteFile.Where(item => item.RaiinNo == raiinNo).ToList();
                //ApproveInfModel approveInfModel = approveInfModelList.FirstOrDefault(a => a.RaiinNo == raiinNo) ?? new ApproveInfModel();
                string tantoName = _userInfoService.GetNameById(raiinInf.TantoId);
                string kaName = _kaService.GetNameById(raiinInf.KaId);

                historyOrderModelList.Add(new HistoryOrderModel(receptionModel, insuranceModel, orderInfList, karteInfModel, kaName, tantoName, tagModel.TagNo, string.Empty, listKarteFileModel));
            }

            return (totalCount, historyOrderModelList);
        }

        public bool CheckExistedFilter(int hpId, int userId, int filterId)
        {
            return NoTrackingDataContext.KarteFilterMsts.Any(u => u.HpId == hpId && u.UserId == userId && u.FilterId == filterId && u.IsDeleted == 0);
        }

        #region private method
        private long SearchKarte(int hpId, long ptId, int isDeleted, List<long> raiinNoList, string keyWord, bool isNext)
        {
            var karteInfEntities = NoTrackingDataContext.KarteInfs
                .Where(k => k.PtId == ptId && 
                            k.HpId == hpId &&
                            k.Text != null &&
                            k.Text.Contains(keyWord) &&
                            raiinNoList.Contains(k.RaiinNo) &&
                            (
                                (k.IsDeleted == DeleteTypes.None) ||
                                (k.IsDeleted == DeleteTypes.Deleted && (isDeleted == 1 || isDeleted == 2)) ||
                                (k.IsDeleted == DeleteTypes.Confirm && isDeleted == 2)
                            )
                       ).AsEnumerable();

            if (isNext)
            {
                karteInfEntities = karteInfEntities.OrderByDescending(k => k.SinDate).OrderByDescending(k => k.RaiinNo);
            }
            else
            {
                karteInfEntities = karteInfEntities.OrderBy(k => k.SinDate).ThenBy(k => k.RaiinNo);
            }

            KarteInf? karteInf = karteInfEntities.FirstOrDefault();

            return karteInf == null ? 0 : karteInf.RaiinNo;
        }

        private long SearchOrder(int hpId, long ptId, int isDeleted, List<long> raiinNoList, string keyWord, bool isNext)
        {
            List<OdrInf> allOdrInfList = NoTrackingDataContext.OdrInfs
                .Where(o => o.HpId == hpId &&
                            o.PtId == ptId &&
                            o.OdrKouiKbn != 10 &&
                            raiinNoList.Contains(o.RaiinNo) &&
                            (
                                (o.IsDeleted == DeleteTypes.None) ||
                                (o.IsDeleted == DeleteTypes.Deleted && (isDeleted == 1 || isDeleted == 2)) ||
                                (o.IsDeleted == DeleteTypes.Confirm && isDeleted == 2)
                            )
                      )
                .ToList();

            List<long> raiinNoListByOrder = allOdrInfList.Select(o => o.RaiinNo).Distinct().ToList();
            List<long> rpNoListByOrder = allOdrInfList.Select(o => o.RpNo).Distinct().ToList();
            List<long> rpEdaNoListByOrder = allOdrInfList.Select(o => o.RpEdaNo).Distinct().ToList();

            var allOdrDetailInfList = NoTrackingDataContext.OdrInfDetails
                .Where(o => o.HpId == hpId &&
                            o.PtId == ptId &&
                            raiinNoListByOrder.Contains(o.RaiinNo) &&
                            rpNoListByOrder.Contains(o.RpNo) &&
                            rpEdaNoListByOrder.Contains(o.RpEdaNo) &&
                            o.ItemName != null &&
                            o.ItemName.Contains(keyWord));

            if (isNext)
            {
                allOdrDetailInfList = allOdrDetailInfList.OrderByDescending(o => o.SinDate).OrderByDescending(o => o.RaiinNo);
            }
            else
            {
                allOdrDetailInfList = allOdrDetailInfList.OrderBy(o => o.SinDate).ThenBy(o => o.RaiinNo);
            }

            OdrInfDetail? odrInfDetail = allOdrDetailInfList.FirstOrDefault();

            return odrInfDetail == null ? 0 : odrInfDetail.RaiinNo;
        }

        private List<KarteInfModel> GetKarteInfList(int hpId, long ptId, int isDeleted, List<long> raiinNoList)
        {
            var karteInfEntities = NoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.HpId == hpId && raiinNoList.Contains(k.RaiinNo) && k.KarteKbn == 1).AsEnumerable();

            if (isDeleted == 0)
            {
                karteInfEntities = karteInfEntities.Where(r => r.IsDeleted == DeleteTypes.None);
            }
            else if (isDeleted == 1)
            {
                karteInfEntities = karteInfEntities.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted);
            }
            else
            {
                karteInfEntities = karteInfEntities.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted || r.IsDeleted == DeleteTypes.Confirm);
            }

            if (karteInfEntities == null)
            {
                return new List<KarteInfModel>();
            }

            var karteInfs = from karte in karteInfEntities
                            join user in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId)
                          on karte.CreateId equals user.UserId into odrUsers
                            from odrUser in odrUsers.DefaultIfEmpty()
                            select Karte.FromKarte(karte, odrUser?.Sname ?? string.Empty);

            return karteInfs.ToList();
        }

        private Dictionary<long, List<OrdInfModel>> GetOrderInfList(int hpId, long ptId, int isDeleted, List<long> raiinNoList)
        {
            List<OdrInf> allOdrInfList = NoTrackingDataContext.OdrInfs
                .Where(o => o.HpId == hpId &&
                            o.PtId == ptId &&
                            o.OdrKouiKbn != 10 &&
                            raiinNoList.Contains(o.RaiinNo) &&
                            (
                                (o.IsDeleted == DeleteTypes.None) ||
                                (o.IsDeleted == DeleteTypes.Deleted && (isDeleted == 1 || isDeleted == 2)) ||
                                (o.IsDeleted == DeleteTypes.Confirm && isDeleted == 2)
                            )
                      )
                .ToList();

            if (!allOdrInfList.Any())
            {
                return new Dictionary<long, List<OrdInfModel>>();
            }

            List<long> raiinNoListByOrder = allOdrInfList.Select(o => o.RaiinNo).Distinct().ToList();
            List<long> rpNoListByOrder = allOdrInfList.Select(o => o.RpNo).Distinct().ToList();
            List<long> rpEdaNoListByOrder = allOdrInfList.Select(o => o.RpEdaNo).Distinct().ToList();

            List<OdrInfDetail> allOdrDetailInfList = NoTrackingDataContext.OdrInfDetails
                .Where(o => o.HpId == hpId && o.PtId == ptId && raiinNoListByOrder.Contains(o.RaiinNo) && rpNoListByOrder.Contains(o.RpNo) && rpEdaNoListByOrder.Contains(o.RpEdaNo))
                .ToList();

            if (!allOdrDetailInfList.Any())
            {
                return new Dictionary<long, List<OrdInfModel>>();
            }

            int minSinDate = allOdrDetailInfList.Min(o => o.SinDate);
            int maxSinDate = allOdrDetailInfList.Max(o => o.SinDate);

            //Read config
            var itemCds = allOdrDetailInfList.Select(od => od.ItemCd).Distinct().ToList();
            var ipnCds = allOdrDetailInfList.Select(od => od.IpnCd).Distinct().ToList();
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.StartDate <= minSinDate && t.EndDate >= maxSinDate && itemCds.Contains(t.ItemCd)).ToList();
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(t => t.HpId == hpId).ToList();
            var ipnNameMsts = NoTrackingDataContext.IpnNameMsts.Where(ipn => ipn.HpId == hpId && ipnCds.Contains(ipn.IpnNameCd) && ipn.StartDate <= minSinDate && ipn.EndDate >= maxSinDate).ToList();

            Dictionary<long, List<OrdInfModel>> result = new Dictionary<long, List<OrdInfModel>>();
            foreach (long raiinNo in raiinNoList)
            {
                List<OdrInf> odrInfList = allOdrInfList.Where(o => o.RaiinNo == raiinNo).ToList();
                List<OrdInfModel> odrInfModelList = new List<OrdInfModel>();
                foreach (OdrInf odrInf in odrInfList)
                {
                    string createName = _userInfoService.GetNameById(odrInf.CreateId);
                    string updateName = _userInfoService.GetNameById(odrInf.UpdateId);

                    List<OdrInfDetail> odrDetailInfList = allOdrDetailInfList.Where(o => o.RaiinNo == raiinNo && o.RpNo == odrInf.RpNo && o.RpEdaNo == odrInf.RpEdaNo).ToList();


                    OrdInfModel ordInfModel = Order.CreateBy(odrInf, odrDetailInfList, tenMsts, kensaMsts, ipnNameMsts, createName, updateName);
                    odrInfModelList.Add(ordInfModel);
                }

                result.Add(raiinNo, odrInfModelList);
            }

            return result;
        }


        private List<int> GetHokenPidListByCondition(int hpId, long ptId, int isDeleted, KarteFilterMstModel karteFilter)
        {
            bool isAllHoken = karteFilter.IsAllHoken;
            bool isHoken = karteFilter.IsHoken;
            bool isJihi = karteFilter.IsJihi;
            bool isRosai = karteFilter.IsRosai;
            bool isJibai = karteFilter.IsJibai;

            return NoTrackingDataContext.PtHokenPatterns
                .Where(p => p.HpId == hpId &&
                            p.PtId == ptId &&
                            (p.IsDeleted == 0 || isDeleted > 0) &&
                            (
                               isAllHoken ||
                               isHoken && (p.HokenKbn == 1 || p.HokenKbn == 2) ||
                               isJihi && p.HokenKbn == 0 ||
                               isRosai && (p.HokenKbn == 11 || p.HokenKbn == 12 || p.HokenKbn == 13) ||
                               isJibai && p.HokenKbn == 14
                            ))
                .Select(p => p.HokenPid)
                .ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        #endregion
    }
}
