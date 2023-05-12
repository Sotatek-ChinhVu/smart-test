using Domain.Constant;
using Domain.Models.SystemConf;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Calculate.Constants;
using Reporting.Calculate.Ika.Models;
using Reporting.Calculate.Interface;
using Reporting.Calculate.Receipt.Constants;
using Reporting.Calculate.Receipt.Models;
using Reporting.Calculate.Receipt.ViewModels;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Receipt.Constants;
using Reporting.Receipt.DB;
using Reporting.Receipt.Mapper;
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
        private readonly IReadRseReportFileService _readRseReportFileService;

        public ReceiptCoReportService(ITenantProvider tenantProvider, ICoReceiptFinder coReceiptFinder, ISystemConfRepository systemConfRepository, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger, IReadRseReportFileService readRseReportFileService) : base(tenantProvider)
        {
            _coReceiptFinder = coReceiptFinder;
            _systemConfRepository = systemConfRepository;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
            _tenantProvider = tenantProvider;
            _readRseReportFileService = readRseReportFileService;
        }

        private List<Calculate.ReceFutan.Models.ReceFutanKbnModel> ReceFutanKbnModels = new();
        private Calculate.ReceFutan.Models.ReceInfModel ReceInf;

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
        private int CurrentPage;

        int _byomeiCharCount;
        int _byomeiRowCount;
        int _tekiyoCharCount;
        int _tekiyoByoCharCount;
        int _tekiyoRowCount;
        int _tekiyoRowCount2;
        int _tekiyoEnRowCount;
        int _tekiyoEnCharCount;

        List<CoReceiptByomeiModel> ByomeiModels = new List<CoReceiptByomeiModel>();
        List<CoReceiptTekiyoModel> TekiyoModels = new List<CoReceiptTekiyoModel>();
        List<CoReceiptTekiyoModel> TekiyoEnModels = new List<CoReceiptTekiyoModel>();

        private List<CoReceiptModel> CoModels;
        private CoReceiptModel CoModel;

        SeikyuType SeikyuType;

        private List<Reporting.Calculate.ReceFutan.Models.ReceFutanKbnModel> ReceFutanKbns { get; set; } = new();

        public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, bool isNoCreatingReceData = false)
        {
            var receSeikyu = _coReceiptFinder.GetReceSeikyu(hpId, ptId, hokenId, sinYm);

            var ReceFutanViewModel = new Reporting.Calculate.ReceFutan.ViewModels.ReceFutanViewModel();

            if (receSeikyu == null)
            {
                SeikyuYm = sinYm;
            }
            else
            {
                if(receSeikyu.SeikyuYm != 999999)
                {

                    SeikyuYm = receSeikyu.SeikyuYm;
                }
                else
                {
                    List<Reporting.Calculate.ReceFutan.Models.ReceInfModel> ReceInfs = ReceFutanViewModel.KaikeiTotalCalculate(ptId, sinYm);
                    List<Reporting.Calculate.ReceFutan.Models.ReceFutanKbnModel> ReceFutanKbn = ReceFutanViewModel.ReceFutanKbns;
                    var receInfCheck = ReceInfs.First(p => p.HokenId == hokenId || p.HokenId2 == hokenId);

                    if(receInfCheck != null)
                    {
                        SeikyuYm = sinYm;
                        ReceFutanKbnModels = ReceFutanKbn;
                    }
                }
            }

            var receInf = _coReceiptFinder.GetReceInf(hpId, ptId, SeikyuYm, sinYm, hokenId);

            // TODO message or somthing process here
            var target = -1;
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

            SeikyuType seikyuType = new SeikyuType(true, true, true, true, true);

            if (isNoCreatingReceData)
            {
                InitParam(hpId, ReceInf, ReceFutanKbnModels, IncludeOutDrug);
                _PrintOut();
                return new ReceiptPreviewMapper(CoModel, ByomeiModels, TekiyoModels, TekiyoEnModels, CurrentPage, HpId, Target, _systemConfRepository, _coReceiptFinder, _tekiyoRowCount, _tekiyoEnRowCount, _tekiyoRowCount2).GetData();
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
                _PrintOut();

                return new ReceiptPreviewMapper(CoModel, ByomeiModels, TekiyoModels, TekiyoEnModels, CurrentPage, HpId, Target, _systemConfRepository, _coReceiptFinder, _tekiyoRowCount, _tekiyoEnRowCount, _tekiyoRowCount2).GetData();
            }
        }

        private void _PrintOut()
        {
            CoModels = GetData();

            int i = 0;
            while (i < CoModels.Count())
            {
                CoModel = CoModels[i];

                // フォームチェック
                if (TargetIsKenpo() ||
                            (Target == TargetConst.Jibai && (int)_systemConfRepository.GetSettingValue(3001, 0, HpId) == 0))
                {
                    GetFormParam("fmReceipt.rse");
                    // 対象が社保国保または、自賠健保準拠
                    _byomeiCharCount -= 3;
                    _tekiyoCharCount -= 13;
                    _tekiyoByoCharCount -= 26;

                    if ((int)_systemConfRepository.GetSettingValue(94001, 1, HpId) == 1)
                    {
                        // 病名欄転帰日記載をする場合
                        _tekiyoByoCharCount -= 4;
                    }

                    // 病名リスト
                    MakeByoList();

                    // 摘要欄リスト
                    if (!(new int[]
                        {
                                        TargetConst.KanagawaRece2,
                                        TargetConst.FukuokaRece2,
                                        TargetConst.SagaRece2,
                                        TargetConst.MiyazakiRece2
                        }.Contains(Target)))
                    {
                        MakeTekiyoList();
                    }

                }
                else if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter }.Contains(Target) ||
                        (Target == TargetConst.Jibai && (int)_systemConfRepository.GetSettingValue(3001, 1, HpId) == 1))
                {
                    // 労災（短期、年金、アフターケア）、自賠労災準拠
                    _byomeiCharCount -= 3;
                    _tekiyoCharCount -= 13;
                    _tekiyoByoCharCount -= 26;
                    if ((int)_systemConfRepository.GetSettingValue(94001, 0, HpId) == 1)
                    {
                        _tekiyoByoCharCount -= 4;
                    }

                    MakeByoList();
                    if (Target != TargetConst.RousaiAfter)
                    {
                        MakeTekiyoEnListForRousai();
                    }
                    MakeTekiyoListForRousai();
                }

                CurrentPage = 1;

                if (Target == TargetConst.RousaiAfter)
                {
                    while (i + 1 < CoModels.Count &&
                        CoModel.PtId == CoModels[i + 1].PtId &&
                        CoModel.SinYm == CoModels[i + 1].SinYm &&
                        CoModel.HokenId == CoModels[i + 1].HokenId
                        )
                    {
                        CoModel = null;
                        CurrentPage = 1;
                        i++;
                        CoModel = CoModels[i];

                        MakeByoList();
                        if (Target != TargetConst.RousaiAfter)
                        {
                            MakeTekiyoEnListForRousai();
                        }
                        MakeTekiyoListForRousai();

                    }
                }

                i++;
            }

        }

        #region initParam
        public void InitParam(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
        {
            HpId = hpId;
            PtId.Add(ptId);
            SeikyuYm = seikyuYm;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public void InitParam(int hpId,
            Reporting.Calculate.ReceFutan.Models.ReceInfModel receInf, List<Reporting.Calculate.ReceFutan.Models.ReceFutanKbnModel> receFutanKbnModels, bool includeOutDrug)
        {
            HpId = hpId;
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
        #endregion
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
                    List<long> targetPtIds = new();

                    foreach (long addPtId in AptId)
                    {
                        if (!gettedPtId.Any(p => p == addPtId))
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


            if (PtId == null || !PtId.Any())
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
                foreach (Reporting.Calculate.ReceFutan.Models.ReceFutanKbnModel receFutanKbnModel in ReceFutanKbns)
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

            if ((int)_systemConfRepository.GetSettingValue(94001, 0, HpId) == 0) //ReceiptByomeiWordWrap
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

        private void GetFormParam(string formfile)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsTekiyo", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsEnTekiyo", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsEnTekiyo", (int)CalculateTypeEnum.GetListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsTekiyo", (int)CalculateTypeEnum.GetListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsTekiyo1", (int)CalculateTypeEnum.GetListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Receipt, formfile, fieldInputList);
            var oMycustomclassname = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            UpdateParamLocal(javaOutputData.responses ?? new());
        }

        private void UpdateParamLocal(List<ObjectCalculateResponse> result)
        {
            foreach (var item in result)
            {
                switch (item.typeInt)
                {
                    case (int)CalculateTypeEnum.GetFormatLength:
                        switch (item.listName)
                        {
                            case "lsByomei":
                                _byomeiCharCount = item.result;
                                break;
                            case "lsTekiyo":
                                _tekiyoCharCount = item.result;
                                _tekiyoByoCharCount = item.result;
                                break;
                            case "lsEnTekiyo":
                                _tekiyoEnCharCount = item.result;
                                break;

                        }
                        break;
                    case (int)CalculateTypeEnum.GetListRowCount:
                        switch (item.listName)
                        {
                            case "lsByomei":
                                _byomeiRowCount = item.result;
                                break;
                            case "lsEnTekiyo":
                                _tekiyoEnRowCount = item.result;
                                break;
                            case "lsTekiyo":
                                _tekiyoRowCount = item.result;
                                break;
                            case "lsTekiyo1":
                                _tekiyoRowCount2 = item.result;
                                break;
                        }
                        break;
                }
            }
        }

        private void MakeByoList()
        {
            string tmpByomeiStartDate = "";
            string tmpByomeiTenki = "";
            string tmpByomeiTenkiDate = "";
            string line = "";

            const string CON_BYOMEI_CONTINUE = "以下、摘要欄";

            if (_byomeiCharCount <= 0 || _byomeiRowCount <= 0) return;

            #region sub function
            // 病名リストに追加
            void _addByomeiList(string addByomei, string addStartDate, string addByomeiTenki, string addByomeiTenkiDate, ref int byomeiIndex)
            {
                bool firstLine = true;
                string wkline = addByomei;

                if (wkline != "")
                {
                    while (wkline != "")
                    {
                        CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > _byomeiCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(tmp, 1, _byomeiCharCount);
                        }

                        if (firstLine)
                        {
                            addByomeiModel.Byomei = $"({byomeiIndex})" + tmp;
                            firstLine = false;
                        }
                        else
                        {
                            addByomeiModel.Byomei = new string(' ', 3) + tmp;
                        }

                        ByomeiModels.Add(addByomeiModel);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }

                    ByomeiModels.Last().StartDate = $"({byomeiIndex})" + addStartDate;
                    ByomeiModels.Last().Tenki = addByomeiTenki + CIUtil.Copy(addByomeiTenkiDate, 3, 9);

                    byomeiIndex++;
                }
            }

            //摘要欄に追加
            void _addTekiyoList(string addByomei, string addStartDate, string addByomeiTenki, string addByomeiTenkiDate, ref int byomeiIndex)
            {
                bool firstLine = true;
                string wkline = addByomei;

                if (addByomei != "")
                {
                    while (wkline != "")
                    {
                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > _tekiyoByoCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(wkline, 1, _tekiyoByoCharCount);
                        }

                        CoReceiptTekiyoModel addTekiyo = new CoReceiptTekiyoModel();

                        if (firstLine)
                        {
                            addTekiyo.Tekiyo = $"({byomeiIndex})".PadRight(4, ' ') + tmp;
                            firstLine = false;
                        }
                        else
                        {
                            addTekiyo.Tekiyo = new string(' ', 4) + tmp;
                        }

                        TekiyoModels.Add(addTekiyo);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }

                    if (TargetIsKenpo())
                    {
                        TekiyoModels.Last().Tekiyo = CIUtil.PadRightB(TekiyoModels.Last().Tekiyo, _tekiyoByoCharCount + 4) + "  " + addStartDate + addByomeiTenki + CIUtil.Copy(addByomeiTenkiDate, 9, 3);
                    }

                    byomeiIndex++;
                }
            }

            //病名データ追加後の病名欄の行数を計算
            int _ifAddRowCount(string addLine)
            {
                int lineCount = CIUtil.LenB(addLine) / _byomeiCharCount;
                if (CIUtil.LenB(addLine) % _byomeiCharCount > 0)
                {
                    lineCount++;
                }

                return ByomeiModels.Count() + lineCount;
            }

            //病名転帰日、出力設定でない場合は空文字を返す
            string _getTenkiDate(SyobyoDataModel syobyoData)
            {
                string ret = "";

                if ((int)_systemConfRepository.GetSettingValue(94001, 1, HpId) == 1 && syobyoData.KamiReceTenkiKbn > ReceTenkiKbnConst.Continued)
                {
                    ret = syobyoData.WTenkiDate;
                }
                return ret;
            }
            #endregion

            // 0-1行複数病名、1-1行1病名、2-1行複数病名(1行にできるだけまとめる）
            int wordwrap = 0;
            if (((int)_systemConfRepository.GetSettingValue(94001, 0, HpId) == 1) || !TargetIsKenpo())
            {
                wordwrap = 1;
            }
            else if ((int)_systemConfRepository.GetSettingValue(94001, 0, HpId) == 2)
            {
                wordwrap = 2;
            }


            if (wordwrap == 0)
            {
                // 1行複数病名

                // まず、_byomeiRowCount行以上必要か考えてみる
                bool overByomeiListRowCount = false;
                int byoIndex = 1;

                for (int j = 0; j < CoModel.SyobyoData.Count(); j++)
                {
                    if (tmpByomeiStartDate == CoModel.SyobyoData[j].WStartDate &&
                       tmpByomeiTenki == CoModel.SyobyoData[j].ReceTenki &&
                       tmpByomeiTenkiDate == _getTenkiDate(CoModel.SyobyoData[j]))
                    {
                        string tmpLine = line;

                        if (tmpLine != "") { tmpLine += ","; }
                        tmpLine += CoModel.SyobyoData[j].ReceByomei;

                        if (_ifAddRowCount(tmpLine) > _byomeiRowCount)
                        {
                            overByomeiListRowCount = true;
                            break;
                        }
                        else
                        {
                            line = tmpLine;
                        }
                    }
                    else
                    {
                        _addByomeiList(line, CoModel.SyobyoData[j].WStartDate, CoModel.SyobyoData[j].ReceTenki, _getTenkiDate(CoModel.SyobyoData[j]), ref byoIndex);
                        line = CoModel.SyobyoData[j].ReceByomei;

                        if (_ifAddRowCount(line) > _byomeiRowCount)
                        {
                            overByomeiListRowCount = true;
                            break;
                        }

                        tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                        tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                        tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                    }
                }

                // 最後をチェック
                if (overByomeiListRowCount == false && line != "")
                {
                    overByomeiListRowCount = _ifAddRowCount(line) > _byomeiRowCount;
                }

                int stopLine = _byomeiRowCount;
                if (overByomeiListRowCount)
                {
                    stopLine--;
                }

                // 改めて追加
                tmpByomeiStartDate = "";
                tmpByomeiTenki = "";
                tmpByomeiTenkiDate = "";

                ByomeiModels.Clear();

                byoIndex = 1;

                line = "";

                bool addTekiyo = false;

                for (int j = 0; j < CoModel.SyobyoData.Count(); j++)
                {
                    if (ByomeiModels.Count() < _byomeiRowCount && addTekiyo == false)
                    {
                        if (tmpByomeiStartDate == CoModel.SyobyoData[j].WStartDate &&
                           tmpByomeiTenki == CoModel.SyobyoData[j].ReceTenki &&
                           tmpByomeiTenkiDate == _getTenkiDate(CoModel.SyobyoData[j]))
                        {
                            string tmpLine = line;

                            if (tmpLine != "") { tmpLine += ","; }
                            tmpLine += CoModel.SyobyoData[j].ReceByomei;

                            if (_ifAddRowCount(tmpLine) > stopLine)
                            {
                                if (overByomeiListRowCount)
                                {
                                    CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                                    addByomeiModel.Byomei = CON_BYOMEI_CONTINUE;

                                    ByomeiModels.Add(addByomeiModel);
                                }

                                // 摘要欄に追加
                                addTekiyo = true;
                            }

                            line = tmpLine;
                        }
                        else
                        {

                            if (line != "")
                            {
                                // 病名欄リストに追加
                                _addByomeiList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                            }

                            line = CoModel.SyobyoData[j].ReceByomei;

                            if (_ifAddRowCount(line) > stopLine)
                            {
                                if (overByomeiListRowCount)
                                {
                                    // 病名欄の最終行に「以下、摘要欄」を追加
                                    CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                                    addByomeiModel.Byomei = CON_BYOMEI_CONTINUE;

                                    ByomeiModels.Add(addByomeiModel);
                                }

                                // 摘要欄に追加
                                addTekiyo = true;
                            }

                            tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                            tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                            tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                        }
                    }
                    else
                    {
                        if (tmpByomeiStartDate == CoModel.SyobyoData[j].WStartDate &&
                           tmpByomeiTenki == CoModel.SyobyoData[j].ReceTenki &&
                           tmpByomeiTenkiDate == _getTenkiDate(CoModel.SyobyoData[j]))
                        {
                            string tmpLine = line;

                            if (tmpLine != "") { tmpLine += ","; }
                            tmpLine += CoModel.SyobyoData[j].ReceByomei;

                            line = tmpLine;
                        }
                        else
                        {

                            if (line != "")
                            {
                                // 摘要欄に追加
                                _addTekiyoList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                            }

                            line = CoModel.SyobyoData[j].ReceByomei;

                            tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                            tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                            tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                        }
                    }
                }

                // 病名欄に空き行がある場合、残りの病名を追加する
                if (line != "")
                {
                    if (ByomeiModels.Count < _byomeiRowCount && addTekiyo == false)
                    {
                        _addByomeiList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                    }
                    else
                    {
                        //摘要欄に追加
                        _addTekiyoList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                    }
                    line = "";
                }
            }
            else if (wordwrap == 1)
            {
                // 1行1病名
                int byoIndex = 1;
                for (int j = 0; j < CoModel.SyobyoData.Count(); j++)
                {
                    if (ByomeiModels.Count() < _byomeiRowCount)
                    {
                        //string tmpLine = "";// = CoModel.SyobyoData[j].ReceByomei;
                        //if (tmpLine != "") { tmpLine += ","; }
                        string tmpLine = CoModel.SyobyoData[j].ReceByomei;
                        int lineCount = _ifAddRowCount(tmpLine);

                        if (lineCount > _byomeiRowCount || (lineCount == _byomeiRowCount && (CoModel.SyobyoData.Count - j - 1 > 0)))
                        {
                            for (int k = ByomeiModels.Count(); k < _byomeiRowCount - 1; k++)
                            {
                                ByomeiModels.Add(new CoReceiptByomeiModel());
                            }

                            CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();
                            addByomeiModel.Byomei = CON_BYOMEI_CONTINUE;
                            ByomeiModels.Add(addByomeiModel);

                            // 摘要欄に追加
                            _addTekiyoList(tmpLine, CoModel.SyobyoData[j].WStartDate, CoModel.SyobyoData[j].ReceTenki, _getTenkiDate(CoModel.SyobyoData[j]), ref byoIndex);
                        }
                        else
                        {
                            _addByomeiList(tmpLine, CoModel.SyobyoData[j].WStartDate, CoModel.SyobyoData[j].ReceTenki, _getTenkiDate(CoModel.SyobyoData[j]), ref byoIndex);
                        }
                        tmpLine = "";
                    }
                    else
                    {
                        string tmpLine = CoModel.SyobyoData[j].ReceByomei;
                        _addTekiyoList(tmpLine, CoModel.SyobyoData[j].WStartDate, CoModel.SyobyoData[j].ReceTenki, _getTenkiDate(CoModel.SyobyoData[j]), ref byoIndex);
                        tmpLine = "";
                    }
                }
            }
            else if (wordwrap == 2)
            {
                // 1行複数病名（できるだけ行の途中で病名を分けない）

                // まず、_byomeiRowCount行以上必要か考えてみる
                bool overByomeiListRowCount = false;
                int byoIndex = 1;

                for (int j = 0; j < CoModel.SyobyoData.Count(); j++)
                {
                    if (tmpByomeiStartDate == CoModel.SyobyoData[j].WStartDate &&
                       tmpByomeiTenki == CoModel.SyobyoData[j].ReceTenki &&
                       tmpByomeiTenkiDate == _getTenkiDate(CoModel.SyobyoData[j]))
                    {
                        string tmpLine = line;

                        if (tmpLine != "") { tmpLine += ","; }
                        tmpLine += CoModel.SyobyoData[j].ReceByomei;

                        if (CIUtil.LenB(tmpLine) <= _byomeiCharCount && string.IsNullOrEmpty(line) == false)
                        {
                            if (_ifAddRowCount(tmpLine) > _byomeiRowCount)
                            {
                                overByomeiListRowCount = true;
                                break;
                            }
                            else
                            {
                                line = tmpLine;
                            }
                        }
                        else
                        {
                            _addByomeiList(line, CoModel.SyobyoData[j].WStartDate, CoModel.SyobyoData[j].ReceTenki, _getTenkiDate(CoModel.SyobyoData[j]), ref byoIndex);
                            line = CoModel.SyobyoData[j].ReceByomei;

                            if (_ifAddRowCount(line) > _byomeiRowCount)
                            {
                                overByomeiListRowCount = true;
                                break;
                            }

                            tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                            tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                            tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                        }
                    }
                    else
                    {
                        _addByomeiList(line, CoModel.SyobyoData[j].WStartDate, CoModel.SyobyoData[j].ReceTenki, _getTenkiDate(CoModel.SyobyoData[j]), ref byoIndex);
                        line = CoModel.SyobyoData[j].ReceByomei;

                        if (_ifAddRowCount(line) > _byomeiRowCount)
                        {
                            overByomeiListRowCount = true;
                            break;
                        }

                        tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                        tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                        tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                    }
                }

                // 最後をチェック
                if (overByomeiListRowCount == false && line != "")
                {
                    overByomeiListRowCount = _ifAddRowCount(line) > _byomeiRowCount;
                }

                int stopLine = _byomeiRowCount;
                if (overByomeiListRowCount)
                {
                    stopLine--;
                }

                // 改めて追加
                tmpByomeiStartDate = "";
                tmpByomeiTenki = "";
                tmpByomeiTenkiDate = "";

                ByomeiModels.Clear();

                byoIndex = 1;

                line = "";

                bool addTekiyo = false;

                for (int j = 0; j < CoModel.SyobyoData.Count(); j++)
                {
                    if (ByomeiModels.Count() < _byomeiRowCount && addTekiyo == false)
                    {
                        if (tmpByomeiStartDate == CoModel.SyobyoData[j].WStartDate &&
                           tmpByomeiTenki == CoModel.SyobyoData[j].ReceTenki &&
                           tmpByomeiTenkiDate == _getTenkiDate(CoModel.SyobyoData[j]))
                        {
                            string tmpLine = line;

                            if (tmpLine != "") { tmpLine += ","; }
                            tmpLine += CoModel.SyobyoData[j].ReceByomei;
                            if (CIUtil.LenB(tmpLine) <= _byomeiCharCount && string.IsNullOrEmpty(line) == false)
                            {
                                if (_ifAddRowCount(tmpLine) > stopLine)
                                {
                                    if (overByomeiListRowCount)
                                    {
                                        CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                                        addByomeiModel.Byomei = CON_BYOMEI_CONTINUE;

                                        ByomeiModels.Add(addByomeiModel);
                                    }

                                    // 摘要欄に追加
                                    addTekiyo = true;
                                }

                                line = tmpLine;
                            }
                            else
                            {
                                if (line != "")
                                {
                                    // 病名欄リストに追加
                                    _addByomeiList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                                }

                                line = CoModel.SyobyoData[j].ReceByomei;

                                if (_ifAddRowCount(line) > stopLine)
                                {
                                    if (overByomeiListRowCount)
                                    {
                                        // 病名欄の最終行に「以下、摘要欄」を追加
                                        CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                                        addByomeiModel.Byomei = CON_BYOMEI_CONTINUE;

                                        ByomeiModels.Add(addByomeiModel);
                                    }

                                    // 摘要欄に追加
                                    addTekiyo = true;
                                }

                                tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                                tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                                tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                            }
                        }
                        else
                        {

                            if (line != "")
                            {
                                // 病名欄リストに追加
                                _addByomeiList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                            }

                            line = CoModel.SyobyoData[j].ReceByomei;

                            if (_ifAddRowCount(line) > stopLine)
                            {
                                if (overByomeiListRowCount)
                                {
                                    // 病名欄の最終行に「以下、摘要欄」を追加
                                    CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                                    addByomeiModel.Byomei = CON_BYOMEI_CONTINUE;

                                    ByomeiModels.Add(addByomeiModel);
                                }

                                // 摘要欄に追加
                                addTekiyo = true;
                            }

                            tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                            tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                            tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                        }
                    }
                    else
                    {
                        if (tmpByomeiStartDate == CoModel.SyobyoData[j].WStartDate &&
                           tmpByomeiTenki == CoModel.SyobyoData[j].ReceTenki &&
                           tmpByomeiTenkiDate == _getTenkiDate(CoModel.SyobyoData[j]))
                        {
                            string tmpLine = line;

                            if (tmpLine != "") { tmpLine += ","; }
                            tmpLine += CoModel.SyobyoData[j].ReceByomei;

                            if (CIUtil.LenB(tmpLine) <= _tekiyoByoCharCount && string.IsNullOrEmpty(line) == false)
                            {
                                line = tmpLine;
                            }
                            else
                            {
                                if (line != "")
                                {
                                    // 摘要欄に追加
                                    _addTekiyoList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                                }

                                line = CoModel.SyobyoData[j].ReceByomei;

                                tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                                tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                                tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                            }
                        }
                        else
                        {

                            if (line != "")
                            {
                                // 摘要欄に追加
                                _addTekiyoList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                            }

                            line = CoModel.SyobyoData[j].ReceByomei;

                            tmpByomeiStartDate = CoModel.SyobyoData[j].WStartDate;
                            tmpByomeiTenki = CoModel.SyobyoData[j].ReceTenki;
                            tmpByomeiTenkiDate = _getTenkiDate(CoModel.SyobyoData[j]);
                        }
                    }
                }

                // 病名欄に空き行がある場合、残りの病名を追加する
                if (line != "")
                {
                    if (ByomeiModels.Count < _byomeiRowCount && addTekiyo == false)
                    {
                        _addByomeiList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                    }
                    else
                    {
                        //摘要欄に追加
                        _addTekiyoList(line, tmpByomeiStartDate, tmpByomeiTenki, tmpByomeiTenkiDate, ref byoIndex);
                    }
                    line = "";
                }
            }

            if (TekiyoModels.Any())
            {
                // 摘要欄に病名を追加する場合、区切り線を付けておく
                CoReceiptTekiyoModel addTekiyo = new CoReceiptTekiyoModel();
                addTekiyo.Tekiyo = new string('-', _tekiyoCharCount + 13);
                TekiyoModels.Add(addTekiyo);
            }
        }

        private bool TargetIsKenpo()
        {
            return new int[]
                {
                    TargetConst.Syaho,
                    TargetConst.Kokuho,
                    TargetConst.Kenpo,
                    TargetConst.Jihi,
                    TargetConst.IwateRece2,
                    TargetConst.KanagawaRece2,
                    TargetConst.NaganoRece2,
                    TargetConst.OsakaSyouni,
                    TargetConst.FukuokaRece2,
                    TargetConst.SagaRece2
                }.Contains(Target);
        }

        private void MakeTekiyoList()
        {
            #region sub function
            //摘要欄に追加
            void _addTekiyoList(string addSinId, string addMark, string addTekiyo, string addTenCount, string addSuryo, string addUnit, int maxCharCount)
            {
                bool firstLine = true;

                AddTekiyoList(TekiyoModels, addSinId, addMark, addTekiyo, addTenCount, addSuryo, addUnit, maxCharCount, 13);
            }

            // 単純に摘要欄に文字列追加する
            void _addTekiyoLine(string sinId, string mark, string tekiyo, string tenCount)
            {
                AddTekiyoLine(TekiyoModels, sinId, mark, tekiyo, tenCount, _tekiyoCharCount, 13);
            }
            #endregion

            // 公費３，４がある場合
            List<(int receSbt, int kohiIndex, string kohiNo)> kohiCheck =
                new List<(int receSbt, int kohiIndex, string kohiNo)>
                {
                    (4, 3, "３"),
                    (5, 4, "４")
                };

            // 公費のみの場合
            if (CoModel.GetReceiptSbt(2) == 2)
            {
                kohiCheck =
                    new List<(int receSbt, int kohiIndex, string kohiNo)>
                    {
                        (3, 3, "３"),
                        (4, 4, "４")
                    };
            }

            bool addKohiInf = false;

            for (int i = 0; i <= 1; i++)
            {
                if (CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= kohiCheck[i].receSbt)
                {
                    if (CoModel.KohiFutansyaNo(kohiCheck[i].kohiIndex) != "")
                    {
                        _addTekiyoLine("", "", $"第{kohiCheck[i].kohiNo}公費", "");
                        _addTekiyoLine("", "", $"公{kohiCheck[i].kohiNo}（{CoModel.KohiFutansyaNo(kohiCheck[i].kohiIndex)}）", "");
                        _addTekiyoLine("", "", $"受　（{CoModel.KohiJyukyusyaNo(kohiCheck[i].kohiIndex)}）", "");
                        _addTekiyoLine("", "", $"実　（{CoModel.KohiNissu(kohiCheck[i].kohiIndex)}）", "");

                        addKohiInf = true;
                    }
                }
            }

            if (addKohiInf)
            {
                _addTekiyoLine("", "", new string('-', _tekiyoCharCount + 13), "");
            }

            // 明細
            int tmpSinId = 0;

            List<SinMeiDataModel> sinmeiDatas =
                CoModel.SinMeiData
                .OrderBy(p => p.FutanSortKey)
                .ThenBy(p => p.RpNo)
                .ThenBy(p => p.SeqNo)
                .ThenBy(p => p.RowNo).ToList();

            int tmpFutanSortKey = 0;

            HashSet<int> KohiIndex = new HashSet<int>();
            List<string> futanKbns = CoModel.TenColFutanKbns;

            if (futanKbns.Count() > 1)
            {
                // 負担区分が複数存在する
                foreach (string futanKbn in futanKbns)
                {
                    foreach (int index in CoModel.FutanKbnToKohiIndex(futanKbn))
                    {
                        KohiIndex.Add(index);
                    }
                }
            }

            // 公費インデックスと公費名称を紐づけ
            List<(int kohiIndex, string kohiName)> kohiNames = new List<(int kohiIndex, string kohiName)>();
            List<(string futanKbn, string kohiName)> futanToKohiName = new List<(string futanKbn, string kohiName)>();

            if (KohiIndex.Count() > 1)
            {
                for (int i = 1; i <= CoModel.KohiCount; i++)
                {
                    List<CoHokenMstModel> hokenMst =
                        _coReceiptFinder.FindHokenMst(
                            HpId,
                            CoModel.SinYm * 100 + 1,
                            CoModel.KohiHokenNo(i),
                            CoModel.KohiHokenEdaNo(i),
                            CoModel.KohiPrefNo(i));
                    if (hokenMst.Any())
                    {
                        string kohiName = "";
                        if (string.IsNullOrEmpty(hokenMst.First().HokenName))
                        {
                            // 公費の名称が取得できなかった場合
                            kohiName = $"公費{i}";
                        }
                        else
                        {
                            kohiName = hokenMst.First().HokenName;
                        }

                        for (int j = 0; j < kohiNames.Count; j++)
                        {
                            if (kohiName == kohiNames[j].kohiName)
                            {
                                // 同じ名前の公費があった場合は、後ろに公費の番号を付ける
                                kohiName = kohiName + $"(公費{i})";
                                kohiNames[j] = (kohiNames[j].kohiIndex, kohiNames[j].kohiName + $"(公費{kohiNames[j].kohiIndex})");
                                break;
                            }
                        }
                        kohiNames.Add((i, kohiName));
                    }
                }

                // 負担区分と公費名称を紐づけ
                foreach (string futanKbn in sinmeiDatas.GroupBy(p => p.FutanKbn).Select(p => p.Key))
                {
                    string kohiName = "";

                    // 負担区分から使用している公費を取得
                    List<int> kohiIndexes = CoModel.FutanKbnToKohiIndex(futanKbn);
                    for (int i = 0; i < kohiIndexes.Count(); i++)
                    {
                        if (kohiNames.Any(p => p.kohiIndex == kohiIndexes[i]))
                        {
                            if (kohiName != "") kohiName += ",";
                            kohiName += kohiNames.Find(p => p.kohiIndex == kohiIndexes[i]).kohiName;
                        }
                    }
                    futanToKohiName.Add((futanKbn, kohiName));
                }
            }

            foreach (SinMeiDataModel sinmeiData in sinmeiDatas)
            {
                string mark = "";
                string sinId = "";

                if (tmpFutanSortKey != sinmeiData.FutanSortKey && !(new int[] { 1, 99 }.Contains(sinmeiData.SinId)))
                {
                    //ソートキーが変わった場合で、レセコメント以外

                    //公費摘要のメッセージを作成
                    string kohiName = "";

                    if (futanToKohiName.Any(p => p.futanKbn == sinmeiData.FutanKbn))
                    {
                        kohiName = futanToKohiName.Find(p => p.futanKbn == sinmeiData.FutanKbn).kohiName;
                    }

                    if (kohiName != "")
                    {
                        // 印字すべき公費だった場合
                        if (TekiyoModels.Count() % _tekiyoRowCount != 0)
                        {
                            _addTekiyoLine("", "", "", "");
                        }
                        int equalCount = (_tekiyoCharCount + 13 - CIUtil.LenB(kohiName) - 18);
                        int leftCount = equalCount / 2;
                        int rightCount = equalCount / 2;
                        if (equalCount % 2 == 1)
                        {
                            rightCount++;
                        }

                        if (leftCount < 0)
                        {
                            leftCount = 0;
                        }
                        if (rightCount < 0)
                        {
                            rightCount = 0;
                        }
                        _addTekiyoLine("", "", (new string('=', leftCount)) + $"  以下、{kohiName}　適用分  " + (new string('=', rightCount)), "");
                    }

                    if (tmpSinId != 1)
                    {
                        tmpSinId = 0;
                    }
                }
                tmpFutanSortKey = sinmeiData.FutanSortKey;

                if (new int[] { 1 }.Contains(sinmeiData.SinId))
                {
                    //レセコメントヘッダー
                    tmpSinId = sinmeiData.SinId;
                }
                else
                {
                    if (sinmeiData.RowNo == 1 && sinmeiData.SinId != 99)
                    {
                        mark = "*";
                    }

                    if (sinmeiData.SinId > 0 && sinmeiData.SinId != tmpSinId)
                    {
                        if (tmpSinId > 0)
                        {
                            _addTekiyoLine("", "", new string('-', _tekiyoCharCount + 13), "");
                        }

                        if (sinmeiData.SinId != 99)
                        {
                            // レセコメントの場合、診療区分を表示したくないので、99の場合は通さない。（1の場合は上の分岐で別れるので大丈夫）
                            sinId = CIUtil.ToStringIgnoreZero(sinmeiData.SinId);
                        }
                        tmpSinId = sinmeiData.SinId;
                    }
                    else if (sinmeiData.SinIdOrg > 0 && sinmeiData.SinIdOrg != tmpSinId)
                    {
                        // 並び順の変更等で同一Rp番号内で別れた場合に備える
                        if (tmpSinId > 0)
                        {
                            _addTekiyoLine("", "", new string('-', _tekiyoCharCount + 13), "");
                        }

                        if (sinmeiData.SinIdOrg != 99)
                        {
                            // レセコメントの場合、診療区分を表示したくないので、99の場合は通さない。（1の場合は上の分岐で別れるので大丈夫）
                            sinId = CIUtil.ToStringIgnoreZero(sinmeiData.SinIdOrg);
                        }
                        tmpSinId = sinmeiData.SinIdOrg;
                    }
                }

                if (new int[] { 1, 99 }.Contains(sinmeiData.SinId))
                {
                    //レセコメントの場合、幅が広い
                    _addTekiyoList(sinId, "", sinmeiData.ItemName, "", "", "", _tekiyoCharCount + 13);
                }
                else
                {
                    //レセコメント以外の場合
                    _addTekiyoList(sinId, mark, sinmeiData.ItemName, sinmeiData.TenKai, sinmeiData.SuryoDsp, sinmeiData.UnitName, _tekiyoCharCount);
                }
            }
        }

        void AddTekiyoList(List<CoReceiptTekiyoModel> targetTekiyols, string addSinId, string addMark, string addTekiyo, string addTenCount, string addSuryo, string addUnit, int maxCharCount, int tenkaiCount)
        {
            bool firstLine = true;
            //string wkline = addTekiyo;

            if (addMark != "" || addTekiyo != "")
            {
                string[] tmpLines = addTekiyo.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string tmpline in tmpLines)
                {
                    string wkline = tmpline;
                    while (wkline != "")
                    {
                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > maxCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(wkline, 1, maxCharCount);
                        }

                        CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();

                        if (firstLine)
                        {
                            addTekiyoModel.SinId = addSinId;
                            addTekiyoModel.Mark = addMark;

                            firstLine = false;
                        }

                        addTekiyoModel.Tekiyo = tmp;

                        targetTekiyols.Add(addTekiyoModel);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }

                    if (string.IsNullOrEmpty(addSuryo))
                    {
                        // 数量なし
                        targetTekiyols.Last().Tekiyo = CIUtil.PadRightB(targetTekiyols.Last().Tekiyo, maxCharCount) + CIUtil.PadLeftB(addTenCount, tenkaiCount);
                    }
                    else
                    {
                        if (CIUtil.LenB(targetTekiyols.Last().Tekiyo) <= maxCharCount - CIUtil.LenB(addSuryo + addUnit) - 1)
                        {
                            targetTekiyols.Last().Tekiyo = CIUtil.PadRightB(targetTekiyols.Last().Tekiyo, maxCharCount - CIUtil.LenB(addSuryo + addUnit) - 1) + " " + addSuryo + addUnit + CIUtil.PadLeftB(addTenCount, tenkaiCount);
                        }
                        else
                        {
                            CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();
                            addTekiyoModel.Tekiyo = CIUtil.PadRightB("", maxCharCount - CIUtil.LenB(addSuryo + addUnit) - 1) + " " + addSuryo + addUnit + CIUtil.PadLeftB(addTenCount, tenkaiCount); ;
                            targetTekiyols.Add(addTekiyoModel);
                        }
                    }
                }
            }
        }
        void AddEnTekiyoList(List<CoReceiptTekiyoModel> targetTekiyols, string addSinId, string addMark, string addTekiyo, string addTenCount, int maxCharCount, int tenkaiCount)
        {
            bool firstLine = true;
            //string wkline = addTekiyo;

            if (addMark != "" || addTekiyo != "")
            {
                string[] tmpLines = addTekiyo.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string tmpline in tmpLines)
                {
                    string wkline = tmpline;
                    while (wkline != "")
                    {
                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > maxCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(wkline, 1, maxCharCount);
                        }

                        CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();

                        if (firstLine)
                        {
                            addTekiyoModel.SinId = addSinId;
                            addTekiyoModel.Mark = addMark;

                            firstLine = false;
                        }

                        addTekiyoModel.Tekiyo = tmp;

                        targetTekiyols.Add(addTekiyoModel);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }

                    targetTekiyols.Last().Tekiyo = CIUtil.PadRightB(targetTekiyols.Last().Tekiyo, maxCharCount) + CIUtil.PadLeftB(addTenCount, tenkaiCount);
                }
            }
        }

        void AddTekiyoLine(List<CoReceiptTekiyoModel> targetTekiyols, string sinId, string mark, string tekiyo, string tenCount, int maxCharCount, int tenkaiCount)
        {
            CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();

            addTekiyoModel.SinId = sinId;
            addTekiyoModel.Mark = mark;
            addTekiyoModel.Tekiyo = CIUtil.PadRightB(tekiyo, maxCharCount) + CIUtil.PadLeftB(tenCount, tenkaiCount);

            targetTekiyols.Add(addTekiyoModel);
        }

        private void MakeTekiyoEnListForRousai()
        {
            #region sub function
            //摘要欄に追加
            void _addEnTekiyoList(string addSinId, string addMark, string addTekiyo, string addTenCount, int maxCharCount)
            {
                AddEnTekiyoList(TekiyoEnModels, addSinId, addMark, addTekiyo, addTenCount, maxCharCount, 0);
            }
            void _addTekiyoList(string addSinId, string addMark, string addTekiyo, string addTenCount, int maxCharCount)
            {
                AddTekiyoList(TekiyoModels, addSinId, addMark, addTekiyo, addTenCount, "", "", maxCharCount, 13);
            }
            #endregion

            // 明細
            List<SinMeiDataModel> sinmeiDatas = CoModel.SinMeiData.Where(p => (rosaiTargetSyukeiSakils.Contains(p.SyukeiSaki) && p.SyukeiSaki != "A110" && p.SyukeiSaki != "A120")).OrderBy(p => p.ReceSortKey).ToList();

            foreach (SinMeiDataModel sinmeiData in sinmeiDatas)
            {
                string mark = "";
                string sinId = "";

                if (sinmeiData.RowNo == 1)
                {
                    mark = "*";
                }

                string tenKai = "";
                if (sinmeiData.TenKai != "")
                {
                    if (sinmeiData.EnTenKbn == 0)
                    {
                        // 点数項目
                        tenKai = $"{sinmeiData.Ten,5}x{sinmeiData.Count,3}";
                    }
                    else
                    {
                        // 金額項目
                        tenKai = $"{sinmeiData.Kingaku,5}円x{sinmeiData.Count,3}";
                    }
                }

                if (_tekiyoEnRowCount > 0 && _tekiyoEnCharCount > 0 &&
                    (TekiyoEnModels.Count() +
                     GetNeedLineCountTekiyo(sinId, mark, sinmeiData.ItemName.Trim() + tenKai, "", _tekiyoEnCharCount, 0) <= _tekiyoEnRowCount))
                {
                    _addEnTekiyoList(sinId, mark, sinmeiData.ItemName.Trim() + tenKai, "", _tekiyoEnCharCount);
                }
                else
                {
                    // 長いときは摘要欄に印字する
                    _addTekiyoList(sinId, mark, sinmeiData.ItemName.Trim(), sinmeiData.TenKai, _tekiyoCharCount);
                }
            }
        }

        private List<string> rosaiTargetSyukeiSakils =
            new List<string>
            {
                    ReceSyukeisaki.EnSyosin,
                    ReceSyukeisaki.EnSaisin,
                    ReceSyukeisaki.EnSido,
                    ReceSyukeisaki.EnKyukyu,
                    ReceSyukeisaki.EnSonota,
            };

        int GetNeedLineCountTekiyo(string addSinId, string addMark, string addTekiyo, string addTenCount, int maxCharCount, int tenkaiCount)
        {
            List<CoReceiptTekiyoModel> targetTekiyols = new List<CoReceiptTekiyoModel>();
            bool firstLine = true;
            //string wkline = addTekiyo;

            if (addMark != "" || addTekiyo != "")
            {
                string[] tmpLines = addTekiyo.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string tmpline in tmpLines)
                {
                    string wkline = tmpline;
                    while (wkline != "")
                    {
                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > maxCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(wkline, 1, maxCharCount);
                        }

                        CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();

                        if (firstLine)
                        {
                            addTekiyoModel.SinId = addSinId;
                            addTekiyoModel.Mark = addMark;

                            firstLine = false;
                        }

                        addTekiyoModel.Tekiyo = tmp;

                        targetTekiyols.Add(addTekiyoModel);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }
                    targetTekiyols.Last().Tekiyo = CIUtil.PadRightB(targetTekiyols.Last().Tekiyo, maxCharCount) + CIUtil.PadLeftB(addTenCount, tenkaiCount);
                }
            }

            return targetTekiyols.Count();
        }

        private void MakeTekiyoListForRousai()
        {
            #region sub function
            //摘要欄に追加
            void _addTekiyoList(string addSinId, string addMark, string addTekiyo, string addTenCount, string addSuryo, string addUnit, int maxCharCount)
            {
                AddTekiyoList(TekiyoModels, addSinId, addMark, addTekiyo, addTenCount, addSuryo, addUnit, maxCharCount, 13);
            }

            // 単純に摘要欄に文字列追加する
            void _addTekiyoLine(string sinId, string mark, string tekiyo, string tenCount)
            {
                AddTekiyoLine(TekiyoModels, sinId, mark, tekiyo, tenCount, _tekiyoCharCount, 13);
            }
            #endregion

            // 明細
            int tmpSinId = 0;

            List<SinMeiDataModel> sinmeiDatas = CoModel.SinMeiData.Where(p => !(rosaiTargetSyukeiSakils.Contains(p.SyukeiSaki))).OrderBy(p => p.ReceSortKey).ToList();

            bool first = true;
            foreach (SinMeiDataModel sinmeiData in sinmeiDatas)
            {
                string mark = "";
                string sinId = "";

                if (new int[] { 1 }.Contains(sinmeiData.SinId))
                {
                    //レセコメントヘッダー
                    tmpSinId = sinmeiData.SinId;
                }
                else
                {
                    if (sinmeiData.RowNo == 1 && sinmeiData.SinId != 99)
                    {
                        mark = "*";
                    }

                    if (sinmeiData.SinId > 0 && sinmeiData.SinId != tmpSinId)
                    {
                        if (tmpSinId > 0)
                        {
                            _addTekiyoLine("", "", new string('-', _tekiyoCharCount + 13), "");
                        }

                        sinId = CIUtil.ToStringIgnoreZero(sinmeiData.SinId);
                        tmpSinId = sinmeiData.SinId;
                    }
                }

                if (new int[] { 1, 99 }.Contains(sinmeiData.SinId))
                {
                    //レセコメントの場合、幅が広い
                    _addTekiyoList(sinId, "", sinmeiData.ItemName, "", "", "", _tekiyoCharCount + 13);
                }
                else
                {
                    if (first && string.IsNullOrEmpty(sinId))
                    {
                        sinId = CIUtil.ToStringIgnoreZero(sinmeiData.SinIdOrg);
                    }
                    //レセコメント以外の場合
                    _addTekiyoList(sinId, mark, sinmeiData.ItemName, sinmeiData.TenKai, sinmeiData.SuryoDsp, sinmeiData.UnitName, _tekiyoCharCount);

                    first = false;
                }
            }
        }
    }
}
