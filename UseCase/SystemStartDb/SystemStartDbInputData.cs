using UseCase.Core.Sync.Core;

namespace UseCase.SystemStartDbs
{
    public class SystemStartDbInputData : IInputData<SystemStartDbOutputData>
    {
        public SystemStartDbInputData(int delDate)
        {
            DelDate = delDate;
        }

        public int DelDate { get; private set; }
    }
}
