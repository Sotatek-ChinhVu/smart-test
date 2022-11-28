﻿using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.VisitingList;
using UseCase.VisitingList.ReceptionLock;

namespace EmrCloudApi.Presenters.VisitingList
{
    public class GetReceptionLockPresenter : IGetReceptionLockOutputPort
    {
        public Response<GetReceptionLockRespone> Result { get; private set; } = new Response<GetReceptionLockRespone>();

        public void Complete(GetReceptionLockOutputData outputData)
        {
            Result.Data = new GetReceptionLockRespone(outputData.ListLockModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetReceptionLockStatus status) => status switch
        {
            GetReceptionLockStatus.Success => ResponseMessage.Success,
            GetReceptionLockStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            GetReceptionLockStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
