using Domain.Models.HpMst;
using Domain.Models.KarteInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.Schema.SaveListImageTodayOrder;

namespace Interactor.Schema;

public class SaveListFileTodayOrderInteractor : ISaveListFileTodayOrderInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;
    private readonly IKarteInfRepository _setKbnMstRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IReceptionRepository _receptionRepository;

    public SaveListFileTodayOrderInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IKarteInfRepository setKbnMstRepository, IPatientInforRepository patientInforRepository, IHpInfRepository hpInfRepository, IReceptionRepository receptionRepository)
    {
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _setKbnMstRepository = setKbnMstRepository;
        _patientInforRepository = patientInforRepository;
        _hpInfRepository = hpInfRepository;
        _receptionRepository = receptionRepository;
    }

    public SaveListFileTodayOrderOutputData Handle(SaveListFileTodayOrderInputData input)
    {
        try
        {
            var ptInf = _patientInforRepository.GetById(input.HpId, input.PtId, 0, 0);
            var validateResponse = ValidateInput(input, ptInf);
            if (validateResponse.Item1 != SaveListFileTodayOrderStatus.ValidateSuccess)
            {
                return new SaveListFileTodayOrderOutputData(validateResponse.Item1);
            }
            List<KarteImgInfModel> listImageSaveTemps = new();
            var listFileItems = validateResponse.Item2;
            if (listFileItems.Any())
            {
                var listFolders = new List<string>() {
                                                        CommonConstants.Store,
                                                        CommonConstants.Karte,
                                                     };

                string path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);

                foreach (var item in listFileItems)
                {
                    var responseUpload = _amazonS3Service.UploadObjectAsync(path, item.FileName, item.StreamImage);
                    var linkImage = responseUpload.Result;
                    if (linkImage.Length > 0)
                    {
                        listImageSaveTemps.Add(new KarteImgInfModel(
                                            input.HpId,
                                            input.PtId,
                                            input.RaiinNo,
                                            linkImage.Replace(_options.BaseAccessUrl + "/", string.Empty),
                                            string.Empty
                                      ));
                    }
                }
                _setKbnMstRepository.SaveListImageKarteImgTemp(listImageSaveTemps);
                return new SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus.Successed);
            }
            return new SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus.Failed);
        }
        catch (Exception)
        {
            return new SaveListFileTodayOrderOutputData(SaveListFileTodayOrderStatus.Failed);
        }
    }

    private Tuple<SaveListFileTodayOrderStatus, List<FileItem>> ValidateInput(SaveListFileTodayOrderInputData input, PatientInforModel? ptInf)
    {
        List<FileItem> listFileItems = new();
        if (ptInf == null)
        {
            return Tuple.Create(SaveListFileTodayOrderStatus.InvalidPtId, listFileItems);
        }
        else if (!_hpInfRepository.CheckHpId(input.HpId))
        {
            return Tuple.Create(SaveListFileTodayOrderStatus.InvalidHpId, listFileItems);
        }
        else if (!_receptionRepository.CheckListNo(new List<long>() { input.RaiinNo }))
        {
            return Tuple.Create(SaveListFileTodayOrderStatus.InvalidRaiinNo, listFileItems);
        }
        if (input.ListImages.Any())
        {
            foreach (var image in input.ListImages)
            {
                string fileName = _amazonS3Service.GetUniqueFileNameKey(image.FileName);
                if (image.StreamImage.Length > 0)
                {
                    listFileItems.Add(new FileItem(fileName, image.StreamImage));
                }
                else
                {
                    return Tuple.Create(SaveListFileTodayOrderStatus.InvalidFileImage, listFileItems);
                }
            }
        }
        else
        {
            return Tuple.Create(SaveListFileTodayOrderStatus.InvalidFileImage, listFileItems);
        }
        return Tuple.Create(SaveListFileTodayOrderStatus.ValidateSuccess, listFileItems);
    }
}
