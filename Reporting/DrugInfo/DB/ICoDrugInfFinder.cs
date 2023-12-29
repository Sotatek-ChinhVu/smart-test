﻿using Domain.Common;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Reporting.DrugInfo.Model;

namespace Reporting.DrugInfo.DB;

public interface ICoDrugInfFinder : IRepositoryBase
{
    PathConf GetPathConf(int grpCode);

    DrugInfoModel GetBasicInfo(int hpId, long ptId, int orderDate = 0);

    List<OrderInfoModel> GetOrderByRaiinNo(long raiinNo);

    string GetYJCode(string itemCd);

    List<SingleDosageMstModel> GetSingleDosageMstCollection(int hpId, string unitName);

    TenMstModel GetTenMstModel(string itemCd);

    List<PiImage> GetProductImages(int hpId, string itemCd);

    (List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList) GetQueryDrugList(int hpId, List<string> itemCdList, int age, int gender);

    List<DrugInf> GetDrugInfo(int hpId, string itemCd, int age, int gender, List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList, List<SystemConfModel> allSystemConfigList);

    PathPicture GetDefaultPathPicture();
}
