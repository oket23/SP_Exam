using SP_Exam.Core.Dtos;
using SP_Exam.Core.Models;

namespace SP_Exam.Core.Interfaces.Service;

public interface IFileService
{
    Task<SearchData> FindWordInDirectoryAsync(string path, MethodParamsDto dto);
    Task<SearchData> FindCopyAndReplaceWordAsync(string path, MethodParamsDto dto);
    Task<SearchData> FindClassesAndInterfacesAsync(string path, MethodParamsDto dto);
}
