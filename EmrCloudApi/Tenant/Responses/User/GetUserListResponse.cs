﻿using Domain.Models.User;

namespace EmrCloudApi.Tenant.Responses.User
{
    public class GetUserListResponse
    {
        public GetUserListResponse(List<UserMstModel> users)
        {
            Users = users;
        }

        public List<UserMstModel> Users { get; private set; }
    }
}
