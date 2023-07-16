# Add a services

## Add a POIDataService
* Add the nuget package Microsoft.Extensions.DependencyInjection
    ```pswh 
    cd .\MyPOIs.Client
    dotnet add package dotnet add package Microsoft.Extensions.Http
    ```
* In Program.cs add a named HttpClient with the MyPOIs.LocalClient

    ```csharp
        AddHttpClient("MyPOIs.LocalAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "/sample-data/"));
    ```

* Create a new folder called Services inside the MyPOIs.Client project
* Create a new class called POIDataService.cs
* Add the following code to the POIDataService.cs
    ```csharp
    using System.Net.Http.Json;
    using MyPOIs.Model;

    namespace MyPOIs.Client.Services;

    public class POIDataService
    {
        private readonly HttpClient _httpClient;

        public POIDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyPOIs.LocalAPI");
            if(_httpClient == null)
            {
                throw new ArgumentNullException("HttpClient MyPOIs.LocalAPI not found in IHttpClientFactory");
            }
        }
        
        public async Task<IEnumerable<POI>?> GetPOIs()
        {
            return await _httpClient.GetFromJsonAsync<POI[]>("api/poi");
        }

    }
    ```
* Register the POIDataService in the Program.cs
    ```csharp
    builder.Services.AddScoped<POIDataService>();
    ```
* Inject the POIDataService in the Gallery.razor
    ```csharp
    @inject POIDataService POIDataService
    ```

* Replace the OninitializedAsync method in the Gallery.razor with the following code
    ```csharp
    protected async override Task OnInitializedAsync()
    {
        POIs = await POIDataService.GetPOIs();
    }
    ```

##  Show a nice POI list
* Show all POI properties
    ```csharp
    @using System.Globalization

    <div id ="POIComponent">
        <img src="@POIData.ImageUri" />
        <h4>@POIData.Name</h4>
        <p>@POIData.Description</p>
        <p>Date:@POIData.Date.ToShortDateString()</p>
        <a href="@GetMapsUrl()" target="_blank">Link to Google Maps</a>
    </div>

    @code {
        [Parameter]
        public POI POIData { get; set; } = new POI();

        private string GetMapsUrl()
        {
            string url = "https://www.google.com/maps/search/?api=1&query=";
            url += POIData.Latitude.ToString(CultureInfo.InvariantCulture) + ",";
            url += POIData.Longitude.ToString(CultureInfo.InvariantCulture);
            return url;
        }
    }
    ```
* To use CSS Isolation create a file in the components folder with the name POIComponent.razor.css
* Add the following code to the POIComponent.razor.css
    ```css
    #POIComponent {
    border:2px solid black;; 
    width:200px;
    }
    #POIComponent > img {
        max-width: 100%;
    }

    ```
* Style the Gallery.razor
    ```css
    <div style="display:flex; flex-wrap:wrap;">
        <GalleryComponent TItem="POI" Items="POIs">
            <POIComponent POIData="context" /> 
        </GalleryComponent>
    </div>
    ```





