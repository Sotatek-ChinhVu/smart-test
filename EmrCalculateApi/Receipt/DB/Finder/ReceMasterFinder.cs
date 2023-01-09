using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Models;
using Infrastructure.Interfaces;

namespace EmrCalculateApi.Receipt.DB.Finder
{
    class ReceMasterFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public ReceMasterFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 医療機関情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <returns></returns>
        public HpInfModel FindHpInf(int hpId, int sinDate)
        {
            return new HpInfModel(
                _tenantDataContext.HpInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate)
                    .OrderByDescending(p=>p.StartDate)
                    .FirstOrDefault());
        }

        /// <summary>
        /// 単位マスタを取得する
        /// </summary>
        /// <param name="sinDate"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public UnitMstModel FindUnitMst(int sinDate, string Unit)
        {
            return new UnitMstModel(
                _tenantDataContext.UnitMsts.FindListQueryableNoTrack(p =>
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.UnitName == Unit)
                    .FirstOrDefault());
        }
        public List<JyusinbiDataModel> FindReceInfJd(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
        {
            var receInfJds = _tenantDataContext.ReceInfJds.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SeikyuYm == seikyuYm &&
                p.SinYm == sinYm &&
                p.HokenId == hokenId
            );
            var entities = receInfJds.AsEnumerable().Select(
            data =>
                new ReceInfJdModel(data)
            ).ToList();

            List<JyusinbiDataModel> results = new List<JyusinbiDataModel>();

            entities?.ForEach(entity => {
                results.Add(new JyusinbiDataModel(new ReceInfJdModel(entity.ReceInfJd)));
            });

            return results;
        }
    }
}
