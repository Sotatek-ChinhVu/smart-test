using Helper.Common;
using UseCase.MstItem.ConvertStringChkJISKj;

namespace Interactor.MstItem;

public class ConvertStringChkJISKjInteractor : IConvertStringChkJISKjInputPort
{
    public ConvertStringChkJISKjOutputData Handle(ConvertStringChkJISKjInputData inputData)
    {
        string sOut = inputData.SOut;
        var result = CIUtil.Chk_JISKj(inputData.InputString, out sOut);
        return new ConvertStringChkJISKjOutputData(result, sOut, ConvertStringChkJISKjStatus.Successed);
    }
}
