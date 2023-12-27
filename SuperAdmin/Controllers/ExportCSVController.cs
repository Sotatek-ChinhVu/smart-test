using Domain.SuperAdminModels.Logger;
using Domain.SuperAdminModels.Tenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdminAPI.Request.AuditLog;
using SuperAdminAPI.Request.Tennant;
using System.Text;
using UseCase.Core.Sync;
using ClosedXML.Excel;
using UseCase.SuperAdmin.ExportCsvLogList;
using UseCase.SuperAdmin.ExportCsvTenantList;
using System.Data;
using System.Net.Mime;
using System.Web;
using Helper.Enum;

namespace SuperAdminAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExportCsvController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public ExportCsvController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpPost("ExportTenantList")]
    public IActionResult ExportTenantList([FromBody] ExportTenantListRequest request)
    {
        var input = new ExportCsvTenantListInputData(request.ColumnView, GetSearchTenantModel(request.SearchModel), request.SortDictionary);
        var output = _bus.Handle(input);
        if (output.Status == ExportCsvTenantListStatus.NoData)
        {
            return Ok("出力データがありません。");
        }
        return RenderCsv(output.Data, "TenantList.csv");
    }

    [HttpGet("ExportAuditLogList")]
    public IActionResult ExportAuditLogList([FromQuery] ExportAuditLogListRequest request)
    {
        var searchModel = new AuditLogSearchModel(
                              request.LogId,
                              request.StartDate,
                              request.EndDate,
                              request.Domain,
                              request.ThreadId,
                              request.LogType,
                              request.HpId,
                              request.UserId,
                              request.LoginKey,
                              request.DepartmentId,
                              request.SinDay,
                              request.EventCd,
                              request.PtId,
                              request.RaiinNo,
                              request.Path,
                              request.RequestInfo,
                              request.ClientIP,
                              request.Desciption);
        var input = new ExportCsvLogListInputData(request.ColumnView, request.TenantId, searchModel, request.SortDictionary);
        var output = _bus.Handle(input);
        if (output.Status == ExportCsvLogListStatus.NoData)
        {
            return Ok("出力データがありません。");
        }
        return RenderAuditLogExcel(request.ColumnView, output.AuditLogList, "AuditLogList.xlsx");
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

    private IActionResult RenderAuditLogExcel(List<AuditLogEnum> columnView, List<AuditLogModel> auditLogList, string fileName)
    {
        ContentDisposition cd = new ContentDisposition
        {
            FileName = HttpUtility.UrlEncode(fileName),
            Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
        };
        Response.Headers.Add("Content-Disposition", cd.ToString());
        var workbook = new XLWorkbook();
        var workSheet = workbook.Worksheets.Add("AuditLog");

        Dictionary<AuditLogEnum, string> columnNameDictionary = new();
        if (columnView.Any())
        {
            foreach (var item in columnView.Where(item => ColumnCsvName.ColumnNameAuditLogDictionary.ContainsKey(item)).ToList())
            {
                columnNameDictionary.Add(item, ColumnCsvName.ColumnNameAuditLogDictionary[item]);
            }
        }
        else
        {
            columnNameDictionary = ColumnCsvName.ColumnNameAuditLogDictionary;
        }

        #region add data to result excel
        int column = 1;
        int row = 1;
        foreach (var item in columnNameDictionary)
        {
            workSheet.Cell(row, column).SetValue(item.Value);
            workSheet.Cell(row, column).Style.Font.FontSize = 13;
            workSheet.Cell(row, column).Style.Font.Bold = true;
            column++;
        }
        row++;
        foreach (var auditLog in auditLogList)
        {
            column = 1;
            foreach (var item in columnNameDictionary)
            {
                switch (item.Key)
                {
                    case AuditLogEnum.LogType:
                        workSheet.Cell(row, column).SetValue(auditLog.LogType);
                        break;
                    case AuditLogEnum.UserId:
                        workSheet.Cell(row, column).SetValue(auditLog.UserId.ToString());
                        break;
                    case AuditLogEnum.LoginKey:
                        workSheet.Cell(row, column).SetValue(auditLog.LoginKey);
                        break;
                    case AuditLogEnum.LogDate:
                        workSheet.Cell(row, column).SetValue(auditLog.LogDate.ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz"));
                        break;
                    case AuditLogEnum.EventCd:
                        workSheet.Cell(row, column).SetValue(auditLog.EventCd);
                        break;
                    case AuditLogEnum.PtId:
                        workSheet.Cell(row, column).SetValue(auditLog.PtId.ToString());
                        break;
                    case AuditLogEnum.SinDay:
                        workSheet.Cell(row, column).SetValue(auditLog.SinDay.ToString());
                        break;
                    case AuditLogEnum.RequestInfo:
                        // the maximum charaters of each cell in excel is 32767
                        if (auditLog.RequestInfo.Length > 32767)
                        {
                            workSheet.Cell(row, column).SetValue(auditLog.RequestInfo.Substring(0, 32767));
                            break;
                        }
                        workSheet.Cell(row, column).SetValue(auditLog.RequestInfo);
                        break;
                    case AuditLogEnum.Desciption:
                        workSheet.Cell(row, column).SetValue(auditLog.Desciption);
                        break;
                    case AuditLogEnum.HpId:
                        workSheet.Cell(row, column).SetValue(auditLog.HpId);
                        break;
                    case AuditLogEnum.RaiinNo:
                        workSheet.Cell(row, column).SetValue(auditLog.RaiinNo);
                        break;
                    case AuditLogEnum.ClientIP:
                        workSheet.Cell(row, column).SetValue(auditLog.ClientIP);
                        break;
                    case AuditLogEnum.ThreadId:
                        workSheet.Cell(row, column).SetValue(auditLog.ThreadId);
                        break;
                    case AuditLogEnum.DepartmentId:
                        workSheet.Cell(row, column).SetValue(auditLog.DepartmentId);
                        break;
                    case AuditLogEnum.Path:
                        workSheet.Cell(row, column).SetValue(auditLog.Path);
                        break;
                    case AuditLogEnum.LogId:
                        workSheet.Cell(row, column).SetValue(auditLog.LogId);
                        break;
                }
                column++;
            }
            row++;
        }
        #endregion
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var byteData = stream.ToArray();
        return File(byteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
    #endregion
}
