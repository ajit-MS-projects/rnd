using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc4DateTemplate.Models
{
    public class ProductViewModel
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
    }
}