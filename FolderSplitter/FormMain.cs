using System;
using System.IO;
using System.Windows.Forms;

namespace FolderSplitter
{
    public partial class FormMain : Form
    {
        string folderStart = null;
        string folderDest = null;
        string startDirectoryName = null;
        long sizeFilesInFilder = 0;
        int numberFolder = 1;

        public FormMain()
        {
            InitializeComponent();
            numericUpDown1_ValueChanged(this, new EventArgs());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderStart = folderBrowserDialog1.SelectedPath;
                startDirectoryName = new DirectoryInfo(folderStart).Name;
                textBox1.Text = folderStart;
                button2.Enabled = true;
                button3.Enabled = false;
                folderDest = null;
                textBox2.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog2.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DialogResult result = folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = folderDestFunction();
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(folderStart))
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                folderDest = folderDestFunction();
                searthAllForders(folderStart);
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                sizeFilesInFilder = 0;
                numberFolder = 1;
            }
        }

        private void searthAllForders(string startLocation)
        {
            if (Directory.Exists(startLocation))
            {
                moveFiles(startLocation);
                foreach (var directory in Directory.GetDirectories(startLocation))
                {
                    searthAllForders(directory);
                }
            }
        }

        private void moveFiles(string PathScan)
        {
            string dirName = PathScan.Remove(0, folderStart.Length);
            foreach (var line in Directory.EnumerateFiles(PathScan))
            {
                if (sizeFilesInFilder >= numericUpDown1.Value)
                {
                    sizeFilesInFilder = 0;
                    numberFolder += 1;
                    folderDest = folderDestFunction();
                }
                string fileName = line.Remove(0, folderStart.Length);
                if (!Directory.Exists(folderDest + dirName))
                {
                    try
                    {
                        Directory.CreateDirectory(folderDest + dirName);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось создать папку: " + folderDest + dirName);
                    }
                }
                try
                {
                    long fileSize = new FileInfo(line).Length;
                    sizeFilesInFilder = sizeFilesInFilder + fileSize;
                    File.Move(line, folderDest + fileName);
                }
                catch
                {
                    MessageBox.Show("Не удалось переместить: " + line);
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = (numericUpDown1.Value / 1024 / 1024).ToString("F") + " MB";
            label5.Text = (numericUpDown1.Value / 1024 / 1024 / 1024).ToString("F") + " GB";
        }

        private string folderDestFunction()
        {
            return pathAddSlash(folderBrowserDialog2.SelectedPath) + startDirectoryName + "_" + numberFolder.ToString();
        }

        private string pathAddSlash(string path)
        {
            if (path.EndsWith(@"/") || path.EndsWith(@"\"))
            {
                return path;
            }
            if (path.Contains(@"/"))
            {
                return path + @"/";
            }
            if (path.Contains(@"\"))
            {
                return path + @"\";
            }
            return path;
        }
    }
}
