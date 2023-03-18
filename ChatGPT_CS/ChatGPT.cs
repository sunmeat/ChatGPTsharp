using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;

// https://scatteredcode.net/chat-gpt-create-c-sharp-code/#write-an-api-code-sample-in-c
// https://platform.openai.com/account/usage
// https://www.codeproject.com/Articles/5350339/Chat-GPT-in-VB-NET-and-Csharp
// https://help.openai.com/en/articles/7039783-chatgpt-api-faq

namespace ChatGPTCS
{
    public partial class ChatGPT : Form
    {
        // add your API key here !!!
        // and into App.config file
        string OPENAI_API_KEY = ""; // https://beta.openai.com/account/api-keys
        SpeechRecognitionEngine oSpeechRecognitionEngine = null;
        SpeechSynthesizer oSpeechSynthesizer = null;

        public ChatGPT()
        {
            InitializeComponent();
            txtQuestion.Select();
        }

        void ChatGPT_Load(object sender, EventArgs e)
        {
            AppSettingsReader oAppSettingsReader = new AppSettingsReader();
            string sApiKey = oAppSettingsReader.GetValue("OPENAI_API_KEY", typeof(string)) + "";

            if (sApiKey == "")
            {
                MessageBox.Show("Please enter your OpenAI API key in the App.config file.");
                Application.Exit();
            }
            else
            {
                OPENAI_API_KEY = sApiKey;
            }
                        
            // SetModels(); 

            SpeechSynthesizer synth = new SpeechSynthesizer();
        }

        void chkListen_CheckedChanged(object sender, EventArgs e)
        {
            SpeechToText();
        }

        void SpeechToText()
        {
            if (oSpeechRecognitionEngine != null)
            {
                oSpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                return;
            }

            oSpeechRecognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            oSpeechRecognitionEngine.LoadGrammar(new DictationGrammar());
            oSpeechRecognitionEngine.SpeechRecognized += OnSpeechRecognized;
            oSpeechRecognitionEngine.SpeechHypothesized += OnSpeechHypothesized;
            oSpeechRecognitionEngine.SetInputToDefaultAudioDevice();
            oSpeechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (txtQuestion.Text != "")
                txtQuestion.Text += "\n";

            string text = e.Result.Text;
            txtQuestion.Text += text;
        }

        void OnSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            string text = e.Result.Text;
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            {
                string sQuestion = txtQuestion.Text;
                if (string.IsNullOrEmpty(sQuestion))
                {
                    MessageBox.Show("Строка запроса чату не может быть пустой!");
                    txtQuestion.Focus();
                    return;
                }

                if (txtAnswer.Text != "")
                {
                    txtAnswer.AppendText("\r\n");
                }

                txtAnswer.AppendText("Кот пишет: " + sQuestion + "\r\n");
                txtQuestion.Text = "";

                try
                {
                    string sAnswer = SendMsg(sQuestion);
                    txtAnswer.AppendText("Чат отвечает: " + sAnswer);
                    SpeechToText(sAnswer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    txtAnswer.AppendText("Ошибка: " + ex.Message);
                }
            }
        }

        public void SpeechToText(string s)
        {
            if (oSpeechSynthesizer == null)
            {
                oSpeechSynthesizer = new SpeechSynthesizer();
                oSpeechSynthesizer.SetOutputToDefaultAudioDevice();
            }

            oSpeechSynthesizer.Speak(s);

            txtQuestion.Text = "";
        }
        
        public string SendMsg(string sQuestion)
        {

            ServicePointManager.SecurityProtocol = 
                SecurityProtocolType.Ssl3 | 
                SecurityProtocolType.Tls12 | 
                SecurityProtocolType.Tls11 | 
                SecurityProtocolType.Tls;
            
            string apiEndpoint = "https://api.openai.com/v1/completions";
            var request = WebRequest.Create(apiEndpoint);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + OPENAI_API_KEY);

            int iMaxTokens = 2048; // можно менять

            string sModel = "text-davinci-002"; // text-davinci-003

            string data = "{";
            data += " \"model\":\"" + sModel + "\",";
            data += " \"prompt\": \"" + PadQuotes(sQuestion) + "\",";
            data += " \"max_tokens\": " + iMaxTokens + ",";
            data += " \"user\": \"1\", ";
            data += " \"temperature\": 0.5, ";
            data += " \"frequency_penalty\": 0.0" + ", "; // Number between -2.0 and 2.0  Positive value decrease the model's likelihood to repeat the same line verbatim.
            data += " \"presence_penalty\": 0.0" + ", "; // Number between -2.0 and 2.0. Positive values increase the model's likelihood to talk about new topics.
            data += " \"stop\": [\"#\", \";\"]"; // Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop sequence.
            data += "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string sJson = streamReader.ReadToEnd();

            var oJavaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> oJson = (Dictionary<string, object>) oJavaScriptSerializer.DeserializeObject(sJson);
            object[] oChoices = (object[])oJson["choices"];
            Dictionary<string, object> oChoice = (Dictionary<string, object>) oChoices[0];
            string sResponse = (string) oChoice["text"];

            return sResponse;
        }

        string PadQuotes(string s)
        {
            if (s.IndexOf("\\") != -1)
                s = s.Replace("\\", @"\\");
                    
            if (s.IndexOf("\n\r") != -1)
                s = s.Replace("\n\r", @"\n");

            if (s.IndexOf("\r") != -1)
                s = s.Replace("\r", @"\r");

            if (s.IndexOf("\n") != -1)
                s = s.Replace("\n", @"\n");

            if (s.IndexOf("\t") != -1)
                s = s.Replace("\t", @"\t");
            
            if (s.IndexOf("\"") != -1)
                return s.Replace("\"", @"""");
            else
                return s;
        }
        
        void SetModels()
        {
            // https://beta.openai.com/docs/models/gpt-3

            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;

            string apiEndpoint = "https://api.openai.com/v1/models";
            var  request = WebRequest.Create(apiEndpoint);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + OPENAI_API_KEY);

            var response = request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string sJson = streamReader.ReadToEnd();

            //cbModel.Items.Clear();

            SortedList oSortedList = new SortedList();
            System.Web.Script.Serialization.JavaScriptSerializer oJavaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> oJson = (Dictionary<string, object>)oJavaScriptSerializer.DeserializeObject(sJson);
            object[] oList = (object[])oJson["data"];
            for (int i = 0; i <= oList.Length - 1; i++)
            {
                Dictionary<string, object> oItem = (Dictionary<string, object>)oList[i];
                string sId = (string) oItem["id"];
                if (oSortedList.ContainsKey(sId) == false)
                {
                    oSortedList.Add(sId, sId);
                }                
            }

            //foreach (DictionaryEntry oItem in oSortedList)
            //    cbModel.Items.Add(oItem.Key);
        }

        void txtQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(null, null);
            }
        }
    }
}