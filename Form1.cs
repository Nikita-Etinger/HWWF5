
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Xceed.Words.NET;
namespace menu
{

    public partial class Form1 : Form
    {

        bool isSave = true;
        private Dictionary<string, string> LoadTranslations(string jsonFilePath)
        {
            // Читаем содержимое JSON-файла и десериализуем его в словарь
            string jsonContent = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
        }
        public Form1()
        {
            InitializeComponent();
            button1.Text = "ENG";
        }
        private void SetLanguage(bool isRussian)
        {

            string jsonFilePath = isRussian ? "ResourceRU.json" : "ResourceENG.json";

            Dictionary<string, string> translations = LoadTranslations(jsonFilePath);
            translations.TryGetValue("fileToolStripMenuItem1", out string x);
            fileToolStripMenuItem.Text = x;

            foreach (ToolStripMenuItem menuItem in menuStrip1.Items)
            {

                if (translations.TryGetValue(menuItem.Name, out string text))
                {

                    menuItem.Text = text;
                }


                foreach (ToolStripItem dropDownItem in menuItem.DropDownItems)
                {

                    if (translations.TryGetValue(dropDownItem.Name, out string dropDownItemText))
                    {

                        dropDownItem.Text = dropDownItemText;
                    }
                }
            }


            button1.Text = isRussian ? "RU" : "ENG";
        }



        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (isSave==false)
            {
                DialogResult result = MessageBox.Show("Закрыть текущий документ без сохранения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt";
            open.FilterIndex = 2;

            open.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(open.FileName);
                richTextBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }
        //хотел добавить поддержку docx но требует лицензионный ключ для работы с ним
        //потому сохраниние в формате txt ну или создавать свой уникальный формат 
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();


            save.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);


            save.Filter = "Text files (*.txt)|*.txt";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                isSave = true;
                using (StreamWriter sr = new StreamWriter(save.FileName))
                {
                    sr.Write(richTextBox1.Text);
                }
            }
        }

        private void styleToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
               
                if (richTextBox1.SelectionLength > 0)
                {
                    richTextBox1.SelectionFont = fontDialog.Font;
                }
                else
                {
                   
                    int cursorPosition = richTextBox1.SelectionStart;
                    richTextBox1.SelectionFont = fontDialog.Font;
                    richTextBox1.SelectionStart = cursorPosition;
                    richTextBox1.SelectionLength = 0;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            bool isRussian = button1.Text == "ENG";
            SetLanguage(isRussian);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (isSave==false)
            {

                DialogResult result = MessageBox.Show("Выйти без сохранения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isSave = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isSave == false)
            {

                DialogResult result = MessageBox.Show("Выйти без сохранения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }
            Application.Exit();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (richTextBox1.SelectionLength > 0)
            {

                richTextBox1.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            richTextBox1.Paste();
        }
    }
}
