using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EducationalPlatform.Extensions
{
        public static class ObservableCollectionExtensions
        {
            public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
            {
                foreach (var item in items)
                {
                    coll.Add(item);
                }
            }
        }
    
}
