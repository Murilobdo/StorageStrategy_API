namespace StorageStrategy.PrintAgent.Printing;

/// <summary>
/// A impressora térmica precisa estar numa porta local (USB, COM do Bluetooth ou LPT). Porta de rede/nuvem
/// (ex.: "Cloud TCP/IP Port" do driver HPRT) manda o papel para fora da máquina e nada sai na impressora.
/// </summary>
public static class PortRules
{
    private static readonly string[] LocalPrefixes = { "USB", "COM", "LPT", "BTH", "TMUSB", "ESDPRT" };

    public static bool IsLocal(string port)
    {
        var p = port.Trim().ToUpperInvariant();
        return LocalPrefixes.Any(prefix => p.StartsWith(prefix, StringComparison.Ordinal));
    }

    public static bool IsCloudOrNetwork(string port)
    {
        var p = port.Trim().ToUpperInvariant();
        return p.Contains("CLOUD") || p.Contains("TCP") || p.StartsWith("IP_") || p.StartsWith("WSD") || p.StartsWith("HTTP");
    }
}
