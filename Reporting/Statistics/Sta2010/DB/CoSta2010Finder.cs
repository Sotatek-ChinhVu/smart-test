using Domain.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2010.Models;

namespace Reporting.Statistics.Sta2010.DB
{
    public class CoSta2010Finder : RepositoryBase, ICoSta2010Finder
    {
        private ICoHpInfFinder _hpInfFinder;

        public CoSta2010Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
        {
            _hpInfFinder = hpInfFinder;
        }

        public void ReleaseResource()
        {
            _hpInfFinder.ReleaseResource();
            DisposeDataContext();
        }

        public CoHpInfModel GetHpInf(int hpId, int sinDate)
        {
            return _hpInfFinder.GetHpInf(hpId, sinDate);
        }

        /// <summary>
        /// レセプト情報
        /// </summary>
        /// <param name="printConf"></param>
        /// <param name="prefNo"></param>
        /// <returns></returns>
        public List<CoReceInfModel> GetReceInfs(int hpId, CoSta2010PrintConf printConf, int prefNo)
        {
            var receInfs = NoTrackingDataContext.ReceInfs.Where(x => x.HpId == hpId);
            if (printConf.KaIds?.Count >= 1)
            {
                //診療科の条件指定
                receInfs = receInfs.Where(r => printConf.KaIds.Contains(r.KaId));
            }
            if (printConf.TantoIds?.Count >= 1)
            {
                //担当医の条件指定
                receInfs = receInfs.Where(r => printConf.TantoIds.Contains(r.TantoId));
            }
            if (printConf.HokenSbts?.Count >= 1)
            {
                //保険種別
                List<int> hokenKbns = new List<int>();
                if (printConf.HokenSbts.Contains(1)) hokenKbns.Add(1);                                              //社保
                if (printConf.HokenSbts.Contains(2) || printConf.HokenSbts.Contains(3)) hokenKbns.Add(2);           //国保・後期
                if (printConf.HokenSbts.Contains(10)) { hokenKbns.Add(11); hokenKbns.Add(12); hokenKbns.Add(13); }  //労災
                if (printConf.HokenSbts.Contains(11)) hokenKbns.Add(14);                                            //自賠
                if (printConf.HokenSbts.Contains(12)) hokenKbns.Add(0);                                             //自費

                receInfs = receInfs.Where(r => hokenKbns.Contains(r.HokenKbn));

                if (printConf.HokenSbts.Contains(2) && !printConf.HokenSbts.Contains(3))
                {
                    //後期を除く
                    receInfs = receInfs.Where(r => !(r.HokenKbn == 2 && r.ReceSbt.Substring(1, 1) == "3"));
                }
                else if (!printConf.HokenSbts.Contains(2) && printConf.HokenSbts.Contains(3))
                {
                    //国保一般・退職を除く
                    receInfs = receInfs.Where(r => !(r.HokenKbn == 2 && r.ReceSbt.Substring(1, 1) != "3"));
                }
            }

            var receStatuses = NoTrackingDataContext.ReceStatuses.Where(x => x.HpId == hpId);
            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(
                p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None
            );
            var ptKohis = NoTrackingDataContext.PtKohis.Where(
                p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None
            );
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteStatus.None);
            //診療科マスタ
            var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId);
            //ユーザーマスタ
            var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && u.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from receInf in receInfs
                join ptInf in ptInfs on
                    new { receInf.HpId, receInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join wrkStatus in receStatuses on
                    new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                    new { wrkStatus.HpId, wrkStatus.PtId, wrkStatus.SeikyuYm, wrkStatus.HokenId, wrkStatus.SinYm } into statusJoin
                from receStatus in statusJoin.DefaultIfEmpty()
                join ptHokenInf in ptHokenInfs on
                    new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                join kaMst in kaMsts on
                    new { receInf.HpId, receInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaMstJoin
                from kaMstj in kaMstJoin.DefaultIfEmpty()
                join userMst in userMsts on
                    new { receInf.HpId, receInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
                from tantoMst in userMstJoin.DefaultIfEmpty()
                where
                    receInf.HpId == hpId &&
                    receInf.SeikyuYm == printConf.SeikyuYm &&
                    (printConf.IsTester ? true : receInf.IsTester == 0)
                select new
                {
                    receInf,
                    ptHokenInf,
                    IsPaperRece = receStatus == null ? 0 : receStatus.IsPaperRece,
                    kaMstj,
                    tantoMst
                }
            );

            //請求区分
            if (printConf.SeikyuTypes?.Count >= 1)
            {
                //紙請求レセプト
                bool isPaper = printConf.SeikyuTypes.Contains(9);

                if (isPaper)
                {
                    joinQuery = joinQuery.Where(r => r.IsPaperRece == 1 || printConf.SeikyuTypes.Contains(r.receInf.SeikyuKbn));
                }
                else
                {
                    joinQuery = joinQuery.Where(r => r.IsPaperRece == 0 && printConf.SeikyuTypes.Contains(r.receInf.SeikyuKbn));
                }
            }

            var result = joinQuery.AsEnumerable().Select(
                data => new CoReceInfModel(
                    receInf: data.receInf,
                    ptHokenInf: data.ptHokenInf,
                    ptKohi1: null, //data.ptKohi1,
                    ptKohi2: null, //data.ptKohi2,
                    ptKohi3: null, //data.ptKohi3,
                    ptKohi4: null, //data.ptKohi4,
                    mainHokensyaNo: printConf.MainHokensyaNo,
                    prefNo: prefNo,
                    data.kaMstj,
                    data.tantoMst
                )
            ).ToList();

            #region '公費情報の設定（joinするとindexを設定しても著しくパフォーマンスが低下するため別で取得する）'
            var kohiDatas = (
                from receInf in receInfs
                join ptKohi in ptKohis on
                    new { receInf.HpId, receInf.PtId, receInf.Kohi1Id } equals
                    new { ptKohi.HpId, ptKohi.PtId, Kohi1Id = ptKohi.HokenId }
                where
                    receInf.HpId == hpId &&
                    receInf.SeikyuYm == printConf.SeikyuYm &&
                    (printConf.IsTester ? true : receInf.IsTester == 0)
                select new
                {
                    ptKohi
                }
            ).AsEnumerable().Select(k => k.ptKohi).ToList();

            kohiDatas.AddRange(
                (
                    from receInf in receInfs
                    join ptKohi in ptKohis on
                        new { receInf.HpId, receInf.PtId, receInf.Kohi2Id } equals
                        new { ptKohi.HpId, ptKohi.PtId, Kohi2Id = ptKohi.HokenId }
                    where
                        receInf.HpId == hpId &&
                        receInf.SeikyuYm == printConf.SeikyuYm &&
                        (printConf.IsTester ? true : receInf.IsTester == 0)
                    select new
                    {
                        ptKohi
                    }
                ).AsEnumerable().Select(k => k.ptKohi).ToList()
            );

            kohiDatas.AddRange(
                (
                    from receInf in receInfs
                    join ptKohi in ptKohis on
                        new { receInf.HpId, receInf.PtId, receInf.Kohi3Id } equals
                        new { ptKohi.HpId, ptKohi.PtId, Kohi3Id = ptKohi.HokenId }
                    where
                        receInf.HpId == hpId &&
                        receInf.SeikyuYm == printConf.SeikyuYm &&
                        (printConf.IsTester ? true : receInf.IsTester == 0)
                    select new
                    {
                        ptKohi
                    }
                ).AsEnumerable().Select(k => k.ptKohi).ToList()
            );

            kohiDatas.AddRange(
                (
                    from receInf in receInfs
                    join ptKohi in ptKohis on
                        new { receInf.HpId, receInf.PtId, receInf.Kohi4Id } equals
                        new { ptKohi.HpId, ptKohi.PtId, Kohi4Id = ptKohi.HokenId }
                    where
                        receInf.HpId == hpId &&
                        receInf.SeikyuYm == printConf.SeikyuYm &&
                        (printConf.IsTester ? true : receInf.IsTester == 0)
                    select new
                    {
                        ptKohi
                    }
                ).AsEnumerable().Select(k => k.ptKohi).ToList()
            );

            foreach (var retData in result)
            {
                if (retData.Kohi1Id == 0) continue;
                retData.PtKohi1 = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.Kohi1Id);

                if (retData.Kohi2Id == 0) continue;
                retData.PtKohi2 = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.Kohi2Id);

                if (retData.Kohi3Id == 0) continue;
                retData.PtKohi3 = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.Kohi3Id);

