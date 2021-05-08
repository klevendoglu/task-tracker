﻿using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using TaskTracker.Authorization.Users;
using TaskTracker.MultiTenancy;

namespace TaskTracker.Web.Models.TenantRegistration
{
    public class TenantRegistrationViewModel : IInputDto
    {
        [Required]
        [StringLength(Tenant.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(User.MaxPlainPasswordLength)]
        public string AdminPassword { get; set; }
    }
}