using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MonshinInf
{
    public class MonshinSaveDto
    {
        public MonshinSaveDto(MonshinInforModel monshinInforModel)
        {
            MonshinInforModel = monshinInforModel;
        }
        public MonshinInforModel MonshinInforModel { get; set; }
    }
}
