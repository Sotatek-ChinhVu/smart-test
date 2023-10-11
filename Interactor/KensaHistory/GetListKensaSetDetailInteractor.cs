using Domain.Models.KensaSet;
using Domain.Models.KensaSetDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.KensaHistory.GetListKensaSet;
using UseCase.KensaHistory.GetListKensaSetDetail;
using UseCase.MstItem.SearchTenMstItem;

namespace Interactor.KensaHistory
{
    public class GetListKensaSetDetailInteractor : IGetListKensaSetDetailInputPort
    {
        private readonly IKensaSetRepository _kensaSetRepository;

        public GetListKensaSetDetailInteractor(IKensaSetRepository kensaSetRepository)
        {
            _kensaSetRepository = kensaSetRepository;
        }
        public GetListKensaSetDetailOutputData Handle(GetListKensaSetDetailInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetListKensaSetDetailOutputData(new List<KensaSetDetailModel>(), SearchTenMstItemStatus.InValidHpId);
            }
            try
            {
                var result = new List<KensaSetDetailModel>();
                result = _kensaSetRepository.GetListKensaSetDetail(inputData.HpId, inputData.SetId);
                return new GetListKensaSetDetailOutputData(result, SearchTenMstItemStatus.Successed);
            }
            finally
            {
                _kensaSetRepository.ReleaseResource();
            }
        }
    }
}
