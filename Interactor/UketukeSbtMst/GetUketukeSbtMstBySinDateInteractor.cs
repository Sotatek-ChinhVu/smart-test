﻿using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using UseCase.UketukeSbtMst.GetBySinDate;

namespace Interactor.UketukeSbtMst;

public class GetUketukeSbtMstBySinDateInteractor : IGetUketukeSbtMstBySinDateInputPort
{
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;
    private readonly IUketukeSbtDayInfRepository _uketukeSbtDayInfRepository;

    public GetUketukeSbtMstBySinDateInteractor(IUketukeSbtMstRepository uketukeSbtMstRepository,
        IUketukeSbtDayInfRepository uketukeSbtDayInfRepository)
    {
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
        _uketukeSbtDayInfRepository = uketukeSbtDayInfRepository;
    }

    public GetUketukeSbtMstBySinDateOutputData Handle(GetUketukeSbtMstBySinDateInputData input)
    {
        if (input.SinDate <= 0)
        {
            return new GetUketukeSbtMstBySinDateOutputData(GetUketukeSbtMstBySinDateStatus.InvalidSinDate);
        }

        var receptionType = GetReceptionTypeBySinDate(input.SinDate);
        var status = receptionType is null ? GetUketukeSbtMstBySinDateStatus.NotFound : GetUketukeSbtMstBySinDateStatus.Success;
        return new GetUketukeSbtMstBySinDateOutputData(status, receptionType);
    }

    private UketukeSbtMstModel? GetReceptionTypeBySinDate(int sinDate)
    {
        var uketukeSbtMsts = _uketukeSbtMstRepository.GetList();
        var uketukeSbtDayInfs = _uketukeSbtDayInfRepository.GetListBySinDate(sinDate);
        // Inner join
        var mstBySinDateQuery =
            from mst in uketukeSbtMsts
            join dayInf in uketukeSbtDayInfs on mst.KbnId equals dayInf.UketukeSbt
            where dayInf.SinDate == sinDate
            orderby dayInf.SeqNo descending
            select mst;

        var mstBySinDate = mstBySinDateQuery.FirstOrDefault();
        if (mstBySinDate is not null)
        {
            return mstBySinDate;
        }

        var firstMst = uketukeSbtMsts.OrderBy(u => u.SortNo).FirstOrDefault();
        if (firstMst is null)
        {
            return null;
        }

        // Create or update the related UketukeSbtDayInf
        _uketukeSbtDayInfRepository.Upsert(sinDate, firstMst.KbnId, 0);

        return firstMst;
    }
}
