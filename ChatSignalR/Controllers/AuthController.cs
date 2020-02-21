using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using ChatSignalR.Hubs;
using ChatSignalR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatSignalR.Controllers
{
   
    public class AuthController : Controller
    {


        private Context con;
        private IMapper map;
        private IHubContext<Chat> hub;
        public AuthController(Context _con, IMapper _map, IHubContext<Chat> _hub)
        {
            hub = _hub;
            con = _con;
            map = _map;
        }



        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckLogin(string Login)
        {
            User user = con.Users.FirstOrDefault(x => x.Login == Login);
            if (user == null) return Json(true);
            return Json(false);
        }


        [HttpPost("/account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Erros = ModelState
                });
            }


            User userCheck = await con.Users.FirstOrDefaultAsync(x => x.Login == model.Login);
            if (userCheck != null)
            {
                ModelState.AddModelError("LoginExist", "Такой логин уже существует");
                return BadRequest(ModelState);
            }
            List<Claim> Claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, model.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),
            };

            string refreshToken = Jwt.GenerateRefreshToken();
            JwtSecurityToken jwt = Jwt.GenerateToken(Claims);
            string jwtEnCoded = new JwtSecurityTokenHandler().WriteToken(jwt);

            User user = new User()
            {
                Login = model.Login,
                Password = model.Password,
                IsOnLine = false,
                RefreshToken = refreshToken
            };
            con.Users.Add(user);
            await con.SaveChangesAsync();
            var usermapper = map.Map<User, UserMapper>(user);
            await hub.Clients.All.SendAsync("NewUser", usermapper);
            return Ok(new
            {
                Jwt= jwtEnCoded,
                RefreshToken= refreshToken,

            });

        }


        [Authorize]
        [HttpGet("/get/profile")]
        public IActionResult profile()
        {
            return Ok(new { Login = HttpContext.User.Identity.Name });
        }



        [HttpPost("account/getToken")]
        public IActionResult getToken(string Login, string Password)
        {
            ClaimsIdentity identity = GetClaims(Login, Password);
            if (identity == null) return BadRequest();
            var jwtEnCoded = new JwtSecurityTokenHandler().WriteToken(Jwt.GenerateToken(identity.Claims));
            return Ok(new
            {
                jwt = jwtEnCoded,
                login = identity.Name,
                refresh= Jwt.SaveRefreshToken(identity.Name, con)
            });        
        }


        [HttpPost("/test/token")]
        public IActionResult refreshToken([FromBody]JwtModel model)
        {
            ClaimsPrincipal principal = Jwt.GetClaimsWithToken(model.accesToken);
            var UserRefreshToken = Jwt.getRefreshTokenInBd(principal.Identity.Name, con);

            if (UserRefreshToken != model.refreshToken)
            {
                HttpContext.Response.Headers.Add("refreshSave", UserRefreshToken);
                HttpContext.Response.Headers.Add("refresh", model.refreshToken);
                return BadRequest();
            }

            string EnCodedJwt = new JwtSecurityTokenHandler().WriteToken(Jwt.GenerateToken(principal.Claims));

            string NewRefreshToken = Jwt.SaveRefreshToken(principal.Identity.Name, con);

            return Ok(new
            {
                jwt = EnCodedJwt,
                Refresh = NewRefreshToken
            });

            
        }



        




   
        public ClaimsIdentity GetClaims(string Login, string Password)
        {
            User user = con.Users.FirstOrDefault(x => x.Login == Login && x.Password == Password);
            if (user == null) return null;


            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),
            };

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        }

    }


    public class Jwt
    {
        public static JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.Now;
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: JwtOptions.ISSUER,
                audience: JwtOptions.AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return jwt;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string str = Convert.ToBase64String(randomNumber);
                return str.Replace("+", "A").Replace("/", "B").Replace("=", "C");
            }
        }

        public static string SaveRefreshToken(string Login, Context con)
        {
            User user = con.Users.FirstOrDefault(x => x.Login == Login);

            user.RefreshToken = GenerateRefreshToken();
            con.Users.Update(user);
            con.SaveChanges();
            return user.RefreshToken;
        }


        public static ClaimsPrincipal GetClaimsWithToken(string token)
        {
            if (token == null) return null;

            var param = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey()
            };

            SecurityToken security;

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, param, out security);

            JwtSecurityToken newJwtToken = security as JwtSecurityToken;

            if (newJwtToken == null || !newJwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;

        }

        public static string getRefreshTokenInBd(string Login, Context con)
        {
            User user = con.Users.FirstOrDefault(x => x.Login == Login);
            return user.RefreshToken;
        }

    }
}
