🎟️ EventHub

*EventHub* is an ASP.NET Core Web API for managing events and attendee registrations. It allows users to register for events and receive a unique tag as proof of registration. Admins can create and manage events, upload images, and add guest speakers.

 Features
✅ Admin can:
  * Create and manage events
  * Upload event cover images
  * Add guest speakers for events
    
 🙋 User can:
  * View available events
  * Register for an event
  * Upload passport photo during registration
  * Receive a **registration tag** as proof

  📦 Clean architecture structure (Controllers + Services + Interfaces)

 💾 EF Core with Code-First migration

📚 Swagger UI for easy API testing

## 🛠️ Tech Stack
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server or SQLite (local dev)
* Swagger / Swashbuckle

## 🔗 Sample API Endpoints

| Endpoint                        | Description                               |
| ------------------------------- | ----------------------------------------- |
| `POST /api/event`               | Admin creates a new event                 |
| `POST /api/event/{id}/speaker`  | Add guest speaker to an event             |
| `POST /api/event/{id}/register` | Register a user for an event (with image) |
| `GET /api/event`                | Get all available events                  |
| `GET /api/event/{id}`           | Get details of a specific event           |

## 📌 Notes

* All registration submissions generate a **unique tag.**
* File uploads (passport photos or event covers) are stored under `EventUploads/`.

## 🤝 Contributions

PRs, issues, and suggestions are welcome!
Help us improve the EventHub experience.
