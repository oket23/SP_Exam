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
            await JsonSerializer.SerializeAsync(fs, filesStats, options, cancellationToken: cts);
        }
        return Path.GetFullPath(path);
    }

    //public async Task<List<FileStats>> FindClassesAndInterfacesAsync(string path, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    //{
    //    var result = new List<FileStats>();

    //    var allFiles = Directory.GetFiles(path,"*.cs");
    //    var allFolders = Directory.GetDirectories(path);

    //    var totalItems = allFiles.Length + allFolders.Length;
    //    var progressItem = 0;

    //    var FileTask = allFiles.Select(async x =>
    //    {
    //        await _semaphore.WaitAsync(cts);
    //        try
    //        {
    //            cts.ThrowIfCancellationRequested();

    //            var text = await File.ReadAllTextAsync(x, cts);
    //            var matches = Regex.Matches(text, @"^\s*(?:\b\w+\s+)*?(class|interface)\s+(\w+)", RegexOptions.Multiline);
    //            var names = new List<string>();

    //            foreach (Match match in matches)
    //            {
    //                names.Add($"{match.Groups[1].Value} {match.Groups[2].Value}");
    //            }

    //            if (matches.Count > 0)
    //            {
    //                lock (_lock)
    //                {
    //                    result.Add(new()
    //                    {
    //                        FileName = Path.GetFileName(x),
    //                        FilePath = Path.GetFullPath(x),
    //                        MatchCount = matches.Count,
    //                        Names = names
    //                    });
    //                }

    //                var current = Interlocked.Increment(ref progressItem);
    //                progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
    //            }
    //        }
    //        finally { _semaphore.Release(); }

    //    }).ToArray();

    //    await Task.WhenAll(FileTask);

    //    var FolderTask = allFolders.Select(async x =>
    //    {
    //        await _semaphore.WaitAsync(cts);
    //        try
    //        {
    //            cts.ThrowIfCancellationRequested();

    //            var tempResult = await FindClassesAndInterfacesAsync(x, cts, progress);
    //            lock (_lock)
    //            {
    //                result.AddRange(tempResult);
    //            }

    //            var current = Interlocked.Increment(ref progressItem);
    //            progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
    //        }
    //        finally { _semaphore.Release(); }

    //    }).ToArray();

    //    await Task.WhenAll(FolderTask);

    //    return result;
    //}
    //public async Task<List<FileStats>> FindCopyAndReplaceWordAsync(string path, string word, string newWord, string copyPath, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    //{
    //    var result = new List<FileStats>();
    //    var newPath = $"{copyPath}\\copy_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}";
    //    Directory.CreateDirectory(newPath);

    //    var allFiles = Directory.GetFiles(path);
    //    var allFolders = Directory.GetDirectories(path);

    //    var totalItems = allFiles.Length + allFolders.Length;
    //    var progressItem = 0;

    //    var FileTask = allFiles.Select(async x =>
    //    {
    //        cts.ThrowIfCancellationRequested();
    //        var text = await File.ReadAllTextAsync(x, cts);

    //        int matches = Regex.Matches(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase).Count;

    //        if (matches > 0)
    //        {
    //            result.Add(new FileStats
    //            {
    //                FileName = Path.GetFileName(x),
    //                FilePath = Path.GetFullPath(x),
    //                MatchCount = matches
    //            });

    //            var updatedText = text.Replace(word, newWord, StringComparison.OrdinalIgnoreCase);

    //            var originalFileName = Path.GetFileNameWithoutExtension(x);
    //            var extension = Path.GetExtension(x);
    //            var modifiedFileName = $"{originalFileName}_copy{extension}";
    //            var targetFilePath = Path.Combine(newPath, modifiedFileName);

    //            await File.WriteAllTextAsync(targetFilePath, updatedText,cts);

    //            var current = Interlocked.Increment(ref progressItem);
    //            progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
    //        }
    //    }).ToArray();

    //    await Task.WhenAll(FileTask);

    //    var FolderTask = allFolders.Select(async x =>
    //    {
    //        cts.ThrowIfCancellationRequested();

    //        var tempResult = await SearchWordInFolderAsync(x, word, cts, progress);

    //        lock (_lock)
    //        {
    //            result.AddRange(tempResult);
    //        }

    //        var current = Interlocked.Increment(ref progressItem);
    //        progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
    //    }).ToArray();

    //    await Task.WhenAll(FolderTask);

    //    return result;
    //}
    //public async Task<List<FileStats>> SearchWordInFolderAsync(string path, string word, CancellationToken cts, IProgress<Tuple<int, string>> progress)
    //{
    //    var result = new List<FileStats>();

    //    var allFiles = Directory.GetFiles(path);
    //    var allFolders = Directory.GetDirectories(path);

    //    var totalItems = allFiles.Length + allFolders.Length;
    //    var progressItem = 0;

    //    var FileTask = allFiles.Select(async x =>
    //    {
    //        cts.ThrowIfCancellationRequested();

    //        var text = await File.ReadAllTextAsync(x, cts);
    //        int matches = Regex.Matches(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase).Count;

    //        //Повільніше на 1 хв на мому ноутбуці Lenovo (регулярний вираз 1.40 хв)
    //        //int matches = text.Split(splitArray.ToCharArray(),StringSplitOptions.RemoveEmptyEntries)
    //        //            .Count(x => string.Equals(x,word,StringComparison.OrdinalIgnoreCase));

    //        if (matches > 0)
    //        {
    //            result.Add(new()
    //            {
    //                FileName = Path.GetFileName(x),
    //                FilePath = Path.GetFullPath(x),
    //                MatchCount = matches
    //            });

    //            var current = Interlocked.Increment(ref progressItem);
    //            progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
    //        }
    //    }).ToArray();

    //    await Task.WhenAll(FileTask);

    //    var FolderTask = allFolders.Select(async x =>
    //    {
    //        cts.ThrowIfCancellationRequested();

    //        var tempResult = await SearchWordInFolderAsync(x, word, cts, progress);
    //        lock (_lock)
    //        {
    //            result.AddRange(tempResult);
    //        }

    //        var current = Interlocked.Increment(ref progressItem);
    //        progress.Report(Tuple.Create((int)((float)current / totalItems * 100), Path.GetFullPath(x)));
    //    }).ToArray();

    //    await Task.WhenAll(FolderTask);

    //    return result;
    //}

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

            var text = await File.ReadAllTextAsync(path, dto.Cts);
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

            var text = await File.ReadAllTextAsync(path, dto.Cts);

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

            var text = await File.ReadAllTextAsync(path, dto.Cts);

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
