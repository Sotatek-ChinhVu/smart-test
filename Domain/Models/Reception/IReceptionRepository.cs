﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Reception
{
    public interface IReceptionRepository
    {
        ReceptionModel? Get(long raiinNo);
    }
}
