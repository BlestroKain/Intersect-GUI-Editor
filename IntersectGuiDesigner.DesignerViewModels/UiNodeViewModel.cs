using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using IntersectGuiDesigner.Core;
using Newtonsoft.Json.Linq;

namespace IntersectGuiDesigner.DesignerViewModels;

public sealed class UiNodeViewModel : ViewModelBase
{
    private string _name;
    private string _boundsText = string.Empty;
    private string _dockText = string.Empty;
    private string _paddingText = string.Empty;
    private string _marginText = string.Empty;
    private bool _hidden;
    private bool _disabled;
    private string _fontName = string.Empty;
    private double? _fontSize;
    private string _textAlign = string.Empty;
    private string _textColor = string.Empty;
    private bool _hasBounds;
    private bool _hasDock;
    private bool _hasPadding;
    private bool _hasMargin;
    private bool _hasHidden;
    private bool _hasDisabled;
    private bool _hasFont;
    private bool _hasTextAlign;
    private bool _hasTextColor;

    public UiNodeViewModel(UiNode model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        _name = Model.Name;
        Children = new ObservableCollection<UiNodeViewModel>(
            Model.Children.Select(child => new UiNodeViewModel(child)));
        RefreshFromModel();
    }

    public UiNode Model { get; }

    public ObservableCollection<UiNodeViewModel> Children { get; }

    public string Name
    {
        get => _name;
        private set => SetProperty(ref _name, value);
    }

    public string BoundsText
    {
        get => _boundsText;
        private set => SetProperty(ref _boundsText, value);
    }

    public string DockText
    {
        get => _dockText;
        private set => SetProperty(ref _dockText, value);
    }

    public string PaddingText
    {
        get => _paddingText;
        private set => SetProperty(ref _paddingText, value);
    }

    public string MarginText
    {
        get => _marginText;
        private set => SetProperty(ref _marginText, value);
    }

    public bool Hidden
    {
        get => _hidden;
        private set => SetProperty(ref _hidden, value);
    }

    public bool Disabled
    {
        get => _disabled;
        private set => SetProperty(ref _disabled, value);
    }

    public string FontName
    {
        get => _fontName;
        private set => SetProperty(ref _fontName, value);
    }

    public double? FontSize
    {
        get => _fontSize;
        private set => SetProperty(ref _fontSize, value);
    }

    public string TextAlign
    {
        get => _textAlign;
        private set => SetProperty(ref _textAlign, value);
    }

    public string TextColor
    {
        get => _textColor;
        private set => SetProperty(ref _textColor, value);
    }

    public bool HasBounds
    {
        get => _hasBounds;
        private set => SetProperty(ref _hasBounds, value);
    }

    public bool HasDock
    {
        get => _hasDock;
        private set => SetProperty(ref _hasDock, value);
    }

    public bool HasPadding
    {
        get => _hasPadding;
        private set => SetProperty(ref _hasPadding, value);
    }

    public bool HasMargin
    {
        get => _hasMargin;
        private set => SetProperty(ref _hasMargin, value);
    }

    public bool HasHidden
    {
        get => _hasHidden;
        private set => SetProperty(ref _hasHidden, value);
    }

    public bool HasDisabled
    {
        get => _hasDisabled;
        private set => SetProperty(ref _hasDisabled, value);
    }

    public bool HasFont
    {
        get => _hasFont;
        private set => SetProperty(ref _hasFont, value);
    }

    public bool HasTextAlign
    {
        get => _hasTextAlign;
        private set => SetProperty(ref _hasTextAlign, value);
    }

    public bool HasTextColor
    {
        get => _hasTextColor;
        private set => SetProperty(ref _hasTextColor, value);
    }

    public void RefreshFromModel()
    {
        Name = Model.Name;
        HasBounds = HasProperty("Bounds");
        BoundsText = HasBounds ? GetRawString("Bounds") : string.Empty;

        HasDock = HasProperty("Dock");
        DockText = HasDock ? Model.GetDock()?.ToString() ?? string.Empty : string.Empty;

        HasPadding = HasProperty("Padding");
        PaddingText = HasPadding ? GetRawString("Padding") : string.Empty;

        HasMargin = HasProperty("Margin");
        MarginText = HasMargin ? GetRawString("Margin") : string.Empty;

        HasHidden = HasProperty("Hidden");
        Hidden = HasHidden && Model.GetHidden().GetValueOrDefault(false);

        HasDisabled = HasProperty("Disabled");
        Disabled = HasDisabled && Model.GetDisabled().GetValueOrDefault(false);

        HasFont = HasProperty("Font");
        FontName = HasFont ? Model.GetFontName() ?? string.Empty : string.Empty;
        FontSize = HasFont ? Model.GetFontSize() : null;

        HasTextAlign = HasProperty("TextAlign");
        TextAlign = HasTextAlign ? Model.GetTextAlign() ?? string.Empty : string.Empty;

        HasTextColor = HasProperty("TextColor");
        TextColor = HasTextColor ? Model.GetTextColor() ?? string.Empty : string.Empty;
    }

    public bool TrySetBounds(string? value, out string? error)
    {
        error = null;
        try
        {
            var bounds = GuiValueParser.ParseBounds(value ?? string.Empty);
            Model.SetBounds(bounds);
            RefreshFromModel();
            return true;
        }
        catch (FormatException ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public bool TrySetDock(string? value, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        if (Enum.TryParse(value, true, out DockStyle parsed))
        {
            Model.SetDock(parsed);
            RefreshFromModel();
            return true;
        }

        error = $"Invalid Dock value: '{value}'.";
        return false;
    }

    public bool TrySetPadding(string? value, out string? error)
    {
        error = null;
        try
        {
            var padding = GuiValueParser.ParsePadding(value ?? string.Empty);
            Model.SetPadding(padding);
            RefreshFromModel();
            return true;
        }
        catch (FormatException ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public bool TrySetMargin(string? value, out string? error)
    {
        error = null;
        try
        {
            var margin = GuiValueParser.ParsePadding(value ?? string.Empty, "margin");
            Model.SetMargin(margin);
            RefreshFromModel();
            return true;
        }
        catch (FormatException ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public void SetHidden(bool isHidden)
    {
        Model.SetHidden(isHidden);
        RefreshFromModel();
    }

    public void SetDisabled(bool isDisabled)
    {
        Model.SetDisabled(isDisabled);
        RefreshFromModel();
    }

    public void SetFont(string? fontName, double? fontSize)
    {
        Model.SetFont(fontName ?? string.Empty, fontSize);
        RefreshFromModel();
    }

    public void SetTextAlign(string? value)
    {
        Model.SetTextAlign(value ?? string.Empty);
        RefreshFromModel();
    }

    public void SetTextColor(string? value)
    {
        Model.SetTextColor(value ?? string.Empty);
        RefreshFromModel();
    }

    public UiNodeViewModel? FindByModel(UiNode? target)
    {
        if (target is null)
        {
            return null;
        }

        if (ReferenceEquals(Model, target))
        {
            return this;
        }

        foreach (var child in Children)
        {
            var match = child.FindByModel(target);
            if (match is not null)
            {
                return match;
            }
        }

        return null;
    }

    private bool HasProperty(string propertyName)
    {
        JToken? token = Model.Raw[propertyName];
        return token is not null && token.Type != JTokenType.Null;
    }

    private string GetRawString(string propertyName)
    {
        JToken? token = Model.Raw[propertyName];
        if (token is null || token.Type == JTokenType.Null)
        {
            return string.Empty;
        }

        return token.Value<string>() ?? string.Empty;
    }
}
