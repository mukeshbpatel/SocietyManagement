using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

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
            string strHtml = header + html + (Signature ? sign : string.Empty) + footer;

            MemoryStream ms = new MemoryStream();
            using (Document document = new Document(PageSize.A4, 25, 25, 30, 30))
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
    }
}