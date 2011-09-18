// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadSpreadsheet.cs" company="RBS GBM">
//   Copyright © RBS GBM 2010
// </copyright>
// <summary>
//   Defines the ReadSpreadsheet type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VersionOneAdapter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Microsoft.Office.Interop.Excel;

    public class ReadSpreadsheet
    {

        public List<Story> ReadAllStories(string area, string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("m", "urn:schemas-microsoft-com:office:spreadsheet");
            namespaceManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            var x = doc.SelectSingleNode("//m:Table", namespaceManager);
            List<Story> list = new List<Story>();
            foreach (XmlElement row in doc.SelectNodes("//m:Table/m:Row", namespaceManager))
            {
                string[] data = new string[8];
                int index = 0;
                foreach (XmlElement cell in row.ChildNodes)
                {
                    if (cell.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"] != null)
                    {
                        index =
                            int.Parse(cell.Attributes["Index", "urn:schemas-microsoft-com:office:spreadsheet"].InnerText)-1;
                    }
                    data[index] = cell.InnerText;
                    index++;
                }

                var story = new Story();
                story.Title = string.Format("{0} - {1} {2}", area, System.Security.SecurityElement.Escape(data[0]), System.Security.SecurityElement.Escape(data[1]));
                StringBuilder stringBuilder = new StringBuilder(data[2] + data[3]);
                if (!string.IsNullOrWhiteSpace(data[5]))
                    stringBuilder.AppendFormat("<P> \n </P><BR/><P>\nAcceptance Criteria: \n{0}</P>", System.Security.SecurityElement.Escape(data[5]));
                if (!string.IsNullOrWhiteSpace(data[4]))
                    stringBuilder.AppendFormat("<P> \n </P><BR/><P>\nAssumptions: \n{0}</P>", System.Security.SecurityElement.Escape(data[4]));
                if (!string.IsNullOrWhiteSpace(data[6]))
                    stringBuilder.AppendFormat("<P> \n </P><BR/><P>\nQuestions: \n{0}</P>", System.Security.SecurityElement.Escape(data[6]));
                story.Description = stringBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(data[7]))
                    story.Estimate = int.Parse(data[7]);
                list.Add(story);
            }

            return list;
        }
    }

    public class Story
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Estimate { get; set; }
    }
}