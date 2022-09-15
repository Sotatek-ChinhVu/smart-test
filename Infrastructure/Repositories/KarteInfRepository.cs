using Domain.Models.KarteInfs;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KarteInfRepository : IKarteInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;

        public KarteInfRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted)
        {
            var karteInfEntity = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.RaiinNo == rainNo && k.SinDate == sinDate && (isDeleted || k.IsDeleted == 0)).ToList();

            if (karteInfEntity == null)
            {
                return new List<KarteInfModel>();
            }
            return karteInfEntity.Select(k => ConvertToModel(k)).ToList();
        }

        public List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos)
        {
            var karteInfEntity = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.HpId == hpId && raiinNos.Contains(k.RaiinNo)).AsEnumerable();

            if (deleteCondition == 0)
            {
                karteInfEntity = karteInfEntity.Where(r => r.IsDeleted == DeleteTypes.None);
            }
            else if (deleteCondition == 1)
            {
                karteInfEntity = karteInfEntity.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted);
            }
            else
            {
                karteInfEntity = karteInfEntity.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted || r.IsDeleted == DeleteTypes.Confirm);
            }

            if (karteInfEntity == null)
            {
                return new List<KarteInfModel>();
            }
            return karteInfEntity.Select(k => ConvertToModel(k)).ToList();
        }

        private KarteInfModel ConvertToModel(KarteInf itemData)
        {
            return new KarteInfModel(
                itemData.HpId,
                itemData.RaiinNo,
                itemData.KarteKbn,
                itemData.SeqNo,
                itemData.PtId,
                itemData.SinDate,
                itemData.Text,
                itemData.IsDeleted,
                itemData.RichText == null ? string.Empty : Encoding.UTF8.GetString(itemData.RichText),
                itemData.CreateDate,
                itemData.UpdateDate
                );
        }

        public bool SaveListImageKarteImgTemp(List<KarteImgInfModel> listModel)
        {
            bool status = false;
            try
            {
                var hpId = listModel.FirstOrDefault()?.HpId;
                var ptId = listModel.FirstOrDefault()?.PtId;
                var listRaiinNo = listModel.Select(item => item.RaiinNo).ToList();
                var listOldFileName = listModel.Select(item => item.OldFileName).ToList();
                var listKarteImgInfs = _tenantTrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId && item.PtId == ptId && listRaiinNo.Contains(item.RaiinNo) && listOldFileName.Contains(item.FileName)).ToList();

                foreach (var model in listModel)
                {
                    var karteImgInf = listKarteImgInfs.FirstOrDefault(item => item.RaiinNo == model.RaiinNo && item.FileName.Equals(model.OldFileName));
                    if (karteImgInf == null)
                    {
                        karteImgInf = new KarteImgInf();
                        karteImgInf.HpId = model.HpId;
                        karteImgInf.RaiinNo = model.RaiinNo;
                        karteImgInf.FileName = model.FileName;
                        karteImgInf.PtId = model.PtId;
                        _tenantTrackingDataContext.KarteImgInfs.Add(karteImgInf);
                    }
                    else
                    {
                        if (model.FileName != String.Empty)
                        {
                            karteImgInf.RaiinNo = model.RaiinNo;
                            karteImgInf.FileName = model.FileName;
                        }
                        else
                        {
                            _tenantTrackingDataContext.KarteImgInfs.Remove(karteImgInf);
                        }
                    }
                }
                _tenantTrackingDataContext.SaveChanges();
                status = true;
                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }
    }
}
