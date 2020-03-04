using System.IO;
using System.Text;

namespace KPIT_K_Foundation
{
    internal class StartUp
    {
        private string _fullFileNamePath;
        private StartUpType _startUpType;
        private Tables _selectedTables;
        private string _nameSpace;
        private bool _isWebAPIStartUp;
        private bool _isUseLogging;
        private bool _isUseCaching;
        private bool _isUseAuditLogging;
        private bool _isEmailNotification;

        private StartUp()
        {
        }

        internal StartUp(string fullFileNamePath, string webAppName, string webAPIName, StartUpType startUpType, Tables selectedTables, bool isUseLogging, bool isUseCaching, bool isUseAuditLogging, bool isWebAPIStartUp, bool isEmailNotification)
        {
            this._fullFileNamePath = fullFileNamePath;
            this._startUpType = startUpType;
            this._selectedTables = selectedTables;
            this._isUseLogging = isUseLogging;
            this._isUseCaching = isUseCaching;
            this._isUseAuditLogging = isUseAuditLogging;
            this._isWebAPIStartUp = isWebAPIStartUp;
            this._isEmailNotification = isEmailNotification;
            this._nameSpace = this._startUpType != StartUpType.WebApp ? webAPIName : webAppName;
            this.GenerateCodeCS();
        }

