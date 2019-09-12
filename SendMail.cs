using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail; // mail gönderme işlemleri için gereklidir.
using System.Text.RegularExpressions; // regex komutunu kullanabilmemiz için gereklidir.

namespace video_processing
{
    class SendMail
    {
        public static bool Send(string MailHesabi, string MailHesapSifresi, string MailUnvan, string MailAdresi, string MailKonu, string MailIcerik, string MailEkleri, string Pop3Host, int Pop3Port)
        {
            try
            {
                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(MailHesabi, MailHesapSifresi);
                // mail göndermek için oturum açtık

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(); // yeni mail oluşturduk
                mail.From = new System.Net.Mail.MailAddress(MailHesabi, MailUnvan); // maili gönderecek hesabı belirttik
                mail.To.Add(MailAdresi); // mail gönderilecek adres
                mail.Subject = MailKonu; // mailin konusu
                mail.IsBodyHtml = true; // mail içeriği html olarak gönderilsin
                mail.Body = MailIcerik; // mailin içeriği
                mail.Attachments.Clear(); // mail eklerini temizledik
                string[] sonuc1 = Regex.Split(MailEkleri, "/");
                // MailEkleri parametresinde mailie ekleyeceğimiz tüm dosyaları aralarına " / " koyarak birbilerine ekledik
                foreach (string items in sonuc1)
                {
                    if (items != "")
                    {
                        mail.Attachments.Add(new Attachment("\\Mail_Eklerinin_Yolu\\" + items));
                        //  MailEkleri parametresinden gelen veriyi " / " işareti sayesinde parçaladık. 
                        // Kaydettiğimiz yerin yolunu ile birlikte dosyaları aldık ve maile ekledik.
                    }
                }
                // göndereceğimiz maili hazırladık.

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Pop3Host, Pop3Port); // smtp servere bağlandık
                smtp.UseDefaultCredentials = false; // varsayılan girişi kullanmadık
                smtp.EnableSsl = true; // ssl kullanımına izin verdik
                smtp.Credentials = cred; // server üzerindeki oturumumuzu yukarıda belirttiğimiz NetworkCredential üzerinden sağladık.
                smtp.Send(mail); // mailimizi gönderdik.
                // smtp yani Simple Mail Transfer Protocol üzerinden maili gönderiyoruz.

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
