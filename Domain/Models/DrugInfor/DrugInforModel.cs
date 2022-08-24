using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugInfor
{
    public class DrugInforModel
    {
        public DrugInforModel(string name, string genericName, string unit, string maker, string vender, int kohatuKbn, double ten, string receUnitName, string mark, string pathPicZai, string pathPicHou)
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
            PathPicZai = pathPicZai;
            PathPicHou = pathPicHou;
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

        public string PathPicZai { get; private set; }

        public string PathPicHou { get; private set; }

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
