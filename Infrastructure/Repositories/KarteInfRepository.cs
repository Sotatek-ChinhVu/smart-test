using Domain.Models.KarteInfs;
using Entity.Tenant;
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

        public List<KarteInfModel> GetList(long ptId, int hpId)
        {
            var karteInfEntity = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId).ToList();

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

        public bool SaveImageKarteImgTemp(KarteImgInfModel model)
        {
            bool status = false;
            try
            {
                var karteImgInf = _tenantTrackingDataContext.KarteImgInfs.FirstOrDefault(item => item.HpId == model.HpId && item.RaiinNo == 0 && item.PtId == model.PtId && item.FileName.Equals(model.OldFileName));

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
