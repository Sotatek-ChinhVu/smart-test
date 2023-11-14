using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UseCase.XmlDTD.OnlineConfirmHistoryData
{
    [Serializable]
    [XmlRoot(ElementName = "ResultList")]
    public class ConfirmResultResultList
    {
        [XmlElement(ElementName = "ResultOfQualificationConfirmation")]
        public ConfirmResultResultOfQualificationConfirmation ResultOfQualificationConfirmation { get; set; } = new();
    }
}
