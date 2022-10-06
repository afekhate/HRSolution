﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.GStaff
{
    public class Staff : BaseObject
    {
        public string? stringType { get; set; }
        public string? stringId { get; set; }
        public string? deletionTimestamp { get; set; }
        public bool? accountEnabled { get; set; }
        public string? city { get; set; }
        public string? companyName { get; set; }
        public string? country { get; set; }
        public DateTime? createdDateTime { get; set; }
        public string? creationType { get; set; }
        public string? department { get; set; }
        public string? displayName { get; set; }
        public string? employeeId { get; set; }
        public string? givenName { get; set; }
        public string? immutableId { get; set; }
        public string? isCompromised { get; set; }
        public string? jobTitle { get; set; }
        public string? lastDirSyncTime { get; set; }
        public string? mail { get; set; }
        public string? mailNickname { get; set; }
        public string? mobile { get; set; }
        public string? postalCode { get; set; }
        public string? preferredLanguage { get; set; }
        public string? state { get; set; }
        public string? streetAddress { get; set; }
        public string? surname { get; set; }
        public string? telephoneNumber { get; set; }
        public string? usageLocation { get; set; }
    }
}
