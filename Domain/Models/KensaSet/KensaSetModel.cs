using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KensaSet
{
    public class KensaSetModel
    {

        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public string SetName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string? CreateMachine { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public int UpdateId { get; private set; }

        public string? UpdateMachine { get; private set; }
    }
}
