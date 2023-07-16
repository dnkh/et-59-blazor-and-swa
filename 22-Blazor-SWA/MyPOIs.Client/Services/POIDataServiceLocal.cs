using System.Net.Http.Json;
using MyPOIs.Models;

namespace MyPOIs.Client.Services;

public class POIDataServiceLocal : IPOIDataService
{
    private readonly HttpClient _httpClient;
    private List<POIData> _pois = new List<POIData>();

    public POIDataServiceLocal(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyPOIs.LocalAPI");
        if(_httpClient == null)
        {
            throw new ArgumentNullException("HttpClient MyPOIs.LocalAPI not found in IHttpClientFactory");
        }
    }

    private async Task LoadPOIsAsync()
    {
        var pois = await _httpClient.GetFromJsonAsync<POIData[]>("api/poi");
        _pois = pois.ToList();
    }

    public async Task<IEnumerable<POIData>?> GetPOIs()
    {
        if(_pois.Count == 0)
        {
            await LoadPOIsAsync();
        }
        return _pois;
    }

    public async Task<POIData?> GetPOI(Guid id)
    {
        if (_pois is null) return null;
        
        return _pois.Where(p => p.id == id).FirstOrDefault();
    }
    public async Task<POIData?> AddPOI(POIData poi)
    {
        var foundPoi = _pois.FirstOrDefault(p => p.id == poi.id);
        if(foundPoi == null)_pois.Add(poi);
        else foundPoi = poi;

        return poi;
    }
    public async Task DeletePOI(POIData poi)
    {
        _pois.Remove(poi);
    }

}
