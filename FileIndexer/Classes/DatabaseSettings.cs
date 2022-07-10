namespace FileIndexer.Classes;
public class DatabaseSettings
{
    public string ConnectionString
    {
        get
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var connectionString = $"Data Source={documentsFolder}{Path.DirectorySeparatorChar}{DataSource};";

            return connectionString;
        }
    }

    public string DataSource { get; set; }
}