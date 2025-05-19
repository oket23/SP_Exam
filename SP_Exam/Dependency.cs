using SP_Exam.Core.Interfaces.Repository;
using SP_Exam.Core.Interfaces.Service;
using SP_Exam.Service;
using SP_Exam.Storage.Repository;

namespace SP_Exam;

public class Dependency
{
    private static IFileRepository GetFileRepository()
    {
        return new FileRepository();
    }
    public static IFileService GetFileService()
    {
        return new FileService(GetFileRepository());
    }
}
