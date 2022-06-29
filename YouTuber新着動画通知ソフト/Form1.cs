using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Net;

namespace YouTuber新着動画通知ソフト
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static class global
        {
            public static string user_name;
            public static string new_video_link;
            public static string new_video_title;
            public static string new_video_sumpic;
            public static string ch_id;
            public static bool is_name;
            public static string api_key;
        }

        public class set { public string ch_id { get; set; } public bool is_name { get; set; } public string api_key { get; set; } }

        private readonly WebClient downloadClient = new WebClient();
        private readonly HttpClient client = new HttpClient();

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            string error = null;
            string error2 = null;
            FileStream fs = new FileStream(@"settings\settings.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
            var settings = sr.ReadToEnd();
            var sett = JsonConvert.DeserializeObject<set>(settings);
            sr.Dispose();
            global.is_name = sett.is_name;
            global.ch_id = sett.ch_id;
            global.api_key = sett.api_key;

            if (global.api_key == ""||global.ch_id=="") { goto notapikey; }
            try
            {
                var urls = "";
                if (global.is_name == false)
                {
                    urls = $"https://www.googleapis.com/youtube/v3/channels?part=snippet&key={global.api_key}&id={global.ch_id}";
                }
                else if (global.is_name == true)
                {
                    urls = $"https://www.googleapis.com/youtube/v3/channels?part=snippet&key={global.api_key}&forUsername={global.ch_id}";
                    var js_d2 = await client.GetStringAsync(urls);
                    var js12 = JsonConvert.DeserializeObject<Root>(js_d2);
                    global.ch_id = js12.items[0].id;
                }

                //---------YTDataAPI取得部分---------//
                //ユーザ情報
                var js_d = await client.GetStringAsync(urls);
                var js1 = JsonConvert.DeserializeObject<Root>(js_d);
                var ch_icon = js1.items[0].snippet.thumbnails.@default.url;

                //---------YTDataAPI取得部分　終---------//

                //---------YT Xml Feed取得部分---------//

                var fileName = $@"https://www.youtube.com/feeds/videos.xml?channel_id={global.ch_id}";
                var xdoc = XDocument.Load(fileName);
                // XDocumentをJSON形式の文字列に変換
                var json = JsonConvert.SerializeXNode(xdoc, Formatting.Indented);
                var data = JsonConvert.DeserializeObject<ConvJson>(json);

                //---------YT Xml Feed取得部分 終---------//

                //data = YouTubeXMLFeedくん//
                global.user_name = data.feed.title;
                global.new_video_title = data.feed.entry[0].title;
                global.new_video_link = data.feed.entry[0].link.Href;
                global.new_video_sumpic = data.feed.entry[0].MediaGroup.MediaThumbnail.Url;
                var new_video_description = data.feed.entry[0].MediaGroup.MediaDescription;
                var update_time = data.feed.entry[0].updated;

                ch_name.Text = "現在設定中のチャンネル名:" + global.user_name;
                label3.Text = "最終更新:" + update_time;

                textBox1.Text = global.new_video_title;
                textBox2.Text = new_video_description;

                string name = System.IO.Path.GetTempPath();
                string path = $@"{name}youtuberNotify\img\";

                //フォルダ有無の確認→なければ作成
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    di.Create();
                }
                //サムネイルダウンロード
                string file = $@"{name}youtuberNotify\img\samune.png";
                Uri u = new Uri(global.new_video_sumpic);
                downloadClient.DownloadFile(u, file);
                //アイコンダウンロード
                string file2 = $@"{name}youtuberNotify\img\icon.png";
                Uri u2 = new Uri(ch_icon);
                downloadClient.DownloadFile(u2, file2);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                error2 = ex.ToString();
                label1.Text = "情報取得に失敗しました。再起動してください";
                label2.Text = "ExceptionMessage";
                label4.Text = "詳細";
                textBox1.Text = error;
                textBox2.Text = error2;
            }
        
        //設定ファイルエラー
        notapikey: 
            if (global.api_key == "") { ch_name.Text = "エラー:APIキーが指定されていません。"; }
            if (global.ch_id == "") { ch_name.Text = "エラー:チャンネルIDが指定されていません"; }
            if (global.api_key == "" && global.ch_id == "") { ch_name.Text = "エラー:チャンネルID、APIキーが指定されていません"; }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start(global.new_video_link);
        }

        private void xmlAct_01_Tick(object sender, EventArgs e)
        {
            var fileName = $@"https://www.youtube.com/feeds/videos.xml?channel_id={global.ch_id}";
            var xdoc = XDocument.Load(fileName);
            // XDocumentをJSON形式の文字列に変換
            var json = JsonConvert.SerializeXNode(xdoc, Formatting.Indented);
            var data = JsonConvert.DeserializeObject<ConvJson>(json);

            global.user_name = data.feed.title;
            global.new_video_title = data.feed.entry[0].title;
            global.new_video_link = data.feed.entry[0].link.Href;
            global.new_video_sumpic = data.feed.entry[0].MediaGroup.MediaThumbnail.Url;
            var new_video_description = data.feed.entry[0].MediaGroup.MediaDescription;
            var update_time = data.feed.entry[0].updated;

            ch_name.Text = "現在設定中のチャンネル名:" + global.user_name;
            label3.Text = "最終更新:" + update_time;

            textBox1.Text = global.new_video_title;
            textBox2.Text = new_video_description;

            string name = System.IO.Path.GetTempPath();
            string file = $@"{name}youtuberNotify\img\samune.png";
            Uri u = new Uri(global.new_video_sumpic);
            downloadClient.DownloadFile(u, file);
        }

        private async void label3_TextChanged(object sender, EventArgs e)
        {
            
            string name = Path.GetTempPath();
            if (global.new_video_title == textBox1.Text)
            {
                await Task.Delay(3000);
                new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText(global.user_name + "が動画内容を更新しました")
                        .AddText(global.new_video_title)
                        .AddHeroImage(new Uri($@"file:///{name}youtuberNotify/img/samune.png"))
                        .AddAppLogoOverride(new Uri($@"file:///{name}youtuberNotify/img/icon.png"))
                        .Show();
                ToastNotificationManagerCompat.OnActivated += this.ToastNotificationManagerCompat_OnActivated;
            }
            else
            {
                await Task.Delay(3000);
                new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText(global.user_name + "の新着動画")
                        .AddText(global.new_video_title)
                        .AddHeroImage(new Uri($@"file:///{name}youtuberNotify/img/samune.png"))
                        .AddAppLogoOverride(new Uri($@"file:///{name}youtuberNotify/img/icon.png"), ToastGenericAppLogoCrop.Circle)
                        .AddButton(new ToastButton("動画をブラウザで開く","openWeb"))
                        .AddButton(new ToastButton("キャンセル", "cancel"))
                        .Show();
                ToastNotificationManagerCompat.OnActivated += this.ToastNotificationManagerCompat_OnActivated;
            }
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            // e.Argument で押されたボタンを確認
            var arg = e.Argument;
            if (arg == "cancel") return;

            System.Diagnostics.Process.Start(global.new_video_link);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }
    }
}
