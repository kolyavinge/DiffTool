using System.Windows;
using System.Windows.Input;

namespace Interactive;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowModel();
    }

    private void VerticalScrollBarOnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        //verticalScrollBar.Value -= e.Delta;
    }
}
