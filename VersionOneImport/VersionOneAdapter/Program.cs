using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VersionOneAdapter
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadSpreadsheet spreadsheet = new ReadSpreadsheet();
            var stories = spreadsheet.ReadAllStories("Contact management", @"c:\Temp\stories.xml");
            stories.RemoveRange(0, 31);

            var create = new CreateStory();
            foreach (var story in stories)
            {
                create.Execute(story);
            }
        }
    }
}
