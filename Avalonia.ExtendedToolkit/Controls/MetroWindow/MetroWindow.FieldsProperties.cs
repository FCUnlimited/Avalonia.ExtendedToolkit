﻿using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;

namespace Avalonia.ExtendedToolkit.Controls
{
    public partial class MetroWindow : Window
    {
        private const string PART_Icon = "PART_Icon";
        private const string PART_TitleBar = "PART_TitleBar";
        private const string PART_WindowTitleBackground = "PART_WindowTitleBackground";
        private const string PART_WindowTitleThumb = "PART_WindowTitleThumb";
        private const string PART_FlyoutModalDragMoveThumb = "PART_FlyoutModalDragMoveThumb";
        private const string PART_LeftWindowCommands = "PART_LeftWindowCommands";
        private const string PART_RightWindowCommands = "PART_RightWindowCommands";
        private const string PART_WindowButtonCommands = "PART_WindowButtonCommands";
        private const string PART_OverlayBox = "PART_OverlayBox";
        private const string PART_MetroActiveDialogContainer = "PART_MetroActiveDialogContainer";
        private const string PART_MetroInactiveDialogsContainer = "PART_MetroInactiveDialogsContainer";
        private const string PART_FlyoutModal = "PART_FlyoutModal";
        private const string PART_Content = "PART_Content";

        private const string PART_TopHorizontalGrip="PART_TopHorizontalGrip";
        private const string PART_BottomHorizontalGrip = "PART_BottomHorizontalGrip";
        private const string PART_LeftVerticalGrip = "PART_LeftVerticalGrip";
        private const string PART_RightVerticalGrip = "PART_RightVerticalGrip";
        private const string PART_TopLeftGrip = "PART_TopLeftGrip";
        private const string PART_BottomLeftGrip = "PART_BottomLeftGrip";
        private const string PART_TopRightGrip = "PART_TopRightGrip";
        private const string PART_BottomRightGrip = "PART_BottomRightGrip";

        private ContentControl _icon;
        private ContentControl _titleBar;
        private Rectangle _titleBarBackground;
        private Thumb _windowTitleThumb;
        private Thumb _flyoutModalDragMoveThumb;
        private IInputElement restoreFocus;
        internal ContentPresenter LeftWindowCommandsPresenter;
        internal ContentPresenter RightWindowCommandsPresenter;
        internal ContentPresenter WindowButtonCommandsPresenter;

        internal Grid overlayBox;
        internal Grid metroActiveDialogContainer;
        internal Grid metroInactiveDialogContainer;

        //private Storyboard overlayStoryboard;
        private Rectangle flyoutModal;

        //private Button _closeButton;
        //private Button _minimiseButton;
        //private Button _restoreButton;
        //private Image _icon;
        //private Grid _titleBar;

        private Grid _bottomHorizontalGrip;
        private Grid _bottomLeftGrip;
        private Grid _bottomRightGrip;
        private Grid _leftVerticalGrip;
        private Grid _rightVerticalGrip;
        private Grid _topHorizontalGrip;
        private Grid _topLeftGrip;
        private Grid _topRightGrip;

        private bool _mouseDown;
        private Point _mouseDownPosition;
    }
}