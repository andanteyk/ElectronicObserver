using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RecordView
{

    public class SortedBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;

        public void ReverseSelf()
        {
            List<T> list = this.Reverse().ToList();
            this.Clear();
            foreach(var s in list)
            {
                this.Add(s);
            }

        }
        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;

            if (items != null)
            {
                ObjectPropertyCompare<T> pc = new ObjectPropertyCompare<T>(property, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }

            sortProperty = property;
            sortDirection = direction;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        //排序
        public void Sort(PropertyDescriptor property, ListSortDirection direction)
        {
            this.ApplySortCore(property, direction);
        }
    }
    class ObjectPropertyCompare<T> : System.Collections.Generic.IComparer<T> 
    {
        private PropertyDescriptor property;
        private ListSortDirection direction;
 
        public ObjectPropertyCompare(PropertyDescriptor property, ListSortDirection direction)
        {
            this.property = property;
            this.direction = direction;
        }
 
        #region IComparer<T>
 
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">相对属性x</param>
        /// <param name="y">相对属性y</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            object xValue = x.GetType().GetProperty(property.Name).GetValue(x, null);
            object yValue = y.GetType().GetProperty(property.Name).GetValue(y, null);
 
            int returnValue;

            if (xValue is DateTime)
            {
                returnValue = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue is IComparable)
            {
                returnValue = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue.Equals(yValue))
            {
                returnValue = 0;
            }
            else
            {
                returnValue = xValue.ToString().CompareTo(yValue.ToString());
            }
 
            if (direction == ListSortDirection.Ascending)
            {
                return returnValue;
            }
            else
            {
                return returnValue * -1;
            }
        }
 
        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }
 
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
 
        #endregion
    }
}
