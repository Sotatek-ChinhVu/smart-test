using Domain.Models.Todo;
using UseCase.Todo.GetTodoInfFinder;

namespace Interactor.Todo;

public class GetTodoInfFinderInteractor : IGetTodoInfFinderInputPort
{
    private readonly ITodoInfRepository _todoInfRepository;
    public GetTodoInfFinderInteractor(ITodoInfRepository todoInfRepository)
    {
        _todoInfRepository = todoInfRepository;
    }

    public GetTodoInfFinderOutputData Handle(GetTodoInfFinderInputData input)
    {
        try
        {
            if (input.HpId <= 0)
            {
                return new GetTodoInfFinderOutputData(GetTodoInfFinderStatus.InvalidHpId, new());
            }
            if (input.TodoNo < 0)
            {
                return new GetTodoInfFinderOutputData(GetTodoInfFinderStatus.InvalidTodoNo, new());
            }
            if (input.TodoEdaNo < 0)
            {
                return new GetTodoInfFinderOutputData(GetTodoInfFinderStatus.InvalidTodoEdaNo, new());
            }

            var todoInf = GetListTodoInfos(input.HpId, input.TodoNo, input.TodoEdaNo, input.IncDone, input.SortByPtNum);

            return new GetTodoInfFinderOutputData(GetTodoInfFinderStatus.Success, todoInf);
        }
        finally
        {
            _todoInfRepository.ReleaseResource();
        }
    }

    private List<GetListTodoInfFinderOutputItem> GetListTodoInfos(int hpId, int todoNo, int todoEdaNo, bool incDone, bool sortByPtNum)
    {
        List<GetListTodoInfFinderOutputItem> result = new(_todoInfRepository.GetList(hpId, todoNo, todoEdaNo, incDone, sortByPtNum: sortByPtNum).Select(x => new GetListTodoInfFinderOutputItem(
                                                                        hpId,
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
