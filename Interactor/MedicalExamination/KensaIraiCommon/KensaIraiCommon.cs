using Domain.Models.GroupInf;
using Domain.Models.KensaIrai;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Service;
using UseCase.MedicalExamination.SaveKensaIrai;

namespace Interactor.MedicalExamination.KensaIraiCommon;

public class KensaIraiCommon : IKensaIraiCommon
{
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IOrdInfRepository _ordInfRepository;
    private readonly IKensaIraiCoReportService _kensaIraiCoReportService;
    private readonly IGroupInfRepository _groupInfRepository;
    private readonly ILoggingHandler? _loggingHandler;
    private readonly ICoKensaIraiFinder _coKensaIraiFinder;

    public KensaIraiCommon(ITenantProvider tenantProvider, IKensaIraiRepository kensaIraiRepository, ISystemConfRepository systemConfRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository, IOrdInfRepository ordInfRepository, IKensaIraiCoReportService kensaIraiCoReportService, IGroupInfRepository groupInfRepository, ICoKensaIraiFinder coKensaIraiFinder)
    {
        _kensaIraiRepository = kensaIraiRepository;
        _systemConfRepository = systemConfRepository;
        _patientInforRepository = patientInforRepository;
        _receptionRepository = receptionRepository;
        _ordInfRepository = ordInfRepository;
        _kensaIraiCoReportService = kensaIraiCoReportService;
        _groupInfRepository = groupInfRepository;
        _coKensaIraiFinder = coKensaIraiFinder;
        var dbContextOptions = tenantProvider.CreateNewTrackingAdminDbContextOption();
        if (dbContextOptions != null)
        {
            _loggingHandler = new LoggingHandler(dbContextOptions, tenantProvider);
        }
        kensaCenterMst = new();
        odrInfModels = new();
        odrInfDetailModels = new();
        kensaInfModels = new();
        kensaInfDetailModels = new();
    }

    private int hpId;
    private int userId;
    private long ptId;
    private int sinDate;
    private long raiinNo;
    private int iraiDate;

    private KensaCenterMstModel kensaCenterMst;
    private List<OrdInfModel> odrInfModels;
    private List<OrdInfDetailModel> odrInfDetailModels;
    private List<KensaInfModel> kensaInfModels;
    private List<KensaInfDetailModel> kensaInfDetailModels;
    private PatientInforModel? ptInfModel;
    private ReceptionRowModel? raiinInfModel;

    private string messageResult = string.Empty;

