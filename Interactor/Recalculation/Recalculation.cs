using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactor.Recalculation
{
    public class Recalculation: IRecalculation
    {
        public void CheckErrorInMonth(int seikyuYm, List<long> ptIds)
        {
            Log.WriteLogStart(ModuleNameConst.EmrCommonView, this, nameof(CheckErrorInMonth), "", ICDebugConf.logLevel);

            try
            {
                IsStopCalc = false;
                AllCheckCount = _recalculationFinder.GetCountReceInfs(ptIds, seikyuYm);
                CheckedCount = 0;

                _receCheckOpts = _recalculationFinder.GetReceCheckOpts();
                _receInfModels = _recalculationFinder.GetReceInfModels(ptIds, seikyuYm);

                _commandHandler.InitBeforeCheckError();
                foreach (var receInfModel in _receInfModels)
                {
                    if (IsStopCalc) break;
                    if (CancellationToken.IsCancellationRequested) return;
                    _commandHandler.ClearReceCmtErr(receInfModel.PtId, receInfModel.HokenId, receInfModel.SinYm);
                    _sinKouiCounts = _recalculationFinder.GetSinKouiCounts(receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                    CheckHoken(receInfModel);
                    CheckByomei(receInfModel);
                    CheckOrder(receInfModel);
                    CheckRosai(receInfModel);
                    CheckAftercare(receInfModel);

                    CheckedCount++;
                }
                _commandHandler.SaveChanged();
                PrintReceCheck(seikyuYm, ptIds);
            }
            finally
            {
                Log.WriteLogEnd(ModuleNameConst.EmrCommonView, this, nameof(CheckErrorInMonth), ICDebugConf.logLevel);
            }
        }
    }
}
