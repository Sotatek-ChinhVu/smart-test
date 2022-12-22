using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using Domain.Constant;

namespace EmrCalculateApi.Ika.DB.Finder
{
    public class IkaCalculateFinder
    {
        private int hospitalId = Hardcode.HospitalID;
        private readonly TenantDataContext _tenantDataContext;

        public IkaCalculateFinder(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        /// <summary>
        /// 患者情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>患者情報</returns>
        public PtInfModel FindPtInf(int hpId, long ptId, int sinDate)
        {
            PtInf ptInf = _tenantDataContext.PtInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.IsDelete == DeleteStatus.None
            ).FirstOrDefault();

            if (ptInf == null)
            {
                return null;
            }
            return new PtInfModel(ptInf, sinDate);
        }

        /// <summary>
        /// 患者妊娠情報
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <returns>患者妊娠情報</returns>
        public List<PtPregnancyModel> FindPtPregnancy(int hpId, long ptId, int sinDate)
        {
            var entities = _tenantDataContext.PtPregnancies.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.StartDate > 0 &&
                    p.StartDate <= sinDate && 
                    (p.EndDate >= sinDate || p.EndDate == 0) &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.StartDate)
                .ToList();

            List<PtPregnancyModel> results = new List<PtPregnancyModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new PtPregnancyModel(entity));
            });

            return results;

        }

        public List<PtHokenPatternModel> FindPtHokenPattern(int hpId, long ptId, int sinDate)
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

            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    //p.StartDate <= sinDate &&
                    //p.EndDate >= sinDate &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.StartDate)
                .ToList();

            var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(h => 
                h.HpId == hpId && 
                (h.PrefNo == 0 || h.PrefNo == prefCd) && 
                h.StartDate <= sinDate);
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(h => 
                h.HpId == hpId && 
                (h.PrefNo == 0 || h.PrefNo == prefCd) && 
                h.StartDate <= sinDate
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

            var kohiPriorities = _tenantDataContext.KohiPriorities.FindListQueryableNoTrack();
            var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack(p=>p.HpId == hpId && p.PtId == ptId);
            //保険番号マスタの取得
            var houbetuMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select new
                {
                    hokenMst.HpId,
                    hokenMst.PrefNo,
                    hokenMst.HokenNo,
                    hokenMst.HokenEdaNo,
                    hokenMst.Houbetu,
                    hokenMst.HokenSbtKbn
                }
            );

            //公費の優先順位を取得
            var ptKohiQuery = (
                from ptKohi in ptKohis
                join houbetuMst in houbetuMsts on
                    new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                    new { houbetuMst.HpId, houbetuMst.HokenNo, houbetuMst.HokenEdaNo, houbetuMst.PrefNo }
                join kPriority in kohiPriorities on
                    new { houbetuMst.PrefNo, houbetuMst.Houbetu } equals
                    new { kPriority.PrefNo, kPriority.Houbetu } into kohiPriorityJoin
                from kohiPriority in kohiPriorityJoin.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    ptKohi.IsDeleted == DeleteStatus.None
                select new
                {
                    ptKohi.HpId,
                    ptKohi.PtId,
                    ptKohi.HokenNo,
                    ptKohi.HokenEdaNo,
                    KohiId = ptKohi.HokenId,
                    ptKohi.PrefNo,
                    houbetuMst.Houbetu,
                    houbetuMst.HokenSbtKbn,
                    //kohiPriority.PriorityNo
                    PriorityNo = kohiPriority == null ? "99999" : kohiPriority.PriorityNo
                }
            );

            var entities = (
                from hokPattern in ptHokenPatterns
                join ptKohis1 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi1Id } equals
                    new { ptKohis1.HpId, ptKohis1.PtId, Kohi1Id = ptKohis1.KohiId } into kohi1Join
                from ptKohi1 in kohi1Join.DefaultIfEmpty()
                join ptKohis2 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi2Id } equals
                    new { ptKohis2.HpId, ptKohis2.PtId, Kohi2Id = ptKohis2.KohiId } into kohi2Join
                from ptKohi2 in kohi2Join.DefaultIfEmpty()
                join ptKohis3 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi3Id } equals
                    new { ptKohis3.HpId, ptKohis3.PtId, Kohi3Id = ptKohis3.KohiId } into kohi3Join
                from ptKohi3 in kohi3Join.DefaultIfEmpty()
                join ptKohis4 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi4Id } equals
                    new { ptKohis4.HpId, ptKohis4.PtId, Kohi4Id = ptKohis4.KohiId } into kohi4Join
                from ptKohi4 in kohi4Join.DefaultIfEmpty()
                where
                    hokPattern.HpId == hpId &&
                    hokPattern.PtId == ptId &&
                    hokPattern.IsDeleted == DeleteStatus.None
                select new
                {
                    hokPattern,
                    Kohi1Houbetu = ptKohi1 == null ? "99" : ptKohi1.Houbetu,
                    kohi1HokenSbtKbn = ptKohi1 == null ? 0 : ptKohi1.HokenSbtKbn,
                    kohi1HokenNo = ptKohi1 == null ? 0 : ptKohi1.HokenNo,
                    kohi1HokenEdaNo = ptKohi1 == null ? 0 : ptKohi1.HokenEdaNo,
                    Kohi1PriorityNo = ptKohi1 == null ? "99999" : ptKohi1.PriorityNo,
                    Kohi2Houbetu = ptKohi2 == null ? "99" : ptKohi2.Houbetu,
                    kohi2HokenSbtKbn = ptKohi2 == null ? 0 : ptKohi2.HokenSbtKbn,
                    kohi2HokenNo = ptKohi2 == null ? 0 : ptKohi2.HokenNo,
                    kohi2HokenEdaNo = ptKohi2 == null ? 0 : ptKohi2.HokenEdaNo,
                    Kohi2PriorityNo = ptKohi2 == null ? "99999" : ptKohi2.PriorityNo,
                    Kohi3Houbetu = ptKohi3 == null ? "99" : ptKohi3.Houbetu,
                    kohi3HokenSbtKbn = ptKohi3 == null ? 0 : ptKohi3.HokenSbtKbn,
                    kohi3HokenNo = ptKohi3 == null ? 0 : ptKohi3.HokenNo,
                    kohi3HokenEdaNo = ptKohi3 == null ? 0 : ptKohi3.HokenEdaNo,
                    Kohi3PriorityNo = ptKohi3 == null ? "99999" : ptKohi3.PriorityNo,
                    Kohi4Houbetu = ptKohi4 == null ? "99" : ptKohi4.Houbetu,
                    kohi4HokenSbtKbn = ptKohi4 == null ? 0 : ptKohi4.HokenSbtKbn,
                    kohi4HokenNo = ptKohi4 == null ? 0 : ptKohi4.HokenNo,
                    kohi4HokenEdaNo = ptKohi4 == null ? 0 : ptKohi4.HokenEdaNo,
                    Kohi4PriorityNo = ptKohi4 == null ? "99999" : ptKohi4.PriorityNo,
                }
            ).ToList();

            List<PtHokenPatternModel> results = new List<PtHokenPatternModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new PtHokenPatternModel(
                        entity.hokPattern, 
                        entity.Kohi1Houbetu, entity.kohi1HokenSbtKbn, entity.kohi1HokenNo, entity.kohi1HokenEdaNo, entity.Kohi1PriorityNo,
                        entity.Kohi2Houbetu, entity.kohi2HokenSbtKbn, entity.kohi2HokenNo, entity.kohi2HokenEdaNo, entity.Kohi2PriorityNo,
                        entity.Kohi3Houbetu, entity.kohi3HokenSbtKbn, entity.kohi3HokenNo, entity.kohi3HokenEdaNo, entity.Kohi3PriorityNo,
                        entity.Kohi4Houbetu, entity.kohi4HokenSbtKbn, entity.kohi4HokenNo, entity.kohi4HokenEdaNo, entity.Kohi4PriorityNo));
            });

            return results;
        }

        /// <summary>
        /// 計算要求取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <returns></returns>
        public CalcStatusModel GetCalcStatus(int hpId, long ptId, int sinDate, string preFix)
        {
            CalcStatusModel calcStatus = null;
            string computerName = (preFix + Hardcode.ComputerName).ToUpper();

            if (hpId >= 0 && ptId >= 0)
            {
                // 指定の患者、診療日の要求を優先してチェック
                //var entities = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                //    p.CreateMachine.ToUpper() == computerName &&
                //    p.HpId == hpId &&
                //    p.PtId == ptId &&
                //    p.SinDate == sinDate &&
                //    p.CalcMode == 0 &&
                //    p.Status == 0)
                //    .OrderBy(p=>p.CalcId)
                //    .ToList();

                //if (entities != null && entities.Any())
                //{
                //    //calcStatus = new CalcStatusModel(entities.OrderByDescending(p => p.CalcId).First());
                //    calcStatus = new CalcStatusModel(entities.First());
                //}

                var entity = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                    p.CreateMachine == computerName &&
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinDate &&
                    p.CalcMode == 0 &&
                    p.Status == 0)
                    .OrderBy(p => p.CalcId)
                    .FirstOrDefault();

                if (entity != null)
                {
                    calcStatus = new CalcStatusModel(entity);
                }
            }

            if (calcStatus == null)
            {
                // 試算
                //var entities2 = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                //    p.CreateMachine.ToUpper() == computerName &&
                //    p.SeikyuUp == 2 &&
                //    p.Status == 0)
                //    .OrderBy(p => p.CalcId)
                //    .ToList();
                //var entity2 = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                //    p.CreateMachine.ToUpper() == computerName &&
                //    p.SeikyuUp == 2 &&
                //    p.Status == 0)
                //    .OrderBy(p => p.CalcId)
                //    .FirstOrDefault();

                ////if (entities2 != null && entities2.Any())
                //if(entity2 != null)
                //{
                //    //calcStatus = new CalcStatusModel(entities2.OrderByDescending(p => p.CalcId).First());
                //    //calcStatus = new CalcStatusModel(entities2.First());
                //    calcStatus = new CalcStatusModel(entity2);
                //}
                //else
                //{
                    // その他
                    //var entities3 = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                    //p.CreateMachine.ToUpper() == computerName &&
                    //p.SeikyuUp == 1 &&
                    //p.Status == 0)
                    //.OrderBy(p => p.CalcId)
                    //.ToList();
                    var entity3 = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                    p.HpId == hospitalId &&
                    p.CreateMachine == computerName &&
                    //p.SeikyuUp == 1 &&
                    p.Status == 0)
                    .OrderBy(p => p.CalcMode)
                    .ThenByDescending(p=>p.SeikyuUp)
                    .ThenBy(p => p.CalcId)
                    .FirstOrDefault();
                    //if (entities3 != null && entities3.Any())
                    if(entity3 != null)
                    {
                        //calcStatus = new CalcStatusModel(entities3.OrderByDescending(p => p.CalcId).First());
                        //calcStatus = new CalcStatusModel(entities3.First());
                        calcStatus = new CalcStatusModel(entity3);
                    }
                    //else
                    //{
                    //    //var entities4 = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                    //    //p.CreateMachine.ToUpper() == computerName &&
                    //    //p.Status == 0)
                    //    //.OrderBy(p => p.CalcId)
                    //    //.ToList();
                    //    var entity4 = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                    //    p.CreateMachine == computerName &&
                    //    p.Status == 0)
                    //    .OrderBy(p => p.CalcId)
                    //    .FirstOrDefault();
                    //    //if (entities4 != null && entities4.Any())
                    //    if (entity4 != null)
                    //    {
                    //        //calcStatus = new CalcStatusModel(entities4.OrderByDescending(p => p.CalcId).First());
                    //        calcStatus = new CalcStatusModel(entity4);
                    //    }
                    //}
                //}
            }

            return calcStatus;
        }
        public List<CalcStatusModel> GetCalcStatusies(List<long> calcIds, string preFix)
        {
            List<CalcStatusModel> calcStatusies = new List<CalcStatusModel>();
            string computerName = (preFix + Hardcode.ComputerName).ToUpper();

            if (calcIds != null && calcIds.Any())
            {
                var entities = _tenantDataContext.CalcStatus.FindListQueryable(p =>
                    p.CreateMachine == computerName &&
                    calcIds.Contains(p.CalcId) &&
                    !(new int[] { 8, 9 }.Contains(p.Status)))
                    .OrderBy(p => p.CalcId)
                    .ToList();

                entities?.ForEach(entity =>
                    calcStatusies.Add(new CalcStatusModel(entity))
                    );
            }

            return calcStatusies;
        }
        /// <summary>
        /// 同一患者番号、診療日の要求を取得する
        /// 同一条件であれば、１回計算すればいいだけなので。
        /// </summary>
        /// <param name="calcStatus"></param>
        /// <returns></returns>
        public List<CalcStatusModel> GetSameCalcStatus(CalcStatusModel calcStatus, string preFix)
        {            
            string computerName = (preFix + Hardcode.ComputerName).ToUpper();

            var entities = _tenantDataContext.CalcStatus.FindListQueryable(p =>
                p.CreateMachine == computerName &&
                p.HpId == calcStatus.HpId &&
                p.PtId == calcStatus.PtId &&
                p.SinDate == calcStatus.SinDate &&
                p.Status == 0).ToList();
            //var entities = _tenantDataContext.CalcStatus.FindListQueryable(p =>
            //    p.CreateMachine == computerName &&
            //    p.HpId == calcStatus.HpId &&
            //    p.PtId == calcStatus.PtId &&
            //    p.SinDate == calcStatus.SinDate &&
            //    p.Status == 0 &&
            //    p.CalcId == calcStatus.CalcId).ToList();
            List<CalcStatusModel> results = new List<CalcStatusModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CalcStatusModel(entity));
            });

            return results;
        }

        public List<CalcStatusModel> GetSameCalcStatus(long CalcId, string preFix)
        {
            string computerName = (preFix + Hardcode.ComputerName).ToUpper();

            var entities = _tenantDataContext.CalcStatus.FindListQueryable(p =>
                p.CreateMachine == computerName &&
                p.CalcId == CalcId).ToList();
            //var entities = _tenantDataContext.CalcStatus.FindListQueryable(p =>
            //    p.CreateMachine == computerName &&
            //    p.HpId == calcStatus.HpId &&
            //    p.PtId == calcStatus.PtId &&
            //    p.SinDate == calcStatus.SinDate &&
            //    p.Status == 0 &&
            //    p.CalcId == calcStatus.CalcId).ToList();
            List<CalcStatusModel> results = new List<CalcStatusModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CalcStatusModel(entity));
            });

            return results;
        }

        /// <summary>
        /// 指定の計算要求が処理中ではないかチェック
        /// </summary>
        /// <param name="calcStatus"></param>
        /// <returns>true: 処理中</returns>
        public bool CheckCalcStatus(CalcStatusModel calcStatus)
        {
            string computerName = Hardcode.ComputerName.ToUpper();
            DateTime dtCheck = DateTime.UtcNow.AddMinutes(-5);

            var entities = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                p.HpId == calcStatus.HpId &&
                p.PtId == calcStatus.PtId &&
                p.SinDate >= calcStatus.SinDate / 100 * 100 + 1 &&
                p.SinDate <= calcStatus.SinDate / 100 * 100 + 31 &&
                //(p.Status == 0 || p.Status == 1) &&
                (p.Status == 1) &&
                p.CalcId != calcStatus.CalcId &&
                p.UpdateDate >= dtCheck);

            return (entities != null && entities.Any());
        }
        /// <summary>
        /// 指定の計算要求が他端末で処理中ではないかチェック
        /// </summary>
        /// <param name="calcStatus"></param>
        /// <returns></returns>
        public bool CheckCalcStatusOther(CalcStatusModel calcStatus)
        {
            string computerName = Hardcode.ComputerName.ToUpper();
            DateTime dtCheck = DateTime.UtcNow.AddMinutes(-5);

            var entities = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                p.HpId == calcStatus.HpId &&
                p.PtId == calcStatus.PtId &&
                p.SinDate >= calcStatus.SinDate / 100 * 100 + 1 &&
                p.SinDate <= calcStatus.SinDate / 100 * 100 + 31 &&
                //(p.Status == 0 || p.Status == 1) &&
                (p.Status == 1) &&
                p.CalcId != calcStatus.CalcId &&
                p.CreateMachine.ToUpper() != computerName &&
                p.UpdateDate >= dtCheck);

            return (entities != null && entities.Any());
        }
        /// <summary>
        /// 指定の計算要求が自端末で処理中ではないかチェック
        /// </summary>
        /// <param name="calcStatus"></param>
        /// <returns>true: 処理中</returns>
        public bool CheckCalcStatusSelf(CalcStatusModel calcStatus)
        {
            string computerName = Hardcode.ComputerName.ToUpper();
            DateTime dtCheck = DateTime.UtcNow.AddMinutes(-5);

            var entities = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                p.HpId == calcStatus.HpId &&
                p.PtId == calcStatus.PtId &&
                p.SinDate >= calcStatus.SinDate / 100 * 100 + 1 &&
                p.SinDate <= calcStatus.SinDate / 100 * 100 + 31 &&
                //(p.Status == 0 || p.Status == 1) &&
                (p.Status == 1) &&
                p.CalcId != calcStatus.CalcId &&
                p.CreateMachine.ToUpper() == computerName &&
                p.UpdateDate >= dtCheck);

            return (entities != null && entities.Any());
        }
        /// <summary>
        /// 指定のCALC_IDの現在のSTATUSを返す
        /// </summary>
        /// <param name="calcId"></param>
        /// <returns></returns>
        public int GetCalcStatus(long calcId)
        {
            int ret = -1;

            var entities = _tenantDataContext.CalcStatus.FindListQueryableNoTrack(p =>
                p.CalcId == calcId);

            if(entities != null && entities.Any())
            {
                ret = entities.First().Status;
            }

            return ret;
        }
        /// <summary>
        /// レセプト状態情報を取得する（STATUS_KBN = 8のもの）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="hokenIds">保険ID</param>
        /// <returns>レセプト状態情報（STATUS_KBN=8のもの）</returns>
        public List<ReceStatusModel> FindReceStatus(int hpId, long ptId, int sinYm, List<int> hokenIds)
        {
            var entities = _tenantDataContext.ReceStatuses.FindList(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm == sinYm &&
                hokenIds.Contains(p.HokenId) &&
                p.StatusKbn == 8 &&
                p.IsDeleted == DeleteStatus.None
            );

            List<ReceStatusModel> results = new List<ReceStatusModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new ReceStatusModel(entity));
            });

            return results;
        }

        public int GetCountCalcInMonth(string prefix)
        {
            string computerName = Hardcode.ComputerName;
            return _tenantDataContext.CalcStatus
                            .FindListQueryableNoTrack(p => p.CreateMachine == prefix + computerName &&
                                                           p.Status == 0).Count();
        }
    }
}
