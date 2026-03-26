using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Boxes.Box;

public class IndexModel(Database db) : PageModel
{
    private readonly Database _db = db;

    public List<Box> Boxes { get; private set; } = new();
    public Dictionary<int, Dictionary<char, string>> Dict { get; private set; } = new();
    public string Error { get; private set; } = "";
    public string FormLetter { get; private set; } = "";
    public string FormNumber { get; private set; } = "";
    public string FormContent { get; private set; } = "";

    public char[] Letters()
    {
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    }

    public void OnGet()
    {
        Boxes = _db.GetAll();
        Dict = _db.GetDict();
    }

    public IActionResult OnPost(string letter, string number, string content)
    {
        FormLetter = letter;
        FormNumber = number;
        FormContent = content;

        if (string.IsNullOrWhiteSpace(letter) || letter.Length != 1 || !char.IsLetter(letter[0]))
        {
            Error = "Letter must be a single letter.";
            Boxes = _db.GetAll();
            Dict = _db.GetDict();
            return Page();
        }

        if (!int.TryParse(number, out var num))
        {
            Error = "Number must be an integer.";
            Boxes = _db.GetAll();
            Dict = _db.GetDict();
            return Page();
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            Error = "Content cannot be empty.";
            Boxes = _db.GetAll();
            Dict = _db.GetDict();
            return Page();
        }

        _db.Add(letter[0], num, content);
        return RedirectToPage();
    }
}
