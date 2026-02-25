using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace _4Layer.Api.Extensions;

public static class SwaggerSetup
{
	public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(options =>
		{
			//options.EnableAnnotations();
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "4Layer API",
				Version = "v1",
				Description = "Clean Architecture API built with ASP.NET Core",
				Contact = new OpenApiContact
				{
					Name = "Nguyễn Thành Nhật",
					Email = "doanandroig@gmail.com"
				}
			});

			// XML comment
			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
			if (File.Exists(xmlPath))
			{
				options.IncludeXmlComments(xmlPath);
			}

			// JWT Bearer
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "Enter 'Bearer {token}'"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});

		return services;
	}

	public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "4Layer API v1");
				options.RoutePrefix = "docs"; // localhost:5000/docs
			});
		}

		return app;
	}
}