        private void GenerateCodeCS()
        {
            using (StreamWriter streamWriter = new StreamWriter(this._fullFileNamePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using Microsoft.AspNetCore.Builder;");
                stringBuilder.AppendLine("using Microsoft.AspNetCore.Hosting;");
                stringBuilder.AppendLine("using Microsoft.Extensions.Configuration;");
                stringBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
                stringBuilder.AppendLine("using System.Data.SqlClient;");
                if (this._isUseLogging)
                    stringBuilder.AppendLine("using Application_Components.Logging;");
                if (this._isUseCaching)
                    stringBuilder.AppendLine("using Application_Components.Caching;");
                if (this._isUseAuditLogging)
                    stringBuilder.AppendLine("using Application_Components.AuditLog;");
                if (this._isEmailNotification)
                    stringBuilder.AppendLine("using Application_Components.EmailNotification;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace " + this._nameSpace);
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine("    public class Startup");
                stringBuilder.AppendLine("    {");
                stringBuilder.AppendLine("        public Startup(IConfiguration configuration)");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine("            Configuration = configuration;");
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("        public IConfiguration Configuration { get; }");
                if (this._isWebAPIStartUp)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("        readonly string apiAllowSpecificOrigins = \"_apiAllowSpecificOrigins\";");
                }
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("        // This method gets called by the runtime. Use this method to add services to the container.");
                stringBuilder.AppendLine("        public void ConfigureServices(IServiceCollection services)");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine("            services.AddMvc();");
                stringBuilder.AppendLine("            services.AddAntiforgery(o => o.HeaderName = \"XSRF - TOKEN\");");
                if (this._isUseLogging)
                    stringBuilder.AppendLine("            services.AddScoped<ILog, Log>();");
                stringBuilder.AppendLine("            services.AddSession(options =>");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                // Set a short timeout for easy testing.");
                stringBuilder.AppendLine("                options.IdleTimeout = TimeSpan.FromSeconds(30);");
                stringBuilder.AppendLine("                options.Cookie.HttpOnly = true;");
                stringBuilder.AppendLine("            });");
                stringBuilder.AppendLine("");
                if (this._isWebAPIStartUp)
                {
                    stringBuilder.AppendLine("            string WebClientURL = Configuration.GetSection(\"AppSettings\").GetSection(\"WebClientURL\").Value;");
                    stringBuilder.AppendLine("            services.AddCors(options =>");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine("                options.AddPolicy(apiAllowSpecificOrigins,");
                    stringBuilder.AppendLine("                builder =>");
                    stringBuilder.AppendLine("                {");
                    stringBuilder.AppendLine("                    builder.WithOrigins(WebClientURL)");
                    stringBuilder.AppendLine("                                       .AllowAnyHeader()");
                    stringBuilder.AppendLine("                                       .AllowAnyMethod();");
                    stringBuilder.AppendLine("                });");
                    stringBuilder.AppendLine("            });");
                    stringBuilder.AppendLine("");
                }
                if (this._isUseCaching)
                {
                    stringBuilder.AppendLine("            // Configure the redis cache server for caching.");
                    stringBuilder.AppendLine("            string Server = Configuration.GetSection(\"AppSettings\").GetSection(\"CacheServer\").GetSection(\"Server\").Value;");
                    stringBuilder.AppendLine("            string Instance = Configuration.GetSection(\"AppSettings\").GetSection(\"CacheServer\").GetSection(\"Instance\").Value;");
                    stringBuilder.AppendLine("            RedisCacheManager.ConfigureRedis(services, Server, Instance);");
                }
                if (this._isUseAuditLogging)
                {
                    stringBuilder.AppendLine("            // Configure the audit log for application event logging.");
                    stringBuilder.AppendLine("            string ConnectionString = Configuration.GetSection(\"AppSettings\").GetSection(\"ConnectionString\").Value;");                    
                    stringBuilder.AppendLine("            AuditLog.ConfigureAuditLog(services, ConnectionString);");
                }
                if (this._isEmailNotification)
                {
                    stringBuilder.Append("services.AddScoped<IEmail, EmailSettings>();");
                }
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.");
                stringBuilder.AppendLine("        public void Configure(IApplicationBuilder app, IHostingEnvironment env)");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine("            if (env.IsDevelopment())");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                app.UseDeveloperExceptionPage();");
                stringBuilder.AppendLine("                app.UseBrowserLink();");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine("            else");
                stringBuilder.AppendLine("            {");
                stringBuilder.AppendLine("                app.UseExceptionHandler(\"/Home/Error\");");
                stringBuilder.AppendLine("            }");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("            app.UseSession();");
                stringBuilder.AppendLine("            app.UseStaticFiles();");
                if (this._isWebAPIStartUp)
                {
                    stringBuilder.AppendLine("            app.UseCors(apiAllowSpecificOrigins);");
                    stringBuilder.AppendLine("            app.UseHttpsRedirection();");
                }
                stringBuilder.AppendLine("");
                if (this._startUpType == StartUpType.WebAPI)
                {
                    stringBuilder.AppendLine("            app.UseMvc(routes =>");
                    stringBuilder.AppendLine("            {");
                    foreach (Table selectedTable in this._selectedTables)
                    {
                        if (selectedTable.PrimaryKeyCount > 1 && !selectedTable.IsContainsPrimaryAndForeignKeyColumnsOnly)
                        {
                            string primaryKeysInCurlies = Functions.GetSlashDelimitedPrimaryKeysInCurlies(selectedTable);
                            stringBuilder.AppendLine("                routes.MapRoute(");
                            stringBuilder.AppendLine("                    \"" + selectedTable.Name + "\",");
                            stringBuilder.AppendLine("                    \"" + selectedTable.Name + "/{action}" + primaryKeysInCurlies + "\",");
                            stringBuilder.AppendLine("                    new { controller = \"" + selectedTable.Name + "\", action = \"Index\" }");
                            stringBuilder.AppendLine("                );");
                            stringBuilder.AppendLine("");
                        }
                    }
                    stringBuilder.AppendLine("                routes.MapRoute(");
                    stringBuilder.AppendLine("                    name: \"default\",");
                    stringBuilder.AppendLine("                    template: \"{controller=Home}/{action=Index}/{id?}\");");
                    stringBuilder.AppendLine("            });");
                }
                else
                {
                    stringBuilder.AppendLine("            app.UseMvc(routes =>");
                    stringBuilder.AppendLine("            {");
                    stringBuilder.AppendLine("                routes.MapRoute(");
                    stringBuilder.AppendLine("                    name: \"default\",");
                    stringBuilder.AppendLine("                    template: \"{controller=Home}/{action=Index}/{id?}\");");
                    stringBuilder.AppendLine("            });");
                }
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine("    }");
                stringBuilder.AppendLine("}");
                streamWriter.Write(stringBuilder.ToString());
            }
        }
    }
}
