using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using CurlSharp;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;


namespace ImageViewerUsingFtp
{
    public partial class Form1 : Form
    {

        public void Ftpdown()
        {
            // 코드 단순화를 위해 하드코드함
            string ftpPath = "ftp://192.168.43.169/ScreenCapture.jpeg";
            string user = "anonymous";  // FTP 익명 로그인시. 아니면 로그인/암호 지정.
            string pwd = "";
            string outputFile = "ScreenCapture.jpeg";

            // WebRequest.Create로 Http,Ftp,File Request 객체를 모두 생성할 수 있다.
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath);
            // FTP 다운로드한다는 것을 표시
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            // 익명 로그인이 아닌 경우 로그인/암호를 제공해야
            req.Credentials = new NetworkCredential(user, pwd);

            // FTP Request 결과를 가져온다.
            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
            {
                // FTP 결과 스트림
                Stream stream = resp.GetResponseStream();

                // 결과를 문자열로 읽기 (바이너리로 읽을 수도 있다)
                string data;
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = reader.ReadToEnd();
                }

                // 로컬 파일로 출력
                File.WriteAllText(outputFile, data);
            }
        }



        public bool FtpFileDownload(String strFtpAddress, String strFtpId, String strFtpPwd, string ftpFileName, String SaveFileName)
        {
            bool isOk = true;

            UriBuilder URI = new UriBuilder(strFtpAddress + ftpFileName);
            URI.Scheme = "ftp";

            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(URI.Uri);
            reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd);
            reqFTP.KeepAlive = false;
            reqFTP.UseBinary = true;
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UsePassive = true;


            FtpWebResponse respFTP = (FtpWebResponse)reqFTP.GetResponse();

            Stream strm = respFTP.GetResponseStream();
            FileStream writeStream = null;

            try
            {
                writeStream = new FileStream(SaveFileName, FileMode.Create);
                int Length = 2048;

                Byte[] buffer = new Byte[Length];
                int bytesRead = strm.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = strm.Read(buffer, 0, Length);
                }
                strm.Close();
                writeStream.Close();

                reqFTP = null;
                respFTP = null;
            }
            catch (Exception ex)
            {
                isOk = false;
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
                if (writeStream != null)
                {
                    writeStream.Close();
                }
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (respFTP != null)
                {
                    respFTP = null;
                }
            }

            return isOk;
        }




        Timer timer = new Timer();
        // System.Threading.Timer timer = new System.Threading.Timer();
        public Form1()
        {
            InitializeComponent();

            timer.Enabled = true;
            timer.Interval = 100;

            timer.Tick += timer_Tick;

            timer.Start();

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void timer_Tick(Object sender, EventArgs e)
        {

            // Ftpdown();
            FtpFileDownload("ftp://192.168.43.169/", null, null, "ScreenCapture.jpeg", "ScreenCapture.jpeg");
            //pictureBox1.Image = Bitmap.FromFile(@"C:\Users\t-h-t\Desktop\monitor_sharing_v20200130\monitorSharing_client\monitorSharing_client\ScreenCapture.jpeg");
            //pictureBox1.Image = Bitmap.FromFile(@"C:\monitorSharingFTP\ScreenCapture.jpeg");
            //pictureBox1.Load(@"C:\Users\t-h-t\Desktop\monitor_sharing_v20200130\monitorSharing_client\monitorSharing_client\ScreenCapture.jpeg");
            //pictureBox1.Load(@"C:\monitorSharingFTP\ScreenCapture.jpeg");
            pictureBox1.Load(@"ScreenCapture.jpeg");

        }




    }
}
