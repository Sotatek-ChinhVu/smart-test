using DocumentFormat.OpenXml.Drawing.Charts;
using Domain.Models.CalculateModel;
using Domain.Models.Ka;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.User;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Interactor.CalculateService;
using UseCase.Receipt.GetListReceInf;
using UseCase.Reception.GetOutDrugOrderList;

namespace Interactor.Reception;

public class GetOutDrugOrderListInteractor : IGetOutDrugOrderListInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;
    private readonly ICalculateService _calculateService;
    private readonly IKaRepository _kaMstRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public GetOutDrugOrderListInteractor(IReceptionRepository raiinInfRepository, ICalculateService calculateService, IKaRepository kaMstRepository, IUserRepository userRepository, IPatientInforRepository patientInforRepository)
    {
        _raiinInfRepository = raiinInfRepository;
        _calculateService = calculateService;
        _kaMstRepository = kaMstRepository;
        _userRepository = userRepository;
        _patientInforRepository = patientInforRepository;
    }

    public GetOutDrugOrderListOutputData Handle(GetOutDrugOrderListInputData inputData)
    {
        try
        {
            List<RaiinInfToPrintModel> result;
            if (inputData.IsPrintPrescription)
            {
                result = _raiinInfRepository.GetOutDrugOrderList(inputData.HpId, inputData.FromDate, inputData.ToDate);
            }
            else
            {
                result = FormatRaiinInfWithReceInfParam(inputData);
            }
            result = result.OrderBy(item => item.PtNum).ToList();
            return new GetOutDrugOrderListOutputData(result, GetOutDrugOrderListStatus.Successed);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
            _kaMstRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _calculateService.ReleaseSource();
        }
    }

    private List<RaiinInfToPrintModel> FormatRaiinInfWithReceInfParam(GetOutDrugOrderListInputData inputData)
    {
        var kaMstList = _kaMstRepository.GetList(DeleteTypes.None);
        var userMstList = _userRepository.GetAll(inputData.SinDate, true, false);
        List<RaiinInfToPrintModel> listSource = new();
        int intStartDate = inputData.FromDate;
        int intEndDate = inputData.ToDate;
        if (intStartDate.ToString().Length == 8)
        {
            intStartDate = intStartDate / 100;
        }
        if (intEndDate.ToString().Length == 8)
        {
            intEndDate = intEndDate / 100;
        }

        List<ReceInfModel> listKaikeFrm = new();

        /// get list model by KaikeiTotalCalculate 
        /// KaikeiTotalCalculate is get for 1 month only
        /// So have to get data per month and group it

        List<int> dateList = new();
        while (intStartDate <= intEndDate)
        {
            dateList.Add(intStartDate);
            intStartDate++;
        }

        object obj = new object();
        Parallel.ForEach(dateList, date =>
        {
            var receInfs = _calculateService.GetListReceInf(new GetInsuranceInfInputData(inputData.HpId, 0, date.AsInteger())).ReceInfModels;
            if (receInfs != null && receInfs.Any())
            {
                lock (obj)
                {
                    listKaikeFrm.AddRange(receInfs);
                }
            }
        });

        listSource.AddRange(listKaikeFrm.Select(u => new RaiinInfToPrintModel(inputData.IsPrintAccountingCard ? PrintMode.PrintAccountingCard : PrintMode.PrintAccountingCardList, u)).ToList());

        var ptIdList = listSource.Select(item => item.PtId).Distinct().ToList();
        var ptInfList = _patientInforRepository.SearchPatient(inputData.HpId, ptIdList);

        listSource = listSource.OrderBy(item => item.SinDate).ToList();
        foreach (var model in listSource)
        {
            // Formart for RaiinInfToPrintModel with param ReceInfModel
            var kaDisplay = kaMstList.FirstOrDefault(item => item.KaId == model.KaId)?.KaName ?? string.Empty;
            var tantoIdDisplay = userMstList.FirstOrDefault(item => item.UserId == model.TantoId)?.Name ?? string.Empty;
            long ptNum = 0;
            string nameBinding = string.Empty;

            var ptInf = ptInfList.FirstOrDefault(item => item.PtId == model.PtId);
            if (ptInf != null)
            {
                ptNum = ptInf.PtNum;
                nameBinding = ptInf.Name;
            }
            model.ChangeParams(kaDisplay, tantoIdDisplay, ptNum, nameBinding);
        }
        return listSource;
    }
}
