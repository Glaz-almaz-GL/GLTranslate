using System;
using System.Collections.Generic;
using System.Text;

namespace GLTranslate.Providers.Google.Generators
{
    /// <summary>
    /// Генератор токена безопасности для Google Translate API.
    /// Алгоритм основан на клиентской реализации Google.
    /// </summary>
    internal static class GoogleTokenGenerator
    {
        private const string Salt1 = "+-a^+6";
        private const string Salt2 = "+-3^+b+-f";

        /// <summary>
        /// Генерирует токен для указанного текста.
        /// </summary>
        /// <param name="text">Текст для перевода.</param>
        /// <returns>Токен в формате "a.a^b".</returns>
        public static string Generate(ReadOnlySpan<char> text)
        {
            long a = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600;
            long b = a;

            foreach (char ch in text)
            {
                a = ProcessToken(a + ch, Salt1);
            }

            a = ProcessToken(a, Salt2);

            if (a < 0)
            {
                a = (a & int.MaxValue) + int.MaxValue + 1;
            }

            a %= 1_000_000;

            return $"{a}.{a ^ b}";
        }

        /// <summary>
        /// Обработка токена с использованием соли.
        /// </summary>
        private static long ProcessToken(long num, string seed)
        {
            for (int i = 0; i < seed.Length - 2; i += 3)
            {
                int d = seed[i + 2];

                if (d >= 'a')
                {
                    d -= 'W';
                }

                if (seed[i + 1] == '+')
                {
                    num = (num + (num >> d)) & uint.MaxValue;
                }
                else
                {
                    num ^= num << d;
                }
            }

            return num;
        }
    }
}
