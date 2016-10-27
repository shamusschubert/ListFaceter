using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ListFaceter
{
    public class FacetedList<T> : IEnumerable<T>, IList<T>
    {
        private Dictionary<string, Dictionary<string, List<T>>> facets;
        public Dictionary<string, Dictionary<string, List<T>>> Facets
        {
            get
            {
                return facets;
            }
        }

        private List<T> _innerList;

        public FacetedList()
        {
            _innerList = new List<T>();
            facets = new Dictionary<string, Dictionary<string, List<T>>>();
        }

        public FacetedList<T> GetInnerFacet(string Property, string Value)
        {
            if (!facets.ContainsKey(Property))
            {
                throw new ArgumentException(String.Format("The property - {0}, is not in the dictionary", Property));
            }

            if (!facets[Property].ContainsKey(Value))
            {
                throw new ArgumentException(String.Format("The property-value pair {0}-{1} is not present in the dictionary", Property, Value));
            }

            List<T> innerList = facets[Property][Value];

            FacetedList<T> newList = new FacetedList<T>();

            foreach (T obj in innerList)
            {
                newList.Add(obj);
            }

            return newList;
        }


        #region IList<T> Members
        public int IndexOf(T item)
        {
            return _innerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _innerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }


        public T this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                _innerList[index] = value;
            }
        }
        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            _innerList.Add(item);

            foreach (PropertyInfo Property in item.GetType().GetProperties())
            {
                if (Property.GetCustomAttributes(typeof(FacetAttribute), true).Count() > 0)
                {
                    if (Property.CanRead)
                    {
                        object propvalue = Property.GetValue(item, null);  //is this safe?

                        object[] attribs = Property.GetCustomAttributes(typeof(FacetAttribute), true);

                        string propname = ((FacetAttribute)attribs[0]).GetName();
                        bool islist = ((FacetAttribute)attribs[0]).IsList();
                        string delimiter = ((FacetAttribute)attribs[0]).GetDelimiter();

                        if (!String.IsNullOrEmpty(propname) && propvalue != null)
                        {
                            string str_propvalue = propvalue.ToString();

                            if (!string.IsNullOrEmpty(str_propvalue))
                            {
                                string[] splitted = null;

                                if(islist)     // value has delimiters which indicates it as an array list
                                {
                                    string[] delim = {delimiter};
                                    splitted = str_propvalue.Split(delim, StringSplitOptions.None);
                                }

                                if (!facets.ContainsKey(propname))
                                {
                                    facets.Add(propname, new Dictionary<string, List<T>>());
                                }

                                if(!islist)
                                {
                                    if (!facets[propname].ContainsKey(str_propvalue))
                                    {
                                        facets[propname].Add(str_propvalue, new List<T>());
                                    }

                                    facets[propname][str_propvalue].Add(item);
                                }
                                else
                                {
                                    // we have array values in splitted[]
                                    foreach(string str in splitted)
                                    {
                                        if (!facets[propname].ContainsKey(str))
                                        {
                                            facets[propname].Add(str, new List<T>());
                                        }

                                        facets[propname][str].Add(item);

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public void Clear()
        {
            _innerList.Clear();
        }

        public bool Contains(T item)
        {
            return _innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return _innerList.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }
        #endregion

    }

}
