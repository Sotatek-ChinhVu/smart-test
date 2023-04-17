using Domain.Models.Todo;
using UseCase.Todo;
using UseCase.Todo.UpsertTodoInf;

namespace Interactor.Todo;

public class UpsertTodoInfInteractor : IUpsertTodoInfInputPort
{
    private readonly ITodoInfRepository _todoInfRepository;

    public UpsertTodoInfInteractor(ITodoInfRepository todoInfRepository)
    {
        _todoInfRepository = todoInfRepository;
    }

    public UpsertTodoInfOutputData Handle(UpsertTodoInfInputData input)
    {
        try
        {
            var validateResult = ValidateDataInput(input.TodoInfs);
            if (validateResult != UpsertTodoInfStatus.Success)
            {
                return new UpsertTodoInfOutputData(validateResult);
            }

            if (input.TodoInfs.Count == 0)
            {
                return new UpsertTodoInfOutputData(UpsertTodoInfStatus.InputNoData);
            }

            var checkInputTodoNo = input.TodoInfs.Where(x => x.TodoNo > 0).Select(x => x.TodoNo);
            if (checkInputTodoNo.Count() != checkInputTodoNo.Distinct().Count())
            {
                return new UpsertTodoInfOutputData(UpsertTodoInfStatus.InvalidTodoInf);
            }

            if (_todoInfRepository.CheckExist(input.TodoInfs.Where(x => x.TodoNo > 0).Select(x => new Tuple<int, int, long>(x.TodoNo, x.TodoEdaNo, x.PtId)).ToList()))
            {
                return new UpsertTodoInfOutputData(UpsertTodoInfStatus.InvalidExistedInput);
            }
            var todoInfList = ConvertToInsertTodoInfDto(input.TodoInfs);
            _todoInfRepository.Upsert(todoInfList, input.UserId, input.HpId);

            return new UpsertTodoInfOutputData(UpsertTodoInfStatus.Success);
        }
        finally
        {
            _todoInfRepository.ReleaseResource();
        }
    }

    private List<TodoInfModel> ConvertToInsertTodoInfDto(List<InsertTodoInfDto> insertTodoinfDtos)
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

    private UpsertTodoInfStatus ValidateDataInput(List<InsertTodoInfDto> insertTodoInfDtos)
    {
        foreach (var insertTodoInf in insertTodoInfDtos)
        {
            if (insertTodoInf.TodoNo <= 0)
            {
                return UpsertTodoInfStatus.InvalidTodoNo;
            }
            else if (insertTodoInf.TodoEdaNo <= 0)
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
}