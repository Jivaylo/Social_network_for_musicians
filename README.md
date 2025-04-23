
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

> Add some UI screenshots here showing:
- Track page with YouTube embed
- Playlists
- Admin dashboard
- Messaging panel

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
