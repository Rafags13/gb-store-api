using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class CreateStockWithIdDto
    {
        public CreateStockWithIdDto(){}
        public int ProductId { get; set; }
        public IEnumerable<CreateStockDto> Variants { get; set; } = Enumerable.Empty<CreateStockDto>();
    }
}
