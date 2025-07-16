using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kur
{
    /// <summary>
    /// Логика взаимодействия для informaciyy.xaml
    /// </summary>
    public partial class informaciyy : Window
    {
        // инициализация менеджера для навигации
        public informaciyy()
        {
            InitializeComponent();
            MainFrame.Navigate(new slova());
            Manager.MainFrame = MainFrame;
        }

        // обработчики событий для кнопок навигации
        private void slovaClick_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new slova());
        }
        private void rifmiClick_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new rifmi());
        }
        private void tipClick_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserPage());
        }
        // обработчик рендеринга контента в главном фрейме
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                bbtn.Visibility = Visibility.Visible;
            }
            else
            {
                bbtn.Visibility = Visibility.Hidden;
            }
        }
        // обработчик кнопки "Назад" для возврата к предыдущему контенту
        private void bbtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
