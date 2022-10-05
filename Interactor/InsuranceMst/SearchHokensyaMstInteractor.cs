using Domain.Models.InsuranceMst;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SearchHokensyaMst.Get;

namespace Interactor.InsuranceMst
{
    public class SearchHokensyaMstInteractor : ISearchHokensyaMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public SearchHokensyaMstInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public SearchHokensyaMstOutputData Handle(SearchHokensyaMstInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new SearchHokensyaMstOutputData(new List<HokensyaMstModel>(), SearchHokensyaMstStatus.InvalidHpId);
            }

            if (inputData.PageIndex < 0)
            {
                return new SearchHokensyaMstOutputData(new List<HokensyaMstModel>(), SearchHokensyaMstStatus.InvalidPageIndex);
            }

            if (inputData.PageCount < 0)
            {
                return new SearchHokensyaMstOutputData(new List<HokensyaMstModel>(), SearchHokensyaMstStatus.InvalidPageCount);
            }

            if (String.IsNullOrEmpty(inputData.Keyword))
            {
                return new SearchHokensyaMstOutputData(new List<HokensyaMstModel>(), SearchHokensyaMstStatus.InvalidKeyword);
            }

            string keyword = CIUtil.ToHalfsize(inputData.Keyword);
            var data = _insuranceMstReponsitory.SearchListDataHokensyaMst(inputData.HpId, inputData.PageIndex, inputData.PageCount,inputData.SinDate, keyword);

            return new SearchHokensyaMstOutputData(data.ToList(), SearchHokensyaMstStatus.Successed);
        }
    }
}
