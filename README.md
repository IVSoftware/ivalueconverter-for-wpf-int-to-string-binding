Your question is **When to use an enumerated in C#* and one thing to consider is whether assigning a "Human-Readable" value provides added value. One might ask if it's worth it in this case to enumberate `PriorityLevel.LEVEL1` if it's just going to be 1 anyway.

Your question mentions that you have a binding to a UI element, and your goal is to display (for example) the values of 1, 2, 3 as "Level 1", "Level 2", and "Level 3" in response to changes in the `PriorityLevel` property. 

One approach is to make an implementation of `IValueConverter` class:

    // Returns a formatted version of level, which can be an int or an enum
    public class PriorityLevelToFormatted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => $"Level {value}";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException("Unused");
    }

This converter can be invoked in the xaml as Text="{Binding Path=PriorityLevel, Converter={StaticResource PriorityLevelConverter}}"

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