namespace TodoMongoMvc.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public string TodoCollectionName { get; set; } = string.Empty;
}
