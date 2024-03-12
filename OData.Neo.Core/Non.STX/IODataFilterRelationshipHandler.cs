using OData.Neo.Core.Models.OTokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData.Neo.Core.Non.STX
{
    public interface IODataFilterRelationshipHandler<T>
    {
        Task HandleRelationshipAsync(IQueryable<T> data, IEnumerable<OToken> tokens);
    }
}
