using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Wpf.Ui.Controls;

namespace kur
{
    public partial class UserWindow : Window
    {
        private readonly IConfiguration _configuration;
        private CancellationTokenSource _searchCancellationTokenSource;
        private readonly Dictionary<string, List<RhymeResult>> _rhymeCache = new();
        private readonly Dictionary<string, List<Phonemes>> _phonemeCache = new();
        private bool _isTyping;
        private Border _gradientShadow;
        private Border _inputCard;
        private Border _rhymesCard;
        private System.Windows.Controls.TextBox _txtInputWord;
        private ListBox _lbRhymes;
        private Wpf.Ui.Controls.Button _btnSearchRhymes;
        private Effect _shadowEffect;
        // Конструктор класса UserWindow
        public UserWindow()
        {
            InitializeComponent();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            SetupUI();
        }
        // Метод для настройки пользовательского интерфейса
        private void SetupUI()
        {
            // Проверяем, если Content уже установлен, чтобы избежать повторной инициализации
            if (Content == null)
            {
                // Настройка свойств окна
                Title = "Окно пользователя";
                Height = 600;
                Width = 1000;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ResizeMode = ResizeMode.NoResize;
                FontFamily = new FontFamily("Segoe UI");
                Background = new SolidColorBrush(Color.FromRgb(245, 243, 255));

                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Content = grid;
                // Создание градиентной тени и эффектов размытия
                _gradientShadow = new Border
                {
                    Width = 500,
                    Height = 260,
                    CornerRadius = new CornerRadius(40),
                    Opacity = 0,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 0),
                    Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1),
                        GradientStops = new GradientStopCollection
                {
                    new GradientStop(Color.FromRgb(138, 92, 245), 0), // Фиолетовый
                    new GradientStop(Color.FromRgb(167, 139, 250), 0.5), // Светло-фиолетовый
                    new GradientStop(Color.FromRgb(199, 210, 254), 1) // Светло-голубой
                }
                    }
                };
                _shadowEffect = new BlurEffect // Синхронизировано с XAML
                {
                    Radius = 150, // Радиус размытия
                    KernelType = KernelType.Gaussian,
                    RenderingBias = RenderingBias.Quality
                };
                _gradientShadow.Effect = _shadowEffect;
                Grid.SetRow(_gradientShadow, 0);
                grid.Children.Add(_gradientShadow);

