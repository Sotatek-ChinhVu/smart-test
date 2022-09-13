using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionComment
{
    public interface IReceptionCommentRepository
    {
        List<ReceptionCommentModel> GetReceptionComments(long raiinNo);
    }
}
