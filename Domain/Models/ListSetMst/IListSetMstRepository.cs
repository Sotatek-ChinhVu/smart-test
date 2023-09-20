﻿using Domain.Common;

namespace Domain.Models.ListSetMst
{
    public interface IListSetMstRepository : IRepositoryBase
    {
        int GetGenerationId(int sinDate);
        List<ListSetMstModel> GetListSetMst(int hpId, int setKbn, int generationId);
    }
}