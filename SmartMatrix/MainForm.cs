using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LexicalTools;
using System.Runtime.InteropServices;

namespace SmartMatrix
{
    public partial class MainForm : Form
    {

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hwnd, int nBar);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        FSM fSM;
        StorageLexem storageLexem;
        LexemAnalyzator lexemAnalyzator;
        public MainForm()
        {    

            InitializeComponent();
            // custom load file
           
            fSM = new FSM();
            storageLexem = new StorageLexem();
            lexemAnalyzator = new LexemAnalyzator(storageLexem, fSM);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "f:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt| matrix files (*.sm)|*.sm";
            //openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader streamReader = new StreamReader(myStream);
                            richTextBoxTextOfProgram.Text = streamReader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void butAnalyzeProgram_Click(object sender, EventArgs e)
        {
            ResetItemsOnChangeProgram();

            lexemAnalyzator.Code = richTextBoxTextOfProgram.Text;
            List<Lexem> listLexem = lexemAnalyzator.GetLexems();
            List<string> lexemsInStr = new List<string>(listLexem.Count);
            foreach (var lexem in listLexem)
            {
                lexemsInStr.Add(lexem.ToString());
            }
            
            using (StreamWriter writer = new StreamWriter(File.Open("F:\\lexems.txt", FileMode.OpenOrCreate)))
            {
                foreach (var lexem in listLexem)
                {
                    listBoxLexems.Items.Add(lexem);
                    writer.WriteLine(lexem);
                }
            }
               
            foreach (var error in lexemAnalyzator.ListError)
            {
                listBoxErrors.Items.Add(error);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = "f:\\";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt| matrix files (*.sm)|*.sm";
            //saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = new StreamWriter(saveFileDialog1.OpenFile())) != null)
                {                     
                    myStream.WriteLine(richTextBoxTextOfProgram.Text);
                    myStream.Close();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reservedLexemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form helpReservedLexemsForm = new HelpReservedLexemsForm();
            helpReservedLexemsForm.Show();
        }

        private int getWidth(float offset)
        {
            return (int)(this.Width * offset);
        }

        private int getHeight(float offset)
        {
            return (int)(this.Height * offset);
        }

        private void textOfProgram_VScroll(object sender, EventArgs e)
        {
            SetScrollPos(richTextBoxNumberOfLines.Handle, (int)System.Windows.Forms.Orientation.Vertical,
                GetScrollPos(richTextBoxTextOfProgram.Handle, (int)System.Windows.Forms.Orientation.Vertical), true);
            PostMessage((IntPtr)richTextBoxNumberOfLines.Handle, 0x115, 4 + 0x10000 *
                GetScrollPos(richTextBoxTextOfProgram.Handle, (int)System.Windows.Forms.Orientation.Vertical), 0);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            butAnalyzeProgram.Width = getWidth(0.17F);
            butAnalyzeProgram.Height = getHeight(0.08F);
            butAnalyzeProgram.Location = new Point((int)(this.Width * 0.14), (int)(this.Height * 0.69));

            richTextBoxTextOfProgram.Width = getWidth(0.44F);
            richTextBoxTextOfProgram.Height = getHeight(0.55F);
            richTextBoxTextOfProgram.Location = new Point((int)(this.Width * 0.04), (int)(this.Height * 0.09));

            listBoxLexems.Width = getWidth(0.4F);
            listBoxLexems.Height = getHeight(0.55F);
            listBoxLexems.Location = new Point((int)(this.Width * 0.53), (int)(this.Height * 0.09));

            listBoxErrors.Width = getWidth(0.4F);
            listBoxErrors.Height = getHeight(0.23F);
            listBoxErrors.Location = new Point((int)(this.Width * 0.53), (int)(this.Height * 0.69));

            richTextBoxNumberOfLines.Width = getWidth(0.03F);
            richTextBoxNumberOfLines.Height = getHeight(0.55F);
            richTextBoxNumberOfLines.Location = new Point((int)(this.Width * 0.01), (int)(this.Height * 0.09));

            ButtonClear.Width = getWidth(0.17F);
            ButtonClear.Height = getHeight(0.08F);
            ButtonClear.Location = new Point((int)(this.Width * 0.14), (int)(this.Height * 0.82));

            label1.Location = new Point((int)(this.Width * 0.68), (int)(this.Height * 0.65));
            label2.Location = new Point((int)(this.Width * 0.61), (int)(this.Height * 0.04));
            label3.Location = new Point((int)(this.Width * 0.18), (int)(this.Height * 0.04));
        }

        

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            richTextBoxTextOfProgram.Text = "";
            ResetItemsOnChangeProgram();
            richTextBoxNumberOfLines.Text = "";
        }

        private void textOfProgram_TextChanged(object sender, EventArgs e)
        {
            richTextBoxNumberOfLines.Clear();
            for (int i = 0; i < richTextBoxTextOfProgram.Lines.Count(); i++)
                richTextBoxNumberOfLines.AppendText(i + 1 + "\r\n");
            textOfProgram_VScroll(sender, e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // custom load file
            StreamReader streamReader = new StreamReader(File.Open("Program.txt", FileMode.Open));
            richTextBoxTextOfProgram.Text = streamReader.ReadToEnd();
            richTextBoxTextOfProgram.TabStop = false;
        }

        private void ResetItemsOnChangeProgram()
        {
            listBoxLexems.Items.Clear();
            listBoxErrors.Items.Clear();
        }
    }
}
