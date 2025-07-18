# RhymeFinder

🔥 Мощное WPF-приложение на C# для поиска рифм, управления словами и фонемами. Поддерживает авторизацию с ролями, кэширование и красивый интерфейс на Wpf.Ui. База — Microsoft SQL Server, ORM — Entity Framework Core.

## ⚙ Основной функционал

- 🔐 **Авторизация с ролями** (Пользователь / Администратор)
- 📚 **Управление словами**:
  - Добавление, редактирование, удаление слов с ударением
  - Импорт из текстового файла (`слово [позиция_ударения]`)
  - Автоматическое разбиение на фонемы
- 👤 **Управление пользователями** (для админа):
  - Создание, редактирование, удаление, поиск
  - Назначение ролей
- 🎯 **Поиск рифм**:
  - Фонемный анализ
  - Кэширование результатов
  - Отображение рифм с типом (например, "точная")
- 💄 **UI и анимации**:
  - Современный дизайн (WPF + Wpf.Ui)
  - Градиенты, тени, плавные переходы и прочие вкусняшки

## 🧰 Требования

| Компонент | Версия |
|----------|--------|
| ОС       | Windows 10/11 |
| .NET     | 6.0+    |
| SQL      | Microsoft SQL Server |
| UI       | WPF + Wpf.Ui |
| ORM      | EF Core |

## 📦 Зависимости

- `EntityFrameworkCore`
- `Wpf.Ui`
- `DocumentFormat.OpenXml`
- `Microsoft.Extensions.Configuration`
- `Microsoft.VisualBasic`

## 🚀 Установка и запуск

1. **Клонируй репу**  
   ```bash
   git clone https://github.com/TemhaN/RhymeFinder.git
   cd RhymeFinder

2. **Настрой БД**

   * Создай базу `RhymeFinderDB`
   * Обнови `appsettings.json`:

     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=localhost;Database=RhymeFinderDB;Trusted_Connection=True;"
       }
     }
     ```

3. **Применяй миграции**

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Ставь зависимости**

   ```bash
   dotnet restore
   ```

5. **Собери и запусти**

   ```bash
   dotnet build
   dotnet run
   ```

## 👨‍💻 Использование

* **Вход**: логин + пароль
* **Управление словами**:

  * Добавление вручную или импорт (`слово 2`)
  * Таблица со всеми словами, поиск, удаление
* **Пользователи**:

  * Создание, назначение ролей, редактирование
  * Быстрый поиск по логину
* **Поиск рифм**:

  * Ввод слова → результат со списком рифм
  * Тип рифмы указывается
  * Всё — с анимацией и кайфом

## 🗂 Структура проекта

```plaintext
📁 Models
 ├─ Words.cs
 ├─ Phonemes.cs
 ├─ WordPhonemes.cs
 ├─ Rhymes.cs
 ├─ RhymeTypes.cs
 └─ Users.cs

📁 Pages
 ├─ Login.xaml
 ├─ AdminWindow.xaml
 ├─ slova.xaml          ← управление словами
 ├─ UserPage.xaml       ← управление пользователями
 └─ UserWindow.xaml     ← поиск рифм

📁 Configuration
 └─ appsettings.json
```

## 📝 Примечания

* 🔓 Пароли пока хранятся в открытом виде — **ОЧЕНЬ РЕКОМЕНДУЕТСЯ** использовать хеширование (например, `BCrypt.Net`)
* 🧠 Кэширование — через `_rhymeCache` и `_phonemeCache`, ускоряет повторный поиск
* 🔤 Поддерживается только **русский язык**
* 📁 Импорт возможен только из `.txt`, формат: `слово [позиция_ударения]`

## 📸 Скриншоты

<div style="display: flex; flex-wrap: wrap; gap: 10px; justify-content: center;">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/1.png?raw=true" alt="RhymeFinder width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/2.png?raw=true" alt="RhymeFinder" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/3.png?raw=true" alt="RhymeFinder" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/4.png?raw=true" alt="RhymeFinder" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/5.png?raw=true" alt="RhymeFinder" width="30%">
</div>

## 🧠 Автор

**TemhaN**  
[GitHub профиль](https://github.com/TemhaN)

## 🧾 Лицензия

Проект распространяется под лицензией MIT.

## 📬 Обратная связь

Нашли баг или хотите предложить улучшение?
Создайте **issue** или отправьте **pull request** в репозиторий!
