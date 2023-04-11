using Helper.Common;
using Reporting.Byomei.Model;
using Reporting.Mappers.Common;

namespace Reporting.Byomei.Mapper
{
    public class ByomeiMapper : CommonReportingRequest
    {
        private List<CoPtByomeiModel> _byomeiList;
        public ByomeiMapper(List<CoPtByomeiModel> byomeiList)
        {
            _byomeiList = byomeiList;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            if (_byomeiList == null ||
                !_byomeiList.Any())
            {
                return new Dictionary<string, string>();
            }
            Dictionary<string, string> data = new Dictionary<string, string>();
            CoPtByomeiModel coModel = _byomeiList.First();

            data.Add("dfPtNum", coModel.PtNum.ToString());
            data.Add("dfPtKanaName", coModel.KanaName.ToString());
            data.Add("dfPtName", coModel.KanjiName.ToString());
            data.Add("dfBirthDay", coModel.BirthDay.ToString());
            data.Add("dfSex", coModel.Sex.ToString());
            data.Add("bcPtNum", coModel.PtNum.ToString());

            int iAge = CIUtil.SDateToAge(coModel.BirthYmd, CIUtil.DateTimeToInt(DateTime.Now));
            data.Add("dfAge", iAge.ToString());

            string bufFrom = string.Empty;
            if (coModel.FromDay != 0)
            {
                bufFrom = CIUtil.SDateToShowSDate(coModel.FromDay);
            }

            string bufTo = string.Empty;
            if (coModel.ToDay != 0)
            {
                bufTo = CIUtil.SDateToShowSDate(coModel.ToDay);
            }

            if (!string.IsNullOrEmpty(bufFrom) || !string.IsNullOrEmpty(bufTo))
            {
                if (!string.IsNullOrEmpty(bufFrom))
                {
                    data.Add("dfStartDate", bufFrom);
                }
                else
                {
                    //CoRep.SetFieldData("dfStartDate", CON_UNSPECIFIED_TIME);
                }

                if (!string.IsNullOrEmpty(bufTo))
                {
                    data.Add("dfEndDate", bufTo);
                }
                else
                {
                    //CoRep.SetFieldData("dfEndDate", CON_UNSPECIFIED_TIME);
                }
            }

            string sMakeYmd = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            data.Add("dfPrintDateTime", sMakeYmd);

            //保険番号が"0"以外の場合にのみ、保険名称を印字する
            if (coModel.HokenPatternName != string.Empty)
            {
                data.Add("dfHokenPattern", coModel.HokenPatternName);
            }

            return data;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            if (_byomeiList == null)
            {
                return new List<Dictionary<string, CellModel>>();
            }

            List<Dictionary<string, CellModel>> result = new List<Dictionary<string, CellModel>>();
            foreach (var item in _byomeiList)
            {
                if (item.ListByomei != null)
                {
                    foreach (var byomei in item.ListByomei)
                    {
                        Dictionary<string, CellModel> data = new Dictionary<string, CellModel>();
                        data.Add("lsByomei", new CellModel(byomei.ByomeiName));
                        data.Add("lsStartDate", new CellModel(byomei.StartDate));
                        data.Add("lsTenkiDate", new CellModel(byomei.TenkiDate));
                        data.Add("lsTenki", new CellModel(byomei.DisplayTenki));
                        result.Add(data);
                    }
                }
            }

            return result;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            if (_byomeiList == null ||
                !_byomeiList.Any())
            {
                return new Dictionary<string, bool>();
            }
            Dictionary<string, bool> data = new Dictionary<string, bool>();
            CoPtByomeiModel coModel = _byomeiList.First();

            string bufFrom = string.Empty;
            if (coModel.FromDay != 0)
            {
                bufFrom = CIUtil.SDateToShowSDate(coModel.FromDay);
            }

            string bufTo = string.Empty;
            if (coModel.ToDay != 0)
            {
                bufTo = CIUtil.SDateToShowSDate(coModel.ToDay);
            }

            if (string.IsNullOrEmpty(bufFrom) && string.IsNullOrEmpty(bufTo))
            {
                data.Add("lblTermTitle", false);
                data.Add("lblTermKara", false);
            }
            else
            {
                data.Add("lblTermTitle", true);
                data.Add("lblTermKara", true);
            }

            return data;
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>() { { "lsByomei", true } };
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Byomei;
        }

        public override string GetRowCountFieldName()
        {
            return "lsByomei";
        }
    }
}
