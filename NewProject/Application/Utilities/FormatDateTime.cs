namespace Application.Utilities;

public static class FormatDateTime
{
    public static string HH_mm_MMM_dd(DateTime time)
    {
        return time.ToString("HH:mm MMM dd");
    } 
}