using System.Text.Json.Serialization;

namespace Domain.Models.MainMenu
{
    public class StaCsvModel
    {
        [JsonConstructor]
        public StaCsvModel(bool isShowTemplateModel, bool isModified, bool isDeleted, int id, int hpId, int reportId, int rowNo, string confName, int dataSbt, string columns, int sortKbn, bool isSelected, string columnsDisplay)
        {
            IsShowTemplateModel = isShowTemplateModel;
            IsModified = isModified;
            IsDeleted = isDeleted;
            Id = id;
            HpId = hpId;
            ReportId = reportId;
            RowNo = rowNo;
            ConfName = confName;
            DataSbt = dataSbt;
            Columns = columns;
            SortKbn = sortKbn;
            IsSelected = isSelected;
            ColumnsDisplay = columnsDisplay;
        }

        public StaCsvModel(int hpId, string confName, int rowNo, int sortKbn, int reportId, int dataSbt, string columns, bool isSelected, string columnsDisplay)
        {
            HpId = hpId;
            ConfName = confName;
            RowNo = rowNo;
            SortKbn = sortKbn;
            ReportId = reportId;
            DataSbt = dataSbt;
            Columns = columns;
            IsSelected = isSelected;
            ColumnsDisplay = columnsDisplay;
        }

        public StaCsvModel(int id, int hpId, string confName, int rowNo, int sortKbn, int reportId, int dataSbt, string columns, bool isSelected, string columnsDisplay)
        {
            Id = id;
            HpId = hpId;
            ReportId = reportId;
            RowNo = rowNo;
            ConfName = confName;
            DataSbt = dataSbt;
            Columns = columns;
            SortKbn = sortKbn;
            IsSelected = isSelected;
            ColumnsDisplay = columnsDisplay;
        }


        public bool IsShowTemplateModel { get; private set; }

        public bool IsModified { get; private set; }

        public bool IsDeleted { get; private set; }

        public int Id { get; private set; }

        public int HpId{ get; private set; }

        public int ReportId{ get; private set; }

        public int RowNo{ get; private set; }

        public string ConfName{ get; private set; }

        public int DataSbt{ get; private set; }

        public string Columns{ get; private set; }

        public int SortKbn{ get; private set; }

        public bool IsSelected{ get; private set; }

        public string ColumnsDisplay{ get; private set; }

        public StaCsvModel ChangeIsModified(bool isModified)
        {
            IsModified = isModified;
            return this;
        }
    }
}
