using System.Net.Http;
using System.Text.Json;
using Hangfire;

namespace Jobs;
public class UpdateConnectionStringJob
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DynamicConnectionStringProvider _connectionStringProvider;

    public UpdateConnectionStringJob(IHttpClientFactory httpClientFactory, DynamicConnectionStringProvider connectionStringProvider)
    {
        _httpClientFactory = httpClientFactory;
        _connectionStringProvider = connectionStringProvider;
    }

    public async Task Execute()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("http://nghiaho.duckdns.org:9200/nghiadeptrai");
            var data = JsonSerializer.Deserialize<JsonElement>(response);
            var newIp = data.GetProperty("ip").GetString();

            if (!string.IsNullOrEmpty(newIp))
            {
                // Kiểm tra kết nối với IP mới trước khi cập nhật
                var tempConnectionString = _connectionStringProvider.GetConnectionString().Replace(_connectionStringProvider.GetConnectionString().Split(';').FirstOrDefault(x => x.StartsWith("Server=")), $"Server={newIp},14332");
                using var connection = new SqlConnection(tempConnectionString);
                try
                {
                    await connection.OpenAsync();
                    _connectionStringProvider.UpdateIp(newIp);
                    Console.WriteLine($"Successfully updated IP to {newIp} after validation.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect with new IP {newIp}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateConnectionStringJob: {ex.Message}");
        }
    }
}