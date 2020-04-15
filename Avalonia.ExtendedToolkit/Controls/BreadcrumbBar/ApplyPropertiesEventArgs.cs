﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Avalonia.ExtendedToolkit.Controls
{
    //ported from https://github.com/jogibear9988/OdysseyWPF.git

    public delegate void ApplyPropertiesEventHandler(object sender, ApplyPropertiesEventArgs e);

    public class ApplyPropertiesEventArgs: RoutedEventArgs
    {
        public ApplyPropertiesEventArgs(object item, BreadcrumbItem breadcrumb, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Item = item;
            Breadcrumb = breadcrumb;
        }

        /// <summary>
        /// The breadcrumb for which to apply the properites.
        /// </summary>
        public BreadcrumbItem Breadcrumb { get; private set; }

        /// <summary>
        /// The data item of the breadcrumb.
        /// </summary>
        public object Item { get; private set; }

        public IBitmap Image { get; set; }

        /// <summary>
        /// The trace that is used to show the title of a breadcrumb.
        /// </summary>
        public object Trace { get; set; }

        /// <summary>
        /// The trace that is used to build the path.
        /// This can be used to remove the trace of the root item in the path, if necassary.
        /// </summary>
        public string TraceValue { get; set; }
    }
}
