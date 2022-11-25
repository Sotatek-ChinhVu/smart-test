﻿using UseCase.Document.GetListDocCategory;

namespace EmrCloudApi.Tenant.Responses.Document;

public class DocCategoryDto
{
    public DocCategoryDto(DocCategoryOutputItem item)
    {
        HpId = item.HpId;
        CategoryCd = item.CategoryCd;
        CategoryName = item.CategoryName;
        SortNo = item.SortNo;
    }

    public int HpId { get; private set; }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public int SortNo { get; private set; }
}
