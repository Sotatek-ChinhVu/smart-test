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
            if (input.TodoNo < 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidTodoNo, new());
            }
            if (input.TodoEdaNo < 0)
            {
                return new GetTodoInfOutputData(GetTodoInfStatus.InvalidTodoEdaNo, new());
            }

            var TodoInf = GetListTodoInfos(input.HpId, input.TodoNo, input.TodoEdaNo, input.IncDone).Select(item => new GetListTodoInfOutputItem(item.HpId,
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
                                                                        item.HokenId))
                                        .ToList();

            return new GetTodoInfOutputData(GetTodoInfStatus.Success, TodoInf);
        }
        finally 
        {
            _todoInfRepository.ReleaseResource();
        }
    }

    private List<GetListTodoInfOutputItem> GetListTodoInfos(int hpId, int todoNo, int todoEdaNo, bool incDone)
    {
        List<GetListTodoInfOutputItem> result = new(_todoInfRepository.GetList(hpId, todoNo, todoEdaNo, incDone).Select(x => new GetListTodoInfOutputItem(
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
