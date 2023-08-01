﻿using Domain.Constant;
using Domain.Models.SystemConf;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Calculate.Constants;
using Reporting.Calculate.Ika.Models;
using Reporting.Calculate.Interface;
using Reporting.Calculate.ReceFutan.ViewModels;
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
using static Helper.Common.CIUtil;
using ReceFutanReceFutanKbnModel = Reporting.Calculate.ReceFutan.Models.ReceFutanKbnModel;
using ReceFutanReceInfModel = Reporting.Calculate.ReceFutan.Models.ReceInfModel;

namespace Reporting.Receipt.Service
{
    public class ReceiptCoReportService : RepositoryBase, IReceiptCoReportService
    {
        private const string RECEIPT_CHECK_FORM_FILE_NAME = "fmRezeCheck.rse";
        private const string RECEIPT_KENPO_FORM_FILE_NAME = "fmReceipt.rse";
        private const string RECEIPT_KANAGAWA2_FORM_FILE_NAME = "fmReceipt_Kanagawa2.rse";
        private const string RECEIPT_ROSAI_TANKI_FORM_FILE_NAME = "fmReceipt_Rosai_Tanki_{0}.rse";
        private const string RECEIPT_ROSAI_TANKI_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_Tanki_Overlay.rse";
        private const string RECEIPT_ROSAI_TANKI_201905_FORM_FILE_NAME = "fmReceipt_Rosai_Tanki_201905_{0}.rse";
        private const string RECEIPT_ROSAI_TANKI_201905_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_Tanki_201905_Overlay.rse";
        private const string RECEIPT_ROSAI_NENKIN_FORM_FILE_NAME = "fmReceipt_Rosai_Nenkin_{0}.rse";
        private const string RECEIPT_ROSAI_NENKIN_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_Nenkin_Overlay.rse";
        private const string RECEIPT_ROSAI_NENKIN_201905_FORM_FILE_NAME = "fmReceipt_Rosai_Nenkin_201905_{0}.rse";
        private const string RECEIPT_ROSAI_NENKIN_201905_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_Nenkin_201905_Overlay.rse";
        private const string RECEIPT_ROSAI_PAGE2_FORM_FILE_NAME = "fmReceipt_Rosai_2Page.rse";
        private const string RECEIPT_ROSAI_PAGE2_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_2Page_Overlay.rse";
        private const string RECEIPT_ROSAI_AFTER_FORM_FILE_NAME = "fmReceipt_Rosai_After.rse";
        private const string RECEIPT_ROSAI_AFTER_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_After_Overlay.rse";
        private const string RECEIPT_ROSAI_AFTER_PAGE2_FORM_FILE_NAME = "fmReceipt_Rosai_After_2Page.rse";
        private const string RECEIPT_ROSAI_AFTER_PAGE2_OVERLAY_FORM_FILE_NAME = "fmReceipt_Rosai_After_2Page_Overlay.rse";
        private const string RECEIPT_JIBAI_KENPO_FORM_FILE_NAME = "fmReceipt_Jibai_Kenpo.rse";
        private const string RECEIPT_JIBAI_ROSAI_FORM_FILE_NAME = "fmReceipt_Jibai_Rosai.rse";
        private const string RECEIPT_JIBAI_PAGE2_FORM_FILE_NAME = "fmReceipt_Jibai_2Page.rse";

        private readonly ICoReceiptFinder CoModelFinder;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        private readonly ITenantProvider _tenantProvider;
        private readonly IReadRseReportFileService _readRseReportFileService;

        public ReceiptCoReportService(ITenantProvider tenantProvider, ICoReceiptFinder coReceiptFinder, ISystemConfRepository systemConfRepository, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger, IReadRseReportFileService readRseReportFileService) : base(tenantProvider)
        {
            CoModelFinder = coReceiptFinder;
            _systemConfRepository = systemConfRepository;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
            _tenantProvider = tenantProvider;
            _readRseReportFileService = readRseReportFileService;
        }

        private List<ReceFutanReceFutanKbnModel> ReceFutanKbnModels = new();
        private ReceFutanReceInfModel ReceInf;

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


        Dictionary<string, string> SingleData = new Dictionary<string, string>();
        List<Dictionary<string, CellModel>> CellData = new List<Dictionary<string, CellModel>>();
        Dictionary<string, string> _fileName = new Dictionary<string, string>();

        private List<CoReceiptModel> CoModels;
        private CoReceiptModel CoModel;

        SeikyuType SeikyuType;

        private List<ReceFutanReceFutanKbnModel> ReceFutanKbns { get; set; } = new();

