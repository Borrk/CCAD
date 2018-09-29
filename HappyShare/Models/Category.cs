using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyShare.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public List<SharedItem> SharedItems { get; set; }
    }
}
