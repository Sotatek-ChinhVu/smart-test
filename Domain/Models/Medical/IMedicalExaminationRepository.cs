using Domain.Common;
using Domain.Models.Diseases;
using Domain.Models.Medical;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;

namespace Domain.Models.MedicalExamination
{
    public interface IMedicalExaminationRepository : IRepositoryBase
    {
        List<CheckedOrderModel> IgakuTokusitu(int hpId, int sinDate, int hokenId, int syosaisinKbn, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou);

        void IgakuTokusituIsChecked(int hpId, int sinDate, int syosaisinKbn, ref List<CheckedOrderModel> checkedOrders, List<OrdInfDetailModel> allOdrInfDetail);

        List<CheckedOrderModel> SihifuToku1(int hpId, long ptId, int sinDate, int hokenId, int syosaisinKbn, long raiinNo, long oyaRaiinNo, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou);

        List<CheckedOrderModel> SihifuToku2(int hpId, long ptId, int sinDate, int hokenId, int iBirthDay, long raiinNo, int syosaisinKbn, long oyaRaiinNo, List<PtDiseaseModel> byomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, List<int> odrInfs, bool isJouhou);

        List<CheckedOrderModel> IgakuTenkan(int hpId, int sinDate, int hokenId, int syosaisinKbn, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou);

        List<CheckedOrderModel> IgakuNanbyo(int hpId, int sinDate, int hokenId, int syosaisinKbn, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou);

        List<CheckedOrderModel> InitPriorityCheckDetail(List<CheckedOrderModel> checkedOrderModelList);

        List<CheckedOrderModel> TouyakuTokusyoSyoho(int hpId, int sinDate, int hokenId, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, List<OrdInfModel> allOdrInf);

        List<CheckedOrderModel> ChikiHokatu(int hpId, long ptId, int userId, int sinDate, int primaryDoctor, int tantoId, List<OrdInfDetailModel> allOdrInfDetail, int syosaisinKbn);

        List<CheckedOrderModel> YakkuZai(int hpId, long ptId, int sinDate, int birthDay, List<OrdInfDetailModel> allOdrInfDetail, List<OrdInfModel> allOdrInf);

        List<CheckedOrderModel> SiIkuji(int hpId, int sinDate, int birthDay, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou, int syosaisinKbn);

        List<CheckedOrderModel> Zanyaku(int hpId, int sinDate, List<OrdInfDetailModel> allOdrInfDetail, List<OrdInfModel> allOrderInf);

        (List<string>, List<SinKouiCountModel>) GetCheckedAfter327Screen(int hpId, long ptId, int sinDate, List<CheckedOrderModel> checkedTenMstResult, bool isTokysyoOrder, bool isTokysyosenOrder);
    }
}
