using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Signature.WebApp
{
    public class ApiSignatureAttribute : AuthorizeAttribute
    {
        private SignatureServiceBase _signatureService;
        private SignatureResult _signatureResult;

        public ApiSignatureAttribute()
        {
            //實作 SignatureServiceBase 抽象類別
            this._signatureService = new SignatureService();
        }


        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            NameValueCollection _collection;
            string _query;
            string _body;

            _query = actionContext.Request.RequestUri.Query;
            _collection = SignatureHelper.ParseQueryString(_query);
            _body = actionContext.Request.Content.ReadAsStringAsync().Result;

            this._signatureResult = _signatureService.Validation(_collection["a"]
                , _collection["s"]
                , _collection["t"]
                , _body);

            return (this._signatureResult.IsValid);
        }
        

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            //如果驗證失敗的話傳回驗證物件提供偵錯
            if (this._signatureResult.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.OK,
                    this._signatureResult
                );
            }
        }
    }

    //測試用的 SignatureServiceBase 實作類別
    public class SignatureService : SignatureServiceBase
    {
        protected override string GetSaltKey(string apikey)
        {
            return ("signature");
        }
    }
}