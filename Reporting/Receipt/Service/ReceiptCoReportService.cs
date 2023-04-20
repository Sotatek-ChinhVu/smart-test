using Domain.Models.SystemConf;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.Models;
using EmrCalculateApi.Receipt.ViewModels;
using Helper.Common;
using Helper.Enum;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Receipt.Constants;
using Reporting.Receipt.DB;
using Reporting.Receipt.Models;
using Reporting.Structs;

namespace Reporting.Receipt.Service
{
    public class ReceiptCoReportService : RepositoryBase, IReceiptCoReportService
    {
        private readonly ICoReceiptFinder _coReceiptFinder;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        private readonly ITenantProvider _tenantProvider;

        public ReceiptCoReportService(ITenantProvider tenantProvider, ICoReceiptFinder coReceiptFinder, ISystemConfRepository systemConfRepository, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger) : base(tenantProvider)
        {
            _coReceiptFinder = coReceiptFinder;
            _systemConfRepository = systemConfRepository;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
            _tenantProvider = tenantProvider;
        }

        private List<EmrCalculateApi.ReceFutan.Models.ReceFutanKbnModel> ReceFutanKbnModels;
        private EmrCalculateApi.ReceFutan.Models.ReceInfModel ReceInf;

        private int HpId;
        private int SeikyuYm;
        private List<long> PtId;
        private int SinYm;
        private int HokenId;
        private int KaId;
        private int GrpId;
        private int TantoId;
        private int Target;
        private string ReceSbt;
        private int PrintNoFrom;
        private int PrintNoTo;

        private int PaperKbn;
        private bool IncludeTester;
        private bool IncludeOutDrug;
        private bool IsPtTest;
        private int Sort;

        SeikyuType SeikyuType;

        private List<EmrCalculateApi.ReceFutan.Models.ReceFutanKbnModel> ReceFutanKbns { get; set; } = new();

        public List<CoReceiptModel> GetReceiptData(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId, ReceiptPreviewModeEnum mode, bool isNoCreatingReceData = false)
        {
            var receInf = _coReceiptFinder.GetReceInf(hpId, ptId, seikyuYm, sinYm, hokenId);

            // TODO message or somthing process here
            if (receInf == null) return new();
            int target = -1;
            switch (receInf.HokenKbn)
            {
                case 0:
                    target = TargetConst.Jihi;
                    break;
                case 1:
                    target = TargetConst.Syaho;
                    break;
                case 2:
                    target = TargetConst.Kokuho;
                    break;
                case 11:
                    target = TargetConst.RousaiTanki;
                    break;
                case 12:
                    target = TargetConst.RousaiNenkin;
                    break;
                case 13:
                    target = TargetConst.RousaiAfter;
                    break;
                case 14:
                    target = TargetConst.Jibai;
                    break;
                default:
                    return new();
            }

            switch (mode)
            {

                case ReceiptPreviewModeEnum.Accounting:
                case ReceiptPreviewModeEnum.ReceiptCheckMedicalDetailIn:
                case ReceiptPreviewModeEnum.ReceiptCheckMedicalDetailOut:
                case ReceiptPreviewModeEnum.ReceiptList:
                    {
                        SeikyuType seikyuType = new SeikyuType(true, true, true, true, true);

                        if (isNoCreatingReceData)
                        {
                            InitParam(hpId, ReceInf, ReceFutanKbnModels, IncludeOutDrug);
                            return GetData();
                        }
                        else
                        {
                            InitParam(hpId, receInf.SeikyuYm
                                        , receInf.PtId
                                        , receInf.SinYm
                                        , receInf.HokenId
                                        , 0
                                        , 0
                                        , target
                                        , ""
                                        , 0
                                        , 999999999
                                        , seikyuType
                                        , IsPtTest
                                        , IncludeOutDrug
                                        , sort: 0);
                            return GetData();
                        }
                    }
                case ReceiptPreviewModeEnum.ReceiptCheckInputSyoujoSyouki:
                    {
                        InitParam(hpId, receInf.PtId, receInf.SeikyuYm, receInf.SinYm, receInf.HokenId);
                        return GetData();
                    }
            }

            return new();
        }

