using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace XFiler.SDK;

public interface INaturalStringComparer : IComparer<string>, IComparer
{
    ListSortDirection SortDirection { get; set; }
}