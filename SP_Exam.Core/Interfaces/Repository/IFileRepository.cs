using SP_Exam.Core.Dtos;
using SP_Exam.Core.Models;

namespace SP_Exam.Core.Interfaces.Repository;

public interface IFileRepository
{
    Task<string> CreateStatisticsFileAsync(string path,List<FileStats> filesStats,CancellationToken cts);
    Task<List<FileStats>> WorkWithFiles(string path, MethodParamsDto dto, Func<string, MethodParamsDto, Task<List<FileStats>>> processFilesAsync);
    Task<List<FileStats>> FindClassesAndInterfacesAsync(string path, MethodParamsDto dto);
    Task<List<FileStats>> FindCopyAndReplaceWordAsync(string path, MethodParamsDto dto);
    Task<List<FileStats>> FindWordInDirectoryAsync(string path, MethodParamsDto dto);

}
