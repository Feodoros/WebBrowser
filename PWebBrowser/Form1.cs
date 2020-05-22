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

        // Обработчик события загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            

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
