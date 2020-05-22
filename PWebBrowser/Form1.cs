using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PWebBrowser
{
    public partial class Form1 : Form
    {
        private string strINIFile = "browser.ini";
        private const string HeadWindow = "[Window]";
        private const string HeadBrowser = "[Browser]";


        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик события закрытия формы
        private void Form1_FormClosed(Object sender, FormClosedEventArgs e)
        {
            WriteSettings();
        }

        // Обработчик события загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {

            if (!File.Exists(strINIFile))
            {
                SetDefaultSettings();
            }

            Dictionary<string, Dictionary<string, string>> settings = ReadSettings();

            Dictionary<string, string> windowSettings = settings[HeadWindow];
            Dictionary<string, string> browserSettings = settings[HeadBrowser];

            // Настройки окна
            this.Width = Int32.Parse(windowSettings["Width"]);
            this.Height = Int32.Parse(windowSettings["Height"]);
            this.Left = Int32.Parse(windowSettings["Left"]);
            this.Top = Int32.Parse(windowSettings["Top"]);
            if(windowSettings["WindowState"] == "Normal")
            {
                this.WindowState = FormWindowState.Normal;
            }
            if (windowSettings["WindowState"] == "Maximized")
            {
                this.WindowState = FormWindowState.Maximized;
            }
            if (windowSettings["WindowState"] == "Minimized")
            {
                this.WindowState = FormWindowState.Minimized;
            }

            // Настройки браузера
            txtURL.Text = browserSettings["URL"];


        }

        private void WriteSettings()
        {
            // Сохраняем настройки окна
            int width = this.Width;
            int height = this.Height;
            int left = this.Left;
            int top = this.Top;
            string stateWindow = this.WindowState.ToString();

            Dictionary<string, string> windowSettings = new Dictionary<string, string>();
            windowSettings["Left"] = left.ToString();
            windowSettings["Top"] = top.ToString();
            windowSettings["Width"] = width.ToString();
            windowSettings["Height"] = height.ToString();
            windowSettings["WindowState"] = stateWindow;


            // Настройки Браузера
            Dictionary<string, string> browserSettings = new Dictionary<string, string>();
            browserSettings["URL"] = txtURL.Text;

            using (var file = File.Open(strINIFile, FileMode.Open, FileAccess.Write))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(HeadWindow);
                    foreach (string key in windowSettings.Keys)
                    {
                        stream.WriteLine($"{key}={windowSettings[key]}");
                    }

                    stream.WriteLine(HeadBrowser);
                    foreach (string key in browserSettings.Keys)
                    {
                        stream.WriteLine($"{key}={browserSettings[key]}");
                    }
                }
            }
        }

        private void SetDefaultSettings()
        {
            string defaultWidth = "Width=400";
            string defaultHeight = "Height=400";
            string defaultLeft = "Left=30";
            string defaultTop = "Top=70";
            string defaultWindowState = "WindowState=Normal";

            string defaultURL = "URL=ya.ru";

            using (var file = File.Open(strINIFile, FileMode.CreateNew))
            {
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(HeadWindow);
                    stream.WriteLine(defaultLeft);
                    stream.WriteLine(defaultTop);
                    stream.WriteLine(defaultWidth);
                    stream.WriteLine(defaultHeight);
                    stream.WriteLine(defaultWindowState);
                    stream.WriteLine(HeadBrowser);
                    stream.WriteLine(defaultURL);

                }
            }
        }

        private Dictionary<string, Dictionary<string, string>> ReadSettings()
        {
            Dictionary<string, Dictionary<string, string>> settings = new Dictionary<string, Dictionary<string, string>>();

            Dictionary<string, string> windowSettings = new Dictionary<string, string>();
            Dictionary<string, string> browserSettings = new Dictionary<string, string>();

            using (var file = File.Open(strINIFile, FileMode.Open, FileAccess.Read))
            {
                using (var stream = new StreamReader(file))
                {
                    string line = stream.ReadLine();

                    if (line == HeadWindow)
                    {
                        while ((line = stream.ReadLine()) != HeadBrowser)
                        {
                            string key = line.Split('=')[0];
                            string value = line.Split('=')[1];
                            windowSettings[key] = value;
                        }

                        line = stream.ReadLine();

                        while (line != "" && line != null)
                        {                            
                            string key = line.Split('=')[0];
                            string value = line.Split('=')[1];
                            browserSettings[key] = value;
                            line = stream.ReadLine();
                        }

                    }

                    else
                    {
                        while ((line = stream.ReadLine()) != HeadWindow)
                        {
                            string key = line.Split('=')[0];
                            string value = line.Split('=')[1];
                            browserSettings[key] = value;
                        }

                        line = stream.ReadLine();

                        while (line != "" && line != null)
                        {                            
                            string key = line.Split('=')[0];
                            string value = line.Split('=')[1];
                            windowSettings[key] = value;
                            line = stream.ReadLine();
                        }
                    }

                }
            }

            settings[HeadWindow] = windowSettings;
            settings[HeadBrowser] = browserSettings;

            return settings;
        }

        // Обработчик события нажатия клавиши
        private void txtURL_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (e.KeyChar == (char)Keys.Return)
            {
                //MessageBox.Show(txtURL.Text);
                wbBrowser.Navigate(txtURL.Text);
                e.Handled = true;
            }

        }

        
    }
}
