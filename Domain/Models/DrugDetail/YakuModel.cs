using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class YakuModel
    {
        public YakuModel(string form, string color, string mark, string konoDetailCmt, string konoSimpleCmt, string fukusayoCmt)
        {
            Form = form;
            Color = color;
            Mark = mark;
            KonoDetailCmt = konoDetailCmt;
            KonoSimpleCmt = konoSimpleCmt;
            FukusayoCmt = fukusayoCmt;
        }

        public YakuModel()
        {
            Form = "";
            Color = "";
            Mark = "";
            KonoDetailCmt = "";
            KonoSimpleCmt = "";
            FukusayoCmt = "";
        }

        public string Form { get; private set; }

        public string Color { get; private set; }

        public string Mark { get; private set; }

        public string KonoDetailCmt { get; private set; }

        public string KonoSimpleCmt { get; private set; }

        public string FukusayoCmt { get; private set; }
    }
}
