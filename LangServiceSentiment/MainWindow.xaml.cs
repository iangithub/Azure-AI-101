using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangServiceSentiment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DoAnalytics_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var uri = "https://{service name from azure}.cognitiveservices.azure.com/text/analytics/v3.1/sentiment";

                //授權金鑰
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{your Subscription-Key}");

                //文字內容
                var reqModel = new ReqModel();
                reqModel.documents = new TextContent[1];
                reqModel.documents[0] = new TextContent()
                {
                    id = "1",
                    language = "zh-hans",
                    text = this.ReqText.Text
                };

                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqModel, Formatting.Indented));
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(uri, content);
                    this.Analytics.Text = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
