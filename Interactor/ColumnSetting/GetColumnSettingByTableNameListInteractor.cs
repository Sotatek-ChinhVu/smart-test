using Domain.Models.ColumnSetting;
using UseCase.ColumnSetting.GetColumnSettingByTableNameList;

namespace Interactor.ColumnSetting;
public class GetColumnSettingByTableNameListInteractor : IGetColumnSettingByTableNameListInputPort
{
    private readonly IColumnSettingRepository _columnSettingRepository;

    public GetColumnSettingByTableNameListInteractor(IColumnSettingRepository columnSettingRepository)
    {
        _columnSettingRepository = columnSettingRepository;
    }

    public GetColumnSettingByTableNameListOutputData Handle(GetColumnSettingByTableNameListInputData input)
    {
        try
        {
            var settingList = _columnSettingRepository.GetList(input.HpId, input.UserId, input.TableNameList);
            return new GetColumnSettingByTableNameListOutputData(settingList, GetColumnSettingByTableNameListStatus.Successed);
        }
        finally
        {
            _columnSettingRepository.ReleaseResource();
        }
    }
}

