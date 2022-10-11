using System.Linq;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.Save
{
    public class SaveDataKubunSettingOutputData : IOutputData
    {
        public SaveDataKubunSettingOutputData(List<string> message)
        {
            Message = message;
        }

        public List<string> Message { get; private set; }

        public override string? ToString()
        {
            return String.Join("|", Message.ToList());
        }
    }
}
