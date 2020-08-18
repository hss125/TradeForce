using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class ProductPage
    {
        public product pro { get; set; }
        public string classify { get; set; }
        public List<productproperty> pros { get; set; }
    }
}