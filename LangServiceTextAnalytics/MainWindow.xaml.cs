using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Windows;


namespace LangServiceTextAnalytics
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

        private async void DoRecognition_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var uri = "https://{language service name}.cognitiveservices.azure.com/text/analytics/v3.1/entities/ /pii?";

                //授權金鑰
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{your Subscription-Key}");


                var queryString = HttpUtility.ParseQueryString(string.Empty);
                //請求參數
                queryString["domain"] = "phi";
                queryString["piiCategories"] = "PhoneNumber,Email";

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
                    var response = await client.PostAsync(uri + queryString, content);
                    this.EntitiesRecognition.Text = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
