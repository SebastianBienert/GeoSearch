using System.Threading.Tasks;

namespace GeoSearch.API.Hubs
{
    public interface IGeoHub
    {
        Task GeoCountUpdate(long currentCount, long currentSum);
    }
}
