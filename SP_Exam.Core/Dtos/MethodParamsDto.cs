namespace SP_Exam.Core.Dtos;

public class MethodParamsDto
{
    public string? Word { get; set; }
    public string? NewWord { get; set; }
    public string? CopyPath { get; set; }
    public CancellationToken Cts { get; set; }
    public IProgress<Tuple<int, string>> Progress { get; set; }
    public int TotalItem { get; set; }
    public int _progressItem; 
}
