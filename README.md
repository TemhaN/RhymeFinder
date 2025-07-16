# 🌸 FlowerShop App

**FlowerShop** — веб-приложение на **ASP.NET Core** для управления цветочным магазином.  
Позволяет пользователям искать, просматривать и добавлять в корзину товары (букеты, цветы, игрушки), оставлять отзывы, управлять профилем и избранным.  
Реализована админ-панель для управления пользователями, товарами и отзывами.  
Приложение использует современный интерфейс на Tailwind CSS и поддерживает авторизацию через JWT и куки.

## ✨ Возможности

- 🔐 **Аутентификация**: регистрация, вход, редактирование профиля, выход.
- 🛒 **Управление товарами**: просмотр, добавление в корзину/избранное, фильтрация и сортировка (по названию, цене).
- 📝 **Отзывы**: добавление, просмотр и удаление отзывов с рейтингом (звёзды).
- ⭐ **Избранное**: добавление и удаление товаров в избранное.
- 🛠️ **Админ-панель**: управление пользователями, товарами (игрушки) и отзывами.
- 🎨 **Современный интерфейс**: адаптивный дизайн с Tailwind CSS, градиенты, анимации, закруглённые элементы.
- 📦 **Корзина и заказы**: добавление товаров в корзину, оформление заказов, история заказов.

## 📋 Требования

- [.NET SDK 6.0+](https://dotnet.microsoft.com/en-us/download)
- SQLite (база данных создаётся автоматически)
- Любой современный браузер (Chrome, Firefox, Edge и т.д.)
- Подключение к серверу FlowerShop (локально или удалённо)

## 🧩 Зависимости

| Библиотека / Технология                 | Назначение                                  |
|----------------------------------------|---------------------------------------------|
| `ASP.NET Core`                         | Основной фреймворк для веб-приложения       |
| `Entity Framework Core`                | ORM для работы с SQLite                     |
| `Tailwind CSS`                         | Стилизация интерфейса                       |
| `Microsoft.AspNetCore.Authentication`  | Аутентификация через JWT и куки             |
| `System.IdentityModel.Tokens.Jwt`      | Работа с JWT-токенами                       |

Полный список — в `*.csproj` и `appsettings.json`.

## 🚀 Установка и запуск

1. **Клонируйте репозиторий**:
   ```bash
   git clone https://github.com/YourUsername/FlowerShop.git
   cd FlowerShop

2. **Установите зависимости**:

   ```bash
   dotnet restore

3. **Настройте `appsettings.json`**:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=flowershop.db"
     },
     "Jwt": {
       "Key": "YourSuperSecretKey1234567890123456",
       "Issuer": "FlowerShop",
       "Audience": "FlowerShopUsers"
     }
   }
   ```

   ⚠️ Убедитесь, что `Jwt:Key` содержит **не менее 32 символов**.
   ✅ База данных SQLite (`flowershop.db`) создаётся автоматически.

4. **Запустите приложение**:

   ```bash
   dotnet run

5. **Для сборки релизной версии**:

   ```bash
   dotnet publish -c Release
   ```

   Сборка будет находиться в `bin/Release/net6.0/publish`.

## 🖱️ Использование

* Запустите приложение: `dotnet run`
* Откройте в браузере: [https://localhost:5001](https://localhost:5001) *(или указанный порт)*
* Зарегистрируйтесь или войдите:

  * `/Auth/Register`
  * `/Auth/Login`
* Просматривайте товары: `/Home/Index`
* Добавляйте в корзину, избранное, оставляйте отзывы
* Управляйте профилем и заказами: `/Profile`
* **Админ-доступ**:

  * Пользователи: `/Admin/Users`
  * Игрушки: `/Admin/Toys`
  * Отзывы: `/Admin/Reviews`

## 📦 Сборка приложения

### Релизная сборка:

```bash
dotnet publish -c Release
```

### Развёртывание на сервере:

1. Скопируйте содержимое `bin/Release/net6.0/publish` на сервер
2. Настройте веб-сервер (например, Kestrel или IIS)
3. Убедитесь, что база данных `flowershop.db` доступна приложению

## 📸 Скриншоты

<div style="display: flex; flex-wrap: wrap; gap: 10px; justify-content: center;">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/1.png?raw=true" alt="Service App" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/2.png?raw=true" alt="Service App" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/3.png?raw=true" alt="Service App" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/4.png?raw=true" alt="Service App" width="30%">
  <img src="https://github.com/TemhaN/RhymeFinder/blob/master/kur/Screenshots/5.png?raw=true" alt="Service App" width="30%">
</div>

## 🧠 Автор

**TemhaN**  
[GitHub профиль](https://github.com/TemhaN)

## 🧾 Лицензия

Проект распространяется под лицензией MIT.

## 📬 Обратная связь

Нашли баг или хотите предложить улучшение?
Создайте **issue** или отправьте **pull request** в репозиторий!

## ⚙️ Технологии

* **ASP.NET Core** — Backend и маршрутизация
* **Entity Framework Core** — Работа с SQLite
* **Razor Pages** — Рендеринг интерфейса
* **Tailwind CSS** — Современная стилизация
* **JWT + Cookie Auth** — Аутентификация
* **JavaScript** — Валидация форм, AJAX, клиентская логика
