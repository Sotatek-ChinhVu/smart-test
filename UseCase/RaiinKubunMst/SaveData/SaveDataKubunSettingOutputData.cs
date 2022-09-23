using System.Linq;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.Save
{
    public class SaveDataKubunSettingOutputData : IOutputData
    {
        public SaveDataKubunSettingOutputData(List<(bool, string)> message)
        {
            Message = message;
        }

        public List<(bool, string)> Message { get; private set; }

        public override string? ToString()
        {
            return String.Join("|", Message.Select(x => x.Item2).ToList());
        }
    }
}
