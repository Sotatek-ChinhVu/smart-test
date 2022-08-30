using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class FukuModel
    {
        public FukuModel(string fukusayoCd)
        {
            FukusayoCd = fukusayoCd;
        }

        public FukuModel()
        {
            FukusayoCd = "";
        }

        public string FukusayoCd { get; private set; }
    }
}
