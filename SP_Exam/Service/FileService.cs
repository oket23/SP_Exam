using System.IO;
using Microsoft.VisualBasic.Devices;
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
        Validatiod(path, cts);

        return await GetResult(path,cts,progress);
    }

    public async Task<SearchData> FindCopyAndReplaceWordAsync(string path, string word, string newWord, string copyPath, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        Validatiod(path,copyPath,word,newWord, cts);

        return await GetResult(path, word,newWord,copyPath, cts, progress);
    }

    public async Task<SearchData> SearchWordInFolderAsync(string path,string word, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {

        Validatiod(path, word, cts);

        return await GetResult(path,word,cts, progress);
    }

    private async Task<SearchData> GetResult(string path, string word,CancellationToken cts,IProgress<Tuple<int,string>> progress)
    {
        var result = new SearchData();
        result.FileStats = await _fileRepository.SearchWordInFolderAsync(path, word, cts, progress);
        result.FullPath = await _fileRepository.CreateStatisticsFileAsync($"stats\\stats_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json", result.FileStats, cts);

        return (result.FileStats.Count == 0) ? throw new FileNotFoundException("Word not found...") : result;
    }
    private async Task<SearchData> GetResult(string path, string word,string newWord, string copyPath, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        var result = new SearchData();
        result.FileStats = await _fileRepository.FindCopyAndReplaceWordAsync(path, word, newWord, copyPath, cts, progress);
        result.FullPath = await _fileRepository.CreateStatisticsFileAsync($"stats\\stats_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json", result.FileStats, cts);

        return (result.FileStats.Count == 0) ? throw new FileNotFoundException("Word not found...") : result;
    }
    private async Task<SearchData> GetResult(string path, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        var result = new SearchData();
        result.FileStats = await _fileRepository.FindClassesAndInterfacesAsync(path, cts, progress);
        result.FullPath = await _fileRepository.CreateStatisticsFileAsync($"stats\\stats_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json", result.FileStats, cts);
        return (result.FileStats.Count == 0) ? throw new FileNotFoundException("Classes or interfaces not found...") : result;
    }

    private void Validatiod(string path,CancellationToken cts) 
    {
        if (string.IsNullOrEmpty(path.Trim()))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }
        if (cts.IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation canceled by user.", cts);
        }
    }
    private void Validatiod(string path,string word, CancellationToken cts)
    {
        if (string.IsNullOrEmpty(path.Trim()))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }
        if (cts.IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation canceled by user.", cts);
        }
        if (string.IsNullOrEmpty(word.Trim()))
        {
            throw new ArgumentException("Word cannot be null or empty", nameof(word));
        }
    }
    private void Validatiod(string path, string copyPath , string word, string newWord, CancellationToken cts)
    {
        if (string.IsNullOrEmpty(path.Trim()))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }
        if (cts.IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation canceled by user.", cts);
        }
        if (string.IsNullOrEmpty(word.Trim()))
        {
            throw new ArgumentException("Word cannot be null or empty", nameof(word));
        }
        if (string.IsNullOrEmpty(newWord.Trim()))
        {
            throw new ArgumentException("New word cannot be null or empty", nameof(newWord));
        }
        if (string.IsNullOrEmpty(copyPath.Trim()))
        {
            throw new ArgumentException("Copy path cannot be null or empty", nameof(copyPath));
        }
    }

}
