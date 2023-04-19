﻿using Domain.Common;

namespace Domain.Models.Todo
{
    public interface ITodoInfRepository : IRepositoryBase
    {
        void Upsert(List<TodoInfModel> upsertTodoList, int userId, int hpId);

        List<TodoInfModel> GetList(int hpId, int todoNo, int todoEdaNo, bool incDone);

        //Item1: TodoNo, Item2: TodoEdaNo, Item3: PtId
        bool CheckExist(List<Tuple<int, int, long>> inputs);
    }
}