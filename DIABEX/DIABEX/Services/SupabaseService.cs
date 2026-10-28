using Supabase;

namespace DIABEX.Services;

public static class SupabaseService
{
    private const string Url = "https://sghmsxckrzujmwxaihig.supabase.co";
    private const string Key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InNnaG1zeGNrcnp1am13eGFpaGlnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3ODgyOTA3ODMsImV4cCI6MjEwMzg2Njc4M30.WJmZVsSo4oEHtNZWLJkOMc3iT0RysR7eCKRPqZueG5I";

    private static Client _client;

    public static Client Instance => _client;

    public static async Task InitializeAsync()
    {
        if (_client != null) return;

        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };

        _client = new Client(Url, Key, options);
        await _client.InitializeAsync();
    }
}