﻿using Domain.Models.DrugInfor;

namespace EmrCloudApi.Responses.DrugInfor.Dto;

public class SinrekiFilterMstDetailDto
{
    public SinrekiFilterMstDetailDto(SinrekiFilterMstDetailModel model)
    {
        Id = model.Id;
        GrpCd = model.GrpCd;
        ItemCd = model.ItemCd;
        ItemName = model.ItemName;
        SortNo = model.SortNo;
        IsExclude = model.IsExclude;
    }
    public long Id { get; private set; }

    public int GrpCd { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public int SortNo { get; private set; }

    public bool IsExclude { get; private set; }
}