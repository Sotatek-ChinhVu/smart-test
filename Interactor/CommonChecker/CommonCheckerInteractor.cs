using Interactor.CommonChecker.CommonMedicalCheck;
using System.Diagnostics;
using UseCase.CommonChecker;

namespace Interactor.CommonChecker
{
    public class CommonCheckerInteractor : IGetOrderCheckerInputPort
    {
        private readonly ICommonMedicalCheck _commonMedicalCheck;

        public CommonCheckerInteractor(ICommonMedicalCheck commonMedicalCheck)
        {
            _commonMedicalCheck = commonMedicalCheck;
        }

        public GetOrderCheckerOutputData Handle(GetOrderCheckerInputData inputData)
        {
            var timer = new Stopwatch();
            timer.Start();

            var checkedResult = _commonMedicalCheck.CheckListOrder(inputData.HpId, inputData.PtId, inputData.SinDay, inputData.CurrentListOdr, inputData.ListCheckingOrder, inputData.SpecialNoteItem, inputData.PtDiseaseModels, inputData.FamilyItems, inputData.IsDataOfDb, inputData.RealTimeCheckerCondition);

            //timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;
            string foo = "Time Check: " + timeTaken.ToString(@"m\:ss\.fff");

            Console.WriteLine(foo);

            //timer = new Stopwatch();
            //timer.Start();
            var result = _commonMedicalCheck.GetErrorDetails(inputData.HpId, inputData.PtId, inputData.SinDay, checkedResult);

            timer.Stop();

            timeTaken = timer.Elapsed;
            foo = "Time Get: " + timeTaken.ToString(@"m\:ss\.fff");

            Console.WriteLine(foo);
            if (checkedResult == null || checkedResult.Count == 0)
            {
                return new GetOrderCheckerOutputData(new(), GetOrderCheckerStatus.Successed);
            }
            else
            {
                return new GetOrderCheckerOutputData(result ?? new(), GetOrderCheckerStatus.Error);
            }
        }
    }
}
