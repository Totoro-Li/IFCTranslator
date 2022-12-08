using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static IFCTranslator.Definitions;
using static IFCTranslator.FileIO;
using static IFCTranslator.Utility;

namespace IFCTranslator
{
    #region Test

    public static class FanyiV3DemoInternalTest
    {
        public static void Test()
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            string url = YoudaoUrl;
            string query = "北京大学";
            string appKey = "19120b4dbcdddd49";
            string appSecret = "KtclDdevlFwg8RG7RtKXYdMkahfYPXrf";
            string salt = DateTime.Now.Millisecond.ToString();
            dic.Add("from", YoudaoLanguageCode.zh_CHS);
            dic.Add("to", YoudaoLanguageCode.en);
            dic.Add("signType", "v3");
            string curtime = GetTimeStamp();
            dic.Add("curtime", curtime);
            string signStr = appKey + Truncate(query) + salt + curtime + appSecret;

            string sign = ComputeHash(signStr, new SHA256CryptoServiceProvider());
            dic.Add("q", HttpUtility.UrlEncode(query));
            dic.Add("appKey", appKey);
            dic.Add("salt", salt);
            dic.Add("sign", sign);
            dic.Add("vocabId", "1AE8CBB8068549A78F3EB2D8CDBE13CE");
            Post(url, dic);
        }

        internal static string[] GetResult(string jsonCode)
        {
            string[] result = new string[3];
            if (JsonConvert.DeserializeObject(jsonCode) is JObject jo)
            {
                if ((string)jo["errorCode"] == "0")
                {
                    result[0] = (string)jo["translation"][0];
                    if (jsonCode.Contains("\"basic\":{") && jsonCode.Contains("\"explains\":")) //如果json数组里有这两个东西，说明该词有例子，可以执行代码
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in jo["basic"]["explains"])
                        {
                            sb.AppendLine(item.ToString());
                        }

                        result[1] = sb.ToString();
                    }

                    if (jsonCode.Contains("\"web\":[") && jsonCode.Contains("\"value\":")) //同上，不过这个是解释
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in jo["web"][1]["value"])
                        {
                            sb.AppendLine(item.ToString());
                        }

