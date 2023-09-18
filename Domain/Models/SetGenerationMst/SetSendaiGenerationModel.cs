﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SetGenerationMst
{
    public class SetSendaiGenerationModel
    {
        public SetSendaiGenerationModel(int hpId, int generationId, int startDate, string startDateStr, int endDate, string endDateStr, int indexRow)
        {
            HpId = hpId;
            GenerationId = generationId;
            StartDate = startDate;
            StartDateStr = startDateStr;
            EndDate = endDate;
            EndDateStr = endDateStr;
            IndexRow = indexRow;
        }

        public int HpId { get; private set; }
        public int GenerationId { get; private set; }
        public int StartDate { get; private set; }
        public string StartDateStr { get; private set; }
        public int EndDate { get; private set; }
        public string EndDateStr { get; private set; }
        public int IndexRow { get; private set; }
    }
}
