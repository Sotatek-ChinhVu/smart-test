using Domain.SuperAdminModels.Tenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using System.Text;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.ExportCsvTenantList;
using UseCase.SuperAdmin.GetTenant;
using UseCase.SuperAdmin.UpgradePremium;

namespace SuperAdminAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExportCSVController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public ExportCSVController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpPost("ExportTenantList")]
    public IActionResult ExportTenantList([FromBody] ExportTenantListRequest request)
    {
        var input = new ExportCsvTenantListInputData(request.ColumnView, GetSearchTenantModel(request.SearchModel), request.SortDictionary, request.Skip, request.Take);
        var output = _bus.Handle(input);
        if (output.Status == ExportCsvTenantListStatus.NoData)
        {
            return Ok("出力データがありません。");
        }
        return RenderCsv(output.Data, "TenantList.csv");
    }

    #region private function
    private SearchTenantModel GetSearchTenantModel(SearchTenantRequestItem requestItem)
    {
        return new SearchTenantModel(
                   requestItem.KeyWord,
                   requestItem.FromDate,
                   requestItem.ToDate,
                   requestItem.Type,
                   requestItem.StatusTenant,
                   requestItem.StorageFull);
    }

    private IActionResult RenderCsv(List<string> dataList, string fileName)
    {
        if (!dataList.Any())
        {
            return Content(@"");
        }
        var csv = new StringBuilder();

        string contentType = "text/csv";

        foreach (var row in dataList)
        {
            csv.AppendLine(row);
        }
        var content = Encoding.UTF8.GetBytes(csv.ToString());
        var result = Encoding.UTF8.GetPreamble().Concat(content).ToArray();
        return File(result, contentType, fileName);
    }
    #endregion
}
