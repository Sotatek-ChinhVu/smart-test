using Helper.Common;
using UseCase.MstItem.ConvertStringChkJISKj;

namespace Interactor.MstItem;

public class ConvertStringChkJISKjInteractor : IConvertStringChkJISKjInputPort
{
    public ConvertStringChkJISKjOutputData Handle(ConvertStringChkJISKjInputData inputData)
    {
        var contentStr = string.Empty;
        string result = string.Empty;
        string itemError = string.Empty;
        foreach (var item in inputData.InputList)
        {
            var errorStr = CIUtil.Chk_JISKj(item, out contentStr);
            if (!string.IsNullOrEmpty(errorStr))
            {
                result = errorStr;
                itemError = item;
                break;
            }
        }
        return new ConvertStringChkJISKjOutputData(result, contentStr, itemError, ConvertStringChkJISKjStatus.Successed);
    }
}
