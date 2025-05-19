namespace SP_Exam.Forms;

public partial class EnterWordForm : Form
{
    private string _newWord;
    public EnterWordForm()
    {
        InitializeComponent();
    }

    private void oKBtn_Click(object sender, EventArgs e)
    {
        _newWord = wordTB.Text.Trim();
        if (string.IsNullOrEmpty(_newWord))
        {
            MessageBox.Show("Please enter a word.", "Input Error", MessageBoxButtons.OK);
        }
        else
        {
            this.Close();
        }
    }
    public string GetWord()
    {
        return wordTB.Text.Trim();
    }
}
