using Domain.Models.KarteFilterMst;
using Helper.Constants;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace Interactor.KarteFilter;

public class SaveKarteFilterMstsInteractor : ISaveKarteFilterInputPort
{
    private int _userId = TempIdentity.UserId;


    private readonly IKarteFilterMstRepository _karteFilterMstRepository;
    public SaveKarteFilterMstsInteractor(IKarteFilterMstRepository karteFilterMstRepository)
    {
        _karteFilterMstRepository = karteFilterMstRepository;
    }

    public SaveKarteFilterOutputData Handle(SaveKarteFilterInputData inputData)
    {
        try
        {
            var listKarteFilterMstModels = new List<KarteFilterMstModel>();
            foreach (var item in inputData.saveKarteFilterMstModelInputs)
            {
                var karteFilterMstModel = new KarteFilterMstModel(
                        item.HpId,
                        item.UserId,
                        item.FilterId,
                        item.FilterName,
                        item.SortNo,
                        item.AutoApply,
                        item.IsDeleted,
                        new KarteFilterDetailModel
                            (
                                item.HpId,
                                item.UserId,
                                item.FilterId,
                                item.karteFilterDetailModel.BookMarkChecked,
                                item.karteFilterDetailModel.ListHokenId,
                                item.karteFilterDetailModel.ListKaId,
                                item.karteFilterDetailModel.ListUserId
                            )
                    );
                listKarteFilterMstModels.Add(karteFilterMstModel);
            }
            _karteFilterMstRepository.SaveList(listKarteFilterMstModels, _userId);
            return new SaveKarteFilterOutputData(SaveKarteFilterStatus.Successed);
        }
        catch
        {
            return new SaveKarteFilterOutputData(SaveKarteFilterStatus.Failed);
        }
    }
}
