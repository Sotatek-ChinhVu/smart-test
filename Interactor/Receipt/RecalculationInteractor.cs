using Domain.Models.Diseases;
using Domain.Models.DrugDetail;
using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.ReceSeikyu;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Helper.Messaging;
using Helper.Messaging.Data;
using Interactor.CalculateService;
using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.MedicalExamination.Calculate;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly IPtDiseaseRepository _ptDiseaseRepository;
    private readonly IOrdInfRepository _ordInfRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ITodayOdrRepository _todayOdrRepository;
    private readonly ICommonMedicalCheck _commonMedicalCheck;
    private readonly IInsuranceMstRepository _insuranceMstRepository;
    private readonly IReceSeikyuRepository _receSeikyuRepository;
    private readonly IDrugDetailRepository _drugDetailRepository;
    private readonly ICalculateService _calculateRepository;
    private readonly ICommonReceRecalculation _commonReceRecalculation;

    private const string _hokenChar = "0";
    private const string _kohi1Char = "1";
    private const string _kohi2Char = "2";
    private const string _kohi3Char = "3";
    private const string _kohi4Char = "4";
    private const string _suspectedSuffix = "の疑い";
    private const string _left = "左";
    private const string _right = "右";
    private const string _both = "両";
    private const string _leftRight = "左右";
    private const string _rightLeft = "右左";
    bool isStopCalc = false;

    public RecalculationInteractor(IReceiptRepository receiptRepository, ISystemConfRepository systemConfRepository, IPtDiseaseRepository ptDiseaseRepository, IOrdInfRepository ordInfRepository, IMstItemRepository mstItemRepository, ITodayOdrRepository todayOdrRepository, ICommonMedicalCheck commonMedicalCheck, IInsuranceMstRepository insuranceMstRepository, IReceSeikyuRepository receSeikyuRepository, IDrugDetailRepository drugDetailRepository, ICalculateService calculateService, ICommonReceRecalculation commonReceRecalculation)
    {
        _receiptRepository = receiptRepository;
        _systemConfRepository = systemConfRepository;
        _ptDiseaseRepository = ptDiseaseRepository;
        _ordInfRepository = ordInfRepository;
        _mstItemRepository = mstItemRepository;
        _todayOdrRepository = todayOdrRepository;
        _commonMedicalCheck = commonMedicalCheck;
        _insuranceMstRepository = insuranceMstRepository;
        _receSeikyuRepository = receSeikyuRepository;
        _drugDetailRepository = drugDetailRepository;
        _calculateRepository = calculateService;
        _commonReceRecalculation = commonReceRecalculation;
    }

    public RecalculationOutputData Handle(RecalculationInputData inputData)
    {
        try
        {
            bool success = true;
            var ptInfCount = inputData.PtIdList.Count;
            // run Recalculation
            if (!isStopCalc && inputData.IsRecalculationCheckBox)
            {
                success = RunCalculateMonth(inputData.HpId, inputData.SinYm, inputData.PtIdList, inputData.UniqueKey, inputData.CancellationToken);
            }

            // run Receipt Aggregation
            if (success && !isStopCalc && inputData.IsReceiptAggregationCheckBox)
            {
                success = ReceFutanCalculateMain(inputData.SinYm, inputData.PtIdList, inputData.UniqueKey, inputData.CancellationToken);
            }

            // check error in month
            if (success && !isStopCalc && inputData.IsCheckErrorCheckBox)
            {
                var receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SinYm, inputData.PtIdList);
                int allCheckCount = receRecalculationList.Count;

                success = _commonReceRecalculation.CheckErrorInMonth(inputData.HpId, inputData.PtIdList, inputData.SinYm, inputData.UserId, receRecalculationList, allCheckCount);
            }

            if (!inputData.IsCheckErrorCheckBox && !inputData.IsReceiptAggregationCheckBox && !inputData.IsRecalculationCheckBox)
            {
                SendMessager(new RecalculationStatus(true, 0, 0, 0, string.Empty, string.Empty));
            }
            return new RecalculationOutputData(success);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _systemConfRepository.ReleaseResource();
            _ptDiseaseRepository.ReleaseResource();
            _ordInfRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
            _todayOdrRepository.ReleaseResource();
            _insuranceMstRepository.ReleaseResource();
            _commonMedicalCheck.ReleaseResource();
            _receSeikyuRepository.ReleaseResource();
            _drugDetailRepository.ReleaseResource();
        }
    }

    private bool RunCalculateMonth(int hpId, int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, 1, 0, 0, "StartCalculateMonth", string.Empty));
        var statusCallBack = Messenger.Instance.SendAsync(new StopCalcStatus());
        isStopCalc = statusCallBack.Result.Result;
        if (isStopCalc)
        {
            return false;
        }
        _calculateRepository.RunCalculateMonth(new CalculateMonthRequest()
        {
            HpId = hpId,
            PtIds = ptInfList,
            SeikyuYm = seikyuYm,
            UniqueKey = uniqueKey
        }, cancellationToken);
        return true;
    }

    private bool ReceFutanCalculateMain(int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, 2, 0, 0, "StartFutanCalculateMain", string.Empty));
        var statusCallBack = Messenger.Instance.SendAsync(new StopCalcStatus());
        isStopCalc = statusCallBack.Result.Result;
        if (isStopCalc)
        {
            return false;
        }
        _calculateRepository.ReceFutanCalculateMain(new ReceCalculateRequest(ptInfList, seikyuYm, uniqueKey), cancellationToken);
        return true;
    }

    private void SendMessager(RecalculationStatus status)
    {
        Messenger.Instance.Send(status);
    }
}