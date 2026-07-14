using System.Text;

namespace Smart_Ring.Behaviors;

public class PhoneFormattingBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.TextChanged += OnEntryTextChanged;
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.TextChanged -= OnEntryTextChanged;
    }

    private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry) return;

        // Evitar bucle infinito al modificar el texto desde el código
        entry.TextChanged -= OnEntryTextChanged;

        string oldText = e.OldTextValue ?? string.Empty;
        string newText = e.NewTextValue ?? string.Empty;

        // Si el usuario está borrando y termina en un guion, facilitamos el borrado del número anterior
        if (oldText.Length > newText.Length && oldText.EndsWith("-"))
        {
            newText = newText.TrimEnd('-');
        }

        // Remover todo lo que no sea un dígito numérico
        string digitsOnly = FilterOnlyDigits(newText);

        // Limitar a un máximo de 10 dígitos (ajusta si requieres más)
        if (digitsOnly.Length > 10)
        {
            digitsOnly = digitsOnly.Substring(0, 10);
        }

        // Aplicar el formato XX-XXXX-XXXX
        string formatted = ApplyPhoneFormat(digitsOnly);

        // Actualizar el valor del Entry y restaurar la posición del cursor al final
        entry.Text = formatted;
        entry.CursorPosition = formatted.Length;

        entry.TextChanged += OnEntryTextChanged;
    }

    private string FilterOnlyDigits(string text)
    {
        var sb = new StringBuilder();
        foreach (char c in text)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    private string ApplyPhoneFormat(string digits)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < digits.Length; i++)
        {
            // Insertar guion después del 2do dígito
            if (i == 2)
            {
                sb.Append('-');
            }
            // Insertar guion después del 6to dígito (2 del primer bloque + 4 del segundo)
            else if (i == 6)
            {
                sb.Append('-');
            }

            sb.Append(digits[i]);
        }

        return sb.ToString();
    }
}