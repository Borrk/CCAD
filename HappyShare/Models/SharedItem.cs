using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyShare.Models
{
    public class SharedItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PictureLink { get; set; }

        public string Location { get; set; }
        public string Address { get; set; }

        public string ContactorPhone { get; set; }
        public string ContactorEmail { get; set; }

        public string Type { get; set; } // item type, like table, sofa, bed, as so on

        [StringLength(200)]
        public string Description { get; set; }

        public Category Category { get; set; }
    }
}
