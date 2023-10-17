using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.SaveConversion;

namespace Interactor.SuperSetDetail;

public class SaveConversionInteractor : ISaveConversionInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public SaveConversionInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public SaveConversionOutputData Handle(SaveConversionInputData inputData)
    {
        try
        {
            var 
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
        }
    }

    private string ValidateData(ConversionItemModel resultConversionItem)
    {
        string emrMessageInfo = "を入力してください。";
        if (string.IsNullOrWhiteSpace(resultConversionItem.ItemCd) ||
                string.IsNullOrWhiteSpace(resultConversionItem.ItemName))
        {
            return "項目" + emrMessageInfo;
        }
        else if (resultConversionItem.Quantity <= 0 && !string.IsNullOrWhiteSpace(resultConversionItem.UnitName))
        {
            return "数量" + emrMessageInfo;
        }
        else if (resultConversionItem.IsCommentMaster && (string.IsNullOrWhiteSpace(resultConversionItem.CmtName) || string.IsNullOrWhiteSpace(resultConversionItem.CmtOpt)))
        {
            string errorMessage = "項目";

            if (resultConversionItem.Is830Cmt)
            {
                errorMessage = "文字情報";
            }
            else if (resultConversionItem.Is831Cmt)
            {
                errorMessage = "診療行為コード";
            }
            else if (resultConversionItem.Is840Cmt)
            {
                errorMessage = "数量";
            }
            else if (resultConversionItem.Is842Cmt)
            {
                errorMessage = "数量";
            }
            else if (resultConversionItem.Is850Cmt)
            {
                if (resultConversionItem.CmtName.Contains("日"))
                {
                    errorMessage = "年月日情報";
                }
                else
                {
                    errorMessage = "年月情報";
                }
            }
            else if (resultConversionItem.Is851Cmt)
            {
                errorMessage = "時刻情報";
            }
            else if (resultConversionItem.Is852Cmt)
            {
                errorMessage = "時間（分）情報";
            }
            else if (resultConversionItem.Is853Cmt)
            {
                errorMessage = "日時情報（日、時間及び分を6桁）";
            }
            else if (resultConversionItem.Is880Cmt)
            {
                errorMessage = "年月日情報及び数字情報を入力してください。" + "\r\n" + "※区切り文字「/」スラッシュを間に入力" + "\r\n" + "　数字情報は数字または次の文字　．－＋≧≦＞＜±";
                emrMessageInfo = string.Empty;
            }
            return errorMessage + emrMessageInfo;
        }
        else if ((resultConversionItem.Is830Cmt || resultConversionItem.Is842Cmt)
                  && !string.IsNullOrWhiteSpace(resultConversionItem.CmtOpt)
                  && resultConversionItem.CmtOpt.Length > 38)
        {
            return "コメントに対する入力値は３８文字以内にしてください。";
        }
        return string.Empty;
    }
}
