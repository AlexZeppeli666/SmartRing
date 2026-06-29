namespace Smart_Ring.Componentes;

public partial class TopBar : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(TopBar),
        string.Empty,
        propertyChanged: OnTitleChanged);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public event EventHandler? BackTapped;

    public TopBar()
    {
        InitializeComponent();

        // AGREGADO: Evaluamos la ruta en cuanto el componente se dibuje en pantalla
        this.Loaded += EvaluateBackButtonVisibility;
    }

    private void EvaluateBackButtonVisibility(object? sender, EventArgs e)
    {
        if (Shell.Current?.CurrentState?.Location == null) return;

        string currentRoute = Shell.Current.CurrentState.Location.ToString();

        // Si la ruta contiene el Perfil o el accesorio Wearable, ocultamos el botón de regresar
        if (currentRoute.Contains("ProfilePage") || currentRoute.Contains("WearableAccessoryPage"))
        {
            BackButton.IsVisible = false;
        }
        else
        {
            BackButton.IsVisible = true;
        }
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TopBar topBar)
        {
            topBar.LabelTitle.Text = (string)newValue;
        }
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        if (BackTapped != null)
        {
            BackTapped.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}