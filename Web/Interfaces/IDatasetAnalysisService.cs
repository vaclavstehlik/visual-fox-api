using System.Threading.Tasks;
using Web.Models;

namespace Web.Interfaces
{
    public interface IDatasetAnalysisService
    {
        Task<DatasetAnalysis> GetDatasetFeatures(string datasetPath);
    }
}
