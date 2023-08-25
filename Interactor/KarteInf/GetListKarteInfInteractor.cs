﻿using Domain.Models.KarteInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
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
    private readonly IReceptionRepository _receptionRepository;
    public GetListKarteInfInteractor(IOptions<AmazonS3Options> optionsAccessor, IKarteInfRepository karteInfRepository, IPatientInforRepository patientInforRepository, IAmazonS3Service amazonS3Service, IReceptionRepository receptionRepository)
    {
        _karteInfRepository = karteInfRepository;
        _options = optionsAccessor.Value;
        _patientInforRepository = patientInforRepository;
        _amazonS3Service = amazonS3Service;
        _receptionRepository = receptionRepository;
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

        try
        {
            var karteInfModel = _karteInfRepository.GetList(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.IsDeleted, inputData.UserId).OrderBy(o => o.KarteKbn).ToList();
            List<KarteFileOutputItem> listFile = new();
            var listKarteFile = _karteInfRepository.GetListKarteFile(inputData.HpId, inputData.PtId, inputData.RaiinNo, false);
            if (listKarteFile.Any())
            {
                var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.PtId, 0, 0);
                List<string> listFolders = new()
                {
                    CommonConstants.Store,
                    CommonConstants.Karte
                };
                string path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);
                foreach (var file in listKarteFile)
                {
                    var fileLink = new StringBuilder();
                    fileLink.Append(_options.BaseAccessUrl);
                    fileLink.Append("/");
                    fileLink.Append(path);
                    fileLink.Append(file.LinkFile);
                    listFile.Add(new KarteFileOutputItem(file.IsSchema, fileLink.ToString()));
                }
            }
            var raiinStatus = _receptionRepository.GetStatusRaiinInf(inputData.HpId, inputData.RaiinNo, inputData.PtId);
            var isKarteExisted = !string.IsNullOrEmpty(karteInfModel.FirstOrDefault()?.Text) || listFile.Any();

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
                                                        isKarteExisted && raiinStatus >= 3,
                                                        GetListKarteInfStatus.Successed);
        }
        finally
        {
            _karteInfRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }
}
