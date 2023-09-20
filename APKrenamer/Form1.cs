using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Csharp.Apk_Reader;

namespace APKrenamer
{
    public partial class mainWindow : Form
    {
        // Initializing variables
        List<string> path = new List<string>();
        string diagCurrentFileName;

        // mainwindow stuff
        public mainWindow()
        {
            InitializeComponent();
        }

        // On btStart click
        private void btStart_Click(object sender, EventArgs e)
        {
            // initialize a new instance of Apk_Reader as var "read"
            var read = new Apk_Reader();
            // Create "output" folder
            Directory.CreateDirectory("./output");
            // For every file on path...
            for (int i = path.Count - 1; i >= 0; i--)
            {
                // ...try to pass it to ApkInfo and save stuff on variables
                try
                {
                    string filename = path[i];
                    diagCurrentFileName = filename;
                    ApkInfo file_info = read.Get_File_Info(filename);
                    string apkName = file_info.apkName;
                    string versionName = file_info.versionName;
                    string packageName = file_info.packageName;
                    string sdkVersion = file_info.sdkVersion;
                    string targetSdkVersion = file_info.targetSdkVersion;
                    string versionCode = file_info.versionCode;
                    string output = textBox2.Text;
                    // Replace the variables with it`s content
                    output = output.Replace("%name%", apkName).Replace("%ver%", versionName).Replace("%package%", packageName)
             .Replace("%sdk%", sdkVersion).Replace("%vercode%", versionCode).Replace("%sdk%", sdkVersion).Replace("%target%", targetSdkVersion);
                    log("File name: " + output);
                    // Copy file with the new filename
                    File.Copy(filename, "./output/" + output + ".apk");
                }
                // In case of exception, ignore it just sending a messagebox
                catch (System.Exception)
                {
                    // Check if it should ignore the error
                    if (checkBox1.Checked)
                    {
                        log("An error ocurred with" + diagCurrentFileName + ", but was ignored!");
                    }
                    else
                    {
                        MessageBox.Show("An error ocurred while copying " + diagCurrentFileName + ", please verify if the filename is valid or try removing special characters from the original filename");
                    }
 
                }
            }
        }

        // On btFile click
        private void btFile_Click(object sender, EventArgs e)
        {
            // Shows a OpenFileDialog named openFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Default dir
                openFileDialog.InitialDirectory = "c:\\";
                // Filters for apk or other files
                openFileDialog.Filter = "Android Package Kit (*.apk)|*.apk|Any file with a valid manifest (*.*)|*.*";
                // Allows user to select more than 1 file
                openFileDialog.Multiselect = true;

                // Checks if the user selected a file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // For every selected file...
                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        // Add it to path
                        path.Add(openFileDialog.FileNames[i]);
                        // Add it to the directory textbox
                        textBox1.AppendText(" " + openFileDialog.FileNames[i]);
                    }
                }
            }
        }

        // For convenience sake, it adds a new line on log
        public void log(string message)
        {
            textBox3.AppendText(Environment.NewLine + message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Variables:\n%target% - Target SDK version\n%sdk% - Minimum SDK version\n%package% - Package name\n%name% - Application name\n%ver% - Application version\n%vercode% - Version code");
        }
    }
}
