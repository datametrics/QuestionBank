using Microsoft.Extensions.Options;

namespace QuestionBank.Components.Model;

public class FormulaItem
{
    public string Topic { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Formula { get; set; } = string.Empty;
}
public class FormulaStore
{
    public List<FormulaItem> Formulas { get; private set; }
    private readonly IDisposable? _subscription;
    public FormulaStore(IOptionsMonitor<List<FormulaItem>> monitor)
    {
        Formulas = monitor.CurrentValue;
        _subscription ??= monitor.OnChange(newValue =>
        {
            Formulas = newValue;
            //NotifyStateChanged();
        });
    }
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
}