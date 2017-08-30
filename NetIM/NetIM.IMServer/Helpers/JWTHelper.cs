using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using NetIM.IMServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetIM.IMServer
{
    public static class JWTHelper
    {
        private static readonly string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1Xz09uFiwDVvVbvk";

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string Encode(object payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, secret);
        }

        public static string Encode(IDictionary<string, object> payload)
        {
            return Encode((object)payload);
        }

        /// <summary>
        /// 解码，可能会抛出TokenExpiredException、SignatureVerificationException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Decode<T>(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            var obj = decoder.DecodeToObject<T>(token, secret, verify: true);
            return obj;
        }

        /// <summary>
        /// 解码，可能会抛出TokenExpiredException、SignatureVerificationException异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IDictionary<string, object> Decode(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            var obj = decoder.DecodeToObject(token, secret, verify: true);
            return obj;
        }

        public static string GetToken(HttpContextBase ctx)
        {
            var cookieToken = ctx.Request.Cookies["Token"];
            if (cookieToken == null)
            {
                return null;
            }
            else
            {
                return cookieToken.Value;
            }
        }

        public static LoginUserInfo GetUserInfoFromToken(string token)
        {
            try
            {
                IDictionary<string, object> payload = Decode(token);
                if (payload.ContainsKey("UserId"))
                {
                    LoginUserInfo userInfo = new LoginUserInfo();
                    userInfo.UserId = (long)payload["UserId"];
                    userInfo.UserNickName = (string)payload["UserNickName"];
                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }
        }

        public static LoginUserInfo GetUserInfo(HttpContextBase ctx)
        {
            string token = GetToken(ctx);
            return GetUserInfoFromToken(token);
        }
    }
}