using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagePassword.Model.AppSide
{
	public static class ConfigManager
	{
		private static readonly string config_file = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "appsettings.json");

		public static string GetSelecetedProvider()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			IConfigurationRoot config = builder
				.AddJsonFile(config_file)
				.Build();
			string value = config["DatabaseProvider"];

			return value ?? "SQLite";
		}

		public static void SetSelectedProvider(string provider)
		{
			string json = File.ReadAllText(config_file);
			JsonNode root = JsonNode.Parse(json);
			root["DatabaseProvider"] = provider;

			string updatedJson = root.ToJsonString(new System.Text.Json.JsonSerializerOptions
			{
				WriteIndented = true,
			});

			File.WriteAllText(config_file, updatedJson);
		}
		public static string GetConnectionString(string key)
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			IConfigurationRoot config = builder.AddJsonFile(config_file).Build();
			return config.GetSection("ConnectionStrings")[key];
		}
	}
}
