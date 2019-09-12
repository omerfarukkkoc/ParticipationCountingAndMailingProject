using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


using Emgu.CV;
using Emgu.CV.Structure;
using Ionic.Zip;
using System.IO;
using MetroFramework.Forms;

using System.Drawing.Imaging;
using MessagingToolkit.QRCode;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

namespace video_processing
{
    public partial class MainForm : MetroForm
    {

        private static VideoCapture Camera = new VideoCapture();
        private VideoWriter vw;
        Mat Frame = new Mat();
        CascadeClassifier Cascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
        Rectangle[] FacesRectangle;

        private static string json_secret_file = @".\client_secret.json";
        private static string application_name = @".NET Google Drive Uploader";
        static DriveUploader driveUploader = new DriveUploader(json_secret_file, application_name);

        FileStream fs;
        StreamWriter sw;

        private static int TotalFaceCount = 0;
        private static int FaceCount = 0;
        private static string VideoRecordName = "Video";
        private bool EndOfDayMailControl = true;

        private static double FrameWidth = 1900;
        private static double FrameHeight = 1080;
        private static int FPS = 10;

        private static int FOURCC;

        private static int ScreenshotDuration = 2000;
        private static int VideoDuration = 10000;
        private static int SessionCount = 1;
        private static int EndOfDayMailHour = 22;
        private static string DirectoryName = "C:\\KatılımcıSaymaUygulaması";

        private string From = "oturumupload@gmail.com";
        private string Password = "oturum123";
        private string Title = "oturumupload@gmail.com";
        private string To = "omerfarukkoc42@gmail.com";
        private string Subject = "Katılımcı Sayısı";
        private string Body = "";
        private string Attachments = "";
        private string SmtpHost = "smtp.live.com";
        private int SmtpPort = 587;
        



        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            info_lbl.Text = "--Sistem Boşta--";
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            if (!Directory.Exists(DirectoryName + "\\Tetik"))
                Directory.CreateDirectory(DirectoryName + "\\Tetik");

            //fileSystemWatcher = new FileSystemWatcher();
            //fileSystemWatcher.Filter = "*.txt";
            //fileSystemWatcher.Path = DirectoryName + "\\Tetik" ;
            //fileSystemWatcher.EnableRaisingEvents = true;
            //fileSystemWatcher.Created += FileSystemWatcher_Created;

            TxtReadTimer.Interval = 1000;
            TxtReadTimer.Start();
        }

        //private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        //{
        //    MainFunction(ScreenshotDuration, VideoDuration);
        //    //if (Directory.Exists(DirectoryName))
        //    //{
        //    //    string FileName = DirectoryName + "\\a.txt";
        //    //    if (File.Exists(FileName) == true)
        //    //    {
        //    //MessageBox.Show(e.Name);
        //    //    }
        //    //}
        //}

        private void TxtReadTimer_Tick(object sender, EventArgs e)
        {
            if (Directory.Exists(DirectoryName))
            {
                string FileName = DirectoryName + "\\Tetik\\a.txt";
                if (File.Exists(FileName) == true)
                {
                    MainFunction(ScreenshotDuration, VideoDuration);
                }
            }
        }

