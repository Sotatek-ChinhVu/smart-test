using Domain.Models.UketukeSbtMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.Upsert;

public class UpsertUketukeSbtMstInputData : IInputData<UpsertUketukeSbtMstOutputData>
{
    public UpsertUketukeSbtMstInputData(List<UketukeSbtMstModel> uketukeSbtMsts, int userId, int hpId) 
    {
        UketukeSbtMsts = uketukeSbtMsts;
        UserId = userId;
        HpId = hpId;
    }

    public List<UketukeSbtMstModel> UketukeSbtMsts { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<UketukeSbtMstModel> ToList()
    {
        return UketukeSbtMsts;
    }
}
