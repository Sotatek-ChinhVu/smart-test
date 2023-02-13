﻿using Domain.Common;

namespace Domain.Models.Family;

public interface IFamilyRepository : IRepositoryBase
{
    List<FamilyModel> GetListFamily(int hpId, long ptId, int sinDate);
}
