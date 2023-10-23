using Domain.Common;
using Helper.Messaging;

namespace Domain.Models.KensaIrai;

public interface IKensaIraiRepository : IRepositoryBase
{
    KensaCenterMstModel GetKensaCenterMst(int hpId, string centerCd);

    List<KensaInfModel> GetKensaInf(int hpId, long ptId, long raiinNo, string centerCd);

    List<KensaInfDetailModel> GetKensaInfDetail(int hpId, long ptId, long raiinNo, string centerCd);

    bool SaveKensaInf(int hpId, int userId, List<KensaInfModel> kensaInfModels, List<KensaInfDetailModel> kensaInfDetailModels);

    List<KensaIraiModel> GetKensaIraiModels(int hpId, long ptId, int startDate, int endDate, string kensaCenterMstCenterCd, int kensaCenterMstPrimaryKbn);

    List<KensaIraiModel> GetKensaIraiModels(int hpId, List<KensaInfModel> kensaInfModelList);

    List<KensaIraiModel> CreateDataKensaIraiRenkei(int hpId, int userId, List<KensaIraiModel> kensaIraiList, string centerCd, int systemDate);

    bool CheckExistCenterCd(int hpId, string centerCd);

    bool CheckExistCenterCd(int hpId, List<string> centerCdList);

    public bool CheckExistIraiCd(int hpId, List<long> iraiCdList);

    List<KensaIraiModel> ReCreateDataKensaIraiRenkei(int hpId, int userId, List<KensaIraiModel> kensaIraiList, int systemDate);

    List<KensaInfModel> GetKensaInfModels(int hpId, int startDate, int endDate, string centerCd = "");

    bool DeleteKensaInfModel(int hpId, int userId, List<KensaInfModel> kensaInfList);

    bool CheckExistIraiCdList(int hpId, List<long> iraiCdList);

    List<long> GetIraiCdNotExistList(int hpId, List<long> iraiCdList);

    List<KensaIraiLogModel> GetKensaIraiLogModels(int hpId, int startDate, int endDate);

    bool SaveKensaIraiLog(int hpId, int userId, KensaIraiLogModel model);

    List<KensaInfMessageModel> SaveKensaIraiImport(int hpId, int userId, IMessenger messenger, List<KensaInfDetailModel> kensaInfDetailList);

    bool SaveKensaResultLog(int hpId, int userId, string KekaFile);
}
