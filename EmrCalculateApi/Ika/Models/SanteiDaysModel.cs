using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class SanteiDaysModel 
    {
        private int _sinDate;
        private string _itemCd;
        private string _odrItemCd;
        private int _santeiKbn;
        private int _hokenKbn;

        public SanteiDaysModel(int sinDate, string itemCd)
        {
            _sinDate = sinDate;
            _itemCd = itemCd;
            _odrItemCd = itemCd;
            _santeiKbn = -1;
            _hokenKbn = -1;
        }
        public SanteiDaysModel(int sinDate, string itemCd, string odrItemCd)
        {
            _sinDate = sinDate;
            _itemCd = itemCd;
            _odrItemCd = odrItemCd;
            _santeiKbn = -1;
            _hokenKbn = -1;
        }
        public SanteiDaysModel(int sinDate, string itemCd, string odrItemCd, int santeiKbn)
        {
            _sinDate = sinDate;
            _itemCd = itemCd;
            _odrItemCd = odrItemCd;
            _santeiKbn = santeiKbn;
            _hokenKbn = -1;
        }
        public SanteiDaysModel(int sinDate, string itemCd, string odrItemCd, int santeiKbn, int hokenKbn)
        {
            _sinDate = sinDate;
            _itemCd = itemCd;
            _odrItemCd = odrItemCd;
            _santeiKbn = santeiKbn;
            _hokenKbn = hokenKbn;
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
        }

        /// <summary>
        /// ITEM_CD
        /// </summary>
        public string ItemCd
        {
            get { return _itemCd ?? string.Empty; }
        }
        public string OdrItemCd
        {
            get { return _odrItemCd ?? string.Empty; }
        }
        /// <summary>
        /// 算定区分
        /// </summary>
        public int SanteiKbn
        {
            get { return _santeiKbn; }
        }
        /// <summary>
        /// 保険区分
        /// </summary>
        public int HokenKbn
        {
            get { return _hokenKbn; }
        }
    }
}
