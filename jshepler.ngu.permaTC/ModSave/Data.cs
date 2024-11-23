using System.Collections.Generic;

namespace jshepler.ngu.mods.ModSave
{
    internal static class Data
    {
        internal static Dictionary<string, object> Values = new();

        internal static T Get<T>(string key, T defaultValue = default)
        {
            if (!Values.ContainsKey(key))
                Values[key] = defaultValue;

            return (T)Values[key];
        }

        internal static void Set(string key, object value)
        {
            if (!Values.ContainsKey(key))
                Values.Add(key, value);
            else
                Values[key] = value;
        }

        internal static float PTC_Timer
        {
            get => Get("PTC_Timer", 0f);
            set => Set("PTC_Timer", value);
        }

        internal static int PTC_TrollCount
        {
            get => Get("PTC_TrollCount", 0);
            set => Set("PTC_TrollCount", value);
        }

        // REMINDER: NO TYPES DEFINED IN MODS ELSE NOT LOADABLE IN VANILLA
    }
}
