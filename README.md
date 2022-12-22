Your question is **When to use an enumerated in C#** and one way to look at this is asking whether assigning a human-readable alias for a value provides some kind of benefit (or not). 

Your question states that you have a binding of the `PriorityLevel` (expressed as an enum or possibly just an int) to a UI element in the XAML.

> My problem is when I show that property in the interface (**with a binding in the XAML**), it appears as (obviously) "LEVEL1" or "LEVEL2" or "LEVEL3". I'm interested in knowing how to display "LEVEL 1" (with a space) instead of displaying "LEVEL1".

As I understand it, your goal is to display (for example) the values of 1, 2, 3 as "Level 1", "Level 2", and "Level 3" in response to changes in the `PriorityLevel` property. One xaml-friendly approach that will work with either an `int` _or_ an `enum`is to make an implementation of `IValueConverter` class and invoke it in the xaml as `Text="{Binding Path=PriorityLevel, Converter={StaticResource PriorityLevelConverter}}"`.

    // Returns a formatted version of PriorityLevel.
    // Works for EITHER int or enum
    public class PriorityLevelToFormatted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => $"Level {(int)value}";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException("Unused");
    }

I [tested](https://github.com/IVSoftware/ivalueconverter-for-wpf-int-to-string-binding.git) this answer with a minimal WPF form that handles a button click by advancing the `PriorityLevel`.

    <iv:Window x:Class="wpf_window_ex.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
            xmlns:local="clr-namespace:wpf_window_ex"
            xmlns:iv="clr-namespace:IVSoftware"
            mc:Ignorable="d"
            Title="Main Window" Height="200" Width="300" WindowStartupLocation="CenterScreen">
        <Window.Resources>
            <local:PriorityLevelToFormatted x:Key="PriorityLevelConverter"/>
        </Window.Resources>
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="*"/>
                <RowDefinition Height="40"/>
                <RowDefinition Height="40"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>
            <TextBlock 
                Name ="textLevel"
                Text="{Binding Path=PriorityLevel, Converter={StaticResource PriorityLevelConverter}}"
                VerticalAlignment="Center"
                HorizontalAlignment="Center"
                Grid.Row="1"/>
            <Button
                Command="{Binding AdvancePriorityCommand}"
                VerticalAlignment="Center"
                HorizontalAlignment="Center"
                Width="150"
                Grid.Row="2">
                <TextBlock>Advance Level</TextBlock>
            </Button>
        </Grid>
    </iv:Window>

[![click response][1]][1]


  [1]: https://i.stack.imgur.com/yvX1C.png