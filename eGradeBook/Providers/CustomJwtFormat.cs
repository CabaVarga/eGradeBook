using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;

namespace eGradeBook.Providers
{
    /// <summary>
    /// Custom Json Web Token formatter
    /// </summary>
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private static readonly byte[] _secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");
        private readonly string _issuer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="issuer"></param>
        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        /// <summary>
        /// Protect the jwt with hmac sha256 signature
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // look here: https://stackoverflow.com/a/48980707/4486196
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(_secret);
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                        securityKey, SecurityAlgorithms.HmacSha256Signature);
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(_issuer, "Any", data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials));
        }

        /// <summary>
        /// Unprotect
        /// </summary>
        /// <param name="protectedText"></param>
        /// <returns></returns>
        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}