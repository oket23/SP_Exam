using SP_Exam.Core.Interfaces.Repository;
using SP_Exam.Core.Interfaces.Service;

namespace SP_Exam.Service;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

}
