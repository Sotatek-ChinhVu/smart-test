using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Diseases
{
    public class PrefixSuffixModel
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public PrefixSuffixModel(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
