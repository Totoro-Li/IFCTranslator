using System.ComponentModel;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static IFCTranslator.Definitions;

namespace IFCTranslator;

public class Utility
{
    public static string GetTimeStamp()
    {
        TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        long millis = (long)ts.TotalMilliseconds;
        return Convert.ToString(millis / 1000);
    }

    static HttpClient? _commonHttpClient;

    /// <summary>
    /// 获得HttpClinet单例，第一次调用自动初始化
    /// </summary>
    public static HttpClient GetHttpClient()
    {
        if (_commonHttpClient == null)
            lock (typeof(Utility))
                if (_commonHttpClient == null)
                {
                    _commonHttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(8) };
                    var headers = _commonHttpClient.DefaultRequestHeaders;
                    headers.UserAgent.ParseAdd(Header);
                    headers.Connection.ParseAdd("keep-alive");
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                }

        return _commonHttpClient;
    }

    public static ITranslator? TranslatorAuto(PlatformApis platform)
    {
        switch (platform)
        {
            case PlatformApis.Youdao:
                YoudaoZhiyun youdao = new YoudaoZhiyun();
                return youdao;
            default:
                return null;
        }
    }

    public static List<string> GetEnumDescList(Type target)
    {
        var temp = Enum.GetNames(target).Select(x => target.GetMember(x)[0].GetCustomAttributes(typeof(DescriptionAttribute), false));
        return temp.Select(x => x.Length > 0 ? ((DescriptionAttribute)x[0]).Description : String.Empty).ToList();
    }

    public static string GetDescription<T>(T enumValue) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
            return String.Empty;

        var description = enumValue.ToString();

        if (description != null)
        {
            var fieldInfo = enumValue.GetType().GetField(description);

            if (fieldInfo != null)
            {
                object[] attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
        }

        return description ?? string.Empty;
    }

    public static string LoadAsBase64(string filename)
    {
        try
        {
            FileStream filestream = new FileStream(filename, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Position = 0;
            filestream.Read(arr, 0, (int)filestream.Length);
            filestream.Close();
            return Convert.ToBase64String(arr);
        }
        catch (Exception ex)
        {
            return String.Empty;
        }
    }

    public static string Truncate(string q)
    {
        if (string.IsNullOrEmpty(q))
            return String.Empty;
        int len = q.Length;
        return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
    }

    public static string ComputeHash(string input, HashAlgorithm algorithm)
    {
        Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
        return BitConverter.ToString(hashedBytes).Replace("-", "");
    }

    public static string Post(string url, Dictionary<String, String> dic)
    {
        string result;
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
        Stream stream = resp.GetResponseStream();
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }

        return result;
    }

    public static async Task<YoudaoErrorCode> PostAndSave(string url, Dictionary<String, String> dic, string fileDir)
    {
        string result;
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
        Stream stream = resp.GetResponseStream();
        if (resp.ContentType == "application/json") //Failure downloading
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = await reader.ReadToEndAsync();
            }

            JObject jo = (JObject)JsonConvert.DeserializeObject(result)!;
            YoudaoErrorCode errorcode = (YoudaoErrorCode)(int)(jo["errorCode"] ?? UnknownEcIndex);
            return errorcode;
        }

        //Success
        FileStream fstream = new FileStream(fileDir, FileMode.Create);
        await stream.CopyToAsync(fstream);
        stream.Close();
        fstream.Close();
        return YoudaoErrorCode.Ec0;
    }
}