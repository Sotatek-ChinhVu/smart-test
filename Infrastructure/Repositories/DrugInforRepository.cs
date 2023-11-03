using Domain.Constant;
using Domain.Models.DrugInfor;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class DrugInforRepository : RepositoryBase, IDrugInforRepository
{
    private readonly IConfiguration _configuration;

    public DrugInforRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        _configuration = configuration;
    }

    public DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd)
    {
        var queryItems = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                                                                && item.StartDate <= sinDate && item.EndDate >= sinDate);

        //Join
        var joinQuery = from m28DrugMst in NoTrackingDataContext.M28DrugMst
                        join tenItem in queryItems
                        on m28DrugMst.KikinCd equals tenItem.ItemCd
                        join m34DrugInfoMain in NoTrackingDataContext.M34DrugInfoMains
                        on m28DrugMst.YjCd equals m34DrugInfoMain.YjCd
                        join drugInf in NoTrackingDataContext.PiProductInfs
                        on m28DrugMst.YjCd equals drugInf.YjCd
                        where string.IsNullOrEmpty(itemCd) || tenItem.ItemCd == itemCd
                        select new
                        {
                            m28DrugMst,
                            tenItem,
                            m34DrugInfoMain,
                            drugInf
                        };

        // piczai pichou
        string pathServerDefault = _configuration["PathImageDrugFolder"] ?? string.Empty;

        var pathConfDb = NoTrackingDataContext.PathConfs.Where(p => p.GrpCd == PicImageConstant.GrpCodeDefault || p.GrpCd == PicImageConstant.GrpCodeCustomDefault).ToList();

        var pathConfDf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeDefault);

        // PicZai
        string defaultPicZai = "";
        string otherPicZai = "";
        string defaultPicHou = "";
        string otherPicHou = "";
        if (pathConfDf != null)
        {
            defaultPicZai = pathConfDf.Path + "zaikei/";
            defaultPicHou = pathConfDf.Path + "housou/";
        }
        else
        {
            defaultPicZai = pathServerDefault + "zaikei/";
            defaultPicHou = pathServerDefault + "housou/";
        }
        string customPathPicZai = "";
        string customPathPicHou = "";
        var customPathConf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeCustomDefault);
        if (customPathConf != null)
        {
            customPathPicZai = customPathConf.Path + "zaikei/";
            customPathPicHou = customPathConf.Path + "housou/";
        }
        else
        {
            customPathPicZai = pathServerDefault + "zaikei/";
            customPathPicHou = pathServerDefault + "housou/";
        }

        var imagePics = NoTrackingDataContext.PiImages.Where(pi => pi.ItemCd == itemCd && (pi.ImageType == PicImageConstant.PicZaikei || pi.ImageType == PicImageConstant.PicHousou)).ToList();

        var otherImagePicZai = imagePics.FirstOrDefault(pi => pi.ImageType == PicImageConstant.PicZaikei);
        if (otherImagePicZai != null)
        {
            otherPicZai = defaultPicZai + otherImagePicZai.FileName ?? string.Empty;
        }

        var otherImagePicHou = imagePics.FirstOrDefault(pi => pi.ImageType == PicImageConstant.PicHousou);
        if (otherImagePicHou != null)
        {
            otherPicHou = defaultPicHou + otherImagePicHou.FileName ?? string.Empty;
        }

        var rs = joinQuery.FirstOrDefault();
        if (rs != null)
        {
            return new DrugInforModel(rs.tenItem != null ? (rs.tenItem.Name ?? string.Empty) : string.Empty,
                                      rs.drugInf != null ? (rs.drugInf.GenericName ?? string.Empty) : string.Empty,
                                      rs.drugInf != null ? (rs.drugInf.Unit ?? string.Empty) : string.Empty,
                                      rs.drugInf != null ? (rs.drugInf.Maker ?? string.Empty) : string.Empty,
                                      rs.drugInf != null ? (rs.drugInf.Vender ?? string.Empty) : string.Empty,
                                      rs.tenItem != null ? rs.tenItem.KohatuKbn : 0,
                                      rs.tenItem != null ? rs.tenItem.Ten : 0,
                                      rs.tenItem != null ? (rs.tenItem.ReceUnitName ?? string.Empty) : string.Empty,
                                      rs.m34DrugInfoMain != null ? (rs.m34DrugInfoMain.Mark ?? string.Empty) : string.Empty,
                                      rs.tenItem != null ? rs.tenItem.YjCd ?? string.Empty : string.Empty,
                                      "",
                                      "",
                                      defaultPicZai,
                                      customPathPicZai,
                                      otherPicZai,
                                      defaultPicHou,
                                      customPathPicHou,
                                      otherPicHou,
                                      new List<string>(),
                                      new List<string>());
        }
        else
        {
            return new DrugInforModel();
        }
    }

    public List<SinrekiFilterMstModel> GetSinrekiFilterMstList(int hpId, int sinDate)
    {
        List<SinrekiFilterMstModel> result = new();
        var sinrekiMstList = NoTrackingDataContext.SinrekiFilterMsts.Where(item => item.HpId == hpId
                                                                                   && item.IsDeleted == 0)
                                                                    .ToList();
        var grpCdList = sinrekiMstList.Select(item => item.GrpCd).Distinct().ToList();
        var detailList = NoTrackingDataContext.SinrekiFilterMstDetails.Where(item => item.HpId == hpId
                                                                                     && item.IsDeleted == 0
                                                                                     && grpCdList.Contains(item.GrpCd))
                                                                      .ToList();
        var itemCdList = detailList.Select(item => item.ItemCd).Distinct().ToList();
        var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                     && item.IsDeleted == 0
                                                                     && item.ItemCd != null
                                                                     && itemCdList.Contains(item.ItemCd)
                                                                     && item.StartDate <= sinDate
                                                                     && sinDate <= item.EndDate)
                                                      .ToList();
        var kouiList = NoTrackingDataContext.SinrekiFilterMstKouis.Where(item => item.HpId == hpId
                                                                                 && grpCdList.Contains(item.GrpCd))
                                                                  .ToList();
        foreach (var mst in sinrekiMstList)
        {
            var sinrekiFilterMstKouiList = kouiList.Where(item => item.GrpCd == mst.GrpCd)
                                                   .Select(item => new SinrekiFilterMstKouiModel(
                                                                       item.GrpCd,
                                                                       item.SeqNo,
                                                                       item.KouiKbnId,
                                                                       item.IsDeleted == 0))
                                                   .ToList();

            var sinrekiFilterMstDetailList = detailList.Where(item => item.GrpCd == mst.GrpCd)
                                                     .Select(item => new SinrekiFilterMstDetailModel(
                                                                item.GrpCd,
                                                                item.ItemCd ?? string.Empty,
                                                                item.SortNo,
                                                                item.IsExclude == 1
                                                            ))
           var mstModel = new SinrekiFilterMstModel(mst.GrpCd,
                mst.Name ?? string.Empty,
                mst.SortNo,)
        }
        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
