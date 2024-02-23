﻿using Domain.Common;

namespace Domain.Models.Todo
{
    public interface ITodoInfRepository : IRepositoryBase
    {
        List<TodoInfModel> Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId);

        List<TodoInfModel> GetList(int hpId, int todoNo, int todoEdaNo, bool incDone, bool isDeleted = false, bool sortByPtNum = false);

        //Item1: TodoNo, Item2: TodoEdaNo, Item3: PtId
        bool CheckExist(int hpId, List<Tuple<int, int, long>> inputs);
    }
}