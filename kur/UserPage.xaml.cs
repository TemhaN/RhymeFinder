using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace kur
{
    public partial class UserPage : Page
    {
        private readonly IConfiguration _configuration;
        private Users _selectedUser;
        private CancellationTokenSource _searchCancellationTokenSource;
        private Task _currentSearchTask;

        // Конструктор класса UserPage
        public UserPage()
        {
            InitializeComponent();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            Loaded += UserPage_Loaded;
        }

        // Обработчик события загрузки страницы
        private async void UserPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUsersAsync();
            var roles = new List<RoleItem>
            {
                new RoleItem { RoleValue = 0, DisplayName = "Пользователь" },
                new RoleItem { RoleValue = 1, DisplayName = "Администратор" }
            };
            comborole.ItemsSource = roles;
        }

        // Метод для загрузки пользователей из базы данных
        private async Task LoadUsersAsync()
        {
            using var entities = CreateDbContext();
            ulist.ItemsSource = await entities.Users.ToListAsync();
        }
        // Метод для создания контекста базы данных
        private void ulist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedUser = (Users)ulist.SelectedItem;
            if (_selectedUser != null)
            {
                usernametext.Text = _selectedUser.Login;
                passwordtext.Password = _selectedUser.Password;
                var selectedRole = ((List<RoleItem>)comborole.ItemsSource).FirstOrDefault(r => r.RoleValue == _selectedUser.Role);
                comborole.SelectedItem = selectedRole;
            }
        }

        // Обработчик события нажатия кнопки "Сохранить"
        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(usernametext.Text) || string.IsNullOrEmpty(passwordtext.Password))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем, что выбрана роль
            var selectedRole = (RoleItem)comborole.SelectedItem;
            if (selectedRole == null)
            {
                MessageBox.Show("Выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Создаем новый контекст базы данных и сохраняем пользователя
                using var entities = CreateDbContext();
                if (_selectedUser == null)
                {
                    _selectedUser = new Users
                    {
                        Login = usernametext.Text,
                        Password = passwordtext.Password,
                        Role = selectedRole.RoleValue
                    };
                    entities.Users.Add(_selectedUser);
                    MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Если пользователь уже выбран, обновляем его данные
                else
                {
                    var user = await entities.Users.FindAsync(_selectedUser.Id);
                    if (user != null)
                    {
                        user.Login = usernametext.Text;
                        user.Password = passwordtext.Password;
                        user.Role = selectedRole.RoleValue;
                        MessageBox.Show("Пользователь успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                await entities.SaveChangesAsync();
                await LoadUsersAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Обработчик события нажатия кнопки "Удалить"
        private async void del_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли пользователь для удаления
            if (_selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Подтверждение удаления
            try
            {
                using var entities = CreateDbContext();
                var user = await entities.Users.FindAsync(_selectedUser.Id);
                if (user != null)
                {
                    // Удаляем пользователя из базы данных
                    entities.Users.Remove(user);
                    await entities.SaveChangesAsync();
                    await LoadUsersAsync();
                    ClearFields();
                    MessageBox.Show("Пользователь успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик события нажатия кнопки "Очистить"
        private void cleanb_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        // Метод для очистки полей ввода
        private void ClearFields()
        {
            usernametext.Text = string.Empty;
            passwordtext.Password = string.Empty;
            comborole.SelectedItem = null;
            _selectedUser = null;
            ulist.SelectedItem = null;
        }

        // Обработчик события изменения текста в поле поиска
        private async void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Отменяем предыдущий поиск, если он еще выполняется
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();

            // Запускаем новый поиск в фоновом режиме
            try
            {
                // Задержка для предотвращения слишком частых запросов к базе данных
                await Task.Delay(300, _searchCancellationTokenSource.Token);
                string searchText = txtSearch.Text.ToLower();
                using var entities = CreateDbContext();
                var filteredUsers = await entities.Users
                    .Where(u => u.Login.ToLower().Contains(searchText))
                    .ToListAsync(_searchCancellationTokenSource.Token);
                ulist.ItemsSource = filteredUsers;
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик события изменения выбранной роли в комбобоксе
        private void comborole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        // Метод для создания контекста базы данных с использованием конфигурации
        private Entities CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<Entities>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;
            return new Entities(options, _configuration);
        }
        // Вспомогательный класс для представления ролей в комбобоксе
        public class RoleItem
        {
            public int RoleValue { get; set; }
            public string DisplayName { get; set; }
        }
    }
}