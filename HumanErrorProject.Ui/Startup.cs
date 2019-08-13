using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.DataAccess.Repositories;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine;
using HumanErrorProject.Engine.Analysis;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using HumanErrorProject.Engine.Utilities.Filter;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.ModelBinders;
using HumanErrorProject.Ui.Options;
using HumanErrorProject.Ui.Services;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HumanErrorProject.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 8;
            });

            services.AddDbContext<HumanErrorProjectContext>(options =>
                
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(s =>
                s.UseSqlServerStorage(Configuration.GetConnectionString("Hangfire")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<HumanErrorProjectContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IHangFireJobService, HangFireJobService>();

            AddRepositoryServices(services);
            AddEngineServices(services);

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
            });

            services.AddTransient<GenericEntityBinderFactory<SurveyQuestion>, SurveyQuestionEntityFactory>();
            services.AddTransient<GenericEntityBinderFactory<SurveyAnswer>, SurveyAnswerEntityFactory>();
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new GenericBinderProvider<SurveyQuestion>());
                options.ModelBinderProviders.Insert(0, new GenericBinderProvider<SurveyAnswer>());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }


        public void AddRepositoryServices(IServiceCollection services)
        {
            services.Configure<ViewOptions>(Configuration.GetSection("ViewOptions"));
            services.Configure<LoggerOptions>(Configuration.GetSection("LoggerOptions"));

            services.AddTransient<IRepository<Student, string>, StudentRepository>();
            services.AddTransient<IRepository<PreAssignment, int>, PreAssignmentRepository>();
            services.AddTransient<IRepository<CourseClass, int>, Repository<CourseClass, int>>();
            services.AddTransient<IRepository<Assignment, int>, AssignmentRepository>();
            services.AddTransient<IRepository<Survey,string>, Repository<Survey, string>>();
            services.AddTransient<IRepository<SnapshotSubmission, int>, Repository<SnapshotSubmission, int>>();
            services.AddTransient<IRepository<Snapshot, int>, Repository<Snapshot, int>>();
            services.AddTransient<IRepository<SnapshotReport, int>, Repository<SnapshotReport, int>>();
            services.AddTransient<IRepository<MarkovModel, int>, Repository<MarkovModel, int>>();
            services.AddTransient<DeleteHelper>();
            services.AddTransient<ColorHelper>();
        }

        public void AddEngineServices(IServiceCollection services)
        {
            services.Configure<EngineOptions>(Configuration.GetSection("EngineOptions"));
            services.Configure<ClangOptions>(Configuration.GetSection("ClangOptions"));
            services.Configure<EngineRunnerOptions>(Configuration.GetSection("EngineRunnerOptions"));
            services.Configure<PowershellOptions>(Configuration.GetSection("PowershellOptions"));
            services.Configure<SendGridOptions>(Configuration.GetSection("SendGridOptions"));

            services.AddTransient<IAbstractSyntaxTreeMetricCreator, AbstractSyntaxTreeMetricCreator>();
            services.AddTransient<IBagOfWordsMetricCreator, BagOfWordsMetricCreator>();
            services.AddTransient<IAssignmentGenerator, AssignmentGenerator>();
            services.AddTransient<IAbstractSyntaxTreeGenerator, ClangAbstractSyntaxTreeGenerator>();
            services.AddTransient<IUnitTestGenerator, PowershellUnitTestGenerator>();
            services.AddTransient<ISnapshotGenerator, SnapshotGenerator>();
            services.AddTransient<ISnapshotMethodGenerator, SnapshotMethodGenerator>();
            services.AddTransient<ISnapshotReportGenerator, SnapshotReportGenerator>();
            services.AddTransient<ILineFilter, ClangLineFilter>();
            services.AddTransient<IAbstractSyntaxTreeClassExtractor, ClangAbstractSyntaxTreeClassExtractor>();
            services.AddTransient<IAbstractSyntaxTreeExtractor, ClangAbstractSyntaxTreeExtractor>();
            services.AddTransient<IAbstractSyntaxTreeMethodExtractor, ClangAbstractSyntaxTreeMethodExtractor>();
            services.AddTransient<ILineSplitter, ClangLineSplitter>();
            services.AddTransient<ISnapshotDateConverter, SnapshotDateConverter>();
            services.AddTransient<IEngine, Engine.Engine>();
            services.AddTransient<IEngineRunner, EngineRunner>();
            services.AddTransient<IEmailService, SendGridEmailService>();
            services.AddSingleton<IEngineLogger, EngineLogger>();
            services.AddTransient<IEngineService, EngineService>();
            services.AddTransient<IMarkovModelCreator, MarkovModelCreator>();
            services.AddTransient<IMarkovModelGenerator, MarkovModelGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            HumanErrorProjectContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions());

            app.UseHangfireDashboard();
            app.UseMvc();

            Seed(userManager, roleManager, context).Wait();
        }

        public async Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            HumanErrorProjectContext context)
        {
            if (await roleManager.FindByNameAsync(IdentityRoleConstants.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(IdentityRoleConstants.Admin));
            }

            if (await roleManager.FindByNameAsync(IdentityRoleConstants.Student) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(IdentityRoleConstants.Student));
            }

            var cobe = "cobe.greene@oit.edu";
            if (await userManager.FindByNameAsync(cobe) == null)
            {
                var user = new IdentityUser()
                {
                    UserName = cobe,
                    Email = cobe,
                };

                var result = await userManager.CreateAsync(user, "Human3rr0rPr0j3ct");
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, IdentityRoleConstants.Admin));
                    await userManager.AddToRoleAsync(user, IdentityRoleConstants.Admin);
                }
            }

            var lucas = "lucas.cordova@oit.edu";
            if (await userManager.FindByNameAsync(lucas) == null)
            {
                var user = new IdentityUser()
                {
                    UserName = lucas,
                    Email = lucas
                };

                var result = await userManager.CreateAsync(user, "Human3rr0rPr0j3ct");
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, IdentityRoleConstants.Admin));
                    await userManager.AddToRoleAsync(user, IdentityRoleConstants.Admin);
                }
            }
        }
    }
}
