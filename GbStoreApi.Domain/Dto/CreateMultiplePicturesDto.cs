using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Dto
{
    public class CreateMultiplePicturesDto
    {
        public required IEnumerable<CreatePictureDto> Pictures { get; set; }
        public int ProductId { get; set; }
    }
}
