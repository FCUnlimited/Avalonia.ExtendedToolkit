﻿using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Avalonia.ExtendedToolkit.Controls
{
    //ported from https://github.com/MahApps/MahApps.Metro

    /// <summary>
    /// progress status ring
    /// </summary>
    public class ProgressRing : TemplatedControl
    {
        private const string PseudoClass_Large = ":large";
        private const string PseudoClass_Small = ":small";
        private const string PseudoClass_Active = ":active";
        private const string PseudoClass_Inactive = ":inactive";

        private List<Action> _deferredActions = new List<Action>();

        /// <summary>
        /// get /set BindableWidth
        /// </summary>
        public double BindableWidth
        {
            get { return (double)GetValue(BindableWidthProperty); }
            set { SetValue(BindableWidthProperty, value); }
        }

        /// <summary>
        /// <see cref="BindableWidth"/>
        /// </summary>
        public static readonly StyledProperty<double> BindableWidthProperty =
            AvaloniaProperty.Register<ProgressRing, double>(nameof(BindableWidth));

        /// <summary>
        /// get/sets IsActive
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// <see cref="IsActive"/>
        /// </summary>
        public static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<ProgressRing, bool>(nameof(IsActive), defaultValue: true);

        /// <summary>
        /// get/sets IsLarge
        /// </summary>
        public bool IsLarge
        {
            get { return (bool)GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }

        /// <summary>
        /// <see cref="IsLarge"/>
        /// </summary>
        public static readonly StyledProperty<bool> IsLargeProperty =
            AvaloniaProperty.Register<ProgressRing, bool>(nameof(IsLarge), defaultValue: true);

        /// <summary>
        /// get/sets MaxSideLength
        /// </summary>
        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            set { SetValue(MaxSideLengthProperty, value); }
        }

        /// <summary>
        /// <see cref="MaxSideLength"/>
        /// </summary>
        public static readonly StyledProperty<double> MaxSideLengthProperty =
            AvaloniaProperty.Register<ProgressRing, double>(nameof(MaxSideLength), defaultValue: default(double));

        /// <summary>
        /// get/sets EllipseDiameter
        /// </summary>
        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }

        /// <summary>
        /// <see cref="EllipseDiameter"/>
        /// </summary>
        public static readonly StyledProperty<double> EllipseDiameterProperty =
            AvaloniaProperty.Register<ProgressRing, double>(nameof(EllipseDiameter));

        /// <summary>
        /// get/sets EllipseOffset
        /// </summary>
        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            set { SetValue(EllipseOffsetProperty, value); }
        }

        /// <summary>
        /// <see cref="EllipseOffset"/>
        /// </summary>
        public static readonly StyledProperty<Thickness> EllipseOffsetProperty =
            AvaloniaProperty.Register<ProgressRing, Thickness>(nameof(EllipseOffset));

        /// <summary>
        /// get/sets EllipseDiameterScale
        /// </summary>
        public double EllipseDiameterScale
        {
            get { return (double)GetValue(EllipseDiameterScaleProperty); }
            set { SetValue(EllipseDiameterScaleProperty, value); }
        }

        /// <summary>
        /// <see cref="EllipseDiameterScale"/>
        /// </summary>
        public static readonly StyledProperty<double> EllipseDiameterScaleProperty =
            AvaloniaProperty.Register<ProgressRing, double>(nameof(EllipseDiameterScale), defaultValue: 1D);

        /// <summary>
        /// add some changed listeners
        /// </summary>
        static ProgressRing()
        {
            BindableWidthProperty.Changed.AddClassHandler<ProgressRing>((o, e) => BindableWidthCallback(o, e));
            IsActiveProperty.Changed.AddClassHandler<ProgressRing>((o, e) => IsActiveChanged(o, e));
            IsVisibleProperty.Changed.AddClassHandler<ProgressRing>((o, e) => IsVisibleChanged(o, e));
            WidthProperty.Changed.AddClassHandler<ProgressRing>((o, e) => OnSizeChanged(o, e));
            HeightProperty.Changed.AddClassHandler<ProgressRing>((o, e) => OnSizeChanged(o, e));
            IsLargeProperty.Changed.AddClassHandler<ProgressRing>((o, e) => IsLargeChangedCallback(o, e));
        }

        private static void IsLargeChangedCallback(ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
        {
            ring.UpdateLargeState();
        }

        private static void OnSizeChanged(ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
        {
            ring.SetValue(BindableWidthProperty, ring.Width);
        }

        private static void IsVisibleChanged(ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                if ((bool)e.NewValue == false)
                {
                    ring.SetValue(ProgressRing.IsActiveProperty, false);
                }
                else
                {
                    ring.SetValue(ProgressRing.IsActiveProperty, true);
                }
            }
        }

        private static void IsActiveChanged(ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
        {
            //if(e.NewValue is bool)
            //    ring.IsVisible = (bool)e.NewValue;
            ring.UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            PseudoClasses.Remove(PseudoClass_Inactive);
            PseudoClasses.Remove(PseudoClass_Active);
            Action action;

            PseudoClasses.Set(PseudoClass_Active, IsActive);
            PseudoClasses.Set(PseudoClass_Inactive, IsActive == false);

            //if (IsActive)
            //{  /*action = () =>*/
            //    this.PseudoClasses.Add(PseudoClass_Active);

            //}
            //else
            ///*action = () =>*/
            //{
            //    this.PseudoClasses.Add(PseudoClass_Inactive);

            //}

            //if (_deferredActions != null)
            //    _deferredActions.Add(action);
            //else
            //    action();
        }

        private static void BindableWidthCallback(ProgressRing ring, AvaloniaPropertyChangedEventArgs e)
        {
            //var action = new Action(() =>
            //{
            ring.SetEllipseDiameter((double)e.NewValue);
            ring.SetEllipseOffset((double)e.NewValue);
            ring.SetMaxSideLength((double)e.NewValue);
            //});

            //if (ring._deferredActions != null)
            //    ring._deferredActions.Add(action);
            //else
            //    action();
        }

        private void SetMaxSideLength(double width)
        {
            SetValue(MaxSideLengthProperty, width <= 20 ? 20 : width);
        }

        private void SetEllipseDiameter(double width)
        {
            SetValue(EllipseDiameterProperty, (width / 8) * EllipseDiameterScale);
        }

        private void SetEllipseOffset(double width)
        {
            SetValue(EllipseOffsetProperty, new Thickness(0, width / 2, 0, 0));
        }

        private void UpdateLargeState()
        {
            PseudoClasses.Remove(PseudoClass_Large);
            PseudoClasses.Remove(PseudoClass_Small);

            Action action;

            PseudoClasses.Set(PseudoClass_Large, IsLarge);
            PseudoClasses.Set(PseudoClass_Small, IsLarge == false);
            //if (IsLarge)
            //    /*action = () =>*/ this.PseudoClasses.Add(PseudoClass_Large);
            //else
            //    /*action = () =>*/ this.PseudoClasses.Add(PseudoClass_Small);

            //if (_deferredActions != null)
            //    _deferredActions.Add(action);
            //else
            //    action();
        }

        /// <summary>
        /// UpdateLargeState, UpdateActiveState
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            //make sure the states get updated
            UpdateLargeState();
            UpdateActiveState();
            if (_deferredActions != null)
            {
                foreach (var action in _deferredActions)
                {
                    action();
                }
            }
            _deferredActions = null;
        }

        /// <summary>
        /// sets the pseudo classes 
        /// </summary>
        /// <param name="context"></param>
        public override void Render(DrawingContext context)
        {
            if (IsLarge)
            {
                PseudoClasses.Set(PseudoClass_Large, IsLarge);
            }
            else
            {
                PseudoClasses.Set(PseudoClass_Small, IsLarge == false);
            }

            if (IsActive)
            {
                PseudoClasses.Set(PseudoClass_Active, IsActive);
            }
            else
            {
                PseudoClasses.Set(PseudoClass_Inactive, IsActive == false);
            }

            base.Render(context);
        }
    }
}
