using System.Text.RegularExpressions;

namespace Smart_Ring.Behaviors;

public class TextOnlyBehavior : Behavior<Entry>
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

        string newText = e.NewTextValue ?? string.Empty;

        // Si está vacío, no evaluamos nada
        if (string.IsNullOrEmpty(newText)) return;

        // Desasociamos temporalmente el evento para evitar bucles al limpiar el texto
        entry.TextChanged -= OnEntryTextChanged;

        // Expresión regular que permite letras (incluyendo acentos y ñ) y espacios.
        // Bloquea números, emojis y caracteres especiales.
        string cleanText = Regex.Replace(newText, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]", "");

        // Si el texto limpio es diferente al que ingresó el usuario, lo reemplazamos
        if (newText != cleanText)
        {
            entry.Text = cleanText;
            // Mantenemos el cursor al final del texto válido
            entry.CursorPosition = cleanText.Length;
        }

        entry.TextChanged += OnEntryTextChanged;
    }
}