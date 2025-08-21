using Helper.Method;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicer;
public class DynamicConnectionStringProvider
{
    private readonly IConfiguration _configuration;
    private string _currentIp;
    private readonly object _lock = new object();

    public DynamicConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _currentIp = "113.172.185.28";
    }

    public void UpdateIp(string newIp)
    {
        if (!string.IsNullOrEmpty(newIp))
        {
            lock (_lock)
            {
                _currentIp = newIp; // Cập nhật IP thread-safe
            }
            Console.WriteLine($"IP updated to: {newIp}");
        }
    }

    public string GetConnectionString()
    {
        var baseConnectionString = General.DecryptString(_configuration.GetConnectionString("DB_Inventory"));
        var parts = baseConnectionString.Split(';');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].Trim().StartsWith("Server=", StringComparison.OrdinalIgnoreCase))
            {
                var serverParts = parts[i].Split('=');
                if (serverParts.Length > 1)
                {
                    var addressParts = serverParts[1].Split(',');
                    var port = addressParts.Length > 1 ? addressParts[1] : "14332";
                    parts[i] = $"Server={_currentIp},{port}";
                }
            }
        }
        return string.Join(";", parts);
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = new SqlConnection(GetConnectionString());
            await connection.OpenAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection test failed: {ex.Message}");
            return false;
        }
    }
}
