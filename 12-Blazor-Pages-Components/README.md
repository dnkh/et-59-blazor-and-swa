# Add some pages and integrated them into the menu

## Add page "Gallery"
* Add new file Gallery.razor inside Pages folder
* Start app with dotnet watch
* Add new entry in NavMenu.razor for the Gallery page


## Add second page with code behind

## Add a component
* Create folder Components
* Add new using statement in _Imports.razor for the Components folder
* Add new file POIComponent.razor
* Add POIComponent to Gallery.razor



## Add Model Project
* dotnet new classlib -n MyPOIs.Model
* dotnet add .\MyPOIS.Client references .\Model\MyPOIs.Model.csproj


## Add Solution file
* dotnet new sln -n MyPOIs
* dotnet sln add .\MyPOIS.Client
* dotnet sln add .\MyPOIs.Model

## Add POI class to model project and use it
* Add POI class to Model project
    ```csharp
    namespace MyPOIs.Model
    {
        public class POI
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string ImageUri { get; set; }
            public DateTime Date { get; set; }
        }
    }

    ```
* Add POI to Gallery.razor
    ```csharp
    @code
    {
        private IEnumerable<POI> POIs { get; set; }

        protected override void OnInitialized()
        {
            POIs = new List<POI>
            {
                new POI { Name = "Spanien" },
                new POI { Name = "Frankreich" },
                new POI { Name = "Norwegen" }
            };

        }
    }
    ```
* Use list of POIs and add POIComponent to Gallery.razor
    ```cshart
    @foreach (var poi in POIs)
    {
        <POIComponent Title="@poi.Name" />
    }
    ```
* Change POIComponent to use POI
    ```csharp
    <h3>@POIData.Name</h3>

    @code {
        [Parameter]
        public POI POIData { get; set; } = new POI();
    }
    ```
* Update usage of POIComponent in Gallery.razor
    ```csharp
    <POIComponent POI="poi" />
    ```

## Refactor Gallery and POIComponent
* Create a new componente GalleryComponent
    ```csharp
    @typeparam TItem


    @foreach (var item in Items)
    {
        @ChildContent(item)
    }

    @code {
        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = default!;

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; } = default!;
    }

    ```
* Make use of the GalleryComponent in Gallery.razor
    ```csharp
    <GalleryComponent TItem="POI" Items="POIs">
        <POIComponent POIData="context" /> 
    </GalleryComponent>
    ```




