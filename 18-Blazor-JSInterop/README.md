# Javascrip Interop / Get geolocation

## Add a new javascript file to the wwwroot folder
* Add a new file to the wwwroot folder and name it geolocation.js
* add the folowing code to the file
    ```javascript
   window.mypois = { 
    sayHello: (name) => {
        console.log('Hello ' + name);
        alert('Hello ' + name);
      }
    };
    ```
## Call the SayHello method from the Poi.razor page
* Add a new button element to the Poi.razor page before the image element
    
    ```html
    <br/>
    <button class="btn btn-primary" @onclick="SayHello">Say Hello</button>
    ```
* Inject the JSRuntime in the Poi.razor page
    ```csharp
    @inject IJSRuntime JSRuntime
    ```
* Call the SayHello method from the Poi.razor page

    ```csharp
    
        private async Task SayHello()
        {
            await JSRuntime.InvokeVoidAsync("mypois.sayHello", "from .NET");
        }
    ```

## Get the geolocation from the browser
* Add the flowing code to the geolocation.js file
       
    ```javascript
    getLocation: (objectref) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                function (position) {
                    console.log('Location changed: ' + position.coords.latitude + ', ' + position.coords.longitude);
                    objectref.invokeMethodAsync('LocationChanged', position.coords.latitude, position.coords.longitude);
                },
                function (error) {
                    console.log('Location error: ' + error.message);
                    objectref.invokeMethodAsync('LocationError', error.message);
                },
                { enableHighAccuracy: true, timeout: 5000, maximumAge: 0 });
        } else {
            objectref.invokeMethodAsync('LocationError', 'Geolocation is not supported by this browser.');
        }
    }
      
    ```
    * Add change the button onclick call to the getLocation method
        ```html
        <button class="btn btn-primary" @onclick="GetCurrentPosition">Get current position</button>
        ```

    * Add a local variable to the Poi.razor page
        ```csharp
        private DotNetObjectReference<Poi>? objRef;
        ```
    * Add the following code to the OnInitialized method
        ```csharp
        objRef = DotNetObjectReference.Create(this);
        ```

    * Add the GetCurrentPosition and the callbacks method to the Poi.razor page
        ```csharp
        private async Task GetCurrentPosition()
        {
            await JSRuntime.InvokeVoidAsync("mypois.getLocation", objRef);
        }

        [JSInvokable]
        public void LocationChanged(double Latitude, double Longitude)
        {
            poiData.Latitude = Latitude;
            poiData.Longitude = Longitude; 

            Console.WriteLine($"LocationChanged called: lat={Latitude}, lon={Longitude}");
            StateHasChanged();
        }

        [JSInvokable]
        public static void LocationError(string errorMessage)
        {
            Console.WriteLine($"LocationError: {errorMessage} ");
        }
        ```
## Publish the application
* Publish the application to the wwwroot folder
    ```powershell
    dotnet publish -c Release -o .\publish\wwwroot
    ```
* Open the Azure portal, create a resource group and a storage account
* In the storage account go to "Static website" and enable the static website
* In the 2 input fields enter index.html and save the changes
* Use VSCode, or Storage Explorer to upload the files from the publish\wwwroot folder to the $web container in the storage account
* Open the Url of the static website in a browser and test the application

 

