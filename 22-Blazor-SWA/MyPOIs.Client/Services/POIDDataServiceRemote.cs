using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MyPOIs.Models;

namespace MyPOIs.Client.Services;

public class POIDataServiceRemote : IPOIDataService
{
    private readonly HttpClient _httpClient;
    private List<POIData> _pois = new List<POIData>();

    public POIDataServiceRemote(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyPOIs.RemoteAPI");
        if(_httpClient == null)
        {
            throw new ArgumentNullException("HttpClient MyPOIs.LocalAPI not found in IHttpClientFactory");
        }
    }

    

    public async Task<IEnumerable<POIData>?> GetPOIs()
    {
        var pois = await _httpClient.GetFromJsonAsync<POIData[]>("api/poi");
        return pois;
    }

    public async Task<POIData?> GetPOI(Guid id)
    {
        var pois = await _httpClient.GetFromJsonAsync<POIData[]>($"api/poi/{id}");
        return pois.FirstOrDefault();
        
    }
    public async Task<POIData?> AddPOI(POIData poi)
    {
        await _httpClient.PostAsJsonAsync<POIData>("api/poi", poi);

        return poi;
    }
    public async Task DeletePOI(POIData poi)
    {
        var json = JsonSerializer.Serialize(poi);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await _httpClient.DeleteAsync($"api/poi/{poi.id}");

    }

}
