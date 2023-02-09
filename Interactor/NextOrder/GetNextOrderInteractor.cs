using Domain.Models.Insurance;
using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.NextOrder.Get;

namespace Interactor.NextOrder;

public class GetNextOrderInteractor : IGetNextOrderInputPort
{
    private readonly INextOrderRepository _nextOrderRepository;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;

    public GetNextOrderInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, INextOrderRepository nextOrderRepository, IInsuranceRepository insuranceRepository, IPatientInforRepository patientInforRepository)
    {
        _nextOrderRepository = nextOrderRepository;
        _insuranceRepository = insuranceRepository;
        _options = optionsAccessor.Value;
        _amazonS3Service = amazonS3Service;
        _patientInforRepository = patientInforRepository;
    }

    public GetNextOrderOutputData Handle(GetNextOrderInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetNextOrderOutputData(GetNextOrderStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new GetNextOrderOutputData(GetNextOrderStatus.InvalidPtId);
            }
            if (inputData.RsvkrtNo <= 0)
            {
                return new GetNextOrderOutputData(GetNextOrderStatus.InvalidRsvkrtNo);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetNextOrderOutputData(GetNextOrderStatus.InvalidSinDate);
            }
            if (inputData.UserId <= 0)
            {
                return new GetNextOrderOutputData(GetNextOrderStatus.InvalidUserId);
            }

            var insurances = _insuranceRepository.GetListHokenPattern(inputData.HpId, inputData.PtId, false).ToList();

            var byomeis = _nextOrderRepository.GetByomeis(inputData.HpId, inputData.PtId, inputData.RsvkrtNo, inputData.RsvkrtKbn);
            var orderInfs = _nextOrderRepository.GetOrderInfs(inputData.HpId, inputData.PtId, inputData.RsvkrtNo, inputData.SinDate, inputData.UserId);
            var karteInf = _nextOrderRepository.GetKarteInf(inputData.HpId, inputData.PtId, inputData.RsvkrtNo);
            var listNextOrderFiles = GetListNextOrderFile(inputData.HpId, inputData.PtId, inputData.RsvkrtNo)
                                                            .Select(item => new NextOrderFileInfItem(item))
                                                            .ToList();

            var orderInfItems = orderInfs.Select(o => new RsvKrtOrderInfItem(o));

            var hokenOdrInfs = orderInfs?
           .GroupBy(odr => odr.HokenPid)
           .Select(grp => grp.FirstOrDefault())
           .ToList();
            if (byomeis.Count == 0 && karteInf.HpId == 0 && karteInf.PtId == 0 && karteInf.SeqNo == 0 && (hokenOdrInfs == null || hokenOdrInfs.Count == 0))
            {
                return new GetNextOrderOutputData(GetNextOrderStatus.NoData);
            }
            var byomeiItems = byomeis.Select(b => new RsvKrtByomeiItem(b)).ToList();

            var obj = new object();
            var tree = new GetNextOrderOutputData(new(), karteInf, byomeiItems, listNextOrderFiles, GetNextOrderStatus.Successed);
            if (hokenOdrInfs?.Count > 0)
            {
                Parallel.ForEach(hokenOdrInfs.Select(h => h?.HokenPid), hokenId =>
                {
                    var insuance = insurances.FirstOrDefault(i => i.HokenPid == hokenId);
                    var groupHoken = new GroupHokenItem(new List<GroupOdrItem>(), hokenId ?? 0, insuance?.HokenName ?? string.Empty);
                    // Find By Group
                    var groupOdrInfs = orderInfItems?.Where(odr => odr.HokenPid == hokenId)
                        .GroupBy(odr => new
                        {
                            odr.HokenPid,
                            odr.GroupOdrKouiKbn,
                            odr.InoutKbn,
                            odr.SyohoSbt,
                            odr.SikyuKbn,
                            odr.TosekiKbn,
                            odr.SanteiKbn
                        })
                        .Select(grp => grp.FirstOrDefault())
                        .ToList();
                    if (groupOdrInfs?.Any() == true)
                    {
                        var objGroupOdrInf = new object();
                        Parallel.ForEach(groupOdrInfs, groupOdrInf =>
                        {
                            var odrInfs = orderInfItems?.Where(odrInf => odrInf.HokenPid == hokenId
                                                    && odrInf.GroupOdrKouiKbn == groupOdrInf?.GroupOdrKouiKbn
                                                    && odrInf.InoutKbn == groupOdrInf?.InoutKbn
                                                    && odrInf.SyohoSbt == groupOdrInf?.SyohoSbt
                                                    && odrInf.SikyuKbn == groupOdrInf?.SikyuKbn
                                                    && odrInf.TosekiKbn == groupOdrInf?.TosekiKbn
                                                    && odrInf.SanteiKbn == groupOdrInf?.SanteiKbn)
                                                .ToList();

                            var group = new GroupOdrItem(insuance?.HokenName ?? string.Empty, new List<RsvKrtOrderInfItem>(), hokenId ?? 0); ;
                            lock (objGroupOdrInf)
                            {
                                if (odrInfs?.Count > 0)
                                    group.OdrInfs.AddRange(odrInfs);
                                groupHoken.GroupOdrItems.Add(group);
                            }
                        });
                    }
                    lock (obj)
                    {
                        tree.GroupHokenItems.Add(groupHoken);
                    }
                });
            }

            return tree;
        }
        catch
        {
            return new GetNextOrderOutputData(GetNextOrderStatus.Failed);
        }
        finally
        {
            _insuranceRepository.ReleaseResource();
            _nextOrderRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }
    private List<NextOrderFileInfModel> GetListNextOrderFile(int hpId, long ptId, long rsvkrtNo)
    {
        var nextOrderFiles = _nextOrderRepository.GetNextOrderFiles(hpId, ptId, rsvkrtNo);
        List<NextOrderFileInfModel> result = new();
        var ptInf = _patientInforRepository.GetById(hpId, ptId, 0, 0);
        List<string> listFolders = new();
        listFolders.Add(CommonConstants.Store);
        listFolders.Add(CommonConstants.Karte);
        listFolders.Add(CommonConstants.NextPic);
        string path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);
        foreach (var file in nextOrderFiles)
        {
            var fileName = new StringBuilder();
            fileName.Append(_options.BaseAccessUrl);
            fileName.Append("/");
            fileName.Append(path);
            fileName.Append(file.LinkFile);
            result.Add(new NextOrderFileInfModel(file.IsSchema, fileName.ToString()));
        }
        return result;
    }
}