                if (retData.Kohi4Id == 0) continue;
                retData.PtKohi4 = kohiDatas.Find(k => k.PtId == retData.PtId && k.HokenId == retData.Kohi4Id);
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 公費法別マスタ（法別番号ごとの公費名称）
        /// </summary>
        /// <param name="seikyuYm"></param>
        /// <returns></returns>
        public List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm)
        {
            int sinDate = seikyuYm * 100 + 31;

            var hokenMstAll = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId);
            //請求月末日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(h =>
                h.HpId == hpId &&
                h.StartDate <= sinDate &&
                new int[] { HokenSbtKbn.Seiho, HokenSbtKbn.Bunten, HokenSbtKbn.Ippan }.Contains(h.HokenSbtKbn)
            ).GroupBy(
                x => new { x.HpId, x.PrefNo, x.HokenNo, x.HokenEdaNo }
            ).Select(
                x => new
                {
                    x.Key.HpId,
                    x.Key.PrefNo,
                    x.Key.HokenNo,
                    x.Key.HokenEdaNo,
                    StartDate = x.Max(d => d.StartDate)
                }
            );
            //保険番号マスタの取得
            var hokenMsts = (
                from hokenMst in hokenMstAll
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                group new
                {
                    hokenMst.PrefNo,
                    hokenMst.Houbetu,
                    hokenMst.HokenNameCd
                } by new { hokenMst.PrefNo, hokenMst.Houbetu, hokenMst.HokenNameCd } into hokenGroup
                select new
                {
                    hokenGroup.Key.PrefNo,
                    hokenGroup.Key.Houbetu,
                    hokenGroup.Key.HokenNameCd
                }
            ).ToList();

            return
                hokenMsts.Select(
                    x => new CoKohiHoubetuMstModel()
                    {
                        PrefNo = x.PrefNo,
                        Houbetu = x.Houbetu,
                        HokenNameCd = x.HokenNameCd
                    }
                ).ToList();
        }

        /// <summary>
        /// 保険者マスタ取得（保険者名）
        /// </summary>
        /// <param name="hokensyaNos">保険者番号</param>
        /// <returns></returns>
        public List<CoHokensyaMstModel> GetHokensyaName(int hpId, List<string> hokensyaNos)
        {
            hokensyaNos = hokensyaNos.Distinct().ToList();
            var coHokensyaMsts = NoTrackingDataContext.HokensyaMsts.Where(
                h => h.HpId == hpId && hokensyaNos.Contains(h.HokensyaNo)
            ).ToList();

            return coHokensyaMsts.Select(h => new CoHokensyaMstModel(h)).ToList();
        }
    }
}