    public SaveKensaIraiOutputData SaveKensaIraiAction(int hpId, int userId, long ptId, int sinDate, long raiinNo)
    {
        try
        {
            // 引数セット
            this.hpId = hpId;
            this.userId = userId;
            this.ptId = ptId;
            this.sinDate = sinDate;
            this.raiinNo = raiinNo;

            List<KensaIraiReportItem> kensaIraiReportItemList = new();
            // 初期処理（設定の取得、および、チェック）
            if (Init())
            {
                // データ取得
                GetData();

                // 取得したデータをチェック
                if (ptInfModel == null)
                {
                    // 患者情報なし
                    messageResult = $"患者情報がみつかりません。 ptid:{ptId}";
                    return new SaveKensaIraiOutputData(messageResult, new(), SaveKensaIraiStatus.Failed);
                }

                if (odrInfModels == null || !odrInfModels.Any() || odrInfDetailModels == null || !odrInfDetailModels.Any())
                {
                    // オーダー情報なし
                    messageResult = $"院外検査オーダーがみつかりません。 ptid:{ptId} raiinNo:{raiinNo}";
                    return new SaveKensaIraiOutputData(messageResult, new(), SaveKensaIraiStatus.Failed);
                }

                // 依頼データを作成する
                kensaIraiReportItemList = MakeIraiData();
            }
            return new SaveKensaIraiOutputData(messageResult, kensaIraiReportItemList, SaveKensaIraiStatus.Successed);
        }
        catch (Exception ex)
        {
            if (_loggingHandler != null)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
            }
            throw;
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
            _systemConfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
            _ordInfRepository.ReleaseResource();
            _groupInfRepository.ReleaseResource();
            if (_loggingHandler != null)
            {
                _loggingHandler.Dispose();
            }
            _coKensaIraiFinder.ReleaseResource();
        }
    }

    private bool Init()
    {
        // 依頼日（システム日付）
        iraiDate = CIUtil.DateTimeToInt(DateTime.Now);

        // ライセンスチェック
        if (_systemConfRepository.GetByGrpCd(hpId, 100019, 0).Val != 1)
        {
            // ライセンスなし
            messageResult = "ライセンスがありません";
            return false;
        }

        // 対象期間チェック
        int beforeDays = 0;
        int afterDays = 0;

        List<string> term = _systemConfRepository.GetByGrpCd(hpId, 100019, 1).Param.Split('-').ToList();

        if (term.Count == 2)
        {
            beforeDays = CIUtil.StrToIntDef(term[0], 0);
            afterDays = CIUtil.StrToIntDef(term[1], 0);
        }

        int beforeDate = CIUtil.DateTimeToInt(DateTime.Now.AddDays(beforeDays * -1));
        int afterDate = CIUtil.DateTimeToInt(DateTime.Now.AddDays(afterDays));

        if (sinDate < beforeDate || sinDate > afterDate)
        {
            // 対象期間外
            messageResult = $"診療日が対象期間外です sindate:{sinDate} term[{beforeDays}-{afterDays}]";
            return false;
        }

        // センターコード
        string odrKensaIraiCenterCd = _systemConfRepository.GetByGrpCd(hpId, 100019, 2).Param;
        kensaCenterMst = _kensaIraiRepository.GetKensaCenterMst(hpId, odrKensaIraiCenterCd);
        if (kensaCenterMst.CenterCd != odrKensaIraiCenterCd)
        {
            // センターコード未登録
            messageResult = $"センターコードが不正です centercd:{odrKensaIraiCenterCd}";
            return false;
        }

        return true;
    }

    /// <summary>
    /// データ取得（患者情報、来院情報、オーダー情報、検査情報）
    /// </summary>
    private void GetData()
    {
        // 患者情報取得
        ptInfModel = _patientInforRepository.GetById(hpId, ptId, sinDate, 0);

        // 来院情報取得
        raiinInfModel = _receptionRepository.GetList(hpId, sinDate, raiinNo, ptId).FirstOrDefault();

        // オーダー情報取得
        odrInfModels = _ordInfRepository.GetIngaiKensaOdrInf(hpId, ptId, sinDate, raiinNo);
        odrInfDetailModels = _ordInfRepository.GetIngaiKensaOdrInfDetail(hpId, ptId, sinDate, raiinNo, kensaCenterMst.CenterCd, kensaCenterMst.PrimaryKbn);

        // 既存の検査情報があれば取得
        kensaInfModels = _kensaIraiRepository.GetKensaInf(hpId, ptId, raiinNo, kensaCenterMst.CenterCd);
        kensaInfDetailModels = _kensaIraiRepository.GetKensaInfDetail(hpId, ptId, raiinNo, kensaCenterMst.CenterCd);
    }

    /// <summary>
    /// 依頼データを作成する
    /// </summary>
    private List<KensaIraiReportItem> MakeIraiData()
    {
        long keyNo = 0;

        // 透析区分別に検査情報を生成する
        for (int toseki = 0; toseki <= 2; toseki++)
        {
            // 依頼キーを取得（-1: エラー、0: 新規追加、>0: 既存の依頼コード）
            long iraiCd = GetIraiCd(toseki);

            if (iraiCd >= 0)
            {
                if (kensaInfDetailModels.Any(p => !string.IsNullOrEmpty(p.ResultVal)))
                {
                    // 既に検査結果が入力されている検査があった場合はエラー
                    messageResult = $"結果取り込み済みの項目が存在するため、依頼ファイルを作成できません iraicd:{iraiCd}";
                }
                else
                {
                    if (odrInfModels != null && odrInfModels.Any(p => p.TosekiKbn == toseki))
                    {
                        int firstOdrId = odrInfModels.Find(p => p.TosekiKbn == toseki)?.CreateId ?? 0;

                        // 至急区分を取得する
                        int sikyu = GetSikyuKbn(toseki);

                        // 透析区分をキーに、対象となるオーダー詳細を取得する
                        List<OrdInfDetailModel> targetOdrDtl = GetTargetOdrInfDetail(toseki);

                        if (targetOdrDtl.Any())
                        {
                            // 依頼詳細レコード削除
                            foreach (KensaInfDetailModel delKensaDtl in kensaInfDetailModels.FindAll(p => p.IraiCd == iraiCd))
                            {
                                delKensaDtl.ChangeIsDeleted(1);
                            }

                            #region 検査情報レコード 追加 or キー設定
                            keyNo++;
                            if (iraiCd == 0)
                            {
                                // iraiCd = 0なら追加
                                int createId = userId;
                                if (_systemConfRepository.GetByGrpCd(hpId, 100019, 7).Val == 2)
                                {
                                    // 福山臨床モードの場合は、先頭の検査オーダーの入力者にする
                                    createId = firstOdrId;
                                }
                                KensaInfModel addkensaInf = new(ptId,
                                                                iraiDate,
                                                                raiinNo,
                                                                iraiCd,
                                                                1,
                                                                0,
                                                                toseki,
                                                                sikyu,
                                                                0,
                                                                kensaCenterMst.CenterCd,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                false,
                                                                createId);
                                addkensaInf.IsAddNew = true;
                                addkensaInf.KeyNo = keyNo;

                                kensaInfModels.Add(addkensaInf);
                            }
                            else
                            {
                                // 既存のレコードを使用する場合は、キーだけ設定
                                var kensaInf = kensaInfModels.FirstOrDefault(p => p.IraiCd == iraiCd);
                                if (kensaInf != null)
                                {
                                    kensaInf.KeyNo = keyNo;
                                    int createId = userId;
                                    if (_systemConfRepository.GetByGrpCd(hpId, 100019, 7).Val == 2)
                                    {
                                        // 福山臨床モードの場合は、先頭の検査オーダーの入力者にする
                                        createId = firstOdrId;
                                    }
                                    kensaInf.UpdateKensaInfModel(createId, toseki, sikyu);

                                    kensaInf.IsUpdate = true;
                                }
                            }
                            #endregion

                            #region 検査情報詳細レコード　追加

                            // 詳細レコード連番
                            int seqNo = 0;

                            foreach (OrdInfDetailModel odrDtl in targetOdrDtl)
                            {
                                seqNo++;
                                long iraiCdItem = 0;
                                if (iraiCd > 0)
                                {
                                    // 既存の依頼コードを使用する場合は、セットする
                                    iraiCdItem = iraiCd;
                                }
                                KensaInfDetailModel addKensaDtl = new(ptId,
                                                                      iraiDate,
                                                                      raiinNo,
                                                                      iraiCdItem,
                                                                      seqNo,
                                                                      odrDtl.KensaItemCd,
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      0,
                                                                      string.Empty,
                                                                      string.Empty,
                                                                      odrDtl.KensaMstModel);
                                addKensaDtl.IsAddNew = true;
                                addKensaDtl.KeyNo = keyNo;

                                kensaInfDetailModels.Add(addKensaDtl);
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        // DBに反映する
        _kensaIraiRepository.SaveKensaInf(hpId, userId, kensaInfModels, kensaInfDetailModels);

        //// 依頼ファイルを作成する
        return SaveIraiFile();
    }

    /// <summary>
    /// 依頼コードを取得する
    /// 透析区分をキーに検査情報を検索
    /// 0件の場合・・・新たに検査情報を作成
    /// 1件の場合・・・既存の依頼コードを流用する
    /// 2件以上の場合・・・エラーとする
    /// </summary>
    /// <param name="toseki"></param>
    /// <returns>
    /// -1: 既存データ複数あり
    ///  0: 既存データなし（新規追加）
    /// >0: 既存の依頼コード
    /// </returns>
    private long GetIraiCd(int toseki)
    {
        long iraiCd = -1;

        if (!kensaInfModels.Any(p => p.TosekiKbn == toseki))
        {
            // 条件に合う検査依頼情報がない場合は、新規追加する
            iraiCd = 0;
        }
        else if (kensaInfModels.Count(p => p.TosekiKbn == toseki) == 1)
        {
            // 条件に合う検査依頼情報が1件の場合は、依頼コードをそのまま使用する
            iraiCd = kensaInfModels.Find(p => p.TosekiKbn == toseki)?.IraiCd ?? 0;
        }
        else
        {
            // 条件に合う検査依頼情報が複数の場合は、エラーとする
            List<string> iraiCds = kensaInfModels.Where(p => p.TosekiKbn == toseki).Select(p => p.IraiCd.ToString()).ToList();

            messageResult = $"同一条件で複数の検査依頼が存在するため連携できません：iraiCd:{string.Join(", ", iraiCds)}";
        }
        return iraiCd;
    }

    /// <summary>
    /// 至急区分を取得する
    /// 透析区分が一致するオーダー情報の中に、1つでも至急区分=1のレコードがあれば1を返す
    /// </summary>
    /// <param name="toseki">透析区分</param>
    /// <returns>1-至急あり</returns>
    private int GetSikyuKbn(int toseki)
    {
        int ret = 0;

        if (odrInfModels.Any(p => p.TosekiKbn == toseki && p.SikyuKbn == 1))
        {
            ret = 1;
        }

        return ret;
    }

    /// <summary>
    /// 透析区分が一致するオーダー詳細を取得する
    /// </summary>
    /// <param name="toseki"></param>
    /// <returns></returns>
    private List<OrdInfDetailModel> GetTargetOdrInfDetail(int toseki)
    {
        List<OrdInfDetailModel> results = new();

        foreach (OrdInfModel odrInf in odrInfModels.FindAll(p => p.TosekiKbn == toseki))
        {
            foreach (OrdInfDetailModel odrDtl in odrInfDetailModels.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
            {
                results.Add(odrDtl);
            }
        }
        return results;
    }

    private List<KensaIraiReportItem> SaveIraiFile()
    {
        List<KensaIraiReportItem> result = new();
        int odrKensaIraiFileType = (int)_systemConfRepository.GetByGrpCd(hpId, 100019, 7).Val;
        var odrKensaIraiKaCode = _systemConfRepository.GetByGrpCd(hpId, 100019, 8).Val;
        var odrkensaIraiKaCodeParam = _systemConfRepository.GetByGrpCd(hpId, 100019, 8).Param;
        var odrKensaIraiSameFileModel = _systemConfRepository.GetByGrpCd(hpId, 100019, 9);
        int odrKensaIraiSameFile = odrKensaIraiSameFileModel.GrpCd == 0 ? 1 : (int)odrKensaIraiSameFileModel.Val;
        var ptIdList = kensaInfModels.Select(item => item.PtId).Distinct().ToList();
        var ptGrpList = _groupInfRepository.GetAllByPtIdList(hpId, ptIdList);

        // KeyNo > 0 ・・・ 依頼対象の検査情報レコード
        foreach (var kensaInf in kensaInfModels.FindAll(p => p.KeyNo > 0))
        {
            int seqNo = 0;

            // detail生成
            List<Reporting.Kensalrai.Model.KensaIraiDetailModel> addKensaIraiDtls = new();
            foreach (var kensaDtl in kensaInfDetailModels.FindAll(p => p.KeyNo == kensaInf.KeyNo).Select(item => item.KensaMstModel).ToList())
            {
                KensaMst kensaMst = new();
                kensaMst.KensaItemCd = kensaDtl.KensaItemCd;
                kensaMst.CenterItemCd1 = kensaDtl.CenterItemCd;
                kensaMst.KensaKana = kensaDtl.KensaKana;
                kensaMst.KensaName = kensaDtl.KensaName;
                kensaMst.ContainerCd = kensaDtl.ContainerCd;
                addKensaIraiDtls.Add(new Reporting.Kensalrai.Model.KensaIraiDetailModel(true, 0, 0, 0, seqNo, kensaMst));
            }

            if (addKensaIraiDtls.Any())
            {
                var weightHeight = _coKensaIraiFinder.GetHeightWeight(hpId, kensaInf.PtId, raiinInfModel?.SinDate ?? 0);

                var raiinInf = _receptionRepository.GetRaiinInf(hpId, kensaInf.PtId, raiinInfModel?.SinDate ?? 0, raiinInfModel?.RaiinNo ?? 0);
                // 検査依頼データ生成
                var addKensaIrai =
                    new Reporting.Kensalrai.Model.KensaIraiModel(
                        raiinInfModel?.SinDate ?? 0,
                        kensaInf.RaiinNo,
                        kensaInf.IraiCd,
                        kensaInf.PtId,
                        kensaInf.PtNum,
                        ptInfModel?.Name ?? string.Empty,
                        ptInfModel?.KanaName ?? string.Empty,
                        ptInfModel?.Sex ?? 0,
                        ptInfModel?.Birthday ?? 0,
                        kensaInf.TosekiKbn,
                        kensaInf.SikyuKbn,
                        raiinInfModel?.KaId ?? 0,
                        weightHeight.weight,
                        weightHeight.height,
                        raiinInf?.TantoName ?? string.Empty,
                        raiinInf?.TantoKanaName ?? string.Empty,
                        raiinInf?.KaSName ?? string.Empty,
                        kensaInf.UpdateDate.ToString("HHmm"),
                        addKensaIraiDtls);
                addKensaIrai.UpdateTime = kensaInf.UpdateDate.ToString("HHmm");

                if (odrKensaIraiFileType == 2)
                {
                    // 福山臨床
                    addKensaIrai.KaName = raiinInfModel?.KaName ?? string.Empty;
                    addKensaIrai.DrName = raiinInfModel?.DrName ?? string.Empty;
                }
                if (odrKensaIraiKaCode == 1)
                {
                    // グループに合わせた科コードを出力する場合

                    addKensaIrai.KaCodeName = string.Empty;

                    // パラメータを分解
                    if (!string.IsNullOrEmpty(odrkensaIraiKaCodeParam))
                    {
                        string[] parameters = odrkensaIraiKaCodeParam.Split(',');

                        foreach (string param in parameters)
                        {
                            string[] dtls = param.Split('=');

                            if (dtls.Count() == 3 && ExistPtGroup(addKensaIrai.PtId, CIUtil.StrToIntDef(dtls[0], 0), dtls[1]))
                            {
                                addKensaIrai.KaCodeName = dtls[2];
                                break;
                            }

                            if (!string.IsNullOrEmpty(addKensaIrai.KaCodeName))
                            {
                                break;
                            }
                        }
                    }
                }
                addKensaIrai.TantoKanaName = raiinInfModel?.TantoKanaName ?? string.Empty;

                // ファイルの内容を取得
                List<string> output = _kensaIraiCoReportService.GetIraiFileData(kensaCenterMst.CenterCd, new List<Reporting.Kensalrai.Model.KensaIraiModel> { addKensaIrai }, odrKensaIraiFileType);
                List<string> outputDummy = _kensaIraiCoReportService.GetIraiFileDataDummy(kensaCenterMst.CenterCd, new List<Reporting.Kensalrai.Model.KensaIraiModel> { addKensaIrai }, odrKensaIraiFileType);
                result.Add(new KensaIraiReportItem(
                               output,
                               outputDummy,
                               odrKensaIraiSameFile,
                               raiinInfModel?.KaId ?? 0,
                               kensaInf.IraiCd,
                               ptInfModel?.PtNum ?? 0));
            }
        }

        bool ExistPtGroup(long ptid, int grpid, string grpcd)
        {
            var exist = ptGrpList.Any(item => item.PtId == ptid
                                              && item.GroupId == grpid
                                              && item.GroupCode == grpcd);
            return exist;
        }

        return result;
    }
}
