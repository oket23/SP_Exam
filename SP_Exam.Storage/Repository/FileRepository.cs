using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using SP_Exam.Core.Interfaces.Repository;
using SP_Exam.Core.Models;

namespace SP_Exam.Storage.Repository;

public class FileRepository : IFileRepository
{
    //private string splitArray = " .,!?\n\r\t()[]{}\"\\"; 
    private static readonly object _lock = new();
    public async Task<string> CreateStatisticsFileAsync(string path,List<FileStats> filesStats, CancellationToken cts)
    {
        var directoryPath = Path.GetDirectoryName(path);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
            WriteIndented = true
        };

        using (var fs = new FileStream(path, FileMode.Create,FileAccess.Write)){
            await JsonSerializer.SerializeAsync(fs, filesStats,options,cancellationToken: cts);
        }
        return Path.GetFullPath(path);
    }

    public async Task<List<FileStats>> FindClassesAndInterfacesAsync(string path, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        var result = new List<FileStats>();

        var allFiles = Directory.GetFiles(path,"*.cs");
        var allFolders = Directory.GetDirectories(path);

        var totalItems = allFiles.Length + allFolders.Length;
        var progressItem = 0;

        var FileTask = allFiles.Select(async x =>
        {
            cts.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(x, cts);
            var matches = Regex.Matches(text, @"^\s*(?:\b\w+\s+)*?(class|interface)\s+(\w+)",RegexOptions.Multiline);
            var names = new List<string>();

            foreach (Match match in matches)
            {
                names.Add($"{match.Groups[1].Value} {match.Groups[2].Value}");
            }

            if (matches.Count > 0)
            {
                result.Add(new()
                {
                    FileName = Path.GetFileName(x),
                    FilePath = Path.GetFullPath(x),
                    MatchCount = matches.Count,
                    Names = names
                });

                var current = Interlocked.Increment(ref progressItem);
                progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
            }
        }).ToArray();

        await Task.WhenAll(FileTask);

        var FolderTask = allFolders.Select(async x =>
        {
            cts.ThrowIfCancellationRequested();

            var tempResult = await FindClassesAndInterfacesAsync(x, cts, progress);
            lock (_lock)
            {
                result.AddRange(tempResult);
            }

            var current = Interlocked.Increment(ref progressItem);
            progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
        }).ToArray();

        await Task.WhenAll(FolderTask);

        return result;
    }

    public Task<List<FileStats>> FindCopyAndReplaceWordAsync(string path, string word, string newWord, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        throw new NotImplementedException();
    }

    public async Task<List<FileStats>> SearchWordInFolderAsync(string path, string word, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    {
        var result = new List<FileStats>();

        var allFiles = Directory.GetFiles(path);
        var allFolders = Directory.GetDirectories(path);

        var totalItems = allFiles.Length + allFolders.Length;
        var progressItem = 0;

        var FileTask = allFiles.Select(async x =>
        {
            cts.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(x, cts);
            int matches = Regex.Matches(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase).Count;

            //Повільніше на 1 хв на мому ноутбуці Lenovo (регулярний вираз 1.40 хв)
            //int matches = text.Split(splitArray.ToCharArray(),StringSplitOptions.RemoveEmptyEntries)
            //            .Count(x => string.Equals(x,word,StringComparison.OrdinalIgnoreCase));

            if (matches > 0)
            {
                result.Add(new()
                {
                    FileName = Path.GetFileName(x),
                    FilePath = Path.GetFullPath(x),
                    MatchCount = matches
                });

                var current = Interlocked.Increment(ref progressItem);
                progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
            }
        }).ToArray();

        await Task.WhenAll(FileTask);

        var FolderTask = allFolders.Select(async x =>
        {
            cts.ThrowIfCancellationRequested();

            var tempResult = await SearchWordInFolderAsync(x, word, cts, progress);
            lock (_lock)
            {
                result.AddRange(tempResult);
            }

            var current = Interlocked.Increment(ref progressItem);
            progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
        }).ToArray();

        await Task.WhenAll(FolderTask);
        
        return result;
    }

}
