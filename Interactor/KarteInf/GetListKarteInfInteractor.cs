using Domain.Models.KarteInfs;
using System.Text;
using UseCase.KarteInfs.GetLists;

namespace Interactor.KarteInfs
{
    public class GetListKarteInfInteractor : IGetListKarteInfInputPort
    {
        private readonly IKarteInfRepository _karteInfRepository;
        public GetListKarteInfInteractor(IKarteInfRepository karteInfRepository)
        {
            _karteInfRepository = karteInfRepository;
        }

        public GetListKarteInfOutputData Handle(GetListKarteInfInputData inputData)
        {
            if (inputData.PtId <= 0)
            {
                return new GetListKarteInfOutputData(new List<GetListKarteInfOuputItem>(), GetListKarteInfStatus.InvalidPtId);
            }
            if (inputData.RaiinNo <= 0)
            {
                return new GetListKarteInfOutputData(new List<GetListKarteInfOuputItem>(), GetListKarteInfStatus.InvalidRaiinNo);
            }
            if (inputData.SinDate <= 0)
            {
                return new GetListKarteInfOutputData(new List<GetListKarteInfOuputItem>(), GetListKarteInfStatus.InvalidSinDate);
            }

            var karteInfModel = _karteInfRepository.GetList(inputData.PtId, inputData.RaiinNo, inputData.SinDate);
            if (karteInfModel == null || karteInfModel.Count == 0)
            {
                return new GetListKarteInfOutputData(new List<GetListKarteInfOuputItem>(), GetListKarteInfStatus.NoData);
            }

            return new GetListKarteInfOutputData(karteInfModel.Select(k => new GetListKarteInfOuputItem(
                k.HpId,
                k.RaiinNo,
                k.KarteKbn,
                k.SeqNo,
                k.PtId,
                k.SinDate,
                k.Text,
                k.IsDeleted,
                k.RichText
             )).ToList(), GetListKarteInfStatus.Successed);
        }
    }
}
