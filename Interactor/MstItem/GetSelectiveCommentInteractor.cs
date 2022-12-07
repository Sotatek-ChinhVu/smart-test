using Domain.Models.MstItem;
using UseCase.MstItem.GetSelectiveComment;

namespace Interactor.MstItem
{
    public class GetSelectiveCommentInteractor : IGetSelectiveCommentInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly List<int> isValidLists = new List<int> { 0, 2 };

        public GetSelectiveCommentInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetSelectiveCommentOutputData Handle(GetSelectiveCommentInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new GetSelectiveCommentOutputData(new(), GetSelectiveCommentStatus.InValidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new GetSelectiveCommentOutputData(new(), GetSelectiveCommentStatus.InvalidSindate);
            }

            var listItemCd = inputData.ListItemCd.Select(i => i.Trim()).ToList();
            if (listItemCd.Count == 0 || listItemCd.Any(i => i.Equals("")))
            {
                return new GetSelectiveCommentOutputData(new(), GetSelectiveCommentStatus.InvalidItemCds);
            }

            try
            {
                var datas = _mstItemRepository.GetSelectiveComment(inputData.HpId, listItemCd, inputData.SinDate, isValidLists);

                var comments = new List<GetSelectiveCommentItemOfList>();

                foreach (var inputCodeItem in datas)
                {
                    var comment = new GetSelectiveCommentItemOfList(new(), inputCodeItem.ItemCd, inputCodeItem.ItemCd, inputCodeItem.SanteiItemCd);

                    if (inputCodeItem.ItemCmtModels.Count <= 0)
                    {
                        continue;
                    }

                    var listCommentWithCode = inputCodeItem.ItemCmtModels.OrderBy(item => item.ItemNo)
                        .ThenBy(item => item.EdaNo)
                        .ThenBy(item => item.SortNo)
                        .ToList();

                    comment.SetData(listCommentWithCode);
                    comments.Add(comment);
                }

                return new GetSelectiveCommentOutputData(comments, GetSelectiveCommentStatus.Successed);
            }
            catch
            {
                return new GetSelectiveCommentOutputData(new(), GetSelectiveCommentStatus.Failed);
            }
        }
    }
}
