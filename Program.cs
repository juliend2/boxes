using Microsoft.Data.Sqlite;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<Database>();

var app = builder.Build();

app.Services.GetRequiredService<Database>().Init();

app.MapRazorPages();
app.Run();

public class Box
{
    public int Id { get; set; }
    public char Letter { get; set; }
    public int Number { get; set; }
    public string Content { get; set; } = "";

    public string GetContent() {
      return Content;
    }
}

public class Database
{
    private const string ConnStr = "Data Source=boxes.db";

    public void Init()
    {
        using var conn = new SqliteConnection(ConnStr);
        conn.Open();
        conn.CreateCommand(
            "CREATE TABLE IF NOT EXISTS boxes (id INTEGER PRIMARY KEY AUTOINCREMENT, letter TEXT NOT NULL, number INTEGER NOT NULL, content TEXT NOT NULL)"
        ).ExecuteNonQuery();
    }

    public int GetLastPossibleNumber()
    {
        using var conn = new SqliteConnection(ConnStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"SELECT    number
              FROM      boxes
              ORDER BY  number DESC
              LIMIT     1 "
            );
        using var reader = cmd.ExecuteReader();
        var list = new List<Box>();
        if (reader.Read()) {
          return reader.GetInt32(0);
        }
        return 0;
    }

    public List<Box> GetAll()
    {
        using var conn = new SqliteConnection(ConnStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"SELECT    id, letter, number, content
              FROM      boxes
              ORDER BY  id DESC"
            );
        using var reader = cmd.ExecuteReader();
        var list = new List<Box>();
        while (reader.Read())
            list.Add(new Box
            {
                Id = reader.GetInt32(0),
                Letter = reader.GetString(1)[0],
                Number = reader.GetInt32(2),
                Content = reader.GetString(3)
            });
        return list;
    }

    public List<Box> GetRecordsById(int searchId)
    {
        using var conn = new SqliteConnection(ConnStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"SELECT id, letter, number, content
              FROM   boxes
              WHERE  number = @num "
            );
        cmd.Parameters.AddWithValue("@num", searchId);
        var results = new List<Box>();

        // 4. Exécuter et lire les données
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add(new Box
            {
              Id = reader.GetInt32(0),
              Letter = reader.GetString(1)[0],
              Number = reader.GetInt32(2),
              Content = reader.GetString(3)
            });
        }

        return results;
    }

    public Dictionary<int, Dictionary<char, string>> GetDict()
    {
      var all = GetAll();
      var lastNumber = GetLastPossibleNumber();
      var dict = new Dictionary<int, Dictionary<char, string>>();
      for (int i = 0; i < lastNumber+1; i++)
      {
        var records = GetRecordsById(i);
        foreach (var box in records)
        {
            var d = new Dictionary<char, string>();
            if (!dict.TryGetValue(i, out d)) {
                dict.Add(i, new Dictionary<char, string>());
            }
            // var SubDict = new Dictionary<char, string>();
            var str = "";
            if (!dict[i].TryGetValue((char)box.Letter, out str)) {
                dict[i] = new Dictionary<char, string>();
                // dict[i].Add((char)box.Letter, box.Content);
            }
            dict[i][(char)box.Letter] = box.Content;
        }
      }

        string json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);
      
      return dict;
    }

    public void Add(char letter, int number, string content)
    {
        using var conn = new SqliteConnection(ConnStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"INSERT INTO boxes (letter, number, content)
              VALUES            ($l, $n, $c)"
              );
        cmd.Parameters.AddWithValue("$l", letter.ToString());
        cmd.Parameters.AddWithValue("$n", number);
        cmd.Parameters.AddWithValue("$c", content);
        cmd.ExecuteNonQuery();
    }
}

public static class SqliteConnectionExtensions
{
    public static SqliteCommand CreateCommand(this SqliteConnection conn, string sql)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        return cmd;
    }
}
