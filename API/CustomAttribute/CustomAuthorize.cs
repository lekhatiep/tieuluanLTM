using API.Data;
using API.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace API.CustomAttribute
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly EnumsList.Role[] listRoles;
        private readonly EnumsList.Role role;
        public CustomAuthorize(EnumsList.Role[] roles)
        {
            listRoles = roles;
        }

        public CustomAuthorize(EnumsList.Role role)
        {
            this.role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userID = int.Parse(identity.FindFirst("id").Value);


                var listRegRole = dbContext.RegistrationRole.Where(x => x.UserID == userID).ToList();

                if (listRegRole.Count() > 0)
                {
                    if (listRoles != null)
                    {
                        foreach (var role in listRoles)
                        {
                            if (!listRegRole.Any(x => x.RoleID == (int)role))
                            {
                                context.Result = new JsonResult(new
                                {
                                    Message = "Request Access Denied"
                                })
                                {
                                    StatusCode = StatusCodes.Status401Unauthorized
                                };
                            }

                        }
                    }
                    //---

                    if (!listRegRole.Any(x => x.RoleID == (int)role))
                    {
                        context.Result = new JsonResult(new
                        {
                            Message = "Request Access Denied"
                        })
                        {
                            StatusCode = StatusCodes.Status401Unauthorized
                        };
                    }

                }


            }
                
        }

    }
}
