﻿using Domain.Models.CalculateModel;
using UseCase.MedicalExamination.TrailAccounting;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class RunTraialCalculateResponse
    {
        public RunTraialCalculateResponse(List<SinMeiDataModel> sinMeiList, List<KaikeiInfDto> kaikeiInfList, List<CalcLogDataModel> calcLogList)
        {
            SinMeiList = sinMeiList;
            KaikeiInfList = kaikeiInfList;
            CalcLogList = calcLogList;
        }

        public List<SinMeiDataModel> SinMeiList { get; private set; }

        public List<KaikeiInfDto> KaikeiInfList { get; private set; }

        public List<CalcLogDataModel> CalcLogList { get; private set; }
    }
}
