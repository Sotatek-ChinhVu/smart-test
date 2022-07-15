using Domain.Models.SpecialNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SpecialNote.Read;

namespace Interactor.SpecialNote
{
    public class GetSpecialNoteInteractor : IGetSpecialNoteInputPort
    {
        private readonly ISpecialNoteRepository _specialNoteRepository;
        public GetSpecialNoteInteractor(ISpecialNoteRepository specialNoteRepository)
        {
            _specialNoteRepository = specialNoteRepository;
        }

        public GetSpecialNoteOutputData Handle(GetSpecialNoteInputData inputData)
        {
            return new GetSpecialNoteOutputData(_specialNoteRepository.Get(inputData.PtId, inputData.SinDate));
        }
    }
}
