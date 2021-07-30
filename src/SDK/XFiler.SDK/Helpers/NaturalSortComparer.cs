using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFiler.SDK
{
    public class NaturalSortComparer : IComparer<string>, IDisposable, IComparer
    {
        #region Private Fields

        private readonly bool _isAscending;

        private Dictionary<string, string[]> _table = new();

        #endregion

        #region Constructor

        public NaturalSortComparer(bool inAscendingOrder = true)
        {
            this._isAscending = inAscendingOrder;
        }

        #endregion

        #region Public Methods

        public int Compare(string x, string y)
        {
            if (x == y)
                return 0;

            if (!_table.TryGetValue(x, out var x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                _table.Add(x, x1);
            }

            if (!_table.TryGetValue(y, out var y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                _table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    returnVal = PartCompare(x1[i], y1[i]);
                    return _isAscending ? returnVal : -returnVal;
                }
            }

            if (y1.Length > x1.Length)
            {
                returnVal = 1;
            }
            else if (x1.Length > y1.Length)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return _isAscending ? returnVal : -returnVal;
        }

        public void Dispose()
        {
            _table.Clear();
            _table = null;
        }

        #endregion

        #region Private Methods

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        #endregion

        public int Compare(object? x, object? y) 
            => Compare(x as string, y as string);
    }
}