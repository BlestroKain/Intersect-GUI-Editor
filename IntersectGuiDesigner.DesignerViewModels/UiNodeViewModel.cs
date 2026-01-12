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
    private double _boundsX;
    private double _boundsY;
    private double _boundsWidth;
    private double _boundsHeight;
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
        set
        {
            if (!SetProperty(ref _boundsText, value))
            {
                return;
            }

            if (!TrySetBounds(value, out _))
            {
                RefreshFromModel();
            }
        }
    }

    public string DockText
    {
        get => _dockText;
        set
        {
            if (!SetProperty(ref _dockText, value))
            {
                return;
            }

            if (!TrySetDock(value, out _))
            {
                RefreshFromModel();
            }
        }
    }

    public string PaddingText
    {
        get => _paddingText;
        set
        {
            if (!SetProperty(ref _paddingText, value))
            {
                return;
            }

            if (!TrySetPadding(value, out _))
            {
                RefreshFromModel();
            }
        }
    }

    public string MarginText
    {
        get => _marginText;
        set
        {
            if (!SetProperty(ref _marginText, value))
            {
                return;
            }

            if (!TrySetMargin(value, out _))
            {
                RefreshFromModel();
            }
        }
    }

    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (SetProperty(ref _hidden, value))
            {
                SetHidden(value);
            }
        }
    }

    public bool Disabled
    {
        get => _disabled;
        set
        {
            if (SetProperty(ref _disabled, value))
            {
                SetDisabled(value);
            }
        }
    }

    public string FontName
    {
        get => _fontName;
        set
        {
            if (SetProperty(ref _fontName, value))
            {
                SetFont(value, FontSize);
            }
        }
    }

    public double? FontSize
    {
        get => _fontSize;
        set
        {
            if (SetProperty(ref _fontSize, value))
            {
                SetFont(FontName, value);
            }
        }
    }

    public string TextAlign
    {
        get => _textAlign;
        set
        {
            if (SetProperty(ref _textAlign, value))
            {
                SetTextAlign(value);
            }
        }
    }

    public string TextColor
    {
        get => _textColor;
        set
        {
            if (SetProperty(ref _textColor, value))
            {
                SetTextColor(value);
            }
        }
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

    public double BoundsX
    {
        get => _boundsX;
        private set => SetProperty(ref _boundsX, value);
    }

    public double BoundsY
    {
        get => _boundsY;
        private set => SetProperty(ref _boundsY, value);
    }

    public double BoundsWidth
    {
        get => _boundsWidth;
        private set => SetProperty(ref _boundsWidth, value);
    }

    public double BoundsHeight
    {
        get => _boundsHeight;
        private set => SetProperty(ref _boundsHeight, value);
    }

    public void RefreshFromModel()
    {
        SetProperty(ref _name, Model.Name, nameof(Name));

        HasBounds = HasProperty("Bounds");
        SetProperty(ref _boundsText, HasBounds ? GetRawString("Bounds") : string.Empty, nameof(BoundsText));

        HasDock = HasProperty("Dock");
        SetProperty(ref _dockText, HasDock ? Model.GetDock()?.ToString() ?? string.Empty : string.Empty, nameof(DockText));

        HasPadding = HasProperty("Padding");
        SetProperty(ref _paddingText, HasPadding ? GetRawString("Padding") : string.Empty, nameof(PaddingText));

        HasMargin = HasProperty("Margin");
        SetProperty(ref _marginText, HasMargin ? GetRawString("Margin") : string.Empty, nameof(MarginText));

        HasHidden = HasProperty("Hidden");
        SetProperty(ref _hidden, HasHidden && Model.GetHidden().GetValueOrDefault(false), nameof(Hidden));

        HasDisabled = HasProperty("Disabled");
        SetProperty(ref _disabled, HasDisabled && Model.GetDisabled().GetValueOrDefault(false), nameof(Disabled));

        HasFont = HasProperty("Font");
        SetProperty(ref _fontName, HasFont ? Model.GetFontName() ?? string.Empty : string.Empty, nameof(FontName));
        SetProperty(ref _fontSize, HasFont ? Model.GetFontSize() : null, nameof(FontSize));

        HasTextAlign = HasProperty("TextAlign");
        SetProperty(ref _textAlign, HasTextAlign ? Model.GetTextAlign() ?? string.Empty : string.Empty, nameof(TextAlign));

        HasTextColor = HasProperty("TextColor");
        SetProperty(ref _textColor, HasTextColor ? Model.GetTextColor() ?? string.Empty : string.Empty, nameof(TextColor));

        var bounds = Model.GetBounds();
        SetProperty(ref _boundsX, bounds?.X ?? 0, nameof(BoundsX));
        SetProperty(ref _boundsY, bounds?.Y ?? 0, nameof(BoundsY));
        SetProperty(ref _boundsWidth, bounds?.Width ?? 0, nameof(BoundsWidth));
        SetProperty(ref _boundsHeight, bounds?.Height ?? 0, nameof(BoundsHeight));
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
