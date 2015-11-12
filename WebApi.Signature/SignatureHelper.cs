using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Signature
{
    public class SignatureHelper
    {
        /// <summary>
        /// 自 1970 年以來所經過的秒數 | UTC 時間
        /// </summary>
        /// <returns>總秒數</returns>
        public static long GetUnixTimeSeconds()
        {
            DateTimeOffset _offset;
            DateTimeOffset _unix;

            _unix = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
            _offset = DateTimeOffset.Now;

            return Convert.ToInt64(
                (_offset - _unix).TotalSeconds
                );
        }

        /// <summary>
        /// 自 1970 年以來指定日期時間所經過的秒數 | UTC 時間
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">時</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <returns>總秒數</returns>
        public static long GetUnixTimeSeconds(int year, int month, int day, int hour, int minute, int second)
        {
            DateTimeOffset _offset;
            DateTimeOffset _unix;

            _unix = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
            _offset = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero);

            return Convert.ToInt64(
                (_offset - _unix).TotalSeconds
                );
        }

        /// <summary>
        /// 使用 MD5 雜湊指定字串，並將結果轉換成字串
        /// </summary>
        /// <param name="content">要雜湊字串</param>
        /// <returns>雜湊結果字串</returns>
        public static string MD5ToHex(string content)
        {
            if (string.IsNullOrWhiteSpace(content) == true)
            {
                return String.Empty;
            }

            StringBuilder _builder;
            Encoding _encoding;
            Byte[] _buffer;
            Byte[] _results;

            _builder = new StringBuilder();
            _encoding = Encoding.GetEncoding("utf-8");
            _buffer = _encoding.GetBytes(content);

            using (MD5 _hash = MD5.Create())
            {
                _results = _hash.ComputeHash(_buffer);

                for (int i = 0; i < _results.Length; i++)
                {
                    _builder.Append(_results[i].ToString("x2"));
                }
            }

            return (_builder.ToString().ToLower());
        }

        /// <summary>
        /// 將查詢字串轉換成 NameValueCollection 物件
        /// </summary>
        /// <param name="query">要轉換的查詢字串 ?xxx=xxx&xxx=xxx 或 xxx=aaa&yyy=bbb 皆可</param>
        /// <returns>NameValueCollection Key,Value 物件</returns>
        public static NameValueCollection ParseQueryString(string query)
        {
            NameValueCollection _collection;

            _collection = new NameValueCollection();

            ParseQueryString(query, _collection);

            return (_collection);
        }

        internal static void ParseQueryString(string query, NameValueCollection result)
        {
            if (query.Length == 0)
                return;

            string decoded = (query);
            //string decoded = HtmlDecode(query);
            int decodedLength = decoded.Length;
            int namePos = 0;
            bool first = true;
            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                        namePos++;
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = decoded.Substring(namePos, valuePos - namePos - 1);
                    //name = UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1), encoding);
                }
                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }
                value = decoded.Substring(valuePos, valueEnd - valuePos);
                //value = UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos), encoding);

                result.Add(name, value);
                if (namePos == -1)
                    break;
            }
        }
    }
}
