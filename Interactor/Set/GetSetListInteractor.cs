using Domain.Models.Set;
using Domain.Models.SetGeneration;
using UseCase.Set.GetList;

namespace Interactor.Set
{
    public class GetSetListInteractor : IGetSetListInputPort
    {
        private readonly ISetRepository _setRepository;
        private readonly ISetGenerationRepository _setGenerationRepository;
        public GetSetListInteractor(ISetRepository setRepository, ISetGenerationRepository setGenerationRepository)
        {
            _setRepository = setRepository;
            _setGenerationRepository = setGenerationRepository;
        }

        public GetSetListOutputData Handle(GetSetListInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new GetSetListOutputData(null, GetSetListStatus.InvalidHpId);
            }
            if (inputData.SetKbn < 0)
            {
                return new GetSetListOutputData(null, GetSetListStatus.InvalidSetKbn);
            }
            if (inputData.SetKbnEdaNo < 0)
            {
                return new GetSetListOutputData(null, GetSetListStatus.InvalidSetKbnEdaNo);
            }
            if (inputData.SinDate < 0)
            {
                return new GetSetListOutputData(null, GetSetListStatus.InvalidSinDate);
            }
            var generationId = _setGenerationRepository.GetGenerationId(inputData.HpId, inputData.SinDate);
            var sets = _setRepository.GetList(inputData.HpId, inputData.SetKbn, inputData.SetKbnEdaNo, inputData.TextSearch);
            var result = sets.Where(r => r.GenerationId == generationId);

            var output = BuildTreeSetKbn(result);

            return (output?.Count > 0) ? new GetSetListOutputData(BuildTreeSetKbn(result), GetSetListStatus.Successed) : new GetSetListOutputData(null, GetSetListStatus.NoData);

        }
        private List<GetSetListOutputItem> BuildTreeSetKbn(IEnumerable<SetMst>? datas)
        {
            List<GetSetListOutputItem> result = new List<GetSetListOutputItem>();
            var topNodes = datas?.Where(c => c.Level2 == 0 && c.Level3 == 0);
            if (topNodes?.Any() != true) { return result; }
            foreach (var item in topNodes)
            {
                var node = new GetSetListOutputItem
                (item.HpId, item.SetCd, item.SetKbn, item.SetKbnEdaNo, item.GenerationId, item.Level1, item.Level2, item.Level3, item.SetName, item.WeightKbn, item.Color, item.IsGroup, datas?.Where(c => c.Level1 == item.Level1 && c.Level2 != 0 && c.Level3 == 0)?
                                .Select(c => new GetSetListOutputItem
                                (c.HpId, c.SetCd, c.SetKbn, c.SetKbnEdaNo, c.GenerationId, c.Level1, c.Level2, c.Level3, c.SetName, c.WeightKbn, c.Color, c.IsGroup, datas.Where(m => m.Level3 != 0 && m.Level1 == item.Level1 && m.Level2 == c.Level2)?
                                    .Select(c => new GetSetListOutputItem
                                    (c.HpId, c.SetCd, c.SetKbn, c.SetKbnEdaNo, c.GenerationId, c.Level1, c.Level2, c.Level3, c.SetName, c.WeightKbn, c.Color, c.IsGroup, new List<GetSetListOutputItem>()
                                    )).ToList()
                                )).ToList()
                );
                result.Add(node);
            }
            return result;
        }

    }
}