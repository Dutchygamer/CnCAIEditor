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

        private void LoadAIClick(object sender, EventArgs e)
        {
            LoadAIFile();
        }

        private void LoadAIFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select AI.ini";
            dialog.Filter = ".ini|*.ini";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    AIController.ReadFileAsString(File.ReadAllText(dialog.FileName));
                }
                catch (Exception e)
                {
                    Console.WriteLine("OH NO!" + e.Message);
                }
            }
        }

        //TODO: doet nog niks nu. Fix editen van plain AI.ini maar eerst voor je hiermee verder gaat kloten
        private void LoadRulesClick(object sender, EventArgs e)
        {
            Console.WriteLine("Whoops!");
            //TODO: enable when when we can edit AI.ini without references
            //LoadRulesFile();
        }

        private void LoadRulesFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select rules.ini";
            dialog.Filter = ".ini|*.ini";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    RulesController.ReadFileAsString(File.ReadAllText(dialog.FileName));
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROOOOOR!" + e.Message);
                }
            }
        }

    }
}
