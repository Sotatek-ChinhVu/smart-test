using Domain.Models.Document;
using UseCase.RaiinListSetting.GetDocCategory;

namespace Interactor.RaiinListSetting
{
    public class GetDocCategoryRaiinInteractor : IGetDocCategoryRaiinInputPort
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDocCategoryRaiinInteractor(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public GetDocCategoryRaiinOutputData Handle(GetDocCategoryRaiinInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus.InvalidHpId, new());
                }

                var data = _documentRepository.GetAllDocCategory(inputData.HpId);

                if (data.Any())
                {
                    return new GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus.Successful, data);
                }
                else
                {
                    return new GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus.NoData, data);
                }
            }
            finally
            {
                _documentRepository.ReleaseResource();
            }
        }
    }
}
