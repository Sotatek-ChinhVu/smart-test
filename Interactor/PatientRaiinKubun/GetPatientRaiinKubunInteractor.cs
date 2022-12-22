using Domain.Models.PatientRaiinKubun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientRaiinKubun.Get;

namespace Interactor.PatientRaiinKubun
{
    public class GetPatientRaiinKubunInteractor : IGetPatientRaiinKubunInputPort
    {
        private readonly IPatientRaiinKubunReponsitory _patientRaiinKubunReponsitory;
        public GetPatientRaiinKubunInteractor(IPatientRaiinKubunReponsitory patientRaiinKubunReponsitory)
        {
            _patientRaiinKubunReponsitory = patientRaiinKubunReponsitory;
        }

        public GetPatientRaiinKubunOutputData Handle(GetPatientRaiinKubunInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetPatientRaiinKubunOutputData(new List<PatientRaiinKubunModel>(), GetPatientRaiinKubunStatus.InvalidPtId);
                }

                if (inputData.PtId < 0)
                {
                    return new GetPatientRaiinKubunOutputData(new List<PatientRaiinKubunModel>(), GetPatientRaiinKubunStatus.InvalidPtId);
                }

                if (inputData.RaiinNo < 0)
                {
                    return new GetPatientRaiinKubunOutputData(new List<PatientRaiinKubunModel>(), GetPatientRaiinKubunStatus.InvalidRaiinNo);
                }

                if (inputData.SinDate < 0)
                {
                    return new GetPatientRaiinKubunOutputData(new List<PatientRaiinKubunModel>(), GetPatientRaiinKubunStatus.InvalidSinDate);
                }

                var data = _patientRaiinKubunReponsitory.GetPatientRaiinKubun(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);

                return new GetPatientRaiinKubunOutputData(data.ToList(), GetPatientRaiinKubunStatus.Successed);
            }
            finally
            {
                _patientRaiinKubunReponsitory.ReleaseResource();
            }
        }
    }
}
