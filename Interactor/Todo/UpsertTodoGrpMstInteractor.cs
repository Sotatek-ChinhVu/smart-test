using Domain.Models.Todo;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Todo;
using UseCase.Todo.UpsertTodoGrpMst;

namespace Interactor.Todo;

public class UpsertTodoGrpMstInteractor : IUpsertTodoGrpMstInputPort
{
    private readonly ITodoGrpMstRepository _todoGrpMstRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public UpsertTodoGrpMstInteractor(ITenantProvider tenantProvider, ITodoGrpMstRepository todoGrpMstRepository)
    {
        _todoGrpMstRepository = todoGrpMstRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public UpsertTodoGrpMstOutputData Handle(UpsertTodoGrpMstInputData input)
    {
        try
        {
            var validateResult = ValidateDataInput(input.TodoGrpMsts);
            if (validateResult != UpsertTodoGrpMstStatus.Success)
            {
                return new UpsertTodoGrpMstOutputData(validateResult);
            }

            if (input.TodoGrpMsts == null || input.TodoGrpMsts.Count == 0)
            {
                return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.InputNoData);
            }

            if (_todoGrpMstRepository.CheckExistedTodoGrpNo(input.HpId, input.TodoGrpMsts.Where(x => x.TodoGrpNo > 0).Select(x => x.TodoGrpNo).ToList()))
            {
                return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.InvalidExistedTodoGrpNoIsDeleted);
            }

            var checkInputTodoGrpNo = input.TodoGrpMsts.Where(x => x.TodoGrpNo > 0).Select(x => x.TodoGrpNo);
            if (checkInputTodoGrpNo.Count() != checkInputTodoGrpNo.Distinct().Count())
            {
                return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.InvalidTodoGrpMst);
            }

            var todoGrpMstList = ConvertToInsertTodoGrpMstDto(input.TodoGrpMsts);

            _todoGrpMstRepository.Upsert(todoGrpMstList, input.UserId, input.HpId);

            return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.Success);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _todoGrpMstRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private List<TodoGrpMstModel> ConvertToInsertTodoGrpMstDto(List<TodoGrpMstDto> insertTodoGrpMstDtos)
    {
        List<TodoGrpMstModel> result = new();
        foreach (var todoGrpMst in insertTodoGrpMstDtos)
        {
            result.Add(new TodoGrpMstModel(
                todoGrpMst.TodoGrpNo,
                todoGrpMst.TodoGrpName,
                todoGrpMst.GrpColor,
                todoGrpMst.SortNo,
                todoGrpMst.IsDeleted));
        }

        return result;
    }

    private UpsertTodoGrpMstStatus ValidateDataInput(List<TodoGrpMstDto> insertTodoGrpMstDtos)
    {
        foreach (var insertTodoGrpMst in insertTodoGrpMstDtos)
        {
            if (insertTodoGrpMst.TodoGrpNo < 0)
            {
                return UpsertTodoGrpMstStatus.InvalidTodoGrpNo;
            }
            else if (insertTodoGrpMst.TodoGrpName.Length > 20)
            {
                return UpsertTodoGrpMstStatus.InvalidTodoGrpName;
            }
            else if (insertTodoGrpMst.GrpColor.Length > 8)
            {
                return UpsertTodoGrpMstStatus.InvalidGrpColor;
            }
            else if (insertTodoGrpMst.SortNo < 0)
            {
                return UpsertTodoGrpMstStatus.InvalidSortNo;
            }
            else if (insertTodoGrpMst.IsDeleted < 0)
            {
                return UpsertTodoGrpMstStatus.InvalidIsDeleted;
            }
        }
        return UpsertTodoGrpMstStatus.Success;
    }
}