using Domain.Models.KarteInfs;
using UseCase.KarteInf.ConvertTextToRichText;

namespace Interactor.KarteInf;

public class ConvertTextToRichTextInteractor : IConvertTextToRichTextInputPort
{
    private readonly IKarteInfRepository _karteInfRepository;

    public ConvertTextToRichTextInteractor(IKarteInfRepository karteInfRepository)
    {
        _karteInfRepository = karteInfRepository;
    }

    public ConvertTextToRichTextOutputData Handle(ConvertTextToRichTextInputData inputData)
    {
        try
        {
            if (_karteInfRepository.ConvertTextToRichText(inputData.HpId, inputData.PtId) > 0)
            {
                return new ConvertTextToRichTextOutputData(ConvertTextToRichTextStatus.Successed);
            }
            return new ConvertTextToRichTextOutputData(ConvertTextToRichTextStatus.InvalidPtId);
        }
        catch (Exception)
        {
            return new ConvertTextToRichTextOutputData(ConvertTextToRichTextStatus.Failed);
        }
        finally
        {
            _karteInfRepository.ReleaseResource();
        }
    }
}
