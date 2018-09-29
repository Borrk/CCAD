using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyShare.Models.SharedItemViewModels
{
    public class SharedItemViewModel
    {
        public List<SharedItem> SharedItems { get; set; }
        public List<Category> Categories { get; set; }
    }
}
