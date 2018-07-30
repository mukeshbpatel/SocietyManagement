using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SocietyManagement.Models
{
    public static class PdfGenrator
    {         
        public static Byte[] HTMLtoPDF(string html,Boolean Signature = true)
        {
            string header = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Header.html"));
            string footer = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Footer.html"));
            string sign = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Signature.html"));
            string css = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Style.css"));
            string strHtml =UpdateSettings("<div id='container'>" + header + html + (Signature ? sign : string.Empty) + footer + "</div>");

            MemoryStream ms = new MemoryStream();
            using (Document document = new Document(PageSize.A4, 25, 25, 25, 25))
            using (MemoryStream srHTML = new MemoryStream(Encoding.UTF8.GetBytes(strHtml)))
            using (MemoryStream srCss = new MemoryStream(Encoding.UTF8.GetBytes(css)))
            {
                PdfWriter writer = PdfWriter.GetInstance(document,ms);
                document.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, srHTML, srCss);
                document.Close();
                ms.Close();
            }
            return ms.GetBuffer();
        }

        public static Byte[] HTMLtoExcel(string html, Boolean Signature = true)
        {
            string header = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Header.html"));
            string footer = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Footer.html"));
            string sign = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Signature.html"));
            string css = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Style.css"));
            string strHtml = UpdateSettings("<style>" + css + "</style><div id='container'>" + header + html + (Signature ? sign : string.Empty) + footer + "</div>");            
            return Encoding.ASCII.GetBytes(strHtml);
        }

        private static string UpdateSettings(string str)
        {
            str = str.Replace("{{Society.Name}}", SiteSetting.Item("Society.Name"))
                .Replace("{{Society.FullName}}", SiteSetting.Item("Society.FullName"))
                .Replace("{{Society.Address}}", SiteSetting.Item("Society.Address"))
                .Replace("{{Society.URL}}", SiteSetting.Item("Society.URL"))
                .Replace("{{Society.Email}}", SiteSetting.Item("Society.Email"))
                .Replace("{{Society.Phone}}", SiteSetting.Item("Society.Phone"))
                .Replace("{{Society.RegNo}}", SiteSetting.Item("Society.RegNo"))
                .Replace("{{Site.Name}}", SiteSetting.Item("Site.Name"))
                .Replace("{{Site.URL}}", SiteSetting.Item("Site.URL"));            
            return str;
        }
    }
}