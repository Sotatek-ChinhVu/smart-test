using Domain.Models.KensaIrai;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Helper.Common;
using UseCase.MedicalExamination.SaveKensaIrai;

namespace Interactor.MedicalExamination;

public class SaveKensaIraiInteractor : ISaveKensaIraiInputPort
{
    private readonly IKensaIraiRepository _kensaIraiRepository;
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IOrdInfRepository _ordInfRepository;

    public SaveKensaIraiInteractor(IKensaIraiRepository kensaIraiRepository, ISystemConfRepository systemConfRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository, IOrdInfRepository ordInfRepository)
    {
        _kensaIraiRepository = kensaIraiRepository;
        _systemConfRepository = systemConfRepository;
        _patientInforRepository = patientInforRepository;
        _receptionRepository = receptionRepository;
        _ordInfRepository = ordInfRepository;
    }

    int hpId;
    long ptId;
    int sinDate;
    long raiinNo;
    int iraiDate;

    KensaCenterMstModel kensaCenterMst;
    List<OrdInfModel> OdrInfModels;
    List<OrdInfDetailModel> OdrInfDetailModels;
    List<KensaInfModel> KensaInfModels;
    List<KensaInfDetailModel> KensaInfDetailModels;
    PatientInforModel? PtInfModel;
    ReceptionRowModel? RaiinInfModel;

    string messageResult;


    public SaveKensaIraiOutputData Handle(SaveKensaIraiInputData inputData)
    {
        try
        {
            // 引数セット
            hpId = inputData.HpId;
            ptId = inputData.PtId;
            sinDate = inputData.SinDate;
            raiinNo = inputData.RaiinNo;

            // 初期処理（設定の取得、および、チェック）
            if (Init())
            {
                // データ取得
                GetData();

                // 取得したデータをチェック
                if (PtInfModel == null)
                {
                    // 患者情報なし
                    messageResult = $"患者情報がみつかりません。 ptid:{ptId}";
                    return new SaveKensaIraiOutputData(messageResult, SaveKensaIraiStatus.Successed);
                }

                if (OdrInfModels == null || !OdrInfModels.Any() || OdrInfDetailModels == null || !OdrInfDetailModels.Any())
                {
                    // オーダー情報なし
                    messageResult = $"院外検査オーダーがみつかりません。 ptid:{ptId} raiinNo:{raiinNo}";
                    return new SaveKensaIraiOutputData(messageResult, SaveKensaIraiStatus.IsDeleteFile);
                }

                // 依頼データを作成する
                //MakeIraiData();
            }
            return new SaveKensaIraiOutputData(messageResult, SaveKensaIraiStatus.Successed);
        }
        finally
        {
            _kensaIraiRepository.ReleaseResource();
            _systemConfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
            _ordInfRepository.ReleaseResource();
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
        PtInfModel = _patientInforRepository.GetById(hpId, ptId, sinDate, 0);

        // 来院情報取得
        RaiinInfModel = _receptionRepository.GetList(hpId, sinDate, raiinNo, ptId).FirstOrDefault();

        // オーダー情報取得
        OdrInfModels = _ordInfRepository.GetIngaiKensaOdrInf(hpId, ptId, sinDate, raiinNo);
        OdrInfDetailModels = _ordInfRepository.GetIngaiKensaOdrInfDetail(hpId, ptId, sinDate, raiinNo, kensaCenterMst.CenterCd, kensaCenterMst.PrimaryKbn);

        // 既存の検査情報があれば取得
        KensaInfModels = _kensaIraiRepository.GetKensaInf(hpId, ptId, raiinNo, kensaCenterMst.CenterCd);
        KensaInfDetailModels = _kensaIraiRepository.GetKensaInfDetail(hpId, ptId, raiinNo, kensaCenterMst.CenterCd);
    }
}
