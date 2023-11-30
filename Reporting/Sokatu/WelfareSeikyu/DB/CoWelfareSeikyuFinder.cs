using Domain.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Calculate.Extensions;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.DB
{
    public class CoWelfareSeikyuFinder : RepositoryBase, ICoWelfareSeikyuFinder
    {
        private readonly ICoHpInfFinder _hpInfFinder;
        private readonly ICoHokensyaMstFinder _hokensyaMstFinder;
        private readonly ICoHokenMstFinder _hokenMstFinder;

        public CoWelfareSeikyuFinder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder, ICoHokensyaMstFinder hokensyaMstFinder, ICoHokenMstFinder hokenMstFinder) : base(tenantProvider)
        {
            _hpInfFinder = hpInfFinder;
            _hokensyaMstFinder = hokensyaMstFinder;
            _hokenMstFinder = hokenMstFinder;
        }

        public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
        {
            return _hpInfFinder.GetHpInf(hpId, seikyuYm);
        }

        public List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos)
        {
            return _hokensyaMstFinder.GetHokensyaName(hpId, hokensyaNos);
        }

        public List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm)
        {
            return _hokenMstFinder.GetKohiHoubetuMst(hpId, seikyuYm);
        }

        private List<CoWelfareReceInfModel> getReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, List<int> kohiHokenNos, List<string> kohiHoubetus,
            FutanCheck futanCheck, int hokenKbn, bool isReceKisai)
        {
            var receInfs = NoTrackingDataContext.ReceInfs.FindListQueryableNoTrack();
            var receStatuses = NoTrackingDataContext.ReceStatuses.FindListQueryableNoTrack();
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.FindListQueryableNoTrack(
                p => p.IsDeleted == DeleteStatus.None
            );
            var ptKohis = NoTrackingDataContext.PtKohis.FindListQueryableNoTrack(
                p => p.IsDeleted == DeleteStatus.None
            );
            var ptInfs = NoTrackingDataContext.PtInfs.FindListQueryableNoTrack(
                p => p.IsDelete == DeleteStatus.None
            );

            var joinQuery = (
                from receInf in receInfs
                join wrkStatus in receStatuses on
                    new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                    new { wrkStatus.HpId, wrkStatus.PtId, wrkStatus.SeikyuYm, wrkStatus.HokenId, wrkStatus.SinYm } into statusJoin
                from receStatus in statusJoin.DefaultIfEmpty()
                join ptInf in ptInfs on
                    new { receInf.HpId, receInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
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
                    receInf.IsTester == 0
                select new
                {
                    receInf,
                    ptInf,
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
            if (seikyuType.IsNormal) Codes.Add(SeikyuKbn.Normal);
            if (seikyuType.IsDelay) Codes.Add(SeikyuKbn.Delay);
            if (seikyuType.IsHenrei) Codes.Add(SeikyuKbn.Henrei);
            if (seikyuType.IsOnline) Codes.Add(SeikyuKbn.Online);

            if (seikyuType.IsPaper)
            {
                joinQuery = joinQuery.Where(r => r.IsPaperRece == 1 || Codes.Contains(r.receInf.SeikyuKbn));
            }
            else
            {
                joinQuery = joinQuery.Where(r => r.IsPaperRece == 0 && Codes.Contains(r.receInf.SeikyuKbn));
            }

            var count0 = joinQuery.ToList();

            //公費負担チェック窓口
            int lowKohiFutan10en = futanCheck == FutanCheck.KohiFutan10en ? 1 : 0;
            //公費負担チェック
            int lowKohiFutan = futanCheck == FutanCheck.KohiFutan ? 1 : 0;
            //一部負担相当額チェック
            int lowIchibuFutan = futanCheck == FutanCheck.IchibuFutan ? 1 : 0;

            //併用レセの公費を含む
            if (isReceKisai)
            {
                //保険番号指定
                if (kohiHokenNos?.Count >= 1)
                {
                    joinQuery = joinQuery.Where(r =>
                        (kohiHokenNos.Contains(r.ptKohi1.HokenNo) && r.receInf.Kohi1Futan10en >= lowKohiFutan10en && r.receInf.Kohi1Futan >= lowKohiFutan && r.receInf.Kohi1IchibuSotogaku + r.receInf.Kohi1Futan >= lowIchibuFutan) ||
                        (kohiHokenNos.Contains(r.ptKohi2.HokenNo) && r.receInf.Kohi2Futan10en >= lowKohiFutan10en && r.receInf.Kohi2Futan >= lowKohiFutan && r.receInf.Kohi2IchibuSotogaku + r.receInf.Kohi2Futan >= lowIchibuFutan) ||
                        (kohiHokenNos.Contains(r.ptKohi3.HokenNo) && r.receInf.Kohi3Futan10en >= lowKohiFutan10en && r.receInf.Kohi3Futan >= lowKohiFutan && r.receInf.Kohi3IchibuSotogaku + r.receInf.Kohi3Futan >= lowIchibuFutan) ||
                        (kohiHokenNos.Contains(r.ptKohi4.HokenNo) && r.receInf.Kohi4Futan10en >= lowKohiFutan10en && r.receInf.Kohi4Futan >= lowKohiFutan && r.receInf.Kohi4IchibuSotogaku + r.receInf.Kohi4Futan >= lowIchibuFutan)
                    );
                }
                //法別番号指定
                if (kohiHoubetus?.Count >= 1)
                {
                    joinQuery = joinQuery.Where(r =>
                        (kohiHoubetus.Contains(r.receInf.Kohi1Houbetu ?? string.Empty) && r.receInf.Kohi1Futan10en >= lowKohiFutan10en && r.receInf.Kohi1Futan >= lowKohiFutan && r.receInf.Kohi1IchibuSotogaku + r.receInf.Kohi1Futan >= lowIchibuFutan) ||
                        (kohiHoubetus.Contains(r.receInf.Kohi2Houbetu ?? string.Empty) && r.receInf.Kohi2Futan10en >= lowKohiFutan10en && r.receInf.Kohi2Futan >= lowKohiFutan && r.receInf.Kohi2IchibuSotogaku + r.receInf.Kohi2Futan >= lowIchibuFutan) ||
                        (kohiHoubetus.Contains(r.receInf.Kohi3Houbetu ?? string.Empty) && r.receInf.Kohi3Futan10en >= lowKohiFutan10en && r.receInf.Kohi3Futan >= lowKohiFutan && r.receInf.Kohi3IchibuSotogaku + r.receInf.Kohi3Futan >= lowIchibuFutan) ||
                        (kohiHoubetus.Contains(r.receInf.Kohi4Houbetu ?? string.Empty) && r.receInf.Kohi4Futan10en >= lowKohiFutan10en && r.receInf.Kohi4Futan >= lowKohiFutan && r.receInf.Kohi4IchibuSotogaku + r.receInf.Kohi4Futan >= lowIchibuFutan)
                    );
                }
            }
            //併用レセの公費を除く
            else
            {
                //保険番号指定
                if (kohiHokenNos?.Count >= 1)
                {
                    joinQuery = joinQuery.Where(r =>
                        (kohiHokenNos.Contains(r.ptKohi1.HokenNo) && r.receInf.Kohi1Futan10en >= lowKohiFutan10en && r.receInf.Kohi1Futan >= lowKohiFutan && r.receInf.Kohi1IchibuSotogaku + r.receInf.Kohi1Futan >= lowIchibuFutan && r.receInf.Kohi1ReceKisai == 0) ||
                        (kohiHokenNos.Contains(r.ptKohi2.HokenNo) && r.receInf.Kohi2Futan10en >= lowKohiFutan10en && r.receInf.Kohi2Futan >= lowKohiFutan && r.receInf.Kohi2IchibuSotogaku + r.receInf.Kohi2Futan >= lowIchibuFutan && r.receInf.Kohi2ReceKisai == 0) ||
                        (kohiHokenNos.Contains(r.ptKohi3.HokenNo) && r.receInf.Kohi3Futan10en >= lowKohiFutan10en && r.receInf.Kohi3Futan >= lowKohiFutan && r.receInf.Kohi3IchibuSotogaku + r.receInf.Kohi3Futan >= lowIchibuFutan && r.receInf.Kohi3ReceKisai == 0) ||
                        (kohiHokenNos.Contains(r.ptKohi4.HokenNo) && r.receInf.Kohi4Futan10en >= lowKohiFutan10en && r.receInf.Kohi4Futan >= lowKohiFutan && r.receInf.Kohi4IchibuSotogaku + r.receInf.Kohi4Futan >= lowIchibuFutan && r.receInf.Kohi4ReceKisai == 0)
                    );
                    
                }
                //法別番号指定
                if (kohiHoubetus?.Count >= 1)
                {
                    joinQuery = joinQuery.Where(r =>
                        (kohiHoubetus.Contains(r.receInf.Kohi1Houbetu ?? string.Empty) && r.receInf.Kohi1Futan10en >= lowKohiFutan10en && r.receInf.Kohi1Futan >= lowKohiFutan && r.receInf.Kohi1IchibuSotogaku + r.receInf.Kohi1Futan >= lowIchibuFutan && r.receInf.Kohi1ReceKisai == 0) ||
                        (kohiHoubetus.Contains(r.receInf.Kohi2Houbetu ?? string.Empty) && r.receInf.Kohi2Futan10en >= lowKohiFutan10en && r.receInf.Kohi2Futan >= lowKohiFutan && r.receInf.Kohi2IchibuSotogaku + r.receInf.Kohi2Futan >= lowIchibuFutan && r.receInf.Kohi2ReceKisai == 0) ||
                        (kohiHoubetus.Contains(r.receInf.Kohi3Houbetu ?? string.Empty) && r.receInf.Kohi3Futan10en >= lowKohiFutan10en && r.receInf.Kohi3Futan >= lowKohiFutan && r.receInf.Kohi3IchibuSotogaku + r.receInf.Kohi3Futan >= lowIchibuFutan && r.receInf.Kohi3ReceKisai == 0) ||
                        (kohiHoubetus.Contains(r.receInf.Kohi4Houbetu ?? string.Empty) && r.receInf.Kohi4Futan10en >= lowKohiFutan10en && r.receInf.Kohi4Futan >= lowKohiFutan && r.receInf.Kohi4IchibuSotogaku + r.receInf.Kohi4Futan >= lowIchibuFutan && r.receInf.Kohi4ReceKisai == 0)
                    );
                }
            }

            //社保国保
            if (hokenKbn == HokenKbn.Syaho)
            {
                joinQuery = joinQuery.Where(r => r.receInf.HokenKbn == HokenKbn.Syaho);
            }
            else if (hokenKbn == HokenKbn.Kokho)
            {
                joinQuery = joinQuery.Where(r => r.receInf.HokenKbn == HokenKbn.Kokho);
            }

            var result = joinQuery.AsEnumerable().Select(
                data => new CoWelfareReceInfModel(
                    receInf: data.receInf,
                    ptInf: data.ptInf,
                    ptHokenInf: data.ptHokenInf,
                    ptKohi1: data.ptKohi1,
                    ptKohi2: data.ptKohi2,
                    ptKohi3: data.ptKohi3,
                    ptKohi4: data.ptKohi4
                )
            ).OrderBy(r => r.ReceInf.SinYm).ThenBy(r => r.PtInf.PtNum).ToList();

            return result;
        }

        public List<CoWelfareReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, List<int> kohiHokenNos, FutanCheck futanCheck, int hokenKbn)
        {
            return getReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, null, futanCheck, hokenKbn, false);
        }

        public List<CoWelfareReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> kohiHoubetus, FutanCheck futanCheck, int hokenKbn, bool isReceKisai = false)
        {
            return getReceInf(hpId, seikyuYm, seikyuType, null, kohiHoubetus, futanCheck, hokenKbn, isReceKisai);
        }

        public bool IsOutDrugOrder(int hpId, long ptId, int sinYm)
        {
            return
                NoTrackingDataContext.OdrInfs.FindListNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate >= sinYm * 100 + 1 &&
                    p.SinDate <= sinYm * 100 + 31 &&
                    p.OdrKouiKbn >= 20 && p.OdrKouiKbn <= 29 &&
                    p.InoutKbn == 1 &&
                    p.IsDeleted == 0
                ).Count >= 1;
        }

        public void ReleaseResource()
        {
            _hpInfFinder.ReleaseResource();
            _hokensyaMstFinder.ReleaseResource();
            _hokenMstFinder.ReleaseResource();
            DisposeDataContext();
        }
    }
}