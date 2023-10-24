using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.UpdateAdoptedByomei;

namespace UseCase.MstItem.DiseaseNameMstSearch
{
    public class DiseaseNameMstSearchInputData : IInputData<DiseaseNameMstSearchOutputData>
    {
        public DiseaseNameMstSearchInputData(int hpId, string keyword, bool chkByoKbn0, bool chkByoKbn1, bool chkSaiKbn, bool chkMiSaiKbn, bool chkSidoKbn, bool chkToku, bool chkHiToku1, bool chkHiToku2, bool chkTenkan, bool chkTokuTenkan, bool chkNanbyo, int pageIndex, int pageSize, bool isCheckPage)
        {
            HpId = hpId;
            Keyword = keyword;
            ChkByoKbn0 = chkByoKbn0;
            ChkByoKbn1 = chkByoKbn1;
            ChkSaiKbn = chkSaiKbn;
            ChkMiSaiKbn = chkMiSaiKbn;
            ChkSidoKbn = chkSidoKbn;
            ChkToku = chkToku;
            ChkHiToku1 = chkHiToku1;
            ChkHiToku2 = chkHiToku2;
            ChkTenkan = chkTenkan;
            ChkTokuTenkan = chkTokuTenkan;
            ChkNanbyo = chkNanbyo;
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsCheckPage = isCheckPage;
        }

        public int HpId { get; private set; }
        public string Keyword { get; private set; }
        public bool ChkByoKbn0 { get; private set; }
        public bool ChkByoKbn1 { get; private set; }
        public bool ChkSaiKbn { get; private set; }
        public bool ChkMiSaiKbn { get; private set; }
        public bool ChkSidoKbn { get; private set; }
        public bool ChkToku { get; private set; }
        public bool ChkHiToku1 { get; private set; }
        public bool ChkHiToku2 { get; private set; }
        public bool ChkTenkan { get; private set; }
        public bool ChkTokuTenkan { get; private set; }
        public bool ChkNanbyo { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public bool IsCheckPage { get; private set; }
    }
}
    