using Domain.Models.Diseases;
using UseCase.Diseases.Upsert;

namespace Interactor.Diseases
{
    public class UpsertPtDiseaseListInteractor : IUpsertPtDiseaseListInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        public UpsertPtDiseaseListInteractor(IPtDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
        }
        public UpsertPtDiseaseListOutputData Handle(UpsertPtDiseaseListInputData inputData)
        {
            try
            {
                if (inputData.ToList().Count == 0) return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListInputNoData);
                _diseaseRepository.Upsert(inputData.ptDiseaseModel.Select(i =>
                        new PtDiseaseModel(
                                0,
                                i.PtId,
                                i.SeqNo,
                                "",
                                i.SortNo,
                                new List<string>() { i.SyusyokuCd1, i.SyusyokuCd2, i.SyusyokuCd3, i.SyusyokuCd4, i.SyusyokuCd5, i.SyusyokuCd6, i.SyusyokuCd7, i.SyusyokuCd8, i.SyusyokuCd9, i.SyusyokuCd10, i.SyusyokuCd11, i.SyusyokuCd12, i.SyusyokuCd13, i.SyusyokuCd14, i.SyusyokuCd15, i.SyusyokuCd16, i.SyusyokuCd17, i.SyusyokuCd18, i.SyusyokuCd19, i.SyusyokuCd20, i.SyusyokuCd21 },
                                i.Byomei,
                                i.StartDate,
                                i.TenkiKbn,
                                i.TenkiDate,
                                i.SyubyoKbn,
                                i.SikkanKbn,
                                i.NanByoCd,
                                i.IsNodspRece,
                                i.IsNodspKarte,
                                i.IsDeleted,
                                i.Id,
                                i.IsImportant,
                                0,
                                "",
                                "",
                                "",
                                "",
                                i.HokenPid,
                                i.HosokuCmt
                            )
                    ).ToList());
                return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.Success);
            }
            catch
            {
                return new UpsertPtDiseaseListOutputData(UpsertPtDiseaseListStatus.PtDiseaseListUpdateNoSuccess);
            }

        }
    }
}
