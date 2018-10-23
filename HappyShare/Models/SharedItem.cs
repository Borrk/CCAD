using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public DateTime PostTime { get; set; }

        [NotMapped]
        public string PostElapsedDays
        {
            get {
                int days = (int)((DateTime.Now - PostTime).TotalDays);
                return (days < 1) ? "Posted: Today" :"Posted: " + days.ToString() + " days ago";
            }
        }

        [NotMapped]
        public float Lat
        {
            get
            {
                var resource = JObject.Parse(Location);
                var lat = resource["lat"];
                return lat.Value<float>();
            }
        }

        [NotMapped]
        public float Lng
        {
            get
            {
                var resource = JObject.Parse(Location);
                var lng = resource["lng"];
                return lng.Value<float>();
            }
        }

        [StringLength(200)]
        public string Description { get; set; }

        public Category Category { get; set; }
    }
}
