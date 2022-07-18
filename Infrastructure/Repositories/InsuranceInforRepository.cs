using Domain.CommonObject;
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
    public class InsuranceInforRepository: IInsuranceInforResponsitory
    {
        private readonly TenantDataContext _tenantDataContext;
        public InsuranceInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }

        public InsuranceInforModel? GetInsuranceInfor(long ptId, int hokenId)
        {
            var data = _tenantDataContext.PtHokenInfs.Where(x => x.PtId == ptId && x.HokenId == hokenId).SingleOrDefault();
            if(data == null)
                return null;
            return new InsuranceInforModel(
                data.HpId,
                data.PtId,
                data.HokenId,
                data.SeqNo,
                data.HokenNo,
                data.EdaNo,
                data.HokenEdaNo,
                data.HokensyaNo,
                data.Kigo,
                data.Bango,
                data.HonkeKbn,
                data.HokenKbn,
                data.Houbetu,
                data.HokensyaName,
                data.HokensyaPost,
                data.HokensyaAddress,
                data.HokensyaTel,
                data.KeizokuKbn,
                data.SikakuDate,
                data.KofuDate,
                data.StartDate,
                data.EndDate,
                data.Rate,
                data.Gendogaku,
                data.KogakuKbn,
                data.KogakuType,
                data.TokureiYm1,
                data.TokureiYm2,
                data.TasukaiYm,
                data.SyokumuKbn,
                data.GenmenKbn,
                data.GenmenRate,
                data.GenmenGaku,
                data.Tokki1,
                data.Tokki2,
                data.Tokki3,
                data.Tokki4,
                data.Tokki5,
                data.RousaiKofuNo,
                data.RousaiSaigaiKbn,
                data.RousaiJigyosyoName,
                data.RousaiPrefName,
                data.RousaiCityName,
                data.RousaiSyobyoDate,
                data.RousaiSyobyoCd,
                data.RousaiRoudouCd,
                data.RousaiKantokuCd,
                data.RousaiReceCount,
                data.RyoyoStartDate,
                data.RyoyoEndDate,
                data.JibaiHokenName,
                data.JibaiHokenTanto,
                data.JibaiHokenTel,
                data.JibaiJyusyouDate
                );
        }   
    }
}
