﻿using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCrud.Web.Models
{
    public class EmployeeType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
