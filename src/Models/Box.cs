namespace Boxes.Box;

public class Box
{
    public int Id { get; set; }
    public char Letter { get; set; }
    public int Number { get; set; }
    public string Content { get; set; } = "";

    public string GetContent()
    {
        return Content;
    }
}