        public void InitParam(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
        {
            HpId = hpId;
            PtId.Add(ptId);
            SeikyuYm = seikyuYm;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public void InitParam(int hpId,
            EmrCalculateApi.ReceFutan.Models.ReceInfModel receInf, List<EmrCalculateApi.ReceFutan.Models.ReceFutanKbnModel> receFutanKbnModels, bool includeOutDrug)
        {
            HpId = hpId;
            SeikyuYm = receInf.SeikyuYm;
            PtId = new List<long>();
            PtId.Add(receInf.PtId);
            SinYm = receInf.SinYm;
            HokenId = receInf.HokenId;
            KaId = 0;
            TantoId = 0;

            switch (receInf.HokenKbn)
            {
                case 0:
                    Target = 99;
                    break;
                case 1:
                    Target = 1;
                    break;
                case 2:
                    Target = 2;
                    break;
                case 11:
                    Target = 10;
                    break;
                case 12:
                    Target = 11;
                    break;
                case 13:
                    Target = 13;
                    break;
                case 14:
                    Target = 20;
                    break;
            }
            ReceSbt = "";
            PrintNoFrom = 0;
            PrintNoTo = 999999999;
            PaperKbn = 0;
            IncludeTester = true;
            IncludeOutDrug = includeOutDrug;
            Sort = 0;

            GrpId = 0;

            ReceInf = receInf;
            ReceFutanKbns = receFutanKbnModels;
        }

        public void InitParam(int hpId,
            int seikyuYm, long ptId, int sinYm, int hokenId, int kaId, int tantoId,
            int target, string receSbt, int printNoFrom, int printNoTo,
            SeikyuType seikyuType, bool includeTester, bool includeOutDrug,
            int sort
        )
        {
            HpId = hpId;
            PtId = new List<long>();
            if (ptId > 0)
            {
                PtId.Add(ptId);
            }

            InitParam(hpId: hpId,
                seikyuYm: seikyuYm, ptId: PtId, sinYm: sinYm, hokenId: hokenId,
                kaId: kaId, tantoId: tantoId, target: target, receSbt: receSbt,
                printNoFrom: printNoFrom, printNoTo: printNoTo,
                seikyuType: seikyuType,
                includeTester: includeTester, includeOutDrug: includeOutDrug,
                sort: sort);
        }

        public void InitParam(int hpId,
            int seikyuYm, List<long> ptId, int sinYm, int hokenId, int kaId, int tantoId,
            int target, string receSbt, int printNoFrom, int printNoTo,
            SeikyuType seikyuType, bool includeTester, bool includeOutDrug,
            int sort
        )
        {
            HpId = hpId;
            SeikyuType = seikyuType;

            SeikyuYm = seikyuYm;
            PtId = new List<long>();
            if (ptId != null)
            {
                PtId.AddRange(ptId.GroupBy(p => p).Select(p => p.Key).ToList());
            }

            SinYm = sinYm;
            HokenId = hokenId;
            KaId = kaId;
            TantoId = tantoId;
            Target = target;
            ReceSbt = receSbt;
            PrintNoFrom = 0;
            PrintNoTo = 999999999;
            IncludeTester = includeTester;
            IncludeOutDrug = includeOutDrug;
            Sort = sort;

            GrpId = 0;
            if (Sort > 100)
            {
                GrpId = Sort % 100;
            }

            if (printNoFrom > 0 && printNoTo > 0 && printNoFrom <= printNoTo)
            {
                PrintNoFrom = printNoFrom;
                PrintNoTo = printNoTo;
            }
        }

        public List<CoReceiptModel> GetData()
        {
            #region mode変換
            int mode = -1;
            switch (Target)
            {
                case TargetConst.Syaho:
                    mode = 0;
                    break;
                case TargetConst.Kokuho:
                    mode = 1;
                    break;
                case TargetConst.Kenpo:
                    mode = 7;
                    break;
                case TargetConst.RousaiTanki:
                    mode = 2;
                    break;
                case TargetConst.RousaiNenkin:
                    mode = 3;
                    break;
                case TargetConst.RousaiAfter:
                    mode = 4;
                    break;
                case TargetConst.Jibai:
                    mode = 5;
                    break;
                case TargetConst.Jihi:
                    mode = 6;
                    break;
            }
            #endregion

            // 対象患者取得
            List<int> seikyuKbns = new List<int>();

            if (SeikyuType.IsNormal)
            {
                seikyuKbns.Add(SeikyuKbnConst.Normal);
            }

            if (SeikyuType.IsDelay)
            {
                seikyuKbns.Add(SeikyuKbnConst.Tukiokure);
            }

            if (SeikyuType.IsHenrei)
            {
                seikyuKbns.Add(SeikyuKbnConst.Henrei);
            }

            if (SeikyuType.IsOnline)
            {
                seikyuKbns.Add(SeikyuKbnConst.OnlineHenrei);
            }

            var receInfModels = new List<ReceInfModel>();
            var sinRpInfModels = new List<SinRpInfModel>();
            var sinKouiModels = new List<SinKouiModel>();
            var sinKouiDetailModels = new List<SinKouiDetailModel>();
            var sinKouiCountModels = new List<SinKouiCountModel>();
            var gettedPtId = new List<long>();


            void _getReceData(List<long> AptId)
            {
                if (ReceInf == null)
                {
                    List<long> targetPtIds = new List<long>();

                    foreach (long addPtId in AptId)
                    {
                        if (gettedPtId.Any(p => p == addPtId) == false)
                        {
                            gettedPtId.Add(addPtId);
                            targetPtIds.Add(addPtId);
                        }
                    }

                    if (Target == TargetConst.FukuokaRece2)
                    {
                        receInfModels.AddRange(
                            _coReceiptFinder.FindReceInfFukuoka(HpId, mode, Target, SeikyuYm, targetPtIds, SinYm, HokenId, ReceSbt, IncludeTester, SeikyuType.IsPaper, seikyuKbns, TantoId, KaId, GrpId)
                            .ToList());
                    }
                    else
                    {
                        receInfModels.AddRange(
                            _coReceiptFinder.FindReceInf(HpId, mode, Target, SeikyuYm, targetPtIds, SinYm, HokenId, ReceSbt, IncludeTester, SeikyuType.IsPaper, seikyuKbns, TantoId, KaId, GrpId)
                            .ToList());
                    }

                    // 算定情報取得

                    sinRpInfModels.AddRange(_coReceiptFinder.FindSinRpInfDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                    sinKouiModels.AddRange(_coReceiptFinder.FindSinKouiDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                    sinKouiDetailModels.AddRange(_coReceiptFinder.FindSinKouiDetailDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                    sinKouiCountModels.AddRange(_coReceiptFinder.FindSinKouiCountDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                }
                else
                {
                    receInfModels.AddRange(_coReceiptFinder.FindReceInf(HpId, ReceInf));

                    int hokenId2 = 0;
                    if (receInfModels != null && receInfModels.Any())
                    {
                        hokenId2 = receInfModels.First().HokenId2;
                    }
                    // 算定情報取得
                    sinRpInfModels.AddRange(_coReceiptFinder.FindSinRpInfDataForPreview(HpId, AptId, SinYm));
                    sinKouiModels.AddRange(_coReceiptFinder.FindSinKouiDataForPreview(HpId, AptId, SinYm, HokenId, hokenId2));
                    sinKouiDetailModels.AddRange(_coReceiptFinder.FindSinKouiDetailDataForPreview(HpId, AptId, SinYm));
                    sinKouiCountModels.AddRange(_coReceiptFinder.FindSinKouiCountDataForPreview(HpId, AptId, SinYm));
                }
            }


            if (PtId == null || PtId.Any() == false)
            {
                // 患者番号の指定がない場合
                _getReceData(PtId);
            }
            else
            {
                // PtIdが多すぎると、stack overflowになるので、100個ずつに分割して取得する
                List<List<long>> tmpPtIdList = new List<List<long>>();
                List<long> tmpPtIds = new List<long>();

                int idx = 0;

                foreach (long tmpPtId in PtId)
                {
                    tmpPtIds.Add(tmpPtId);
                    idx++;

                    if (idx >= 100)
                    {
                        tmpPtIdList.Add(tmpPtIds);
                        tmpPtIds = new List<long>();
                        idx = 0;
                    }
                }

                if (tmpPtIds.Any())
                {
                    tmpPtIdList.Add(tmpPtIds);
                }

                foreach (List<long> ptIds in tmpPtIdList)
                {
                    _getReceData(ptIds);
                }
            }

            #region ソート

            if (PtId != null && PtId.Any())
            {
                List<ReceInfModel> tmp = new List<ReceInfModel>();

                foreach (long ptid in PtId)
                {
                    tmp.AddRange(receInfModels.FindAll(p => p.PtId == ptid));
                }

                receInfModels.Clear();
                receInfModels.AddRange(tmp);
            }
            else if (Sort == 0)
            {
                // 患者番号順
                receInfModels =
                    receInfModels
                    .OrderBy(p => p.SinYm)
                    .ThenBy(p => p.PtNum)
                    .ThenBy(p => p.PtId)
                    .ToList();
            }
            else if (Sort == 1)
            {
                // カナ氏名順
                receInfModels =
                    receInfModels
                    .OrderBy(p => p.SinYm)
                    .ThenBy(p => p.PtInf.KanaName)
                    .ThenBy(p => p.PtNum)
                    .ThenBy(p => p.PtId)
                    .ToList();
            }
            else if (Sort == 2)
            {
                // 保険者番号順
                receInfModels =
                    receInfModels
                    .OrderBy(p => p.SinYm)
                    .ThenBy(p => p.HokensyaNo)
                    .ThenBy(p => p.PtNum)
                    .ThenBy(p => p.PtId)
                    .ToList();
            }
            else if (Sort == 3)
            {
                // レセ種別順
                receInfModels =
                    receInfModels
                    .OrderBy(p => p.SinYm)
                    .ThenBy(p => p.HokenKbn)
                    .ThenBy(p => p.ReceSbt)
                    .ThenBy(p => p.PtInf.KanaName)
                    .ThenBy(p => p.PtNum)
                    .ThenBy(p => p.PtId)
                    .ToList();
            }
            else if (Sort > 100)
            {
                // 分類順
                receInfModels =
                    receInfModels
                    .OrderBy(p => p.SinYm)
                    .ThenByDescending(p => !string.IsNullOrEmpty(p.GrpCd))
                    .ThenBy(p => p.GrpCd)
                    .ThenBy(p => p.PtNum)
                    .ThenBy(p => p.PtId)
                    .ToList();
            }
            #endregion

            // ループ
            List<CoReceiptModel> retCoReceiptModels = new List<CoReceiptModel>();

            int receiptNo = 1;

            foreach (ReceInfModel receInfModel in receInfModels)
            {
                // レセプトデータを取得する

                if (Target == TargetConst.KanagawaRece2)
                {
                    // 神奈川レセプト２枚目
                    for (int i = 1; i <= 4; i++)
                    {
                        if (new List<string> { "80", "81", "85", "88", "89" }.Contains(receInfModel.KohiHoubetu(i)))
                        {
                            if (receInfModel.KohiReceKisai(i) == 0)
                            {
                                receInfModel.KohiReceKisai(i, 1);
                                receInfModel.ReceSbt =
                                    CIUtil.Copy(receInfModel.ReceSbt, 1, 2) +
                                    (CIUtil.StrToIntDef(CIUtil.Copy(receInfModel.ReceSbt, 3, 1), 0) + 1).ToString() +
                                    CIUtil.Copy(receInfModel.ReceSbt, 4, 1);
                            }
                            break;
                        }
                    }
                }

                List<SinRpInfModel> filteredSinRpInfs = sinRpInfModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<SinKouiModel> filteredSinKouis = sinKouiModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm && (p.HokenId == receInfModel.HokenId || p.HokenId == receInfModel.HokenId2));
                List<SinKouiDetailModel> filteredSinKouiDetails = sinKouiDetailModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);
                List<SinKouiCountModel> filteredSinKouiCounts = sinKouiCountModels.FindAll(p => p.PtId == receInfModel.PtId && p.SinYm == receInfModel.SinYm);

                List<CoReceiptModel> coReceiptModels = new List<CoReceiptModel>();

                // 病院情報取得
                HpInfModel hpInfModel = _coReceiptFinder.FindHpInf(HpId, receInfModel.SinYm * 100 + 1);

                if (Target != TargetConst.RousaiAfter)
                {
                    coReceiptModels.Add(GetReceiptData(
                        Target, IncludeOutDrug, SeikyuYm,
                        hpInfModel, receInfModel,
                        filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts));
                }
                else
                {
                    // アフターケア
                    coReceiptModels = GetReceiptDataForAfterCare(
                        IncludeOutDrug, SeikyuYm,
                        hpInfModel, receInfModel, filteredSinRpInfs, filteredSinKouis, filteredSinKouiDetails, filteredSinKouiCounts);
                }

                if (filteredSinRpInfs.Count() <= 0)
                {
                    Console.WriteLine(string.Format("明細なし　ptId:{0} sinYM{1}", receInfModel.PtId, receInfModel.SinYm));
                }
                else
                {
                    for (int i = 0; i < coReceiptModels.Count; i++)
                    {
                        coReceiptModels[i].ReceiptNo = receiptNo;

                        if (receiptNo >= PrintNoFrom && receiptNo <= PrintNoTo)
                        {
                            retCoReceiptModels.Add(coReceiptModels[i]);
                        }

                        receiptNo++;
                    }
                }
            }

            return retCoReceiptModels;
        }

        public CoReceiptModel GetReceiptData(
            int target, bool includeOutDrug, int seikyuYm,
            HpInfModel hpInfModel, ReceInfModel receInf,
            List<SinRpInfModel> sinRpInfs, List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinDtls, List<SinKouiCountModel> sinKouiCounts)
        {
            int hpId = receInf.HpId;
            long ptId = receInf.PtId;
            int sinYm = receInf.SinYm;

            // 基本情報
            PtInfModel ptInfModel = GetPtInfModel(ptId, sinYm, receInf);

            // 保険情報
            HokenDataModel hokenDataModel = GetHokenDataModel(ptId, receInf);

            // 公費情報
            List<KohiDataModel> kohiDataModels;
            List<KohiDataModel> kohiDataModelsAll;
            (kohiDataModels, kohiDataModelsAll) = GetKohiDataModels(ptId, receInf);

            // 傷病名情報            
            List<SyobyoDataModel> syobyoDataModels = GetSyobyoDataModels(ptId, sinYm, receInf, seikyuYm);

            // 診療明細情報
            SinMeiViewModel sinMeiViewModels = null;
            SinMeiViewModel sinMeiViewModelsForTen = null;

            List<ReceFutanKbnModel> receFutanKbnModels = null;
            if (ReceFutanKbns != null)
            {
                receFutanKbnModels = new List<ReceFutanKbnModel>();
                foreach (EmrCalculateApi.ReceFutan.Models.ReceFutanKbnModel receFutanKbnModel in ReceFutanKbns)
                {
                    receFutanKbnModels.Add(new ReceFutanKbnModel(receFutanKbnModel.ReceFutanKbn));
                }
            }

            if (sinRpInfs != null)
            {
                sinMeiViewModels = new SinMeiViewModel(SinMeiMode.PaperRece, includeOutDrug, hpId, ptId, sinYm, seikyuYm, receInf, receFutanKbnModels, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts,
                    _tenantProvider, _systemConfigProvider, _emrLogger);
                if (new List<int> { TargetConst.RousaiTanki, TargetConst.RousaiNenkin }.Contains(Target))
                {
                    sinMeiViewModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensuRousai, includeOutDrug, hpId, ptId, sinYm, seikyuYm, receInf, receFutanKbnModels, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts,
                        _tenantProvider, _systemConfigProvider, _emrLogger);
                }
                else
                {
                    sinMeiViewModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensu, includeOutDrug, hpId, ptId, sinYm, seikyuYm, receInf, receFutanKbnModels, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts
                        , _tenantProvider, _systemConfigProvider, _emrLogger);
                }
            }
            else
            {
                sinMeiViewModels = new SinMeiViewModel(SinMeiMode.PaperRece, includeOutDrug, hpId, ptId, sinYm, receInf, ptInfModel, receInf.Tokki
                    , _tenantProvider, _systemConfigProvider, _emrLogger);
                if (new List<int> { TargetConst.RousaiTanki, TargetConst.RousaiNenkin }.Contains(Target))
                {
                    sinMeiViewModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensuRousai, includeOutDrug, hpId, ptId, sinYm, receInf, ptInfModel, receInf.Tokki
                        , _tenantProvider, _systemConfigProvider, _emrLogger);
                }
                else
                {
                    sinMeiViewModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensu, includeOutDrug, hpId, ptId, sinYm, receInf, ptInfModel, receInf.Tokki
                        , _tenantProvider, _systemConfigProvider, _emrLogger);
                }
            }




            // 点数欄情報
            CoReceiptTensuModel coReceiptTensuModel = new CoReceiptTensuModel(sinMeiViewModelsForTen);

            // 旧姓
            PtKyuseiModel ptKyuseiModel = _coReceiptFinder.FindPtKyusei(HpId, ptId, sinMeiViewModels.LastSinDate);

            // 労災レセプト情報取得
            RousaiReceiptModel rousaiReceiptModel = null;
            SyobyoKeikaModel syobyoKeikaModel = null;
            List<int> tuuinDays = new List<int>();

            if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter }.Contains(target))
            {
                // 労災レセプト情報
                rousaiReceiptModel = GetRousaiReceiptData(ptId, sinYm, receInf, ptInfModel, ptKyuseiModel, hokenDataModel, sinMeiViewModels);

                // 傷病の経過
                syobyoKeikaModel = _coReceiptFinder.FindSyobyoKeika(HpId, ptId, sinYm, receInf.HokenId);

            }
            else if (new int[] { TargetConst.Jibai }.Contains(target))
            {
                // 自賠責
                rousaiReceiptModel =
                    GetRousaiReceiptDataForJibai(ptId, sinYm, receInf, ptInfModel, ptKyuseiModel, hokenDataModel, sinMeiViewModels);

                // 通院日情報取得
                tuuinDays = _coReceiptFinder.FindTuuinDays(HpId, ptId, receInf.SinYm, receInf.HokenId);
            }

