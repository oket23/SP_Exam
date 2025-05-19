using SP_Exam.Core.Interfaces.Repository;
using SP_Exam.Core.Interfaces.Service;
using SP_Exam.Core.Models;

namespace SP_Exam.Service;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<SearchData> FindClassesAndInterfacesAsync(string path, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        if (string.IsNullOrEmpty(path.Trim()))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }
        if (cts.IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation canceled by user.", cts);
        }

        var result = new SearchData();
        result.FileStats = await _fileRepository.FindClassesAndInterfacesAsync(path, cts, progress);
        result.FullPath = await _fileRepository.CreateStatisticsFileAsync($"stats\\stats_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json", result.FileStats, cts);
        return (result.FileStats.Count == 0) ? throw new FileNotFoundException("Classes or interfaces not found...") : result;
    }

    public Task<SearchData> FindCopyAndReplaceWordAsync(string path, string word, string newWord, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        throw new NotImplementedException();
        //Доробити срочно
    }

    public async Task<SearchData> SearchWordInFolderAsync(string path,string word, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        if (string.IsNullOrEmpty(word.Trim()))
        {
            throw new ArgumentException("Word cannot be null or empty",nameof(word));
        }
        if (string.IsNullOrEmpty(path.Trim()))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }
        if (cts.IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation canceled by user.", cts);
        }

        var result = new SearchData();
        result.FileStats = await _fileRepository.SearchWordInFolderAsync(path, word, cts, progress);
        result.FullPath = await _fileRepository.CreateStatisticsFileAsync($"stats\\stats_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json", result.FileStats, cts);

        return (result.FileStats.Count == 0) ? throw new FileNotFoundException("Word not found...") : result;
    }
}
