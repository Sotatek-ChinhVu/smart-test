using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.DB
{
    public class CoKoukiSeikyuFinder : ICoKoukiSeikyuFinder
    {
        private readonly ICoHpInfFinder _hpInfFinder;
        private readonly ICoHokensyaMstFinder _hokensyaMstFinder;
        private readonly ICoHokenMstFinder _hokenMstFinder;
        private readonly ICoKokhoSeikyuFinder _kokhoSeikyuFinder;

        public CoKoukiSeikyuFinder(ICoHpInfFinder hpInfFinder, ICoHokensyaMstFinder hokensyaMstFinder, ICoKokhoSeikyuFinder kokhoSeikyuFinder, ICoHokenMstFinder hokenMstFinder)
        {
            _hpInfFinder = hpInfFinder;
            _hokensyaMstFinder = hokensyaMstFinder;
            _kokhoSeikyuFinder = kokhoSeikyuFinder;
            _hokenMstFinder = hokenMstFinder;
        }

        public List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos)
        {
            return _hokensyaMstFinder.GetHokensyaName(hpId, hokensyaNos);
        }

        public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
        {
            return _hpInfFinder.GetHpInf(hpId, seikyuYm);
        }

        public List<CoKaMstModel> GetKaMst(int hpId)
        {
            return _hpInfFinder.GetKaMst(hpId);
        }

        public List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm)
        {
            return _hokenMstFinder.GetKohiHoubetuMst(hpId, seikyuYm);
        }

        public List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, KokhoKind kokhoKind, PrefKbn prefKbn, int prefNo, HokensyaNoKbn mainHokensyaNo)
        {
            return
                _kokhoSeikyuFinder.GetReceInf(
                    hpId: hpId,
                    seikyuYm: seikyuYm,
                    seikyuType: seikyuType,
                    kokhoKind: kokhoKind,
                    prefKbn: prefKbn,
                    prefNo: prefNo,
                    mainHokensyaNo: mainHokensyaNo
                );
        }

        public void ReleaseResource()
        {
            _hpInfFinder.ReleaseResource();
            _hokensyaMstFinder.ReleaseResource();
            _kokhoSeikyuFinder.ReleaseResource();
            _hokenMstFinder.ReleaseResource();
        }
    }
}
