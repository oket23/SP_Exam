using SP_Exam.Core.Models;

namespace SP_Exam.Core.Interfaces.Service;

public interface IFileService
{
    Task<SearchData> SearchWordInFolderAsync(string path,string word, CancellationToken cts, IProgress<Tuple<int, string>> progress);
    Task<SearchData> FindCopyAndReplaceWordAsync(string path, string word, string newWord, CancellationToken cts, IProgress<Tuple<int, string>> progress);
    Task<SearchData> FindClassesAndInterfacesAsync(string path, CancellationToken cts, IProgress<Tuple<int, string>> progress);
}
