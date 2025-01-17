namespace Domain.Utilities;

public static class FormatDateTime
{
    public static string ToViewAbleDateTime(DateTime time)
    {
        return time.ToString("HH:mm MMM dd");
    } 
}