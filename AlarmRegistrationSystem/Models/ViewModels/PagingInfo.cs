﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models.ViewModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public PagingInfo()
        {
            TotalItems = 0;
            ItemsPerPage = 1;
            CurrentPage = 1;
        }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / (decimal)ItemsPerPage);
    }
}