                _inputCard = new Border
                {
                    Width = 450,
                    CornerRadius = new CornerRadius(40), // Синхронизировано с XAML
                    Background = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        Opacity = 0.15,
                        BlurRadius = 6,
                        ShadowDepth = 0
                    },
                    RenderTransform = new ScaleTransform(1, 1)
                };
                // Добавляем карточку ввода в сетку
                Grid.SetRow(_inputCard, 0);
                grid.Children.Add(_inputCard);

                var stackPanel = new StackPanel { Margin = new Thickness(25) };
                _inputCard.Child = stackPanel;
                // Создание заголовка и элементов управления ввода
                var titleText = new System.Windows.Controls.TextBlock
                {
                    Text = "Найти рифму",
                    Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                    FontSize = 24,
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 15),
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        Opacity = 0.1,
                        BlurRadius = 4,
                        ShadowDepth = 0
                    }
                };
                //  Добавляем заголовок в стек-панель
                stackPanel.Children.Add(titleText);

                // Создание текстового поля для ввода слова
                _txtInputWord = new System.Windows.Controls.TextBox
                {
                    Name = "txtInputWord",
                    Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                    FontSize = 15,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204)),
                    BorderThickness = new Thickness(1.5),
                    Padding = new Thickness(12, 10, 12, 10),
                    Background = Brushes.White
                };
                // Синхронизировано с XAML
                _txtInputWord.TextChanged += TxtInputWord_TextChanged;
                stackPanel.Children.Add(_txtInputWord);

                _btnSearchRhymes = new Wpf.Ui.Controls.Button
                {
                    Content = "Поиск рифм",
                    Background = new SolidColorBrush(Color.FromRgb(138, 92, 245)),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14,
                    Width = 160,
                    Height = 50,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                _btnSearchRhymes.Click += BtnSearchRhymes_Click;
                ApplyButtonAnimations();
                stackPanel.Children.Add(_btnSearchRhymes);
                // Создание карточки для отображения рифм
                _rhymesCard = new Border
                {
                    Name = "RhymesCard",
                    Width = 450,
                    CornerRadius = new CornerRadius(40),
                    Background = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20),
                    Visibility = Visibility.Collapsed,
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        Opacity = 0.15,
                        BlurRadius = 6,
                        ShadowDepth = 0
                    },
                    RenderTransform = new ScaleTransform(1, 1)
                };
                Grid.SetRow(_rhymesCard, 1);
                grid.Children.Add(_rhymesCard);

                _lbRhymes = new ListBox
                {
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    MaxHeight = 350,
                    Margin = new Thickness(15)
                };
                _lbRhymes.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                _rhymesCard.Child = _lbRhymes;
            }
            else
            {
                _gradientShadow = (Border)FindName("GradientShadow");
                _inputCard = (Border)FindName("InputCard");
                _rhymesCard = (Border)FindName("RhymesCard");
                _txtInputWord = (System.Windows.Controls.TextBox)FindName("txtInputWord");
                _lbRhymes = (ListBox)FindName("lbRhymes");
                _btnSearchRhymes = (Wpf.Ui.Controls.Button)FindName("btnSearchRhymes");
                _shadowEffect = _gradientShadow.Effect;
                ApplyButtonAnimations();
            }

            Loaded += (s, e) => StartInitialAnimations();
        }
        // Метод для запуска начальных анимаций
        private void StartInitialAnimations()
        {
            var cardOpacity = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4));
            var cardScaleY = new DoubleAnimation(0.95, 1, TimeSpan.FromSeconds(0.4));
            _inputCard.BeginAnimation(UIElement.OpacityProperty, cardOpacity);
            _inputCard.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, cardScaleY);
        }
        // Метод для запуска анимации пульсации тени
        private void StartShadowPulseAnimation()
        {
            if (_gradientShadow.Effect == null)
            {
                _gradientShadow.Effect = _shadowEffect;
            }

            // Анимация прозрачности и размытия тени
            var shadowOpacity = new DoubleAnimation(0.4, 0.8, TimeSpan.FromSeconds(2))
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            var shadowBlur = new DoubleAnimation(100, 150, TimeSpan.FromSeconds(2)) // Анимация радиуса размытия
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            // Применяем анимации к градиентной тени
            _gradientShadow.BeginAnimation(UIElement.OpacityProperty, shadowOpacity);
            if (_gradientShadow.Effect is BlurEffect blurEffect)
            {
                blurEffect.BeginAnimation(BlurEffect.RadiusProperty, shadowBlur);
            }
            // Анимация градиентной тени
            if (_gradientShadow.Background is LinearGradientBrush gradient)
            {
                // Анимация цветов градиента
                var color1 = new ColorAnimation(Color.FromRgb(138, 92, 245), Color.FromRgb(167, 139, 250), TimeSpan.FromSeconds(2))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                var color2 = new ColorAnimation(Color.FromRgb(167, 139, 250), Color.FromRgb(199, 210, 254), TimeSpan.FromSeconds(2))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                var color3 = new ColorAnimation(Color.FromRgb(199, 210, 254), Color.FromRgb(138, 92, 245), TimeSpan.FromSeconds(2))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                // Применяем анимации к градиентным стопам
                gradient.GradientStops[0].BeginAnimation(GradientStop.ColorProperty, color1);
                gradient.GradientStops[1].BeginAnimation(GradientStop.ColorProperty, color2);
                gradient.GradientStops[2].BeginAnimation(GradientStop.ColorProperty, color3);
            }
        }
        // Метод для остановки анимации тени
        private void StopShadowAnimation()
        {
            // Проверяем, что анимация тени запущена
            if (!_isTyping)
            {
                // Остановка анимации тени
                var fadeOut = new DoubleAnimation(_gradientShadow.Opacity, 0, TimeSpan.FromSeconds(0.5));
                fadeOut.Completed += (s, e) =>
                {
                    // Очищаем анимации и эффекты
                    _gradientShadow.BeginAnimation(UIElement.OpacityProperty, null);
                    if (_gradientShadow.Effect is BlurEffect blurEffect)
                    {
                        blurEffect.BeginAnimation(BlurEffect.RadiusProperty, null);
                    }

                    if (_gradientShadow.Background is LinearGradientBrush gradient)
                    {
                        gradient.GradientStops[0].BeginAnimation(GradientStop.ColorProperty, null);
                        gradient.GradientStops[1].BeginAnimation(GradientStop.ColorProperty, null);
                        gradient.GradientStops[2].BeginAnimation(GradientStop.ColorProperty, null);
                    }
                };
                _gradientShadow.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
        }
        // Метод для запуска анимации карточки рифм
        private void StartRhymesCardAnimation()
        {
            var cardOpacity = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4));
            var cardScaleY = new DoubleAnimation(0.95, 1, TimeSpan.FromSeconds(0.4));
            _rhymesCard.BeginAnimation(UIElement.OpacityProperty, cardOpacity);
            _rhymesCard.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, cardScaleY);
        }
        // Метод для запуска анимации элементов списка рифм
        private void StartListItemAnimation(ListBoxItem item)
        {
            item.RenderTransform = new TranslateTransform(0, 10);
            var opacity = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            var translateY = new DoubleAnimation(10, 0, TimeSpan.FromSeconds(0.3));
            item.BeginAnimation(UIElement.OpacityProperty, opacity);
            item.RenderTransform.BeginAnimation(TranslateTransform.YProperty, translateY);
        }
        // Метод для применения анимаций к кнопке поиска рифм
        private void ApplyButtonAnimations()
        {
            // Применяем эффекты наведения и нажатия к кнопке поиска рифм
            _btnSearchRhymes.MouseEnter += (s, e) =>
            {
                _btnSearchRhymes.Background = new SolidColorBrush(Color.FromRgb(167, 139, 250));
                _btnSearchRhymes.Effect = new DropShadowEffect
                {
                    Color = Color.FromRgb(138, 92, 245),
                    Opacity = 0.3,
                    BlurRadius = 8,
                    ShadowDepth = 0
                };
            };
            _btnSearchRhymes.MouseLeave += (s, e) =>
            {
                _btnSearchRhymes.Background = new SolidColorBrush(Color.FromRgb(138, 92, 245));
                _btnSearchRhymes.Effect = null;
            };
            _btnSearchRhymes.PreviewMouseDown += (s, e) =>
            {
                _btnSearchRhymes.Background = new SolidColorBrush(Color.FromRgb(124, 58, 237));
            };
            _btnSearchRhymes.PreviewMouseUp += (s, e) =>
            {
                _btnSearchRhymes.Background = new SolidColorBrush(Color.FromRgb(167, 139, 250));
            };
        }
        // Обработчик события изменения текста в поле ввода слова
        private async void TxtInputWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isTyping)
            {
                _isTyping = true;
                StartShadowPulseAnimation();
            }
            await SearchRhymesAsync();
        }
        // Обработчик события нажатия клавиши Enter в поле ввода слова
        private async Task SearchRhymesAsync()
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Задержка перед началом поиска, чтобы избежать слишком частых запросов
                await Task.Delay(300, _searchCancellationTokenSource.Token);
                string inputWord = _txtInputWord.Text.ToLower().Trim();

                if (string.IsNullOrWhiteSpace(inputWord))
                {
                    ClearRhymes("Введите слово.");
                    return;
                }
                // Проверяем кэш рифм

                if (_rhymeCache.TryGetValue(inputWord, out var cachedRhymes))
                {
                    DisplayRhymes(cachedRhymes);
                    return;
                }
                // Проверяем кэш фонем
                using var entities = CreateDbContext();
                var rhymingWords = await FindRhymesInRhymesTableAsync(entities, inputWord);

                if (rhymingWords.Count == 0)
                {
                    var phonemes = await GetPhonemesForWordAsync(entities, inputWord);
                    if (phonemes.Count == 0)
                    {
                        ClearRhymes("Фонемы для слова не найдены.");
                        return;
                    }
                    // Если фонемы найдены, ищем рифмы по фонемам
                    int matchCount = Math.Min(3, phonemes.Count);
                    rhymingWords = await FindRhymingWordsByPhonemesAsync(entities, inputWord, phonemes, matchCount);
                }
                // Если рифмы не найдены, выводим сообщение
                _rhymeCache[inputWord] = rhymingWords;
                DisplayRhymes(rhymingWords);
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при поиске рифм: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Остановка анимации тени, если поле ввода пустое
            finally
            {
                _isTyping = false;
                if (string.IsNullOrWhiteSpace(_txtInputWord.Text))
                {
                    StopShadowAnimation();
                }
            }
        }

        // Метод для очистки списка рифм и отображения сообщения
        private void ClearRhymes(string message)
        {
            _rhymesCard.Visibility = Visibility.Collapsed;
            _lbRhymes.Items.Clear();
            _lbRhymes.Items.Add(message);
            if (string.IsNullOrWhiteSpace(_txtInputWord.Text))
            {
                StopShadowAnimation();
            }
        }
        // Метод для отображения рифм в ListBox
        private void DisplayRhymes(List<RhymeResult> rhymes)
        {
            // Очищаем предыдущие рифмы
            _lbRhymes.Items.Clear();

            if (rhymes.Count == 0)
            {
                ClearRhymes("Рифмы не найдены.");
                return;
            }
            // Проверяем, видима ли карточка рифм, и запускаем анимацию, если она не видима
            if (_rhymesCard.Visibility != Visibility.Visible)
            {
                _rhymesCard.Visibility = Visibility.Visible;
                StartRhymesCardAnimation();
            }
            // Добавляем рифмы в ListBox
            foreach (var rhyme in rhymes)
            {
                // Создаем новый элемент ListBoxItem для каждой рифмы
                var item = new ListBoxItem
                {
                    Content = $"{rhyme.Word} ({rhyme.RhymeType})",
                    Background = Brushes.Transparent,
                    Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 15,
                    Padding = new Thickness(12),
                    Margin = new Thickness(5, 3, 5, 3),
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                // Добавляем эффекты наведения и нажатия на элемент списка
                item.MouseEnter += (s, e) => item.Background = new SolidColorBrush(Color.FromRgb(237, 233, 254));
                item.MouseLeave += (s, e) => item.Background = item.IsSelected ? new SolidColorBrush(Color.FromRgb(138, 92, 245)) : Brushes.Transparent;
                item.Selected += (s, e) =>
                {
                    item.Background = new SolidColorBrush(Color.FromRgb(138, 92, 245));
                    item.Foreground = Brushes.White;
                };
                item.Unselected += (s, e) =>
                {
                    item.Background = Brushes.Transparent;
                    item.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                };
                _lbRhymes.Items.Add(item);
                StartListItemAnimation(item);
            }
        }
        // Класс для хранения результатов рифм
        public class RhymeResult
        {
            public string Word { get; set; }
            public string RhymeType { get; set; }
        }
        // Класс для хранения фонем
        private async Task<List<RhymeResult>> FindRhymesInRhymesTableAsync(Entities entities, string inputWord)
        {
            var result = new List<RhymeResult>();
            var input = await entities.Words.FirstOrDefaultAsync(w => w.Word == inputWord);
            // Если слово не найдено, возвращаем пустой список
            if (input == null) return result;
            // Получаем фонемы для входного слова
            var rhymes = await entities.Rhymes
                .Where(r => r.WordId == input.Id)
                .Take(50)
                .ToListAsync();
            // Если рифмы не найдены, возвращаем пустой список
            foreach (var rhyme in rhymes)
            {
                var rhymingWord = await entities.Words.FirstOrDefaultAsync(w => w.Id == rhyme.RhymeId);
                if (rhymingWord == null) continue;

                var rhymeType = await entities.RhymeTypes.FirstOrDefaultAsync(rt => rt.Id == rhyme.RhymeTypeId);
                result.Add(new RhymeResult
                {
                    Word = rhymingWord.Word,
                    RhymeType = rhymeType?.RhymeName ?? "неизвестный"
                });
            }

            return result;
        }
        // Метод для поиска рифм по фонемам
        private async Task<List<RhymeResult>> FindRhymingWordsByPhonemesAsync(Entities entities, string inputWord, List<Phonemes> inputPhonemes, int matchCount)
        {
            var result = new List<RhymeResult>();
            var allWords = await entities.Words
                .Where(w => w.Word != inputWord)
                .Take(100)
                .ToListAsync();
            // Получаем фонемы для входного слова
            foreach (var word in allWords)
            {
                var phonemes = await GetPhonemesForWordAsync(entities, word.Word);

                if (phonemes.Count < matchCount || inputPhonemes.Count < matchCount)
                    continue;
                // Проверяем, совпадают ли последние matchCount фонемы
                bool match = true;
                for (int i = 0; i < matchCount; i++)
                {
                    if (phonemes[phonemes.Count - 1 - i].Id != inputPhonemes[inputPhonemes.Count - 1 - i].Id)
                    {
                        match = false;
                        break;
                    }
                }
                // Если совпадают, добавляем слово в результат
                if (match)
                {
                    result.Add(new RhymeResult { Word = word.Word, RhymeType = "точная" });
                }
            }

            return result;
        }
        // Метод для получения фонем для слова из базы данных
        private async Task<List<Phonemes>> GetPhonemesForWordAsync(Entities entities, string word)
        {
            if (_phonemeCache.TryGetValue(word, out var cached))
                return cached;

            var result = new List<Phonemes>();
            // Проверяем, что слово не пустое
            foreach (char c in word.ToUpper())
            {
                var phoneme = await entities.Phonemes.FirstOrDefaultAsync(p => p.Phoneme == c.ToString());
                if (phoneme != null)
                    result.Add(phoneme);
            }

            _phonemeCache[word] = result;
            return result;
        }
        // Обработчик события нажатия кнопки "Поиск рифм"
        private async void BtnSearchRhymes_Click(object sender, RoutedEventArgs e)
        {
            await SearchRhymesAsync();
        }
        // Метод для создания контекста базы данных
        private Entities CreateDbContext()
        {
            // Создаем новый контекст базы данных с использованием конфигурации
            var options = new DbContextOptionsBuilder<Entities>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;

            return new Entities(options, _configuration);
        }
    }
}