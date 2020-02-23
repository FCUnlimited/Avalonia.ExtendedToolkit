﻿using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Specialized;
using System.Linq;

namespace Avalonia.ExtendedToolkit.Controls
{
    public class HamburgerMenu: ContentControl
    {
        public new AvaloniaList<HamburgerMenuItem> Content
        {
            get { return (AvaloniaList<HamburgerMenuItem>)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public new static readonly StyledProperty<AvaloniaList<HamburgerMenuItem>> ContentProperty =
            AvaloniaProperty.Register<HamburgerMenu, AvaloniaList<HamburgerMenuItem>>(nameof(Content));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly StyledProperty<bool> IsOpenProperty =
            AvaloniaProperty.Register<HamburgerMenu, bool>(nameof(IsOpen), defaultValue:false);

        public IBrush MenuIconColor
        {
            get { return (IBrush)GetValue(MenuIconColorProperty); }
            set { SetValue(MenuIconColorProperty, value); }
        }

        public static readonly StyledProperty<IBrush> MenuIconColorProperty =
            AvaloniaProperty.Register<HamburgerMenu, IBrush>(nameof(MenuIconColor));

        public IBrush SelectionIndicatorColor
        {
            get { return (IBrush)GetValue(SelectionIndicatorColorProperty); }
            set { SetValue(SelectionIndicatorColorProperty, value); }
        }

        public static readonly StyledProperty<IBrush> SelectionIndicatorColorProperty =
            AvaloniaProperty.Register<HamburgerMenu, IBrush>(nameof(SelectionIndicatorColor));

        public IBrush MenuItemForeground
        {
            get { return (IBrush)GetValue(MenuItemForegroundProperty); }
            set { SetValue(MenuItemForegroundProperty, value); }
        }

        public static readonly StyledProperty<IBrush> MenuItemForegroundProperty =
            AvaloniaProperty.Register<HamburgerMenu, IBrush>(nameof(MenuItemForeground));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly StyledProperty<int> SelectedIndexProperty =
            AvaloniaProperty.Register<HamburgerMenu, int>(nameof(SelectedIndex), defaultValue:0);

        public HamburgerMenu()
        {
            Content = new AvaloniaList<HamburgerMenuItem>();
            Content.CollectionChanged += Content_CollectionChanged;
        }

        private void Content_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<HamburgerMenuItem>())
                {
                    item.PropertyChanged += MenuItem_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
            }
        }

        private void MenuItem_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "IsSelected")
            {
                HamburgerMenuItem menuItem = sender as HamburgerMenuItem;
                SelectedIndex = Content.IndexOf(menuItem);
            }
        }
    }
}