using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public class AddAlrgyDrugListOutputData : IOutputData
    {
        public AddAlrgyDrugListOutputData(List<KeyValuePair<int, AddAlrgyDrugListStatus>> keyValuePairs)
        {
            KeyPairValues = keyValuePairs;
        }

        public List<KeyValuePair<int, AddAlrgyDrugListStatus>> KeyPairValues { get; private set; }
    }
}
