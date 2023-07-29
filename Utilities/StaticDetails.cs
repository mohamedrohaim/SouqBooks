using System;
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
		public const string Vendor = "Vendor";
		public const string UserRole = "User";
		public const string EmployeeRole = "Employee";
        
        //order managment
        public const string StautsPending = "Pending";
		public const string StatusaApproved = "Approved";
		public const string StatusInProcess = "InProcess";
		public const string StatusShipped = "Shipped";
		public const string StatusCanceled= "Canceled";
		public const string StatusRefused = "Refused";


        //payment status

        public const string PaymentStautsPending = "Pending";
        public const string PaymentStatusaApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayPayment";
        public const string PaymentStatusRejected = "Rejected";

    }
}
