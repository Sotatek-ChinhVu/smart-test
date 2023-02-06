using Domain.Models.UketukeSbtMst;
using Helper.Constants;
using static Helper.Constants.UketukeSbtMstConstant;
using UseCase.UketukeSbtMst.Upsert;

namespace Interactor.UketukeSbtMst;

public class UpsertUketukeSbtMstInteractor : IUpsertUketukeSbtMstInputPort
{
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;

    public UpsertUketukeSbtMstInteractor(IUketukeSbtMstRepository uketukeSbtMstRepository)
    {
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
    }

    public UpsertUketukeSbtMstOutputData Handle(UpsertUketukeSbtMstInputData inputdata)
    {
        try
        {
            if(inputdata.ToList() == null || inputdata.ToList().Count == 0)
            {
                return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.InputNoData);
            }

            foreach (var data in inputdata.ToList())
            {
                var status = data.Validation();
                if (status != ValidationStatus.Valid)
                {
                    return new UpsertUketukeSbtMstOutputData(ConvertStatus(status));
                } 
            }

            var checkInputKbnId = inputdata.UketukeSbtMsts.Where(x => x.KbnId > 0).Select(x => x.KbnId);
            if(checkInputKbnId.Count() != checkInputKbnId.Distinct().Count())
            {
                return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.UketukeListExistedInputData);
            }

            if(!_uketukeSbtMstRepository.CheckExistedKbnId(inputdata.UketukeSbtMsts.Where(x => x.KbnId > 0).Select(x => x.KbnId).ToList()))
            {
                return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.UketukeListInvalidExistedKbnId);
            }

            _uketukeSbtMstRepository.Upsert(inputdata.ToList(), inputdata.UserId, inputdata.HpId);

            return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.Success);
        }
        catch
        {
            return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.Failed);
        }
        finally
        {
            _uketukeSbtMstRepository.ReleaseResource();
        }
    }
    private static UpsertUketukeSbtMstStatus ConvertStatus(ValidationStatus status)
    {
        if (status == ValidationStatus.InvalidKbnId)
            return UpsertUketukeSbtMstStatus.InvalidKbnId;
        if (status == ValidationStatus.InvalidKbnName)
            return UpsertUketukeSbtMstStatus.InvalidKbnName;
        if (status == ValidationStatus.InvalidSortNo)
            return UpsertUketukeSbtMstStatus.InvalidSortNo;
        if (status == ValidationStatus.InvalidIsDeleted)
            return UpsertUketukeSbtMstStatus.InvalidIsDeleted;
        if (status == ValidationStatus.InputNoData)
            return UpsertUketukeSbtMstStatus.InputNoData;
        if (status == ValidationStatus.UketukeListInvalidNoExistedKbnId)
            return UpsertUketukeSbtMstStatus.UketukeListInvalidExistedKbnId;
        if (status == ValidationStatus.UketukeListExistedInputData)
            return UpsertUketukeSbtMstStatus.UketukeListExistedInputData;

        return UpsertUketukeSbtMstStatus.Success;
    }
}
