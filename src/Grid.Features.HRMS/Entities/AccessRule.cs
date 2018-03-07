using Grid.Features.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class AccessRule: EntityBase
    {
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(300)]
        public string PasswordQuestion { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string PasswordAnswer { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string PasswordResetCode { get; set; }

        public DateTime? LastPasswordResetDate { get; set; }

        public bool EmailVerified { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string EmailVerificationCode { get; set; }

        public DateTime? EmailVerificationDate { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public DateTime? LastLockoutDate { get; set; }

        public static AccessRule CreateNewUserAccessRule(bool isApproved)
        {
            return new AccessRule
            {
                IsApproved = isApproved,
                LastActivityDate = DateTime.UtcNow,
                EmailVerificationCode = Guid.NewGuid().ToString("N")
            };
        }
    }
}
