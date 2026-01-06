namespace QuestionBank.Components.Services;

using System.Timers;

public class QuizTimer(int totalMinutes, Action onTick, Action onComplete) : IDisposable
{
    private Timer? _timer;
    private int _elapsedSeconds;

    public string DisplayTime { get; private set; } = "00:00:00";

    public void Start()
    {
        _timer = new Timer(1000);
        var totalSeconds = totalMinutes * 60;

        _timer.Elapsed += (_, _) =>
        {
            if (_elapsedSeconds < totalMinutes * 60)
            {
                _elapsedSeconds++;
                totalSeconds--;
                DisplayTime = FormatTime(totalSeconds);
                onTick();
            }
            else
            {
                Stop();
                onComplete();
            }
        };

        _timer.Start();
    }

    public void Stop()
    {
        _timer?.Stop();
    }

    private string FormatTime(int totalSeconds)
    {
        var hours = totalSeconds / 3600;
        var minutes = (totalSeconds % 3600) / 60;
        var seconds = totalSeconds % 60;
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
