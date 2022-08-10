using Domain.Models.ColumnSetting;
using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Helper.Constants;
using UseCase.Reception.GetVisitingColumnSettings;

namespace Interactor.Reception;

public class GetVisitingColumnSettingsInteractor : IGetVisitingColumnSettingsInputPort
{
    private readonly IColumnSettingRepository _columnSettingRepository;
    private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;

    public GetVisitingColumnSettingsInteractor(IColumnSettingRepository columnSettingRepository,
        IRaiinKubunMstRepository raiinKubunMstRepository)
    {
        _columnSettingRepository = columnSettingRepository;
        _raiinKubunMstRepository = raiinKubunMstRepository;
    }

    public GetVisitingColumnSettingsOutputData Handle(GetVisitingColumnSettingsInputData input)
    {
        var settings = GetSettings(input.UserId);
        return new GetVisitingColumnSettingsOutputData(GetVisitingColumnSettingsStatus.Success, settings);
    }

    private List<ColumnSettingModel> GetSettings(int userId)
    {
        var columnSettings = _columnSettingRepository.GetList(userId, ColumnSettingInfo.Visiting.TableName);
        return StandardizeSettings(columnSettings, userId);
    }

    private List<ColumnSettingModel> StandardizeSettings(List<ColumnSettingModel> columnSettings, int userId)
    {
        var staticColumnNames = GetStaticColumnNames();
        var dynamicColumnNames = _raiinKubunMstRepository.GetList(false).OrderBy(r => r.SortNo).Select(r => r.GroupId.ToString());
        var allColumnNames = staticColumnNames.Concat(dynamicColumnNames);
        var maxDisplayOrder = columnSettings.Any() ? columnSettings.Max(c => c.DisplayOrder) : 0;

        var matchingColNameToSettingQuery =
            from colName in allColumnNames
            join colSetting in columnSettings on colName equals colSetting.ColumnName into colSettings
            from setting in colSettings.DefaultIfEmpty()
            // Set the default value for the missing setting
            select setting ?? new ColumnSettingModel(userId, ColumnSettingInfo.Visiting.TableName, colName,
                ++maxDisplayOrder, false, false, ColumnSettingInfo.Visiting.ColumnDefaultWidth);

        return matchingColNameToSettingQuery.OrderByDescending(c => c.IsPinned).ThenBy(c => c.DisplayOrder).ToList();
    }

    private IEnumerable<string> GetStaticColumnNames()
    {
        var colNames = new string[] {
            nameof(ReceptionRowModel.UketukeNo),
            nameof(ReceptionRowModel.SameVisit),
            nameof(ReceptionRowModel.Status),
            nameof(ReceptionRowModel.PtNum),
            nameof(ReceptionRowModel.KanaName),
            nameof(ReceptionRowModel.Name),
            nameof(ReceptionRowModel.Sex),
            nameof(ReceptionRowModel.Birthday),
            nameof(ReceptionRowModel.Age),
            nameof(ReceptionRowModel.IsNameDuplicate),
            nameof(ReceptionRowModel.YoyakuTime),
            nameof(ReceptionRowModel.ReservationName),
            nameof(ReceptionRowModel.UketukeSbtId),
            nameof(ReceptionRowModel.UketukeTime),
            nameof(ReceptionRowModel.SinStartTime),
            nameof(ReceptionRowModel.SinEndTime),
            nameof(ReceptionRowModel.KaikeiTime),
            nameof(ReceptionRowModel.RaiinCmt),
            nameof(ReceptionRowModel.PtComment),
            nameof(ReceptionRowModel.HokenPatternName),
            nameof(ReceptionRowModel.TantoId),
            nameof(ReceptionRowModel.KaId),
            nameof(ReceptionRowModel.LastVisitDate),
            nameof(ReceptionRowModel.Sname),
            nameof(ReceptionRowModel.RaiinRemark),
            nameof(ReceptionRowModel.ConfirmationState),
            nameof(ReceptionRowModel.ConfirmationResult)
        };

        // Camel case column names
        return colNames.Select(colName => FirstCharToLower(colName));
    }

    private string FirstCharToLower(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToLower(s[0]) + s.Substring(1);
    }
}
