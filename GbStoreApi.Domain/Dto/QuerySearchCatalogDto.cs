using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class QuerySearchCatalogDto
    {
        public QuerySearchCatalogDto()
        {

        }

        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Colors { get; set; } = string.Empty;
        public string Sizes { get; set; } = string.Empty;
    }
}
