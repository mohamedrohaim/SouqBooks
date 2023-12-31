﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
	public static class StaticDetails
	{
        // roles
        public static List<string> roles = new List<string>
         {
            StaticDetails.AdminRole,
            StaticDetails.CompanyRole,
            StaticDetails.UserRole,
            StaticDetails.EmployeeRole
         };
        public const string AdminRole = "Admin";
		public const string CompanyRole = "Company";
		public const string UserRole = "User";
		public const string EmployeeRole = "Employee";


		public const string StatusaInProgress = "In Progress";
		public const string StatusInTransit = "being delivered";
		public const string StatusCanceled= "Canceled";
		public const string StatusCompleted = "Completed";


        //payment status

        public const string PaymentStautsPending = "Pending";
        public const string PaymentStatusaApproved = "Approved";
        public const string PaymentStatusPayOnReciving = "Pay on receiving";
        public const string PaymentStatusRejected = "Rejected";

    }
}
