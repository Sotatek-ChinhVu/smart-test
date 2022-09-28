using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.ImportantNote;
using UseCase.SpecialNote.AddAlrgyDrugList;
using static Helper.Constants.PtAlrgyDrugConst;

namespace Interactor.SpecialNote
{
    public class AddAlrgyDrugListInteractor : IAddAlrgyDrugListInputPort
    {
        private readonly IImportantNoteRepository _importantNoteRepository;
        private readonly IPatientInforRepository _patientInfoRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public AddAlrgyDrugListInteractor(IImportantNoteRepository importantNoteRepository, IPatientInforRepository patientInfoRepository, IMstItemRepository mstItemRepository)
        {
            _importantNoteRepository = importantNoteRepository;
            _patientInfoRepository = patientInfoRepository;
            _mstItemRepository = mstItemRepository;
        }

        public AddAlrgyDrugListOutputData Handle(AddAlrgyDrugListInputData inputDatas)
        {
            try
            {
                List<KeyValuePair<int, AddAlrgyDrugListStatus>> keyValuePairs = new();

                var datas = inputDatas.DataList.Distinct();
                var ptId = datas?.FirstOrDefault()?.PtId;
                var alrgyDrugOlds = ptId == null ? new List<PtAlrgyDrugModel>() : _importantNoteRepository.GetAlrgyDrugList(long.Parse(ptId.ToString() ?? string.Empty));

                var count = 0;
                var alrgyDrugs = datas?.Select(
                        i => new PtAlrgyDrugModel(
                                0,
                                i.PtId,
                                0,
                                0,
                                i.ItemCd,
                                i.DrugName,
                                i.StartDate,
                                i.EndDate,
                                i.Cmt,
                                0
                            )
                    ).ToList() ?? new List<PtAlrgyDrugModel>();

                if (alrgyDrugs == null || alrgyDrugs.Count == 0)
                {
                    keyValuePairs.Add(new(-1, AddAlrgyDrugListStatus.InputNoData));
                    return new AddAlrgyDrugListOutputData(keyValuePairs);
                }

                foreach (var item in alrgyDrugs)
                {
                    var valid = item.Validation();
                    if (valid != ValidationStatus.Valid)
                    {
                        keyValuePairs.Add(new(count, CovertToAddAlrgyDrugListStatus(valid)));
                    }
                    if (alrgyDrugOlds.Any(a => a.ItemCd == item.ItemCd) && !keyValuePairs.Any(k => k.Key == count))
                    {
                        keyValuePairs.Add(new(count, AddAlrgyDrugListStatus.InvalidDuplicate));
                    }

                    var checkPtId = _patientInfoRepository.CheckListId(new List<long> { item.PtId });
                    var chekTenMst = _mstItemRepository.CheckItemCd(item.ItemCd);

                    if (!checkPtId && !keyValuePairs.Any(k => k.Key == count))
                    {
                        keyValuePairs.Add(new(count, AddAlrgyDrugListStatus.PtIdNoExist));
                    }
                    if (!chekTenMst && !keyValuePairs.Any(k => k.Key == count))
                    {
                        keyValuePairs.Add(new(count, AddAlrgyDrugListStatus.ItemCdNoExist));
                    }
                    count++;
                }

                foreach (var item in keyValuePairs)
                {
                    alrgyDrugs?.RemoveAt(item.Key);
                }
                if (alrgyDrugs?.Count > 0)
                    _importantNoteRepository.AddAlrgyDrugList(alrgyDrugs);

                return new AddAlrgyDrugListOutputData(keyValuePairs);
            }
            catch
            {
                return new AddAlrgyDrugListOutputData(new List<KeyValuePair<int, AddAlrgyDrugListStatus>>() { new(-1, AddAlrgyDrugListStatus.Failed) });
            }
        }

        private static AddAlrgyDrugListStatus CovertToAddAlrgyDrugListStatus(ValidationStatus validationStatus)
        {
            return validationStatus switch
            {
                ValidationStatus.InvalidPtId => AddAlrgyDrugListStatus.InvalidPtId,
                ValidationStatus.InvalidSortNo => AddAlrgyDrugListStatus.InvalidSortNo,
                ValidationStatus.InvalidStartDate => AddAlrgyDrugListStatus.InvalidStartDate,
                ValidationStatus.InvalidEndDate => AddAlrgyDrugListStatus.InvalidEndDate,
                ValidationStatus.InvalidItemCd => AddAlrgyDrugListStatus.InvalidItemCd,
                ValidationStatus.InvalidDrugName => AddAlrgyDrugListStatus.InvalidDrugName,
                ValidationStatus.InvalidCmt => AddAlrgyDrugListStatus.InvalidCmt,
                _ => AddAlrgyDrugListStatus.Successed,
            };
        }
    }
}
