﻿using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Repositories;
using System.Linq;
using System.Text;
using UseCase.NextOrder.Get;
using UseCase.PatientInfor.GetListPatient;

namespace Interactor.PatientInfor;

public class GetListPatientInfoInteractor : IGetListPatientInfoInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    public GetListPatientInfoInteractor(IPatientInforRepository patientInfoRepository)
    {
        _patientInforRepository = patientInfoRepository;
    }
    public GetListPatientInfoOutputData Handle(GetListPatientInfoInputData input)
    {
        try
        {
            if (input.HpId <= 0)
            {
                return new GetListPatientInfoOutputData(GetListPatientInfoStatus.InvalidHpId, new());
            }
            if(input.PtId <= 0)
            {
                return new GetListPatientInfoOutputData(GetListPatientInfoStatus.InvalidPtId, new());
            }
            if (input.PageIndex < 1)
            {
                return new GetListPatientInfoOutputData(GetListPatientInfoStatus.InvalidPageIndex, new());
            }
            if (input.PageSize < 0)
            {
                return new GetListPatientInfoOutputData(GetListPatientInfoStatus.InvalidPageSize, new());
            }
            var listPatientInfs = GetListPatientInfos(input.HpId, input.PtId, input.PageIndex, input.PageSize).Select(item => new GetListPatientInfoInputItem(
                                                                                     item.HpId, 
                                                                                     item.PtId, 
                                                                                     item.PtNum, 
                                                                                     item.KanaName, 
                                                                                     item.Name, 
                                                                                     item.Birthday, 
                                                                                     item.LastVisitDate)).ToList();

            return new GetListPatientInfoOutputData(GetListPatientInfoStatus.Success, listPatientInfs);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }

    private List<PatientInforModel> GetListPatientInfos(int hpId, long ptId, int pageIndex, int pageSize)
    {
        List<PatientInforModel> result = new(_patientInforRepository.SearchPatient(hpId, ptId, pageIndex, pageSize).Select(x => new PatientInforModel(
                                                    x.HpId,
                                                    x.PtId,
                                                    x.PtNum,
                                                    x.KanaName,
                                                    x.Name,
                                                    x.Birthday,
                                                    x.LastVisitDate)));
        return result;
    }
}