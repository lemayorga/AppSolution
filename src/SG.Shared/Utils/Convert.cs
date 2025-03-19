namespace SG.Shared.Utils;

public static class ConvertUtil
{
    public static (int, int, int, TimeSpan) SecondTo(int totalSeconds)
    {
        int hours = totalSeconds  / 3600;
        int minutes = (totalSeconds  % 3600) / 60;
        int seconds = (totalSeconds  % 60);
        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(totalSeconds));

        return (hours,minutes, seconds, t);
    }

    public  static int MinutesToSeconds(int minutos)
    {
        return minutos * 60;
    }
}
