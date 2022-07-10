using FileIndexer.Classes;
using FileIndexer.Models;
using FileIndexer.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileIndexer.Services;
public interface IIndexerService
{
    public string Name { get; }

    public void IndexFiles();
}

public class IndexerService : IIndexerService
{
    public string Name { get; }
    private readonly ILogger<IndexerService> _Logger;
    private readonly IRepository<MediaFile> _MediaFileRepository;
    private readonly IndexerSettings _IndexerSettings;
    private readonly IStringTokenizer _StringTokenizer;
    private readonly TagRepository _TagRepository;
    private readonly IRepository<MediaFileTag> _MediaFileTagRepository;

    public IndexerService(ILogger<IndexerService> logger, IRepository<MediaFile> mediaFileRepository, IOptions<IndexerSettings> indexerSettings, IStringTokenizer stringTokenizer, TagRepository tagRepository, IRepository<MediaFileTag> mediaFileTag)
    {
        Name = nameof(IndexerService);
        _Logger = logger;
        _MediaFileRepository = mediaFileRepository;
        _IndexerSettings = indexerSettings.Value;

        _Logger.Log(LogLevel.Information, $"{Name} started");
        _StringTokenizer = stringTokenizer;
        _TagRepository = tagRepository;
        _MediaFileTagRepository = mediaFileTag;
    }


    public void IndexFiles()
    {
        _Logger.Log(LogLevel.Information, "Indexing files in folders");

        var extensions = _IndexerSettings.Extensions;
        foreach (var folder in _IndexerSettings.Folders)
        {
            _Logger.Log(LogLevel.Information, $"Indexing folder: {folder}");
            var rootPath = new DirectoryInfo(folder);
            var files = rootPath.EnumerateFiles("*.*", SearchOption.AllDirectories);

            _Logger.Log(LogLevel.Information, $"Found {files.Count()} files to index");
            foreach (var file in files)
            {
                if (extensions.Contains(file.Extension))
                {
                    _Logger.Log(LogLevel.Information, $"Found file: {file.Name}");
                    var mediaFile = new MediaFile(file);
                    var allTags = _StringTokenizer.GetAllTokens(file.Name, file.Directory);

                    var mediaFileId = _MediaFileRepository.Create(mediaFile);
                    var tags = new List<Tag>();
                    //Make sure tags exist in the DB and get tag IDs
                    foreach (var tag in allTags)
                    {
                        var newTag = new Tag(tag);

                        var tagId = _TagRepository.EnsureExists(newTag);
                        var newMediaFileTag = new MediaFileTag(mediaFileId, tagId);
                        _MediaFileTagRepository.Create(newMediaFileTag);
                    }

                }
            }
        }
    }
}