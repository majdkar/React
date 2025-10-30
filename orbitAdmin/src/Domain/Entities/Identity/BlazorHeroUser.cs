using SchoolV01.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Domain.Interfaces.Chat;
using SchoolV01.Domain.Models.Chat;

namespace SchoolV01.Domain.Entities.Identity
{
    public class BlazorHeroUser : IdentityUser<string>, IChatUser, IAuditableEntity<string>
    {
        public BlazorHeroUser()
        {
            ChatHistoryFromUsers = [];
            ChatHistoryToUsers = [];
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string HomePhoneNumber { get; set; }
        public string ClientType { get; set; }

        [Column(TypeName = "text")]
        public string PictureUrl { get; set; }
        public string Address { get; set; }

        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool Deleted { get; set; } = false;

        public ICollection<ChatHistory<BlazorHeroUser>> ChatHistoryFromUsers { get; set; }
        public ICollection<ChatHistory<BlazorHeroUser>> ChatHistoryToUsers { get; set; }

        public void DeleteUser()
        {
            Deleted = true;
            //Email = null;
            //NormalizedEmail = null;
            EmailConfirmed = false;
            UserName = null;
            NormalizedUserName = null;
            IsActive = false;
        }

    }

}