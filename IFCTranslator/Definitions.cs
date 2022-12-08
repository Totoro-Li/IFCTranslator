using System.ComponentModel;
using DeepL;

namespace IFCTranslator;

public class Definitions
{
    public const string Header = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";

    public const int UnknownEcIndex = 99999;

    public delegate void TranslateEventHandler(object sender, TranslateEventArgs e);

    public class TranslateEventArgs
    {
        public TranslateEventType EventType { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public enum TranslateEventType
    {
        [Description("Error Code")] Error,
        [Description("Status Code")] Status
    }

    public enum PlatformApis
    {
        [Description("DeepL翻译")] DeepL,
        [Description("有道翻译")] Youdao,
        [Description("谷歌翻译")] Google
    }

    #region DeepL Definitions

// The following are the enumeration of supported languages of DeepL
    public enum DeepLLanguages
    {
        Bulgarian,
        Czech,
        Danish,
        German,
        Greek,
        English,
        EnglishBritish,
        EnglishAmerican,
        Spanish,
        Estonian,
        Finnish,
        French,
        Hungarian,
        Indonesian,
        Italian,
        Japanese,
        Lithuanian,
        Latvian,
        Dutch,
        Polish,
        Portuguese,
        PortugueseBrazilian,
        PortugueseEuropean,
        Romanian,
        Russian,
        Slovak,
        Slovenian,
        Swedish,
        Turkish,
        Ukrainian,
        Chinese
    }

    #endregion

    #region Youdao Definitions

    public const string YoudaoUrl = "https://openapi.youdao.com/api";

    public const string YoudaoUploadUrl = "https://openapi.youdao.com/file_trans/upload";

    public const string YoudaoQueryUrl = "https://openapi.youdao.com/file_trans/query";

    public const string YoudaoDownloadUrl = "https://openapi.youdao.com/file_trans/download";


    public enum YoudaoLanguages
    {
        [Description("中文")] zh_CHS,
        [Description("中文繁体")] zh_CHT,
        [Description("英文")] en,
        [Description("日文")] ja,
        [Description("韩文")] ko,
        [Description("法文")] fr,
        [Description("西班牙文")] es,
        [Description("葡萄牙文")] pt,
        [Description("意大利文")] it,
        [Description("俄文")] ru,
        [Description("越南文")] vi,
        [Description("德文")] de,
        [Description("阿拉伯文")] ar,
        [Description("印尼文")] id,
        [Description("南非荷兰语")] af,
        [Description("波斯尼亚语")] bs,
        [Description("保加利亚语")] bg,
        [Description("粤语")] yue,
        [Description("加泰隆语")] ca,
        [Description("克罗地亚语")] hr,
        [Description("捷克语")] cs,
        [Description("丹麦语")] da,
        [Description("荷兰语")] nl,
        [Description("爱沙尼亚语")] et,
        [Description("斐济语")] fj,
        [Description("芬兰语")] fi,
        [Description("希腊语")] el,
        [Description("海地克里奥尔语")] ht,
        [Description("希伯来语")] he,
        [Description("印地语")] hi,
        [Description("白苗语")] mww,
        [Description("匈牙利语")] hu,
        [Description("斯瓦希里语")] sw,
        [Description("克林贡语")] tlh,
        [Description("拉脱维亚语")] lv,
        [Description("立陶宛语")] lt,
        [Description("马来语")] ms,
        [Description("马耳他语")] mt,
        [Description("挪威语")] no,
        [Description("波斯语")] fa,
        [Description("波兰语")] pl,
        [Description("克雷塔罗奥托米语")] otq,
        [Description("罗马尼亚语")] ro,
        [Description("塞尔维亚语(西里尔文)")] sr_Cyrl,
        [Description("塞尔维亚语(拉丁文)")] sr_Latn,
        [Description("斯洛伐克语")] sk,
        [Description("斯洛文尼亚语")] sl,
        [Description("瑞典语")] sv,
        [Description("塔希提语")] ty,
        [Description("泰语")] th,
        [Description("汤加语")] to,
        [Description("土耳其语")] tr,
        [Description("乌克兰语")] uk,
        [Description("乌尔都语")] ur,
        [Description("威尔士语")] cy,
        [Description("尤卡坦玛雅语")] yua,
        [Description("阿尔巴尼亚语")] sq,
        [Description("阿姆哈拉语")] am,
        [Description("亚美尼亚语")] hy,
        [Description("阿塞拜疆语")] az,
        [Description("孟加拉语")] bn,
        [Description("巴斯克语")] eu,
        [Description("白俄罗斯语")] be,
        [Description("宿务语")] ceb,
        [Description("科西嘉语")] co,
        [Description("世界语")] eo,
        [Description("菲律宾语")] tl,
        [Description("弗里西语")] fy,
        [Description("加利西亚语")] gl,
        [Description("格鲁吉亚语")] ka,
        [Description("古吉拉特语")] gu,
        [Description("豪萨语")] ha,
        [Description("夏威夷语")] haw,
        [Description("冰岛语")] isl,
        [Description("伊博语")] ig,
        [Description("爱尔兰语")] ga,
        [Description("爪哇语")] jw,
        [Description("卡纳达语")] kn,
        [Description("哈萨克语")] kk,
        [Description("高棉语")] km,
        [Description("库尔德语")] ku,
        [Description("柯尔克孜语")] ky,
        [Description("老挝语")] lo,
        [Description("拉丁语")] la,
        [Description("卢森堡语")] lb,
        [Description("马其顿语")] mk,
        [Description("马尔加什语")] mg,
        [Description("马拉雅拉姆语")] ml,
        [Description("毛利语")] mi,
        [Description("马拉地语")] mr,
        [Description("蒙古语")] mn,
        [Description("缅甸语")] my,
        [Description("尼泊尔语")] ne,
        [Description("齐切瓦语")] ny,
        [Description("普什图语")] ps,
        [Description("旁遮普语")] pa,
        [Description("萨摩亚语")] sm,
        [Description("苏格兰盖尔语")] gd,
        [Description("塞索托语")] st,
        [Description("修纳语")] sn,
        [Description("信德语")] sd,
        [Description("僧伽罗语")] si,
        [Description("索马里语")] so,
        [Description("巽他语")] su,
        [Description("塔吉克语")] tg,
        [Description("泰米尔语")] ta,
        [Description("泰卢固语")] te,
        [Description("乌兹别克语")] uz,
        [Description("南非科萨语")] xh,
        [Description("意第绪语")] yi,
        [Description("约鲁巴语")] yo,
        [Description("南非祖鲁语")] zu,
        [Description("自动识别")] auto,
    }

    public static class YoudaoLanguageCode
    {
        public const string zh_CHS = "zh-CHS"; //中文
        public const string zh_CHT = "zh-CHT"; //中文繁体
        public const string en = "en"; //英文
        public const string ja = "ja"; //日文
        public const string ko = "ko"; //韩文
        public const string fr = "fr"; //法文
        public const string es = "es"; //西班牙文
        public const string pt = "pt"; //葡萄牙文
        public const string it = "it"; //意大利文
        public const string ru = "ru"; //俄文
        public const string vi = "vi"; //越南文
        public const string de = "de"; //德文
        public const string ar = "ar"; //阿拉伯文
        public const string id = "id"; //印尼文
        public const string af = "af"; //南非荷兰语
        public const string bs = "bs"; //波斯尼亚语
        public const string bg = "bg"; //保加利亚语
        public const string yue = "yue"; //粤语
        public const string ca = "ca"; //加泰隆语
        public const string hr = "hr"; //克罗地亚语
        public const string cs = "cs"; //捷克语
        public const string da = "da"; //丹麦语
        public const string nl = "nl"; //荷兰语
        public const string et = "et"; //爱沙尼亚语
        public const string fj = "fj"; //斐济语
        public const string fi = "fi"; //芬兰语
        public const string el = "el"; //希腊语
        public const string ht = "ht"; //海地克里奥尔语
        public const string he = "he"; //希伯来语
        public const string hi = "hi"; //印地语
        public const string mww = "mww"; //白苗语
        public const string hu = "hu"; //匈牙利语
        public const string sw = "sw"; //斯瓦希里语
        public const string tlh = "tlh"; //克林贡语
        public const string lv = "lv"; //拉脱维亚语
        public const string lt = "lt"; //立陶宛语
        public const string ms = "ms"; //马来语
        public const string mt = "mt"; //马耳他语
        public const string no = "no"; //挪威语
        public const string fa = "fa"; //波斯语
        public const string pl = "pl"; //波兰语
        public const string otq = "otq"; //克雷塔罗奥托米语
        public const string ro = "ro"; //罗马尼亚语
        public const string sr_Cyrl = "sr_Cyrl"; //塞尔维亚语(西里尔文)
        public const string sr_Latn = "sr_Latn"; //塞尔维亚语(拉丁文)
        public const string sk = "sk"; //斯洛伐克语
        public const string sl = "sl"; //斯洛文尼亚语
        public const string sv = "sv"; //瑞典语
        public const string ty = "ty"; //塔希提语
        public const string th = "th"; //泰语
        public const string to = "to"; //汤加语
        public const string tr = "tr"; //土耳其语
        public const string uk = "uk"; //乌克兰语
        public const string ur = "ur"; //乌尔都语
        public const string cy = "cy"; //威尔士语
        public const string yua = "yua"; //尤卡坦玛雅语
        public const string sq = "sq"; //阿尔巴尼亚语
        public const string am = "am"; //阿姆哈拉语
        public const string hy = "hy"; //亚美尼亚语
        public const string az = "az"; //阿塞拜疆语
        public const string bn = "bn"; //孟加拉语
        public const string eu = "eu"; //巴斯克语
        public const string be = "be"; //白俄罗斯语
        public const string ceb = "ceb"; //宿务语
        public const string co = "co"; //科西嘉语
        public const string eo = "eo"; //世界语
        public const string tl = "tl"; //菲律宾语
        public const string fy = "fy"; //弗里西语
        public const string gl = "gl"; //加利西亚语
        public const string ka = "ka"; //格鲁吉亚语
        public const string gu = "gu"; //古吉拉特语
        public const string ha = "ha"; //豪萨语
        public const string haw = "haw"; //夏威夷语
        public const string isl = "is"; //冰岛语
        public const string ig = "ig"; //伊博语
        public const string ga = "ga"; //爱尔兰语
        public const string jw = "jw"; //爪哇语
        public const string kn = "kn"; //卡纳达语
        public const string kk = "kk"; //哈萨克语
        public const string km = "km"; //高棉语
        public const string ku = "ku"; //库尔德语
        public const string ky = "ky"; //柯尔克孜语
        public const string lo = "lo"; //老挝语
        public const string la = "la"; //拉丁语
        public const string lb = "lb"; //卢森堡语
        public const string mk = "mk"; //马其顿语
        public const string mg = "mg"; //马尔加什语
        public const string ml = "ml"; //马拉雅拉姆语
        public const string mi = "mi"; //毛利语
        public const string mr = "mr"; //马拉地语
        public const string mn = "mn"; //蒙古语
        public const string my = "my"; //缅甸语
        public const string ne = "ne"; //尼泊尔语
        public const string ny = "ny"; //齐切瓦语
        public const string ps = "ps"; //普什图语
        public const string pa = "pa"; //旁遮普语
        public const string sm = "sm"; //萨摩亚语
        public const string gd = "gd"; //苏格兰盖尔语
        public const string st = "st"; //塞索托语
        public const string sn = "sn"; //修纳语
        public const string sd = "sd"; //信德语
        public const string si = "si"; //僧伽罗语
        public const string so = "so"; //索马里语
        public const string su = "su"; //巽他语
        public const string tg = "tg"; //塔吉克语
        public const string ta = "ta"; //泰米尔语
        public const string te = "te"; //泰卢固语
        public const string uz = "uz"; //乌兹别克语
        public const string xh = "xh"; //南非科萨语
        public const string yi = "yi"; //意第绪语
        public const string yo = "yo"; //约鲁巴语
        public const string zu = "zu"; //南非祖鲁语
        public const string auto = "auto"; //自动识别
    }

    public enum YoudaoErrorCode
    {
        [Description("OK")] Ec0 = 0,
        [Description("未知API错误")] EcUnknown = UnknownEcIndex,

        [Description("缺少必填的参数，首先确保必填参数齐全，然后，确认参数书写是否正确。")]
        Ec101 = 101,
        [Description("不支持的语言类型")] Ec102 = 102,
        [Description("翻译文本过长")] Ec103 = 103,
        [Description("不支持的API类型")] Ec104 = 104,
        [Description("不支持的签名类型")] Ec105 = 105,
        [Description("不支持的响应类型")] Ec106 = 106,
        [Description("不支持的传输加密类型")] Ec107 = 107,

        [Description("应用ID无效，注册账号，登录后台创建应用和实例并完成绑定，可获得应用ID和应用密钥等信息")]
        Ec108 = 108,
        [Description("batchLog格式不正确")] Ec109 = 109,

        [Description("无相关服务的有效实例，应用没有绑定服务实例，可以新建服务实例，绑定服务实例。注：某些服务的翻译结果发音需要tts实例，需要在控制台创建语音合成实例绑定应用后方能使用。")]
        Ec110 = 110,
        [Description("开发者账号无效")] Ec111 = 111,
        [Description("请求服务无效")] Ec112 = 112,
        [Description("q不能为空")] Ec113 = 113,
        [Description("不支持的图片传输方式")] Ec114 = 114,

        [Description("解密失败，可能为DES,BASE64,URLDecode的错误")]
        Ec201 = 201,

        [Description("签名检验失败，如果确认应用ID和应用密钥的正确性，仍返回202，一般是编码问题。请确保翻译文本q为UTF-8编码.")]
        Ec202 = 202,
        [Description("访问IP地址不在可访问IP列表")] Ec203 = 203,

        [Description("请求的接口与应用的平台类型不一致，确保接入方式（AndroidSDK、IOSSDK、API）与创建的应用平台类型一致。如有疑问请参考入门指南")]
        Ec205 = 205,
        [Description("因为时间戳无效导致签名校验失败")] Ec206 = 206,
        [Description("重放请求")] Ec207 = 207,
        [Description("辞典查询失败")] Ec301 = 301,
        [Description("翻译查询失败")] Ec302 = 302,
        [Description("服务端的其它异常")] Ec303 = 303,
        [Description("会话闲置太久超时")] Ec304 = 304,
        [Description("账户已经欠费停")] Ec401 = 401,
        [Description("offlinesdk不可用")] Ec402 = 402,
        [Description("访问频率受限,请稍后访问")] Ec411 = 411,
        [Description("长请求过于频繁，请稍后访问")] Ec412 = 412,
        [Description("无效的OCR类型")] Ec1001 = 1001,
        [Description("不支持的OCRimage类型")] Ec1002 = 1002,
        [Description("不支持的OCRLanguage类型")] Ec1003 = 1003,
        [Description("识别图片过大")] Ec1004 = 1004,
        [Description("图片base64解密失败")] Ec1201 = 1201,
        [Description("OCR段落识别失败")] Ec1301 = 1301,
        [Description("访问频率受限")] Ec1411 = 1411,
        [Description("超过最大识别字节数")] Ec1412 = 1412,
        [Description("不支持的语言识别Language类型")] Ec2003 = 2003,
        [Description("合成字符过长")] Ec2004 = 2004,
        [Description("不支持的音频文件类型")] Ec2005 = 2005,
        [Description("不支持的发音类型")] Ec2006 = 2006,
        [Description("解密失败")] Ec2201 = 2201,
        [Description("服务的异常")] Ec2301 = 2301,
        [Description("访问频率受限,请稍后访问")] Ec2411 = 2411,
        [Description("超过最大请求字符数")] Ec2412 = 2412,
        [Description("不支持的语音格式")] Ec3001 = 3001,
        [Description("不支持的语音采样率")] Ec3002 = 3002,
        [Description("不支持的语音声道")] Ec3003 = 3003,
        [Description("不支持的语音上传类型")] Ec3004 = 3004,
        [Description("不支持的语言类型")] Ec3005 = 3005,
        [Description("不支持的识别类型")] Ec3006 = 3006,
        [Description("识别音频文件过大")] Ec3007 = 3007,
        [Description("识别音频时长过长")] Ec3008 = 3008,
        [Description("不支持的音频文件类型")] Ec3009 = 3009,
        [Description("不支持的发音类型")] Ec3010 = 3010,
        [Description("解密失败")] Ec3201 = 3201,
        [Description("语音识别失败")] Ec3301 = 3301,
        [Description("语音翻译失败")] Ec3302 = 3302,
        [Description("服务的异常")] Ec3303 = 3303,
        [Description("访问频率受限,请稍后访问")] Ec3411 = 3411,
        [Description("超过最大请求字符数")] Ec3412 = 3412,
        [Description("不支持的语音识别格式")] Ec4001 = 4001,
        [Description("不支持的语音识别采样率")] Ec4002 = 4002,
        [Description("不支持的语音识别声道")] Ec4003 = 4003,
        [Description("不支持的语音上传类型")] Ec4004 = 4004,
        [Description("不支持的语言类型")] Ec4005 = 4005,
        [Description("识别音频文件过大")] Ec4006 = 4006,
        [Description("识别音频时长过长")] Ec4007 = 4007,
        [Description("解密失败")] Ec4201 = 4201,
        [Description("语音识别失败")] Ec4301 = 4301,
        [Description("服务的异常")] Ec4303 = 4303,
        [Description("访问频率受限,请稍后访问")] Ec4411 = 4411,
        [Description("超过最大请求时长")] Ec4412 = 4412,
        [Description("无效的OCR类型")] Ec5001 = 5001,
        [Description("不支持的OCRimage类型")] Ec5002 = 5002,
        [Description("不支持的语言类型")] Ec5003 = 5003,
        [Description("识别图片过大")] Ec5004 = 5004,
        [Description("不支持的图片类型")] Ec5005 = 5005,
        [Description("文件为空")] Ec5006 = 5006,
        [Description("解密错误，图片base64解密失败")] Ec5201 = 5201,
        [Description("OCR段落识别失败")] Ec5301 = 5301,
        [Description("访问频率受限")] Ec5411 = 5411,
        [Description("超过最大识别流量")] Ec5412 = 5412,
        [Description("不支持的语音格式")] Ec9001 = 9001,
        [Description("不支持的语音采样率")] Ec9002 = 9002,
        [Description("不支持的语音声道")] Ec9003 = 9003,
        [Description("不支持的语音上传类型")] Ec9004 = 9004,
        [Description("不支持的语音识别Language类型")] Ec9005 = 9005,
        [Description("ASR识别失败")] Ec9301 = 9301,
        [Description("服务器内部错误")] Ec9303 = 9303,
        [Description("访问频率受限（超过最大调用次数）")] Ec9411 = 9411,
        [Description("超过最大处理语音长度")] Ec9412 = 9412,
        [Description("无效的OCR类型")] Ec10001 = 10001,
        [Description("不支持的OCRimage类型")] Ec10002 = 10002,
        [Description("识别图片过大")] Ec10004 = 10004,
        [Description("图片base64解密失败")] Ec10201 = 10201,
        [Description("OCR段落识别失败")] Ec10301 = 10301,
        [Description("访问频率受限")] Ec10411 = 10411,
        [Description("超过最大识别流量")] Ec10412 = 10412,
        [Description("不支持的语音识别格式")] Ec11001 = 11001,
        [Description("不支持的语音识别采样率")] Ec11002 = 11002,
        [Description("不支持的语音识别声道")] Ec11003 = 11003,
        [Description("不支持的语音上传类型")] Ec11004 = 11004,
        [Description("不支持的语言类型")] Ec11005 = 11005,
        [Description("识别音频文件过大")] Ec11006 = 11006,
        [Description("识别音频时长过长，最大支持30s")] Ec11007 = 11007,
        [Description("解密失败")] Ec11201 = 11201,
        [Description("语音识别失败")] Ec11301 = 11301,
        [Description("服务的异常")] Ec11303 = 11303,
        [Description("访问频率受限,请稍后访问")] Ec11411 = 11411,
        [Description("超过最大请求时长")] Ec11412 = 11412,
        [Description("图片尺寸过大")] Ec12001 = 12001,
        [Description("图片base64解密失败")] Ec12002 = 12002,
        [Description("引擎服务器返回错误")] Ec12003 = 12003,
        [Description("图片为空")] Ec12004 = 12004,
        [Description("不支持的识别图片类型")] Ec12005 = 12005,
        [Description("图片无匹配结果")] Ec12006 = 12006,
        [Description("不支持的角度类型")] Ec13001 = 13001,
        [Description("不支持的文件类型")] Ec13002 = 13002,
        [Description("表格识别图片过大")] Ec13003 = 13003,
        [Description("文件为空")] Ec13004 = 13004,
        [Description("表格识别失败")] Ec13301 = 13301,
        [Description("需要图片")] Ec15001 = 15001,
        [Description("图片过大（1M）")] Ec15002 = 15002,
        [Description("服务调用失败")] Ec15003 = 15003,
        [Description("需要图片")] Ec17001 = 17001,
        [Description("图片过大（1M）")] Ec17002 = 17002,
        [Description("识别类型未找到")] Ec17003 = 17003,
        [Description("不支持的识别类型")] Ec17004 = 17004,
        [Description("服务调用失败")] Ec17005 = 17005,
        [Description("需要参数")] Ec18001 = 18001,
        [Description("需要流水号")] Ec18002 = 18002,
        [Description("需要文件名")] Ec18003 = 18003,
        [Description("需要文件类型")] Ec18004 = 18004,
        [Description("需要源语言")] Ec18005 = 18005,
        [Description("需要目标语言")] Ec18006 = 18006,
        [Description("需要翻译文件")] Ec18007 = 18007,
        [Description("上传文件失败")] Ec18008 = 18008,
        [Description("错误的流水号")] Ec18009 = 18009,
        [Description("未完成")] Ec18010 = 18010,
        [Description("转换失败")] Ec18011 = 18011,
        [Description("找不到文件")] Ec18012 = 18012,
        [Description("需要文件下载类型")] Ec18013 = 18013,
        [Description("不支持的语言")] Ec18014 = 18014,
        [Description("不支持的文件类型")] Ec18015 = 18015,
        [Description("不支持的下载类型")] Ec18016 = 18016,
        [Description("文件过大")] Ec18017 = 18017
    }

    public enum YoudaoStatusCode
    {
        [Description("上传中")] Sc1 = 1,
        [Description("转换中")] Sc2 = 2,
        [Description("翻译中")] Sc3 = 3,
        [Description("已完成")] Sc4 = 4,
        [Description("生成中")] Sc5 = 5,
        [Description("上传失败")] Sc_1 = -1,
        [Description("转换失败")] Sc_2 = -2,
        [Description("翻译失败")] Sc_3 = -3,
        [Description("已取消")] Sc_4 = -4,
        [Description("生成失败")] Sc_5 = -5,
        [Description("翻译失败")] Sc_10 = -10,
        [Description("文件被删除")] Sc_11 = -11,
        [Description("未知状态")] ScUnknown = UnknownEcIndex
    }

    #endregion
}