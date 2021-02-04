using System.Data;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPostgreConnection
    {
        Task NonQuery(string query, string conn);
        Task<DataSet> ReturnQuery(string query, string conn);
    }
}
