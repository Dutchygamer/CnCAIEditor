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

namespace CnCAIEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadButtonClick(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void LoadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Test";
            dialog.Filter = "ini files|*.ini";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string file = dialog.FileName;
                try
                {
                    //Controller.ReadFileAsStringArray(File.ReadAllLines(file));
                    Controller.ReadFileAsString(File.ReadAllText(file));
                }
                catch (Exception e)
                {
                    Console.WriteLine("OH NO!");
                }
            }
        }
    }
}
