using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections;

namespace Okta_Test.Account
{
    public class CheckClaimValue
    {
        public string GetClaimValue(string key1, string token1)
        {
            //Assume the input is in a control called txtJwtIn,
            //and the output will be placed in a control called txtJwtOut
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtInput = token1;

            //Check if readable token (string is in a JWT format)
            var readableToken = jwtHandler.CanReadToken(jwtInput);
            if (readableToken == true)
            {
                var token = jwtHandler.ReadJwtToken(jwtInput);

                //Extract the headers of the JWT
                //var headers = token.Header;
                //var jwtHeader = "{";
                //foreach (var h in headers)
                //{
                //    jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
                //}
                //jwtHeader += "}";

                //Extract the payload of the JWT
                var claims = token.Claims;
                //var jwtPayload = "{";
                foreach (Claim c in claims)
                {
                    if (c.ToString().Contains(key1))
                    {
                        return c.Value;
                    }
                    //jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
                }
                //jwtPayload += "}";
                return "";
            }
            return "";
        }

        public ArrayList GetAll(string token1)
        {
            //Assume the input is in a control called txtJwtIn,
            //and the output will be placed in a control called txtJwtOut
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtInput = token1;
            ArrayList myAL = new ArrayList(); ;

            //Check if readable token (string is in a JWT format)
            var readableToken = jwtHandler.CanReadToken(jwtInput);
            if (readableToken == true)
            {
                var token = jwtHandler.ReadJwtToken(jwtInput);

                //Extract the headers of the JWT
                //var headers = token.Header;
                //var jwtHeader = "{";
                //foreach (var h in headers)
                //{
                //    jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
                //}
                //jwtHeader += "}";

                //Extract the payload of the JWT
                var claims = token.Claims;
                //var jwtPayload = "{";
                foreach (Claim c in claims)
                {

                    myAL.Add(c.Type + ":" + c.Value);
                    
                    //jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
                }
                //jwtPayload += "}";
                return myAL;
            }
            return myAL;
        }
    }
}