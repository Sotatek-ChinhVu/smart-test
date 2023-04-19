using Domain.Models.MedicalExamination;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.AddAutoItem;
using UseCase.MedicalExamination.GetContainerMst;
using UseCase.OrdInfs.GetListTrees;
using UseCase.RaiinKubunMst.GetListColumnName;

namespace Interactor.MedicalExamination
{
    public class GetContainerMstInteractor : IGetContainerMstInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;
        public GetContainerMstInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }

        public GetContainerMstOutputData Handle(GetContainerMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetContainerMstOutputData(GetContainerMstStatus.InvalidHpId, new());
                }
             
                if (inputData.SinDate < 0)
                {
                    return new GetContainerMstOutputData(GetContainerMstStatus.InvalidSinDate, new());
                }
                var odrInfItems = inputData.OdrInfItems.Select(o => new Tuple<int, int, int, List<Tuple<string, string>>>(o.InoutKbn, o.OdrKouiKbn, o.IsDeleted, o.OdrInfDetailItems.Select(od => new Tuple<string, string>(od.ItemCd, od.MasterSbt)).ToList())).ToList();
                var result = _medicalExaminationRepository.GetContainerMstModels(inputData.HpId, inputData.SinDate, odrInfItems, inputData.DefaultChecked);

                return new GetContainerMstOutputData(GetContainerMstStatus.Successed, result.Select(r => new KensaPrinterItem(
                        r.ItemCd,
                        r.ContainerName,
                        r.ContainerCd,
                        r.IsChecked,
                        r.KensaLabel,
                        r.SelectedPrinterName,
                        r.TextBoxBorderThickness,
                        r.ComboboxBorderThickness,
                        r.ItemName,
                        r.InoutKbn,
                        r.OdrKouiKbn
                        )).ToList());
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
    }
}
