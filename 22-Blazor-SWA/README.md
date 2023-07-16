# Add a function app as backend
## Create a local function app
* create a folder Api

* Call the Azure Functions Core Tools to create a new function app
    ```
    ```powershell
    func init
    func new --name mypois
    ```
* Select the following options:
    * dotnet
    * c#
    * HttpTrigger
## Make short test that the app works
* Run the function app locally
    ```powershell
    func start
    ```
* Open a browser and call the function app
    ```
    http://localhost:7071/api/mypois?name=.NET

    ```

## Create CosmmosDB in Azure Portal
* Database name: MyPOIs
* Container name: POIs


## Add CosmosDB to the function app
* Add the following NuGet packages to the project
    ```powershell
    dotnet add package Microsoft.Azure.WebJobs.Extensions.CosmosDB --version 3.0.10
    ```

