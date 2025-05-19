namespace SP_Exam.Core.Models;

public class FileStats
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public int MatchCount { get; set; }
    public List<string>? Names { get; set; }
}
