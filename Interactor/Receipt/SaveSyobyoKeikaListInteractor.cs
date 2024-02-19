using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Receipt;
using UseCase.Receipt.SaveListSyobyoKeika;

namespace Interactor.Receipt;

public class SaveSyobyoKeikaListInteractor : ISaveSyobyoKeikaListInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveSyobyoKeikaListInteractor(ITenantProvider tenantProvider, IReceiptRepository receiptRepository, IPatientInforRepository patientInforRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _patientInforRepository = patientInforRepository;
        _insuranceRepository = insuranceRepository;
        _mstItemRepository = mstItemRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveSyobyoKeikaListOutputData Handle(SaveSyobyoKeikaListInputData inputData)
    {
        try
        {
            var responseValidate = ValidateInput(inputData);
            if (responseValidate != SaveSyobyoKeikaListStatus.ValidateSuccess)
            {
                return new SaveSyobyoKeikaListOutputData(responseValidate, new());
            }
            var listSyobyoKeikaModel = inputData.SyobyoKeikaList.Select(item => ConvertToSyobyoKeikaModel(inputData.PtId, inputData.SinYm, inputData.HokenId, item))
                                                                .ToList();

            var syobyoKeikaDBList = _receiptRepository.GetSyobyoKeikaList(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.HokenId);
            var seqNoListDB = syobyoKeikaDBList.Select(item => item.SeqNo).Distinct().ToList();
            List<SyobyoKeikaItem> syobyoKeikaInvalidList = inputData.SyobyoKeikaList.Where(item => item.SeqNo > 0 && !seqNoListDB.Contains(item.SeqNo)).ToList();

            if (_receiptRepository.SaveSyobyoKeikaList(inputData.HpId, inputData.UserId, listSyobyoKeikaModel))
            {
                return new SaveSyobyoKeikaListOutputData(SaveSyobyoKeikaListStatus.Successed, syobyoKeikaInvalidList);
            }
            return new SaveSyobyoKeikaListOutputData(SaveSyobyoKeikaListStatus.Failed, syobyoKeikaInvalidList);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _insuranceRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private SyobyoKeikaModel ConvertToSyobyoKeikaModel(long ptId, int sinYm, int hokenId, SyobyoKeikaItem inputItem)
    {
        return new SyobyoKeikaModel(
                                       ptId,
                                       sinYm,
                                       inputItem.SinDay,
                                       hokenId,
                                       inputItem.SeqNo,
                                       inputItem.Keika,
                                       inputItem.IsDeleted
                                   );
    }

    private SaveSyobyoKeikaListStatus ValidateInput(SaveSyobyoKeikaListInputData inputData)
    {
        var listHokenKbn = new List<int> { 11, 12, 13 };
        if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(inputData.HpId, new List<long>() { inputData.PtId }))
        {
            return SaveSyobyoKeikaListStatus.InvalidPtId;
        }
        else if (inputData.SinYm.ToString().Length != 6)
        {
            return SaveSyobyoKeikaListStatus.InvalidSinYm;
        }
        var hokenKbn = _insuranceRepository.GetHokenKbnByHokenId(inputData.HpId, inputData.HokenId, inputData.PtId);
        if (inputData.HokenId < 0 || !listHokenKbn.Contains(hokenKbn))
        {
            return SaveSyobyoKeikaListStatus.InvalidHokenId;
        }
        else if (!inputData.SyobyoKeikaList.Any())
        {
            return SaveSyobyoKeikaListStatus.Failed;
        }
        else if (hokenKbn == 13 && inputData.SyobyoKeikaList.Any(item => item.SinDay > 31
        || item.SinDay < 0))
        {
            return SaveSyobyoKeikaListStatus.InvalidSinDay;
        }
        //else if ((hokenKbn == 11 || hokenKbn == 12) && (inputData.SyobyoKeikaList.Count > 1 || inputData.SyobyoKeikaList.Any(item => item.SinDay != 0)))
        //{
        //    return SaveSyobyoKeikaListStatus.InvalidSinDay;
        //}
        //else if ((hokenKbn == 13) && inputData.SyobyoKeikaList.Any(item => !item.IsDeleted && item.Keika == string.Empty))
        //{
        //    return SaveSyobyoKeikaListStatus.InvalidKeika;
        //}
        return SaveSyobyoKeikaListStatus.ValidateSuccess;
    }
}
