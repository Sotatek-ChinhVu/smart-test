using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.DiseaseNameMstSearch;

namespace Interactor.MstItem
{
    public class DiseaseNameMstSearchInteractor : IDiseaseNameMstSearchInputPort
    {
        private readonly IMstItemRepository _inputItemRepository;

        public DiseaseNameMstSearchInteractor(IMstItemRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public DiseaseNameMstSearchOutputData Handle(DiseaseNameMstSearchInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new DiseaseNameMstSearchOutputData(new List<ByomeiMstModel>(), DiseaseNameMstSearchStatus.InvalidHpId);
                }

                var listData = _inputItemRepository.DiseaseNameMstSearch(inputData.HpId, inputData.Keyword, inputData.ChkByoKbn0, inputData.ChkByoKbn1, inputData.ChkSaiKbn, inputData.ChkMiSaiKbn, inputData.ChkSidoKbn, inputData.ChkToku, inputData.ChkHiToku1, inputData.ChkHiToku2, inputData.ChkTenkan, inputData.ChkTokuTenkan, inputData.ChkNanbyo, inputData.PageIndex, inputData.PageSize, inputData.IsCheckPage);
                return new DiseaseNameMstSearchOutputData(listData, DiseaseNameMstSearchStatus.Successful);
            }
            catch (Exception)
            {
                return new DiseaseNameMstSearchOutputData(new List<ByomeiMstModel>(), DiseaseNameMstSearchStatus.Failed);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
