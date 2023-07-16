# Add /edit a new POI

## Add page to show / edit a POI
* Add a new file in the page folder and name ist Poi.razor
* Add the following code to the file
    ```csharp
    @page "/poi/{Id:guid}"

    <h2>Poi</h2>

    @code {
        [Parameter]
        public Guid Id { get; set; }
    }
    ```
* Add a new method in the POIDataServices class
    ```csharp
    public async Task<POI?> GetPOI(Guid id)
    {
        var pois = await _httpClient.GetFromJsonAsync<POI[]>("api/poi");

        if (pois is null) return null;
        
        return pois.Where(p => p.Id == id).FirstOrDefault();
    }
    ```
    * Inject the POIDataServices class in the Poi.razor page
    


## Add data annotation to the POI class
* Add the Sytem.ComponentModel.Annotations package to the MyPOIs.Model project
    ```powershell
        dotnet add .\MyPOIs.Model\ package System.ComponentModel.Annotations
    ```	
 * Add the [Required] attribute to the Name property
    ```csharp
    public class POI
    {
        public Guid	Id {get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUri { get; set; }
        public DateTime Date { get; set; }
    }
    ```



