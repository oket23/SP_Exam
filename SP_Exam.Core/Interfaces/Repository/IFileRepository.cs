using SP_Exam.Core.Models;

namespace SP_Exam.Core.Interfaces.Repository;

public interface IFileRepository
{
    Task<List<FileStats>> SearchWordInFolderAsync(string path,string word,CancellationToken cts, IProgress<Tuple<int, string>> progress);
    Task<List<FileStats>> FindCopyAndReplaceWordAsync(string path,string word,string newWord, string copyPath, CancellationToken cts, IProgress<Tuple<int, string>> progress);
    Task<List<FileStats>> FindClassesAndInterfacesAsync(string path, CancellationToken cts, IProgress<Tuple<int, string>> progress);
    Task<string> CreateStatisticsFileAsync(string path,List<FileStats> filesStats,CancellationToken cts);
}
