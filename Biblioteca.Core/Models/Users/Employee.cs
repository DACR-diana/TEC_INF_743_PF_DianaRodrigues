﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Biblioteca.Core.Models.Users
{
    public class Employee : User
    {
        public string EmployeeNumber { get; set; }
    }
}
