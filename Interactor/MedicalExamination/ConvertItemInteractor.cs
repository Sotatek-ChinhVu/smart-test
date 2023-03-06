using CommonCheckers;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Domain.Models.MstItem;
using Domain.Models.TodayOdr;
using Helper.Common;
using Helper.Constants;
using UseCase.MedicalExamination.CheckedExpired;
using UseCase.MedicalExamination.ConvertItem;
using UseCase.MedicalExamination.GetCheckedOrder;
using OdrInfDetailItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfDetailItem;

namespace Interactor.MedicalExamination
{
    public class ConvertItemInteractor : IConvertItemInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public ConvertItemInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public ConvertItemOutputData Handle(ConvertItemInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidHpId, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InValidUserId, new());
                }
                if (inputData.ExpiredItems.Count == 0 || inputData.OdrInfItems.Count == 0)
                {
                    return new ConvertItemOutputData(ConvertItemStatus.InputNotData, new());
                }

                var result = _mstItemRepository.ExceConversionItem(inputData.HpId, inputData.UserId, inputData.ExpiredItems);

   

                return new ConvertItemOutputData(ConvertItemStatus.Successed, result);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }


    }
}
