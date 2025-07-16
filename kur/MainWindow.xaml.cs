using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Windows;

namespace kur
{
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptions<Entities> _dbOptions;

        public MainWindow()
        {
            // Инициализация компонентов и конфигурации
            InitializeComponent();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _dbOptions = new DbContextOptionsBuilder<Entities>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;
        }
        // Обработчик события регистрации пользователя
        private async void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что поля логина и пароля заполнены
                string log = BoxLog.Text.Trim();
                string pass = BoxPassword.Password.Trim();

                if (log.Length < 4 || pass.Length < 8)
                {
                    MessageBox.Show("Слишком короткий Логин/Пароль");
                    return;
                }

                // Создаем новый контекст для каждой операции
                await using (var entities = new Entities(_dbOptions, _configuration))
                {
                    var user = await entities.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Login == log && p.Password == pass);

                    // Проверяем, существует ли пользователь с таким логином
                    if (user != null)
                    {
                        MessageBox.Show("Такой пользователь уже зарегистрирован!");
                        return;
                    }
                    var newUser = new Users { Login = log, Password = pass, Role = 0 };
                    entities.Users.Add(newUser);
                    // Сохраняем изменения в базе данных
                    await entities.SaveChangesAsync();
                    MessageBox.Show("Добро пожаловать, " + newUser.Login);

                    new UserWindow().Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        // Обработчик события закрытия окна
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Обработчик события входа пользователя
        private async void BtnVhod_Click(object sender, RoutedEventArgs e)
        {
            string log = BoxLog.Text.Trim();
            string pass = BoxPassword.Password.Trim();
            // Проверяем, что поля логина и пароля заполнены
            if (string.IsNullOrEmpty(log) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            // Создаем новый контекст для каждой операции
            await using (var entities = new Entities(_dbOptions, _configuration))
            {
                var user = await entities.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Login == log && p.Password == pass);
                // Проверяем, существует ли пользователь с таким логином и паролем
                if (user != null)
                {
                    MessageBox.Show("Вы вошли как " + user.Login);
                    if (user.Role == 1)
                    {
                        new informaciyy().Show();
                        this.Close();
                    }
                    else
                    {
                        new UserWindow().Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Вы ввели неверный Логин/Пароль");
                }
            }
        }
    }
}