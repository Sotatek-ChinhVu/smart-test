using Domain.Models.CalculateModel;
using Helper.Enum;
using Helper.Messaging.Data;
using Helper.Messaging;
using Infrastructure.Interfaces;
using Interactor.CalculateService;
using Newtonsoft.Json;
using System.Net.Http;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;
using UseCase.MedicalExamination.Calculate;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.Receipt.GetListReceInf;
using UseCase.Receipt.Recalculation;
using UseCase.Core.Sync;
using Helper.Common;
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
