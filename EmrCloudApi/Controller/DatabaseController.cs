using EmrCloudApi.Requests.Database;
using EmrCloudApi.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.MigrateDatabase;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public DatabaseController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("MigrateDatabase")]
        public ActionResult<Response> GetList([FromBody] MigrateDatabaseRequest request)
        {
            var input = new MigrateDatabaseInputData(request.UserName, request.Password);
            var output = _bus.Handle(input);

            Response response = new Response();
            response.Status = (byte)output.Status;

            switch (output.Status)
            {
                case MigrateDatabaseStatus.Successed:
                    response.Message = "Successed";
                    break;
                case MigrateDatabaseStatus.MigrateDataFailed:
                    response.Message = "MigrateDataFailed";
                    break;
                case MigrateDatabaseStatus.LoginFailed:
                    response.Message = "LoginFailed";
                    break;
            }

            return new ActionResult<Response>(response);
        }
    }
}
