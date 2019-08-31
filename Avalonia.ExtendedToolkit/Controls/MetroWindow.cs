﻿using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using System;

namespace Avalonia.ExtendedToolkit.Controls
{
    /// <summary>
    /// Interaction logic for <see cref="MetroWindow"/> xaml.
    /// </summary>
    public class MetroWindow : Window, IStyleable
    {
        private Grid _titleBar;

        private Grid _bottomHorizontalGrip;
        private Grid _bottomLeftGrip;
        private Grid _bottomRightGrip;
        private Grid _leftVerticalGrip;
        private Grid _rightVerticalGrip;
        private Grid _topHorizontalGrip;
        private Grid _topLeftGrip;
        private Grid _topRightGrip;

        private Button _closeButton;
        private Button _minimiseButton;
        private Button _restoreButton;

        private Image _icon;

        private bool _mouseDown;
        private Point _mouseDownPosition;

        public event EventHandler<Size> OnSizeChanged;

        /// <summary>
        /// Defines the <see cref="IsChromeVisible"/> property.
        /// </summary>
        public static readonly AvaloniaProperty<bool> IsChromeVisibleProperty =
            AvaloniaProperty.Register<MetroWindow, bool>(nameof(IsChromeVisible), true);

        /// <summary>
        /// Defines the <see cref="TitleBarContent"/> property.
        /// </summary>
        public static readonly AvaloniaProperty<Control> TitleBarContentProperty =
            AvaloniaProperty.Register<MetroWindow, Control>(nameof(TitleBarContent));

        /// <summary>
        ///  Gets or sets the flag indicating whether chrome is visible.
        /// </summary>
        public bool IsChromeVisible
        {
            get => GetValue(IsChromeVisibleProperty);
            set => SetValue(IsChromeVisibleProperty, value);
        }

        /// <summary>
        ///  Gets or sets the title bar content control.
        /// </summary>
        public Control TitleBarContent
        {
            get => GetValue(TitleBarContentProperty);
            set => SetValue(TitleBarContentProperty, value);
        }

        Type IStyleable.StyleKey => typeof(MetroWindow);

        private void ToggleWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;

                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (_topHorizontalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.North);
            }
            else if (_bottomHorizontalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.South);
            }
            else if (_leftVerticalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.West);
            }
            else if (_rightVerticalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.East);
            }
            else if (_topLeftGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.NorthWest);
            }
            else if (_bottomLeftGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.SouthWest);
            }
            else if (_topRightGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.NorthEast);
            }
            else if (_bottomRightGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.SouthEast);
            }
            else if (_titleBar.IsPointerOver)
            {
                _mouseDown = true;
                _mouseDownPosition = e.GetPosition(this);
            }
            else
            {
                _mouseDown = false;
            }

            base.OnPointerPressed(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            _mouseDown = false;
            base.OnPointerReleased(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (_titleBar.IsPointerOver && _mouseDown)
            {
                WindowState = WindowState.Normal;
                BeginMoveDrag();
                _mouseDown = false;
            }

            base.OnPointerMoved(e);
        }

        protected override void HandleResized(Size clientSize)
        {
            base.HandleResized(clientSize);

            OnSizeChanged?.Invoke(this, clientSize);
        }

        /// <inheritdoc/>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _titleBar = e.NameScope.Find<Grid>("titlebar");
            _minimiseButton = e.NameScope.Find<Button>("minimiseButton");
            _restoreButton = e.NameScope.Find<Button>("restoreButton");
            _closeButton = e.NameScope.Find<Button>("closeButton");
            _icon = e.NameScope.Find<Image>("icon");

            _topHorizontalGrip = e.NameScope.Find<Grid>("topHorizontalGrip");
            _bottomHorizontalGrip = e.NameScope.Find<Grid>("bottomHorizontalGrip");
            _leftVerticalGrip = e.NameScope.Find<Grid>("leftVerticalGrip");
            _rightVerticalGrip = e.NameScope.Find<Grid>("rightVerticalGrip");

            _topLeftGrip = e.NameScope.Find<Grid>("topLeftGrip");
            _bottomLeftGrip = e.NameScope.Find<Grid>("bottomLeftGrip");
            _topRightGrip = e.NameScope.Find<Grid>("topRightGrip");
            _bottomRightGrip = e.NameScope.Find<Grid>("bottomRightGrip");

            _minimiseButton.Click += (sender, ee) => { WindowState = WindowState.Minimized; };

            _restoreButton.Click += (sender, ee) => { ToggleWindowState(); };

            _titleBar.DoubleTapped += (sender, ee) => { ToggleWindowState(); };

            _closeButton.Click += (sender, ee) => { Close(); };

            //icon.DoubleTapped += (sender, ee) => { Close(); };
        }
    }
}