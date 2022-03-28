using Analytics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangServiceKeyPhrases
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
                var uri = "https://{your service name}.cognitiveservices.azure.com/text/analytics/v3.1/keyPhrases";

                //授權金鑰
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{Subscription-Key}");

                //文字內容
                var reqModel = new ReqModel();
                reqModel.documents = new Document[] { new Document() { id = "1", language = "zh", text = this.ReqText.Text } };

                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reqModel, Formatting.Indented));
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var resp = await client.PostAsync(uri, content);
                    var resultString = await resp.Content.ReadAsStringAsync();
                    foreach (var item in JsonConvert.DeserializeObject<AnalyticsResult>(resultString).documents[0].keyPhrases)
                    {
                        this.Analytics.Text += "\n\n\n" + item;
                    }
                }
            }
        }
    }
}
