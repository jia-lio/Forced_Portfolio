using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nullbytes
{
    public static class Localize
    {
        public static bool Initialized { get; private set; }
        public static Locale Current { get; private set; } = Locale.KR;

        public static event Action<Locale> OnChanged;

        private static readonly Dictionary<string, string> _map = new();

        public static void Initialize()
        {
            //저장된 언어를 기반으로 불러오는거 해야함!!
            //일단 시스템 기반 불러오기
            Current = DetectFromSystem();
            
            Rebuild();

            Initialized = true;
            OnChanged?.Invoke(Current);
        }

        private static void Rebuild()
        {
            _map.Clear();

            var datas = ExcelTable.Instance.GetTableSO<LocalizationTableSO, LocalizationTable>();
            foreach (var data in datas.rows)
            {
                if (string.IsNullOrEmpty(data.Key))
                    continue;

                var text = Current switch
                {
                    Locale.KR => data.KR,
                    Locale.EN => data.EN,
                    Locale.JA => data.JA,
                    _ => data.EN
                };

                if (string.IsNullOrEmpty(text))
                {
                    text = string.IsNullOrEmpty(data.EN) ? data.Key : data.EN;
                }

                _map[data.Key] = text;
            }
        }

        public static void SetLocale(Locale locale)
        {
            if (Initialized == false)
                return;
            
            if (Current == locale)
                return;

            Current = locale;
            Rebuild();
            
            OnChanged?.Invoke(Current);
        }

        public static string Get(string key, params object[] args)
        {
            if (Initialized == false)
                return key;

            if (string.IsNullOrEmpty(key))
                return string.Empty;

            if (_map.TryGetValue(key, out var v) == false || string.IsNullOrEmpty(v))
            {
                v = key;
            }

            return (args != null && args.Length > 0) ? string.Format(v, args) : v;
        }

        private static Locale DetectFromSystem() => Application.systemLanguage switch
        {
            SystemLanguage.Korean => Locale.KR,
            SystemLanguage.English => Locale.EN,
            SystemLanguage.Japanese => Locale.JA,
        };
    }
}