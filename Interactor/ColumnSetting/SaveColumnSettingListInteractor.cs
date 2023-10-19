using Domain.Models.ColumnSetting;
using UseCase.ColumnSetting.SaveList;

namespace Interactor.ColumnSetting;

public class SaveColumnSettingListInteractor : ISaveColumnSettingListInputPort
{
    private readonly IColumnSettingRepository _columnSettingRepository;

    public SaveColumnSettingListInteractor(IColumnSettingRepository columnSettingRepository)
    {
        _columnSettingRepository = columnSettingRepository;
    }

    public SaveColumnSettingListOutputData Handle(SaveColumnSettingListInputData input)
    {
        try
        {
            bool success = _columnSettingRepository.SaveList(input.Settings);
            var status = success ? SaveColumnSettingListStatus.Success : SaveColumnSettingListStatus.Failed;
            return new SaveColumnSettingListOutputData(status);
        }
        finally
        {
            _columnSettingRepository.ReleaseResource();
        }
    }
}
