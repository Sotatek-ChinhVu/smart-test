﻿namespace Domain.Models.KarteKbnMst
{
    public interface IKarteKbnMstRepository
    {
        List<KarteKbnMstModel> GetList(int hpId, bool isDeleted);

        bool CheckKarteKbn(int karteKbn);

        List<KarteKbnMstModel> GetCheckKarteKbns(List<int> karteKbns);
    }
}
