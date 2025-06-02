
using DQ3rAPI.Options;
using Serilog;

namespace DQ3rAPI;

public class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Configuration.AddJsonFile("datamap.json").Build();

		builder.Services.Configure<RomFileOptions>(
			builder.Configuration.GetSection(RomFileOptions.RomFile));

		// Add support to logging with SERILOG
		builder.Host.UseSerilog((context, configuration) =>
			configuration.ReadFrom.Configuration(context.Configuration));

		builder.Services.AddControllers();

		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment()) {
			app.MapOpenApi();
		}

		// Add support to logging request with SERILOG
		app.UseSerilogRequestLogging();

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}

