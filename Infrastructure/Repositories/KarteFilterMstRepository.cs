using Domain.Models.KarteFilterMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KarteFilterMstRepository : IKarteFilterMstRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantData;

    public KarteFilterMstRepository(ITenantProvider tenantDataContext)
    {
        _tenantNoTrackingDataContext = tenantDataContext.GetNoTrackingDataContext();
        _tenantData = tenantDataContext.GetTrackingTenantDataContext();
    }

    public List<KarteFilterMstModel> GetList(int hpId, int userId)
    {
        var karteMstList = _tenantData.KarteFilterMsts
                          .Where(u => u.HpId == hpId && u.UserId == userId && u.IsDeleted != 1)
                          .ToList();

        var filterMstIdList = karteMstList.Select(k => k.FilterId).ToList();

        var filterDetailList = _tenantData.KarteFilterDetails
            .Where(item => item.HpId == hpId && item.UserId == userId && filterMstIdList.Contains(item.FilterId))
            .ToList();

        List<KarteFilterMstModel> result = new();
        foreach (var karteMst in karteMstList)
        {
            var filterId = karteMst.FilterId;
            var isBookMarkChecked = filterDetailList.FirstOrDefault(detail => detail.FilterId == filterId && detail.FilterItemCd == 1) != null;
            var listHokenId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 3).Select(item => item.FilterEdaNo).ToList();
            var listKaId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 4).Select(item => item.FilterEdaNo).ToList();
            var listUserId = filterDetailList.Where(detail => detail.FilterId == filterId && detail.FilterItemCd == 2).Select(item => item.FilterEdaNo).ToList();

            var detailModel = new KarteFilterDetailModel(hpId, userId, filterId, isBookMarkChecked, listHokenId, listKaId, listUserId);

            result.Add(new KarteFilterMstModel(
                karteMst.HpId,
                karteMst.UserId,
                karteMst.FilterId,
                karteMst.FilterName,
                karteMst.SortNo,
                karteMst.AutoApply,
                karteMst.IsDeleted,
                detailModel));
        }
        return result;
    }

    public bool SaveList(List<KarteFilterMstModel> karteFilterMstModels, int userId, int hpId)
    {
        bool status = false;
        var executionStrategy = _tenantData.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using (var transaction = _tenantData.Database.BeginTransaction())
                {
                    try
                    {
                        // List Save New KarteFilter
                        var listKarteFilterMstModelToAdd = karteFilterMstModels
                            .Where(mst => mst.FilterId == 0 && mst.IsDeleted != 1)
                            .ToList();

                        var listKarteFilterDetails = _tenantData.KarteFilterDetails.AsNoTracking().Where(detail => detail.HpId == hpId && detail.UserId == userId).ToList();
                        var listBookMarkChecked = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 1).ToList();
                        var listOldKaId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 4).ToList();
                        var listOldUserId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 2).ToList();
                        var listOldHokenId = listKarteFilterDetails.Where(detail =>detail.FilterItemCd == 3).ToList();

                        if (listKarteFilterMstModelToAdd.Any())
                        {
                            var listKarteFilterMstToAdd = new List<KarteFilterMst>();

                            var dictionaryKarteFilterMstMap = new Dictionary<KarteFilterMstModel, KarteFilterMst>();

                            // Map Dictionary
                            foreach (var mst in listKarteFilterMstModelToAdd)
                            {
                                KarteFilterMst karteFilterMst = new KarteFilterMst()
                                {
                                    HpId = mst.HpId,
                                    UserId = mst.UserId,
                                    FilterId = mst.FilterId,
                                    FilterName = mst.FilterName,
                                    SortNo = mst.SortNo,
                                    AutoApply = mst.AutoApply,
                                    IsDeleted = mst.IsDeleted,
                                    CreateDate = DateTime.UtcNow,
                                    CreateId = userId,
                                    UpdateDate = DateTime.UtcNow,
                                    UpdateId = userId
                                };
                                dictionaryKarteFilterMstMap.Add(mst, karteFilterMst);
                                listKarteFilterMstToAdd.Add(karteFilterMst);
                            }

                            // Save List KarteFilterMst
                            _tenantData.KarteFilterMsts.AddRange(listKarteFilterMstToAdd);
                            _tenantData.SaveChanges();

                            // Save KarteFilterDetail
                            foreach (var model in listKarteFilterMstModelToAdd)
                            {
                                var newKarteFilterMstModel = new KarteFilterMstModel(
                                        model.HpId,
                                        model.UserId,
                                        dictionaryKarteFilterMstMap[model].FilterId,
                                        model.FilterName,
                                        model.SortNo,
                                        model.AutoApply,
                                        model.IsDeleted,
                                        model.KarteFilterDetailModel
                                    );
                                SaveKarteFilterDetail(newKarteFilterMstModel, listBookMarkChecked, listOldKaId, listOldUserId, listOldHokenId);
                            }
                        }

                        // List Update KarteFilter
                        var listKarteFilterMstToUpdate = karteFilterMstModels
                            .Where(mst => mst.FilterId != 0)
                            .ToList();
                        if (listKarteFilterMstToUpdate.Any())
                        {
                            // Update KarteFilterMst
                            foreach (var item in listKarteFilterMstToUpdate)
                            {
                                var karteFilterMst = _tenantData.KarteFilterMsts.FirstOrDefault(mst => mst.HpId == item.HpId && mst.UserId == item.UserId && mst.FilterId == item.FilterId);
                                if (karteFilterMst != null)
                                {
                                    karteFilterMst.UpdateDate = DateTime.UtcNow;
                                    karteFilterMst.UpdateId = userId;
                                    karteFilterMst.FilterId = item.FilterId;
                                    karteFilterMst.FilterName = item.FilterName;
                                    karteFilterMst.AutoApply = item.AutoApply;
                                    karteFilterMst.IsDeleted = item.IsDeleted;
                                    karteFilterMst.SortNo = item.SortNo;
                                }
                            }

                            // Save KarteFilterDetail
                            foreach (var item in listKarteFilterMstToUpdate)
                            {
                                SaveKarteFilterDetail(item, listBookMarkChecked, listOldKaId, listOldUserId, listOldHokenId);
                            }
                        }
                        _tenantData.SaveChanges();

                        status = true;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        status = false;
                        transaction.Rollback();
                    }
                }
            });
        return status;
    }

    private KarteFilterDetail ConvertKarteFilterDetail(int hpId, int userId, long filterId, int filterItemCd, int filterEdaNo)
    {
        return new KarteFilterDetail()
        {
            HpId = hpId,
            UserId = userId,
            FilterId = filterId,
            FilterItemCd = filterItemCd,
            FilterEdaNo = filterEdaNo,
            Val = 1
        };
    }
    private void SaveKarteFilterDetail(KarteFilterMstModel item, List<KarteFilterDetail> listBookMarkChecked, List<KarteFilterDetail> listOldKaId, List<KarteFilterDetail> listOldUserId, List<KarteFilterDetail> listOldHokenId)
    {
        if (item.KarteFilterDetailModel != null && item.IsDeleted != 1)
        {
            listBookMarkChecked = listBookMarkChecked.Where(x => x.FilterId == item.FilterId).ToList();
            // BookMarkChecked
            if (listBookMarkChecked.Any() && !item.KarteFilterDetailModel.BookMarkChecked)
            {
                _tenantData.KarteFilterDetails.Remove(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 1, 0));
            }
            if (item.KarteFilterDetailModel.BookMarkChecked && !listBookMarkChecked.Any())
            {
                _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 1, 0));
            }

            // ListKaId 
            listOldKaId = listOldKaId.Where(x => x.FilterId == item.FilterId).ToList();
            _tenantData.KarteFilterDetails.RemoveRange(listOldKaId);
            if (item.KarteFilterDetailModel.ListKaId.Any())
            {
                foreach (var id in item.KarteFilterDetailModel.ListKaId.Distinct())
                {
                    _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 4, id));
                }
            }
            else
            {
                _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 4, 0));
            }

            // ListUserId 
            listOldUserId = listOldUserId.Where(x => x.FilterId == item.FilterId).ToList();
            _tenantData.KarteFilterDetails.RemoveRange(listOldUserId);
            if (item.KarteFilterDetailModel.ListUserId.Any())
            {
                foreach (var id in item.KarteFilterDetailModel.ListUserId.Distinct())
                {
                    _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 2, id));
                }
            }
            else
            {
                _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 2, 0));
            }

            // ListHokenId
            listOldHokenId = listOldHokenId.Where(x => x.FilterId == item.FilterId).ToList();
            _tenantData.KarteFilterDetails.RemoveRange(listOldHokenId);
            if (item.KarteFilterDetailModel.ListUserId.Any())
            {
                foreach (var id in item.KarteFilterDetailModel.ListHokenId.Distinct())
                {
                    _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 3, id));
                }
            }
            else
            {
                _tenantData.KarteFilterDetails.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 3, 0));
            }
        }
        if (item.IsDeleted == 1)
        {
            // Remove all KarteFilterDetail
            var listOldKarteFilterDetails = _tenantData.KarteFilterDetails.AsNoTracking().Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId).ToList();
            _tenantData.KarteFilterDetails.RemoveRange(listOldKarteFilterDetails);
        }
    }
}