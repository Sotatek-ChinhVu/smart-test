using Domain.Models.FlowSheet;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using UseCase.FlowSheet.Upsert;

namespace Interactor.FlowSheet
{
    public class UpsertFlowSheetInteractor : IUpsertFlowSheetInputPort
    {
        private readonly IFlowSheetRepository _flowsheetRepository;
        private readonly IPatientInforRepository _patientRepository;
        private readonly IReceptionRepository _receptionRepository;
        public UpsertFlowSheetInteractor(IFlowSheetRepository repository, IPatientInforRepository patientRepository, IReceptionRepository receptionRepository)
        {
            _flowsheetRepository = repository;
            _patientRepository = patientRepository;
            _receptionRepository = receptionRepository;
        }

        public UpsertFlowSheetOutputData Handle(UpsertFlowSheetInputData inputData)
        {

            if (inputData.ToList().Count == 0)
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.InputDataNoValid);
            }
            if (inputData.Items.Any(i => i.RainNo <= 0))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RainNoNoValid);
            }
            if (inputData.Items.Any(i => i.PtId <= 0))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.PtIdNoValid);
            }
            if (inputData.Items.Any(i => i.SinDate < 19000000 || i.SinDate > 30000000))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.SinDateNoValid);
            }
            if (inputData.Items.Any(i => i.Flag))
            {
                try
                {
                    var checkTagNo = inputData.Items.Where(i => i.Flag).Any(i => int.Parse(i.Value) < 0 || int.Parse(i.Value) > 7);
                    if (checkTagNo)
                    {
                        return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.TagNoNoValid);
                    }
                }
                catch
                {
                    return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.TagNoNoValid);
                }
            }
            if (!_patientRepository.CheckExistListId(inputData.Items.Select(i => i.PtId).Distinct().ToList()))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.PtIdNoExist);
            }
            if (!_receptionRepository.CheckListNo(inputData.Items.Select(i => i.RainNo).ToList()))
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.RaiinNoExist);
            }

            try
            {
                var dataTags = inputData?.ToList()?.Where(i => i.Flag).Select(i => new FlowSheetModel(
                        i.SinDate,
                        int.Parse(i.Value),
                        "",
                        i.RainNo,
                        0,
                        string.Empty,
                        0,
                        true,
                        true,
                        new List<RaiinListInfModel>(),
                        i.PtId
                    )).ToList() ?? new List<FlowSheetModel>();
                _flowsheetRepository.UpsertTag(dataTags);

                var dataCmts = inputData?.ToList()?.Where(i => !i.Flag).Select(i => new FlowSheetModel(
                       i.SinDate,
                       0,
                       "",
                       i.RainNo,
                       0,
                       i.Value,
                       0,
                       true,
                       true,
                       new List<RaiinListInfModel>(),
                       i.PtId
                   )).ToList() ?? new List<FlowSheetModel>();
                _flowsheetRepository.UpsertCmt(dataCmts);

                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.Success);
            }
            catch
            {
                return new UpsertFlowSheetOutputData(UpsertFlowSheetStatus.UpdateNoSuccess);
            }
        }
    }
}
