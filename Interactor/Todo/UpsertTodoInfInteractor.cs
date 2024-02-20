using Domain.Models.Todo;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Todo;
using UseCase.Todo.GetTodoInfFinder;
using UseCase.Todo.UpsertTodoInf;

namespace Interactor.Todo;

public class UpsertTodoInfInteractor : IUpsertTodoInfInputPort
{
    private readonly ITodoInfRepository _todoInfRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public UpsertTodoInfInteractor(ITenantProvider tenantProvider, ITodoInfRepository todoInfRepository)
    {
        _todoInfRepository = todoInfRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public UpsertTodoInfOutputData Handle(UpsertTodoInfInputData input)
    {
        try
        {
            var validateResult = ValidateDataInput(input.TodoInfs);
            if (validateResult != UpsertTodoInfStatus.Success)
            {
                return new UpsertTodoInfOutputData(new(), validateResult);
            }

            if (input.TodoInfs.Count == 0)
            {
                return new UpsertTodoInfOutputData(new(), UpsertTodoInfStatus.InputNoData);
            }

            var checkInputTodoNo = input.TodoInfs.Where(x => x.TodoNo > 0).Select(x => x.TodoNo);
            if (checkInputTodoNo.Count() != checkInputTodoNo.Distinct().Count())
            {
                return new UpsertTodoInfOutputData(new(), UpsertTodoInfStatus.InvalidTodoInf);
            }

            if (!_todoInfRepository.CheckExist(input.HpId, input.TodoInfs.Where(x => x.TodoNo > 0).Select(x => new Tuple<int, int, long>(x.TodoNo, x.TodoEdaNo, x.PtId)).ToList()))
            {
                return new UpsertTodoInfOutputData(new(), UpsertTodoInfStatus.InvalidExistedInput);
            }
            var todoInfList = ConvertToInsertTodoInfDto(input.TodoInfs);
            var upsertResult = _todoInfRepository.Upsert(todoInfList, input.UserId, input.HpId);
            var result = GetListTodoInfos(upsertResult);
            return new UpsertTodoInfOutputData(result, UpsertTodoInfStatus.Success);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _todoInfRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private List<TodoInfModel> ConvertToInsertTodoInfDto(List<TodoInfDto> insertTodoinfDtos)
    {
        List<TodoInfModel> result = new();
        foreach (var todoInf in insertTodoinfDtos)
        {
            result.Add(new TodoInfModel(
                todoInf.TodoNo,
                todoInf.TodoEdaNo,
                todoInf.PtId,
                todoInf.SinDate,
                todoInf.RaiinNo,
                todoInf.TodoKbnNo,
                todoInf.TodoGrpNo,
                todoInf.Tanto,
                todoInf.Term,
                todoInf.Cmt1,
                todoInf.Cmt2,
                todoInf.IsDone,
                todoInf.IsDeleted));
        }

        return result;
    }

    private UpsertTodoInfStatus ValidateDataInput(List<TodoInfDto> insertTodoInfDtos)
    {
        foreach (var insertTodoInf in insertTodoInfDtos)
        {
            if (insertTodoInf.TodoNo < 0)
            {
                return UpsertTodoInfStatus.InvalidTodoNo;
            }
            else if (insertTodoInf.TodoEdaNo < 0)
            {
                return UpsertTodoInfStatus.InvalidTodoEdaNo;
            }
            else if (insertTodoInf.PtId <= 0)
            {
                return UpsertTodoInfStatus.InvalidPtId;
            }
            else if (insertTodoInf.SinDate < 0)
            {
                return UpsertTodoInfStatus.InvalidSinDate;
            }
            else if (insertTodoInf.RaiinNo < 0)
            {
                return UpsertTodoInfStatus.InvalidRaiinNo;
            }
            else if (insertTodoInf.TodoKbnNo < 0)
            {
                return UpsertTodoInfStatus.InvalidTodoKbnNo;
            }
            else if (insertTodoInf.TodoGrpNo < 0)
            {
                return UpsertTodoInfStatus.InvalidTodoGrpNo;
            }
            else if (insertTodoInf.Tanto < 0)
            {
                return UpsertTodoInfStatus.InvalidTanto;
            }
            else if (insertTodoInf.Term < 0)
            {
                return UpsertTodoInfStatus.InvalidTerm;
            }
            else if (insertTodoInf.IsDone < 0)
            {
                return UpsertTodoInfStatus.InvalidIsDone;
            }
            else if (insertTodoInf.IsDeleted < 0)
            {
                return UpsertTodoInfStatus.InvalidIsDeleted;
            }
        }

        return UpsertTodoInfStatus.Success;
    }

    private List<GetListTodoInfFinderOutputItem> GetListTodoInfos(List<TodoInfModel> todoInfModels)
    {
        List<GetListTodoInfFinderOutputItem> result = new(todoInfModels.Select(x => new GetListTodoInfFinderOutputItem(
                                                                        x.HpId,
                                                                        x.PtId,
                                                                        x.PtNum,
                                                                        x.PatientName,
                                                                        x.SinDate,
                                                                        x.PrimaryDoctorName,
                                                                        x.KaSname,
                                                                        x.TodoKbnName,
                                                                        x.Cmt1,
                                                                        x.CreateDate,
                                                                        x.CreaterName,
                                                                        x.TantoName,
                                                                        x.Cmt2,
                                                                        x.UpdateDate,
                                                                        x.UpdaterName,
                                                                        x.TodoGrpName,
                                                                        x.Term,
                                                                        x.HokenPid,
                                                                        x.Houbetu,
                                                                        x.HokenKbn,
                                                                        x.HokensyaNo,
                                                                        x.HokenId,
                                                                        x.Tanto,
                                                                        x.TodoNo,
                                                                        x.TodoEdaNo,
                                                                        x.RaiinNo,
                                                                        x.TodoKbnNo,
                                                                        x.TodoGrpNo,
                                                                        x.IsDone,
                                                                        x.Status,
                                                                        x.Sex,
                                                                        x.GroupColor,
                                                                        x.CreateId
                                                                        )));
        return result;
    }
}