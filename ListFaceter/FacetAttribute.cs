using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListFaceter
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FacetAttribute : System.Attribute
    {
        private string name = string.Empty;
        private string delimiter = string.Empty;
        private bool islist = false;

        public FacetAttribute(string Name)
        {
            this.name = Name;
        }

        public FacetAttribute(string Name, bool IsList, string Delimiter)
        {
            this.name = Name;
            this.islist = IsList;
            this.delimiter = Delimiter;
        }


        public string GetName()
        {
            return name;
        }

        public string GetDelimiter()
        {
            return delimiter;
        }

        public bool IsList()
        {
            return islist;
        }

    }
}
