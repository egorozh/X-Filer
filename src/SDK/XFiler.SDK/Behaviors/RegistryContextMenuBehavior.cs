using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace XFiler.SDK;

public sealed class RegistryContextMenuBehavior : Behavior<ContextMenu>
{
    #region Attached Properties

    public static readonly DependencyProperty RootItemProperty = DependencyProperty.RegisterAttached(
        "RootItem", typeof(bool), typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(false));

    public static void SetRootItem(DependencyObject element, bool value)
        => element.SetValue(RootItemProperty, value);

    public static bool GetRootItem(DependencyObject element)
        => (bool) element.GetValue(RootItemProperty);

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty NativeContextMenuLoaderProperty = DependencyProperty.Register(
        nameof(NativeContextMenuLoader), typeof(INativeContextMenuLoader),
        typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(default(INativeContextMenuLoader), PropsChangedCallback));

    public static readonly DependencyProperty FileInfoModelProperty = DependencyProperty.Register(
        nameof(FileInfoModel), typeof(IFileSystemModel),
        typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(default(IFileSystemModel), PropsChangedCallback));

    private static void PropsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RegistryContextMenuBehavior behavior)
        {
            if (behavior.NativeContextMenuLoader != null && behavior.FileInfoModel != null)
                behavior.LoadNativeContextMenuItems(behavior.NativeContextMenuLoader, behavior.FileInfoModel);
        }
    }

    #endregion

    #region Public Properties

    public INativeContextMenuLoader? NativeContextMenuLoader
    {
        get => (INativeContextMenuLoader) GetValue(NativeContextMenuLoaderProperty);
        set => SetValue(NativeContextMenuLoaderProperty, value);
    }

    public IFileSystemModel? FileInfoModel
    {
        get => (IFileSystemModel) GetValue(FileInfoModelProperty);
        set => SetValue(FileInfoModelProperty, value);
    }

    #endregion

    #region Private Methods

    private void LoadNativeContextMenuItems(INativeContextMenuLoader loader, IFileSystemModel fileInfo)
    {
        ClearOldItems();

        var rootItem = AssociatedObject.Items.OfType<MenuItem>()
            .FirstOrDefault(GetRootItem);

        var index = AssociatedObject.Items.Count;

        if (rootItem != null)
            index = AssociatedObject.Items.IndexOf(rootItem) + 1;

        var models = loader.CreateMenuItems(new[] {fileInfo.Info.FullName});

        foreach (var model in models)
            AssociatedObject.Items.Insert(index++, CreateRegistryMenuItem(loader, model));

        AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
    }

    private void ClearOldItems()
    {
        if (AssociatedObject.Items is { } items)
        {
            var oldItems = items.OfType<MenuItem>()
                .Where(i => i.DataContext is IRegistryContextMenuModel)
                .ToList();

            foreach (var oldItem in oldItems) 
                AssociatedObject.Items.Remove(oldItem);
        }
    }

    private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;

        NativeContextMenuLoader?.DisposeContextMenu();
    }

    private static MenuItem CreateRegistryMenuItem(INativeContextMenuLoader loader, IRegistryContextMenuModel model)
    {
        List<MenuItem>? subItems = null;
        Image? icon = null;

        if (model.Children != null)
        {
            subItems = model.Children
                .Select(child => CreateRegistryMenuItem(loader, child))
                .ToList();
        }

        if (model.Icon != null)
        {
            icon = new Image
            {
                Source = model.Icon
            };
        }

        MenuItem item = new()
        {
            Header = model.Name,
            DataContext = model,
            Command = loader.InvokeCommand,
            CommandParameter = model,
            ItemsSource = subItems,
            Icon = icon
        };

        return item;
    }

    #endregion
}