namespace WebApi.Signature
{
    /// <summary>
    /// 簽章製作與驗證的基底類別
    /// </summary>
    public abstract class SignatureServiceBase
    {

        /// <summary>
        /// 驗證簽章結果
        /// </summary>
        /// <param name="apikey">金鑰</param>
        /// <param name="signature">簽章</param>
        /// <param name="timestamp">時間戳記</param>
        /// <param name="requestBody">請求內容</param>
        /// <param name="errorSecond">時間戳記的誤差允許秒數</param>
        /// <returns>驗證結果</returns>
        public SignatureResult Validation(string apikey
            , string signature
            , string timestamp
            , string requestBody
            , int errorSecond = 300)
        {
            string _serverSaltKey = string.Empty;
            string _serverSignature = string.Empty;
            string _serverRequestBody = string.Empty;
            string _serverHash = string.Empty;
            string _serverHashBody = string.Empty;

            long _serverTimestamp = 0;
            long _clientTimestamp = 0;

            SignatureResult _message;

            _message = new SignatureResult();
            _message.IsValid = (false);

            //如果 apikey , signature , timestamp 不存在回傳錯誤訊息
            if (string.IsNullOrWhiteSpace(apikey) == true
                && string.IsNullOrWhiteSpace(signature) == true
                && string.IsNullOrWhiteSpace(timestamp) == true)
            {
                _message.Message = "apikey , signature , timestamp is required";

                return _message;
            }

            if (string.IsNullOrWhiteSpace(apikey) == true)
            {
                _message.Message = "apikey is required";
                return _message;
            }
            if (string.IsNullOrWhiteSpace(signature) == true)
            {
                _message.Message = "signature is required";
                return _message;
            }
            if (string.IsNullOrWhiteSpace(timestamp) == true)
            {
                _message.Message = "timestamp is required";
                return _message;
            }


            if (long.TryParse(timestamp, out _clientTimestamp) == false)
            {
                _message.Message = "timestamp is not valid integer";
                return _message;
            }


            _serverTimestamp = SignatureHelper.GetUnixTimeSeconds();

            //驗證時間戳記是否在誤差數值範圍
            if ((_serverTimestamp - errorSecond) <= _clientTimestamp
                 && _clientTimestamp <= (_serverTimestamp + errorSecond))
            {
            }
            else
            {
                _message.Message = "timestamp not valid";
                _message.t = _serverTimestamp;

                return _message;
            }

            //取得簽章加鹽字串
            _serverSaltKey = this.GetSaltKey(apikey);

            if (string.IsNullOrWhiteSpace(_serverSaltKey) == true)
            {
                _message.Message = "apikey not valid";

                return _message;
            }

            //取得簽章
            _serverRequestBody = requestBody;
            _serverHashBody = string.Format("{0}{1}{2}"
                , _serverRequestBody
                , _clientTimestamp
                , _serverSaltKey);

            _serverHash = SignatureHelper.MD5ToHex(_serverHashBody);


            //比對簽章結果
            if (_serverHash == signature)
            {
                //結果正確
            }
            else
            {
                //結果不正確，並提供伺服器端用來驗證的內容
                _message.Message = "signature not valid";
                _message.RequestBody = requestBody;
                _message.s = _serverHash;
                _message.t = _clientTimestamp;
                _message.a = apikey;

                return _message;
            }

            _message.IsValid = (true);

            return _message;
        }


        /// <summary>
        /// 取得指定加鹽用金鑰
        /// </summary>
        /// <param name="apikey">要查詢的應用程式金鑰</param>
        /// <returns>加鹽金鑰</returns>
        protected abstract string GetSaltKey(string apikey);
    }
}
