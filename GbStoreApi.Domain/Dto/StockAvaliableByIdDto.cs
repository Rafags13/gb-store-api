using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class StockAvaliableByIdDto
    {
        public int StockId { get; set; }
        public bool IsAvaliable { get; set; }
    }
}
