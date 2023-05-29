﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RentDesktop.Infrastructure.Extensions
{
    internal static class ObservableCollectionExtensions
    {
        public static void ResetItems<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.AddRange(items);
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}
