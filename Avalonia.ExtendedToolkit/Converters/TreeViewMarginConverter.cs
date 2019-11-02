﻿using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Avalonia.ExtendedToolkit.Extensions;

namespace Avalonia.ExtendedToolkit.Converters
{
    public class TreeViewMarginConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as TreeViewItem;
            if (item == null)
                return new Thickness(0);

            return new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }


    }

    public static class TreeViewItemExtensions
    {
        public static int GetDepth(this TreeViewItem item)
        {
            return item.GetAncestors().TakeWhile(e => !(e is TreeView)).OfType<TreeViewItem>().Count();
        }
    }

}
