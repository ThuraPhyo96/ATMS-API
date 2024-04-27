﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ATMS.Web.Dto.Models
{
    public class BankName
    {
        public int BankNameId { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Name { get; set; }

        [StringLength(MaxLength.L_12)]
        public string Code { get; set; }

        [StringLength(MaxLength.L_20)]
        public string TelephoneNumber { get; set; }

        [StringLength(MaxLength.L_100)]
        public string Email { get; set; }

        [StringLength(MaxLength.L_500)]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public List<BankBranchName> BankBranchNames { get; set; }

        public BankName()
        {
                
        }

        public BankName(string name, string code, string telephoneNumber, string email, string address, bool isActive)
        {
            Name = name;
            Code = code;
            TelephoneNumber = telephoneNumber;
            Email = email;
            Address = address;
            IsActive = isActive;
        }
    }
}