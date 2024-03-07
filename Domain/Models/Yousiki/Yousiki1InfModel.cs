using Helper.Extension;

namespace Domain.Models.Yousiki;

public class Yousiki1InfModel
{
    public Yousiki1InfModel(int hpId, long ptNum, string name, bool isTester, long ptId, int sinYm, int dataType, int status, Dictionary<int, int> statusDic, int seqNo, List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        HpId = hpId;
        PtNum = ptNum;
        Name = name;
        IsTester = isTester;
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        Status = status;
        StatusDic = statusDic;
        SeqNo = seqNo;
        Yousiki1InfDetailList = yousiki1InfDetailList.OrderBy(item => item.CodeNo)
                                                     .ThenBy(item => item.RowNo)
                                                     .ThenBy(item => item.Payload)
                                                     .ToList();
        FilterYousiki1InfDetailList = Yousiki1InfDetailList.Where(FilterDetails).ToList();
    }

    public Yousiki1InfModel(int hpId, long ptId, int sinYm, int dataType, int seqNo, int isDeleted, int status)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        IsDeleted = isDeleted;
        Status = status;
        Name = string.Empty;
        StatusDic = new();
        Yousiki1InfDetailList = new();
        DataTypeSeqNoDic = new();
        FilterYousiki1InfDetailList = new(); 
        FilterYousiki1InfDetailList = new();

    }

    public Yousiki1InfModel(int hpId, long ptId, int sinYm, int dataType, int seqNo, int isDeleted, int status, long ptNum, string name)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        IsDeleted = isDeleted;
        Status = status;
        PtNum = ptNum;
        Name = name;
        StatusDic = new();
        Yousiki1InfDetailList = new();
        FilterYousiki1InfDetailList = new();
        DataTypeSeqNoDic = new();
        FilterYousiki1InfDetailList = new();
    }

    public Yousiki1InfModel()
    {
        Name = string.Empty;
        StatusDic = new();
        DataTypeSeqNoDic = new();
        FilterYousiki1InfDetailList = new();
        Yousiki1InfDetailList = new();
    }

    public Yousiki1InfModel ChangeStatusDic(Dictionary<int, int> statusDic, Dictionary<int, int> dataTypeSeqNoDic)
    {
        StatusDic = statusDic;
        DataTypeSeqNoDic = dataTypeSeqNoDic;
        return this;
    }

    public Yousiki1InfModel ChangeYousiki1InfDetailList(List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        return this;
    }

    public int HpId { get; private set; }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public bool IsTester { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int Status { get; private set; }

    public Dictionary<int, int> StatusDic { get; private set; }

    public int SeqNo { get; private set; }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<Yousiki1InfDetailModel> FilterYousiki1InfDetailList { get; private set; }

    public Dictionary<int, int> DataTypeSeqNoDic { get; private set; }

    public int IsDeleted { get; private set; }

    #region private function
    private bool FilterDetails(Yousiki1InfDetailModel yousiki1InfDetail)
    {
        switch (DataType)
        {
            case 0:
                return FilterDetailsOnCommon(yousiki1InfDetail);
            case 1:
                return FilterDetailsOnLivingHabit(yousiki1InfDetail);
            case 2:
                return FilterDetailsOnAtHome(yousiki1InfDetail);
            case 3:
                return FilterDetailsOnRehabilitation(yousiki1InfDetail);
        }
        return true;
    }

    private bool FilterDetailsOnLivingHabit(Yousiki1InfDetailModel yousiki1InfDetail)
    {
        //他院による紹介の有無 
        if (yousiki1InfDetail.CodeNo == "LR00001" && yousiki1InfDetail.Payload == 3)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LR00001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 2 && x.Value == "1");
        }
        #region 糖尿病
        //自院管理の有無 
        if (yousiki1InfDetail.CodeNo == "LMDM001" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMDM001" && x.RowNo == 0 && x.Payload == 1 && (x.Value == "1" || x.Value == "2" || x.Value == "3"));
        }
        //診断年月
        if (yousiki1InfDetail.CodeNo == "LMDM002" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 1)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMDM001" && x.RowNo == 0 && x.Payload == 2 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        //血糖コントロール
        List<int> payloads = new List<int>() { 1, 2, 3, 4 };
        if (yousiki1InfDetail.CodeNo == "LMDM003" && yousiki1InfDetail.RowNo == 0 && payloads.Any(x => x == yousiki1InfDetail.Payload))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMDM001" && x.RowNo == 0 && x.Payload == 2 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        #endregion

        #region 高血圧症
        //自院管理の有無
        if (yousiki1InfDetail.CodeNo == "LMHTN01" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHTN01" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        //診断年月
        if (yousiki1InfDetail.CodeNo == "LMHTN02" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 1)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHTN01" && x.RowNo == 0 && x.Payload == 2 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        //血圧分類
        if (yousiki1InfDetail.CodeNo == "LMHTN03" && yousiki1InfDetail.RowNo == 0 && payloads.Any(x => x == yousiki1InfDetail.Payload))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHTN01" && x.RowNo == 0 && x.Payload == 2 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        #endregion

        #region 脂質異常症
        //診断年月
        if (yousiki1InfDetail.CodeNo == "LMDL002" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 1)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMDL001" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        //リスク分類
        if (yousiki1InfDetail.CodeNo == "LMDL003" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 1)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMDL001" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        //LDLコレステロール
        if (yousiki1InfDetail.CodeNo == "LMDL003" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMDL001" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1") && !string.IsNullOrEmpty(yousiki1InfDetail.Value);
        }
        #endregion

        #region 脳卒中
        //有無（既往含む）
        if (yousiki1InfDetail.CodeNo == "LMHCA01" && yousiki1InfDetail.RowNo == 0 && (yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHCA01" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        //発症（診断）年月
        if (yousiki1InfDetail.CodeNo == "LMHCA02" && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHCA02" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 1 && (x.Value == "1" || x.Value == "2" || x.Value == "3" || x.Value == "4"));
        }
        #endregion

        #region 急性冠症候群
        //種類（既往含む）
        if (yousiki1InfDetail.CodeNo == "LMHACS1" && yousiki1InfDetail.RowNo == 0 && (yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHACS1" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        //発症（診断）年月
        if (yousiki1InfDetail.CodeNo == "LMHACS2" && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHACS2" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 1 && (x.Value == "1" || x.Value == "2"));
        }
        #endregion

        #region 心不全
        if (yousiki1InfDetail.CodeNo == "LMHHF01" && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHHF01" && x.Payload == 1 && (x.Value == "1" || x.Value == "2"));
        }
        #endregion

        #region 急性大動脈解離
        if (yousiki1InfDetail.CodeNo == "LMHAAD1" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHAAD1" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        #endregion

        #region 慢性腎臓病
        if (yousiki1InfDetail.CodeNo == "LMHCKD1" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 2)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHCKD1" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        #endregion

        #region 高尿酸血症
        if (yousiki1InfDetail.CodeNo == "LMHH001" && yousiki1InfDetail.RowNo == 0 && (yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "LMHH001" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        #endregion
        return true;
    }

    private bool FilterDetailsOnRehabilitation(Yousiki1InfDetailModel yousiki1InfDetail)
    {
        //外来受診情報
        if (yousiki1InfDetail.CodeNo == "RR00001" && (yousiki1InfDetail.Payload == 3))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "RR00001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 2 && x.Value == "1");
        }

        if (yousiki1InfDetail.CodeNo == "RCD0001" && yousiki1InfDetail.Payload == 6)
        {
            //File records 8002 only if there is (suspected) 8002. Do not record other modifier codes
            bool isVisible = !(string.IsNullOrEmpty(yousiki1InfDetail.Value) || !yousiki1InfDetail.Value.Contains("8002"));
            if (isVisible && yousiki1InfDetail.Value.Contains("8002"))
            {
                yousiki1InfDetail.ChangeValue("8002");
            }
            return isVisible;
        }

        if (yousiki1InfDetail.CodeNo == "RCD0001" && yousiki1InfDetail.Payload == 3 && yousiki1InfDetail.Value.AsInteger() <= 0)
        {
            yousiki1InfDetail.ChangeValue(string.Empty);
        }

        return true;
    }

    private bool FilterDetailsOnCommon(Yousiki1InfDetailModel yousiki1InfDetail)
    {
        //喫煙歴
        if (yousiki1InfDetail.CodeNo == "CPFS001" && yousiki1InfDetail.RowNo == 0 && (yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "CPFS001" && x.RowNo == 0 && x.Payload == 1 && x.Value.AsInteger() > 0);
        }

        //診断情報/傷病 
        if (yousiki1InfDetail.CodeNo == "CD00001" && (yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3 || yousiki1InfDetail.Payload == 4))
        {
            if (yousiki1InfDetail.Payload == 4)
            {
                if (!string.IsNullOrEmpty(yousiki1InfDetail.Value) && yousiki1InfDetail.Value.Contains("8002"))
                {
                    yousiki1InfDetail.ChangeValue("8002");
                    return true;
                }

                return false;
            }

            return Yousiki1InfDetailList.Any(x => x.CodeNo == "CD00001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 1 && x.Value.AsInteger() > 0);
        }


        //入院の状況
        if (yousiki1InfDetail.CodeNo == "CH00001" && (yousiki1InfDetail.Payload == 9 || yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3 || yousiki1InfDetail.Payload == 4))
        {
            var result = Yousiki1InfDetailList.Any(x => x.CodeNo == "CH00001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 1 && x.Value.AsInteger() > 0);
            if (!result)
            {
                return false;
            }

            if (yousiki1InfDetail.Payload == 4)
            {
                if (!string.IsNullOrEmpty(yousiki1InfDetail.Value) && yousiki1InfDetail.Value.Contains("8002"))
                {
                    yousiki1InfDetail.ChangeValue("8002");
                    return true;
                }

                return false;
            }
        }


        //終診情報
        if (yousiki1InfDetail.CodeNo == "CDF0001" && (yousiki1InfDetail.Payload == 9 || yousiki1InfDetail.Payload == 3 ||
                                                      yousiki1InfDetail.Payload == 4 || yousiki1InfDetail.Payload == 5 ||
                                                      yousiki1InfDetail.Payload == 2))
        {

            var result = Yousiki1InfDetailList.Any(x => x.CodeNo == "CDF0001" && x.RowNo == 0 && x.Payload == 1 && x.Value.AsInteger() > 0);
            if (yousiki1InfDetail.Payload == 2)
            {
                if (result)
                {
                    return true;
                }
                return false;
            }

            result = Yousiki1InfDetailList.Any(x => x.CodeNo == "CDF0001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 1 && x.Value == "4");
            if (!result)
            {
                return false;
            }

            if (yousiki1InfDetail.Payload == 5)
            {
                if (!string.IsNullOrEmpty(yousiki1InfDetail.Value) && yousiki1InfDetail.Value.Contains("8002"))
                {
                    yousiki1InfDetail.ChangeValue("8002");
                    return true;
                }

                return false;
            }
        }

        return true;
    }

    private bool FilterDetailsOnAtHome(Yousiki1InfDetailModel yousiki1InfDetail)
    {
        //訪問の主傷病
        if (yousiki1InfDetail.CodeNo == "HCVD001" && yousiki1InfDetail.RowNo == 0 && (yousiki1InfDetail.Payload == 2 || yousiki1InfDetail.Payload == 3))
        {
            //When outputting a file, output as empty if the existence of own hospital diagnosis is other than "1"
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "HCVD001" && x.RowNo == 0 && x.Payload == 1 && x.Value == "1");
        }
        if (yousiki1InfDetail.CodeNo == "HCVD001" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 4)
        {
            //File records 8002 only if there is (suspected) 8002. Do not record other modifier codes
            bool isVisible = !(string.IsNullOrEmpty(yousiki1InfDetail.Value) || !yousiki1InfDetail.Value.Contains("8002"));
            if (isVisible && yousiki1InfDetail.Value.Contains("8002"))
            {
                yousiki1InfDetail.ChangeValue("8002");
            }
            return isVisible;
        }
        //入院の状況
        if (yousiki1InfDetail.CodeNo == "HCH0001" && (yousiki1InfDetail.Payload == 4 || yousiki1InfDetail.Payload == 5))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "HCH0001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 3 && x.Value == "1");
        }
        if (yousiki1InfDetail.CodeNo == "HCH0001" && yousiki1InfDetail.Payload == 6)
        {
            bool isVisible = !(string.IsNullOrEmpty(yousiki1InfDetail.Value) || !yousiki1InfDetail.Value.Contains("8002"));
            if (isVisible && yousiki1InfDetail.Value.Contains("8002"))
            {
                yousiki1InfDetail.ChangeValue("8002");
            }
            return isVisible;
        }
        //往診の状況
        if (yousiki1InfDetail.CodeNo == "HCHC001" && (yousiki1InfDetail.Payload == 3 || yousiki1InfDetail.Payload == 4))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "HCHC001" && x.RowNo == yousiki1InfDetail.RowNo && x.Payload == 2 && x.Value == "1");
        }
        if (yousiki1InfDetail.CodeNo == "HCHC001" && yousiki1InfDetail.Payload == 5)
        {
            bool isVisible = !(string.IsNullOrEmpty(yousiki1InfDetail.Value) || !yousiki1InfDetail.Value.Contains("8002"));
            if (isVisible && yousiki1InfDetail.Value.Contains("8002"))
            {
                yousiki1InfDetail.ChangeValue("8002");
            }
            return isVisible;
        }
        //がんの傷病
        //自院診断の有無
        if (yousiki1InfDetail.CodeNo == "HPCD001" && yousiki1InfDetail.RowNo == 0)
        {
            bool isEnableHPS001 = Yousiki1InfDetailList.Any(x => x.CodeNo == "HPS0001" && x.RowNo == 0 && x.Payload == 1 && x.Value.Length > 0 && x.Value[0] == '1');
            if (!isEnableHPS001)
            {
                return false;
            }
            if (yousiki1InfDetail.Payload == 1)
            {
                return isEnableHPS001;
            }
            //傷病名
            if (yousiki1InfDetail.Payload == 9)
            {
                return isEnableHPS001;
            }
            //ICD10コード
            if (yousiki1InfDetail.Payload == 2)
            {
                return !Yousiki1InfDetailList.Any(x => x.CodeNo == "HPCD001" && x.RowNo == 0 && x.Payload == 1 && (string.IsNullOrEmpty(x.Value) || x.Value != "1"));
            }
            //傷病名コード
            if (yousiki1InfDetail.Payload == 3)
            {
                return !Yousiki1InfDetailList.Any(x => x.CodeNo == "HPCD001" && x.RowNo == 0 && x.Payload == 1 && (string.IsNullOrEmpty(x.Value) || x.Value != "1"));
            }
            //修飾語コード
            if (yousiki1InfDetail.Payload == 4)
            {
                bool isVisible = !(string.IsNullOrEmpty(yousiki1InfDetail.Value) || !yousiki1InfDetail.Value.Contains("8002"));
                if (isVisible && yousiki1InfDetail.Value.Contains("8002"))
                {
                    yousiki1InfDetail.ChangeValue("8002");
                }
                return isVisible;
            }
        }

        //がんのStaging分類 
        List<int> payloads = new() { 1, 2, 3, 4, 5 };
        if (yousiki1InfDetail.CodeNo == "HPCS001" && yousiki1InfDetail.RowNo == 0 && payloads.Any(x => x == yousiki1InfDetail.Payload))
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "HPS0001" && x.RowNo == 0 && x.Payload == 1 && x.Value.Length > 0 && x.Value[0] == '1');
        }
        if (yousiki1InfDetail.CodeNo == "HPCNRS1" && yousiki1InfDetail.RowNo == 0 && yousiki1InfDetail.Payload == 1)
        {
            return Yousiki1InfDetailList.Any(x => x.CodeNo == "HPS0001" && x.RowNo == 0 && x.Payload == 1 && x.Value.Length > 0 && x.Value[0] == '1');
        }
        return true;
    }
    #endregion
}
