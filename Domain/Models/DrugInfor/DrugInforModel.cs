using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugInfor
{
    public class DrugInforModel
    {
        public DrugInforModel(string name, string genericName, string unit, string maker, string vender, int kohatuKbn, double ten, string receUnitName, string mark, string yjCode, string pathPicZai, string pathPicHou, string defaultPathPicZai, string customPathPicZai, string otherPicZai, string defaultPathPicHou, string customPathPicHou, string otherPicHou)
        {
            Name = name;
            GenericName = genericName;
            Unit = unit;
            Maker = maker;
            Vender = vender;
            KohatuKbn = kohatuKbn;
            Ten = ten;
            ReceUnitName = receUnitName;
            Mark = mark;
            YjCode = yjCode;
            PathPicZai = pathPicZai;
            PathPicHou = pathPicHou;
            DefaultPathPicZai = defaultPathPicZai;
            CustomPathPicZai = customPathPicZai;
            OtherPicZai = otherPicZai;
            DefaultPathPicHou = defaultPathPicHou;
            CustomPathPicHou = customPathPicHou;
            OtherPicHou = otherPicHou;
        }

        public DrugInforModel()
        {
            Name = "";
            GenericName = "";
            Unit = "";
            Maker = "";
            Vender = "";
            KohatuKbn = 0;
            Ten = 0;
            ReceUnitName = "";
            Mark = "";
            YjCode = "";
            PathPicZai = "";
            PathPicHou = "";
            DefaultPathPicZai = "";
            CustomPathPicZai = "";
            OtherPicZai = "";
            DefaultPathPicHou = "";
            CustomPathPicHou = "";
            OtherPicHou = "";
        }

        public string Name { get; private set; }

        public string GenericName { get; private set; }

        public string Unit { get; private set; }

        public string Maker { get; private set; }

        public string Vender { get; private set; }

        public int KohatuKbn { get; private set; }

        public double Ten { get; private set; }

        public string ReceUnitName { get; private set; }

        public string Mark { get; private set; }

        public string YjCode { get; private set; }

        public string PathPicZai { get; set; }

        public string PathPicHou { get; set; }

        public string DefaultPathPicZai { get; set; }

        public string CustomPathPicZai { get; set; }

        public string OtherPicZai { get; set; }

        public string DefaultPathPicHou { get; set; }

        public string CustomPathPicHou { get; set; }

        public string OtherPicHou { get; set; }

        public string KohatuKbnName
        {
            get
            {
                switch (KohatuKbn)
                {
                    case 0:
                        return "なし";

                    case 1:
                        return "ー";

                    case 2:
                        return "あり";

                    default:
                        return "";
                }
            }
        }
    }
}
