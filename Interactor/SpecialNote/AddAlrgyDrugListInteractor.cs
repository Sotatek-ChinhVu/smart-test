using Domain.Models.PtCmtInf;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using static Helper.Constants.PtAlrgyDrugConst;

namespace Interactor.SpecialNote
{
    public class AddAlrgyDrugListInteractor : IAddAlrgyDrugListInputPort
    {
        private readonly IImportantNoteRepository _importantNoteRepository;

        public AddAlrgyDrugListInteractor(IImportantNoteRepository importantNoteRepository)
        {
            _importantNoteRepository = importantNoteRepository;
        }

        public AddAlrgyDrugListOutputData Handle(AddAlrgyDrugListInputData inputDatas)
        {
            List<KeyValuePair<int, AddAlrgyDrugListStatus>> keyValuePairs = new List<KeyValuePair<int, AddAlrgyDrugListStatus>>();

            var datas = inputDatas.DataList.Distinct();
            var ptId = datas?.FirstOrDefault()?.PtId;
            var alrgyDrugOlds = ptId == null ? new List<PtAlrgyDrugModel>() : _importantNoteRepository.GetAlrgyDrugList(long.Parse(ptId.ToString() ?? string.Empty));

            var count = 0;
            if (datas?.Count() > 0)
            {
                foreach (var item in datas)
                {
                    if (!alrgyDrugOlds.Any(a => a.ItemCd == item.ItemCd))
                        keyValuePairs.Add(new(count, AddAlrgyDrugListStatus.InvalidDuplicate));
                    count++;
                }
            }

            var alrgyDrugs = datas?.Select(
                    i => new PtAlrgyDrugModel(
                            i.HpId,
                            i.PtId,
                            i.SeqNo,
                            i.SortNo,
                            i.ItemCd,
                            i.DrugName,
                            i.StartDate,
                            i.EndDate,
                            i.Cmt,
                            i.IsDeleted
                        )
                ).ToList() ?? new List<PtAlrgyDrugModel>();

            if (alrgyDrugs.Count == 0)
                keyValuePairs.Add(new(-1, AddAlrgyDrugListStatus.InputNoData));

            count = 0;
            foreach (var item in alrgyDrugs)
            {
                var valid = item.Validation();
                if (valid != ValidationStatus.Valid)
                {
                    keyValuePairs.Add(new(count, CovertToAddAlrgyDrugListStatus(valid)));
                }
                count++;
            }
            return new AddAlrgyDrugListOutputData(keyValuePairs);
        }
        private AddAlrgyDrugListStatus CovertToAddAlrgyDrugListStatus(ValidationStatus validationStatus)
        {
            switch (validationStatus)
            {
                case ValidationStatus.InvalidHpId:
                    return AddAlrgyDrugListStatus.InvalidHpId;
                case ValidationStatus.InvalidPtId:
                    return AddAlrgyDrugListStatus.InvalidPtId;
                case ValidationStatus.InvalidSeqNo:
                    return AddAlrgyDrugListStatus.InvalidSeqNo;
                case ValidationStatus.InvalidSortNo:
                    return AddAlrgyDrugListStatus.InvalidSortNo;
                case ValidationStatus.InvalidStartDate:
                    return AddAlrgyDrugListStatus.InvalidStartDate;
                case ValidationStatus.InvalidEndDate:
                    return AddAlrgyDrugListStatus.InvalidEndDate;
                case ValidationStatus.InvalidIsDeleted:
                    return AddAlrgyDrugListStatus.InvalidIsDeleted;
                default:
                    return AddAlrgyDrugListStatus.Successed;
            }
        }
    }
}
