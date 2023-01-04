using Domain.Models.GroupInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.GroupInf.GetList;

namespace Interactor.GrpInf
{
    public class GroupInfInteractor : IGetListGroupInfInputPort
    {
        private readonly IGroupInfRepository _groupInfRepository;
        public GroupInfInteractor(IGroupInfRepository groupInfRepository)
        {
            _groupInfRepository = groupInfRepository;
        }

        public GetListGroupInfOutputData Handle(GetListGroupInfInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetListGroupInfOutputData(new List<GroupInfModel>(), GetListGroupInfSatus.InValidHpId);
                }
                if (inputData.PtId < 0)
                {
                    return new GetListGroupInfOutputData(new List<GroupInfModel>(), GetListGroupInfSatus.InvalidPtId);
                }

                var data = _groupInfRepository.GetDataGroup(inputData.HpId, inputData.PtId);

                return new GetListGroupInfOutputData(data.ToList(), GetListGroupInfSatus.Successed);
            }
            finally
            {
                _groupInfRepository.ReleaseResource();
            }
        }
    }
}
