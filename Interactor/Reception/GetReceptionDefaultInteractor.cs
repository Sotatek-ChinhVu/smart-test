﻿using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Reception.GetReceptionDefault;

namespace Interactor.Reception
{
    public class GetReceptionDefaultInteractor : IGetReceptionDefaultInputPort
    {
        private readonly IReceptionRepository _receptionRepository;
        public GetReceptionDefaultInteractor(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }

        public GetReceptionDefaultOutputData Handle(GetReceptionDefaultInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetReceptionDefaultOutputData(new ReceptionModel(), GetReceptionDefaultStatus.InvalidHpId);
                }

                if (inputData.PtId < 0)
                {
                    return new GetReceptionDefaultOutputData(new ReceptionModel(), GetReceptionDefaultStatus.InvalidPtId);
                }

                if (inputData.Sindate < 0)
                {
                    return new GetReceptionDefaultOutputData(new ReceptionModel(), GetReceptionDefaultStatus.InvalidSindate);
                }

                if (inputData.DefaultDoctorSetting < 0)
                {
                    return new GetReceptionDefaultOutputData(new ReceptionModel(), GetReceptionDefaultStatus.InvalidDefautDoctorSetting);
                }

                var data = _receptionRepository.GetDataDefaultReception(inputData.HpId, inputData.PtId, inputData.Sindate, inputData.DefaultDoctorSetting);
                return new GetReceptionDefaultOutputData(data, GetReceptionDefaultStatus.Successed);
            }
            catch
            {
                return new GetReceptionDefaultOutputData(new ReceptionModel(), GetReceptionDefaultStatus.Failed);
            }
        }
    }
}
