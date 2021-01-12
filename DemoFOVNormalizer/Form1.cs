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

namespace DemoFOVNormalizer {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        List<string> files = new List<string>();
        List<string> selectedFiles = new List<string>();

        private void button1_Click(object sender, EventArgs e) {
            try {
                foreach (string path in selectedFiles) {
                    byte[] content = File.ReadAllBytes(path);
                    byte[] pattern = new byte[] { 0x63, 0x6C, 0x5F, 0x66, 0x6F, 0x76 };
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) {
                        foreach (int pos in content.Locate(pattern)) {
                            fs.Position = pos + 7;
                            fs.WriteByte(0x39);
                            fs.Position = pos + 8;
                            fs.WriteByte(0x30);
                            if (content[pos + 9] != 0x20) {
                                fs.Position = pos + 9;
                                fs.WriteByte(0x00);
                            }
                        }
                    }
                }
            } catch (Exception ex) { }
        }

        private void button2_Click(object sender, EventArgs e) {
            for (int i = 0; i < checkedListBox1.Items.Count; i++) {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            for (int i = 0; i < checkedListBox1.Items.Count; i++) {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            try {
                folderBrowserDialog1.ShowDialog();
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            } catch (Exception ex) { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            try {
                checkedListBox1.Items.Clear();
                files.Clear();
                foreach (string file in Directory.GetFiles(textBox1.Text)) {
                    files.Add(file);
                    checkedListBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            } catch (Exception ex) { }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            try {
                selectedFiles.Clear();
                foreach (int index in checkedListBox1.CheckedIndices) {
                    selectedFiles.Add(files[index]);
                }
            } catch (Exception ex) { }
        }
    }
}
