using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MedicalExamination.GetHistory;

namespace DevExpress.Models
{
    public class Karte2Model
    {
        public string Id { get; set; }
        public List<TempObj> tempObjs;
    }
    public class TempObj
    {
        public string Name { get; set; }
        public string Class { get; set; }
    }
}
