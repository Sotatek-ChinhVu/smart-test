using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using Domain.Constant;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace EmrCalculateApi.Ika.DB.Finder
{
    public class MasterFinder 
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;
        private readonly TenantDataContext _tenantDataContext;

        public MasterFinder(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        /// <summary>
        /// 点数マスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>点数マスタ情報</returns>
        public List<TenMstModel> FindTenMst(int hpId, int sinDate)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 点数マスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>点数マスタ情報</returns>
        public List<TenMstModel> FindTenMst(int hpId, int sinDate, List<string> itemCds)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    itemCds.Contains(p.ItemCd))
                .ToList();

            return entities.Select(p => new TenMstModel(p)).ToList();
        }
        /// <summary>
        /// 点数マスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>点数マスタ情報</returns>
        public List<TenMstModel> FindTenMstByItemCd(int hpId, int sinDate, string itemCd)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.ItemCd == itemCd &&
                    (p.StartDate <= sinDate &&
                      (p.EndDate >= sinDate || p.EndDate == 12341234)))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;
        }

        public List<TenMstModel> FindTenMstByItemCd(int hpId, List<string> itemCds)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    itemCds.Contains(p.ItemCd) )
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 指定の包括区分の項目のリストを取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="hokatuKbn">包括区分</param>
        /// <returns></returns>
        public List<TenMstModel> FindTenMstByHokatuKbn(int hpId, int sinDate, int hokatuKbn)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.HokatuKbn == hokatuKbn &&
                    (p.StartDate <= sinDate &&
                      (p.EndDate >= sinDate || p.EndDate == 12341234)))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;
        }

        public List<TenMstModel> FindTenMstByHokatuKbn(int hpId, int sinDate, int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.HokatuKbn == hokatuKbn &&
                    p.CdKbn == cdKbn &&
                    p.CdKbnno == cdKbnno &&
                    p.CdEdano == cdEdano &&
                    p.CdKouno == cdKouno &&
                    (p.StartDate <= sinDate &&
                      (p.EndDate >= sinDate || p.EndDate == 12341234)))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// コメント区分マスタ取得
        /// </summary>
        /// <param name="hpId">医療機関識別コード</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<CmtKbnMstModel> FindCmtKbnMst(int hpId, int sinDate, List<string> itemCds)
        {
            var entities = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    itemCds.Contains(p.ItemCd) &&
                    (p.StartDate <= sinDate &&
                      (p.EndDate >= sinDate || p.EndDate == 12341234)))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<CmtKbnMstModel> results = new List<CmtKbnMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CmtKbnMstModel(entity));
            });

            return results;
        }
        public List<CmtKbnMstModel> FindCmtKbnMstByItemCd(int hpId, int sinDate, string itemCd)
        {
            var entities = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.ItemCd == itemCd &&
                    (p.StartDate <= sinDate &&
                      (p.EndDate >= sinDate || p.EndDate == 12341234)))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<CmtKbnMstModel> results = new List<CmtKbnMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CmtKbnMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 点数マスタコメントマスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>点数マスタコメントマスタ情報</returns>
        public List<TenMstModel> FindCommentMst(int hpId, int sinDate)
        {
            const string conFncName = nameof(FindCommentMst);

            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    (p.MasterSbt == "C" || p.MasterSbt == "D") &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;

        }
        /// <summary>
        /// 点数マスタを取得する（向精神薬区分）
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="itemCds"></param>
        /// <returns></returns>
        public List<TenMstModel> FindTenMstByKouseisinKbn(int hpId, int sinDate, List<int> kouseisinKbns)
        {
            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    kouseisinKbns.Contains(p.KouseisinKbn))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<TenMstModel> results = new List<TenMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new TenMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 点数マスタから在宅の週単位計算項目を取得する
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>点数マスタ情報</returns>
        //public List<TenMstModel> GetZaiWeekCalc(int hpId, int sinDate)
        //{
        //    const string conFncName = nameof(GetZaiWeekCalc);

        //    var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
        //            p.HpId == hpId &&
        //            p.StartDate <= sinDate &&
        //            p.EndDate >= sinDate &&
        //            p.CdKbn == "C" &&
        //            p.CdKbnno == 3 &&
        //            p.CdEdano == 0 &&
        //            new int[] { 1, 2, 4 }.Contains(p.CdKouno))
        //        .OrderBy(p => p.HpId)
        //        .ThenBy(p => p.ItemCd)
        //        .ThenBy(p => p.StartDate)
        //        .ToList();

        //    List<TenMstModel> results = new List<TenMstModel>();

        //    entities?.ForEach(entity =>
        //    {
        //        results.Add(new TenMstModel(entity));
        //    });

        //    return results;
        //}
        public List<string> GetZaiWeekCalc(int hpId, int sinDate)
        {
            const string conFncName = nameof(GetZaiWeekCalc);

            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.CdKbn == "C" &&
                    p.CdKbnno == 3 &&
                    p.CdEdano == 0 &&
                    new int[] { 1, 2, 4 }.Contains(p.CdKouno))
                .Select(p => new { p.HpId, p.StartDate, p.ItemCd })
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<string> results = new List<string>();

            entities?.ForEach(entity =>
            {
                results.Add(entity.ItemCd);
            });

            return results;
        }
        /// <summary>
        /// 点数マスタから在医総・施医総項目を取得する
        /// ※在医総はCdEdaNo = 0, 施医総はCdEdaNo = 2で見分けられる(201804時点)
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>点数マスタ情報</returns>
        //public List<TenMstModel> GetZaiisoList(int hpId, int sinDate)
        //{
        //    const string conFncName = nameof(GetZaiisoList);

        //    var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
        //            p.HpId == hpId &&
        //            p.StartDate <= sinDate &&
        //            p.EndDate >= sinDate &&
        //            p.CdKbn == "C" &&
        //            p.CdKbnno == 2 &&
        //            p.Kokuji2 == "1")
        //        .OrderBy(p => p.HpId)
        //        .ThenBy(p => p.ItemCd)
        //        .ThenBy(p => p.StartDate)
        //        .ToList();

        //    List<TenMstModel> results = new List<TenMstModel>();

        //    entities?.ForEach(entity =>
        //    {
        //        results.Add(new TenMstModel(entity));
        //    });

        //    return results;
        //}
        public List<string> GetZaiisoList(int hpId, int sinDate)
        {
            const string conFncName = nameof(GetZaiisoList);

            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.CdKbn == "C" &&
                    p.CdKbnno == 2 &&
                    p.Kokuji2 == "1")
                .Select(p => new { p.HpId, p.StartDate, p.ItemCd })
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<string> results = new List<string>();

            entities?.ForEach(entity =>
            {
                results.Add(entity.ItemCd);
            });

            return results;
        }
        /// <summary>
        /// 在宅療養指導管理料項目を取得する
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns></returns>
        public List<string> GetZaitakuryoyo(int hpId, int sinDate)
        {
            const string conFncName = nameof(GetZaitakuryoyo);

            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.CdKbn == "C" &&
                    ((p.CdKbnno >= 100 && p.CdKbnno <= 120) || (p.CdKbnno == 1)) &&
                    new string[] { "1", "3", "5" }.Contains(p.Kokuji2))
                .Select(p => new { p.HpId, p.StartDate, p.ItemCd })
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            List<string> results = new List<string>();

            entities?.ForEach(entity =>
            {
                results.Add(entity.ItemCd);
            });

            return results;
        }

        /// <summary>
        /// 外来感染症対策向上加算等の加算対象項目を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="koui">診療行為　1: 医学管理, 2:在宅医療</param>
        /// <returns>外来感染症対策向上加算等の加算対象項目</returns>
        public List<string> GetGairaiKansenTgt(int hpId, int sinDate, int koui)
        {
            const string conFncName = nameof(GetGairaiKansenTgt);

            List<string> results = new List<string>();

            var entities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.GairaiKansen == koui && //行為区分ごとに加算対象が異なる
                    new string[] { "1", "3", "5" }.Contains(p.Kokuji2)) //基本項目
                .Select(p => new { p.HpId, p.StartDate, p.ItemCd })
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate)
                .ToList();

            entities?.ForEach(entity =>
            {
                results.Add(entity.ItemCd);
            });

            return results;

        }

        /// <summary>
        /// 休日マスタ
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <returns></returns>
        public List<HolidayMstModel> FindHolidayMst(int hpId)
        {
            var entities = _tenantDataContext.HolidayMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.IsDeleted != DeleteStatus.DeleteFlag)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.SinDate)
                .ToList();

            List<HolidayMstModel> results = new List<HolidayMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new HolidayMstModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 指定日の休日区分を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定日の休日区分
        ///     0:マスタ登録なし
        ///     1:祝日
        ///     2:休診日
        /// </returns>
        public int GetHolidayKbn(int hpId, int sinDate)
        {
            var entities = _tenantDataContext.HolidayMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.SinDate == sinDate &&
                    p.IsDeleted != DeleteStatus.DeleteFlag)
                .ToList();

            if (entities.Any())
            {
                return entities[0].HolidayKbn;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 指定日が休日（休日加算が算定可能な日）かどうか判断する
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns>true - 休日（休日加算算定可） false - 休日以外（休日加算算定不可）</returns>
        public bool IsKyujitu(int hpId, int sinDate)
        {
            var entities = _tenantDataContext.HolidayMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.SinDate == sinDate &&
                    p.HolidayKbn > 0 &&
                    p.KyusinKbn > 0 &&
                    p.IsDeleted != DeleteStatus.DeleteFlag)
                .ToList();

            return entities.Any();

        }

        /// <summary>
        /// 自動算定マスタ
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<AutoSanteiMstModel> FindAutoSanteiMst(int hpId, int sinDate)
        {
            const string conFncName = nameof(FindAutoSanteiMst);

            var entities = _tenantDataContext.AutoSanteiMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ToList();

            List<AutoSanteiMstModel> results = new List<AutoSanteiMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new AutoSanteiMstModel(entity));
            });

            return results;
        }

        private List<int> unitCdls  = new List<int>
                {
                        53,121,131,138,141,142,143,144,145,146,147,148,159,997,998,999
                };

        /// <summary>
        /// 算定回数マスタから指定の項目の情報を取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns></returns>
        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisu(int hpId, int sinDate, bool isRosai, string itemCd)
        {
            const string conFncName = nameof(FindDensiSanteiKaisu);

            var entities = _tenantDataContext.DensiSanteiKaisus.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.ItemCd == itemCd &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isRosai ? 2 : 1)) &&
                    p.IsInvalid == 0 &&
                    unitCdls.Contains(p.UnitCd))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ToList();

            //_emrLogger.WriteLogMsg( this, conFncName, entities.AsQueryable().AsString());
            return entities.Select(p => new DensiSanteiKaisuModel(p)).ToList();
        }

        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisu(int hpId, int sinDate, bool isRosai, List<string> itemCds)
        {
            const string conFncName = nameof(FindDensiSanteiKaisu);

            var entities = _tenantDataContext.DensiSanteiKaisus.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    itemCds.Contains(p.ItemCd) &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isRosai ? 2 : 1)) &&
                    p.IsInvalid == 0 &&
                    unitCdls.Contains(p.UnitCd))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ToList();

            //_emrLogger.WriteLogMsg( this, conFncName, entities.AsQueryable().AsString());
            return entities.Select(p => new DensiSanteiKaisuModel(p)).ToList();
        }

        /// <summary>
        /// 電子算定回数マスタをすべて取得する
        /// </summary>
        /// <returns></returns>
        public List<DensiSanteiKaisuModel> FindAllDensiSanteiKaisu()
        {
            const string conFncName = nameof(FindAllDensiSanteiKaisu);

            var entities = _tenantDataContext.DensiSanteiKaisus.FindListQueryableNoTrack(p =>
                    p.HpId == Hardcode.HospitalID &&
                    p.IsInvalid == 0 &&
                    unitCdls.Contains(p.UnitCd))
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ToList();

            //_emrLogger.WriteLogMsg( this, conFncName, entities.AsQueryable().AsString());
            return entities.Select(p => new DensiSanteiKaisuModel(p)).ToList();
        }
        /// <summary>
        /// 項目グループマスタをすべて取得する
        /// </summary>
        /// <returns></returns>
        public List<ItemGrpMstModel> FindAllItemGrpMst()
        {
            const string conFncName = nameof(FindAllItemGrpMst);

            var entities = _tenantDataContext.itemGrpMsts.FindListQueryableNoTrack(p =>
                    p.HpId == Hardcode.HospitalID)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemGrpCd)
                .ThenBy(p => p.SeqNo)
                .ToList();

            return entities.Select(p => new ItemGrpMstModel(p)).ToList();
        }
        /// <summary>
        /// 労災合成マスタ
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<RousaiGoseiMstModel> FindRousaiGoseiMst(int hpId, int sinDate)
        {
            const string conFncName = nameof(FindRousaiGoseiMst);

            var entities = _tenantDataContext.RousaiGoseiMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.GoseiGrp)
                .ToList();

            List<RousaiGoseiMstModel> results = new List<RousaiGoseiMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new RousaiGoseiMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 電子点数表包括マスタ取得
        /// 当月算定している or 今回算定している項目に関連するマスタを取得
        /// 包括マスタは、最大でも当月内の設定しかないので、HOUKATU_GRPもHOUKATUも当月使用している項目のコードで絞り込む
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="isRosai">労災かどうか true:労災 false:労災以外</param>
        /// <param name="itemCds">今回算定している項目の診療行為コード</param>
        /// <returns>電子点数表包括マスタ</returns>
        public List<DensiHoukatuMstModel> GetDensiHoukatu(int hpId, long ptId, int sinDate, bool isRosai, List<string> itemCds, bool excludeMaybe)
        {
            int targetKbn = isRosai ? 2 : 1;
            int sinYm = sinDate / 100;
            var densiHoukatuGrps = _tenantDataContext.DensiHoukatuGrps.FindListQueryableNoTrack(d =>
                d.HpId == hpId &&
                d.StartDate <= sinDate &&
                d.EndDate >= sinDate &&
                itemCds.Contains(d.ItemCd) &&
                (d.TargetKbn == 0 || d.TargetKbn == (targetKbn)) &&
                (excludeMaybe ? d.SpJyoken == 0 : true) &&
                d.IsInvalid == 0);
            var densiHoukatus = _tenantDataContext.DensiHoukatus.FindListQueryableNoTrack(d =>
                d.HpId == hpId &&
                d.StartDate <= sinDate &&
                d.EndDate >= sinDate &&
                (d.TargetKbn == 0 || d.TargetKbn == (targetKbn)) &&
                ((d.HoukatuTerm >= 1 && d.HoukatuTerm <= 3) || d.HoukatuTerm == 99) &&
                d.IsInvalid == 0);
            // 当月の算定情報を取得
            var sinDtls = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
                s.HpId == hpId &&
                s.PtId == ptId &&
                s.SinYm == sinYm
                )
                .Select(p=>p.ItemCd)
                .Distinct().ToList();
            if(sinDtls != null)
            {
                itemCds.AddRange(sinDtls);
            }

            var joinQuery = (
                from densiHoukatuGrp in densiHoukatuGrps
                join densiHoukatu in densiHoukatus on
                    new { densiHoukatuGrp.HpId, densiHoukatuGrp.HoukatuGrpNo } equals
                    new { densiHoukatu.HpId, densiHoukatu.HoukatuGrpNo }
                where
                    densiHoukatuGrp.HpId == hpId &&
                    densiHoukatuGrp.StartDate <= sinDate &&
                    densiHoukatuGrp.EndDate >= sinDate &&
                    itemCds.Contains(densiHoukatuGrp.ItemCd) &&
                    densiHoukatuGrp.IsInvalid == 0 &&
                    //( sinDtls.Select(p=>p.ItemCd).Contains(densiHoukatu.ItemCd) ||
                      itemCds.Contains(densiHoukatu.ItemCd) 
                orderby
                    densiHoukatu.HoukatuTerm
                select new
                {
                    densiHoukatuGrp,
                    densiHoukatu
                }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new DensiHoukatuMstModel(
                        data.densiHoukatuGrp,
                        data.densiHoukatu
                    )
                )
                .OrderBy(p=>p.HoukatuGrpNo)
                .ThenBy(p=>p.DensiHoukatu.ItemCd)
                .ThenBy(p=>p.HoukatuTerm)
                .ThenBy(p=>p.ItemCd)
                .ThenByDescending(p => p.DensiHoukatuGrp.UserSetting)
                .ToList();

            List<DensiHoukatuMstModel> results = new List<DensiHoukatuMstModel>();

            string houkatuGrpNo = "";
            int houkatuTerm = 0;
            string itemCd = "";
            string houkatuItemCd = "";
            
            entities?.ForEach(entity => {
                if (houkatuGrpNo != entity.HoukatuGrpNo ||
                   houkatuItemCd != entity.DensiHoukatu.ItemCd || 
                   houkatuTerm != entity.HoukatuTerm ||
                   itemCd != entity.ItemCd)
                {
                    results.Add(new DensiHoukatuMstModel(entity.DensiHoukatuGrp, entity.DensiHoukatu));

                    houkatuGrpNo = entity.HoukatuGrpNo;
                    houkatuItemCd = entity.DensiHoukatu.ItemCd;
                    houkatuTerm = entity.HoukatuTerm;
                    itemCd = entity.ItemCd;
                }
            });

            return results;
        }

        /// <summary>
        /// 電子点数表包括マスタ取得
        /// 指定の項目が包括する項目のリストを取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="isRosai">労災かどうか true:労災 false:労災以外</param>
        /// <param name="itemCds">今回算定している項目の診療行為コード</param>
        /// <returns>電子点数表包括マスタ</returns>
        public List<DensiHoukatuMstModel> GetDensiHiHoukatu(int hpId, long ptId, int sinDate, bool isRosai, List<string> itemCds)
        {
            int targetKbn = isRosai ? 2 : 1;
            int sinYm = sinDate / 100;
            var densiHoukatuGrps = _tenantDataContext.DensiHoukatuGrps.FindListQueryableNoTrack(d =>
                d.HpId == hpId &&
                d.StartDate <= sinDate &&
                d.EndDate >= sinDate &&
                (d.TargetKbn == 0 || d.TargetKbn == (targetKbn)) &&
                d.IsInvalid == 0);
            var densiHoukatus = _tenantDataContext.DensiHoukatus.FindListQueryableNoTrack(d =>
                d.HpId == hpId &&
                d.StartDate <= sinDate &&
                d.EndDate >= sinDate &&
                itemCds.Contains(d.ItemCd) &&
                (d.TargetKbn == 0 || d.TargetKbn == (targetKbn)) &&
                (d.HoukatuTerm >= 1 && d.HoukatuTerm <= 3) &&
                d.IsInvalid == 0);
            //// 当月の算定情報を取得
            //var sinDtls = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
            //    s.HpId == hpId &&
            //    s.PtId == ptId &&
            //    s.SinYm == sinYm
            //    )
            //    .Select(p => p.ItemCd)
            //    .Distinct().ToList();
            //if (sinDtls != null)
            //{
            //    itemCds.AddRange(sinDtls);
            //}

            var joinQuery = (
                from densiHoukatuGrp in densiHoukatuGrps
                join densiHoukatu in densiHoukatus on
                    new { densiHoukatuGrp.HpId, densiHoukatuGrp.HoukatuGrpNo } equals
                    new { densiHoukatu.HpId, densiHoukatu.HoukatuGrpNo }
                where
                    densiHoukatuGrp.HpId == hpId &&
                    densiHoukatuGrp.StartDate <= sinDate &&
                    densiHoukatuGrp.EndDate >= sinDate &&
                    //itemCds.Contains(densiHoukatuGrp.ItemCd) &&
                    densiHoukatuGrp.IsInvalid == 0 
                      //( sinDtls.Select(p=>p.ItemCd).Contains(densiHoukatu.ItemCd) ||
                      //itemCds.Contains(densiHoukatu.ItemCd)
                orderby
                    densiHoukatu.HoukatuTerm
                select new
                {
                    densiHoukatuGrp,
                    densiHoukatu
                }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new DensiHoukatuMstModel(
                        data.densiHoukatuGrp,
                        data.densiHoukatu
                    )
                )
                .ToList();

            List<DensiHoukatuMstModel> results = new List<DensiHoukatuMstModel>();

            entities?.ForEach(entity => {
                results.Add(new DensiHoukatuMstModel(entity.DensiHoukatuGrp, entity.DensiHoukatu));
            });

            return results;
        }

        /// <summary>
        /// 背反マスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <param name="isRosai"></param>
        /// <param name="itemCds"></param>
        /// <returns></returns>
        public List<DensiHaihanMstModel> GetDensiHaihanAll(int hpId, int sinDate, bool isRosai, List<string> itemCds, bool excludeMaybe)
        {
            const string conFncName = nameof(GetDensiHaihanDay);

            // 0:同日
            var densiHaihanDay = _tenantDataContext.DensiHaihanDays.FindListQueryableNoTrack(d =>
                    d.HpId == hpId &&
                    d.StartDate <= sinDate &&
                    d.EndDate >= sinDate &&
                    itemCds.Contains(d.ItemCd1) &&
                    (d.HaihanKbn == 2 || d.HaihanKbn == 3) &&
                    (d.TargetKbn == 0 || d.TargetKbn == (isRosai ? 2 : 1)) &&
                    (excludeMaybe ? d.SpJyoken == 0 : true) &&
                    (excludeMaybe ? d.HaihanKbn != 3 : true) &&
                    d.IsInvalid == 0)
                    .Select(p=>
                        new {
                            HpId = p.HpId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            ItemCd1 = p.ItemCd1,
                            ItemCd2 = p.ItemCd2,
                            HaihanKbn = p.HaihanKbn,
                            SpJyoken = p.SpJyoken,
                            TermCnt = 1,
                            TermSbt = 2,
                            TargetKbn = p.TargetKbn,
                            UserSetting = p.UserSetting,
                            MstSbt = 0});

            // 1:同月
            var densiHaihanMonth = _tenantDataContext.DensiHaihanMonths.FindListQueryableNoTrack(d =>
                    d.HpId == hpId &&
                    d.StartDate <= sinDate &&
                    d.EndDate >= sinDate &&
                    itemCds.Contains(d.ItemCd1) &&
                    (d.HaihanKbn == 2 || d.HaihanKbn == 3) &&
                    (d.TargetKbn == 0 || d.TargetKbn == (isRosai ? 2 : 1)) &&
                    (excludeMaybe ? d.SpJyoken == 0 : true) &&
                    (excludeMaybe ? d.HaihanKbn != 3 : true) &&
                    d.IsInvalid == 0)
                    .Select(p =>
                        new {
                            HpId = p.HpId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            ItemCd1 = p.ItemCd1,
                            ItemCd2 = p.ItemCd2,
                            HaihanKbn = p.HaihanKbn,
                            SpJyoken = p.SpJyoken,
                            TermCnt = 1,
                            TermSbt = 6,
                            TargetKbn = p.TargetKbn,
                            UserSetting = p.UserSetting,
                            MstSbt = 1
                        });

            // 2:同週
            var densiHaihanWeek = _tenantDataContext.DensiHaihanWeeks.FindListQueryableNoTrack(d =>
                    d.HpId == hpId &&
                    d.StartDate <= sinDate &&
                    d.EndDate >= sinDate &&
                    itemCds.Contains(d.ItemCd1) &&
                    (d.HaihanKbn == 2 || d.HaihanKbn == 3) &&
                    (d.TargetKbn == 0 || d.TargetKbn == (isRosai ? 2 : 1)) &&
                    (excludeMaybe ? d.SpJyoken == 0 : true) &&
                    (excludeMaybe ? d.HaihanKbn != 3 : true) &&
                    d.IsInvalid == 0)
                    .Select(p =>
                        new {
                            HpId = p.HpId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            ItemCd1 = p.ItemCd1,
                            ItemCd2 = p.ItemCd2,
                            HaihanKbn = p.HaihanKbn,
                            SpJyoken = p.SpJyoken,
                            TermCnt = 1,
                            TermSbt = 3,
                            TargetKbn = p.TargetKbn,
                            UserSetting = p.UserSetting,
                            MstSbt = 2
                        });

            // 3:同来院
            var densiHaihanKarte = _tenantDataContext.DensiHaihanKartes.FindListQueryableNoTrack(d =>
                    d.HpId == hpId &&
                    d.StartDate <= sinDate &&
                    d.EndDate >= sinDate &&
                    itemCds.Contains(d.ItemCd1) &&
                    (d.HaihanKbn == 2 || d.HaihanKbn == 3) &&
                    (d.TargetKbn == 0 || d.TargetKbn == (isRosai ? 2 : 1)) &&
                    (excludeMaybe ? d.SpJyoken == 0 : true) &&
                    (excludeMaybe ? d.HaihanKbn != 3 : true) &&
                    d.IsInvalid == 0)
                    .Select(p =>
                        new {
                            HpId = p.HpId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            ItemCd1 = p.ItemCd1,
                            ItemCd2 = p.ItemCd2,
                            HaihanKbn = p.HaihanKbn,
                            SpJyoken = p.SpJyoken,
                            TermCnt = 1,
                            TermSbt = 1,
                            TargetKbn = p.TargetKbn,
                            UserSetting = p.UserSetting,
                            MstSbt = 3
                        });

            // カスタム
            var densiHaihanCustom = _tenantDataContext.DensiHaihanCustoms.FindListQueryableNoTrack(d =>
                    d.HpId == hpId &&
                    d.StartDate <= sinDate &&
                    d.EndDate >= sinDate &&
                    itemCds.Contains(d.ItemCd1) &&
                    (d.HaihanKbn == 2 || d.HaihanKbn == 3) &&
                    (d.TargetKbn == 0 || d.TargetKbn == (isRosai ? 2 : 1)) &&
                    (excludeMaybe ? d.SpJyoken == 0 : true) &&
                    (excludeMaybe ? d.HaihanKbn != 3 : true) &&
                    d.IsInvalid == 0)
                    .Select(p =>
                        new {
                            HpId = p.HpId,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            ItemCd1 = p.ItemCd1,
                            ItemCd2 = p.ItemCd2,
                            HaihanKbn = p.HaihanKbn,
                            SpJyoken = p.SpJyoken,
                            TermCnt = p.TermCnt,
                            TermSbt = p.TermSbt,
                            TargetKbn = p.TargetKbn,
                            UserSetting = p.UserSetting,
                            MstSbt = 4
                        });

            var unionQuery = 
                densiHaihanDay.Union(densiHaihanMonth).Union(densiHaihanWeek).Union(densiHaihanKarte).Union(densiHaihanCustom)
                    .OrderBy(p => p.MstSbt)    
                    .ThenBy(p => p.TermSbt)
                    .ThenBy(p => p.TermCnt)
                    .ThenBy(p => p.ItemCd1)
                    .ThenBy(p => p.ItemCd2)
                    //.ThenBy(p => p.StartDate)
                    .ThenByDescending(p => p.UserSetting)
                    .ThenBy(p => p.HaihanKbn)
                    .ThenBy(p => p.SpJyoken);

            int mstSbt = 0;
            int termSbt = 0;
            int termCnt = 0;
            string itemCd1 = "";
            string itemCd2 = "";


            List<DensiHaihanMstModel> results = new List<DensiHaihanMstModel>();
            foreach(var union in unionQuery)
            {
                if (mstSbt != union.MstSbt ||
                   termSbt != union.TermSbt ||
                   termCnt != union.TermCnt ||
                   itemCd1 != union.ItemCd1 ||
                   itemCd2 != union.ItemCd2)
                {
                    results.Add(new DensiHaihanMstModel());
                    results.Last().HpId = union.HpId;
                    results.Last().StartDate = union.StartDate;
                    results.Last().EndDate = union.EndDate;
                    results.Last().ItemCd1 = union.ItemCd1;
                    results.Last().ItemCd2 = union.ItemCd2;
                    results.Last().HaihanKbn = union.HaihanKbn;
                    results.Last().SpJyoken = union.SpJyoken;
                    results.Last().TermCnt = union.TermCnt;
                    results.Last().termSbt = union.TermSbt;
                    results.Last().TargetKbn = union.TargetKbn;
                    results.Last().mstSbt = union.MstSbt;

                    mstSbt = union.MstSbt;
                    termSbt = union.TermSbt;
                    termCnt = union.TermCnt;
                    itemCd1 = union.ItemCd1;
                    itemCd2 = union.ItemCd2;
                }
            }
            return results;
        }

        /// <summary>
        /// 背反マスタ（日）
        /// ITEM_CD1をキーに検索
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<DensiHaihanDayModel> GetDensiHaihanDay(int hpId, int sinDate, bool isKenpo, string itemCd)
        {
            const string conFncName = nameof(GetDensiHaihanDay);

            var entities = _tenantDataContext.DensiHaihanDays.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.ItemCd1 == itemCd &&
                    (p.HaihanKbn == 2 || p.HaihanKbn == 3) &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isKenpo ? 1 : 2)) &&
                    p.IsInvalid == 0)
                .ToList();

            List<DensiHaihanDayModel> results = new List<DensiHaihanDayModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new DensiHaihanDayModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 背反マスタ（月）
        /// ITEM_CD1をキーに検索
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<DensiHaihanMonthModel> GetDensiHaihanMonth(int hpId, int sinDate, bool isKenpo, string itemCd)
        {
            const string conFncName = nameof(GetDensiHaihanMonth);

            var entities = _tenantDataContext.DensiHaihanMonths.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.ItemCd1 == itemCd &&
                    (p.HaihanKbn == 2 || p.HaihanKbn == 3) &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isKenpo ? 1 : 2)) &&
                    p.IsInvalid == 0)
                .ToList();

            List<DensiHaihanMonthModel> results = new List<DensiHaihanMonthModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new DensiHaihanMonthModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 背反マスタ（週）
        /// ITEM_CD1をキーに検索
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<DensiHaihanWeekModel> GetDensiHaihanWeek(int hpId, int sinDate, bool isKenpo, string itemCd)
        {
            const string conFncName = nameof(GetDensiHaihanWeek);

            var entities = _tenantDataContext.DensiHaihanWeeks.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.ItemCd1 == itemCd &&
                    (p.HaihanKbn == 2 || p.HaihanKbn == 3) &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isKenpo ? 1 : 2)) &&
                    p.IsInvalid == 0)
                .ToList();

            List<DensiHaihanWeekModel> results = new List<DensiHaihanWeekModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new DensiHaihanWeekModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 背反マスタ（来院）
        /// ITEM_CD1をキーに検索
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<DensiHaihanKarteModel> GetDensiHaihanKarte(int hpId, int sinDate, bool isKenpo, string itemCd)
        {
            const string conFncName = nameof(GetDensiHaihanKarte);

            var entities = _tenantDataContext.DensiHaihanKartes.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.ItemCd1 == itemCd &&
                    (p.HaihanKbn == 2 || p.HaihanKbn == 3) &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isKenpo ? 1 : 2)) &&
                    p.IsInvalid == 0)
                .ToList();

            List<DensiHaihanKarteModel> results = new List<DensiHaihanKarteModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new DensiHaihanKarteModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 背反マスタ（カスタム）
        /// ITEM_CD1をキーに検索
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<DensiHaihanCustomModel> GetDensiHaihanCustom(int hpId, int sinDate, bool isKenpo, string itemCd)
        {
            const string conFncName = nameof(GetDensiHaihanCustom);

            var entities = _tenantDataContext.DensiHaihanCustoms.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.ItemCd1 == itemCd &&
                    (p.HaihanKbn == 2 || p.HaihanKbn == 3) &&
                    (p.TargetKbn == 0 || p.TargetKbn == (isKenpo ? 1 : 2)) &&
                    p.IsInvalid == 0)
                .ToList();

            List<DensiHaihanCustomModel> results = new List<DensiHaihanCustomModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new DensiHaihanCustomModel(entity));
            });

            return results;
        }

        public List<PriorityHaihanMstModel> GetPriorityHaihanAll(int hpId, int sinDate, bool isRosai, List<string> itemCds, bool excludeMaybe)
        {
            const string conFncName = nameof(GetPriorityHaihanAll);

            var priorityHaihans = _tenantDataContext.PriorityHaihanMsts.FindListQueryableNoTrack(d =>
                    d.HpId == hpId &&
                    d.StartDate <= sinDate &&
                    d.EndDate >= sinDate &&
                    (itemCds.Contains(d.ItemCd3) ||
                     itemCds.Contains(d.ItemCd4) ||
                     itemCds.Contains(d.ItemCd5) ||
                     itemCds.Contains(d.ItemCd6) ||
                     itemCds.Contains(d.ItemCd7) ||
                     itemCds.Contains(d.ItemCd8) ||
                     itemCds.Contains(d.ItemCd9)) &&
                    (d.TargetKbn == 0 || d.TargetKbn == (isRosai ? 2 : 1)) &&
                    (excludeMaybe ? d.SpJyoken == 0 : true) &&
                    d.IsInvalid == 0)
                    .Select(p =>
                        new
                        {
                            p
                        });
                        
            List<PriorityHaihanMstModel> results = new List<PriorityHaihanMstModel>();
            foreach (var priorityHaihan in priorityHaihans)
            {
                results.Add(new PriorityHaihanMstModel(priorityHaihan.p));
            }
            return results;
        }

        /// <summary>
        /// システム世代設定
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <returns></returns>
        public List<SystemGenerationConfModel> FindSystemGenerationConf(int hpId, int sinDate)
        {
            var entities = _tenantDataContext.SystemGenerationConfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate
                    )
                .ToList();

            List<SystemGenerationConfModel> results = new List<SystemGenerationConfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new SystemGenerationConfModel(entity));
            });

            return results;
        }

        public List<IpnKasanExcludeModel> FindIpnKasanExclude(int hpId, int sinDate)
        {
            var entities = _tenantDataContext.ipnKasanExcludes.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate
                    )
                .ToList();

            List<IpnKasanExcludeModel> results = new List<IpnKasanExcludeModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new IpnKasanExcludeModel(entity));
            });

            return results;
        }

        public List<IpnKasanExcludeItemModel> FindIpnKasanExcludeItem(int hpId, int sinDate)
        {
            var entities = _tenantDataContext.ipnKasanExcludeItems.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate
                    )
                .ToList();

            List<IpnKasanExcludeItemModel> results = new List<IpnKasanExcludeItemModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new IpnKasanExcludeItemModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 一般名処方加算マスタを取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="ipnNameCd">一般名コード</param>
        /// <returns></returns>
        public List<IpnKasanMstModel> FindIpnKasanMst(int hpId, int sinDate, string ipnNameCd)
        {
            var entities = _tenantDataContext.IpnKasanMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.EndDate >= sinDate &&
                i.IpnNameCd == ipnNameCd &&
                i.IsDeleted == DeleteStatus.None)
                .OrderByDescending(p => p.StartDate)
                .ToList();

            List<IpnKasanMstModel> results = new List<IpnKasanMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new IpnKasanMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 一般名処方加算マスタを取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="ipnNameCds">一般名コードのリスト</param>
        /// <returns></returns>
        public List<IpnKasanMstModel> FindIpnKasanMst(int hpId, int sinDate, List<string> ipnNameCds)
        {
            var entities = _tenantDataContext.IpnKasanMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.EndDate >= sinDate &&
                ipnNameCds.Contains(i.IpnNameCd) &&
                i.IsDeleted == DeleteStatus.None)
                .OrderByDescending(p => p.StartDate)
                .ToList();

            List<IpnKasanMstModel> results = new List<IpnKasanMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new IpnKasanMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 最低薬価マスタを取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="ipnNameCd">一般名コード</param>
        /// <returns>
        /// 該当する最低薬価マスタを開始日降順に並べたリスト
        /// 先頭行が最新
        /// </returns>
        public List<IpnMinYakkaMstModel> FindIpnMinYakkaMst(int hpId, int sinDate, string ipnNameCd)
        {
            var entities = _tenantDataContext.IpnMinYakkaMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.EndDate >= sinDate &&
                i.IpnNameCd == ipnNameCd &&
                i.IsDeleted == DeleteStatus.None)
                .OrderByDescending(p=>p.StartDate)
                .ToList();

            List<IpnMinYakkaMstModel> results = new List<IpnMinYakkaMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new IpnMinYakkaMstModel(entity));
            });

            return results;
        }
        /// <summary>
        /// 最低薬価マスタを取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="ipnNameCd">一般名コードのリスト</param>
        /// <returns>
        /// </returns>
        public List<IpnMinYakkaMstModel> FindIpnMinYakkaMst(int hpId, int sinDate, List<string> ipnNameCd)
        {
            var entities = _tenantDataContext.IpnMinYakkaMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.EndDate >= sinDate &&
                ipnNameCd.Contains(i.IpnNameCd) &&
                i.IsDeleted == DeleteStatus.None)
                .OrderByDescending(p => p.StartDate)
                .ToList();

            List<IpnMinYakkaMstModel> results = new List<IpnMinYakkaMstModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new IpnMinYakkaMstModel(entity));
            });

            return results;
        }
        public KaMst FindKaMst(int hpId, int kaId)
        {
            var entities = _tenantDataContext.KaMsts.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.KaId == kaId &&
                p.IsDeleted == DeleteStatus.None)
                .FirstOrDefault();
            
            if (entities == null)
            {
                return null;
            }
            return entities;

        }
        /// <summary>
        /// システム世代設定取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <param name="grpCd"></param>
        /// <param name="grpEdaNo"></param>
        /// <returns></returns>
        public List<SystemGenerationConfModel> FindSystemGenerationConf(int hpId, int sinDate, int grpCd, int grpEdaNo = -1)
        {
            var entities = _tenantDataContext.SystemGenerationConfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.GrpCd == grpCd &&
                (grpEdaNo >= 0 ? p.GrpEdaNo == grpEdaNo : true) &&
                p.StartDate <= sinDate &&
                p.EndDate >= sinDate
            )
           .ToList();

            List<SystemGenerationConfModel> results = new List<SystemGenerationConfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new SystemGenerationConfModel(entity));
            });

            return results;
        }

        /// <summary>
        /// レセ電コメント関連マスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <returns></returns>
        public List<RecedenCmtSelectModel> FindRecedenCmtSelect(int hpId, int sinDate)
        {
            var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate //&&
                    //p.CmtSbt > 0
                    )
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ThenByDescending(p => p.StartDate);
            var recedenCmtSelects = _tenantDataContext.RecedenCmtSelects.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.StartDate <= sinDate &&
                p.EndDate >= sinDate &&
                p.IsInvalid != 1
            );

            var entities =
                (
                from recedenCmtSelect in recedenCmtSelects
                join tenMst in tenMsts on
                    new { recedenCmtSelect.HpId, ItemCd = recedenCmtSelect.CommentCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } 
                select new
                {
                    recedenCmtSelect,
                    cmtsbt = tenMst == null ? 0 : tenMst.CmtSbt,
                    cmtcol1 = tenMst == null ? 0 : tenMst.CmtCol1,
                    cmtcolketa1 = tenMst == null ? 0 : tenMst.CmtColKeta1,
                    cmtcol2 = tenMst == null ? 0 : tenMst.CmtCol2,
                    cmtcolketa2 = tenMst == null ? 0 : tenMst.CmtColKeta2,
                    cmtcol3 = tenMst == null ? 0 : tenMst.CmtCol3,
                    cmtcolketa3 = tenMst == null ? 0 : tenMst.CmtColKeta3,
                    cmtcol4 = tenMst == null ? 0 : tenMst.CmtCol4,
                    cmtcolketa4 = tenMst == null ? 0 : tenMst.CmtColKeta4,
                    name = tenMst == null ? "" : tenMst.Name
                }).ToList();

            List<RecedenCmtSelectModel> results = new List<RecedenCmtSelectModel>();

            entities?.ForEach(entity =>
                {
                    results.Add(
                        new RecedenCmtSelectModel(
                            entity.recedenCmtSelect, 
                            entity.cmtsbt,
                            entity.cmtcol1,
                            entity.cmtcolketa1,
                            entity.cmtcol2,
                            entity.cmtcolketa2,
                            entity.cmtcol3,
                            entity.cmtcolketa3,
                            entity.cmtcol4,
                            entity.cmtcolketa4,
                            entity.name
                            ));
                }
            );

            return results;
        }
        /// <summary>
        /// 電子算定回数マスタをすべて取得する
        /// </summary>
        /// <returns></returns>
        public List<KouiHoukatuMstModel> FindKouiHoukatuMst()
        {
            const string conFncName = nameof(FindKouiHoukatuMst);

            var entities = _tenantDataContext.KouiHoukatuMsts.FindListQueryableNoTrack(p =>
                    p.HpId == Session.HospitalID &&
                    p.IsInvalid == 0)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.ItemCd)
                .ToList();

            return entities.Select(p => new KouiHoukatuMstModel(p)).ToList();
        }
        public TenMstModel FindTenMstBySanteiItemCd(string santeiItemCd, int sinDate)
        {
            var entity = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                   p.HpId == Hardcode.HospitalID &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.SanteiItemCd == santeiItemCd)
               .FirstOrDefault();

            if (entity != null)
            {
                return new TenMstModel(entity);
            }
            return null;
        }

        /// <summary>
        /// コメント項目を文字列展開する
        /// </summary>
        /// <param name="sinDate">診療日</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="cmtCol">位置（1文字目を1とした位置）※C#の文字列は0始まりで処理するので注意！</param>
        /// <param name="cmtLen">長さ</param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="comment">コメント文</param>
        /// <param name="maskEdit"></param>
        /// <returns>展開したコメントを返す</returns>
        public string GetCommentStr(int sinDate, string itemCd, List<int> cmtCol, List<int> cmtLen, string oldName, string newName, ref string comment, bool maskEdit = false)
        {
            string ret = oldName;

            string _getReceName(string santeiItemCd)
            {
                string receName = "";

                TenMstModel tenMst = FindTenMstBySanteiItemCd(CIUtil.ToNarrow(santeiItemCd), sinDate);

                if (tenMst != null)
                {
                    receName = tenMst.ReceName;
                }

                return receName;
            }

            // 半角→全角で、全角に統一する
            comment = CIUtil.ToWide(comment);

            switch (CIUtil.Copy(itemCd, 1, 3))
            {
                case "810":
                    ret = comment;
                    break;
                case "820":
                    ret = newName;
                    break;
                case "830":
                    ret = newName + CIUtil.ToWide(comment);
                    break;
                case "831":
                    ret = newName + _getReceName(comment);
                    break;
                case "840":
                    if (itemCd == ItemCdConst.GazoDensibaitaiHozon)
                    {
                        newName = "電子媒体保存撮影　　　回";
                    }

                    int maxLen = 0;

                    for (int i = 0; i < cmtLen.Count; i++)
                    {
                        if (cmtLen[i] == 0) break;
                        maxLen += cmtLen[i];
                    }

                    if (maxLen > 0)
                    {
                        if (maskEdit == true && maxLen > comment.Length)
                        {
                            comment = comment + new string('＊', maxLen - comment.Length);
                        }

                        int num; // TryParse dummy
                        if (comment.Length < maxLen && int.TryParse(CIUtil.ToNarrow(comment), out num))
                        {
                            comment = new string('０', maxLen - comment.Length) + comment;
                        }

                        if (maxLen == comment.Length)
                        {
                            ret = newName;
                            int start = 0;

                            for (int i = 0; i < cmtCol.Count; i++)
                            {
                                if (cmtCol[i] <= 0) break;

                                ret = ret.Remove(cmtCol[i] - 1, cmtLen[i]).Insert(cmtCol[i] - 1, comment.Substring(start, cmtLen[i]));
                                start += cmtLen[i];
                            }
                        }
                    }

                    break;
                case "842":
                    ret = newName + CIUtil.ToWide(comment);
                    break;
                case "850":

                    string gengo = "";

                    if (comment.Length == 8)
                    {
                        //西暦を和暦に変換
                        int num;
                        if (int.TryParse(CIUtil.ToNarrow(comment), out num))
                        {
                            comment = CIUtil.ToWide(CIUtil.SDateToWDate(num).ToString());
                        }
                    }

                    switch (CIUtil.Copy(comment, 1, 1))
                    {
                        case "１": gengo = "明治"; break;
                        case "２": gengo = "大正"; break;
                        case "３": gengo = "昭和"; break;
                        case "４": gengo = "平成"; break;
                        case "５": gengo = "令和"; break;
                    }

                    if (newName.IndexOf("日") >= 0)
                    {
                        ret = $"{newName}{gengo}{CIUtil.Copy(comment, 2, 2)}年{CIUtil.Copy(comment, 4, 2)}月{CIUtil.Copy(comment, 6, 2)}日";
                    }
                    else
                    {
                        ret = $"{newName}{gengo}{CIUtil.Copy(comment, 2, 2)}年{CIUtil.Copy(comment, 4, 2)}月";
                    }

                    break;
                case "851":
                    ret = $"{newName}{CIUtil.Copy(comment, 1, 2)}時{CIUtil.Copy(comment, 3, 2)}分";
                    break;
                case "852":
                    ret = $"{newName}{comment.TrimStart('０')}分";
                    break;
                case "853":
                    ret = $"{newName}{CIUtil.Copy(comment, 1, 2)}日　{CIUtil.Copy(comment, 3, 2)}時{CIUtil.Copy(comment, 5, 2)}分";
                    break;
                case "880":

                    string gengo2 = "";
                    string cmtOptDate = CIUtil.Copy(comment, 1, 7);
                    string cmtOptValue = CIUtil.Copy(comment, 8, 8);

                    switch (CIUtil.Copy(cmtOptDate, 1, 1))
                    {
                        case "１": gengo2 = "明治"; break;
                        case "２": gengo2 = "大正"; break;
                        case "３": gengo2 = "昭和"; break;
                        case "４": gengo2 = "平成"; break;
                        case "５": gengo2 = "令和"; break;
                    }

                    if (CIUtil.StrToIntDef(CIUtil.ToNarrow(cmtOptValue), -1) != 0)
                    {
                        cmtOptValue = CIUtil.ToNarrow(cmtOptValue).TrimStart('0');
                        if (CIUtil.Copy(cmtOptValue, 1, 1) == ".")
                        {
                            cmtOptValue = "0" + cmtOptValue;
                        }
                    }
                    else
                    {
                        cmtOptValue = "0";
                    }

                    ret = $"{newName}{gengo2}{CIUtil.Copy(cmtOptDate, 2, 2)}年{CIUtil.Copy(cmtOptDate, 4, 2)}月{CIUtil.Copy(cmtOptDate, 6, 2)}日　検査値：{CIUtil.ToWide(cmtOptValue)}";
                    break;
            }

            return ret;

        }

        public int GetPrefCd(int hpId, int sinDate)
        {
            var hospitalInfo = _tenantDataContext.HpInfs
                .Where(p => p.HpId == hpId && p.StartDate <= sinDate)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;
            if (hospitalInfo != null)
            {
                prefCd = hospitalInfo.PrefNo;
            }

            return prefCd;
        }
    }
}
