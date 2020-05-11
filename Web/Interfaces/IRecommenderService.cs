using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface IRecommenderService
    {
        Task<string> GetRanking(JsonElement userCriteria);
    }
}
