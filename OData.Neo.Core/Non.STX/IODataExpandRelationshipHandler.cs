using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData.Neo.Core.Non.STX
{
    public interface IODataExpandRelationshipHandler<T>
    {
        Task HandleRelationshipAsync(IQueryable<T> data, IEnumerable<string> properties);
    }
}
