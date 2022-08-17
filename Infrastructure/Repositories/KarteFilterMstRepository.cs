using Domain.Models.KarteFilterMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
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
        var result = _tenantNoTrackingDataContext.KarteFilterMsts
                          .Where(u => u.HpId == hpId && u.UserId == userId && u.IsDeleted != 1)
                          .AsEnumerable()
                          .Select(u => new KarteFilterMstModel(
                                 u.HpId,
                                 u.UserId,
                                 u.FilterId,
                                 u.FilterName,
                                 u.SortNo,
                                 u.AutoApply,
                                 u.IsDeleted,
                                 GetKarteFilterDetailModel(u.HpId, u.UserId, u.FilterId)
                              )).ToList();
        return result;
    }

    public bool SaveList(List<KarteFilterMstModel> karteFilterMstModels, int userId)
    {
        bool status = false;
        try
        {
            #region Save KarteFilterMst

            // List Save New KarteFilter
            var listKarteFilterMstModelToAdd = karteFilterMstModels
                .Where(mst => mst.FilterId == 0 && mst.IsDeleted != 1)
                .ToList();

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
                    SaveKarteFilterDetail(newKarteFilterMstModel);
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
                    SaveKarteFilterDetail(item);
                }
            }
            #endregion

            _tenantData.SaveChanges();
            _tenantNoTrackingDataContext.SaveChanges();

            status = true;
        }
        catch
        {
            status = false;
        }
        return status;
    }

    private KarteFilterDetailModel GetKarteFilterDetailModel(int hpId, int userId, long filterId)
    {
        var listKarteFilterDetails = _tenantNoTrackingDataContext.KarteFilterDetails.Where(item => item.HpId == hpId && item.UserId == userId && item.FilterId == filterId).ToList();
        var isBookMarkChecked = listKarteFilterDetails.FirstOrDefault(detail => detail.FilterItemCd == 1) != null ? true : false;
        var listHokenId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 3).Select(item => item.FilterEdaNo).ToList();
        var listKaId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 4).Select(item => item.FilterEdaNo).ToList();
        var listUserId = listKarteFilterDetails.Where(detail => detail.FilterItemCd == 2).Select(item => item.FilterEdaNo).ToList();
        return new KarteFilterDetailModel(hpId, userId, filterId, isBookMarkChecked, listHokenId, listKaId, listUserId);
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

    private void SaveKarteFilterDetail(KarteFilterMstModel item)
    {
        if (item.KarteFilterDetailModel != null && item.IsDeleted != 1)
        {
            // BookMarkChecked
            var listBookMarkChecked = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 1).ToList();
            if (listBookMarkChecked.Any() && !item.KarteFilterDetailModel.BookMarkChecked)
            {
                _tenantNoTrackingDataContext.Remove(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 1, 0));
            }
            if (item.KarteFilterDetailModel.BookMarkChecked && !listBookMarkChecked.Any())
            {
                _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 1, 0));
            }

            // ListKaId 
            if (item.KarteFilterDetailModel.ListKaId.Any())
            {
                var listOldKaId = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 4).ToList();
                _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldKaId);
                foreach (var id in item.KarteFilterDetailModel.ListKaId)
                {
                    _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 4, id));
                }
            }
            else
            {
                var listOldKaId = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 4).ToList();
                _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldKaId);
                _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 4, 0));
            }

            // ListUserId 
            if (item.KarteFilterDetailModel.ListUserId.Any())
            {
                var listOldUserId = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 2).ToList();
                _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldUserId);

                foreach (var id in item.KarteFilterDetailModel.ListUserId)
                {
                    _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 2, id));
                }
            }
            else
            {
                var listOldUserId = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 2).ToList();
                _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldUserId);
                _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 2, 0));
            }

            // ListHokenId
            if (item.KarteFilterDetailModel.ListUserId.Any())
            {
                var listOldHokenId = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 3).ToList();
                _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldHokenId);
                foreach (var id in item.KarteFilterDetailModel.ListHokenId)
                {
                    _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 3, id));
                }
            }
            else
            {
                var listOldUserId = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId && detail.FilterItemCd == 3).ToList();
                _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldUserId);
                _tenantNoTrackingDataContext.Add(ConvertKarteFilterDetail(item.HpId, item.UserId, item.FilterId, 3, 0));
            }
        }
        if (item.IsDeleted == 1)
        {
            // Remove all KarteFilterDetail
            var listOldKarteFilterDetails = _tenantNoTrackingDataContext.KarteFilterDetails.Where(detail => detail.HpId == item.HpId && detail.UserId == item.UserId && detail.FilterId == item.FilterId).ToList();
            _tenantNoTrackingDataContext.KarteFilterDetails.RemoveRange(listOldKarteFilterDetails);
        }
    }
}