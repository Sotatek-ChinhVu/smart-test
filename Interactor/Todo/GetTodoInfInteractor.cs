using Domain.Models.Todo;
using UseCase.Reception.GetListRaiinInf;
using UseCase.Todo.GetTodoInf;

namespace Interactor.Todo;

public class GetTodoInfInteractor : IGetTodoInfInputPort
{
    private readonly ITodoInfRepository _todoInfRepository;
    public GetTodoInfInteractor(ITodoInfRepository todoInfRepository)
    {
        _todoInfRepository = todoInfRepository;
    }

    public GetTodoInfOutputData Handle(GetTodoInfInputData input)
    {
        try
        {
            if(input.HpId <= 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidHpId, new());
            }
            if (input.TodoNo <= 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidTodoNo, new());
            }
            if (input.TodoEdaNo < 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidTodoEdaNo, new());
            }
            if (input.PtId <= 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidPtId, new());
            }
            if (input.IsDone < 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidIsDone, new());
            }

            var TodoInf = GetListTodoInfos(input.HpId, input.TodoNo, input.TodoEdaNo, input.PtId, input.IsDone).Select(item => new GetListTodoInfOutputItem(item.HpId,
                                        item.PtNum,
                                        item.PatientName,
                                        item.SinDate,
                                        item.PrimaryDoctorName,
                                        item.KaSname,
                                        item.TodoKbnName,
                                        item.TantoName,
                                        item.Cmt2,
                                        item.CreateDate,
                                        item.UpdaterName,
                                        item.Cmt1,
                                        item.CreaterName,
                                        item.TodoGrpName,
                                        item.Term,
                                        item.Houbetu,
                                        item.HokensyaNo,    
                                        item.HokenPid,
                                        item.HokenKbn,
                                        item.HokenId))
                                        .ToList();

            return new GetTodoInfOutputData(GetTodoInfStatus.Success, TodoInf);
        }
        finally 
        {
            _todoInfRepository.ReleaseResource();
        }
    }

    private List<GetListTodoInfOutputItem> GetListTodoInfos(int hpId, int todoNo, int todoEdaNo, int ptId, int isDone)
    {
        List<GetListTodoInfOutputItem> result = new(_todoInfRepository.GetList(hpId, todoNo, todoEdaNo, ptId, isDone).Select(x => new GetListTodoInfOutputItem(
                                                                        hpId,
                                                                        x.PtNum,
                                                                        x.PatientName,
                                                                        x.SinDate,
                                                                        x.PrimaryDoctorName,
                                                                        x.KaSname,
                                                                        x.TodoKbnName,
                                                                        x.TantoName,
                                                                        x.Cmt2,
                                                                        x.CreateDate,
                                                                        x.UpdaterName,
                                                                        x.Cmt1,
                                                                        x.CreaterName,
                                                                        x.TodoGrpName,
                                                                        x.Term,
                                                                        x.Houbetu,
                                                                        x.HokensyaNo,
                                                                        x.HokenPid,
                                                                        x.HokenKbn,
                                                                        x.HokenId)));
        return result;
    }
}
