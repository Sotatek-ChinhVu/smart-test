using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class SyokiModel
    {
        public SyokiModel(string fukusayoInitCmt)
        {
            FukusayoInitCmt = fukusayoInitCmt;
        }

        public SyokiModel()
        {
            FukusayoInitCmt = "";
        }

        public string FukusayoInitCmt { get; private set; }
    }
}
