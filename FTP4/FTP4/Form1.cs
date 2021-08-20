using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;



namespace FTP4
{
    public partial class Form1 : Form
    {
        public object downloadFiles { get; private set; }

        public Form1()
        {
            InitializeComponent();

        }


        //MM/dd


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime bugun = DateTime.Now;
             bugun.ToString();
             DateTime today = bugun.Date;
             
             FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://serverIp"+"/" +today.ToString("MM/dd/yyyy") );
             request.Credentials = new NetworkCredential("username", "password");
             request.UsePassive = true;
             request.UseBinary = true;
             request.KeepAlive = false;
             Console.WriteLine("Getting the response");

             request.Method = WebRequestMethods.Ftp.MakeDirectory;
             //var resp = (FtpWebResponse)request.GetResponse();

             using (var resp = (FtpWebResponse)request.GetResponse())
             {
                 Console.WriteLine(resp.StatusCode);
             }
            Thread.Sleep(1000);
            string[] veriler1 = new string[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                veriler1[i] = listBox1.Items[i].ToString();
            }

            try
            {
                for (int i = 1; i <=listBox1.Items.Count; i++)
                {
                    string select = listBox1.GetItemText(listBox1.SelectedItem);

                    FileInfo FI = new FileInfo(@"C:/Users/İbrahim Hakkı/Desktop/avc/" + veriler1[i]);



                   
                    string uri = "ftp://serverIp/" + today.ToString("MM/dd/yyyy") + "/" + FI.Name;
                    
                    FtpWebRequest FTP;
                    
                    FTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                   
                    FTP.Credentials = new NetworkCredential("username", "password");
                   
                    FTP.KeepAlive = false;
                   
                    FTP.Method = WebRequestMethods.Ftp.UploadFile;
                  
                    FTP.UseBinary = true;
                    
                    FTP.ContentLength = FI.Length;
                   
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;
                    
                    FileStream FS = FI.OpenRead();
                    try
                    {
                        Stream strm = FTP.GetRequestStream();
                        contentLen = FS.Read(buff, 0, buffLength);
                        while (contentLen != 0)//dosya bitene kadar gönderme işlemi
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = FS.Read(buff, 0, buffLength);
                        }
                        strm.Close();
                        FS.Close();
                        
                    }
                    catch (Exception ex)
                    {
                       MessageBox.Show(ex.Message, "Hata");
                    }

                }
           }
            catch
            {

            }

        }






        private void Form1_Load(object sender, EventArgs e)
        {

            listBox1.SelectionMode = SelectionMode.MultiExtended;
            
        }
        private void sil()
        {
            DateTime bugun = DateTime.Now.AddDays(-7);
            bugun.ToString();
            DateTime today = bugun.Date;


            List<string> files = new List<string>();
            try
            {
                FtpWebRequest request = FtpWebRequest.Create("ftp://serverIp/" + today.ToString("MM/dd/yyyy")) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential("username", "password");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                while (!reader.EndOfStream)
                {
                    Application.DoEvents();
                    files.Add(reader.ReadLine());
                }
                response.Close();
                responseStream.Close();
                reader.Close();
                foreach (string file in files)
                {
                    listBox2.Items.Add(file);

                }
                listBox1.Items.Clear();




            }
            catch (Exception ex)
            {

            }
        }


       

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo("C:/Users/");
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo fi in files)
            {
                listBox1.Items.Add(fi.Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FtpWebRequest FTP;
            DateTime bugun = DateTime.Now.AddDays(-7);
            bugun.ToString();
            DateTime today = bugun.Date;

            sil();


            string[] veriler2 = new string[listBox2.Items.Count];
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                veriler2[i] = listBox2.Items[i].ToString();
            }
            try
            {
                for (int i = 0; i <= listBox2.Items.Count; i++)
                {

                    FTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://backup.iletmen.com.tr/" + today.ToString("MM/dd/yyyy") + "/" + veriler2[i]));


                    FTP.UseBinary = true;

                    FTP.Credentials = new NetworkCredential("backup", "Xyz9d8!1");

                    FTP.Method = WebRequestMethods.Ftp.DeleteFile;

                    FtpWebResponse response = (FtpWebResponse)FTP.GetResponse();

                    Console.WriteLine(response.StatusDescription);

                }
                





            }
            catch (Exception)
            {


            }



        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            new Process() { StartInfo = new ProcessStartInfo("D:\\file-address") { Verb = "runas" } }.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FtpWebRequest FTP;
            DateTime bugun = DateTime.Now.AddDays(-7);
            bugun.ToString();
            DateTime today = bugun.Date;
            listBox2.Items.Clear();
            if (listBox2.Items.Count == 0)
            {
                try

                {
                    

                    FTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://serverIp" + today.ToString("MM/dd/yyy")));

                    FTP.UseBinary = true; FTP.Credentials = new NetworkCredential("username", "password");

                    FTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                    FtpWebResponse response = (FtpWebResponse)FTP.GetResponse();

                    Console.WriteLine(response.StatusDescription);
                    label1.Visible = true;

                }

                catch (Exception ex)

                {



                }
            }
        }
    }
         

}

