using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Framework
{
    public class EmailContentBuilder
    {
        private string template { get; set; }
        private string emailContent { get; set; }

        private Dictionary<string, string> dicReplace;

        public EmailContentBuilder(string path, Dictionary<string, string> dicReplaceItem)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(HttpContext.Current.Server.MapPath(path));
            this.template = xml.InnerXml;
            this.emailContent = this.template;
            this.dicReplace = dicReplaceItem;
        }

        private void Replace()
        {
            foreach (var item in dicReplace)
            {

                this.emailContent = this.emailContent.Replace($"%{item.Key}%", item.Value);
            }
        }

        public virtual void Construct()
        {
            this.Replace();
        }

        public string GetContent()
        {
            return this.emailContent;
        }

    }
}
