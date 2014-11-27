using System.Collections.Generic;
using System.ComponentModel;

namespace Emulation.Debugger.MVVM
{
    internal static class PropertyChangedEventArgsCache
    {
        private static Dictionary<string, PropertyChangedEventArgs> eventArgsCache;
        private static object gate = new object();

        public static PropertyChangedEventArgs GetOrCreate(string propertyName)
        {
            PropertyChangedEventArgs result;

            lock (gate)
            {
                if (eventArgsCache == null)
                {
                    eventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
                }

                if (!eventArgsCache.TryGetValue(propertyName, out result))
                {
                    result = new PropertyChangedEventArgs(propertyName);
                    eventArgsCache.Add(propertyName, result);
                }
            }

            return result;
        }
    }
}