            CoReceiptModel coReceiptModel =
                new CoReceiptModel(
                    receInf, ptInfModel, hokenDataModel, kohiDataModels, kohiDataModelsAll, syobyoDataModels, sinMeiViewModels.SinMei,
                     ptKyuseiModel, rousaiReceiptModel ?? new(new(), new(), new(), new(), new(), 0), syobyoKeikaModel ?? new(new()), hpInfModel, coReceiptTensuModel);

            if (new int[] { TargetConst.Jibai }.Contains(target))
            {
                // 通院日
                coReceiptModel.TuuinDays = tuuinDays;
            }

            return coReceiptModel;
        }

        private PtInfModel GetPtInfModel(long ptId, int sinYm, ReceInfModel receInf)
        {
            PtInfModel ptInfModel = null;

            if (receInf.PtInf != null)
            {
                ptInfModel = new PtInfModel(receInf.PtInf, sinYm * 100 + 1);
            }
            else
            {
                ptInfModel = _coReceiptFinder.FindPtInf(HpId, ptId, sinYm * 100 + 1);
            }

            return ptInfModel;
        }

        private HokenDataModel GetHokenDataModel(long ptId, ReceInfModel receInf)
        {
            HokenDataModel hokenDataModel = null;
            if (receInf.PtHokenInf != null)
            {
                hokenDataModel = new HokenDataModel(receInf.PtHokenInf);
            }
            else if (receInf.HokenId > 0)
            {
                hokenDataModel = _coReceiptFinder.FindHokenData(HpId, ptId, receInf.HokenId);
            }

            if (hokenDataModel != null)
            {
                hokenDataModel.JituNissu = receInf.HokenNissu;
                hokenDataModel.TotalTen = receInf.HokenReceTensu;
                hokenDataModel.FutanKingaku = receInf.HokenReceFutan;
            }

            return hokenDataModel;
        }

        private (List<KohiDataModel>, List<KohiDataModel>) GetKohiDataModels(long ptId, ReceInfModel receInf)
        {
            var kohiDataModels = new List<KohiDataModel>();
            var kohiDataModelsAll = new List<KohiDataModel>();

            // 公費ID
            //List<int> kohiIds = new List<int>();
            var kohiDatas = _coReceiptFinder.FindKohiData(HpId, ptId, receInf.SinYm * 100 + 1);
            if (kohiDatas.Any())
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (kohiDatas.Any(p => p.PtKohi.HokenId == receInf.KohiId(i)))
                    {
                        kohiDataModelsAll.Add(kohiDatas.Find(p => p.PtKohi.HokenId == receInf.KohiId(i)));
                        kohiDataModelsAll.Last().JituNissu = receInf.KohiNissu(i);
                        kohiDataModelsAll.Last().ReceTen = receInf.KohiReceTensu(i);
                        kohiDataModelsAll.Last().ReceFutan = receInf.KohiReceFutan(i);
                        kohiDataModelsAll.Last().ReceKyufu = receInf.KohiReceKyufu(i);
                        kohiDataModelsAll.Last().Futan = receInf.KohiFutan(i);
                        kohiDataModelsAll.Last().Futan10en = receInf.KohiFutan10en(i);

                        if (receInf.KohiReceKisai(i) == 1 && receInf.KohiId(i) > 0)
                        {
                            if (kohiDatas.Any(p => p.PtKohi.HokenId == receInf.KohiId(i)))
                            {
                                kohiDataModels.Add(kohiDatas.Find(p => p.PtKohi.HokenId == receInf.KohiId(i)));
                                kohiDataModels.Last().JituNissu = receInf.KohiNissu(i);
                                kohiDataModels.Last().ReceTen = receInf.KohiReceTensu(i);
                                kohiDataModels.Last().ReceFutan = receInf.KohiReceFutan(i);
                                kohiDataModels.Last().ReceKyufu = receInf.KohiReceKyufu(i);
                                kohiDataModels.Last().Futan = receInf.KohiFutan(i);
                                kohiDataModels.Last().Futan10en = receInf.KohiFutan10en(i);
                            }
                        }
                    }
                }
            }
            return (kohiDataModels, kohiDataModelsAll);
        }

        private List<SyobyoDataModel> GetSyobyoDataModels(long ptId, int sinYm, ReceInfModel receInf, int outputYm)
        {
            List<SyobyoDataModel> syobyoDataModels = _coReceiptFinder.FindSyobyoData(HpId, ptId, sinYm, receInf.HokenId, outputYm);

            if ((int)_systemConfRepository.GetSettingValue(94001, 0, HpId) == 1) //ReceiptByomeiWordWrap
            {
                syobyoDataModels = syobyoDataModels.OrderBy(p => p.StartDate).ThenBy(p => p.TenkiDate).ThenBy(p => p.KamiReceTenkiKbn).ThenByDescending(p => p.SyubyoKbn).ThenBy(p => p.SortNo).ToList();
            }
            else
            {
                syobyoDataModels = syobyoDataModels.OrderByDescending(p => p.SyubyoKbn).ThenBy(p => p.StartDate).ThenBy(p => p.TenkiDate).ThenBy(p => p.KamiReceTenkiKbn).ThenBy(p => p.SortNo).ToList();
            }

            return syobyoDataModels;
        }

        public List<CoReceiptModel> GetReceiptDataForAfterCare(
            bool includeOutDrug, int outputYm,
            HpInfModel hpInfModel, ReceInfModel receInf, List<SinRpInfModel> sinRpInfs, List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinDtls, List<SinKouiCountModel> sinKouiCounts)
        {
            int hpId = receInf.HpId;
            long ptId = receInf.PtId;
            int sinYm = receInf.SinYm;
            List<CoReceiptModel> coReceiptModels = new List<CoReceiptModel>();

            // 基本情報
            PtInfModel ptInfModel = GetPtInfModel(ptId, sinYm, receInf);

            // 保険情報
            HokenDataModel hokenDataModel = GetHokenDataModel(ptId, receInf);

            // 公費情報
            List<KohiDataModel> kohiDataModels;
            List<KohiDataModel> kohiDataModelsAll;
            (kohiDataModels, kohiDataModelsAll) = GetKohiDataModels(ptId, receInf);

            // 傷病名情報            
            List<SyobyoDataModel> syobyoDataModels = GetSyobyoDataModels(ptId, sinYm, receInf, outputYm);

            // 会計情報取得
            List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)> kaikeiDayInfs =
                GetKaikeiDayInfs(ptId, sinYm, receInf);

            // 会計日ごとに診療明細情報を取得する
            foreach ((int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei) kaikeiDayInf in kaikeiDayInfs)
            {
                // 診療明細情報
                SinMeiViewModel sinMeiDataModels = null;
                SinMeiViewModel sinMeiDataModelsForTen = null;

                if (sinRpInfs != null)
                {
                    sinMeiDataModels = new SinMeiViewModel(SinMeiMode.AfterCare, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts,
                        _tenantProvider, _systemConfigProvider, _emrLogger);
                    if (new List<int> { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter }.Contains(Target))
                    {
                        sinMeiDataModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensuAfter, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts,
                            _tenantProvider, _systemConfigProvider, _emrLogger);
                    }
                    else
                    {
                        sinMeiDataModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensu, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki, sinRpInfs, sinKouis, sinDtls, sinKouiCounts,
                            _tenantProvider, _systemConfigProvider, _emrLogger);
                    }
                }
                else
                {
                    sinMeiDataModels = new SinMeiViewModel(SinMeiMode.AfterCare, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki,
                        _tenantProvider, _systemConfigProvider, _emrLogger);
                    if (new List<int> { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter }.Contains(Target))
                    {
                        sinMeiDataModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensuAfter, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki,
                            _tenantProvider, _systemConfigProvider, _emrLogger);
                    }
                    else
                    {
                        sinMeiDataModelsForTen = new SinMeiViewModel(SinMeiMode.ReceTensu, includeOutDrug, hpId, ptId, kaikeiDayInf.sinDate, kaikeiDayInf.raiinNos, receInf, ptInfModel, receInf.Tokki,
                            _tenantProvider, _systemConfigProvider, _emrLogger);
                    }
                }

                // 点数欄情報
                CoReceiptTensuModel coReceiptTensuModel = new CoReceiptTensuModel(sinMeiDataModelsForTen);

                // 旧姓
                PtKyuseiModel ptKyuseiModel = _coReceiptFinder.FindPtKyusei(HpId, ptId, sinMeiDataModels.LastSinDate);

                // 労災レセプト情報
                RousaiReceiptModel rousaiReceiptModel = null;
                SyobyoKeikaModel syobyoKeikaModel = null;

                // 労災レセプト情報
                rousaiReceiptModel = GetRousaiReceiptData(ptId, sinYm, receInf, ptInfModel, ptKyuseiModel, hokenDataModel, sinMeiDataModels);
                syobyoKeikaModel = _coReceiptFinder.FindSyobyoKeikaForAfter(HpId, ptId, kaikeiDayInf.sinDate, receInf.HokenId);

                //レセプト電算モデルを作成する
                CoReceiptModel coReceiptModel =
                    new CoReceiptModel(
                        receInf, ptInfModel, hokenDataModel, kohiDataModels, kohiDataModelsAll, syobyoDataModels, sinMeiDataModels.SinMei,
                         ptKyuseiModel, rousaiReceiptModel, syobyoKeikaModel, hpInfModel, coReceiptTensuModel);

                // 診療日（アフターケア用）
                coReceiptModel.SinDate = kaikeiDayInf.sinDate;
                // 小計（アフターケア用）
                coReceiptModel.AfterSyokei = kaikeiDayInf.Syokei;
                // 小計イ（アフターケア用）
                coReceiptModel.AfterSyokeiGaku_I = kaikeiDayInf.SyokeiGaku_I;
                // 小計ロ（アフターケア用）
                coReceiptModel.AfterSyokeiGaku_RO = kaikeiDayInf.SyokeiGaku_RO;
                // 検査日（アフターケア用）
                coReceiptModel.KensaDate = 0;
                if (_coReceiptFinder.ZenkaiKensaDate(HpId, ptId, kaikeiDayInf.sinDate, receInf.HokenId) == kaikeiDayInf.sinDate)
                {
                    coReceiptModel.KensaDate = kaikeiDayInf.sinDate;
                }
                // 前回検査日（アフターケア用）
                coReceiptModel.ZenkaiKensaDate = _coReceiptFinder.ZenkaiKensaDate(HpId, ptId, kaikeiDayInf.sinDate - 1, receInf.HokenId);

                coReceiptModels.Add(coReceiptModel);
            }

            return coReceiptModels;
        }

        private List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)>
            GetKaikeiDayInfs(long ptId, int sinYm, ReceInfModel receInf)
        {
            List<CoKaikeiDetailModel> kaikeiDtls = _coReceiptFinder.FindKaikeiDetail(HpId, ptId, sinYm, receInf.HokenId);

            List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)> kaikeiDayInfs =
                new List<(int sinDate, List<long> raiinNos, int TotalI, int TotalRo, int Syokei)>();

            int tmpSinDate = 0;
            List<long> tmpRaiinNos = new List<long>();
            int tmpSyokeiGaku_I = 0;
            int tmpSyokeiGaku_RO = 0;
            int tmpSyokei = 0;

            foreach (CoKaikeiDetailModel kaikeiDtl in kaikeiDtls)
            {
                if (tmpSinDate != kaikeiDtl.SinDate)
                {
                    if (tmpSinDate > 0)
                    {
                        kaikeiDayInfs.Add((tmpSinDate, new List<long>(tmpRaiinNos), tmpSyokeiGaku_I, tmpSyokeiGaku_RO, tmpSyokei));
                    }

                    tmpSinDate = kaikeiDtl.SinDate;
                    tmpRaiinNos.Clear();
                    tmpRaiinNos.Add(kaikeiDtl.RaiinNo);
                    tmpSyokeiGaku_I = kaikeiDtl.RousaiIFutan;
                    tmpSyokeiGaku_RO = kaikeiDtl.RousaiRoFutan;
                    tmpSyokei = kaikeiDtl.RousaiIFutan / receInf.HokenMst.EnTen;
                }
                else
                {
                    tmpRaiinNos.Add(kaikeiDtl.RaiinNo);
                    tmpSyokeiGaku_I += kaikeiDtl.RousaiIFutan;
                    tmpSyokeiGaku_RO += kaikeiDtl.RousaiRoFutan;
                    tmpSyokei += kaikeiDtl.RousaiIFutan / receInf.HokenMst.EnTen;
                }
            }

            if (tmpSinDate > 0)
            {
                kaikeiDayInfs.Add((tmpSinDate, new List<long>(tmpRaiinNos), tmpSyokeiGaku_I, tmpSyokeiGaku_RO, tmpSyokei));
            }

            return kaikeiDayInfs;
        }

        private RousaiReceiptModel GetRousaiReceiptData(long ptId, int sinYm, ReceInfModel receInf, PtInfModel ptInfModel, PtKyuseiModel ptKyuseiModel, HokenDataModel hokenDataModel, SinMeiViewModel sinMeiViewModel)
        {
            RousaiReceiptModel rousaiReceiptModel = null;

            rousaiReceiptModel =
                new RousaiReceiptModel(hokenDataModel.PtHokenInf, _coReceiptFinder.FindPtRousaiTenki(HpId, ptId, sinYm, receInf.HokenId), ptInfModel.PtInf, (ptKyuseiModel != null ? ptKyuseiModel.PtKyusei : null), receInf.RousaiCount, SeikyuYm);

            if (rousaiReceiptModel != null)
            {
                rousaiReceiptModel.JituNissu = receInf.HokenNissu;
                rousaiReceiptModel.Syokei = receInf.RousaiIFutan / receInf.HokenMst.EnTen;
                rousaiReceiptModel.SyokeiGaku_I = receInf.RousaiIFutan;
                rousaiReceiptModel.SyokeiGaku_RO = receInf.RousaiRoFutan;

                //療養開始日
                rousaiReceiptModel.RyoyoStartDate =
                    GetRyoyoStartDate(sinYm, sinMeiViewModel.FirstSyosinDate, hokenDataModel.PtHokenInf.RyoyoStartDate);

                //療養終了日
                rousaiReceiptModel.RyoyoEndDate = GetRyoyoEndDate(sinYm, sinMeiViewModel.LastSinDate, hokenDataModel.PtHokenInf.RyoyoEndDate, rousaiReceiptModel.Tenki);

            }

            return rousaiReceiptModel;
        }

        private int GetRyoyoStartDate(int sinYm, int firstSyosinDate, int ryoyoStartDate)
        {
            int ret = ryoyoStartDate;
            if (ret > 0)
            {
                if (ret / 100 < sinYm)
                {
                    // 療養開始日が診療月以前の場合は、診療月1日
                    ret = sinYm * 100 + 1;
                }
                else if (ret / 100 > sinYm)
                {
                    ret = firstSyosinDate;
                }
            }
            else
            {
                // 未設定の場合、初診算定日
                ret = firstSyosinDate;
            }

            if (ret == 0)
            {
                // 上の処理で取得できないときは診療月1日
                ret = sinYm * 100 + 1;
            }
            return ret;
        }

        private int GetRyoyoEndDate(int sinYm, int lastSinDate, int ryoyoEndDate, int tenki)
        {
            int ret = ryoyoEndDate;

            if (tenki == 3)
            {
                // 転帰が継続の場合、診療月末日を設定
                ret = CIUtil.GetLastDateOfMonth(sinYm * 100 + 1);
            }
            else
            {
                //// 転帰が継続以外の場合
                ret = lastSinDate;
            }

            if (ryoyoEndDate > 0 && ryoyoEndDate / 100 == sinYm && ret > ryoyoEndDate)
            {
                // 療養終了日の指定があり、当月である場合、療養終了日は指定の値を超えないようにする
                ret = ryoyoEndDate;
            }

            return ret;

        }

        private RousaiReceiptModel GetRousaiReceiptDataForJibai(long ptId, int sinYm, ReceInfModel receInf, PtInfModel ptInfModel, PtKyuseiModel ptKyusei, HokenDataModel hokenDataModel, SinMeiViewModel sinMeiViewModel)
        {
            RousaiReceiptModel rousaiReceiptModel = null;

            rousaiReceiptModel =
                new RousaiReceiptModel(hokenDataModel.PtHokenInf, _coReceiptFinder.FindPtRousaiTenki(HpId, ptId, sinYm, receInf.HokenId), ptInfModel.PtInf, (ptKyusei != null ? ptKyusei.PtKyusei : null), receInf.RousaiCount, SeikyuYm);

            if (rousaiReceiptModel != null)
            {
                rousaiReceiptModel.JituNissu = receInf.HokenNissu;

                //療養開始日
                rousaiReceiptModel.RyoyoStartDate =
                    GetRyoyoStartDate(sinYm, sinMeiViewModel.FirstSyosinDate, hokenDataModel.PtHokenInf.RyoyoStartDate);

                //療養終了日
                rousaiReceiptModel.RyoyoEndDate = GetRyoyoEndDate(sinYm, sinMeiViewModel.LastSinDate, 0, rousaiReceiptModel.Tenki);

            }

            return rousaiReceiptModel;
        }

    }
}
