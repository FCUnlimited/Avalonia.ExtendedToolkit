﻿using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Avalonia.ExtendedToolkit.Controls
{
    public class ExplorerBar : ItemsControl
    {
        /// <summary>
        /// The default value for the <see cref="ItemsControl.ItemsPanel"/> property.
        /// </summary>
        private static readonly FuncTemplate<IPanel> DefaultPanel =
            new FuncTemplate<IPanel>(() => new VirtualizingStackPanel());


        public Type StyleKey => typeof(ExplorerBar);

        static ExplorerBar()
        {
            ItemsPanelProperty.OverrideDefaultValue<ExplorerBar>(DefaultPanel);
        }

    }
}
