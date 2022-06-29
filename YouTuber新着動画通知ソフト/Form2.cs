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
using Newtonsoft.Json;

namespace YouTuber新着動画通知ソフト
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public class set { public string ch_id { get; set; } public bool is_name { get; set; } public string api_key { get; set; } }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.MaximizeBox=false;
            var sr = new StreamReader(@"settings\settings.json", System.Text.Encoding.UTF8);
            var settings = sr.ReadToEnd();
            var sett = JsonConvert.DeserializeObject<set>(settings);
            textBox2.Text = sett.api_key;
            textBox1.Text = "https://www.youtube.com/channel/" + sett.ch_id;
            checkBox1.Checked = sett.is_name;
            sr.Dispose();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            set t = new set();
            string id = textBox1.Text;

            t.ch_id = id.Remove(0,32);
            t.api_key = textBox2.Text;
            t.is_name = checkBox1.Checked;

            var jsonData = JsonConvert.SerializeObject(t);
            var str = jsonData;
            using(var sw = new StreamWriter(@"settings\settings.json",false,System.Text.Encoding.UTF8))
            {
                sw.Write(str);
            }
            await Task.Delay(1000);
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
