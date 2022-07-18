using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public InsuranceRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public IEnumerable<InsuranceModel> GetInsuranceListById(long ptId)
        {
            return _tenantDataContext.PtHokenInfs.Where(x => x.PtId == ptId).Select(
                x => new InsuranceModel(
                    x.HpId,
                    x.PtId,
                    x.HokenId,
                    x.SeqNo,
                    x.HokenNo,
                    x.EdaNo,
                    x.HokenEdaNo,
                    x.HokensyaNo,
                    x.Kigo,
                    x.Bango,
                    x.HonkeKbn,
                    x.HokenKbn,
                    x.Houbetu,
                    x.HokensyaName,
                    x.HokensyaPost,
                    x.HokensyaAddress,
                    x.HokensyaTel,
                    x.KeizokuKbn,
                    x.SikakuDate,
                    x.KofuDate,
                    x.StartDate,
                    x.EndDate,
                    x.Rate,
                    x.Gendogaku,
                    x.KogakuKbn,
                    x.KogakuType,
                    x.TokureiYm1,
                    x.TokureiYm2,
                    x.TasukaiYm,
                    x.SyokumuKbn,
                    x.GenmenKbn,
                    x.GenmenRate,
                    x.GenmenGaku,
                    x.Tokki1,
                    x.Tokki2,
                    x.Tokki3,
                    x.Tokki4,
                    x.Tokki5,
                    x.RousaiKofuNo,
                    x.RousaiSaigaiKbn,
                    x.RousaiJigyosyoName,
                    x.RousaiPrefName,
                    x.RousaiCityName,
                    x.RousaiSyobyoDate,
                    x.RousaiSyobyoCd,
                    x.RousaiRoudouCd,
                    x.RousaiKantokuCd,
                    x.RousaiReceCount,
                    x.RyoyoStartDate,
                    x.RyoyoEndDate,
                    x.JibaiHokenName,
                    x.JibaiHokenTanto,
                    x.JibaiHokenTel,
                    x.JibaiJyusyouDate
                    )).ToList();
        }
    }
}