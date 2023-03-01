using Amazon.S3.Model.Internal.MarshallTransformations;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace EmrCalculateApi.Implementation
{
    public class SystemConfigProvider : ISystemConfigProvider
    {
        private readonly List<SystemConf> _systemConfigs;
        public SystemConfigProvider(ITenantProvider tenantProvider)
        {
            _systemConfigs = tenantProvider.GetNoTrackingDataContext().SystemConfs.ToList();
        }

        public int GetChokiDateRange()
        {
            return (int)GetSettingValue(3006, 3, 0);
        }

        public int GetChokiFutan()
        {
            return (int)GetSettingValue(3006, 2, 0);
        }

        public int GetJibaiJunkyo()
        {
            return (int)GetSettingValue(3001, 0);
        }

        public double GetJibaiRousaiRate()
        {
            return GetSettingValue(3001, 1);
        }

        public int GetRoundKogakuPtFutan()
        {
            return (int)GetSettingValue(3016, 0);
        }
        public int GetReceiptTantoIdTarget()
        {
            return (int)GetSettingValue(6002, 1);
        }
        public int GetReceiptKaIdTarget()
        {
            return (int)GetSettingValue(6002, 0);
        }
        public int GetHokensyuHandling()
        {
            return (int)GetSettingValue(3013);
        }
        public int GetCalcCheckKensaDuplicateLog()
        {
            return (int)GetSettingValue(3019, 0);
        }
        public int GetHoukatuHaihanCheckMode()
        {
            return (int)GetSettingValue(3008, 0, 0);
        }
        public int GetHoukatuHaihanLogputMode()
        {
            return (int)GetSettingValue(3009, 0, 0);
        }
        public int GetHoukatuHaihanSPJyokenLogputMode()
        {
            return (int)GetSettingValue(3009, 1, 0);
        }
        public int GetHoumonKangoSaisinHokatu()
        {
            return (int)GetSettingValue(3023, 0, 0);
        }
        public int GetKensaMarumeBuntenSyaho()
        {
            return (int)GetSettingValue(3017, 0);
        }
        public int GetKensaMarumeBuntenKokuho() 
        {
            return (int) GetSettingValue(3017, 1);
        }
        public int GetReceNoDspComment()
        {
            return (int)GetSettingValue(3012, 0, 0); 
        }
        public int GetOutDrugYohoDsp()
        {
            return (int)GetSettingValue(3005, 0, 1); 
        }
        public int GetSyohoRinjiDays()
        {
            return (int)GetSettingValue(3002, 0, 14);
        }
        public int GetRousaiRecedenLicense()
        {
            return (int)GetSettingValue(100003, 0);
        }
        public int GetAfterCareRecedenLicense()
        {
            return (int)GetSettingValue(100003, 1);
        }
        public string GetRousaiRecedenStartYm()
        {
            return GetSettingParam(100003, 0);
        }
        public string GetAfterCareRecedenStartYm()
        {
            return GetSettingParam(100003, 1);
        }
        public int GetDrugPid()
        {
            return (int)GetSettingValue(3007, 0, 0);
        }
        public int GetSyouniCounselingCheck()
        {
            return (int)GetSettingValue(3024, 0, 0);
        }
        public int GetInDrugYohoComment()
        {
            return (int)GetSettingValue(3022, 0, 0);
        }
        public int GetCalcAutoComment()
        {
            return (int)GetSettingValue(3018, 0);
        }
        public string GetNaraFukusiReceCmtStartDate()
        {
            return (string)GetSettingParam(3011, 0, "");
        }
        public int GetNaraFukusiReceCmt()
        {
            return (int)GetSettingValue(3011, 0, 0);
        }
        public int GetReceiptOutDrgSinId()
        {
            return (int)GetSettingValue(94006, 0);
        }
        public int GetReceiptCommentTenCount()
        {
            return (int)GetSettingValue(94007, 0);
        }
        public int GetSameRpMerge()
        {
            return (int)GetSettingValue(3014);
        }

        public int GetChokiTokki()
        {
            return (int)GetSettingValue(3006, 1, 0);
        }

        public int GetReceKyufuKisai()
        {
            return (int)GetSettingValue(3010, 0, 0);
        }

        public int GetReceKyufuKisai2()
        {
            return (int)GetSettingValue(3010, 1, 0);
        }

        private double GetSettingValue(int groupCd, int grpEdaNo = 0, int defaultValue = 0)
        {
            SystemConf? systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            return systemConf != null ? systemConf.Val : defaultValue;
        }
        private string GetSettingParam(int groupCd, int grpEdaNo = 0, string defaultParam = "")
        {
            SystemConf systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            return systemConf != null ? systemConf.Param : defaultParam;
        }
    }
}
