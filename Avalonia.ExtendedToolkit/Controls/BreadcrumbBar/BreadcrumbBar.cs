﻿using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Avalonia.ExtendedToolkit.Controls
{
    public class BreadcrumbBar : ContentControl, IAddChild
    {
        private const string partComboBox = "PART_ComboBox";
        private const string partRoot = "PART_Root";

        private ComboBox comboBox;
        private BreadcrumbButton rootButton;

        /// <summary>
        /// On initializing, it is possible that the Path property is set before the RootItem property, thus the declarative xaml Path would be overwritten by settings the
        /// RootItem property later. To avoid this affect, setting the Path also sets initPath on initializing and after initializing, the Path is restored by this value:
        /// </summary>
        private string initPath;

        public Type StyleKey => typeof(BreadcrumbBar);

        private ObservableCollection<Button> buttons = new ObservableCollection<Button>();

        /// <summary>
        /// Gets the collection of buttons to appear on the right of the breadcrumb bar.
        /// </summary>
        public ObservableCollection<Button> Buttons
        {
            get { return buttons; }
        }

        /// <summary>
        /// Gets or sets the DropDownItems for the combobox.
        /// </summary>
        public IEnumerable DropDownItems
        {
            get { return comboBoxControlItems.Items; }
        }

        // A helper class to store the DropDownItems since ItemCollection has no public creator:
        private ItemsControl comboBoxControlItems;

        private ObservableCollection<object> traces;

        public bool HideRootNode
        {
            get { return (bool)GetValue(HideRootNodeProperty); }
            set { SetValue(HideRootNodeProperty, value); }
        }

        public static readonly StyledProperty<bool> HideRootNodeProperty =
            AvaloniaProperty.Register<BreadcrumbBar, bool>(nameof(HideRootNode), defaultValue: true);

        public bool HasDropDownItems
        {
            get { return (bool)GetValue(HasDropDownItemsProperty); }
            set { SetValue(HasDropDownItemsProperty, value); }
        }

        public static readonly StyledProperty<bool> HasDropDownItemsProperty =
            AvaloniaProperty.Register<BreadcrumbBar, bool>(nameof(HasDropDownItems));

        public ItemsPanelTemplate DropDownItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(DropDownItemsPanelProperty); }
            set { SetValue(DropDownItemsPanelProperty, value); }
        }

        public static readonly StyledProperty<ItemsPanelTemplate> DropDownItemsPanelProperty =
            AvaloniaProperty.Register<BreadcrumbBar, ItemsPanelTemplate>(nameof(DropDownItemsPanel));

        public bool IsRootSelected
        {
            get { return (bool)GetValue(IsRootSelectedProperty); }
            set { SetValue(IsRootSelectedProperty, value); }
        }

        public static readonly StyledProperty<bool> IsRootSelectedProperty =
            AvaloniaProperty.Register<BreadcrumbBar, bool>(nameof(IsRootSelected), defaultValue: true);

        public IDataTemplate DropDownItemTemplate
        {
            get { return (IDataTemplate)GetValue(DropDownItemTemplateProperty); }
            set { SetValue(DropDownItemTemplateProperty, value); }
        }

        public static readonly StyledProperty<IDataTemplate> DropDownItemTemplateProperty =
            AvaloniaProperty.Register<BreadcrumbBar, IDataTemplate>(nameof(DropDownItemTemplate));

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public static readonly StyledProperty<bool> IsEditableProperty =
            AvaloniaProperty.Register<BreadcrumbBar, bool>(nameof(IsEditable), defaultValue: true);

        
        public IDataTemplate OverflowItemTemplate
        {
            get { return (IDataTemplate)GetValue(OverflowItemTemplateProperty); }
            set { SetValue(OverflowItemTemplateProperty, value); }
        }

        public static readonly StyledProperty<IDataTemplate> OverflowItemTemplateProperty =
            AvaloniaProperty.Register<BreadcrumbBar, IDataTemplate>(nameof(OverflowItemTemplate));

        public IEnumerable CollapsedTraces
        {
            get { return (IEnumerable)GetValue(CollapsedTracesProperty); }
            set { SetValue(CollapsedTracesProperty, value); }
        }

        public static readonly StyledProperty<IEnumerable> CollapsedTracesProperty =
            AvaloniaProperty.Register<BreadcrumbBar, IEnumerable>(nameof(CollapsedTraces));

        public object Root
        {
            get { return (object)GetValue(RootProperty); }
            set { SetValue(RootProperty, value); }
        }

        public static readonly StyledProperty<object> RootProperty =
            AvaloniaProperty.Register<BreadcrumbBar, object>(nameof(Root));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly StyledProperty<object> SelectedItemProperty =
            AvaloniaProperty.Register<BreadcrumbBar, object>(nameof(SelectedItem));

        public BreadcrumbItem SelectedBreadcrumb
        {
            get { return (BreadcrumbItem)GetValue(SelectedBreadcrumbProperty); }
            set { SetValue(SelectedBreadcrumbProperty, value); }
        }

        public static readonly StyledProperty<BreadcrumbItem> SelectedBreadcrumbProperty =
            AvaloniaProperty.Register<BreadcrumbBar, BreadcrumbItem>(nameof(SelectedBreadcrumb));

        public bool IsOverflowPressed
        {
            get { return (bool)GetValue(IsOverflowPressedProperty); }
            set { SetValue(IsOverflowPressedProperty, value); }
        }

        public static readonly StyledProperty<bool> IsOverflowPressedProperty =
            AvaloniaProperty.Register<BreadcrumbBar, bool>(nameof(IsOverflowPressed));

        public BreadcrumbItem RootItem
        {
            get { return (BreadcrumbItem)GetValue(RootItemProperty); }
            set { SetValue(RootItemProperty, value); }
        }

        public static readonly StyledProperty<BreadcrumbItem> RootItemProperty =
            AvaloniaProperty.Register<BreadcrumbBar, BreadcrumbItem>(nameof(RootItem));

        
        public IDataTemplate BreadcrumbItemTemplate
        {
            get { return (IDataTemplate)GetValue(BreadcrumbItemTemplateProperty); }
            set { SetValue(BreadcrumbItemTemplateProperty, value); }
        }

        public static readonly StyledProperty<IDataTemplate> BreadcrumbItemTemplateProperty =
            AvaloniaProperty.Register<BreadcrumbBar, IDataTemplate>(nameof(BreadcrumbItemTemplate));

        public ButtonMode OverflowMode
        {
            get { return (ButtonMode)GetValue(OverflowModeProperty); }
            set { SetValue(OverflowModeProperty, value); }
        }

        public static readonly StyledProperty<ButtonMode> OverflowModeProperty =
            AvaloniaProperty.Register<BreadcrumbBar, ButtonMode>(nameof(OverflowMode), defaultValue: ButtonMode.Overflow);

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly StyledProperty<bool> IsDropDownOpenProperty =
            AvaloniaProperty.Register<BreadcrumbBar, bool>(nameof(IsDropDownOpen));

        public string SeparatorString
        {
            get { return (string)GetValue(SeparatorStringProperty); }
            set { SetValue(SeparatorStringProperty, value); }
        }

        public static readonly StyledProperty<string> SeparatorStringProperty =
            AvaloniaProperty.Register<BreadcrumbBar, string>(nameof(SeparatorString), defaultValue: System.IO.Path.DirectorySeparatorChar.ToString());

        public string PathBinding
        {
            get { return (string)GetValue(PathBindingProperty); }
            set { SetValue(PathBindingProperty, value); }
        }

        public static readonly StyledProperty<string> PathBindingProperty =
            AvaloniaProperty.Register<BreadcrumbBar, string>(nameof(PathBinding));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly StyledProperty<string> PathProperty =
            AvaloniaProperty.Register<BreadcrumbBar, string>(nameof(Path), defaultValue: string.Empty);

        public int SelectedDropDownIndex
        {
            get { return (int)GetValue(SelectedDropDownIndexProperty); }
            set { SetValue(SelectedDropDownIndexProperty, value); }
        }

        public static readonly StyledProperty<int> SelectedDropDownIndexProperty =
            AvaloniaProperty.Register<BreadcrumbBar, int>(nameof(SelectedDropDownIndex), defaultValue: -1);

        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public static readonly StyledProperty<double> ProgressValueProperty =
            AvaloniaProperty.Register<BreadcrumbBar, double>(nameof(ProgressValue), defaultValue: 0.0d,
                coerce: (o, e) => { return CoerceProgressValue(o, e); });

        public double ProgressMaximum
        {
            get { return (double)GetValue(ProgressMaximumProperty); }
            set { SetValue(ProgressMaximumProperty, value); }
        }

        public static readonly StyledProperty<double> ProgressMaximumProperty =
            AvaloniaProperty.Register<BreadcrumbBar, double>(nameof(ProgressMaximum), defaultValue: 100d
                , coerce: (o, e) => { return CoerceProgressMaximum(o, e); });

        public double ProgressMinimum
        {
            get { return (double)GetValue(ProgressMinimumProperty); }
            set { SetValue(ProgressMinimumProperty, value); }
        }

        public static readonly StyledProperty<double> ProgressMinimumProperty =
            AvaloniaProperty.Register<BreadcrumbBar, double>(nameof(ProgressMinimum), defaultValue: 0.0d
                , coerce: (o, e) => { return CoerceProgressMinimum(o, e); });

        public ICommand ShowDropDownCommand { get; private set; }

        public ICommand SelectTraceCommand { get; private set; }

        public ICommand SelectRootCommand { get; private set; }

        public static RoutedEvent<RoutedEventArgs> BreadcrumbItemDropDownOpenedEvent =
                    RoutedEvent.Register<BreadcrumbBar, RoutedEventArgs>(nameof(BreadcrumbItemDropDownOpenedEvent), RoutingStrategies.Bubble);

        public event EventHandler BreadcrumbItemDropDownOpened
        {
            add
            {
                AddHandler(BreadcrumbItemDropDownOpenedEvent, value);
            }
            remove
            {
                RemoveHandler(BreadcrumbItemDropDownOpenedEvent, value);
            }
        }

        public static RoutedEvent<RoutedEventArgs> BreadcrumbItemDropDownClosedEvent =
                    RoutedEvent.Register<BreadcrumbBar, RoutedEventArgs>(nameof(BreadcrumbItemDropDownClosedEvent), RoutingStrategies.Bubble);

        public event EventHandler BreadcrumbItemDropDownClosed
        {
            add
            {
                AddHandler(BreadcrumbItemDropDownClosedEvent, value);
            }
            remove
            {
                RemoveHandler(BreadcrumbItemDropDownClosedEvent, value);
            }
        }

        public static RoutedEvent<RoutedEventArgs> ProgressValueChangedEvent =
                    RoutedEvent.Register<BreadcrumbBar, RoutedEventArgs>(nameof(ProgressValueChangedEvent), RoutingStrategies.Bubble);

        public event EventHandler ProgressValueChanged
        {
            add
            {
                AddHandler(ProgressValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ProgressValueChangedEvent, value);
            }
        }

        public static RoutedEvent<ApplyPropertiesEventArgs> ApplyPropertiesEvent =
                    RoutedEvent.Register<BreadcrumbBar, ApplyPropertiesEventArgs>(nameof(ApplyPropertiesEvent), RoutingStrategies.Bubble);

        public event ApplyPropertiesEventHandler ApplyProperties
        {
            add
            {
                AddHandler(ApplyPropertiesEvent, value);
            }
            remove
            {
                RemoveHandler(ApplyPropertiesEvent, value);
            }
        }

        public static RoutedEvent<RoutedEventArgs> SelectedBreadcrumbChangedEvent =
                    RoutedEvent.Register<BreadcrumbBar, RoutedEventArgs>(nameof(SelectedBreadcrumbChangedEvent), RoutingStrategies.Bubble);

        public event EventHandler SelectedBreadcrumbChanged
        {
            add
            {
                AddHandler(SelectedBreadcrumbChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedBreadcrumbChangedEvent, value);
            }
        }

        public static RoutedEvent<RoutedEventArgs> PathChangedEvent =
                    RoutedEvent.Register<BreadcrumbBar, RoutedEventArgs>(nameof(PathChangedEvent), RoutingStrategies.Bubble);

        public event EventHandler PathChanged
        {
            add
            {
                AddHandler(PathChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PathChangedEvent, value);
            }
        }

        // <summary>
        /// Occurs before acessing the Items property of a BreadcrumbItem. This event can be used to populate the Items on demand.
        /// </summary>
        public static RoutedEvent<BreadcrumbItemEventArgs> PopulateItemsEvent =
                    RoutedEvent.Register<BreadcrumbBar, BreadcrumbItemEventArgs>(nameof(PopulateItemsEvent), RoutingStrategies.Bubble);

        public event BreadcrumbItemEventHandler PopulateItems
        {
            add
            {
                AddHandler(PopulateItemsEvent, value);
            }
            remove
            {
                RemoveHandler(PopulateItemsEvent, value);
            }
        }

        public static RoutedEvent<RoutedEventArgs> PathConversionEvent =
                    RoutedEvent.Register<BreadcrumbBar, RoutedEventArgs>(nameof(PathConversionEvent), RoutingStrategies.Bubble);

        public event EventHandler PathConversion
        {
            add
            {
                AddHandler(PathConversionEvent, value);
            }
            remove
            {
                RemoveHandler(PathConversionEvent, value);
            }
        }

        /// <summary>
        /// Gets the number of the first breadcrumb to hide in the path if descending breadcrumbs are selected.
        /// </summary>
        private int BreadcrumbsToHide
        {
            get { return HideRootNode ? 1 : 0; }
        }

        /// <summary>
        /// Gets or sets the TraceBinding property that will be set to every child BreadcrumbItem. This is not a dependency property!
        /// </summary>
        public Binding TraceBinding { get; set; }

        /// <summary>
        /// Gets or sets the ImageBinding property that will be set to every child BreadcrumbItem. This is not a dependency property!
        /// </summary>
        public Binding ImageBinding { get; set; }

        public BreadcrumbBar()
        {
            RootProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => OnRootPropertyChanged(o, e)));
            SelectedItemProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => OnSelectedItemPropertyChanged(o, e)));
            SelectedBreadcrumbProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => OnSelectedBreadcrumbPropertyChanged(o, e)));
            IsOverflowPressedProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => OverflowPressedChanged(o, e)));
            RootItemProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => OnRootItemPropertyChanged(o, e)));
            IsDropDownOpenProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => IsDropDownOpenChanged(o, e)));
            PathProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => PathPropertyChanged(o, e)));
            PathBindingProperty.Changed.AddClassHandler<BreadcrumbBar>((o, e) => OnPathBindingPropertyChanged(o, e));
            ProgressValueProperty.Changed.AddClassHandler<BreadcrumbBar>(((o, e) => ProgressValuePropertyChanged(o, e)));

            ShowDropDownCommand = ReactiveCommand.Create<object>(o => ShowDropDownExecuted(o), outputScheduler: RxApp.MainThreadScheduler);
            SelectTraceCommand = ReactiveCommand.Create<object>(o => SelectTraceCommandExecuted(o), outputScheduler: RxApp.MainThreadScheduler);
            SelectRootCommand = ReactiveCommand.Create<object>(o => SelectRootCommandExecuted(o), outputScheduler: RxApp.MainThreadScheduler);

            comboBoxControlItems = new ItemsControl();
            Binding b = new Binding("HasItems");
            b.Source = comboBoxControlItems;
            this.Bind(BreadcrumbBar.HasDropDownItemsProperty, b);

            traces = new ObservableCollection<object>();
            CollapsedTraces = traces;

            AddHandler(BreadcrumbItem.SelectionChangedEvent, breadcrumbItemSelectedItemChanged);
            AddHandler(BreadcrumbItem.TraceChangedEvent, breadcrumbItemTraceValueChanged);
            AddHandler(BreadcrumbItem.SelectionChangedEvent, breadcrumbItemSelectionChangedEvent);
            AddHandler(BreadcrumbItem.DropDownPressedChangedEvent, breadcrumbItemDropDownChangedEvent);
            AddHandler(Button.ClickEvent, buttonClickedEvent);
            traces.Add(null);

            this.PointerPressed += OnMouseDown;
        }

        static BreadcrumbBar()
        {
            //BorderThicknessProperty.OverrideDefaultValue<BreadcrumbBar>(new Thickness(1));
            //BorderThicknessProperty.OverrideDefaultValue<BreadcrumbBar>(new Thickness(1));
            //Color c = new Color(255, 245, 245, 245);
            //BackgroundProperty.OverrideDefaultValue<BreadcrumbBar>(new SolidColorBrush(c));
        }

        private void breadcrumbItemSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            BreadcrumbItem breadcrumb = e.Source as BreadcrumbItem;
            if (breadcrumb != null && breadcrumb.SelectedBreadcrumb != null) breadcrumb = breadcrumb.SelectedBreadcrumb;
            SelectedBreadcrumb = breadcrumb;

            if (SelectedBreadcrumb != null)
            {
                SelectedItem = SelectedBreadcrumb.Data;
            }
            Path = GetEditPath();
        }

        private void breadcrumbItemTraceValueChanged(object sender, RoutedEventArgs e)
        {
            if (e.Source == RootItem)
            {
                //TODO: This causes Path binding not to work (see PasswordSafe):
                //Path = GetEditPath();
            }
        }

        private void breadcrumbItemSelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            BreadcrumbItem parent = e.Source as BreadcrumbItem;
            if (parent != null && parent.SelectedBreadcrumb != null)
            {
                OnPopulateItems(parent.SelectedBreadcrumb);
            }
        }

        private void breadcrumbItemDropDownChangedEvent(object sender, RoutedEventArgs e)
        {
            BreadcrumbItem breadcrumb = e.Source as BreadcrumbItem;
            if (breadcrumb.IsDropDownPressed)
            {
                OnBreadcrumbItemDropDownOpened(e);
            }
            else
            {
                OnBreadcrumbItemDropDownClosed(e);
            }
        }

        /// <summary>
        /// Remove the focus from a button when it was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonClickedEvent(object sender, RoutedEventArgs e)
        {
            if (!this.IsDropDownOpen)
            {
                // focus only if it has aleady focus and the button
                // that was pressed is not the drop down button of the combobox:
                if (/*this.IsKeyboardFocusWithin &&*/ (e.Source is BreadcrumbButton))
                {
                    //   this.Focus();
                }
            }
        }

        /// <summary>
        /// Occurs before acessing the Items property of a BreadcrumbItem. This event can be used to populate the Items on demand.
        /// </summary>
        protected virtual void OnPopulateItems(BreadcrumbItem item)
        {
            BreadcrumbItemEventArgs args = new BreadcrumbItemEventArgs(item, BreadcrumbBar.PopulateItemsEvent);
            RaiseEvent(args);
        }

        /// <summary>
        /// Occurs when the dropdown of a BreadcrumbItem is opened.
        /// </summary>
        protected virtual void OnBreadcrumbItemDropDownOpened(RoutedEventArgs e)
        {
            BreadcrumbItemEventArgs args = new BreadcrumbItemEventArgs(e.Source as BreadcrumbItem, BreadcrumbItemDropDownOpenedEvent);
            RaiseEvent(args);
        }

        /// <summary>
        /// Occurs when the dropdown of a BreadcrumbItem is closed.
        /// </summary>
        protected virtual void OnBreadcrumbItemDropDownClosed(RoutedEventArgs e)
        {
            BreadcrumbItemEventArgs args = new BreadcrumbItemEventArgs(e.Source as BreadcrumbItem, BreadcrumbItemDropDownClosedEvent);
            RaiseEvent(args);
        }

        private void SelectRootCommandExecuted(object sender)
        {
            BreadcrumbItem item = sender as BreadcrumbItem;
            if (item != null)
            {
                item.SelectedItem = null;
            }
        }

        private void SelectTraceCommandExecuted(object sender)
        {
            BreadcrumbItem item = sender as BreadcrumbItem;
            if (item != null)
            {
                item.SelectedItem = null;
            }
        }

        private void ShowDropDownExecuted(object sender)
        {
            BreadcrumbBar bar = sender as BreadcrumbBar;
            if (bar.IsEditable && bar.DropDownItems.OfType<object>().Count() > 0) bar.IsDropDownOpen = true;
        }

        private static double CoerceProgressMaximum(IAvaloniaObject d, double baseValue)
        {
            BreadcrumbBar bar = d as BreadcrumbBar;
            double value = (double)baseValue;
            if (value < bar.ProgressMinimum) value = bar.ProgressMinimum;
            if (value < bar.ProgressValue) bar.ProgressValue = value;
            if (value < 0) value = 0;

            return value;
        }

        private static double CoerceProgressMinimum(IAvaloniaObject d, double baseValue)
        {
            BreadcrumbBar bar = d as BreadcrumbBar;
            double value = (double)baseValue;
            if (value > bar.ProgressMaximum) value = bar.ProgressMaximum;
            if (value > bar.ProgressValue) bar.ProgressValue = value;

            return value;
        }

        private static double CoerceProgressValue(IAvaloniaObject o, double baseValue)
        {
            BreadcrumbBar bar = o as BreadcrumbBar;
            double value = (double)baseValue;
            if (value > bar.ProgressMaximum) value = bar.ProgressMaximum;
            if (value < bar.ProgressMinimum) value = bar.ProgressMinimum;

            return value;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            CheckOverflowImage();
            return size;
        }

        /// <summary>
        /// Check what image to display in the drop down button of the overflow button:
        /// </summary>
        private void CheckOverflowImage()
        {
            bool isOverflow = (RootItem != null && RootItem.SelectedBreadcrumb != null && RootItem.SelectedBreadcrumb.IsOverflow);
            OverflowMode = isOverflow ? ButtonMode.Overflow : ButtonMode.Breadcrumb;
        }

        /// <summary>
        /// Build the list of traces for the overflow button.
        /// </summary>
        private void BuildTraces()
        {
            BreadcrumbItem item = RootItem;

            traces.Clear();
            if (item != null && item.IsOverflow)
            {
                foreach (object trace in item.Items)
                {
                    MenuItem menuItem = new MenuItem();//new RibbonMenuItem();
                    menuItem.Tag = trace;
                    BreadcrumbItem bcItem = item.ContainerFromItem(trace);

                    menuItem.Click += menuItem_Click;
                    menuItem.Icon = GetImage(bcItem != null ? bcItem.Image : null);
                    if (trace == RootItem.SelectedItem) menuItem.FontWeight = FontWeight.Bold;
                    traces.Add(menuItem);
                    if (bcItem != null) menuItem.Header = bcItem.TraceValue;
                }
                traces.Insert(0, new Separator());
                MenuItem rootMenuItem = new MenuItem();//new RibbonMenuItem();
                rootMenuItem.Header = item.TraceValue;
                rootMenuItem.Command = SelectRootCommand;
                rootMenuItem.CommandParameter = item;
                rootMenuItem.Icon = GetImage(item.Image);
                traces.Insert(0, rootMenuItem);
            }

            item = item != null ? item.SelectedBreadcrumb : null;

            while (item != null)
            {
                if (!item.IsOverflow) break;
                MenuItem traceMenuItem = new MenuItem();
                traceMenuItem.Header = item.TraceValue;
                traceMenuItem.Command = SelectRootCommand;
                traceMenuItem.CommandParameter = item;
                traceMenuItem.Icon = GetImage(item.Image);
                traces.Insert(0, traceMenuItem);
                item = item.SelectedBreadcrumb;
            }
        }

        private void menuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = e.Source as MenuItem;
            if (RootItem != null && item != null)
            {
                object dataItem = item.Tag;
                if (dataItem != null && dataItem.Equals(RootItem.SelectedItem)) RootItem.SelectedItem = null;
                RootItem.SelectedItem = dataItem;
            }
        }

        private object GetImage(IImage imageSource)
        {
            if (imageSource == null) return null;
            Image image = new Image();
            image.Source = imageSource;
            image.Stretch = Stretch.Fill;
            //image.SnapsToDevicePixels = true;
            image.Width = image.Height = 16;

            return image;
        }

        private void OnPathBindingPropertyChanged(BreadcrumbBar o, AvaloniaPropertyChangedEventArgs e)
        {
            o.SetValue(PathProperty, e.NewValue);
        }

        private void ProgressValuePropertyChanged(BreadcrumbBar bar, AvaloniaPropertyChangedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(BreadcrumbBar.ProgressValueChangedEvent);
            bar.RaiseEvent(args);
        }

        private void PathPropertyChanged(BreadcrumbBar bar, AvaloniaPropertyChangedEventArgs e)
        {
            string newPath = e.NewValue as string;
            //   bar.PathBinding = newPath;

            if (!bar.IsInitialized)
            {
                bar.Path = bar.initPath = newPath;
            }
            else
            {
                bar.BuildBreadcrumbsFromPath(newPath);
                bar.OnPathChanged(e.OldValue as string, newPath);
            }
        }

        /// <summary>
        /// Occurs when the Path property is changed.
        /// </summary>
        protected virtual void OnPathChanged(string oldValue, string newValue)
        {
            BuildBreadcrumbsFromPath(newValue);
            if (this.IsInitialized)//Loaded
            {
                RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(oldValue, newValue, PathChangedEvent);
                RaiseEvent(args);
            }
        }

        /// <summary>
        /// Traces the specified path and builds the associated BreadcrumbItems.
        /// </summary>
        /// <param name="path">The traces separated by the SepearatorString property.</param>
        private bool BuildBreadcrumbsFromPath(string newPath)
        {
            PathConversionEventArgs e = new PathConversionEventArgs(PathConversionEventArgs.ConversionMode.EditToDisplay, newPath, Root, PathConversionEvent);
            RaiseEvent(e);
            newPath = e.DisplayPath;

            BreadcrumbItem item = RootItem;
            if (item == null)
            {
                this.Path = null;
                return false;
            }

            newPath = RemoveLastEmptySeparator(newPath);
            string[] traces = newPath.Split(new string[] { SeparatorString }, StringSplitOptions.RemoveEmptyEntries);
            if (traces.Length == 0) RootItem.SelectedItem = null;
            int index = 0;

            List<int> itemIndex = new List<int>();

            // if the root is specified as first trace, then skip:
            int length = traces.Length;
            int max = BreadcrumbsToHide;
            if (traces.Length > index && max > 0 && traces[index] == (RootItem.TraceValue))
            {
                length--;
                index++;
                max--;
            }

            for (int i = index; i < traces.Length; i++)
            {
                if (item == null) break;

                string trace = traces[i];
                OnPopulateItems(item);
                object next = item.GetTraceItem(trace);
                if (next == null) break;
                itemIndex.Add(item.Items.OfType<object>().ToList().IndexOf(next));
                BreadcrumbItem container = item.ContainerFromItem(next);

                item = container;
            }
            if (length != itemIndex.Count)
            {
                //recover the last path:
                Path = GetDisplayPath();
                return false;
            }

            // temporarily remove the SelectionChangedEvent handler to minimize processing of events while building the breadcrumb items:
            RemoveHandler(BreadcrumbItem.SelectionChangedEvent, breadcrumbItemSelectedItemChanged);
            try
            {
                item = RootItem;
                for (int i = 0; i < itemIndex.Count; i++)
                {
                    if (item == null) break;
                    item.SelectedIndex = itemIndex[i];
                    item = item.SelectedBreadcrumb;
                }
                if (item != null) item.SelectedItem = null;
                SelectedBreadcrumb = item;
                SelectedItem = item != null ? item.Data : null;
            }
            finally
            {
                AddHandler(BreadcrumbItem.SelectionChangedEvent, breadcrumbItemSelectedItemChanged);
            }

            return true;
        }

        /// <summary>
        /// Remove the last separator string from the path if there is no additional trace.
        /// </summary>
        /// <param name="path">The path from which to remove the last separator.</param>
        /// <returns>The path without an unecassary last separator.</returns>
        private string RemoveLastEmptySeparator(string path)
        {
            path = path.Trim();
            int sepLength = SeparatorString.Length;
            if (path.EndsWith(SeparatorString))
            {
                path = path.Remove(path.Length - sepLength, sepLength);
            }
            return path;
        }

        private void IsDropDownOpenChanged(BreadcrumbBar bar, AvaloniaPropertyChangedEventArgs e)
        {
            bar.OnDropDownOpenChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Occurs when the IsDropDownOpen property is changed.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnDropDownOpenChanged(bool oldValue, bool newValue)
        {
            if (comboBox != null && newValue)
            {
                SetInputState();
                if (IsEditable)
                {
                    comboBox.IsVisible = true;

                    comboBox.IsDropDownOpen = true;
                }
            }
        }

        private void OnRootItemPropertyChanged(BreadcrumbBar o, AvaloniaPropertyChangedEventArgs e)
        {
            o.OnRootItemChanged((BreadcrumbItem)e.OldValue, (BreadcrumbItem)e.NewValue);
        }

        /// <summary>
        /// Occurs when the selected item of an embedded BreadcrumbItem is changed.
        /// </summary>
        /// <param name="oldvalue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnRootItemChanged(BreadcrumbItem oldValue, BreadcrumbItem newValue)
        {
        }

        private void OverflowPressedChanged(BreadcrumbBar bar, object e)
        {
            bar.OnOverflowPressedChanged();
        }

        /// <summary>
        /// Occurs when the IsOverflowPressed property is changed.
        /// </summary>
        protected virtual void OnOverflowPressedChanged()
        {
            // rebuild the list of tracess to show in the popup of the overflow button:
            if (IsOverflowPressed)
            {
                BuildTraces();
            }
        }

        private void OnSelectedBreadcrumbPropertyChanged(BreadcrumbBar bar, AvaloniaPropertyChangedEventArgs e)
        {
            BreadcrumbItem selected = e.NewValue as BreadcrumbItem;
            bar.IsRootSelected = selected == bar.RootItem;
            if (bar.IsInitialized)
            {
                RoutedPropertyChangedEventArgs<BreadcrumbItem> args = new RoutedPropertyChangedEventArgs<BreadcrumbItem>(e.OldValue as BreadcrumbItem, e.NewValue as BreadcrumbItem, BreadcrumbBar.SelectedBreadcrumbChangedEvent);
                bar.RaiseEvent(args);
            }
        }

        private void OnSelectedItemPropertyChanged(BreadcrumbBar bar, AvaloniaPropertyChangedEventArgs e)
        {
            bar.OnSelectedItemChanged(bar, e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Occurs when the selected item of an embedded BreadcrumbItem is changed.
        /// </summary>
        /// <param name="oldvalue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSelectedItemChanged(BreadcrumbBar sender, object oldvalue, object newValue)
        {
        }

        private void OnRootPropertyChanged(BreadcrumbBar bar, AvaloniaPropertyChangedEventArgs e)
        {
            bar.OnRootChanged(bar, e.OldValue, e.NewValue);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            //if (Root == null)
            //{
            //    Root = Content as BreadcrumbItem;

            //}
        }


        /// <summary>
        /// Occurs when the Root property is changed.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnRootChanged(BreadcrumbBar bar, object oldValue, object newValue)
        {
            if (_isInitial == false)
            {
                newValue = GetFirstItem(newValue);
            }
            BreadcrumbItem oldRoot = oldValue as BreadcrumbItem;
            if (oldRoot != null)
            {
                oldRoot.IsRoot = false;
            }

            if (newValue == null)
            {
                RootItem = null;
                Path = null;
            }
            else
            {
                BreadcrumbItem root = newValue as BreadcrumbItem;
                if (root == null)
                {
                    if(_isInitial==false)
                    {
                        root = BreadcrumbItem.CreateItem(newValue);
                    }
                    else
                    {
                        root = BreadcrumbItem.CreateInitialItem(newValue);
                    }
                    
                }
                if (root != null)
                {
                    root.IsRoot = true;
                }
                if (oldValue != null)
                {
                    this.LogicalChildren.Remove((ILogical)oldValue);
                }
                RootItem = root;
                if (root != null)
                {

                    //bool result = LogicalTree.LogicalExtensions.GetLogicalParent(root as ILogical) == null;
                    ////if (LogicalTree.LogicalExtensions.GetLogicalParent(root as ILogical) == null)
                    //{
                    //    this.LogicalChildren.Remove(root);

                    //    this.LogicalChildren.Add(root);
                    //}
                }
                SelectedItem = root != null ? root.DataContext : null;
                if (IsInitialized)
                {
                    SelectedBreadcrumb = root;
                }
                else
                {
                    SelectedBreadcrumb = root;
                }
            }
        }

        /// <summary>
        /// Gets the first item of the specified value if it is a collection, otherwise it returns the value itself.
        /// </summary>
        /// <param name="entity">A collection, otherwise an object.</param>
        /// <returns>The first item of the collection, otherwise the entity.</returns>
        private object GetFirstItem(object entity)
        {
            ICollection c = entity as ICollection;
            if (c != null)
            {
                foreach (object item in c)
                {
                    return item;
                }
            }
            return entity;
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            comboBox = e.NameScope.Find<ComboBox>(partComboBox);
            rootButton = e.NameScope.Find<BreadcrumbButton>(partRoot);

            ToggleButton toggleButton = e.NameScope.Find<ToggleButton>("dropDown");
            //toggleButton.Checked += (o, e) =>
            //{
            //    if (toggleButton.IsChecked == true)
            //    {
            //        IsDropDownOpen = true;
            //    }
            //    else
            //    {
            //        IsDropDownOpen = false;
            //    }

            //};


            if (comboBox != null)
            {
                //comboBox.DropDownClosed += new EventHandler(comboBox_DropDownClosed);
                comboBox.PropertyChanged += ComboBox_PropertyChanged;

                //comboBox.IsKeyboardFocusWithinChanged += new DependencyPropertyChangedEventHandler(comboBox_IsKeyboardFocusWithinChanged);
                comboBox.KeyDown += comboBox_KeyDown;

                PseudoClasses.Set(":comboBoxIsVisible", comboBox.IsVisible);
            }
            if (rootButton != null)
            {
                rootButton.PointerPressed += rootButton_Click;
            }

            BreadcrumbItem breadcrumbItem = Content as BreadcrumbItem;



            _isInitial = true;
            RaisePropertyChanged(RootProperty, null,new BindingValue<object>( breadcrumbItem.Items));
            _isInitial = false;


            //RaisePropertyChanged(PathProperty, string.Empty, Path);

        }

        private void rootButton_Click(object sender, PointerPressedEventArgs e)
        {
            SetInputState();
        }

        bool _oldValue = true;
        private static bool _isInitial;

        private void ComboBox_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (e.Property.Name == nameof(ComboBox.IsVisible))
            {
                PseudoClasses.Set(":comboBoxIsVisible", comboBox.IsVisible);
            }


            if (e.Property.Name == nameof(ComboBox.IsDropDownOpen))
            {


                if (_oldValue == comboBox.IsDropDownOpen)
                    return;


                if (comboBox.IsDropDownOpen == false)
                {
                    comboBox_DropDownClosed(comboBox, EventArgs.Empty);
                }
                else
                {

                }

                _oldValue = comboBox.IsDropDownOpen;
            }
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            IsDropDownOpen = false;
            Path = comboBox.SelectedItem.ToString();//todo check
        }

        private void comboBox_KeyDown(object sender, Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Input.Key.Escape: Exit(false); break;
                case Input.Key.Enter: Exit(true); break;
                default: return;
            }
            e.Handled = true;
        }

        private void OnMouseDown(object sender, PointerPressedEventArgs e)
        {
            if (e.Handled) return;

            var prop = e.GetCurrentPoint(this).Properties;

            if (prop.IsLeftButtonPressed)
            {
                e.Handled = true;
                SetInputState();
            }
        }

        private void SetInputState()
        {
            if (comboBox != null && IsEditable)
            {
                //comboBox.Text = Path;
                comboBox.IsVisible = true;
                comboBox.Focus();
            }
        }

        /// <summary>
        /// Gets the edit path from the tracess of the BreacrumbItems.
        /// </summary>
        /// <returns></returns>
        public string GetEditPath()
        {
            string displayPath = GetDisplayPath();
            PathConversionEventArgs e = new PathConversionEventArgs(PathConversionEventArgs.ConversionMode.DisplayToEdit, displayPath, Root, PathConversionEvent);
            RaiseEvent(e);
            return e.EditPath;
        }

        /// <summary>
        /// Gets the path of the specified BreadcrumbItem.
        /// </summary>
        /// <param name="item">The BreadrumbItem for which to determine the path.</param>
        /// <returns>The path of the BreadcrumbItem which is the concenation of all Traces from all selected breadcrumbs.</returns>
        public string PathFromBreadcrumbItem(BreadcrumbItem item)
        {
            StringBuilder sb = new StringBuilder();
            while (item != null)
            {
                if (item == RootItem && sb.Length > 0) break;
                if (sb.Length > 0) sb.Insert(0, SeparatorString);
                sb.Insert(0, item.TraceValue);
                item = item.ParentBreadcrumbItem;
            }
            PathConversionEventArgs e = new PathConversionEventArgs(PathConversionEventArgs.ConversionMode.DisplayToEdit, sb.ToString(), Root, PathConversionEvent);
            RaiseEvent(e);
            return e.EditPath;
        }

        /// <summary>
        /// Gets the display path from the traces of the BreacrumbItems.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayPath()
        {
            string separator = SeparatorString;
            StringBuilder sb = new StringBuilder();
            BreadcrumbItem item = RootItem;
            int index = 0;
            while (item != null)
            {
                if (sb.Length > 0) sb.Append(separator);
                if (index >= BreadcrumbsToHide || item.SelectedItem == null)
                {
                    sb.Append(item.GetTracePathValue());
                }
                index++;
                item = item.SelectedBreadcrumb;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Do what's necassary to do when the BreadcrumbBar has lost focus.
        /// </summary>
        private void Exit(bool updatePath)
        {
            if (comboBox != null)
            {
                if (updatePath && comboBox.IsVisible)
                {
                    Path = comboBox.SelectedItem.ToString();
                }
                comboBox.IsVisible = false;
            }
        }

        public void AddChild(object value)
        {
            this.Root = value;
        }

        public void AddText(string text)
        {
            AddChild(text);
        }
    }
}