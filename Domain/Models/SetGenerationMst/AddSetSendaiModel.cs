using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SetGenerationMst
{
    public class AddSetSendaiModel
    {
        public AddSetSendaiModel(int targetGeneration, int sourceGeneration)
        {
            TargetGeneration = targetGeneration;
            SourceGeneration = sourceGeneration;
        }

        public int TargetGeneration { get; private set; }
        public int SourceGeneration { get; private set; }
    }
}
