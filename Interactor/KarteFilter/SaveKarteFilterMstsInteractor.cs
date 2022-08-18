using Domain.Models.KarteFilterMst;
using Helper.Constants;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace Interactor.KarteFilter;

public class SaveKarteFilterMstsInteractor : ISaveKarteFilterInputPort
{
    private readonly int _userId = TempIdentity.UserId;
    private readonly int _hpId = TempIdentity.HpId;
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
                            item.HpId,
                            item.UserId,
                            item.FilterId,
                            item.FilterName,
                            item.SortNo,
                            item.AutoApply,
                            item.IsDeleted,
                            new KarteFilterDetailModel(
                                item.HpId,
                                item.UserId,
                                item.FilterId,
                                item.KarteFilterDetailModel.BookMarkChecked,
                                item.KarteFilterDetailModel.ListHokenId,
                                item.KarteFilterDetailModel.ListKaId,
                                item.KarteFilterDetailModel.ListUserId
                            )
                        );
                    listKarteFilterMstModels.Add(karteFilterMstModel);
                }
                if (_karteFilterMstRepository.SaveList(listKarteFilterMstModels, _userId, _hpId))
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
    }
}
