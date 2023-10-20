using Domain.Models.HokenMst;
using Helper.Common;
using UseCase.HokenMst.GetHokenMst;

namespace Interactor.HokenMst
{
    public class GetHokenMstInteractor : IGetHokenMstInputPort
    {
        private readonly IHokenMstRepository _hokenMstRepository;

        public GetHokenMstInteractor(IHokenMstRepository hokenMstRepository)
        {
            _hokenMstRepository = hokenMstRepository;
        }

        public GetHokenMstOutputData Handle(GetHokenMstInputData inputData)
        {
            try
            {
                var allHokenMsts = _hokenMstRepository.FindHokenMst(inputData.HpId);
                int dateNow = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
                var expireHokenMsts = allHokenMsts.Where(x => x.StartDate <= dateNow && x.EndDate >= dateNow).ToList();

                var kohis = expireHokenMsts.Where(kohiInf =>
                  kohiInf.HokenSbtKbn == 2
                  || kohiInf.HokenSbtKbn == 5
                  || kohiInf.HokenSbtKbn == 6
                  || kohiInf.HokenSbtKbn == 7);

                var kohiModels = kohis.Select(x => new HokenMstItem(x.SelectedValueMaster, x.DisplayTextMasterWithoutHokenNo, x.Houbetu, x.HoubetuDisplayText, x.HokenNo, x.HokenEdaNo)).ToList();

                var kohiModelWithFutansyanos = expireHokenMsts.Where(kohiInf =>
                (kohiInf.HokenSbtKbn == 2
                || kohiInf.HokenSbtKbn == 5
                || kohiInf.HokenSbtKbn == 6
                || kohiInf.HokenSbtKbn == 7) &&
                kohiInf.IsFutansyaNoCheck == 1 &&
                kohiInf.HokenEdaNo == 0);

                var kohiDict = new Dictionary<string, string>();
                foreach (var item in kohiModelWithFutansyanos)
                {
                    if (!kohiDict.ContainsKey(item.Houbetu))
                    {
                        kohiDict.Add(item.Houbetu, item.HoubetuDisplayText);
                    }
                }

                var hokenInfModels = expireHokenMsts.Where(hokenInf =>
                (hokenInf.HokenSbtKbn == 1) &&
                hokenInf.IsFutansyaNoCheck == 1 &&
                hokenInf.HokenEdaNo == 0);

                var hokenInfDict = new Dictionary<string, string>();
                foreach (var item in hokenInfModels)
                {
                    if (!hokenInfDict.ContainsKey(item.Houbetu))
                    {
                        hokenInfDict.Add(item.Houbetu, item.HoubetuDisplayText);
                    }
                }

                return new GetHokenMstOutputData(hokenInfDict, kohiDict, kohiModels, GetHokenMstStatus.Successed);
            }
            finally
            {
                _hokenMstRepository.ReleaseResource();
            }
        }
    }
}
