
using Domain.Models.SetGenerationMst;
using Domain.Models.SetMst;
using UseCase.SetMst.GetList;

namespace Interactor.SetMst
{
    public class GetSetMstListInteractor : IGetSetMstListInputPort
    {
        private readonly ISetMstRepository _setRepository;
        private readonly ISetGenerationMstRepository _setGenerationRepository;
        public GetSetMstListInteractor(ISetMstRepository setRepository, ISetGenerationMstRepository setGenerationRepository)
        {
            _setRepository = setRepository;
            _setGenerationRepository = setGenerationRepository;
        }

        public GetSetMstListOutputData Handle(GetSetMstListInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidHpId);
            }
            if (inputData.SetKbn < 0)
            {
                return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidSetKbn);
            }
            if (inputData.SetKbnEdaNo < 0)
            {
                return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidSetKbnEdaNo);
            }
            if (inputData.SinDate < 0)
            {
                return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidSinDate);
            }
            var generationId = _setGenerationRepository.GetGenerationId(inputData.HpId, inputData.SinDate);
            var sets = _setRepository.GetList(inputData.HpId, inputData.SetKbn, inputData.SetKbnEdaNo, inputData.TextSearch);
            var result = sets.Where(r => r.GenerationId == generationId);

            var output = BuildTreeSetKbn(result);

            return (output?.Count > 0) ? new GetSetMstListOutputData(BuildTreeSetKbn(result), GetSetMstListStatus.Successed) : new GetSetMstListOutputData(null, GetSetMstListStatus.NoData);

        }
        private List<GetSetMstListOutputItem> BuildTreeSetKbn(IEnumerable<SetMstModel>? datas)
        {
            List<GetSetMstListOutputItem> result = new List<GetSetMstListOutputItem>();
            var topNodes = datas?.Where(c => c.Level2 == 0 && c.Level3 == 0);
            if (topNodes?.Any() != true) { return result; }
            var obj = new object();

            Parallel.ForEach(topNodes, item =>
            {
                var node = new GetSetMstListOutputItem(
                    item.HpId,
                    item.SetCd,
                    item.SetKbn,
                    item.SetKbnEdaNo,
                    item.GenerationId,
                    item.Level1,
                    item.Level2,
                    item.Level3,
                    item.SetName,
                    item.WeightKbn,
                    item.Color,
                    item.IsGroup,
                    datas?.Where(c => c.Level1 == item.Level1 && c.Level2 != 0 && c.Level3 == 0)?
                            .Select(c => new GetSetMstListOutputItem(
                                c.HpId,
                                c.SetCd,
                                c.SetKbn,
                                c.SetKbnEdaNo,
                                c.GenerationId,
                                c.Level1,
                                c.Level2,
                                c.Level3,
                                c.SetName,
                                c.WeightKbn,
                                c.Color,
                                c.IsGroup,
                                datas.Where(m => m.Level3 != 0 && m.Level1 == item.Level1 && m.Level2 == c.Level2)?
                                    .Select(c => new GetSetMstListOutputItem(
                                        c.HpId,
                                        c.SetCd,
                                        c.SetKbn,
                                        c.SetKbnEdaNo,
                                        c.GenerationId,
                                        c.Level1,
                                        c.Level2,
                                        c.Level3,
                                        c.SetName,
                                        c.WeightKbn,
                                        c.Color,
                                        c.IsGroup,
                                        new List<GetSetMstListOutputItem>() ?? new List<GetSetMstListOutputItem>()
                                    )).ToList().OrderBy(s => s.Level1).ThenBy(s => s.Level2).ThenBy(s => s.Level3).ToList() ?? new List<GetSetMstListOutputItem>()
                            )).ToList().OrderBy(s => s.Level1).ThenBy(s => s.Level2).ThenBy(s => s.Level3).ToList() ?? new List<GetSetMstListOutputItem>()
                    );

                lock (obj)
                {
                    result.Add(node);
                }
            });
            return result.OrderBy(s => s.Level1)
          .ThenBy(s => s.Level2)
          .ThenBy(s => s.Level3).ToList();
        }
    }
}