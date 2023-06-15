using Pulumi;
using Pulumi.Azure.ApiManagement.Outputs;
using Pulumi.Azure.AppInsights;
using Pulumi.Azure.AppService;
using Pulumi.Azure.AppService.Inputs;
using Pulumi.Azure.Bot;
using Pulumi.Azure.Core;
using Pulumi.Azure.PostgreSql;
using Pulumi.Azure.Storage;
using System.Collections.Generic;

return await Deployment.RunAsync(() =>
{
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("joireajr");

    // Create an Azure Storage Account
    var storageAccount = new Account("joireajrstorage", new AccountArgs
    {
        ResourceGroupName = resourceGroup.Name,
        AccountReplicationType = "LRS",
        AccountTier = "Standard"
    });

    var applicationInsights = new Insights("", new InsightsArgs()
    {
        ResourceGroupName = resourceGroup.Name,
        RetentionInDays = 30
    });

    var servicePlan = new ServicePlan("eatae", new ServicePlanArgs()
    {
        ResourceGroupName = resourceGroup.Name,
        Location = resourceGroup.Location,
        OsType = "Linux",
        SkuName = "P1v2",
    });

    var sql = new FlexibleServer("joireajrserver", new FlexibleServerArgs()
    {
        ResourceGroupName = resourceGroup.Name,
        Version = "14",
        AdministratorLogin = "psqladmin",
        AdministratorPassword = "H@Sh1CoR3!",
        BackupRetentionDays = 7,
        StorageMb = 32768,
        SkuName = "B_Standard_B1ms"

    });
    var db = new FlexibleServerDatabase("deagaegdb", new FlexibleServerDatabaseArgs()
    {
        ServerId = sql.Id,
        Collation = "en_US.utf8",
        Charset = "utf8",
    });
    
    

    var webApp = new LinuxWebApp("joireajrsd", new LinuxWebAppArgs()
    {
        ResourceGroupName = resourceGroup.Name,
        AppSettings = new InputMap<string>()
        {
            {"ConnectionString", "test"},
        },
        ServicePlanId = servicePlan.Id,
        SiteConfig = new LinuxWebAppSiteConfigArgs()
        {
            AlwaysOn=false
            
        }
    });

    
    
    // Export the connection string for the storage account
    return new Dictionary<string, object?>
    {
        
        ["connectionString"] = $"Server={sql.Name}.postgres.database.azure.com;Database=blazorbase;Port=5432;User Id=;Password=;Ssl Mode=VerifyFull;"
    };
});
