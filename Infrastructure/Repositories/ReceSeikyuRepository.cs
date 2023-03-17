using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Mapping;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReceSeikyuRepository : RepositoryBase, IReceSeikyuRepository
    {
        public ReceSeikyuRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm, bool isIncludingUnConfirmed, long ptNumSearch, bool noFilter, bool isFilterMonthlyDelay, bool isFilterReturn, bool isFilterOnlineReturn)
        {
            var ptInfo = NoTrackingDataContext.PtInfs.Where(u => u.HpId == hpId &&
                                                                                u.IsDelete == 0);

            var listReceSeikyu = NoTrackingDataContext.ReceSeikyus.Where(u => u.HpId == hpId &&
                                                                                       u.IsDeleted == 0);

            var ptHoken = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId && u.IsDeleted == 0);


            var listRecedenHenjiyuu = NoTrackingDataContext.RecedenHenJiyuus.Where(u => u.HpId == hpId &&
                                                                                                 u.IsDeleted == 0);

            var recedenHenjiyuuInfo = from recedenHenjiyuu in listRecedenHenjiyuu
                                      join ptHokenInf in ptHoken on
                                           new { recedenHenjiyuu.PtId, recedenHenjiyuu.HokenId } equals
                                           new { ptHokenInf.PtId, ptHokenInf.HokenId } into lj
                                      from item in lj.DefaultIfEmpty()
                                      select new
                                      {
                                          RecedenHenJiyuu = recedenHenjiyuu,
                                          PtHokenInfItem = item
                                      };

            var receSeikyuInf = from receSeikyu in listReceSeikyu
                                join ptHokenInf in ptHoken on
                                  new { receSeikyu.PtId, receSeikyu.HokenId } equals
                                  new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenList
                                from item in ptHokenList.DefaultIfEmpty()
                                join ptInf in ptInfo on receSeikyu.PtId equals ptInf.PtId
                                where receSeikyu.IsDeleted == DeleteTypes.None
                                      && !(!isIncludingUnConfirmed && receSeikyu.SeikyuYm == 999999)
                                      && !(sinYm > 0 && receSeikyu.SeikyuYm != 999999 && receSeikyu.SeikyuYm != sinYm)

                                      && (ptNumSearch == 0 || ptInf.PtNum == ptNumSearch)
                                      && (noFilter ||
                                            (isFilterMonthlyDelay && receSeikyu.SeikyuKbn == 1) ||
                                            (isFilterReturn && receSeikyu.SeikyuKbn == 2) ||
                                            (isFilterOnlineReturn && receSeikyu.SeikyuKbn == 3)
                                          )
                                select new
                                {
                                    PtInfo = ptInf,
                                    ReceSeikyu = receSeikyu,
                                    PtHokenInfItem = item ?? new PtHokenInf()
                                };

            var query = from receSeikyu in receSeikyuInf
                        select new
                        {
                            recedenHenjiyuuList = recedenHenjiyuuInfo.Where(x => x.RecedenHenJiyuu.PtId == receSeikyu.ReceSeikyu.PtId
                                                                                && x.RecedenHenJiyuu.SinYm == receSeikyu.ReceSeikyu.SinYm
                                                                                && x.RecedenHenJiyuu.HokenId == receSeikyu.ReceSeikyu.HokenId).AsEnumerable(),
                            ReceSeikyuPending = receSeikyu,
                        };

            return query.AsEnumerable().Select(x => new ReceSeikyuModel(sinDate,
                                                          x.ReceSeikyuPending.PtInfo.HpId,
                                                          x.ReceSeikyuPending.PtInfo.PtId,
                                                          x.ReceSeikyuPending.PtInfo.Name ?? string.Empty,
                                                          x.ReceSeikyuPending.ReceSeikyu.SinYm,
                                                          sinYm,
                                                          x.ReceSeikyuPending.PtHokenInfItem.HokenId,
                                                          x.ReceSeikyuPending.PtHokenInfItem.HokensyaNo ?? string.Empty,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeqNo,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuYm,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuKbn,
                                                          x.ReceSeikyuPending.ReceSeikyu.PreHokenId,
                                                          x.ReceSeikyuPending.ReceSeikyu.Cmt ?? string.Empty,
                                                          x.ReceSeikyuPending.PtInfo.PtNum,
                                                          x.ReceSeikyuPending.PtHokenInfItem.HokenKbn,
                                                          x.ReceSeikyuPending.PtHokenInfItem.Houbetu ?? string.Empty,
                                                          x.ReceSeikyuPending.PtHokenInfItem.StartDate,
                                                          x.ReceSeikyuPending.PtHokenInfItem.EndDate,
                                                          false,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuYm,
                                                          x.ReceSeikyuPending.ReceSeikyu.SinYm,
                                                          false,
                                                          DeleteTypes.None,
                                                          x.ReceSeikyuPending.ReceSeikyu.SeikyuYm != 999999,
                                                          x.recedenHenjiyuuList.Select(m => new RecedenHenJiyuuModel(hpId,
                                                                                                                    m.RecedenHenJiyuu.PtId,
                                                                                                                    m.PtHokenInfItem.HokenId,
                                                                                                                    sinYm,
                                                                                                                    m.RecedenHenJiyuu.SeqNo,
                                                                                                                    m.RecedenHenJiyuu.HenreiJiyuuCd ?? string.Empty,
                                                                                                                    m.RecedenHenJiyuu.HenreiJiyuu ?? string.Empty,
                                                                                                                    m.RecedenHenJiyuu.Hosoku ?? string.Empty,
                                                                                                                    0,
                                                                                                                    m.PtHokenInfItem.HokenKbn,
                                                                                                                    m.PtHokenInfItem.Houbetu ?? string.Empty,
                                                                                                                    m.PtHokenInfItem.SikakuDate,
                                                                                                                    m.PtHokenInfItem.EndDate,
                                                                                                                    m.PtHokenInfItem.HokensyaNo ?? string.Empty)).ToList()
                                                          )).OrderByDescending(o => o.SeikyuKbn).ThenBy(u => u.SinYm).ThenBy(i => i.PtNum).ToList();
        }

        public List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int seikyuYm, List<long> ptIdList)
        {
            List<ReceSeikyuModel> result = new();
            var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                    && item.IsDelete == DeleteTypes.None
                                                                    && (ptIdList.Count <= 0 || ptIdList.Contains(item.PtId)))
                                                     .ToList();

            ptIdList = ptInfList.Select(item => item.PtId).Distinct().ToList();
            var receSeikyus = NoTrackingDataContext.ReceSeikyus.Where(item => item.HpId == hpId
                                                                              && item.SeikyuYm == seikyuYm
                                                                              && item.IsDeleted == DeleteTypes.None
                                                                              && ptIdList.Contains(item.PtId))
                                                                .ToList();

            foreach (var ptInf in ptInfList)
            {
                var receSeikyu = receSeikyus.FirstOrDefault(item => item.PtId == ptInf.PtId);
                if (receSeikyu == null)
                {
                    continue;
                }
                result.Add(new ReceSeikyuModel(
                               ptInf.PtId,
                               receSeikyu?.SinYm ?? 0,
                               receSeikyu?.HokenId ?? 0,
                               ptInf.PtNum,
                               receSeikyu?.SeikyuKbn ?? 0
                          ));
            }
            return result;
        }

        
        public IEnumerable<RegisterSeikyuModel> SearchReceInf(int hpId, long ptNum, int sinYm)
        {
            PtInf? ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(u => u.HpId == hpId && u.PtNum == ptNum && u.IsDelete == 0);
            if (ptInf == null) return new List<RegisterSeikyuModel>();

            var listPtHokenInf = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId && u.PtId == ptInf.PtId && u.IsDeleted == 0);
            var listReceInf = NoTrackingDataContext.ReceInfs.Where(u => u.HpId == hpId && u.PtId == ptInf.PtId && (sinYm <= 0 || u.SinYm <= sinYm)).OrderBy(u => u.SinYm);
            var query = from receInf in listReceInf
                        join ptHoken in listPtHokenInf on receInf.HokenId equals ptHoken.HokenId
                        select new
                        {
                            ReceInf = receInf,
                            PtHokenInfo = ptHoken
                        };

            return query.AsEnumerable().Select(u => new RegisterSeikyuModel(ptInf.PtId, 
                                                                            ptInf.Name ?? string.Empty,
                                                                            u.ReceInf.SinYm,
                                                                            u.ReceInf.SeikyuYm,
                                                                            u.ReceInf.SeikyuKbn,
                                                                            0,
                                                                            u.ReceInf.HokenId,
                                                                            u.ReceInf.HokensyaNo ?? string.Empty,
                                                                            u.ReceInf.HokenKbn,
                                                                            u.ReceInf.Houbetu ?? string.Empty,
                                                                            u.ReceInf.HonkeKbn,
                                                                            u.PtHokenInfo.StartDate,
                                                                            u.PtHokenInfo.EndDate,
                                                                            false)).OrderByDescending(u => u.SeikyuYm).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public void EntryDeleteHenJiyuu(long ptId, int sinYm, int preHokenId, int userId)
        {
            var henJiyuuList = TrackingDataContext.RecedenHenJiyuus.Where(item => item.HpId == Session.HospitalID
                                                                              && item.PtId == ptId
                                                                              && item.SinYm == sinYm
                                                                              && (item.HokenId == preHokenId || preHokenId == 0)
                                                                              && item.IsDeleted == 0).ToList();
            foreach (var henJiyuu in henJiyuuList)
            {
                henJiyuu.IsDeleted = DeleteTypes.Deleted;
                henJiyuu.UpdateId = userId;
                henJiyuu.UpdateDate = CIUtil.GetJapanDateTimeNow();
            }
        }

        public bool InsertNewReceSeikyu(List<ReceSeikyuModel> listInsert, int userId , int hpId)
        {
            var addedList = listInsert.Select(item => Mapper.Map(item, new ReceSeikyu(), (src, dest) =>
            {
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.SeqNo = 0;
                dest.HpId = hpId;
                if (src.HpId == 0 && src.Cmt == "返戻ファイルより登録")
                {
                    dest.SeikyuKbn = 3;
                }
                return dest;
            })).ToList();
            TrackingDataContext.ReceSeikyus.AddRange(addedList);
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool SaveReceSeiKyu(int hpId, int userId , List<ReceSeikyuModel> data)
        {
            var addedList = data.FindAll(item => item.SeqNo == 0 && item.OriginSinYm != item.SinYm).Select(item => Mapper.Map(item , new ReceSeikyu(), (src,dest) =>
            {
                dest.CreateDate = CIUtil.GetJapanDateTimeNow();
                dest.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dest.CreateId = userId;
                dest.UpdateId = userId;
                dest.HpId = hpId;
                if (src.HpId == 0 && src.Cmt == "返戻ファイルより登録")
                {
                    dest.SeikyuKbn = 3;
                }
                return dest;
            })).ToList();
            TrackingDataContext.ReceSeikyus.AddRange(addedList);

            foreach(var item in data.Where(x => x.SeqNo != 0 && x.OriginSinYm == x.SinYm))
            {
                var update = TrackingDataContext.ReceSeikyus.FirstOrDefault(x => x.SeqNo == item.SeqNo);
                if(update != null)
                {
                    if(item.IsDeleted == DeleteTypes.Deleted)
                    {
                        update.IsDeleted = DeleteTypes.Deleted;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = userId;
                    }
                    else
                    {
                        update.SeikyuYm = item.SeikyuYm;
                        update.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        update.UpdateId = userId;
                        update.SeikyuKbn = item.SeikyuKbn;
                        update.PreHokenId = item.PreHokenId;
                        update.Cmt = item.Cmt;
                    }
                }
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public bool RemoveReceSeikyuDuplicateIfExist(long ptId, int sinYm, int hokenId, int userId, int hpId)
        {
            var model = TrackingDataContext.ReceSeikyus.FirstOrDefault(u => u.HpId == hpId &&
                                                                             u.PtId == ptId &&
                                                                             u.SinYm == sinYm &&
                                                                             u.HokenId == hokenId &&
                                                                             u.SeikyuYm != 999999 &&
                                                                             u.IsDeleted == 0);

            if (model is null)
                return true;
            else
            {
                model.IsDeleted = DeleteTypes.Deleted;
                return TrackingDataContext.SaveChanges() > 0;
            }
        }

        public bool UpdateSeikyuYmReceipSeikyuIfExist(long ptId, int sinYm, int hokenId, int seikyuYm, int userId, int hpId)
        {
            var model = TrackingDataContext.ReceSeikyus.FirstOrDefault(u => u.HpId == hpId &&
                                                                             u.PtId == ptId &&
                                                                             u.SinYm == sinYm &&
                                                                             u.HokenId == hokenId &&
                                                                             u.SeikyuYm == 999999 &&
                                                                             u.IsDeleted == 0);
            if (model is null)
                return true;
            else
            {
                model.SeikyuYm = seikyuYm;
                return TrackingDataContext.SaveChanges() > 0;
            }
        }
    }
}
