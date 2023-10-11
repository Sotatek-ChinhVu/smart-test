﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Logger
{
    public class AuditLog
    {
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        public string TenantId { get; set; } = string.Empty;

        public string Domain { get; set; } = string.Empty;

        public string ThreadId { get; set; } = string.Empty;

        public string LogType { get; set; } = string.Empty;

        public int HpId { get; set; }

        public int UserId { get; set; }

        public string LoginId { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public DateTime LogDate { get; set; }

        [MaxLength(11)]
        public string? EventCd { get; set; } = string.Empty;

        public long PtId { get; set; }

        public int SinDay { get; set; }

        public long RaiinNo { get; set; }

        [MaxLength(1000)]
        public string? Path { get; set; } = string.Empty;

        public string RequestInfo { get; set; } = string.Empty;

        [MaxLength(60)]
        public string ClientIP { get; set; } = string.Empty;

        public string Desciption { get; set; } = string.Empty;
    }
}
