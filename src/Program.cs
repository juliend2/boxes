using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Extensions.Azure;
using System.Text.Json;
using Boxes.Box;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<Database>(
    new Database(builder.Configuration.GetConnectionString("DefaultConnection")!)
);

var app = builder.Build();
var authProvider = new ActiveDirectoryAuthenticationProvider();
SqlAuthenticationProvider.SetProvider(SqlAuthenticationMethod.ActiveDirectoryDefault, authProvider);
app.Services.GetRequiredService<Database>().Init();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();

public class Database(string connStr)
{
    public void Init()
    {
        using var conn = new SqlConnection(connStr);
        conn.Open();
        conn.CreateCommand(
            @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='boxes' AND xtype='U')
              CREATE TABLE boxes (
                  id      INT PRIMARY KEY IDENTITY(1,1),
                  letter  NVARCHAR(1)   NOT NULL,
                  number  INT           NOT NULL,
                  content NVARCHAR(MAX) NOT NULL,
                  CONSTRAINT UQ_boxes_letter_number UNIQUE (letter, number)
              );"
        ).ExecuteNonQuery();
    }

    public int GetLastPossibleNumber()
    {
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"SELECT TOP 1 number
              FROM   boxes
              ORDER BY number DESC"
        );
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return reader.GetInt32(0);
        return 0;
    }

    public List<Box> GetAll()
    {
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"SELECT   id, letter, number, content
              FROM     boxes
              ORDER BY id DESC"
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
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"SELECT id, letter, number, content
              FROM   boxes
              WHERE  number = @num"
        );
        cmd.Parameters.AddWithValue("@num", searchId);
        var results = new List<Box>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            results.Add(new Box
            {
                Id = reader.GetInt32(0),
                Letter = reader.GetString(1)[0],
                Number = reader.GetInt32(2),
                Content = reader.GetString(3)
            });
        return results;
    }

    public Dictionary<int, Dictionary<char, string>> GetDict()
    {
        var lastNumber = GetLastPossibleNumber();
        var dict = new Dictionary<int, Dictionary<char, string>>();
        for (int i = 0; i < lastNumber + 1; i++)
        {
            var records = GetRecordsById(i);
            foreach (var box in records)
            {
                if (!dict.ContainsKey(i))
                    dict.Add(i, new Dictionary<char, string>());
                dict[i][(char)box.Letter] = box.Content;
            }
        }

        string json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);

        return dict;
    }

    public void Add(char letter, int number, string content)
    {
        using var conn = new SqlConnection(connStr);
        conn.Open();
        var cmd = conn.CreateCommand(
            @"MERGE boxes WITH (HOLDLOCK) AS target
              USING (VALUES (@letter, @number, @content)) AS src (letter, number, content)
                ON target.letter = src.letter AND target.number = src.number
              WHEN MATCHED THEN
                UPDATE SET content = src.content
              WHEN NOT MATCHED THEN
                INSERT (letter, number, content) VALUES (src.letter, src.number, src.content);"
        );
        cmd.Parameters.AddWithValue("@letter", letter.ToString());
        cmd.Parameters.AddWithValue("@number", number);
        cmd.Parameters.AddWithValue("@content", content);
        cmd.ExecuteNonQuery();
    }
}

public static class SqlConnectionExtensions
{
    public static SqlCommand CreateCommand(this SqlConnection conn, string sql)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        return cmd;
    }
}
