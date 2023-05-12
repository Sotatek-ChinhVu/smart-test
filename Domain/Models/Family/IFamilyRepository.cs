﻿using Domain.Common;

namespace Domain.Models.Family;

public interface IFamilyRepository : IRepositoryBase
{
    List<FamilyModel> GetFamilyList(int hpId, long ptId, int sinDate);

    List<FamilyModel> GetFamilyReverserList(int hpId, long familyPtId, List<long> ptIdInputList);

    bool SaveFamilyList(int hpId, int userId, List<FamilyModel> familyList);

    List<FamilyModel> GetListByPtId(int hpId, long ptId);

    bool CheckExistFamilyRekiList(int hpId, List<long> familyRekiIdList);

    List<RaiinInfModel> GetRaiinInfListByPtId(int hpId, long ptId);

    List<FamilyModel> GetFamilyListByPtId(int hpId, long ptId, int sinDate);

    List<FamilyModel> GetMaybeFamilyList(int hpId, long ptId, int sinDate);
}
