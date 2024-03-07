﻿using Domain.Common;

namespace Domain.Models.GroupInf;

public interface IGroupInfRepository : IRepositoryBase
{
    IEnumerable<GroupInfModel> GetDataGroup(int hpId, long ptId);

    IEnumerable<GroupInfModel> GetAllByPtIdList(List<long> ptIdList, int hpId);

    List<GroupInfModel> GetAllByPtIdList(int hpId, List<long> ptIdList);
}
