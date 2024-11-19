﻿using System.ComponentModel.DataAnnotations;

namespace DotNetCoreCrud.Web.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string EmployeeType { get; set; }
        [Required]
        public int EmployeeTypeId { get; set; }
    }
}
