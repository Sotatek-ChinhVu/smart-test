using Domain.Models.KarteFilterMst;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace Interactor.KarteFilter;

public class SaveKarteFilterMstsInteractor : ISaveKarteFilterInputPort
{
    private readonly IKarteFilterMstRepository _karteFilterMstRepository;
    public SaveKarteFilterMstsInteractor(IKarteFilterMstRepository karteFilterMstRepository)
    {
        _karteFilterMstRepository = karteFilterMstRepository;
    }

    public SaveKarteFilterOutputData Handle(SaveKarteFilterInputData inputData)
    {
        try
        {
            if (inputData.SaveKarteFilterMstModelInputs != null)
            {
                var listKarteFilterMstModels = new List<KarteFilterMstModel>();
                foreach (var item in inputData.SaveKarteFilterMstModelInputs)
                {
                    var karteFilterMstModel = new KarteFilterMstModel(
                            inputData.HpId,
                            inputData.UserId,
                            item.FilterId,
                            item.FilterName,
                            item.SortNo,
                            item.AutoApply,
                            item.IsDeleted,
                            new KarteFilterDetailModel(
                                inputData.HpId,
                                inputData.UserId,
                                item.FilterId,
                                item.KarteFilterDetailModel.BookMarkChecked,
                                item.KarteFilterDetailModel.ListHokenId,
                                item.KarteFilterDetailModel.ListKaId,
                                item.KarteFilterDetailModel.ListUserId
                            )
                        );
                    listKarteFilterMstModels.Add(karteFilterMstModel);
                }
                if (_karteFilterMstRepository.SaveList(listKarteFilterMstModels, inputData.UserId, inputData.HpId))
                {
                    return new SaveKarteFilterOutputData(SaveKarteFilterStatus.Successed);
                }
            }
            return new SaveKarteFilterOutputData(SaveKarteFilterStatus.Failed);
        }
        catch
        {
            return new SaveKarteFilterOutputData(SaveKarteFilterStatus.Failed);
        }
        finally
        {
            _karteFilterMstRepository.ReleaseResource();
        }
    }
}
