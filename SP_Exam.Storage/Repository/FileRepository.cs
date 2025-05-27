using SP_Exam.Core.Dtos;
using SP_Exam.Core.Interfaces.Repository;
using SP_Exam.Core.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SP_Exam.Storage.Repository;

public class FileRepository : IFileRepository
{
    public async Task<string> CreateStatisticsFileAsync(string path, List<FileStats> filesStats, CancellationToken cts)
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

        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            await JsonSerializer.SerializeAsync(fs, filesStats, options, cancellationToken: cts).ConfigureAwait(false);
        }
        return Path.GetFullPath(path);
    }

    public async Task<List<FileStats>> WorkWithFiles(string path, MethodParamsDto dto, Func<string, MethodParamsDto, Task<List<FileStats>>> processFilesAsync)
    {
        var result = new List<FileStats>();

        var allFiles = Directory.GetFiles(path);
        var allFolders = Directory.GetDirectories(path);

        dto.TotalItem = CountAllFiles(path);
        dto._progressItem = 0;

        var fileTasks = allFiles.Select(x => processFilesAsync(x, dto)).ToArray();
        var fileResults = await Task.WhenAll(fileTasks);

        foreach (var fileList in fileResults)
        {
            result.AddRange(fileList);
        }

        var folderTasks = allFolders.Select(x => processFilesAsync(x, dto)).ToArray();
        var folderResults = await Task.WhenAll(folderTasks);

        foreach (var folderList in folderResults)
        {
            result.AddRange(folderList);
        }

        return result;
    }

    public async Task<List<FileStats>> FindWordInDirectoryAsync(string path, MethodParamsDto dto)
    {
        var result = new List<FileStats>();

        if (File.Exists(path))
        {
            dto.Cts.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(path, dto.Cts).ConfigureAwait(false);
            int matches = Regex.Matches(text, $@"\b{Regex.Escape(dto.Word)}\b", RegexOptions.IgnoreCase).Count;

            if (matches > 0)
            {
                result.Add(new()
                {
                    FileName = Path.GetFileName(path),
                    FilePath = Path.GetFullPath(path),
                    MatchCount = matches
                });  
            }
            var current = Interlocked.Increment(ref dto._progressItem);
            dto.Progress.Report(Tuple.Create((int)((float)current / dto.TotalItem * 100), Path.GetFullPath(path)));
        }
        else if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var subResult = await FindWordInDirectoryAsync(file, dto);
                result.AddRange(subResult);
            }

            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                var subResult = await FindWordInDirectoryAsync(dir, dto);
                result.AddRange(subResult);
            }
        }
        return result;
    }
    public async Task<List<FileStats>> FindCopyAndReplaceWordAsync(string path, MethodParamsDto dto)
    {
        var result = new List<FileStats>();

        if (File.Exists(path))
        {
            dto.Cts.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(path, dto.Cts).ConfigureAwait(false);

            int matches = Regex.Matches(text, $@"\b{Regex.Escape(dto.Word)}\b", RegexOptions.IgnoreCase).Count;

            if (matches > 0)
            {
                result.Add(new FileStats
                {
                    FileName = Path.GetFileName(path),
                    FilePath = Path.GetFullPath(path),
                    MatchCount = matches
                });

                var updatedText = text.Replace(dto.Word, dto.NewWord, StringComparison.OrdinalIgnoreCase);

                var originalFileName = Path.GetFileNameWithoutExtension(path);
                var extension = Path.GetExtension(path);
                var modifiedFileName = $"{originalFileName}_copy{extension}";
                var targetFilePath = Path.Combine(dto.CopyPath, modifiedFileName);

                await File.WriteAllTextAsync(targetFilePath, updatedText, dto.Cts);

            }
            var current = Interlocked.Increment(ref dto._progressItem);
            dto.Progress.Report(Tuple.Create((int)((float)current / dto.TotalItem * 100), Path.GetFullPath(path)));
        }
        else if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var subResult = await FindCopyAndReplaceWordAsync(file, dto);
                result.AddRange(subResult);
            }

            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                var subResult = await FindCopyAndReplaceWordAsync(dir, dto);
                result.AddRange(subResult);
            }
        }

        return result;
    }
    public async Task<List<FileStats>> FindClassesAndInterfacesAsync(string path, MethodParamsDto dto)
    {
        var result = new List<FileStats>();

        if (File.Exists(path))
        {
            dto.Cts.ThrowIfCancellationRequested();

            var text = await File.ReadAllTextAsync(path, dto.Cts).ConfigureAwait(false);

            var matches = Regex.Matches(text, @"^\s*(?:\b\w+\s+)*?(class|interface)\s+(\w+)", RegexOptions.Multiline);
            var names = new List<string>();

            foreach (Match match in matches)
            {
                names.Add($"{match.Groups[1].Value} {match.Groups[2].Value}");
            }

            if (matches.Count > 0)
            {
                result.Add(new()
                {
                    FileName = Path.GetFileName(path),
                    FilePath = Path.GetFullPath(path),
                    MatchCount = matches.Count,
                    Names = names
                });
            }
            var current = Interlocked.Increment(ref dto._progressItem);
            dto.Progress.Report(Tuple.Create((int)((float)current / dto.TotalItem * 100), Path.GetFullPath(path)));
        }
        else if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var subResult = await FindClassesAndInterfacesAsync(file, dto);
                result.AddRange(subResult);
            }

            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                var subResult = await FindClassesAndInterfacesAsync(dir, dto);
                result.AddRange(subResult);
            }
        }

        return result;
    }
    public static int CountAllFiles(string path)
    {
        int count = 0;
        try
        {
            count += Directory.GetFiles(path).Length;

            foreach (var item in Directory.GetDirectories(path))
            {
                count += CountAllFiles(item);
            }
        }
        catch (ArgumentNullException)
        {
            
        }
        return count;
    }
}
