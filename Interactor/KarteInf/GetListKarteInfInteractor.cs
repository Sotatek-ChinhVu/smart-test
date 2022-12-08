using Domain.Models.KarteInfs;
using Domain.Models.PatientInfor;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.KarteInf.GetList;

namespace Interactor.KarteInfs;

public class GetListKarteInfInteractor : IGetListKarteInfInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IKarteInfRepository _karteInfRepository;
    private readonly AmazonS3Options _options;
    private readonly IPatientInforRepository _patientInforRepository;
    public GetListKarteInfInteractor(IOptions<AmazonS3Options> optionsAccessor, IKarteInfRepository karteInfRepository, IPatientInforRepository patientInforRepository, IAmazonS3Service amazonS3Service)
    {
        _karteInfRepository = karteInfRepository;
        _options = optionsAccessor.Value;
        _patientInforRepository = patientInforRepository;
        _amazonS3Service = amazonS3Service;
    }

    public GetListKarteInfOutputData Handle(GetListKarteInfInputData inputData)
    {
        if (inputData.PtId <= 0)
        {
            return new GetListKarteInfOutputData(GetListKarteInfStatus.InvalidPtId);
        }
        if (inputData.RaiinNo <= 0)
        {
            return new GetListKarteInfOutputData(GetListKarteInfStatus.InvalidRaiinNo);
        }
        if (inputData.SinDate <= 0)
        {
            return new GetListKarteInfOutputData(GetListKarteInfStatus.InvalidSinDate);
        }

        var karteInfModel = _karteInfRepository.GetList(inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.IsDeleted).OrderBy(o => o.KarteKbn).ToList();
        if (karteInfModel == null || karteInfModel.Count == 0)
        {
            return new GetListKarteInfOutputData(GetListKarteInfStatus.NoData);
        }

        List<string> listFile = new();
        var listKarteFile = _karteInfRepository.GetListKarteFile(inputData.HpId, inputData.PtId, inputData.RaiinNo, false);
        if (listKarteFile.Any())
        {
            var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0);
            List<string> listFolders = new();
            listFolders.Add(CommonConstants.Store);
            listFolders.Add(CommonConstants.Karte);
            string path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);
            var fileName = new StringBuilder();
            fileName.Append(_options.BaseAccessUrl);
            fileName.Append("/");
            fileName.Append(path);
            foreach (var file in listKarteFile)
            {
                fileName.Append(file);
                listFile.Add(fileName.ToString());
            }
        }

        return new GetListKarteInfOutputData(karteInfModel.Select(k =>
                                                    new GetListKarteInfOuputItem(
                                                        k.HpId,
                                                        k.RaiinNo,
                                                        k.KarteKbn,
                                                        k.SeqNo,
                                                        k.PtId,
                                                        k.SinDate,
                                                        k.Text,
                                                        k.IsDeleted,
                                                        k.RichText
                                                    )).ToList(),
                                                    listFile,
                                                    GetListKarteInfStatus.Successed);
    }
}