        public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, bool isNoCreatingReceData = false)
        {
            var receSeikyu = CoModelFinder.GetReceSeikyu(hpId, ptId, hokenId, sinYm);

            var receFutanViewModel = new ReceFutanViewModel();

            if (receSeikyu == null)
            {
                SeikyuYm = sinYm;
            }
            else
            {
                if (receSeikyu.SeikyuYm != 999999)
                {

                    SeikyuYm = receSeikyu.SeikyuYm;
                }
                else
                {
                    List<ReceFutanReceInfModel> ReceInfs = receFutanViewModel.KaikeiTotalCalculate(ptId, sinYm);
                    List<ReceFutanReceFutanKbnModel> ReceFutanKbn = receFutanViewModel.ReceFutanKbns;
                    var receInfCheck = ReceInfs.First(p => p.HokenId == hokenId || p.HokenId2 == hokenId);

                    if (receInfCheck != null)
                    {
                        SeikyuYm = sinYm;
                        ReceFutanKbnModels = ReceFutanKbn;
                    }
                }
            }

            var receInf = CoModelFinder.GetReceInf(hpId, ptId, SeikyuYm, sinYm, hokenId);

            if (receInf == null) return new();

            // TODO message or something process here
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
                return new ReceiptPreviewMapper(_fileName, CoModel, ByomeiModels, TekiyoModels, TekiyoEnModels, CurrentPage, HpId, Target, _systemConfRepository, CoModelFinder, _tekiyoRowCount, _tekiyoEnRowCount, _tekiyoRowCount2).GetData();
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

                return new ReceiptPreviewMapper(_fileName, CoModel, ByomeiModels, TekiyoModels, TekiyoEnModels, CurrentPage, HpId, Target, _systemConfRepository, CoModelFinder, _tekiyoRowCount, _tekiyoEnRowCount, _tekiyoRowCount2).GetData();
            }
        }

        private void _PrintOut()
        {
            CoModels = GetData();

            int i = 0;
            while (i < CoModels.Count())
            {
                CoModel = CoModels[i];
                var formName = GetFormFileName(CurrentPage);
                _fileName.Add((i + 1).ToString(), formName);
                GetFormParam(formName);

                _byomeiCharCount -= 3;
                _tekiyoCharCount -= 13;
                _tekiyoByoCharCount -= 26;
                // フォームチェック
                if (TargetIsKenpo() ||
                            (Target == TargetConst.Jibai && (int)_systemConfRepository.GetSettingValue(3001, 0, HpId) == 0))
                {
                    // 対象が社保国保または、自賠健保準拠
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

                bool isNextPageExits = true;
                CurrentPage = 1;
                // レセプト印刷
                while (isNextPageExits)
                {
                    isNextPageExits = UpdateDrawForm();
                    CurrentPage++;

                    if (isNextPageExits && new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter, TargetConst.Jibai }.Contains(Target))
                    {
                        // 労災自賠の場合、2ページ目は様式が異なる
                        //initResult = CoInit(CurrentPage);
                        CoRep.OpenForm(GetFormFileName(CurrentPage));
                        _tekiyoRowCount2 = CoRep.GetListRowCount("lsTekiyo1");
                    }
                }

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

        private string GetFormFileName(int page)
        {
            string ret = "";

            if (Target == TargetConst.KanagawaRece2)
            {
                // 神奈川レセプト２枚目
                ret = RECEIPT_KANAGAWA2_FORM_FILE_NAME;
            }
            else if (TargetIsKenpo())
            {
                ret = RECEIPT_KENPO_FORM_FILE_NAME;
            }
            else
            {
                switch (Target)
                {
                    case TargetConst.RousaiTanki:
                        // 労災短期
                        if (page <= 1)
                        {
                            // 1ページ目
                            if ((int)_systemConfRepository.GetSettingValue(94002, 0, HpId) == 1)
                            {
                                // 2019/05 元号対応様式変更
                                //if (OutputMode == CoOutputMode.Print && Preview == false)
                                //{
                                //    // 紙出力
                                //    ret = string.Format(RECEIPT_ROSAI_TANKI_201905_FORM_FILE_NAME, 1);
                                //}
                                //else
                                //{
                                // プレビュー
                                ret = RECEIPT_ROSAI_TANKI_201905_OVERLAY_FORM_FILE_NAME;
                                //}
                            }
                            else
                            {
                                //if (OutputMode == CoOutputMode.Print && Preview == false)
                                //{
                                //    // 紙出力
                                //    ret = string.Format(RECEIPT_ROSAI_TANKI_FORM_FILE_NAME, 1);
                                //}
                                //else
                                //{
                                // プレビュー
                                ret = RECEIPT_ROSAI_TANKI_OVERLAY_FORM_FILE_NAME;
                                //}
                            }
                        }
                        else
                        {
                            // 2ページ目以降
                            //if (OutputMode == CoOutputMode.Print && Preview == false)
                            //{
                            //    ret = RECEIPT_ROSAI_PAGE2_FORM_FILE_NAME;
                            //}
                            //else
                            //{
                            ret = RECEIPT_ROSAI_PAGE2_OVERLAY_FORM_FILE_NAME;
                            //}
                        }
                        break;
                    case TargetConst.RousaiNenkin:
                        // 労災傷病年金
                        if (page <= 1)
                        {
                            // 1ページ目

                            // 2019/05 以前
                            //if (OutputMode == CoOutputMode.Print && Preview == false)
                            //{
                            //    // 紙出力
                            //    ret = string.Format(Paths.RECEIPT_ROSAI_NENKIN_FORM_FILE_NAME, 1);
                            //}
                            //else
                            //{
                            // プレビュー
                            ret = RECEIPT_ROSAI_NENKIN_OVERLAY_FORM_FILE_NAME;
                            //}
                        }
                        else
                        {
                            // 2ページ目以降
                            //if (OutputMode == CoOutputMode.Print && Preview == false)
                            //{
                            //    // 紙出力
                            //    ret = Paths.RECEIPT_ROSAI_PAGE2_FORM_FILE_NAME;
                            //}
                            //else
                            //{
                            // プレビュー
                            ret = RECEIPT_ROSAI_PAGE2_OVERLAY_FORM_FILE_NAME;
                            //}
                        }
                        break;
                    case TargetConst.RousaiAfter:
                        // アフターケア
                        if (page <= 1)
                        {
                            // 1ページ目
                            //if (OutputMode == CoOutputMode.Print && Preview == false)
                            //{
                            //    // 紙出力
                            //    ret = Paths.RECEIPT_ROSAI_AFTER_FORM_FILE_NAME;
                            //}
                            //else
                            //{
                            // プレビュー
                            ret = RECEIPT_ROSAI_AFTER_OVERLAY_FORM_FILE_NAME;
                            // }
                        }
                        else
                        {
                            // 2ページ目以降
                            //if (OutputMode == CoOutputMode.Print && Preview == false)
                            //{
                            //    // 紙出力
                            //    ret = Paths.RECEIPT_ROSAI_AFTER_PAGE2_FORM_FILE_NAME;
                            //}
                            //else
                            //{
                            // プレビュー
                            ret = RECEIPT_ROSAI_AFTER_PAGE2_OVERLAY_FORM_FILE_NAME;
                            //}
                        }
                        break;
                    case TargetConst.Jibai:
                        // 自賠
                        if (page <= 1)
                        {
                            //if (SystemConfig.Instance.JibaiJunkyo == 0)
                            //{
                            //    // 健保準拠
                            //    ret = Paths.RECEIPT_JIBAI_KENPO_FORM_FILE_NAME;
                            //}
                            //else
                            //{
                            // 労災準拠
                            ret = RECEIPT_JIBAI_ROSAI_FORM_FILE_NAME;
                            // }
                        }
                        else
                        {
                            // 2ページ目以降
                            ret = RECEIPT_JIBAI_PAGE2_FORM_FILE_NAME;
                        }
                        break;
                }
            }

            return ret;
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

        public void InitParam(int hpId, ReceFutanReceInfModel receInf, List<ReceFutanReceFutanKbnModel> receFutanKbnModels, bool includeOutDrug)
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
                            CoModelFinder.FindReceInfFukuoka(HpId, mode, Target, SeikyuYm, targetPtIds, SinYm, HokenId, ReceSbt, IncludeTester, SeikyuType.IsPaper, seikyuKbns, TantoId, KaId, GrpId)
                            .ToList());
                    }
                    else
                    {
                        receInfModels.AddRange(
                            CoModelFinder.FindReceInf(HpId, mode, Target, SeikyuYm, targetPtIds, SinYm, HokenId, ReceSbt, IncludeTester, SeikyuType.IsPaper, seikyuKbns, TantoId, KaId, GrpId)
                            .ToList());
                    }

                    // 算定情報取得

                    sinRpInfModels.AddRange(CoModelFinder.FindSinRpInfDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                    sinKouiModels.AddRange(CoModelFinder.FindSinKouiDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                    sinKouiDetailModels.AddRange(CoModelFinder.FindSinKouiDetailDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                    sinKouiCountModels.AddRange(CoModelFinder.FindSinKouiCountDataForRece(HpId, SeikyuYm, targetPtIds, SinYm, HokenId, mode, IncludeTester, seikyuKbns, TantoId, KaId));
                }
                else
                {
                    receInfModels.AddRange(CoModelFinder.FindReceInf(HpId, ReceInf));

                    int hokenId2 = 0;
                    if (receInfModels != null && receInfModels.Any())
                    {
                        hokenId2 = receInfModels.First().HokenId2;
                    }
                    // 算定情報取得
                    sinRpInfModels.AddRange(CoModelFinder.FindSinRpInfDataForPreview(HpId, AptId, SinYm));
                    sinKouiModels.AddRange(CoModelFinder.FindSinKouiDataForPreview(HpId, AptId, SinYm, HokenId, hokenId2));
                    sinKouiDetailModels.AddRange(CoModelFinder.FindSinKouiDetailDataForPreview(HpId, AptId, SinYm));
                    sinKouiCountModels.AddRange(CoModelFinder.FindSinKouiCountDataForPreview(HpId, AptId, SinYm));
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
                HpInfModel hpInfModel = CoModelFinder.FindHpInf(HpId, receInfModel.SinYm * 100 + 1);

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
                foreach (ReceFutanReceFutanKbnModel receFutanKbnModel in ReceFutanKbns)
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
            PtKyuseiModel ptKyuseiModel = CoModelFinder.FindPtKyusei(HpId, ptId, sinMeiViewModels.LastSinDate);

            // 労災レセプト情報取得
            RousaiReceiptModel rousaiReceiptModel = null;
            SyobyoKeikaModel syobyoKeikaModel = null;
            List<int> tuuinDays = new List<int>();

            if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter }.Contains(target))
            {
                // 労災レセプト情報
                rousaiReceiptModel = GetRousaiReceiptData(ptId, sinYm, receInf, ptInfModel, ptKyuseiModel, hokenDataModel, sinMeiViewModels);

                // 傷病の経過
                syobyoKeikaModel = CoModelFinder.FindSyobyoKeika(HpId, ptId, sinYm, receInf.HokenId);

            }
            else if (new int[] { TargetConst.Jibai }.Contains(target))
            {
                // 自賠責
                rousaiReceiptModel =
                    GetRousaiReceiptDataForJibai(ptId, sinYm, receInf, ptInfModel, ptKyuseiModel, hokenDataModel, sinMeiViewModels);

                // 通院日情報取得
                tuuinDays = CoModelFinder.FindTuuinDays(HpId, ptId, receInf.SinYm, receInf.HokenId);
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
                ptInfModel = CoModelFinder.FindPtInf(HpId, ptId, sinYm * 100 + 1);
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
                hokenDataModel = CoModelFinder.FindHokenData(HpId, ptId, receInf.HokenId);
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
            var kohiDatas = CoModelFinder.FindKohiData(HpId, ptId, receInf.SinYm * 100 + 1);
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
            List<SyobyoDataModel> syobyoDataModels = CoModelFinder.FindSyobyoData(HpId, ptId, sinYm, receInf.HokenId, outputYm);

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
                PtKyuseiModel ptKyuseiModel = CoModelFinder.FindPtKyusei(HpId, ptId, sinMeiDataModels.LastSinDate);

                // 労災レセプト情報
                RousaiReceiptModel rousaiReceiptModel = null;
                SyobyoKeikaModel syobyoKeikaModel = null;

                // 労災レセプト情報
                rousaiReceiptModel = GetRousaiReceiptData(ptId, sinYm, receInf, ptInfModel, ptKyuseiModel, hokenDataModel, sinMeiDataModels);
                syobyoKeikaModel = CoModelFinder.FindSyobyoKeikaForAfter(HpId, ptId, kaikeiDayInf.sinDate, receInf.HokenId);

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
                if (CoModelFinder.ZenkaiKensaDate(HpId, ptId, kaikeiDayInf.sinDate, receInf.HokenId) == kaikeiDayInf.sinDate)
                {
                    coReceiptModel.KensaDate = kaikeiDayInf.sinDate;
                }
                // 前回検査日（アフターケア用）
                coReceiptModel.ZenkaiKensaDate = CoModelFinder.ZenkaiKensaDate(HpId, ptId, kaikeiDayInf.sinDate - 1, receInf.HokenId);

                coReceiptModels.Add(coReceiptModel);
            }

            return coReceiptModels;
        }

        private List<(int sinDate, List<long> raiinNos, int SyokeiGaku_I, int SyokeiGaku_RO, int Syokei)>
            GetKaikeiDayInfs(long ptId, int sinYm, ReceInfModel receInf)
        {
            List<CoKaikeiDetailModel> kaikeiDtls = CoModelFinder.FindKaikeiDetail(HpId, ptId, sinYm, receInf.HokenId);

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
                new RousaiReceiptModel(hokenDataModel.PtHokenInf, CoModelFinder.FindPtRousaiTenki(HpId, ptId, sinYm, receInf.HokenId), ptInfModel.PtInf, (ptKyuseiModel != null ? ptKyuseiModel.PtKyusei : null), receInf.RousaiCount, SeikyuYm);

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
                new RousaiReceiptModel(hokenDataModel.PtHokenInf, CoModelFinder.FindPtRousaiTenki(HpId, ptId, sinYm, receInf.HokenId), ptInfModel.PtInf, (ptKyusei != null ? ptKyusei.PtKyusei : null), receInf.RousaiCount, SeikyuYm);

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
                        CoModelFinder.FindHokenMst(
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

        private bool UpdateDrawForm(out bool hasNextPage)
        {
            bool _hasNextPage = true;
            #region SubMethod

            // ヘッダーの印刷処理
            int UpdateFormHeader()
            {
                if (TargetIsKenpo())
                {
                    // 健保
                    PrintReceiptHeaderForKenpo();
                }
                else if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin }.Contains(Target))
                {
                    // 労災（短期・傷病年金）
                    PrintReceiptHeaderForRousai();
                }
                else if (new int[] { TargetConst.RousaiAfter }.Contains(Target))
                {
                    // アフターケア
                    PrintReceiptHeaderForAfter();
                }
                else if (new int[] { TargetConst.Jibai }.Contains(Target))
                {
                    if (_systemConfRepository.GetSettingValue(3001, 0, HpId) == 0)
                    {
                        // 自賠健保準拠
                        PrintReceiptHeaderForJibaiKenpo();
                    }
                    else
                    {
                        // 自賠労災準拠
                        PrintReceiptHeaderForJibaiRousai();
                    }
                }
                return 1;
            }

            // 本体部分印刷処理
            int UpdateFormBody()
            {

                if (_tekiyoRowCount <= 0) return -1;
                if ((TekiyoModels?.Count() ?? 0) <= 0 && (CurrentPage > 1 || (TekiyoEnModels?.Count() ?? 0) <= 0)) return -1;

                int tekiyoIndex = (CurrentPage - 1) * _tekiyoRowCount;

                //摘要欄印刷
                if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter, TargetConst.Jibai }.Contains(Target) && CurrentPage > 1)
                {
                    // 労災続紙
                    tekiyoIndex = _tekiyoRowCount + (CurrentPage - 2) * (_tekiyoRowCount2 * 2);

                    for (int i = 1; i <= 2; i++)
                    {
                        for (short j = 0; j < _tekiyoRowCount2; j++)
                        {
                            if (tekiyoIndex < TekiyoModels.Count())
                            {
                                var data = new Dictionary<string, CellModel>();
                                data.Add($"lsSinId{i}", new CellModel(TekiyoModels[tekiyoIndex].SinId));
                                data.Add($"lsTekiyoMark{i}", new CellModel(TekiyoModels[tekiyoIndex].Mark));
                                data.Add($"lsTekiyo{i}", new CellModel(TekiyoModels[tekiyoIndex].Tekiyo));
                                CellData.Add(data);
                                tekiyoIndex++;
                                if (tekiyoIndex >= TekiyoModels.Count())
                                {
                                    _hasNextPage = false;
                                    break;
                                }

                                CellData.Add(data);
                            }
                        }
                    }

                }
                else
                {
                    if (TekiyoModels.Any())
                    {
                        for (short i = 0; i < _tekiyoRowCount; i++)
                        {
                            if (tekiyoIndex < TekiyoModels.Count())
                            {
                                CoRep.ListText("lsSinId", 0, i, TekiyoModels[tekiyoIndex].SinId);
                                CoRep.ListText("lsTekiyoMark", 0, i, TekiyoModels[tekiyoIndex].Mark);
                                CoRep.ListText("lsTekiyo", 0, i, TekiyoModels[tekiyoIndex].Tekiyo);

                                tekiyoIndex++;
                                if (tekiyoIndex >= TekiyoModels.Count())
                                {
                                    _hasNextPage = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        _hasNextPage = false;
                    }
                }

                if ((new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin }.Contains(Target) || (Target == TargetConst.Jibai && SystemConfig.Instance.JibaiJunkyo == 1)) && CurrentPage == 1)
                {
                    // 労災 円項目用　本紙のみ
                    int tekiyoEnIndex = (CurrentPage - 1) * _tekiyoEnRowCount;

                    if (tekiyoEnIndex < TekiyoEnModels.Count())
                    {
                        for (short i = 0; i < _tekiyoEnRowCount; i++)
                        {
                            CoRep.ListText("lsEnTekiyo", 0, i, TekiyoEnModels[tekiyoEnIndex].Tekiyo);

                            tekiyoEnIndex++;
                            if (tekiyoEnIndex >= TekiyoEnModels.Count())
                            {
                                //_hasNextPage = false;
                                break;
                            }
                        }
                    }
                }

                // 長野県福岡県レセプト2枚目の場合、2ページ目以降は印字しない
                if (new int[] {
                    TargetConst.NaganoRece2,
                    TargetConst.FukuokaRece2 }.Contains(Target))
                {
                    tekiyoIndex = -1;
                }

                return tekiyoIndex;

            }

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }
            catch (Exception e)
            {
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            //hasNextPage = false;
            return true;
        }

        private void PrintReceiptHeaderForKenpo()
        {
            // 公費マスタを取得しておく（各種特殊処理で使用）
            List<KohiDataModel> kohiDatas = CoModelFinder.FindKohiData(HpId, CoModel.PtId, CoModel.SinYm * 100 + 1);

            #region Sub Function
            // 患者番号+法別番号
            string _getPtNo()
            {
                string ret = CoModel.PtNum.ToString();

                string houbetu = "";

                for (int i = 1; i <= 4; i++)
                {
                    if (CoModel.ReceKisai(i) == 0 &&
                        CoModel.KohiHoubetuReceInf(i) != "" &&
                        CIUtil.StrToIntDef(CoModel.KohiHoubetuReceInf(i), 999) <= 99)
                    {
                        if (houbetu != "")
                        {
                            houbetu += ",";
                        }
                        houbetu += CoModel.KohiHoubetuReceInf(i);
                    }
                }

                if (houbetu != "")
                {
                    ret += "(" + houbetu + ")";
                }

                return ret;
            }

            // 社保国保
            string _getSyaKoku()
            {
                string ret = "1 社";

                if (CoModel.HokenKbn == 2)
                {
                    ret = "2 国";
                }
                else if (CoModel.HokenKbn == 0)
                {
                    ret = "0 自";
                }

                return ret;
            }

            // 診療年月
            string _getSinYM()
            {
                string ret = "";

                int wDate = CIUtil.SDateToWDate(CoModel.SinYm * 100 + 1);
                int gengo = wDate / 1000000;

                switch (gengo)
                {
                    case 1:
                        ret = "明治　";
                        break;
                    case 2:
                        ret = "大正　";
                        break;
                    case 3:
                        ret = "昭和　";
                        break;
                    case 4:
                        ret = "平成　";
                        break;
                    case 5:
                        ret = "令和　";
                        break;
                }

                ret += string.Format("{0, 2}年{1, 2}月", wDate % 1000000 / 10000, wDate % 10000 / 100);

                return ret;
            }

            // レセ種別１
            string _getReceSbt1()
            {
                string ret = "";

                if (CoModel.HokenKbn == 0)
                {
                    ret = "0 自費";
                }
                else
                {
                    switch (CIUtil.Copy(CoModel.ReceiptSbt, 2, 1))
                    {
                        case "1":
                            if (CoModel.HokenKbn == 1)
                            {
                                ret = "1 社";
                            }
                            else
                            {
                                ret = "1 国";
                            }
                            break;
                        case "2":
                            ret = "2 公費";
                            break;
                        case "3":
                            ret = "3 後期";
                            break;
                        case "4":
                            ret = "4 退職";
                            break;
                    }
                }
                return ret;
            }

            // レセ種別２
            string _getReceSbt2()
            {
                string ret = "";

                switch (CIUtil.Copy(CoModel.ReceiptSbt, 3, 1))
                {
                    case "1":
                        ret = "1 単独";
                        break;
                    case "2":
                        ret = "2 ２併";
                        break;
                    case "3":
                    case "4":
                    case "5":
                        ret = "3 ３併";
                        break;
                }
                return ret;
            }

            // レセ種別３
            string _getReceSbt3()
            {
                string ret = "";

                switch (CIUtil.Copy(CoModel.ReceiptSbt, 4, 1))
                {
                    case "2":
                        ret = "2 本外";
                        break;
                    case "4":
                        ret = "4 六外";
                        break;
                    case "6":
                        ret = "6 家外";
                        break;
                    case "8":
                        ret = "8 高外一";
                        break;
                    case "0":
                        ret = "0 高外７";
                        break;
                    case "x":
                        if (CoModel.HokensyaNo != null && CoModel.HokensyaNo.StartsWith("97"))
                        {
                            if (CoModel.HonkeKbn == 1)
                            {
                                ret = "2 本外";
                            }
                            else if (CoModel.HonkeKbn == 2)
                            {
                                ret = "6 家外";
                            }

                            if (!(CoModel.IsStudent))
                            {
                                ret = "4 六外";
                            }
                            else if (CoModel.IsElder)
                            {
                                ret = "8 高外一";
                            }
                        }
                        break;
                }
                return ret;
            }

            // 性別
            string _getSex()
            {
                string ret = "1 男";
                if (CoModel.Sex == 2)
                {
                    ret = "2 女";
                }
                return ret;
            }

            // 生年月日
            string _getBirthDay()
            {
                string ret = "";

                int wDate = CIUtil.SDateToWDate(CoModel.BirthDay);
                int gengo = wDate / 1000000;

                switch (gengo)
                {
                    case 1:
                        ret = "1明";
                        break;
                    case 2:
                        ret = "2大";
                        break;
                    case 3:
                        ret = "3昭";
                        break;
                    case 4:
                        ret = "4平";
                        break;
                    case 5:
                        ret = "5令";
                        break;
                }

                ret += string.Format("{0, 2}.{1, 2}.{2, 2}", wDate % 1000000 / 10000, wDate % 10000 / 100, wDate % 100);

                return ret;
            }
            // 職務上の事由
            string _getSyokumuJiyu()
            {
                string ret = "";

                switch (CoModel.SyokumuKbn)
                {
                    case 1:
                        ret = "1 職務上";
                        break;
                    case 2:
                        ret = "2 下船後３月以内";
                        break;
                    case 3:
                        ret = "3 通勤災害";
                        break;
                }

                return ret;
            }
            //公費給付額（かっこ書き）
            string _getKohiKyufu(int index)
            {
                string ret = CIUtil.ToStringIgnoreNull(CoModel.KohiKyufu(index));

                if (ret != "")
                {
                    ret = "(" + string.Format("{0:N0}", CIUtil.StrToIntDef(ret, 0)) + ")";
                }

                return ret;
            }
            //国保減免
            string _getGenmenKbn()
            {
                string ret = "";

                switch (CoModel.GenmenKbn)
                {
                    case 1:
                        if (CoModel.GenmenGaku > 0)
                        {
                            ret = "減額 " + CoModel.GenmenGaku.ToString() + "円";
                        }
                        else
                        {
                            ret = "減額 " + (CoModel.GenmenRate / 10).ToString() + "割";
                        }
                        break;
                    case 2:
                        ret = "免除";
                        break;
                    case 3:
                        ret = "支払猶予";
                        break;
                }

                return ret;
            }

            //チェック
            string _getCheck(string str)
            {
                int[] baisu = new int[] { 2, 3, 4, 5, 6, 7 };
                int index = 0;
                int total = 0;

                for (int i = str.Length - 1; i >= 0; i--)
                {
                    total += CIUtil.StrToIntDef(str[i].ToString(), 0) * baisu[index];
                    index++;
                    if (index >= baisu.Length)
                    {
                        index = 0;
                    }
                }
                total = total % 11;

                if (total <= 1)
                {
                    total = 0;
                }
                else
                {
                    total = 11 - total;
                }

                return total.ToString();
            }
            // 数字のみ抽出
            string _getNums(string str)
            {
                List<string> nums = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string narrow = CIUtil.ToNarrow(str);
                string ret = "";
                for (int i = 1; i <= narrow.Length; i++)
                {
                    if (nums.Contains(CIUtil.Copy(narrow, i, 1)))
                    {
                        ret += CIUtil.Copy(narrow, i, 1);
                    }
                }
                return ret;
            }
            //OCR1
            string _getOCR1()
            {
                string ret = "";

                // 保険者番号
                string hokensyaNo = CoModel.HokensyaNo.PadLeft(8, '0');
                //医療機関コード
                string hpCd = CoModel.HpCd.PadLeft(7, '0');
                //請求点数
                string tensu = CoModel.HokenReceTensu?.ToString().PadLeft(7, '0') ?? string.Empty;
                //チェック1
                string chk1 = _getCheck(tensu);
                //生月日
                string birthDay = (CoModel.BirthDay % 10000).ToString().PadLeft(5, '0');
                //チェック2
                string chk2 = _getCheck(birthDay);
                //一部負担金
                string futan = CoModel.HokenReceFutan?.ToString().PadLeft(5, '0') ?? string.Empty;
                //チェック3
                string chk3 = _getCheck(futan);
                //チェック4
                string chk4 = _getCheck(hokensyaNo + hpCd + tensu + chk1 + birthDay + chk2 + futan + chk3);
                //実日数
                string nissu = CoModel.HokenNissu?.ToString().PadLeft(2, '0') ?? string.Empty;
                //診療年月
                string sinYm = (CIUtil.SDateToWDate(CoModel.SinYm * 100 + 1) % 1000000 / 100).ToString().PadLeft(4, '0');
                //チェック5
                string chk5 = _getCheck(nissu + sinYm);
                //市町村番号
                string sicyo = new string('0', 8);
                //受給者番号
                string jyukyu = new string('0', 7);
                //チェック6
                string chk6 = "0";
                //都道府県番号
                string prefNo = CoModel.PrefNo.ToString().PadLeft(2, '0');
                //点数表
                string hyo = "1";
                //保険種別１
                string receSbt1 = CIUtil.Copy(CoModel.ReceiptSbt, 2, 1);
                //保険種別２
                string receSbt2 = CIUtil.Copy(CoModel.ReceiptSbt, 3, 1);
                //本人家族
                string honka = CIUtil.Copy(CoModel.ReceiptSbt, 4, 1);
                //整理番号
                string seiriNo = "1";
                //チェック7
                string chk7 = _getCheck(prefNo + hyo + receSbt1 + receSbt2 + honka + seiriNo);

                ret = hokensyaNo + hpCd + tensu + chk1 + birthDay + chk2 + futan + chk3 + chk4 + nissu + sinYm + chk5 + sicyo + jyukyu + chk6 + prefNo + hyo + receSbt1 + receSbt2 + honka + seiriNo + chk7;

                return ret;
            }
            //OCR2
            string _getOCR2()
            {
                string ret = "";

                string retA = "";
                string retB = "";

                //性別
                string sex = CoModel.Sex.ToString().PadLeft(1, '0');
                //元号
                string gengo = $"{CIUtil.SDateToWDate(CoModel.BirthDay) / 1000000,0}";
                //生年
                string birthY = (CIUtil.SDateToWDate(CoModel.BirthDay) % 1000000 / 10000).ToString().PadLeft(2, '0');
                //チェック1
                string chk1 = _getCheck(sex + gengo + birthY);
                //記号
                string kigo = _getNums(CoModel.Kigo).PadLeft(10, '0');
                //番号
                string bango = _getNums(CoModel.Bango).PadLeft(10, '0');
                //チェック2
                string chk2 = _getCheck(kigo + bango);

                retA = sex + gengo + birthY + chk1 + kigo + bango + chk2;

                if (CIUtil.Copy(CoModel.ReceiptSbt, 2, 1) == "2" || CIUtil.Copy(CoModel.ReceiptSbt, 3, 1) != "1")
                {
                    //公１負担者番号
                    string k1FutanNo = CoModel.KohiFutansyaNo(1).PadLeft(8, '0');
                    //公１受給者番号
                    string k1JyukyuNo = CoModel.KohiJyukyusyaNo(1).PadLeft(7, '0');
                    //チェック3
                    string chk3 = _getCheck(k1FutanNo + k1JyukyuNo);
                    //公１実日数
                    string k1Nissu = CoModel.KohiNissu(1)?.ToString().PadLeft(2, '0') ?? string.Empty;
                    //公１請求点数
                    string k1Tensu = CoModel.KohiReceTensu(1)?.ToString().PadLeft(7, '0') ?? string.Empty;
                    //チェック4
                    string chk4 = _getCheck(k1Nissu + k1Tensu);
                    //公１薬剤一部負担金
                    string k1Yaku = new string('0', 5);
                    //チェック5
                    string chk5 = "0";
                    //公１患者負担額
                    string k1Futan = CoModel.KohiReceFutan(1)?.ToString().PadLeft(5, '0') ?? string.Empty;
                    //チェック6
                    string chk6 = _getCheck(k1Futan);
                    //チェック7
                    string chk7 = _getCheck(k1FutanNo + k1JyukyuNo + chk3 + k1Nissu + k1Tensu + chk4 + k1Yaku + chk5 + k1Futan + chk6);
                    //予備
                    string yobi = new string(' ', 2);

                    retB = k1FutanNo + k1JyukyuNo + chk3 + k1Nissu + k1Tensu + chk4 + k1Yaku + chk5 + k1Futan + chk6 + chk7 + yobi;
                }
                ret = retA + retB;

                return ret;
            }
            //OCR3
            string _getOCR3()
            {
                string ret = "";

                if ((CIUtil.Copy(CoModel.ReceiptSbt, 2, 1) == "2" && CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= 2) ||
                     (CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= 3))
                {
                    //公２負担者番号
                    string k2FutanNo = CoModel.KohiFutansyaNo(2).PadLeft(8, '0');
                    //公２受給者番号
                    string k2JyukyuNo = CoModel.KohiJyukyusyaNo(2).PadLeft(7, '0');
                    //チェック1
                    string chk1 = _getCheck(k2FutanNo + k2JyukyuNo);
                    //公２実日数
                    string k2Nissu = CoModel.KohiNissu(2)?.ToString().PadLeft(2, '0') ?? string.Empty;
                    //公２請求点数
                    string k2Tensu = CoModel.KohiReceTensu(2)?.ToString().PadLeft(7, '0') ?? string.Empty;
                    //チェック2
                    string chk2 = _getCheck(k2Nissu + k2Tensu);
                    //公２薬剤一部負担金
                    string k2Yaku = new string('0', 5);
                    //チェック3
                    string chk3 = "0";
                    //公２患者負担額
                    string k2Futan = $"{CoModel.KohiReceFutan(2),00000}";
                    //チェック4
                    string chk4 = _getCheck(k2Futan);
                    //チェック5
                    string chk5 = _getCheck(k2FutanNo + k2JyukyuNo + chk1 + k2Nissu + k2Tensu + chk2 + k2Yaku + chk3 + k2Futan + chk4);
                    //予備
                    string yobi = new string(' ', 28);

                    ret = k2FutanNo + k2JyukyuNo + chk1 + k2Nissu + k2Tensu + chk2 + k2Yaku + chk3 + k2Futan + chk4 + chk5 + yobi;
                }

                return ret;
            }

            #endregion

            #region sub print
            // 実日数
            void _printoutJituNissu()
            {
                //実日数（保険）
                SingleData.Add("dfJituNissuHo", CIUtil.ToStringIgnoreNull(CoModel.HokenNissu));
                //実日数（公１）
                if ((CoModel.HokenNissu == null && CoModel.KohiNissu(1) != null) ||
                    (CoModel.HokenNissu ?? 0) != (CoModel.KohiNissu(1) ?? 0))
                {
                    SingleData.Add("dfJituNissuK1", CIUtil.ToStringIgnoreNull(CoModel.KohiNissu(1)));
                }
                //実日数（公２）
                if ((CoModel.KohiNissu(1) == null && CoModel.KohiNissu(2) != null) ||
                    (CoModel.KohiNissu(1) ?? 0) != (CoModel.KohiNissu(2) ?? 0))
                {
                    SingleData.Add("dfJituNissuK2", CIUtil.ToStringIgnoreNull(CoModel.KohiNissu(2)));
                }
            }

            // 特記事項
            void _printoutTokki()
            {
                List<string> tokki = CoModel.TokkiJiko;

                SingleData.Add("dfTokki1", $"{tokki[0]} {tokki[1]}");
                SingleData.Add("dfTokki2", $"{tokki[2]} {tokki[3]}");
                SingleData.Add("dfTokki3", tokki[4]);
            }

            //点数欄印字処理
            void _printoutTenCol()
            {
                // 初診
                SingleData.Add("dfSyosinJikan", CoModel.SyosinJikangai);

                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                            "1200",
                            "1220",
                            "1230",
                            "1240",
                            "1250",
                            "2110",
                            "2310",
                            "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(CoModel.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                            //new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150" },
                            new List<string>{ "1100" },
                            new List<string>{ "1200" },
                            new List<string>{ "1220" },
                            new List<string>{ "1230" },
                            new List<string>{ "1240" },
                            new List<string>{ "1250" },
                            new List<string>{ "1400" },
                            new List<string>{ "1410" },
                            new List<string>{ "1420" },
                            new List<string>{ "1430" },
                            new List<string>{ "2100" },
                            new List<string>{ "2110" },
                            new List<string>{ "2200" },
                            new List<string>{ "2300" },
                            new List<string>{ "2310" },
                            new List<string>{ "2500" },
                            new List<string>{ "2600" },
                            new List<string>{ "3110" },
                            new List<string>{ "3210" },
                            new List<string>{ "3310" },
                            new List<string>{ "4000" },
                            new List<string>{ "5000" },
                            new List<string>{ "6000" },
                            new List<string>{ "7000" },
                            new List<string>{ "8000" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColCount(syukeisaki, onlySI)));
                }
                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                            new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                            new List<string>{ "1200" },
                            new List<string>{ "1220" },
                            new List<string>{ "1230" },
                            new List<string>{ "1240" },
                            new List<string>{ "1250" },
                            new List<string>{ "1300" },
                            new List<string>{ "1400" },
                            new List<string>{ "1410" },
                            new List<string>{ "1420" },
                            new List<string>{ "1430" },
                            new List<string>{ "1440" },
                            new List<string>{ "1450" },
                            new List<string>{ "2100" },
                            new List<string>{ "2110" },
                            new List<string>{ "2200" },
                            new List<string>{ "2300" },
                            new List<string>{ "2310" },
                            new List<string>{ "2500" },
                            new List<string>{ "2600" },
                            new List<string>{ "2700" },
                            new List<string>{ "3110" },
                            new List<string>{ "3210" },
                            new List<string>{ "3310" },
                            new List<string>{ "4000" },
                            new List<string>{ "4010" },
                            new List<string>{ "5000" },
                            new List<string>{ "5010" },
                            new List<string>{ "6000" },
                            new List<string>{ "6010" },
                            new List<string>{ "7000" },
                            new List<string>{ "7010" },
                            new List<string>{ "8000" },
                            new List<string>{ "8010" },
                            new List<string>{ "8020" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTotalTen(syukeisaki)));
                }
                // 公費分点分
                if (CoModel.GetReceiptSbt(2) != 2 && CoModel.GetReceiptSbt(3) > 0 && CoModel.Tensu != (CoModel.KohiReceTensu(1) ?? 0))
                {
                    foreach (List<string> syukeisaki in totalSyukeiSakils)
                    {
                        SingleData.Add("dfKTen_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTotalTenKohi(syukeisaki, 1)));
                    }
                }

                // 初診
                List<(string syukeiSaki, string comment)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                            (ReceSyukeisaki.SyosinJikanGai, "時間外"),
                            (ReceSyukeisaki.SyosinKyujitu, "休日"),
                            (ReceSyukeisaki.SyosinSinya, "深夜")
                    };
                string Syosin = "";

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (CoModel.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        if (Syosin.Contains(syosinSyukeisaki[j].comment) == false)
                        {
                            if (Syosin != "") Syosin += ",";
                            Syosin += syosinSyukeisaki[j].comment;
                        }
                    }
                }

                if (Syosin != "")
                {
                    SingleData.Add("dfSyosin", Syosin);
                }
            }

            void _setValFieldZero(string field, string value)
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    if (value == "0")
                    {
                        SingleData.Add(field + "_0", value);
                    }
                    else
                    {
                        SingleData.Add(field, value);
                    }
                }
            }

            //療養の給付欄
            void _printoutRyoyoKyufu()
            {
                //保険点数
                if (Target == TargetConst.OsakaSyouni && (int)_systemConfRepository.GetSettingValue(94005, 0, HpId) == 1)
                {
                    // 大阪小児喘息レセプト(東大阪タイプ)の場合
                    SingleData.Add($"dfTensuHo", CIUtil.ToStringIgnoreNull(CoModel.KohiTensuReceInf(1)));
                }
                else
                {
                    // 通常の場合
                    SingleData.Add("dfTensuHo", CIUtil.ToStringIgnoreNull(CoModel.HokenReceTensu));
                }

                //保険決定点
                if (Target == TargetConst.OsakaSyouni && (int)_systemConfRepository.GetSettingValue(94005, 0, HpId) == 0)
                {
                    // 大阪小児喘息レセプト(大阪タイプ)の場合
                    SingleData.Add($"dfTensuZensoku", CIUtil.ToStringIgnoreNull(CoModel.KohiTensuReceInf(1)));
                }

                //国保減免
                SingleData.Add("dfKokuhoGenmen", _getGenmenKbn());

                string kohiField4 = "";
                if (CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (CoModel.GetReceiptSbt(2) == 2 && CoModel.GetReceiptSbt(3) >= 3))
                {
                    // 公費が3つ以上のときは、印字するフィールドを変える
                    kohiField4 = "4";
                }

                //保険一部負担額
                SingleData.Add($"dfIchibu{kohiField4}Ho", CIUtil.ToStringIgnoreNull(CoModel.HokenReceFutan));
                //かっこ書き１
                SingleData.Add($"dfKyufu{kohiField4}K1", _getKohiKyufu(1));
                //かっこ書き２
                SingleData.Add($"dfKyufu{kohiField4}K2", _getKohiKyufu(2));

                //公１点数
                if (CoModel.HokenReceTensu != (CoModel.KohiReceTensu(1) ?? 0))
                {
                    _setValFieldZero($"dfTensu{kohiField4}K1", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(1)));
                }

                //小児喘息割合
                if (Target == TargetConst.OsakaSyouni)
                {
                    // 大阪小児喘息レセプトの場合
                    SingleData.Add("dfZensokuWari", CoModel.HokenRate / 10 + "割");
                }

                //公１一部負担額
                if (Target == TargetConst.OsakaSyouni)
                {
                    // 大阪小児喘息レセプトの場合
                    _setValFieldZero($"dfIchibu{kohiField4}K1", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutanReceInf(1)));
                }
                else
                {
                    // 通常
                    _setValFieldZero($"dfIchibu{kohiField4}K1", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(1)));
                }

                //公２点数
                if (CoModel.Tensu != (CoModel.KohiReceTensu(2) ?? 0) ||
                    (CoModel.KohiReceTensu(1) ?? 0) != (CoModel.KohiReceTensu(2) ?? 0))
                {
                    // 総点数と異なる or 一つ上の公費点数と異なる
                    _setValFieldZero($"dfTensu{kohiField4}K2", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(2)));
                }
                //公２一部負担額
                _setValFieldZero($"dfIchibu{kohiField4}K2", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(2)));

                if (CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (CoModel.GetReceiptSbt(2) == 2 && CoModel.GetReceiptSbt(3) >= 3))
                {
                    //公３点数
                    if (CoModel.Tensu != (CoModel.KohiReceTensu(3) ?? 0) ||
                        (CoModel.KohiReceTensu(2) ?? 0) != (CoModel.KohiReceTensu(3) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K3", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(3)));
                    }
                    //公３一部負担額
                    _setValFieldZero("dfIchibu4K3", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(3)));
                    //かっこ書き３
                    SingleData.Add($"dfKyufu{kohiField4}K3", _getKohiKyufu(3));
                    //公４点数
                    if (CoModel.Tensu != (CoModel.KohiReceTensu(4) ?? 0) ||
                        (CoModel.KohiReceTensu(3) ?? 0) != (CoModel.KohiReceTensu(4) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K4", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(4)));
                    }
                    //公４一部負担額
                    _setValFieldZero("dfIchibu4K4", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(4)));
                    //かっこ書き４
                    SingleData.Add($"dfKyufu{kohiField4}K4", _getKohiKyufu(4));
                }
            }
            //療養の給付欄（神奈川レセプト２枚目用）
            void _printoutRyoyoKyufuForKanagawa2()
            {
                // 神奈川レセプト２枚目

                int kanagawaKohiIndex = 0;
                string kanagawaKohiHoubetu = "";
                int kanagawaKohiFutanRate = 0;

                for (int i = 1; i <= 4; i++)
                {
                    if (new List<string> { "80", "81", "85", "88", "89" }.Contains(CoModel.KohiHoubetu(i)))
                    {
                        kanagawaKohiIndex = i;
                        kanagawaKohiHoubetu = CoModel.KohiHoubetu(i);
                        kanagawaKohiFutanRate = CoModel.KohiRate(i);

                        break;
                    }
                }

                //保険点数
                if (new List<string> { "88", "89" }.Contains(kanagawaKohiHoubetu) &&
                    CoModel.KohiReceTensu(kanagawaKohiIndex) != CoModel.HokenReceTensu)
                {
                    // 88, 89で点数が保険分と異なる場合
                    SingleData.Add("dfTensuHoTorikesi", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(kanagawaKohiIndex)));
                    SingleData.Add("dfTensuHoKohi", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(kanagawaKohiIndex)));
                }
                else
                {
                    SingleData.Add("dfTensuHo", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(kanagawaKohiIndex)));
                }

                //保険一部負担額
                if (CoModel.KohiKyufu(kanagawaKohiIndex) > 0)
                {
                    SingleData.Add("dfIchibuHo", CIUtil.ToStringIgnoreNull(CoModel.KohiKyufu(kanagawaKohiIndex)));
                }
                else
                {
                    SingleData.Add("dfIchibuHo", CIUtil.ToStringIgnoreNull(CoModel.HokenReceFutan));
                }

                //かっこ書き１
                SingleData.Add("dfKyufuK1", _getKohiKyufu(1));
                //かっこ書き２
                SingleData.Add("dfKyufuK2", _getKohiKyufu(2));
                //国保減免
                SingleData.Add("dfKokuhoGenmen", _getGenmenKbn());

                string kohiField4 = "";
                if (CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (CoModel.GetReceiptSbt(2) == 2 && CoModel.GetReceiptSbt(3) >= 3))
                {
                    // 公費が3つ以上のときは、印字するフィールドを変える
                    kohiField4 = "4";
                }

                //公１点数
                if (CoModel.Tensu != (CoModel.KohiReceTensu(1) ?? 0))
                {
                    _setValFieldZero($"dfTensu{kohiField4}K1", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(1)));
                }
                //小児喘息割合

                //公１一部負担額
                if (kanagawaKohiIndex == 1 && kanagawaKohiFutanRate == 100)
                {
                    // 公費の負担率が100%の場合は印字しない
                }
                else
                {
                    _setValFieldZero($"dfIchibu{kohiField4}K1", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(1)));
                }

                //公２点数
                if (CoModel.Tensu != (CoModel.KohiReceTensu(2) ?? 0) ||
                    (CoModel.KohiReceTensu(1) ?? 0) != (CoModel.KohiReceTensu(2) ?? 0))
                {
                    // 総点数と異なる or 一つ上の公費点数と異なる
                    _setValFieldZero($"dfTensu{kohiField4}K2", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(2)));
                }
                //公２一部負担額
                if (kanagawaKohiIndex == 2 && kanagawaKohiFutanRate == 100)
                {
                    // 公費の負担率が100%の場合は印字しない
                }
                else
                {
                    _setValFieldZero($"dfIchibu{kohiField4}K2", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(2)));
                }

                if (CIUtil.StrToIntDef(CIUtil.Copy(CoModel.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (CoModel.GetReceiptSbt(2) == 2 && CoModel.GetReceiptSbt(3) >= 3))
                {
                    //公３点数
                    if (CoModel.Tensu != (CoModel.KohiReceTensu(3) ?? 0) ||
                        (CoModel.KohiReceTensu(2) ?? 0) != (CoModel.KohiReceTensu(3) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K3", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(3)));
                    }
                    //公３一部負担額
                    if (kanagawaKohiIndex == 3 && kanagawaKohiFutanRate == 100)
                    {
                        // 公費の負担率が100%の場合は印字しない
                    }
                    else
                    {
                        _setValFieldZero("dfIchibu4K3", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(3)));
                    }
                    //公４点数
                    if (CoModel.Tensu != (CoModel.KohiReceTensu(4) ?? 0) ||
                        (CoModel.KohiReceTensu(3) ?? 0) != (CoModel.KohiReceTensu(4) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K4", CIUtil.ToStringIgnoreNull(CoModel.KohiReceTensu(4)));
                    }
                    //公４一部負担額
                    if (kanagawaKohiIndex == 4 && kanagawaKohiFutanRate == 100)
                    {
                        // 公費の負担率が100%の場合は印字しない
                    }
                    else
                    {
                        _setValFieldZero("dfIchibu4K4", CIUtil.ToStringIgnoreNull(CoModel.KohiReceFutan(4)));
                    }
                }
            }

            //OCR印字処理
            void _printoutOCR()
            {
                SingleData.Add("dfOCR1", _getOCR1());
                SingleData.Add("dfOCR2", _getOCR2());
                SingleData.Add("dfOCR3", _getOCR3());
            }

            // 都道府県別特殊処理
            void _tokusyu()
            {
                if (CoModel.PrefNo == PrefCode.Iwate)
                {
                    #region 岩手県

                    if (kohiDatas.Any())
                    {
                        string fukusi = "";
                        string tokusyuNo = "";

                        for (int i = 1; i <= 4; i++)
                        {
                            if (CoModel.KohiHokenIdAll(i) > 0)
                            {
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == CoModel.KohiHokenIdAll(i));

                                if (kohiData.Any())
                                {
                                    if (new int[] { 110, 111 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "乳";
                                    }
                                    else if (new int[] { 120, 121 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "妊";
                                    }
                                    else if (new int[] { 130, 131 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "重";
                                    }
                                    else if (new int[] { 140, 141 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "母";
                                    }
                                    else if (new int[] { 150, 151, 160, 161 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "寡";
                                    }
                                    else if (new int[] { 170, 171 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "父";
                                    }
                                    else if (new int[] { 181 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "中";
                                    }
                                    else if (new int[] { 191 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "小";
                                    }

                                    if (fukusi != "")
                                    {
                                        // 福祉公費の場合、特殊番号を控えておく（レセプト２枚目の場合に印字）
                                        tokusyuNo = kohiData.First().TokusyuNo;
                                        break;
                                    }
                                }
                            }
                        }

                        if (fukusi != "")
                        {
                            SingleData.Add("dfFukusiKigo1", fukusi);

                            if (Target == TargetConst.IwateRece2)
                            {
                                if (tokusyuNo != "")
                                {
                                    SingleData.Add("dfFutanNoFukusi", Copy(tokusyuNo, 1, 8).PadRight(8, ' '));
                                    if (tokusyuNo.Length > 8)
                                    {
                                        SingleData.Add("dfJyukyuNoFukusi", Copy(tokusyuNo, 9, 7).PadRight(7, ' '));
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (CoModel.PrefNo == PrefCode.Tokyo)
                {
                    #region 東京都

                    // 生保交付番号
                    for (int i = 1; i <= 4; i++)
                    {
                        if (CoModel.KohiHoubetu(i) == "12" || CoModel.KohiHoubetu(i) == "25")
                        {
                            SingleData.Add("dfKofu", CoModel.KohiTokusyuNo(i));
                            break;
                        }
                    }
                    # endregion
                }
                else if (CoModel.PrefNo == PrefCode.Nagano)
                {
                    #region 長野県
                    if (Target == TargetConst.NaganoRece2)
                    {
                        // 長野県レセプト2枚目
                        for (int i = 1; i <= 4; i++)
                        {
                            if (CoModel.KohiHokenIdAll(i) > 0)
                            {
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == CoModel.KohiHokenIdAll(i));

                                if (kohiData.Any() && kohiData.First().TokusyuNo != "")
                                {
                                    // 福祉番号
                                    SingleData.Add("dfFukusiNo", kohiData.First().TokusyuNo);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (CoModel.PrefNo == PrefCode.Shiga)
                {
                    #region 滋賀県
                    SingleData.Add("df25KogakuKbn", CoModel.KogakuKbnMessage);
                    #endregion
                }
                else if (CoModel.PrefNo == PrefCode.Osaka)
                {
                    #region 大阪府
                    if (Target == TargetConst.OsakaSyouni)
                    {
                        // 大阪小児喘息
                        for (int i = 1; i <= 4; i++)
                        {
                            if (CoModel.KohiHokenIdAll(i) > 0 && new int[] { 198, 298 }.Contains(CoModel.KohiHokenIdAll(i)))
                            {
                                // 小児喘息公費
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == CoModel.KohiHokenIdAll(i));

                                if (kohiData.Any() && kohiData.First().TokusyuNo != "")
                                {
                                    // 福祉番号
                                    if ((int)_systemConfRepository.GetSettingValue(94005, 0, HpId) == 0)
                                    {
                                        // 大阪市タイプ
                                        if (kohiData.First().TokusyuNo.Length == 8)
                                        {
                                            SingleData.Add("dfFutanNoFukusi", kohiData.First().TokusyuNo);
                                        }
                                    }
                                    else
                                    {
                                        // 東大阪市タイプ
                                        SingleData.Add("dfSyoniZensoku", kohiData.First().TokusyuNo);
                                    }

                                    break;
                                }
                            }
                        }

                        SingleData.Add("dfFukusiKigo2", "（ゼ）");
                    }
                    #endregion
                }
                else if (CoModel.PrefNo == PrefCode.Nara)
                {
                    // 奈良県
                    if (CoModel.HokenKbn == 1)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            if (new List<string> { "41", "71", "81", "91" }.Contains(CoModel.KohiHoubetuAll(i)))
                            {
                                SingleData.Add("dfFukusiKigo1", "奈福");
                            }
                        }
                    }
                }
                else if (CoModel.PrefNo == PrefCode.Fukuoka)
                {
                    #region 福岡
                    if (CoModel.HokenKbn == 1)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            {
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == CoModel.KohiHokenIdAll(i));

                                if (kohiData.Any())
                                {
                                    List<(int hokenNo, string kigo)> fukuokaFukusi =
                                        new List<(int hokenNo, string kigo)>
                                        {
                                            ( 180, "障" ),
                                            ( 181, "乳" ),
                                            ( 190, "親" )
                                        };

                                    foreach ((int hokenNo, string kigo) in fukuokaFukusi)
                                    {
                                        string fukusiKigo = "";

                                        if (kohiData.First().HokenNo == hokenNo)
                                        {
                                            if (new int[] { 2, 3, 5 }.Contains(kohiData.First().ReceSeikyuKbn))
                                            {
                                                // 社保単独の設定の場合
                                                fukusiKigo = kigo;
                                            }
                                            int kyufuGai = 0;
                                            if (CoModel.PtFutan > 0)
                                            {
                                                kyufuGai =
                                                    CoModel.TotalIryoHi - CoModel.HokenFutan10en - CoModel.KogakuFutan10en - (CoModel.KohiFutan10enReceInf(i) ?? 0);
                                            }
                                            _printoutFukuokaFukusi(fukusiKigo, kohiData.First().FutansyaNo, kohiData.First().JyukyusyaNo, kyufuGai);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            // 福岡福祉記号印字処理
            void _printoutFukuokaFukusi(string fukusiKigo, string futansyaNo, string jyukyusyaNo, int kyufuGai)
            {
                if (fukusiKigo != "")
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        SingleData.Add($"dfFukusiKigo{i}", fukusiKigo);
                        SingleData.Add($"dfFukusiKigoMaru{i}", "〇");
                    }
                }

                if (Target == TargetConst.FukuokaRece2)
                {
                    SingleData.Add("dfFukuokaFukusiFutanNoCaption1", "乳・障・親");
                    SingleData.Add("dfFukuokaFukusiFutanNoCaption2", "負担者番号");
                    SingleData.Add("dfFukuokaFukusiFutanNo", futansyaNo);
                    SingleData.Add("dfFukuokaFukusiJyukyuNoCaption1", "乳・障・親");
                    SingleData.Add("dfFukuokaFukusiJyukyuNoCaption2", "受給者番号");
                    SingleData.Add("dfFukuokaFukusiJyukyuNo", jyukyusyaNo);
                    SingleData.Add("dfFukuokaFukusiKyufuGaiCaption1", "乳・障・親");
                    SingleData.Add("dfFukuokaFukusiKyufuGaiCaption2", "給付外の額");
                    SingleData.Add("dfFukuokaFukusiKyufuGai", kyufuGai.ToString() + "円");
                }
            }

            #endregion


            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 患者番号 + 法別番号
            SingleData.Add("dfPtNo", _getPtNo());

            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{CoModel.PtNum:D9}");

            // 帳票番号
            if (!(new int[] { TargetConst.KanagawaRece2 }.Contains(Target)))
            {
                SingleData.Add("dfPrintNo", getPrintNo());
            }

            // 社保国保
            SingleData.Add("dfSyaKoku", _getSyaKoku());

            // 診療年月
            SingleData.Add("dfSinYM", _getSinYM());

            // 医療機関コード
            SingleData.Add("dfHpNo", CIUtil.FormatHpCd(CoModel.HpCd, CoModel.PrefNo));

            // レセ種別１
            SingleData.Add("dfReceSbt1", _getReceSbt1());

            // レセ種別２
            SingleData.Add("dfReceSbt2", _getReceSbt2());

            // レセ種別３
            SingleData.Add("dfReceSbt3", _getReceSbt3());

            //漢字氏名
            SingleData.Add("dfPtKanjiName", CoModel.PtName);

            // 保険者番号
            SingleData.Add("dfHokensyaNo", string.Format("{0, 8}", CoModel.HokensyaNo));

            // 記号
            SingleData.Add("dfKigo", CoModel.Kigo);

            // 番号
            SingleData.Add("dfBango", CoModel.Bango);

            // 枝番
            SingleData.Add("dfEdaNo", CoModel.EdaNo);
            #endregion

            if (CurrentPage == 1 || CIUtil.Copy(CoModel.ReceiptSbt, 2, 1) == "2")
            {
                // 公費医療の場合は、続紙にも公１の負担者番号・受給者番号を印字

                //公１負担者番号
                SingleData.Add("dfFutanNoK1", string.Format("{0, 8}", CoModel.KohiFutansyaNo(1)));
                //公１受給者番号
                SingleData.Add("dfJyukyuNoK1", string.Format("{0, 7}", CoModel.KohiJyukyusyaNo(1)));
            }

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                //公２負担者番号
                SingleData.Add("dfFutanNoK2", string.Format("{0, 8}", CoModel.KohiFutansyaNo(2)));
                //公２受給者番号
                SingleData.Add("dfJyukyuNoK2", string.Format("{0, 7}", CoModel.KohiJyukyusyaNo(2)));

                // 負担率
                if (CoModel.HokenKbn == 2 && CoModel.GetReceiptSbt(2) != 3)
                {
                    SingleData.Add("dfKyufuWari", ((100 - CoModel.HokenRate) / 10).ToString() + "割");
                }

                // 県番号
                SingleData.Add("dfPrefNo", CoModel.PrefNo.ToString());

                //カナ氏名
                SingleData.Add("dfPtKanaName", CoModel.PtKanaName);

                //性別
                SingleData.Add("dfSex", _getSex());

                //生年月日
                SingleData.Add("dfBirthDay", _getBirthDay());

                //職務上の事由
                SingleData.Add("dfSyokumuJiyu", _getSyokumuJiyu());

                //特記事項
                _printoutTokki();

                //医療機関住所
                SingleData.Add("txHpAddress", CoModel.HpAddress);

                //医療機関名
                SingleData.Add("dfHpName", CoModel.ReceHpName);

                //医療機関電話番号
                SingleData.Add("dfHpTel", CoModel.HpTel);

                //病名欄
                foreach (var item in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(item.Byomei));
                    data.Add("lsByomeiStart", new CellModel(item.StartDate));
                    data.Add("lsByomeiTenki", new CellModel(item.Tenki));
                    CellData.Add(data);
                }

                // 実日数
                _printoutJituNissu();

                //点数欄
                if (!(new int[]
                    {
                        TargetConst.KanagawaRece2,
                        TargetConst.FukuokaRece2,
                        TargetConst.SagaRece2,
                        TargetConst.MiyazakiRece2
                    }.Contains(Target)))
                {
                    _printoutTenCol();
                }

                // 療養の給付欄
                if (Target == TargetConst.KanagawaRece2)
                {
                    _printoutRyoyoKyufuForKanagawa2();
                }
                else
                {
                    _printoutRyoyoKyufu();
                }

                //OCR
                if (!(new int[]
                    {
                        TargetConst.KanagawaRece2,
                        TargetConst.OsakaSyouni,
                        TargetConst.FukuokaRece2,
                        TargetConst.MiyazakiRece2
                    }.Contains(Target)))
                {
                    _printoutOCR();
                }

                // 都道府県別処理
                _tokusyu();


                #endregion
            }
            else if (CurrentPage >= 2)
            {
                #region 本紙以外
                // Page
                SingleData.Add("dfPage", $"P.{CurrentPage}");
                #endregion
            }
        }

        private void PrintReceiptHeaderForRousai()
        {
            #region Sub Function
            // 年月を労災レセプト様式に応じた書式に変換する
            string _getYm(int Ym)
            {
                string ret = "";

                if ((int)_systemConfRepository.GetSettingValue(94002, 0, HpId) == 1)
                {
                    ret = CIUtil.SDateToWDateForRousai(Ym).ToString();
                }
                else
                {
                    // 旧様式は元号なし
                    ret = $"{(CIUtil.SDateToWDateForRousai(Ym) % 1000000):D6}";
                }
                return ret;
            }
            // 傷病開始日
            string _getSyobyoDate()
            {
                return _getYm(CoModel.RousaiSyobyoDate);
            }
            // 療養開始日
            string _getRyoyoStartDate()
            {
                return _getYm(CoModel.RyoyoStartDate);
            }
            // 療養開始日
            string _getRyoyoEndDate()
            {
                return _getYm(CoModel.RyoyoEndDate);
            }

            // 実日数
            string _getJituNissu()
            {
                string ret = CIUtil.ToStringIgnoreNull(CoModel.RousaiJituNissu);
                if (CoModel.RousaiJituNissu != null)
                {
                    if (CoModel.RousaiJituNissu == 0)
                    {
                        ret = "999";
                    }
                }
                return ret;
            }

            // 年齢
            string _getAge()
            {
                string ret = CIUtil.SDateToAge(CoModel.BirthDay, CIUtil.GetLastDateOfMonth(CoModel.SinYm * 100 + 1)).ToString();
                return ret;
            }
            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 医療機関コード
            SingleData.Add("dfHpNo", CIUtil.FormatHpCd(CoModel.RousaiHpCd, 0));
            //医療機関名
            SingleData.Add("dfHpName", CoModel.ReceHpName);
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{CoModel.PtNum:D9}");
            // 漢字氏名
            SingleData.Add("dfPtKanjiName", CoModel.PtName);
            // 年齢
            SingleData.Add("dfAge", _getAge());
            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", CoModel.PtNum.ToString());
                // 請求回数
                SingleData.Add("dfReceCount", CoModel.RousaiReceCount.ToString());
                //新継再別
                SingleData.Add("dfSinkei", CoModel.RousaiSinkei.ToString());

                //転帰
                SingleData.Add("dfTenki", CoModel.RousaiTenki.ToString());

                //労災交付番号
                SingleData.Add("dfRousaiKofuNo", CoModel.RousaiKofu);

                //生年月日
                int rousaiBirthDay = CIUtil.SDateToWDateForRousai(CoModel.BirthDay);
                SingleData.Add("dfBirthDay", rousaiBirthDay.ToString());

                string gengo = "";

                if (rousaiBirthDay > 0)
                {
                    switch (rousaiBirthDay / 1000000)
                    {
                        case 1: gengo = "M"; break;
                        case 3: gengo = "T"; break;
                        case 5: gengo = "S"; break;
                        case 7: gengo = "H"; break;
                        case 9: gengo = "R"; break;
                    }

                    SingleData.Add("dfBirthGengo" + gengo, "〇");

                    SingleData.Add("dfBirthY", (rousaiBirthDay / 10000 % 100).ToString());
                    SingleData.Add("dfBirthM", (rousaiBirthDay / 100 % 100).ToString());
                    SingleData.Add("dfBirthD", (rousaiBirthDay % 100).ToString());
                }

                //傷病開始日
                SingleData.Add("dfRousaiSyobyoDate", _getSyobyoDate());

                //療養開始日
                SingleData.Add("dfRyoyoStartDate", _getRyoyoStartDate());

                //療養終了日
                SingleData.Add("dfRyoyoEndDate", _getRyoyoEndDate());

                //実日数
                SingleData.Add("dfNissu", _getJituNissu().PadLeft(3, ' '));

                //合計金額
                SingleData.Add("dfTotal", $"{CoModel.RousaiTotal.ToString(),7}");

                //事業所名
                SingleData.Add("txJigyosyoName", CoModel.JigyosyoName);
                //事業所所在地（都道府県）
                SingleData.Add("dfPrefName", CoModel.RousaiPrefName);

                //事業所所在地（市町村）
                SingleData.Add("dfCityName", CoModel.RousaiCityName);

                //傷病の経過
                SingleData.Add("txSyobyoKeika", CoModel.SyobyoKeika);

                //小計
                SingleData.Add("dfSyokei", CoModel.RousaiSyokei?.ToString() ?? string.Empty);
                //イ
                SingleData.Add("dfTenTotal", CoModel.RousaiSyokeiGaku_I?.ToString() ?? string.Empty);
                //ロ
                SingleData.Add("dfEnTotal", CoModel.RousaiSyokeiGaku_RO?.ToString() ?? string.Empty);
                //病名欄
                short i = 0;
                foreach (CoReceiptByomeiModel byomei in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(byomei.Byomei));
                    CellData.Add(data);
                    i++;
                }

                //点数欄
                // 点数
                List<List<string>> tensuSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{"1200" },
                        new List<string>{"1220", "1221" },
                        new List<string>{"1230" },
                        new List<string>{"1240" },
                        new List<string>{"1250" },
                        new List<string>{"2110" },
                        new List<string>{"2310" },
                        new List<string>{"2500" }
                    };
                foreach (List<string> syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150" },
                        new List<string>{ "1200" },
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1400" },
                        new List<string>{ "1410" },
                        new List<string>{ "1420" },
                        new List<string>{ "1430" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "5000" },
                        new List<string>{ "6000" },
                        new List<string>{ "7000" },
                        new List<string>{ "8000" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColCount(syukeisaki, onlySI)));
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200" },
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "1400" },
                        new List<string>{ "1410" },
                        new List<string>{ "1420" },
                        new List<string>{ "1430" },
                        new List<string>{ "1440" },
                        new List<string>{ "1450" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTotalTen(syukeisaki)));
                }

                // その他金額
                List<(int count, double kingaku)> tencolKingakuSonotas = CoModel.TenColKingakuSonota("A180");

                int sonotaIndex = 1;
                foreach ((int count, double kingaku) tencolKingakuSonota in tencolKingakuSonotas)
                {
                    SingleData.Add($"dfCount_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.count) + " 回");
                    SingleData.Add($"dfTotal_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.kingaku));

                    sonotaIndex++;
                    if (sonotaIndex > 4) break;
                }

                tencolKingakuSonotas = CoModel.TenColKingakuSonota("A131");

                foreach ((int count, double kingaku) tencolKingakuSonota in tencolKingakuSonotas)
                {
                    SingleData.Add($"dfCount_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.count) + " 回");
                    SingleData.Add($"dfTotal_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.kingaku));

                    sonotaIndex++;
                    if (sonotaIndex > 4) break;
                }

                // 初診
                List<(string syukeiSaki, string field)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                        (ReceSyukeisaki.SyosinJikanGai, "dfSyosinJikanGai"),
                        (ReceSyukeisaki.SyosinKyujitu, "dfSyosinKyujitu"),
                        (ReceSyukeisaki.SyosinSinya, "dfSyosinSinya")
                    };

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (CoModel.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        SingleData.Add(syosinSyukeisaki[j].field, "〇");
                    }
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                if (Target == TargetConst.RousaiTanki)
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiRoudouNo", CoModel.RousaiKofu);
                }
                else
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiNenkinNo", CoModel.RousaiKofu);
                }
                #endregion
            }
        }

        private void PrintReceiptHeaderForAfter()
        {
            #region Sub Function

            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            //医療機関名
            SingleData.Add("dfHpName", CoModel.ReceHpName);
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{CoModel.PtNum:D9}");
            //漢字氏名
            SingleData.Add("dfPtKanjiName", CoModel.PtName);
            //労災交付番号
            SingleData.Add("dfRousaiKofuNo", CoModel.RousaiKofu);

            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", CoModel.PtNum.ToString());

                // 診療年月日
                SingleData.Add("dfSinDate", CIUtil.SDateToWDateForRousai(CoModel.SinDate).ToString());

                // 検査日
                if (CoModel.KensaDate > 0)
                {
                    SingleData.Add("dfKensaDate", CIUtil.SDateToWDateForRousai(CoModel.KensaDate).ToString());
                }
                // 傷病名コード
                SingleData.Add("dfSyobyoCd", CoModel.SyobyoCd);

                // 前回検査日
                if (CoModel.ZenkaiKensaDate > 0)
                {
                    WarekiYmd wareki = CIUtil.SDateToShowWDate3(CoModel.ZenkaiKensaDate);

                    SingleData.Add("dfZenkaiKensaYear", wareki.Gengo + wareki.Year);
                    SingleData.Add("dfZenkaiKensaMonth", wareki.Month.ToString());
                    SingleData.Add("dfZenkaiKensaDay", wareki.Day.ToString());
                }

                //傷病の経過
                SingleData.Add("txSyobyoKeika", CoModel.SyobyoKeika);
                //合計金額
                SingleData.Add("dfTotal", $"{CoModel.AfterTotal.ToString(),7}");
                //小計
                SingleData.Add("dfSyokei", CoModel.AfterSyokei.ToString());
                //イ
                SingleData.Add("dfTenTotal", CoModel.AfterSyokeiGaku_I.ToString());
                //ロ
                SingleData.Add("dfEnTotal", CoModel.AfterSyokeiGaku_RO.ToString());

                //点数欄
                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                        "2110",
                        "2310",
                        "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(CoModel.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200" },
                        new List<string>{ "1300" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "5000" },
                        new List<string>{ "6000" },
                        new List<string>{ "7000" },
                        new List<string>{ "8000" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    if (syukeisaki[0] == "1100")
                    {
                        bool onlySI =
                            (syukeisaki.Any(p =>
                                new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                        int count = CoModel.TenColCount(syukeisaki, onlySI);
                        if (count == 0 && CoModel.TenColTotalTen(syukeisaki) > 0)
                        {
                            // 初診の加算の場合、TENCOL_COUNT=0になってしまうことがある
                            // その場合、初診の回数を入れる
                            count = CoModel.TenColCount(new List<string> { "A110" }, false);
                        }
                        SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(count));
                    }
                    else
                    {
                        bool onlySI =
                            (syukeisaki.Any(p =>
                                new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                        SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColCount(syukeisaki, onlySI)));
                    }
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200", "1220", "1221", "1230", "1240", "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTotalTen(syukeisaki)));
                }

                // 初診
                List<string> syosinSyukeisaki =
                    new List<string>
                    {
                        ReceSyukeisaki.SyosinJikanGai,
                        ReceSyukeisaki.SyosinKyujitu,
                        ReceSyukeisaki.SyosinSinya
                    };

                if (CoModel.TenColTensuSum(syosinSyukeisaki) > 0)
                {
                    SingleData.Add("dfSyosin1", "〇");
                }

                if (CoModel.TenColTensuSum(new List<string> { "1100", "1110", "1120", "1130", "1140", "1150", "1189" }) > 0)
                {
                    WarekiYmd wareki = CIUtil.SDateToShowWDate3(CoModel.SinDate);

                    SingleData.Add("dfSyosinYear", wareki.Gengo + wareki.Year);
                    SingleData.Add("dfSyosinMonth", wareki.Month.ToString());
                    SingleData.Add("dfSyosinDay", wareki.Day.ToString());
                }

                //再診
                List<string> saisinSyukeisaki =
                    new List<string>
                    {
                        ReceSyukeisaki.SaisinJikangai,
                        ReceSyukeisaki.SaisinKyujitu,
                        ReceSyukeisaki.SaisinSinya
                    };

                if (CoModel.TenColTensuSum(syosinSyukeisaki) > 0)
                {
                    SingleData.Add("dfSaisin1", "〇");
                }

                if (CoModel.TenColTensuSum(new List<string> { "1200", "1220", "1221", "1230", "1240", "1250" }) > 0)
                {
                    WarekiYmd wareki = CIUtil.SDateToShowWDate3(CoModel.SinDate);

                    SingleData.Add("dfSaisinYear", wareki.Gengo + wareki.Year);
                    SingleData.Add("dfSaisinMonth", wareki.Month.ToString());
                    SingleData.Add("dfSaisinDay", wareki.Day.ToString());
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                // 医療機関コード
                SingleData.Add("dfHpNo", CIUtil.FormatHpCd(CoModel.RousaiHpCd, 0));
                #endregion
            }

        }

        private void PrintReceiptHeaderForJibaiKenpo()
        {
            #region Sub Function
            // 性別
            string _getSex()
            {
                string ret = "男";
                if (CoModel.Sex == 2)
                {
                    ret = "女";
                }
                return ret;
            }
            // 年齢
            string _getAge()
            {
                string ret = CIUtil.SDateToAge(CoModel.BirthDay, CIUtil.GetLastDateOfMonth(CoModel.SinYm * 100 + 1)).ToString();
                return ret;
            }
            // 初診
            string _getSyosin()
            {
                List<(string syukeiSaki, string comment)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                                            (ReceSyukeisaki.SyosinJikanGai, "時間外"),
                                            (ReceSyukeisaki.SyosinKyujitu, "休日"),
                                            (ReceSyukeisaki.SyosinSinya, "深夜")
                    };
                string Syosin = "";

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (CoModel.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        if (Syosin.Contains(syosinSyukeisaki[j].comment) == false)
                        {
                            if (Syosin != "") Syosin += ",";
                            Syosin += syosinSyukeisaki[j].comment;
                        }
                    }
                }

                return Syosin;
            }
            // 西暦(yyyymmdd）を和暦に変換して各フィールドに印字する
            void _printoutWarekiField(int date, string field, bool gengo, bool year, bool month, bool day)
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);
                if (wareki.Ymd != "")
                {
                    if (gengo) SingleData.Add($"{field}Gengo", wareki.Gengo);
                    if (year) SingleData.Add($"{field}Year", wareki.Year.ToString());
                    if (month) SingleData.Add($"{field}Month", wareki.Month.ToString());
                    if (day) SingleData.Add($"{field}Day", wareki.Day.ToString());
                }
            }
            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{CoModel.PtNum:D9}");
            //漢字氏名
            SingleData.Add("dfPtKanjiName", CoModel.PtName);
            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", CoModel.PtNum.ToString());

                // 診療年月
                _printoutWarekiField(CoModel.SinYm * 100 + 1, "dfSin", true, true, true, false);
                //生年
                _printoutWarekiField(CoModel.BirthDay, "dfBirth", true, true, false, false);
                //性別
                SingleData.Add("dfSex", _getSex());
                //年齢
                SingleData.Add("dfAge", _getAge());
                //受傷日
                _printoutWarekiField(CoModel.JibaiJyusyouDate, "dfJyusyou", true, true, true, true);
                //初診日
                _printoutWarekiField(CoModel.JibaiSyosinDate, "dfSyosin", true, true, true, true);
                //診療期間自
                _printoutWarekiField(CoModel.RyoyoStartDate, "dfSinryoStart", true, true, true, true);
                //診療期間至
                _printoutWarekiField(CoModel.RyoyoEndDate, "dfSinryoEnd", true, true, true, true);

                //実日数
                SingleData.Add("dfNissu", CIUtil.ToStringIgnoreNull(CoModel.RousaiJituNissu));
                //転帰
                switch (CoModel.RousaiTenki)
                {
                    case 1:
                        SingleData.Add("dfTenkiTiyu", "〇");
                        break;
                    case 3:
                        SingleData.Add("dfTenkiKeizoku", "〇");
                        break;
                    case 5:
                        SingleData.Add("dfTenkiTeni", "〇");
                        break;
                    case 7:
                        SingleData.Add("dfTenkiTyusi", "〇");
                        break;
                    case 9:
                        SingleData.Add("dfTenkiSibo", "〇");
                        break;
                }
                // 初診
                SingleData.Add("dfSyosin", _getSyosin());
                // 円点レート
                SingleData.Add("dfEnTen", CoModel.EnTen.ToString());

                //ニ（その他）
                SingleData.Add("dfTotal_NI", CIUtil.ToStringIgnoreZero(CoModel.JibaiNiFutan));
                //ホ（診断書料）
                SingleData.Add("dfTotal_HO", CIUtil.ToStringIgnoreZero(CoModel.JibaiHoSindan));
                //ヘ（明細書料）
                SingleData.Add("dfTotal_HE", CIUtil.ToStringIgnoreZero(CoModel.JibaiHeMeisai));

                // 診断書枚数
                SingleData.Add("dfSindanCount", CIUtil.ToStringIgnoreZero(CoModel.JibaiHoSindanCount));
                // 明細書枚数
                SingleData.Add("dfMeisaiCount", CIUtil.ToStringIgnoreZero(CoModel.JibaiHeMeisaiCount));

                //合計D
                SingleData.Add("dfTotal_D", CoModel.JibaiDFutan.ToString());

                //点数合計
                //SingleData.Add("dfTotal_Ten", CoModel.HokenReceTensu);
                SingleData.Add("dfTotal_Ten", CoModel.JibaiKenpoTensu.ToString());
                //点数x円点レート
                SingleData.Add("dfTotal_EN", CoModel.JibaiKenpoFutan.ToString());

                //総合計
                SingleData.Add("dfTotal_ABCD1", (CoModel.JibaiKenpoFutan + CoModel.JibaiDFutan).ToString());
                SingleData.Add("dfTotal_ABCD2", (CoModel.JibaiKenpoFutan + CoModel.JibaiDFutan).ToString());

                //通院月
                SingleData.Add("dfTuinMonth", (CoModel.SinYm % 100).ToString());
                //通院日
                foreach (int tuuinDay in CoModel.TuuinDays)
                {
                    SingleData.Add($"dfDay{tuuinDay % 100}", "〇");
                }
                SingleData.Add("dfDayTotal", CoModel.TuuinDays.Count().ToString());

                //保険会社名
                SingleData.Add("dfHokenName", CoModel.JibaiHokenName);

                //医療機関住所
                SingleData.Add("txHpAddress", CoModel.HpAddress);
                //医療機関名
                SingleData.Add("dfHpName", CoModel.ReceHpName);
                //医師名
                SingleData.Add("dfKaisetuName", CoModel.KaisetuName);
                //医療機関電話番号
                SingleData.Add("dfHpTel", CoModel.HpTel);

                //病名欄

                foreach (CoReceiptByomeiModel byomei in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(byomei.Byomei));
                    CellData.Add(data);
                }

                //点数欄
                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                        "2110",
                        "2310",
                        "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(CoModel.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                            new List<string>{ "1200" },
                            new List<string>{ "1220" },
                            new List<string>{ "1230" },
                            new List<string>{ "1240" },
                            new List<string>{ "1250" },
                            new List<string>{ "2100" },
                            new List<string>{ "2110" },
                            new List<string>{ "2200" },
                            new List<string>{ "2300" },
                            new List<string>{ "2310" },
                            new List<string>{ "2500" },
                            new List<string>{ "2600" },
                            new List<string>{ "3110" },
                            new List<string>{ "3210" },
                            new List<string>{ "3310" },
                            new List<string>{ "4000" },
                            new List<string>{ "5000" },
                            new List<string>{ "6000" },
                            new List<string>{ "7000" },
                            new List<string>{ "8000" },
                            new List<string>{ "8010" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColCount(syukeisaki, onlySI)));
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200" },
                        new List<string>{ "1220" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "1900" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" }
                    };

                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    double ten;
                    ten = CoModel.TenColTotalTen(syukeisaki);
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(ten));
                    SingleData.Add("dfTotalEN_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(ten * CoModel.EnTen));
                }

                //小計
                double syokei;
                List<List<string>> syokeiSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string> { "1100", "1110", "1120", "1130", "1140", "1150", "1189", "1200", "1220", "1230", "1240", "1250", "1300", "1900" },
                        new List<string> { "2100", "2110", "2200", "2300", "2310", "2500", "2600", "2700" },
                        new List<string> { "3110", "3210", "3310" },
                        new List<string> { "4000", "4010" },
                        new List<string> { "5000", "5010" },
                        new List<string> { "6000", "6010" },
                        new List<string> { "7000", "7010" },
                        new List<string> { "8000", "8010", "8020" }
                    };
                foreach (List<string> syukeisaki in syokeiSyukeiSakils)
                {
                    syokei = CoModel.TenColTotalTen(syukeisaki);
                    SingleData.Add("dfSyokei_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(syokei));
                    SingleData.Add("dfSyokeiEN_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(syokei * CoModel.EnTen));
                }

                // 初診
                List<(string syukeiSaki, string field)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                        (ReceSyukeisaki.SyosinJikanGai, "dfSyosinJikanGai"),
                        (ReceSyukeisaki.SyosinKyujitu, "dfSyosinKyujitu"),
                        (ReceSyukeisaki.SyosinSinya, "dfSyosinSinya")
                    };

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (CoModel.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        SingleData.Add(syosinSyukeisaki[j].field, "〇");
                    }
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                if (Target == TargetConst.RousaiTanki)
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiRoudouNo", CoModel.RousaiKofu);
                }
                else
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiNenkinNo", CoModel.RousaiKofu);
                }
                #endregion
            }
        }

        /// <summary>
        /// 自賠責（労災準拠）用ヘッダー印字処理
        /// </summary>
        private void PrintReceiptHeaderForJibaiRousai()
        {
            #region Sub Function

            // 性別
            string _getSex()
            {
                string ret = "男";
                if (CoModel.Sex == 2)
                {
                    ret = "女";
                }
                return ret;
            }
            // 年齢
            string _getAge()
            {
                string ret = CIUtil.SDateToAge(CoModel.BirthDay, CIUtil.GetLastDateOfMonth(CoModel.SinYm * 100 + 1)).ToString();
                return ret;
            }
            // 初診
            string _getSyosin()
            {
                List<(string syukeiSaki, string comment)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                                            (ReceSyukeisaki.SyosinJikanGai, "時間外"),
                                            (ReceSyukeisaki.SyosinKyujitu, "休日"),
                                            (ReceSyukeisaki.SyosinSinya, "深夜")
                    };
                string Syosin = "";

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (CoModel.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        if (Syosin.Contains(syosinSyukeisaki[j].comment) == false)
                        {
                            if (Syosin != "") Syosin += ",";
                            Syosin += syosinSyukeisaki[j].comment;
                        }
                    }
                }

                return Syosin;
            }
            // 西暦(yyyymmdd）を和暦に変換して各フィールドに印字する
            void _printoutWarekiField(int date, string field, bool gengo, bool year, bool month, bool day)
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);
                if (wareki.Ymd != "")
                {
                    if (gengo) SingleData.Add($"{field}Gengo", wareki.Gengo);
                    if (year) SingleData.Add($"{field}Year", wareki.Year.ToString());
                    if (month) SingleData.Add($"{field}Month", wareki.Month.ToString());
                    if (day) SingleData.Add($"{field}Day", wareki.Day.ToString());
                }
            }
            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{CoModel.PtNum:D9}");
            //漢字氏名
            SingleData.Add("dfPtKanjiName", CoModel.PtName);
            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", CoModel.PtNum.ToString());
                // 診療年月
                _printoutWarekiField(CoModel.SinYm * 100 + 1, "dfSin", true, true, true, false);
                //生年
                _printoutWarekiField(CoModel.BirthDay, "dfBirth", true, true, false, false);

                //性別
                SingleData.Add("dfSex", _getSex());
                //年齢
                SingleData.Add("dfAge", _getAge());
                //受傷日
                _printoutWarekiField(CoModel.JibaiJyusyouDate, "dfJyusyou", true, true, true, true);
                //初診日
                _printoutWarekiField(CoModel.JibaiSyosinDate, "dfSyosin", true, true, true, true);
                //診療期間自
                _printoutWarekiField(CoModel.RyoyoStartDate, "dfSinryoStart", true, true, true, true);
                //診療期間至
                _printoutWarekiField(CoModel.RyoyoEndDate, "dfSinryoEnd", true, true, true, true);
                //実日数
                SingleData.Add("dfNissu", CIUtil.ToStringIgnoreNull(CoModel.RousaiJituNissu));
                //転帰
                switch (CoModel.RousaiTenki)
                {
                    case 1:
                        SingleData.Add("dfTenkiTiyu", "〇");
                        break;
                    case 3:
                        SingleData.Add("dfTenkiKeizoku", "〇");
                        break;
                    case 5:
                        SingleData.Add("dfTenkiTeni", "〇");
                        break;
                    case 7:
                        SingleData.Add("dfTenkiTyusi", "〇");
                        break;
                    case 9:
                        SingleData.Add("dfTenkiSibo", "〇");
                        break;
                }
                // 初診
                SingleData.Add("dfSyosin", _getSyosin());
                //イ
                SingleData.Add("dfTotal_I", CIUtil.ToStringIgnoreZero(CoModel.JibaiITensu));
                //ロ
                SingleData.Add("dfTotal_RO", CIUtil.ToStringIgnoreZero(CoModel.JibaiRoTensu));
                //ハ
                SingleData.Add("dfTotal_HA", CIUtil.ToStringIgnoreZero(CoModel.JibaiHaFutan));
                //ニ（その他）
                SingleData.Add("dfTotal_NI", CIUtil.ToStringIgnoreZero(CoModel.JibaiNiFutan));
                //ホ（診断書料）
                SingleData.Add("dfTotal_HO", CIUtil.ToStringIgnoreZero(CoModel.JibaiHoSindan));
                //ヘ（明細書料）
                SingleData.Add("dfTotal_HE", CIUtil.ToStringIgnoreZero(CoModel.JibaiHeMeisai));
                // 診断書枚数
                SingleData.Add("dfSindanCount", CIUtil.ToStringIgnoreZero(CoModel.JibaiHoSindanCount));
                // 明細書枚数
                SingleData.Add("dfMeisaiCount", CIUtil.ToStringIgnoreZero(CoModel.JibaiHeMeisaiCount));

                //加算率
                SingleData.Add("dfRateA", _systemConfRepository.GetSettingValue(3001, 1, HpId).ToString());
                SingleData.Add("dfRateC", _systemConfRepository.GetSettingValue(3001, 1, HpId).ToString());
                //合計A
                SingleData.Add("dfTotal_A", CoModel.JibaiAFutan.ToString());
                //合計B
                SingleData.Add("dfTotal_B", CoModel.JibaiBFutan.ToString());
                //合計C
                SingleData.Add("dfTotal_C", CoModel.JibaiCFutan.ToString());
                //合計D
                SingleData.Add("dfTotal_D", CoModel.JibaiDFutan.ToString());
                //合計ABCD
                SingleData.Add("dfTotal_ABCD1", CoModel.JibaiABCDFutan.ToString());
                SingleData.Add("dfTotal_ABCD2", CoModel.JibaiABCDFutan.ToString());
                //通院月
                SingleData.Add("dfTuinMonth", (CoModel.SinYm % 100).ToString());
                //通院日
                foreach (int tuuinDay in CoModel.TuuinDays)
                {
                    SingleData.Add($"dfDay{tuuinDay % 100}", "〇");
                }
                SingleData.Add("dfDayTotal", CoModel.TuuinDays.Count().ToString());
                //保険会社名
                SingleData.Add("dfHokenName", CoModel.JibaiHokenName);

                //医療機関住所
                SingleData.Add("txHpAddress", CoModel.HpAddress);
                //医療機関名
                SingleData.Add("dfHpName", CoModel.ReceHpName);
                //医師名
                SingleData.Add("dfKaisetuName", CoModel.KaisetuName);
                //医療機関電話番号
                SingleData.Add("dfHpTel", CoModel.HpTel);

                //病名欄
                foreach (CoReceiptByomeiModel byomei in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(byomei.Byomei));

                    CellData.Add(data);
                }

                //点数欄
                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                        "2110",
                        "2310",
                        "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(CoModel.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "5000" },
                        new List<string>{ "6000" },
                        new List<string>{ "7000" },
                        new List<string>{ "8000" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColCount(syukeisaki, onlySI)));
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "1900" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "3900" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" },
                        new List<string>{ "A131" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTotalTen(syukeisaki)));
                }

                //小計
                List<List<string>> syokeiSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189", "1220", "1221", "1230", "1240", "1250", "1300", "1900" },
                        new List<string>{ "2110", "2310", "2500", "2600", "2700" },
                        new List<string>{ "2100", "2200", "2300" },
                        new List<string>{ "3110", "3210", "3310" },
                        new List<string>{ "3900" }
                    };
                foreach (List<string> syukeisaki in syokeiSyukeiSakils)
                {
                    SingleData.Add("dfSyokei_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(CoModel.TenColTotalTen(syukeisaki)));
                }

                // 初診
                List<(string syukeiSaki, string field)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                        (ReceSyukeisaki.SyosinJikanGai, "dfSyosinJikanGai"),
                        (ReceSyukeisaki.SyosinKyujitu, "dfSyosinKyujitu"),
                        (ReceSyukeisaki.SyosinSinya, "dfSyosinSinya")
                    };

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (CoModel.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        SingleData.Add(syosinSyukeisaki[j].field, "〇");
                    }
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                if (Target == TargetConst.RousaiTanki)
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiRoudouNo", CoModel.RousaiKofu);
                }
                else
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiNenkinNo", CoModel.RousaiKofu);
                }
                #endregion
            }
        }

        private string getPrintNo()
        {
            string ret = CoModel.ReceiptNo.ToString();
            if (CoModel.Output == 1)
            {
                // 印刷済み
                ret = "R" + ret;
            }

            if (CoModel.SeikyuKbn == SeikyuKbnConst.Henrei)
            {
                // 返戻
                ret += "(H)";
            }

            return ret;
        }
    }
}
