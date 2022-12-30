﻿using Domain.Common;

namespace Domain.Models.Santei;

public interface ISanteiInfRepository : IRepositoryBase
{
    List<SanteiInfModel> GetListSanteiInf(int hpId, long ptId, int sinDate);

    List<string> GetListSanteiByomeis(int hpId, long ptId, int sinDate, int hokenPid);

    bool SaveListSanteiInf(int hpId, int userId, SanteiInfModel model);

    bool SaveListSanteiInfDetail(int hpId, int userId, SanteiInfDetailModel model);
}
