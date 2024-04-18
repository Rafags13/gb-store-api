using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class StockDto
    {
        public required string ColorName { get; set; }
        public required string SizeName { get; set; }
        public required int Amount { get; set; }
    }
}
