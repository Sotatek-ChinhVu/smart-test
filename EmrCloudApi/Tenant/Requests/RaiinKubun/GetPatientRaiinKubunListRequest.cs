<<<<<<<< HEAD:EmrCloudApi/Tenant/Requests/PatientRaiinKubun/PatientRaiinKubunRequest.cs
﻿namespace EmrCloudApi.Tenant.Requests.PatientRaiinKubun
========
﻿namespace EmrCloudApi.Requests.RaiinKubun
>>>>>>>> develop:EmrCloudApi/Tenant/Requests/RaiinKubun/GetPatientRaiinKubunListRequest.cs
{
    public class GetPatientRaiinKubunListRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int RaiinNo { get; set; }

        public int SinDate { get; set; }
    }
}
