using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Photo_App.Models
{
    public partial class MyPhotos
    {
        public string Id { get; set; }
        public string PhotoName { get; set; }
        public string Url { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
