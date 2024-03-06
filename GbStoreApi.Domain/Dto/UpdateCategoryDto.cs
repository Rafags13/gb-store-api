using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class UpdateCategoryDto
    {
        public string OldCategoryName { get; set; } = string.Empty;
        public string NewCategoryName { get; set;} = string.Empty;
    }
}
