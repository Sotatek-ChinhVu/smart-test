using Domain.Models.ColumnSetting;
using UseCase.ColumnSetting.GetList;

namespace Interactor.ColumnSetting;

public class GetColumnSettingListInteractor : IGetColumnSettingListInputPort
{
    private readonly IColumnSettingRepository _columnSettingRepository;

    public GetColumnSettingListInteractor(IColumnSettingRepository columnSettingRepository)
    {
        _columnSettingRepository = columnSettingRepository;
    }

    public GetColumnSettingListOutputData Handle(GetColumnSettingListInputData input)
    {
        var settings = _columnSettingRepository.GetList(input.UserId, input.TableName);
        var status = settings.Any() ? GetColumnSettingListStatus.Success : GetColumnSettingListStatus.NoData;
        return new GetColumnSettingListOutputData(status, settings);
    }
}
