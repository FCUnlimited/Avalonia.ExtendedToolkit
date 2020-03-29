﻿using System;
using Avalonia.Controlz.Controls;
using Avalonia.Media;

namespace Avalonia.ExtendedToolkit.Controls
{
    /// <summary>
    /// GroupBox with CharacterCasing
    /// </summary>
    public class MetroGroupBox : GroupBox
    {
        public Type StyleKey => typeof(MetroGroupBox);

        public CharacterCasing ContentCharacterCasing
        {
            get { return (CharacterCasing)GetValue(ContentCharacterCasingProperty); }
            set { SetValue(ContentCharacterCasingProperty, value); }
        }

        public static readonly StyledProperty<CharacterCasing> ContentCharacterCasingProperty =
            AvaloniaProperty.Register<MetroGroupBox, CharacterCasing>(nameof(ContentCharacterCasing));

        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public static readonly StyledProperty<double> HeaderFontSizeProperty =
            AvaloniaProperty.Register<MetroGroupBox, double>(nameof(HeaderFontSize), defaultValue: 12);

        public IBrush HeaderForeground
        {
            get { return (IBrush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public static readonly StyledProperty<IBrush> HeaderForegroundProperty =
            AvaloniaProperty.Register<MetroGroupBox, IBrush>(nameof(HeaderForeground));

        public FontFamily HeaderFontFamily
        {
            get { return (FontFamily)GetValue(HeaderFontFamilyProperty); }
            set { SetValue(HeaderFontFamilyProperty, value); }
        }

        public static readonly StyledProperty<FontFamily> HeaderFontFamilyProperty =
            AvaloniaProperty.Register<MetroGroupBox, FontFamily>(nameof(HeaderFontFamily), defaultValue: FontFamily.Default);

        public FontWeight HeaderFontWeight
        {
            get { return (FontWeight)GetValue(HeaderFontWeightProperty); }
            set { SetValue(HeaderFontWeightProperty, value); }
        }

        public static readonly StyledProperty<FontWeight> HeaderFontWeightProperty =
            AvaloniaProperty.Register<MetroGroupBox, FontWeight>(nameof(HeaderFontWeight), defaultValue: FontWeight.Normal);
    }
}
