using System.Text.Json.Serialization;

namespace Domain.Models.Accounting
{
    public class AccountingFormMstModel
    {
        [JsonConstructor]
        public AccountingFormMstModel(int hpId, int formNo, string formName, int formType, int printSort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, string form, int @base, int sortNo, int isDeleted, DateTime createDate, DateTime updateDate, int createId, int updateId, bool modelModified)
        {
            HpId = hpId;
            FormNo = formNo;
            FormName = formName;
            FormType = formType;
            PrintSort = printSort;
            MiseisanKbn = miseisanKbn;
            SaiKbn = saiKbn;
            MisyuKbn = misyuKbn;
            SeikyuKbn = seikyuKbn;
            HokenKbn = hokenKbn;
            Form = form;
            Base = @base;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            UpdateDate = updateDate;
            ModelModified = modelModified;
            CreateId = createId;
            UpdateId = updateId;
        }

        public int HpId { get; private set; }

        public int FormNo { get; private set; }

        public string FormName { get; private set; }

        public int FormType { get; private set; }

        public int PrintSort { get; private set; }

        public int MiseisanKbn { get; private set; }

        public int SaiKbn { get; private set; }

        public int MisyuKbn { get; private set; }

        public int SeikyuKbn { get; private set; }

        public int HokenKbn { get; private set; }

        public string Form { get; private set; }

        public int Base { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public int CreateId { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int UpdateId { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public bool ModelModified { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(FormName)
                && string.IsNullOrEmpty(Form)
                && FormType == 0
                && PrintSort == 0
                && MiseisanKbn == 0
                && SaiKbn == 0
                && MisyuKbn == 0
                && SeikyuKbn == 0
                && HokenKbn == 0
                && Base == 0;
        }
    }
}
