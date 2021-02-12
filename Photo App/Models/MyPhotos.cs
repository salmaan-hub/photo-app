﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Photo_App.Models
{
    public partial class MyPhotos
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhotoName { get; set; }
        public string Url { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        [DisplayName("Upload")]
        public IFormFile ImageFile { get; set; }
    }
}
