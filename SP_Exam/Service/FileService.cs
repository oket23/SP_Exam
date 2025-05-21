using SP_Exam.Core.Dtos;
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

    public Task<SearchData> FindClassesAndInterfacesAsync(string path, MethodParamsDto dto)
    {
        Validatiod(path, dto.Cts);

        return GetResult(path, dto, _fileRepository.FindClassesAndInterfacesAsync);
    }

    public Task<SearchData> FindCopyAndReplaceWordAsync(string path, MethodParamsDto dto)
    {
        Validatiod(path, dto.CopyPath, dto.Word, dto.NewWord, dto.Cts);

        return GetResult(path, dto, _fileRepository.FindCopyAndReplaceWordAsync);
    }

    public Task<SearchData> FindWordInDirectoryAsync(string path, MethodParamsDto dto)
    {
        Validatiod(path, dto.Word, dto.Cts);

        return GetResult(path,dto, _fileRepository.FindWordInDirectoryAsync);
    }

    private async Task<SearchData> GetResult(string path, MethodParamsDto dto, Func<string, MethodParamsDto, Task<List<FileStats>>> processFilesAsync)
    {
        var result = new SearchData();
        result.FileStats = await _fileRepository.WorkWithFiles(path,dto,processFilesAsync);
        result.FullPath = await _fileRepository.CreateStatisticsFileAsync($"stats\\stats_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.json", result.FileStats, dto.Cts);

        return (result.FileStats.Count == 0) ? throw new FileNotFoundException("Nothing found...") : result;
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
