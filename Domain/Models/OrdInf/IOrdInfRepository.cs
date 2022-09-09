﻿namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInfModel ord);

        OrdInfModel Read(int ordId);

        void Update(OrdInfModel ord);

        void Delete(int ordId);

        IEnumerable<OrdInfModel> GetList(long ptId, long raiinNo, int sinDate, bool isDeleted);

        IEnumerable<OrdInfModel> GetList(long ptId, int hpId, long raiinNo);
        IEnumerable<ApproveInfModel> GetApproveInf(int hpId, long ptId, bool isDeleted);
    }
}
