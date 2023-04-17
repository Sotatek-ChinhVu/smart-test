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

            var TodoInf = GetListTodoInfos(input.HpId, input.TodoNo, input.TodoEdaNo, input.IncDone).Select(item => new GetListTodoInfFinderOutputItem(item.HpId,
                                                                        item.PtId,
                                                                        item.PtNum,
                                                                        item.PatientName,
                                                                        item.SinDate,
                                                                        item.PrimaryDoctorName,
                                                                        item.KaSname,
                                                                        item.TodoKbnName,
                                                                        item.Cmt1,
                                                                        item.CreateDate,
                                                                        item.CreaterName,
                                                                        item.TantoName,
                                                                        item.Cmt2,
                                                                        item.UpdateDate,
                                                                        item.UpdaterName,
                                                                        item.TodoGrpName,
                                                                        item.Term,
                                                                        item.HokenPid,
                                                                        item.Houbetu,
                                                                        item.HokenKbn,
                                                                        item.HokensyaNo,
                                                                        item.HokenId)).ToList();

            return new GetTodoInfFinderOutputData(GetTodoInfFinderStatus.Success, TodoInf);
        }
        finally
        {
            _todoInfRepository.ReleaseResource();
        }
    }

    private List<GetListTodoInfFinderOutputItem> GetListTodoInfos(int hpId, int todoNo, int todoEdaNo, bool incDone)
    {
        List<GetListTodoInfFinderOutputItem> result = new(_todoInfRepository.GetList(hpId, todoNo, todoEdaNo, incDone).Select(x => new GetListTodoInfFinderOutputItem(
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
                                                                        x.HokenId)));
        return result;
    }
}
