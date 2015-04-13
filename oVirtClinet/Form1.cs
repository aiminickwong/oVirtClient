using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace oVirtClinet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "欢迎使用 OvirtClient";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string serverUrl = textBox1.Text.Trim();
            string username = textBox2.Text.Trim();
            string password = textBox3.Text;
            serverUrl = "ovirt-engine.studio.hrsoft.net";
            username = "admin@internal";
            password = "ta992080fe";

            if(serverUrl == "" && username == "" && password == "")
            {
                MessageBox.Show("服务器地址&用户名&密码不能为空","错误");
            }else
            {
                string certUrl = "http://" + serverUrl + "/ca.crt";
                serverUrl = "https://" + serverUrl + "/api";
                string tmpPath = System.IO.Path.GetTempPath()+ "ca.crt";
                WebClient webClient = new WebClient();
                webClient.DownloadFile(certUrl, tmpPath);
                HttpWebRequest request = WebRequest.Create(serverUrl) as HttpWebRequest;
                request.Credentials = new NetworkCredential(username, password);

                request.ClientCertificates.Add(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(tmpPath));
               
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    if(request.HaveResponse == true && response != null)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        StringBuilder sbSource = new StringBuilder(reader.ReadToEnd());
                        MessageBox.Show(sbSource.ToString());
                    }
                }catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
