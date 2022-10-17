using Domain.Models.PtTag;
using Domain.Models.UserConf;
using Entity.Tenant;
using UseCase.StickyNote;

namespace Interactor.StickyNote
{
    public class GetSettingStickyNoteInteractor : IGetSettingStickyNoteInputPort
    {
        private readonly IUserConfRepository _userConfRepository;

        public GetSettingStickyNoteInteractor(IUserConfRepository userConfRepository)
        {
            _userConfRepository = userConfRepository;
        }

        public GetSettingStickyNoteOutputData Handle(GetSettingStickyNoteInputData inputData)
        {
            if (inputData.UserId < 0) return new GetSettingStickyNoteOutputData(UpdateStickyNoteStatus.InvalidValue);

            var listUserConf = _userConfRepository.GetList(inputData.UserId,925,925);
            int fontSize = 14;
            int opacity = 200;
            int width = 570;
            int height = 300;
            int tagGrpCd = -1;

            if (listUserConf.Exists(x => x.GrpItemCd == 0))
            {
                fontSize = listUserConf.Where(x => x.GrpItemCd == 0).First().Val;
            }

            if (listUserConf.Exists(x => x.GrpItemCd == 2))
            {
                opacity = listUserConf.Where(x => x.GrpItemCd == 2).First().Val;
            }

            if (listUserConf.Exists(x => x.GrpItemCd == 3))
            {
                width = listUserConf.Where(x => x.GrpItemCd == 3).First().Val;
            }

            if (listUserConf.Exists(x => x.GrpItemCd == 4))
            {
                height = listUserConf.Where(x => x.GrpItemCd == 4).First().Val;
            }

            if (listUserConf.Exists(x => x.GrpItemCd == 5))
            {
                tagGrpCd = listUserConf.Where(x => x.GrpItemCd == 5).First().Val;
            }

            return new GetSettingStickyNoteOutputData(0, 99999999,fontSize,opacity,width,height,1,1,1,tagGrpCd);
        }
    }
}
