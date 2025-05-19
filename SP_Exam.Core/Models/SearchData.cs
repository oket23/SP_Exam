namespace SP_Exam.Core.Models;

public class SearchData
{
    public string FullPath { get; set; }
    public List<FileStats> FileStats {  get; set; }
    public int AllMatched => FileStats.Sum(x => x.MatchCount);
    public int FilesMatched => FileStats.Count;
    public List<string>? AllNames { get; set; }
}
