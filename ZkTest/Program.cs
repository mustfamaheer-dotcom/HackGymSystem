using System.Reflection;
using Microsoft.Data.Sqlite;

Type? t = Type.GetTypeFromCLSID(new Guid("00853A19-BD51-419B-9269-2DABE57EB61F"));
dynamic dev = Activator.CreateInstance(t);

bool ok = dev.Connect_Net("192.168.1.201", 4370);
Console.WriteLine($"Connect: {ok}");
dev.EnableDevice(1, false);
dev.RefreshData(1);
Thread.Sleep(500);

string serial = "";
dev.GetSerialNumber(1, ref serial);
Console.WriteLine($"Serial: {serial}");

string dbPath = @"D:\Hack gym system\GymDb.db";
Console.WriteLine($"\nReading from {dbPath}...");
var members = new List<(string code, string name)>();
using (var conn = new SqliteConnection($"Data Source={dbPath}"))
{
    conn.Open();
    var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT Code, FullName FROM Members WHERE IsDeleted = 0 ORDER BY Code";
    using var reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        string code = reader.GetInt32(0).ToString();
        string name = reader.IsDBNull(1) ? "" : reader.GetString(1);
        members.Add((code, name));
    }
}
Console.WriteLine($"Members: {members.Count}");

Console.WriteLine("\nPushing...");
int okCount = 0, failCount = 0;
var sw = System.Diagnostics.Stopwatch.StartNew();

foreach (var m in members)
{
    try
    {
        bool result = dev.SSR_SetUserInfo(1, m.code, m.name.Length > 24 ? m.name[..24] : m.name, "", 0, true);
        if (result) okCount++;
        else failCount++;
    }
    catch { failCount++; }

    if ((okCount + failCount) % 500 == 0)
        Console.WriteLine($"  {okCount + failCount}/{members.Count} (OK={okCount} Fail={failCount})");
}

sw.Stop();
Console.WriteLine($"\nDone: {okCount} OK, {failCount} Failed in {sw.Elapsed.TotalSeconds:F1}s");

dev.EnableDevice(1, true);
dev.Disconnect();
Console.WriteLine("Done.");
