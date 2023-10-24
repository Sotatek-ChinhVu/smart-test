using Domain.Models.MstItem;
using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.SaveConversion;

namespace Interactor.SuperSetDetail;

public class SaveConversionInteractor : ISaveConversionInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveConversionInteractor(ISuperSetDetailRepository superSetDetailRepository, IMstItemRepository mstItemRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveConversionOutputData Handle(SaveConversionInputData inputData)
    {
        try
        {
            if (!_mstItemRepository.CheckItemCd(inputData.ConversionItemCd))
            {
                return new SaveConversionOutputData(SaveConversionStatus.InvalidConversionItemCd);
            }
            else if (!_mstItemRepository.CheckItemCd(inputData.SourceItemCd))
            {
                return new SaveConversionOutputData(SaveConversionStatus.InvalidSourceItemCd);
            }
            if (_superSetDetailRepository.SaveConversionItemInf(inputData.HpId, inputData.UserId, inputData.ConversionItemCd, inputData.SourceItemCd))
            {
                return new SaveConversionOutputData(SaveConversionStatus.Successed);
            }
            return new SaveConversionOutputData(SaveConversionStatus.Failed);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }
}
