using Domain.Models.KensaInfDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.KensaInfDetail.GetListByPtIdAndSinDate;

namespace Interactor.KensaInfDetail
{
    internal class GetKensaInfDetailInteractor : IGetListKensaInfDetailByPtIdAndSinDateInputPort
    {
        private readonly IKensaInfDetailRepository _kensaInfDetailRepository;
        public GetKensaInfDetailInteractor(IKensaInfDetailRepository kensaInfDetailRepository)
        {
            _kensaInfDetailRepository = kensaInfDetailRepository;
        }
        public GetListKensaInfDetailByPtIdAndSinDateOutputData Handle(GetListKensaInfDetailByPtIdAndSinDateInputData inputData)
        {
            var datas = _kensaInfDetailRepository.GetListByPtIdAndSinDate(inputData.PtId, inputData.SinDate);
            return new GetListKensaInfDetailByPtIdAndSinDateOutputData(datas);
        }
    }
}
