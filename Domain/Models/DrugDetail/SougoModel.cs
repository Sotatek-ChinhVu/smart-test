using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class SougoModel
    {
        public SougoModel(string interactionPatCd)
        {
            InteractionPatCd = interactionPatCd;
        }

        public SougoModel()
        {
            InteractionPatCd = "";
        }

        public string InteractionPatCd { get; private set; }
    }
}
