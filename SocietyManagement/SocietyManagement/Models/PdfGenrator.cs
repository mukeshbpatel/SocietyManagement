using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SocietyManagement.Models
{
    public class ITextEvents : PdfPageEventHelper
    {
        string html = null;
        public ITextEvents(string html)
        {
            this.html = html;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            ColumnText ct = new ColumnText(writer.DirectContent);
            XMLWorkerHelper.GetInstance().ParseXHtml(new ColumnTextElementHandler(ct), new StringReader(html));
            ct.SetSimpleColumn(document.Left, 50, document.Right, document.GetBottom(-200), 10, Element.ALIGN_MIDDLE);
            ct.Go();
        }
    }
    public class ColumnTextElementHandler:IElementHandler
    {
        ColumnText ct = null;
        public ColumnTextElementHandler(ColumnText ct)
        {
            this.ct = ct;
        }
        public void Add(IWritable w)
        {
            if(w is WritableElement)
            {
                foreach (IElement e in ((WritableElement)w).Elements())
                {
                    ct.AddElement(e);
                }
            }
        }
    }

    public static class PdfGenrator
    {         
        public static Byte[] HTMLtoPDF(string html,Boolean Signature = false,Boolean Footer = true)
        {
            string header = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Header.html"));            
            string sign = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Signature.html"));
            string css = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Style.css"));
            string strHtml =Helper.UpdateSettings(header + html + (Signature ? sign : string.Empty));

            MemoryStream ms = new MemoryStream();
            using (Document document = new Document(PageSize.A4, 25, 25, 20, 10))
            using (MemoryStream srHTML = new MemoryStream(Encoding.UTF8.GetBytes(strHtml)))
            using (MemoryStream srCss = new MemoryStream(Encoding.UTF8.GetBytes(css)))
            {
                PdfWriter writer = PdfWriter.GetInstance(document,ms);
                if (Footer)
                {
                    string footer = Helper.UpdateSettings(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Footer.html")));
                    writer.PageEvent = new ITextEvents(footer);
                }                    
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
            string strHtml = Helper.UpdateSettings("<style>" + css + "</style>" + header + html + (Signature ? sign : string.Empty) + footer);            
            return Encoding.ASCII.GetBytes(strHtml);
        }

       
    }
}