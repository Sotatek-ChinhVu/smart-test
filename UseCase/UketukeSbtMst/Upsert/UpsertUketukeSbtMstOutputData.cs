using Domain.Models.UketukeSbtMst;
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.Upsert;

public class UpsertUketukeSbtMstOutputData : IOutputData
{
    public UpsertUketukeSbtMstStatus Status { get; private set; }

    public UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus status) 
    {
        Status= status;
    }
}
