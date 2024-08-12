using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Interfaces
{
    public interface IBucketService
    {
        Task<IEnumerable<string>> GetAllBuckets();
        Task<string?> GetCurrentPictureBucket();
    }
}
