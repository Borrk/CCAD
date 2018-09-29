using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyShare.Models
{
    public class Subscription
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string EmailAddress { get; set; }

        // The format of the subscrition
        // Type1 | Type2 | ...
        // example: an user subscribe for Furniture->table, sofa, and PET->cat, dog.
        // then the SubscribedItemTag = "table | sofa | cat | dog".
        // By using reg-expresion, we can simplify our matching of our subscrition.
        public string SubscribedItemTag { get; set; }  
    }
}
