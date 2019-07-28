using System;
using System.Data.Entity;
using System.Web.Http;
using eGradeBook.Infrastructure;
using eGradeBook.Models;
using eGradeBook.Providers;
using eGradeBook.Repositories;
using eGradeBook.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using NLog;
using Owin;
using Unity;
using Unity.Lifetime;
using Unity.NLog;
using Unity.WebApi;


[assembly: OwinStartup(typeof(eGradeBook.Startup))]

namespace eGradeBook
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = SetupUnity();
            ConfigureOAuth(app, container);

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new UnityDependencyResolver(container);
            WebApiConfig.Register(config);


            // trebace i cors...
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app, UnityContainer container)
        {
            string issuer = "http://localhost/";
            byte[] secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomAuthorizationServerProvider(container),
                AccessTokenFormat = new CustomJwtFormat(issuer)
            };

            JwtBearerAuthenticationOptions jwtBearerOptions = new JwtBearerAuthenticationOptions()
            {
                AllowedAudiences = new[] { "Any" },
                AuthenticationMode = AuthenticationMode.Active,
                IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                {
                    new SymmetricKeyIssuerSecurityKeyProvider(issuer, secret)
                }
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseJwtBearerAuthentication(jwtBearerOptions);
        }

        private UnityContainer SetupUnity()
        {
            var container = new UnityContainer().EnableDiagnostic();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.AddNewExtension<NLogExtension>();


            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<DbContext, GradeBookContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            // --- Users
            container.RegisterType<IGenericRepository<GradeBookUser>, GenericRepository<GradeBookUser>>();
            container.RegisterType<IGenericRepository<TeacherUser>, GenericRepository<TeacherUser>>();
            container.RegisterType<IGenericRepository<StudentUser>, GenericRepository<StudentUser>>();
            container.RegisterType<IGenericRepository<ParentUser>, GenericRepository<ParentUser>>();

            // --- Basic entities
            container.RegisterType<IGenericRepository<Course>, GenericRepository<Course>>();
            container.RegisterType<IGenericRepository<SchoolClass>, GenericRepository<SchoolClass>>();

            // --- Associations
            container.RegisterType<IGenericRepository<Teaching>, GenericRepository<Teaching>>();
            container.RegisterType<IGenericRepository<Program>, GenericRepository<Program>>();
            container.RegisterType<IGenericRepository<Taking>, GenericRepository<Taking>>();
            container.RegisterType<IGenericRepository<StudentParent>, GenericRepository<StudentParent>>();

            // --- Grading
            container.RegisterType<IGenericRepository<Grade>, GenericRepository<Grade>>();
            container.RegisterType<IGenericRepository<FinalGrade>, GenericRepository<FinalGrade>>();
            container.RegisterType<IAuthRepository, AuthRepository>();


            // --- Services
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<IStudentsService, StudentsService>();
            container.RegisterType<ITeachersService, TeachersService>();
            container.RegisterType<IParentsService, ParentsService>();

            container.RegisterType<ICoursesService, CoursesService>();


            container.RegisterType<ITeachingsService, TeachingsService>();
            container.RegisterType<IProgramsService, ProgramsService>();

            container.RegisterType<IGradesService, GradesService>();
            container.RegisterType<IFinalGradesService, FinalGradesService>();

            return container;
        }
    }
}
