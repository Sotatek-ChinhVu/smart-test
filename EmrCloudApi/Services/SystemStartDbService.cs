using Helper.Common;
using UseCase.Core.Sync;
using UseCase.SystemStartDbs;

namespace EmrCloudApi.Services
{
    public class SystemStartDbService : ISystemStartDbService
    {
        private readonly UseCaseBus _bus;

        public SystemStartDbService(UseCaseBus bus)
        {
            _bus = bus;
        }

        public void DeleteAndUpdateData()
        {
            Console.WriteLine("Delete : " + DateTime.Now.ToString());

            int dateDelete = CIUtil.DateTimeToInt(DateTime.Now.AddDays(-90));
            var input = new SystemStartDbInputData(dateDelete);
            var output = _bus.Handle(input);
        }
    }
}