        private void MainFunction(int interval1, int interval2)
        {

            try
            {
                Camera.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, FrameWidth);
                Camera.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, FrameHeight);
                Camera.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, FPS);
                TxtReadTimer.Stop();
                QR_PB.SizeMode = PictureBoxSizeMode.StretchImage;
                QR_PB.Image = new Bitmap(Application.StartupPath + "\\3d-loading.gif");
                info_lbl.Text = "Video Kaydı Yapılıyor...";
                FOURCC = Convert.ToInt32(Camera.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FourCC));
                vw = new VideoWriter(VideoRecordName + ".mp4", FOURCC, FPS, new Size(Convert.ToInt32(FrameWidth), Convert.ToInt32(FrameHeight)), true);
                Application.Idle += new EventHandler(ProcessFrame);
                
                
                FaceCountTimer.Interval = interval1;
                FaceCountTimer.Start();
                VideoRecordTimer.Interval = interval2;
                VideoRecordTimer.Start();

                if (DateTime.Now.Hour >= EndOfDayMailHour)
                {
                    if (EndOfDayMailControl)
                        EndOfDaySendMailAndWriteFile();
                }

            }
            catch (Exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "\nUygulama Başlatma Esnasında Hata\nSistemi Yeniden Başlatın \nSorun Devam Ederse Üreticinize Başvurun", "HATA!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ProcessFrame(object sender, EventArgs e)
        {
            Frame = Camera.QueryFrame();
            //Frame_PB.Image = Frame.ToImage<Bgr, Byte>().Bitmap;
            vw.Write(Frame);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            vw.Dispose();
            Application.Idle -= new EventHandler(ProcessFrame);
            VideoRecordTimer.Stop();
            VideoFileUpload(VideoRecordName);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                FaceCountTimer.Stop();
                var InputImage = Frame.ToImage<Bgr, Byte>();

                if (FaceDetectionFunction(InputImage))
                {
                    TotalFaceCount = FaceCount + TotalFaceCount;
                    Body = DateTime.Now.ToString("dd-MM-yyyy HH:mm") + " ' te baslayan oturumda " + FaceCount.ToString() + " kisi vardır.";
                    SendMail.Send(From, Password, Title, To, Subject, Body, Attachments, SmtpHost, SmtpPort);
                    FaceCount = 0;
                }
            }
            catch(Exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "\nMail Gönderimi Esnasında Hata\nSistemi Yeniden Başlatın \nSorun Devam Ederse Üreticinize Başvurun", "HATA!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    }
        
        public bool FaceDetectionFunction(Image<Bgr, byte> imgInput)
        {
            bool returnValue = false;

            try
            {
                var imgGray = imgInput.Convert<Gray, byte>().Clone();
                FacesRectangle = Cascade.DetectMultiScale(imgGray, 1.1, 4);
                foreach (var face in FacesRectangle)
                {
                    imgInput.Draw(face, new Bgr(255, 0, 0), 2);
                }
                //Image_PB.Image = imgInput.Bitmap;
                FaceCount = FacesRectangle.Length;
                returnValue = true;
            }
            catch (Exception)
            {
                //MetroFramework.MetroMessageBox.Show(this, "\nKişi Sayma Esnasında Hata\nSistemi Yeniden Başlatın \nSorun Devam Ederse Üreticinize Başvurun", "HATA!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return returnValue;
        }
        

        private void VideoFileUpload(string FileName)
        {
            info_lbl.Text = "Video Yükleniyor...";
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFile(FileName + ".mp4");
                zip.Save(FileName + ".zip");

                string path = Application.StartupPath + "\\" + FileName + ".mp4";
                //try
                //{
                    byte[] byteArray = File.ReadAllBytes(path);

                    string filePath = path;
                    string fileName = FileName + ".mp4";
                    string description = FileName + ".mp4";
                    string fileType = "video/mp4";
                    driveUploader.UploadFile(ref byteArray, fileName, description, fileType);


                    info_lbl.Text = "QR Kod Oluşturuluyor...";
                    
                    QR_PB.Image = KareKodOlustur(DriveUploader.FileId);
                    
                    info_lbl.Text = "QR Kod Oluşturuldu.";

                    if (File.Exists(FileName + ".mp4"))
                    {
                        File.Delete(FileName + ".zip");
                        File.Delete(FileName + ".mp4");
                    }
                    if (Directory.Exists(DirectoryName))
                    {
                        string txtName = DirectoryName + "\\Tetik\\a.txt";
                        if (File.Exists(txtName) == true)
                        {
                            File.Delete(txtName);
                        }
                    }

                    TxtReadTimer.Start();

                //}
                //catch (Exception)
                //{
                //    MetroFramework.MetroMessageBox.Show(this, "\nDosya Yükleme Esnasında Hata\nSistemi Yeniden Başlatın \nSorun Devam Ederse Üreticinize Başvurun", "HATA!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}


            }
        }
        private Image KareKodOlustur(string FileId)
        {
            var url = "https://drive.google.com/open?id=" + FileId;
            QRCodeEncoder qe = new QRCodeEncoder();
            qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            qe.QRCodeVersion = 4;
            Bitmap bm = qe.Encode(url);
            return bm;
        }

        private void EndOfDaySendMailAndWriteFile()
        {
            try
            { 
                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                string FileName = DirectoryName + "\\" + Date + ".txt";

                Body = Date + " tarihindeki günlük toplam katılımcı sayısı " + TotalFaceCount.ToString() + " 'dır.";

                SendMail.Send(From, Password, Title, To, Subject, Body, Attachments, SmtpHost, SmtpPort);
                
                fs = new FileStream(FileName, FileMode.Create);
                sw = new StreamWriter(fs);
                sw.WriteLine(Body);
                EndOfDayMailControl = false;
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                MetroFramework.MetroMessageBox.Show(this, "\nGünsonu İşleminde Hata\nSistemi Yeniden Başlatın \nSorun Devam Ederse Üreticinize Başvurun", "HATA!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