                        result[2] = sb.ToString();
                    }

                    return result;
                }
                else
                {
                    result[0] = "";
                    result[1] = "";
                    result[2] = "";
                    return result;
                }
            }
            else
            {
                result[0] = "";
                result[1] = "";
                result[2] = "";
                return result;
            }
        }

        private static void Post(string url, Dictionary<String, String> dic)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }

            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp.ContentType.ToLower().Equals("audio/mp3"))
            {
                SaveBinaryFile(resp, "合成的音频存储路径");
            }
            else
            {
                Stream stream = resp.GetResponseStream();
                string result;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }

                Console.WriteLine(result);
            }
        }

        private static string Truncate(string? q)
        {
            if (q != null)
            {
                int len = q.Length;
                return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
            }

            return string.Empty;
        }

        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            string filePath = FileName + DateTime.Now.Millisecond.ToString() + ".mp3";
            bool value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                Stream outStream = File.Create(filePath);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                } while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                value = false;
            }

            return value;
        }
    }

    #endregion

    public class YoudaoZhiyun : ITranslator
    {
        private string _appId, _appSecret;
        private string? _errorInfo;
        private Dictionary<int, string> TokenMap { get; set; } = new();

        private int _currentToken = 0;
        public int GetToken() => _currentToken++;

        public YoudaoZhiyun()
        {
            var iniFile = new IniFile("config.ini");
            _appId = iniFile.IniReadValue("YoudaoZhiyun", "AppId");
            _appSecret = iniFile.IniReadValue("YoudaoZhiyun", "AppSecret");
        }

        public void TokenMapAdd(int token, string flowNumber)
        {
            TokenMap.Add(token, flowNumber);
        }

        public string TokenMapGet(int token)
        {
            return TokenMap[token];
        }

        public void TranslatorInit(string appid, string appsecret)
        {
            _appId = appid;
            _appSecret = appsecret;
        }


        public async Task<string?> TranslateAsync(string sourceText, string desLang, string srcLang)
        {
            if (sourceText == String.Empty || desLang == String.Empty || srcLang == String.Empty)
            {
                _errorInfo = "Param Missing";
                return null;
            }

            string q = sourceText;
            string input = q.Length <= 20 ? q : q.Substring(0, 10) + q.Length + q.Substring(q.Length - 10);
            string salt = DateTime.Now.Millisecond.ToString();
            string curtime = GetTimeStamp();
            SHA256 sha = SHA256.Create();
            string sign = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(_appId + input + salt + curtime + _appSecret))).Replace("-", "").ToLower();
            sha.Dispose();

            Dictionary<String, String> dic = new Dictionary<String, String>
            {
                { "from", LangConversion(srcLang) },
                { "to", LangConversion(desLang) },
                { "signType", "v3" },
                { "curtime", curtime },
                { "appKey", _appId },
                { "salt", salt },
                { "sign", sign },
                { "q", HttpUtility.UrlEncode(q) }
            };

            string payload = BuildPayload(dic);

            StringContent request = new StringContent(payload, null, "application/x-www-form-urlencoded");

            try
            {
                HttpResponseMessage response = await GetHttpClient().PostAsync(YoudaoUrl, request);
                if (response.IsSuccessStatusCode)
                {
                    string resultStr = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<YoudaoZhiyunResult>(resultStr);
                    if (result.errorCode == "0")
                    {
                        return string.Join("\n", result.translation);
                    }
                    else
                    {
                        _errorInfo = "API error code: " + result.errorCode;
                        return null;
                    }
                }
                else
                {
                    _errorInfo = "API response code: " + response.StatusCode;
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _errorInfo = ex.Message;
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _errorInfo = ex.Message;
                return null;
            }
        }

        public string GetLastError()
        {
            return _errorInfo;
        }

        public event TranslateEventHandler? OnTranslateEvent;

        /// <summary>
        /// 有道智云API申请地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_allpyAPI()
        {
            return "https://ai.youdao.com/product-fanyi-text.s";
        }

        /// <summary>
        /// 有道智云API额度查询地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_bill()
        {
            return "https://ai.youdao.com/console";
        }

        /// <summary>
        /// 有道智云API文档地址（错误代码）
        /// </summary>
        /// <returns></returns>
        public static string GetUrl_Doc()
        {
            return "https://ai.youdao.com/DOCSIRMA/html/自然语言翻译/API文档/文本翻译服务/文本翻译服务-API文档.html";
        }

        private string BuildPayload(Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }

            return builder.ToString();
        }

        private string LangConversion(string lang)
        {
            if (lang == "zh")
                return "zh-CHS";
            else if (lang == "jp")
                return "ja";
            else if (lang == "kr")
                return "ko";
            else
                return lang;
        }


        public void SetHandler(TranslateEventHandler handler)
        {
            OnTranslateEvent += handler;
        }

        public void RemoveHandler(TranslateEventHandler handler)
        {
            OnTranslateEvent -= handler;
        }

        /// <summary>
        /// Upload file to Youdao Zhiyun
        /// Output:
        /// 字段名 | 类型 | 字段说明
        /// errorCode | text | 错误码，一定存在
        /// flownumber | text | 文档流水号，上传成功一定存在
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="srcLang"></param>
        /// <param name="dstLang"></param>
        /// <returns>Token to retrieve flow number </returns>
        public YoudaoErrorCode Upload(string filepath, string srcLang, string dstLang)
        {
            Dictionary<String, String> dic = new();
            string url = YoudaoUploadUrl;

            FileInfo fi=new FileInfo(filepath);
            #region Get Sign

            string q = LoadAsBase64(filepath);
            string salt = DateTime.Now.Millisecond.ToString();
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            long millis = (long)ts.TotalMilliseconds;
            string curtime = Convert.ToString(millis / 1000);
            SHA256 sha = SHA256.Create();
            string sign = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(_appId + Truncate(q) + salt + curtime + _appSecret))).Replace("-", "").ToLower();
            sha.Dispose();

            #endregion

            dic.Add("q", HttpUtility.UrlEncode(q));
            dic.Add("fileName", "ToUpload");
            dic.Add("fileType", fi.Extension[1..]);
            dic.Add("langFrom", srcLang);
            dic.Add("langTo", dstLang);
            dic.Add("appKey", _appId);
            dic.Add("salt", salt);
            dic.Add("curtime", curtime);
            dic.Add("sign", sign);
            dic.Add("docType", "json");
            dic.Add("signType", "v3");
            string result = Post(url, dic);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result)!;
            Console.WriteLine(result);
            YoudaoErrorCode errorcode = (YoudaoErrorCode)(int)(jo["errorCode"] ?? UnknownEcIndex);
            if (errorcode == YoudaoErrorCode.Ec0)
            {
                string? flowNumber = (string?)jo["flowNumber"];
                if (!string.IsNullOrEmpty(flowNumber))
                {
                    TokenMapAdd(GetToken(), flowNumber);
                    return YoudaoErrorCode.Ec0;
                }

                _errorInfo = "API error code: " + jo["errorCode"];
                return errorcode;
            }

            _errorInfo = "API error code: " + errorcode;
            OnTranslateEvent?.Invoke(this, new TranslateEventArgs { Code = (int)errorcode, Message = GetDescription(errorcode) });
            return errorcode;
        }

        /// <summary>
        /// Get document processing status
        /// 
        /// Output:
        /// errorCode | text | 错误码，一定存在
        /// status | text | 进度状态码，查询成功一定存在
        /// statusString | text | 进度状态描述，查询成功一定存在
        /// </summary>
        /// <param name="flowNumber"></param>
        /// <returns></returns>
        public YoudaoErrorCode Query(string flowNumber)
        {
            Dictionary<String, String> dic = new();

            string url = YoudaoQueryUrl;

            #region Get Sign

            string salt = DateTime.Now.Millisecond.ToString();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long millis = (long)ts.TotalMilliseconds;
            string curtime = Convert.ToString(millis / 1000);
            SHA256 sha = SHA256.Create();
            string sign = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(_appId + Truncate(flowNumber) + salt + curtime + _appSecret))).Replace("-", "").ToLower();
            sha.Dispose();

            #endregion

            dic.Add("flownumber", flowNumber);
            dic.Add("appKey", _appId);
            dic.Add("salt", salt);
            dic.Add("curtime", curtime);
            dic.Add("sign", sign);
            dic.Add("docType", "json");
            dic.Add("signType", "v3");
            string result = Post(url, dic);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result)!;
            Console.WriteLine(result);
            YoudaoErrorCode errorcode = (YoudaoErrorCode)(int)(jo["errorCode"] ?? UnknownEcIndex);
            if (errorcode == YoudaoErrorCode.Ec0)
            {
                YoudaoStatusCode status = (YoudaoStatusCode)(int)(jo["status"] ?? UnknownEcIndex);
                OnTranslateEvent?.Invoke(this, new TranslateEventArgs { EventType = TranslateEventType.Status, Code = (int)status, Message = GetDescription(status) });
                _errorInfo = "Status code: " + jo["status"];
                return errorcode;
            }

            _errorInfo = "API error code: " + errorcode;
            OnTranslateEvent?.Invoke(this, new TranslateEventArgs { Code = (int)errorcode, Message = GetDescription(errorcode) });
            return errorcode;
        }

        public YoudaoErrorCode Query(int token)
        {
            string? flowNumber = TokenMapGet(token);
            if (string.IsNullOrEmpty(flowNumber))
            {
                _errorInfo = "Token not found";
                return YoudaoErrorCode.EcUnknown;
            }
            return Query(flowNumber);
        }

        public async Task<YoudaoErrorCode> Download(string flowNumber, string fileDir)
        {
            Dictionary<String, String> dic = new();

            string url = YoudaoDownloadUrl;

            #region Get Sign

            string salt = DateTime.Now.Millisecond.ToString();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long millis = (long)ts.TotalMilliseconds;
            string curtime = Convert.ToString(millis / 1000);
            SHA256 sha = SHA256.Create();
            string sign = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(_appId + Truncate(flowNumber) + salt + curtime + _appSecret))).Replace("-", "").ToLower();
            sha.Dispose();

            #endregion

            dic.Add("flownumber", flowNumber);
            dic.Add("downloadFileType", "word");
            dic.Add("appKey", _appId);
            dic.Add("salt", salt);
            dic.Add("curtime", curtime);
            dic.Add("sign", sign);
            dic.Add("docType", "json");
            dic.Add("signType", "v3");
            YoudaoErrorCode errorCode = await PostAndSave(url, dic, fileDir);
            OnTranslateEvent?.Invoke(this, new TranslateEventArgs { EventType = TranslateEventType.Error, Code = (int)errorCode, Message = GetDescription(errorCode) });
            return errorCode;
        }
        public async Task<YoudaoErrorCode> Download(int token, string fileDir)
        {
            string? flowNumber = TokenMapGet(token);
            if (string.IsNullOrEmpty(flowNumber))
            {
                _errorInfo = "Token not found";
                return YoudaoErrorCode.EcUnknown;
            }
            return await Download(flowNumber, fileDir);
        }
    }

    struct YoudaoZhiyunResult
    {
        public string errorCode, query, l;
        public string[] translation;
    }
}