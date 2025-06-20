﻿using Dao.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cars_MVC.Models
{
    public class CarComponentCompatibility
    {
        public int CarComponentId1 { get; set; }

        public int CarComponentId2 { get; set; }

        [ValidateNever]
        public virtual CarComponent? CarComponentId1Navigation { get; set; }
        [ValidateNever]
        public virtual CarComponent? CarComponentId2Navigation { get; set; }

    }

}