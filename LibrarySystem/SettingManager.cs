using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LibrarySystem
{
    class SettingManager
    {
        private string Username = "";
        private string Password = "";
        private int Fine = 0;
        private int DaysAllowed = 0;
        private int MaxBookAllowed = 0;
        private XmlDocument xmldoc = null;

        public SettingManager()
        {
            xmldoc = new XmlDocument();
            xmldoc.Load("Settings.xml");
            Username = xmldoc.DocumentElement.SelectSingleNode("/setting/username").InnerText.Replace("\r\n","").Trim();
            Password = xmldoc.DocumentElement.SelectSingleNode("/setting/password").InnerText.Replace("\r\n", "").Trim();
            Fine = int.Parse(xmldoc.DocumentElement.SelectSingleNode("/setting/fine").InnerText);
            DaysAllowed = int.Parse(xmldoc.DocumentElement.SelectSingleNode("/setting/daysallowed").InnerText);
            MaxBookAllowed = int.Parse(xmldoc.DocumentElement.SelectSingleNode("/setting/maxbookallowed").InnerText);
        }

        public bool checklogin(string Username, string Password)
        {
            if (Username == this.Username && Password == this.Password)
                return true;
            return false;
        }

        public void updateSetting(string username, string password, int fine, int daysallowed, int maxbookallowed)
        {
            xmldoc.DocumentElement.SelectSingleNode("/setting/username").InnerText = username;
            xmldoc.DocumentElement.SelectSingleNode("/setting/password").InnerText = password;
            xmldoc.DocumentElement.SelectSingleNode("/setting/fine").InnerText = fine.ToString();
            xmldoc.DocumentElement.SelectSingleNode("/setting/daysallowed").InnerText = daysallowed.ToString();
            xmldoc.DocumentElement.SelectSingleNode("/setting/maxbookallowed").InnerText = maxbookallowed.ToString();
            xmldoc.Save("Settings.xml");
        }

        public string username()
        {
            return this.Username;
        }

        public string password()
        {
            return this.Password;
        }

        public int fine()
        {
            return this.Fine;
        }

        public int daysallowed()
        {
            return this.DaysAllowed;
        }

        public int maxbookallowed()
        {
            return this.MaxBookAllowed;
        }
    }
}
