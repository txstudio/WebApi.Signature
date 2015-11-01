namespace WebApi.Signature
{
    /// <summary>簽章結果</summary>
    public class SignatureResult
    {
        /// <summary>驗證成功</summary>
        public bool IsValid { get; set; }
        /// <summary>異常訊息</summary>
        public string Message { get; set; }

        /// <summary>應用程式金鑰</summary>
        public string a { get; set; }
        /// <summary>簽章</summary>
        public string s { get; set; }
        /// <summary>時間戳記</summary>
        public long t { get; set; }
        /// <summary>請求內容</summary>
        public string RequestBody { get; set; }
    }
}
