﻿using Domain.Common;
using Reporting.NameLabel.Models;

namespace Reporting.NameLabel.DB
{
    public interface ICoNameLabelFinder : IRepositoryBase
    {
        CoPtInfModel FindPtInf(long ptId);
    }
}