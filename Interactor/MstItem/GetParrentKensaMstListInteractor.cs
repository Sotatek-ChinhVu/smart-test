using Domain.Models.KensaIrai;
using UseCase.MstItem.GetParrentKensaMst;

namespace Interactor.MstItem
{
    public class GetParrentKensaMstListInteractor : IGetParrentKensaMstInputPort
    {
        private readonly IKensaMstFinder _kensaMstFinder;

        public GetParrentKensaMstListInteractor(IKensaMstFinder kensaMstFinder)
        {
            _kensaMstFinder = kensaMstFinder;
        }
        public GetParrentKensaMstOutputData Handle(GetParrentKensaMstInputData inputData)
        {
            try
            {
                var data = _kensaMstFinder.GetParrentKensaMstModels(inputData.HpId, inputData.KeyWord);
                if (data.Count == 0)
                {
                    return new GetParrentKensaMstOutputData(new(), GetParrentKensaMstStatus.NoData);
                }
                else
                {
                    return new GetParrentKensaMstOutputData(data, GetParrentKensaMstStatus.Successful);
                }
            }
            finally
            {
                _kensaMstFinder.ReleaseResource();
            }
        }
    }
}
