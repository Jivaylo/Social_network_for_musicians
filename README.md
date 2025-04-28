
# 🎧 SoundSocial – Social Network for Musicians

SoundSocial is a feature-rich ASP.NET Core MVC web application that connects musicians and music lovers. Users can upload and share tracks, create playlists, follow artists, send private messages, and explore trending music. It's like a mini Spotify meets SoundCloud, built from scratch!

---

## 🚀 Features

- ✅ **Authentication**: Secure registration & login system with custom profile management
- 🎵 **Upload Music**: Supports MP3/WAV files and YouTube video links with dynamic preview
- 🎨 **Track Images**: Add custom images to your tracks for better visuals
- 🔁 **Play Count**: Tracks total plays, even for YouTube embeds
- ❤️ **Like/Dislike System**: Interactive feedback on each track
- 📥 **Playlists**: Create, manage and organize custom playlists
- 💬 **Comments**: Social interaction on every track
- 🔔 **Direct Messages**: Secure user-to-user messaging
- 🔍 **User Search**: Find and follow other artists
- 👤 **Public Profiles**: View uploaded tracks, followers and bios
- 📊 **Admin Dashboard**: Promote users, ban accounts, view total stats
- 🚨 **Report System**: Users can report tracks or users for moderation

---

## 📷 Screenshots

1. Начална страница (Home Page)

Потребителят е посрещнат от модерна начална страница със заглавие SoundSocial, бутон за вход (Login) и регистрация (Join Now).
На дъното са визуализирани основните стъпки: Create, Connect и Grow.
![Image](https://github.com/user-attachments/assets/bc1ade6c-955b-4e82-b181-673ea2d71aec)

2. Структура на базата данни (Database Diagram)

ER Диаграма, визуализираща връзките между основните таблици: Users, Tracks, Playlists, Messages, Comments, Reports, Genres и други.
Диаграмата показва как се организират данните и взаимодействията в платформата.
![Image](https://github.com/user-attachments/assets/71808815-40d9-4423-b64b-0059f789a00d)

3. Страница за вход (Login Page)

Екран за вход с полета за имейл и парола.
Потребителите могат да запазят сесията си чрез опцията "Remember me" и да се прехвърлят към регистрация, ако нямат акаунт.
![Image](https://github.com/user-attachments/assets/a1433ed4-0ff4-4c33-b713-bd56fade4675)

4. Качване на нова песен (Upload New Track)

Форма за качване на нова песен или добавяне на линк към YouTube.
Потребителят може да въведе заглавие, описание, да качи файл или снимка, както и да избере категория на песента.
![Image](https://github.com/user-attachments/assets/f83c712f-985a-4e58-9961-1499bb873294)

5. Търсене и последване на потребители (Find and Follow Users)

Функция за търсене на потребители по имейл.
Може да последвате даден потребител чрез бутона Follow или да му изпратите лично съобщение чрез бутона Message.
![Image](https://github.com/user-attachments/assets/834dd8f9-048d-4373-b1e2-e80587ef89e3)

6. Страница на песен с YouTube вграждане (Track Page with YouTube Embed)

Показване на страница на качена песен с вградено YouTube видео.
Има информация за песента: описание, категория, качен от кого и кога, както и броя на слушанията.
Потребителите могат да харесват, нехаресват, редактират или докладват песента.
![Image](https://github.com/user-attachments/assets/5883bba2-39da-4cdd-ba14-f53f979a1787)

---

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core MVC
- **Frontend**: Razor Views + Bootstrap 5
- **Database**: Entity Framework Core with SQL Server
- **Auth**: ASP.NET Identity
- **Other**: YouTube IFrame API, File Uploads, Custom Validation, Partial Views

---

## 📂 Folder Structure

```
/Controllers
/Views
    /Tracks
    /Users
    /Playlists
    /Messages
    /Reports
/Models
/Data
    /ApplicationDbContext.cs
wwwroot/
```

---

## 📦 Setup Instructions

1. Clone this repo:
```bash
git clone https://github.com/yourusername/soundsocial.git
```

2. Open in **Visual Studio 2022+**

3. Update `appsettings.json` with your connection string

4. Run migrations:
```bash
dotnet ef database update
```

5. Run the project:
```bash
dotnet run
```

---

## 🔐 Default Admin Login (Optional)

If seeded:

```txt
📧 Email: admin@music.com  
🔑 Password: Admin123!
```

---

## ✅ Todo & Improvements

- [ ] Add real email sender (SendGrid, MailKit)
- [ ] Dark mode toggle 🎨
- [ ] Mobile responsive playlist UI
- [ ] Upload thumbnails for YouTube tracks

---

## 🤝 Contributions

Pull requests and forks are welcome! Let’s grow the musician network together 🎶

---

## 📄 License

MIT License © [Ivaylo Ivanov]
