using MyPOIs.Models;

namespace MyPOIs.Client.Services;
public interface IPOIDataService
{
    Task<IEnumerable<POIData>?> GetPOIs();
    Task<POIData?> GetPOI(Guid id);
    Task<POIData?> AddPOI(POIData poi);
    Task DeletePOI(POIData poi);

}