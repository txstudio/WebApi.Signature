using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(WebApi.Signature.WebApp.Startup))]

namespace WebApi.Signature.WebApp
{
    // OWIN 啟動類別
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration _config;

            _config = new HttpConfiguration();

            //啟用路由屬性設定
            _config.MapHttpAttributeRoutes();

            //移除 Xml 序列化設定
            _config.Formatters.Remove(_config.Formatters.XmlFormatter);

            //移除 FormUrlEncoded 設定：就是 key=value&key2=value2 那種
            _config.Formatters.Remove(_config.Formatters.FormUrlEncodedFormatter);

            //設定 Json 不序列化 Null 屬性
            _config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling
                = NullValueHandling.Ignore;

            app.UseWebApi(_config);
        }
    }
}
