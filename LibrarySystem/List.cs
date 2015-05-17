using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem
{
    class List
    {
        public enum Kind
        {
            Both,
            Fiction,
            NonFiction
        }

        public enum FictionType
        {
            All,
            Novel,
            ShortStory
        }

        public enum NonFictionType
        {
            All,
            Science,
            Medical,
            History,
            English,
            Computer,
            Math,
            Management,
            Accounting,
            Chinese
        }

        public static string[] Course = new string[]
        {
            "All",
            "BS Information Technology",
            "BS Computer Science",
            "BS Management",
            "BS Accounting",
            "BS Nursing",
            "BS Education",
            "BS Marketing"
        };

        public static string[] DiffReport = new string[]
        {
            "List of Student With Books Not Yet Returned"
        };
    }
}
