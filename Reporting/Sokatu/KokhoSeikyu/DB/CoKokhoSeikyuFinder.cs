using Domain.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using CalculateService.Constants;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.Common.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.DB
{
    public class CoKokhoSeikyuFinder : RepositoryBase, ICoKokhoSeikyuFinder
    {
        private readonly ICoHpInfFinder _hpInfFinder;
        private readonly ICoHokensyaMstFinder _hokensyaMstFinder;
        private readonly ICoHokenMstFinder _hokenMstFinder;

        public CoKokhoSeikyuFinder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder, ICoHokensyaMstFinder hokensyaMstFinder, ICoHokenMstFinder hokenMstFinder) : base(tenantProvider)
        {
            _hpInfFinder = hpInfFinder;
            _hokensyaMstFinder = hokensyaMstFinder;
            _hokenMstFinder = hokenMstFinder;
        }

        public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
        {
            return _hpInfFinder.GetHpInf(hpId, seikyuYm);
        }

        public List<CoKaMstModel> GetKaMst(int hpId)
        {
            return _hpInfFinder.GetKaMst(hpId);
        }

        public List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos)
        {
            return _hokensyaMstFinder.GetHokensyaName(hpId, hokensyaNos);
        }

        public List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm)
        {
            return _hokenMstFinder.GetKohiHoubetuMst(hpId, seikyuYm);
        }

        public List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, KokhoKind kokhoKind, PrefKbn prefKbn, int prefNo, HokensyaNoKbn mainHokensyaNo)
        {
            var receInfs = NoTrackingDataContext.ReceInfs.Where(x => x.HpId == hpId);
            var receStatuses = NoTrackingDataContext.ReceStatuses.Where(x => x.HpId == hpId);
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(
                p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None
            );
            var ptKohis = NoTrackingDataContext.PtKohis.Where(
                p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None
            );

            var joinQuery = (
                from receInf in receInfs
                join wrkStatus in receStatuses on
                    new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                    new { wrkStatus.HpId, wrkStatus.PtId, wrkStatus.SeikyuYm, wrkStatus.HokenId, wrkStatus.SinYm } into statusJoin
                from receStatus in statusJoin.DefaultIfEmpty()
                join ptHokenInf in ptHokenInfs on
                    new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                join wrkKohi1 in ptKohis on
                    new { receInf.HpId, receInf.PtId, receInf.Kohi1Id } equals
                    new { wrkKohi1.HpId, wrkKohi1.PtId, Kohi1Id = wrkKohi1.HokenId } into ptKohi1Join
                from ptKohi1 in ptKohi1Join.DefaultIfEmpty()
                join wrkKohi2 in ptKohis on
                    new { receInf.HpId, receInf.PtId, receInf.Kohi2Id } equals
                    new { wrkKohi2.HpId, wrkKohi2.PtId, Kohi2Id = wrkKohi2.HokenId } into ptKohi2Join
                from ptKohi2 in ptKohi2Join.DefaultIfEmpty()
                join wrkKohi3 in ptKohis on
                    new { receInf.HpId, receInf.PtId, receInf.Kohi3Id } equals
                    new { wrkKohi3.HpId, wrkKohi3.PtId, Kohi3Id = wrkKohi3.HokenId } into ptKohi3Join
                from ptKohi3 in ptKohi3Join.DefaultIfEmpty()
                join wrkKohi4 in ptKohis on
                    new { receInf.HpId, receInf.PtId, receInf.Kohi4Id } equals
                    new { wrkKohi4.HpId, wrkKohi4.PtId, Kohi4Id = wrkKohi4.HokenId } into ptKohi4Join
                from ptKohi4 in ptKohi4Join.DefaultIfEmpty()
                where
                    receInf.HpId == hpId &&
                    receInf.SeikyuYm == seikyuYm &&
                    //receInf.HokenKbn == HokenKbn.Kokho &&
                    receInf.IsTester == 0
                select new
                {
                    receInf,
                    ptHokenInf,
                    ptKohi1,
                    ptKohi2,
                    ptKohi3,
                    ptKohi4,
                    IsPaperRece = receStatus == null ? 0 : receStatus.IsPaperRece,
                }
            );

            //請求区分
            List<int> Codes = new List<int>();
            if (seikyuType.IsNormal) Codes.Add(Helper.Constants.SeikyuKbn.Normal);
            if (seikyuType.IsDelay) Codes.Add(Helper.Constants.SeikyuKbn.Delay);
            if (seikyuType.IsHenrei) Codes.Add(Helper.Constants.SeikyuKbn.Henrei);
            if (seikyuType.IsOnline) Codes.Add(Helper.Constants.SeikyuKbn.Online);

            if (seikyuType.IsPaper)
            {
                joinQuery = joinQuery.Where(r => r.IsPaperRece == 1 || Codes.Contains(r.receInf.SeikyuKbn));
            }
            else
            {
                joinQuery = joinQuery.Where(r => r.IsPaperRece == 0 && Codes.Contains(r.receInf.SeikyuKbn));
            }

            //国保種類
            switch (kokhoKind)
            {
                case KokhoKind.Kokho:
                    joinQuery = joinQuery.Where(r => r.receInf.HokenKbn == HokenKbn.Kokho && (r.receInf.ReceSbt.Substring(1, 1) == "1" || r.receInf.ReceSbt.Substring(1, 1) == "4"));
                    break;
                case KokhoKind.Kouki:
                    joinQuery = joinQuery.Where(r => r.receInf.HokenKbn == HokenKbn.Kokho && r.receInf.ReceSbt.Substring(1, 1) == "3");
                    break;
                case KokhoKind.Tokuyohi:
                    joinQuery = joinQuery.Where(r => r.receInf.ReceSbt.Substring(0, 1) == "9" && new string[] { "1", "3", "4" }.Contains(r.receInf.ReceSbt.Substring(1, 1)));
                    break;
                case KokhoKind.TokuyohiKokho:
                    joinQuery = joinQuery.Where(r => r.receInf.ReceSbt.Substring(0, 1) == "9" && new string[] { "1", "4" }.Contains(r.receInf.ReceSbt.Substring(1, 1)));
                    break;
                case KokhoKind.TokuyohiKouki:
                    joinQuery = joinQuery.Where(r => r.receInf.ReceSbt.Substring(0, 1) == "9" && new string[] { "3" }.Contains(r.receInf.ReceSbt.Substring(1, 1)));
                    break;
                default:
                    joinQuery = joinQuery.Where(r => r.receInf.HokenKbn == HokenKbn.Kokho);
                    break;
            }

            //県内県外
            List<string> prefIn = new List<string>();
            prefIn.Add(string.Format("{0:D2}", prefNo));
            prefIn.Add(string.Format("{0:D2}", prefNo + 50));

            if (prefKbn == PrefKbn.PrefIn)
            {
                joinQuery = joinQuery.Where(r =>
                    prefIn.Contains(r.receInf.HokensyaNo.Substring(r.receInf.HokensyaNo.Length - 6, 2))
                );
            }
            else if (prefKbn == PrefKbn.PrefOut)
            {
                joinQuery = joinQuery.Where(r =>
                    !prefIn.Contains(r.receInf.HokensyaNo.Substring(r.receInf.HokensyaNo.Length - 6, 2))
                );
            }

            var result = joinQuery.AsEnumerable().Select(
                data => new CoReceInfModel(
                    receInf: data.receInf,
                    ptHokenInf: data.ptHokenInf,
                    ptKohi1: data.ptKohi1,
                    ptKohi2: data.ptKohi2,
                    ptKohi3: data.ptKohi3,
                    ptKohi4: data.ptKohi4,
                    mainHokensyaNo: mainHokensyaNo,
                    prefNo: prefNo
                )
            ).ToList();

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
            _hokenMstFinder.ReleaseResource();
            _hpInfFinder.ReleaseResource();
            _hokensyaMstFinder.ReleaseResource();
        }
    }
}