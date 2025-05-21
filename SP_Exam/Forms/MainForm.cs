using SP_Exam.Core.Dtos;
using System.Text;
namespace SP_Exam.Forms;

public partial class MainForm : Form
{
    private CancellationTokenSource _cts;
    public MainForm()
    {
        _cts = new CancellationTokenSource();
        InitializeComponent();
    }

    private async void searchWordBtn_Click(object sender, EventArgs e)
    {
        try
        {
            StartPreset();
           
            var fileService = Dependency.GetFileService();
            var folderPath = GetFolder();

            var dto = new MethodParamsDto
            {
                Cts = _cts.Token,
                Word = wordTB.Text.Trim(),
                Progress = new Progress<Tuple<int, string>>(UpdateProgressBar)
            };

            var searchData = await fileService.FindWordInDirectoryAsync(folderPath, dto);

            var sb = new StringBuilder();
            sb.AppendLine($"Word: {wordTB.Text.Trim()}\nFiles found:{searchData.FilesMatched}\nWords found: {searchData.AllMatched}\nFile with statistics in folder: {searchData.FullPath}");

            mainRTB.Text = sb.ToString();
            MessageBox.Show("Ready!");
        }
        catch (OperationCanceledException ex)
        {
            MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        finally
        {
            EndPreset();
        }
    }
    private async void classFindBtn_Click(object sender, EventArgs e)
    {
        try
        {
            StartPreset();

            var fileService = Dependency.GetFileService();
            var folderPath = GetFolder();

            var dto = new MethodParamsDto
            {
                Cts = _cts.Token,
                Progress = new Progress<Tuple<int, string>>(UpdateProgressBar)
            };

            var searchData = await fileService.FindClassesAndInterfacesAsync(folderPath, dto);

            var sb = new StringBuilder();
            sb.AppendLine("Classes and Interfaces:");
            foreach (var item in searchData.FileStats)
            {
                sb.AppendLine(string.Join("\n", item.Names));
            }
            sb.AppendLine($"Files found: {searchData.FilesMatched}\nWords found: {searchData.AllMatched}\nFile with statistics in folder: {searchData.FullPath}");

            mainRTB.Text = sb.ToString();
            MessageBox.Show("Ready!");
        }
        catch (OperationCanceledException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        finally
        {
            EndPreset();
        }
    }
    private async void fCaRWBtn_Click(object sender, EventArgs e)
    {
        try
        {
            StartPreset();
            var fileService = Dependency.GetFileService();
            var folderPath = GetFolder();
            MessageBox.Show("Enter the folder to copy the files to.");
            var folderCopyPath = GetFolder();

            var temp = new EnterWordForm();
            temp.ShowDialog();

            var dto = new MethodParamsDto()
            {
                Word = wordTB.Text.Trim(),
                NewWord = temp.GetWord(),
                CopyPath = folderCopyPath,
                Progress = new Progress<Tuple<int, string>>(UpdateProgressBar),
                Cts = _cts.Token
            };
            var searchData = await fileService.FindCopyAndReplaceWordAsync(folderPath, dto);

            var sb = new StringBuilder();

            sb.AppendLine($"Files found: {searchData.FilesMatched}\nWords found: {searchData.AllMatched}\nFile with statistics in folder: {searchData.FullPath}\nFile successfully copied to folder {folderCopyPath}");
            mainRTB.Text = sb.ToString();

            MessageBox.Show("Ready!");
        }
        catch (OperationCanceledException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
        }
        finally
        {
            EndPreset();
        }
    }
    private void CancelBtn_Click(object sender, EventArgs e)
    {
        _cts.Cancel();
        EndPreset();
    }
    private void UpdateProgressBar(Tuple<int, string> progress)
    {
        mainPB.Value = progress.Item1;
        PBLaybel.Text = progress.Item2;
    }
    private void EndPreset()
    {
        mainPB.Value = 0;
        PBLaybel.Text = " ";
        classFindBtn.Enabled = true;
        fCaRWBtn.Enabled = true;
        searchWordBtn.Enabled = true;
    }
    private void StartPreset()
    {
        mainPB.Value = 0;
        PBLaybel.Text = " ";
        classFindBtn.Enabled = false;
        fCaRWBtn.Enabled = false;
        searchWordBtn.Enabled = false;
        _cts = new CancellationTokenSource();
    }
    private static string GetFolder()
    {
        using (FolderBrowserDialog dialog = new FolderBrowserDialog())
        {
            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.SelectedPath : throw new ArgumentException("Select valid folder!");
        }
    }
}
