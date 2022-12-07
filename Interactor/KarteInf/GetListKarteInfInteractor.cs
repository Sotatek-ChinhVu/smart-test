using Domain.Models.KarteInfs;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.KarteInfs.GetLists;

namespace Interactor.KarteInfs;

public class GetListKarteInfInteractor : IGetListKarteInfInputPort
{
    private readonly IKarteInfRepository _karteInfRepository;
    private readonly AmazonS3Options _options;
    public GetListKarteInfInteractor(IOptions<AmazonS3Options> optionsAccessor, IKarteInfRepository karteInfRepository)
    {
        _karteInfRepository = karteInfRepository;
        _options = optionsAccessor.Value;
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
        var listKarteFile = _karteInfRepository.GetListKarteFile(inputData.HpId, inputData.PtId, inputData.RaiinNo);
        if (listKarteFile.Any())
        {
            foreach (var file in listKarteFile)
            {
                listFile.Add(_options.BaseAccessUrl + "/" + file.FileName);
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
