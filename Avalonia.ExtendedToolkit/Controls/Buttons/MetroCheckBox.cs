﻿using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.ExtendedToolkit.Controls
{
    public class MetroCheckBox : CheckBox
    {
        public IBrush FocusBorderBrush
        {
            get { return (IBrush)GetValue(FocusBorderBrushProperty); }
            set { SetValue(FocusBorderBrushProperty, value); }
        }

        public static readonly AvaloniaProperty FocusBorderBrushProperty =
            AvaloniaProperty.Register<MetroCheckBox, IBrush>(nameof(FocusBorderBrush));




        public IBrush MouseOverBorderBrush
        {
            get { return (IBrush)GetValue(MouseOverBorderBrushProperty); }
            set { SetValue(MouseOverBorderBrushProperty, value); }
        }


        public static readonly AvaloniaProperty MouseOverBorderBrushProperty =
            AvaloniaProperty.Register<MetroCheckBox, IBrush>(nameof(MouseOverBorderBrush));



        public bool IsIndeterminate
        {

            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }


        public static readonly AvaloniaProperty IsIndeterminateProperty =
            AvaloniaProperty.Register<MetroCheckBox, bool>(nameof(IsIndeterminate));



        public FlowDirection FlowDirection
        {
            get { return (FlowDirection)GetValue(FlowDirectionProperty); }
            set { SetValue(FlowDirectionProperty, value); }
        }


        public static readonly AvaloniaProperty FlowDirectionProperty =
            AvaloniaProperty.Register<MetroCheckBox, FlowDirection>(nameof(FlowDirection));




        public FlowDirection ContentDirection
        {
            get { return (FlowDirection)GetValue(ContentDirectionProperty); }
            set { SetValue(ContentDirectionProperty, value); }
        }


        public static readonly AvaloniaProperty ContentDirectionProperty =
            AvaloniaProperty.Register<MetroCheckBox, FlowDirection>(nameof(ContentDirection));
        private Path _checkBoxPath;
        private Rectangle _indeterminateCheck;

        static MetroCheckBox()
        {
            IsCheckedProperty.Changed.AddClassHandler<MetroCheckBox>((o, e) => OnIsCheckChanged(o, e));
            IsIndeterminateProperty.Changed.AddClassHandler<MetroCheckBox>((o, e) => OnIsIndeterminateChanged(o, e));
        }

        private static void OnIsIndeterminateChanged(MetroCheckBox metroCheckBox, AvaloniaPropertyChangedEventArgs e)
        {
            if (metroCheckBox._indeterminateCheck != null)
            {
                metroCheckBox._indeterminateCheck.Opacity = e.NewValue != null && (bool)e.NewValue ? 1 : 0;
            }
        }

        private static void OnIsCheckChanged(MetroCheckBox metroCheckBox, AvaloniaPropertyChangedEventArgs e)
        {
            if (metroCheckBox._checkBoxPath != null)
            {
                metroCheckBox._checkBoxPath.Opacity = e.NewValue != null && (bool)e.NewValue ? 1 : 0;

                if (e.NewValue == null&& metroCheckBox.IsThreeState)
                {
                    metroCheckBox._checkBoxPath.Opacity = 0;
                }
            }

            if (metroCheckBox.IsThreeState)
            {
                metroCheckBox.IsIndeterminate = e.NewValue == null;
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            _checkBoxPath = e.NameScope.Find<Path>("checkBox");
            _indeterminateCheck = e.NameScope.Find<Rectangle>("IndeterminateCheck");

            //set init value 
            bool? isChecked = IsChecked;
            if(isChecked.HasValue)
            {
                IsChecked = null;
            }
            else
            {
                IsChecked = false;
            }
            
            IsChecked = isChecked;

            base.OnTemplateApplied(e);
        }


    }
}
