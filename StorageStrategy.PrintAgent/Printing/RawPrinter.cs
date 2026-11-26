using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace StorageStrategy.PrintAgent.Printing;

public interface IRawPrinter
{
    /// <summary>Problema conhecido da impressora (offline, sem papel...) ou null se parece pronta.</summary>
    string? GetProblem(string printerName);

    /// <summary>Envia os bytes ESC/POS como RAW para o spooler do Windows.</summary>
    void Print(string printerName, byte[] payload, string documentName);
}

/// <summary>Spooler do Windows (winspool.drv) com datatype RAW: o ESC/POS chega intacto na impressora (research R9).</summary>
public class RawPrinter : IRawPrinter
{
    public string? GetProblem(string printerName)
    {
        if (!OperatingSystem.IsWindows())
            return "O agente de impressão só funciona no Windows";

        return WindowsSpooler.GetProblem(printerName);
    }

    public void Print(string printerName, byte[] payload, string documentName)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("O agente de impressão só funciona no Windows");

        WindowsSpooler.WriteRaw(printerName, payload, documentName);
    }
}

[SupportedOSPlatform("windows")]
internal static class WindowsSpooler
{
    // PRINTER_INFO_2.Status
    private const uint PRINTER_STATUS_ERROR = 0x00000002;
    private const uint PRINTER_STATUS_PAPER_JAM = 0x00000008;
    private const uint PRINTER_STATUS_PAPER_OUT = 0x00000010;
    private const uint PRINTER_STATUS_OFFLINE = 0x00000080;
    private const uint PRINTER_STATUS_NOT_AVAILABLE = 0x00001000;
    private const uint PRINTER_STATUS_DOOR_OPEN = 0x00400000;
    // PRINTER_INFO_2.Attributes
    private const uint PRINTER_ATTRIBUTE_WORK_OFFLINE = 0x00000400;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private class DOCINFO
    {
        public string? pDocName;
        public string? pOutputFile;
        public string? pDataType;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct PRINTER_INFO_2
    {
        public IntPtr pServerName, pPrinterName, pShareName, pPortName, pDriverName, pComment, pLocation,
            pDevMode, pSepFile, pPrintProcessor, pDatatype, pParameters, pSecurityDescriptor;
        public uint Attributes, Priority, DefaultPriority, StartTime, UntilTime, Status, cJobs, AveragePPM;
    }

    [DllImport("winspool.drv", EntryPoint = "OpenPrinterW", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", EntryPoint = "StartDocPrinterW", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int StartDocPrinter(IntPtr hPrinter, int level, [In] DOCINFO di);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool WritePrinter(IntPtr hPrinter, byte[] pBytes, int dwCount, out int dwWritten);

    [DllImport("winspool.drv", EntryPoint = "GetPrinterW", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool GetPrinter(IntPtr hPrinter, int level, IntPtr pPrinter, int cbBuf, out int pcbNeeded);

    public static string? GetProblem(string printerName)
    {
        if (!OpenPrinter(printerName, out var handle, IntPtr.Zero))
            return $"Impressora \"{printerName}\" não encontrada no Windows";

        try
        {
            GetPrinter(handle, 2, IntPtr.Zero, 0, out var needed);
            if (needed <= 0)
                return null; // sem informação de status: tenta imprimir mesmo assim

            var buffer = Marshal.AllocHGlobal(needed);
            try
            {
                if (!GetPrinter(handle, 2, buffer, needed, out _))
                    return null;

                var info = Marshal.PtrToStructure<PRINTER_INFO_2>(buffer);
                var port = Marshal.PtrToStringUni(info.pPortName) ?? string.Empty;
                if (PortRules.IsCloudOrNetwork(port))
                    return $"Impressora configurada na porta \"{port}\" (rede/nuvem). Troque para a porta USB ou COM no Windows";
                if ((info.Attributes & PRINTER_ATTRIBUTE_WORK_OFFLINE) != 0 || (info.Status & PRINTER_STATUS_OFFLINE) != 0)
                    return "Impressora offline";
                if ((info.Status & PRINTER_STATUS_PAPER_OUT) != 0)
                    return "Sem papel";
                if ((info.Status & PRINTER_STATUS_PAPER_JAM) != 0)
                    return "Papel enroscado";
                if ((info.Status & PRINTER_STATUS_DOOR_OPEN) != 0)
                    return "Tampa da impressora aberta";
                if ((info.Status & PRINTER_STATUS_NOT_AVAILABLE) != 0)
                    return "Impressora indisponível";
                if ((info.Status & PRINTER_STATUS_ERROR) != 0)
                    return "Impressora com erro";
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        finally
        {
            ClosePrinter(handle);
        }
    }

    public static void WriteRaw(string printerName, byte[] payload, string documentName)
    {
        if (!OpenPrinter(printerName, out var handle, IntPtr.Zero))
            throw new IOException($"Impressora \"{printerName}\" não encontrada no Windows", new Win32Exception(Marshal.GetLastWin32Error()));

        try
        {
            var doc = new DOCINFO { pDocName = documentName, pDataType = "RAW" };
            if (StartDocPrinter(handle, 1, doc) == 0)
                throw new IOException("Falha ao iniciar a impressão", new Win32Exception(Marshal.GetLastWin32Error()));

            try
            {
                if (!StartPagePrinter(handle))
                    throw new IOException("Falha ao iniciar a página", new Win32Exception(Marshal.GetLastWin32Error()));

                try
                {
                    if (!WritePrinter(handle, payload, payload.Length, out var written) || written != payload.Length)
                        throw new IOException("Falha ao enviar os dados para a impressora", new Win32Exception(Marshal.GetLastWin32Error()));
                }
                finally
                {
                    EndPagePrinter(handle);
                }
            }
            finally
            {
                EndDocPrinter(handle);
            }
        }
        finally
        {
            ClosePrinter(handle);
        }
    }
}
