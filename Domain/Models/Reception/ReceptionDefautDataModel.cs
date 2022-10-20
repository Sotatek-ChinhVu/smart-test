using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Reception
{
    public class ReceptionDefautDataModel
    {
        public ReceptionDefautDataModel(int defaultTantoId, int defaultKaId)
        {
            DefaultTantoId = defaultTantoId;
            DefaultKaId = defaultKaId;
        }
        public ReceptionDefautDataModel()
        {
        }

        public int DefaultTantoId { get; set; }

        public int DefaultKaId { get; set; }
    }
}
