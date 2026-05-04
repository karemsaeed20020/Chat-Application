# 💬 Chat Application

A modern and features rich real-time chat application that enables seamless communication with instant messaging, multimedia sharing.

# Project Mind Map
![Image](https://github.com/user-attachments/assets/881cdb26-510f-492a-90d7-44f27bfa90d8)

# Project Endpoints
# Authentication
<img width="1831" height="627" alt="Image" src="https://github.com/user-attachments/assets/bef95507-9cb1-4d6e-8fed-13fe9a3be99d" />

# Account
<img width="1813" height="627" alt="Image" src="https://github.com/user-attachments/assets/e938ab41-9d75-41bb-b6e9-7c566f6293c6" />

# User
<img width="1797" height="886" alt="Image" src="https://github.com/user-attachments/assets/e75e5f4c-9df2-4d2a-9d55-d0b4143f039c" />

# Message
<img width="1812" height="677" alt="Image" src="https://github.com/user-attachments/assets/9f68c3f8-ff64-4bef-9d8a-843394097164" />

<img width="1826" height="627" alt="Image" src="https://github.com/user-attachments/assets/b5eccd52-5c4e-4c63-9600-5290ba6a22ca" />

# Room
<img width="1811" height="757" alt="Image" src="https://github.com/user-attachments/assets/9dc97980-b58b-4334-9736-48d46e15186d" />

<img width="1816" height="433" alt="Image" src="https://github.com/user-attachments/assets/8b4b70a1-5ee3-44e2-97af-44cddf8a7495" />

## ✨ Features

### Core Functionality
- 🔐 **User Authentication** - Secure sign up, login, and logout
- 💬 **Real-time Messaging** - Instant message delivery using WebSocket technology
- 👥 **Private Chat** - One-on-one conversations with other users
- 👨‍👩‍👧‍👦 **Group Chat** - Create and manage group conversations
- 📎 **Media Sharing** - Send and receive images, videos, and files
- 🔍 **User Search** - Find and connect with other users
### Advanced Features
- ✅ **Message Read Receipts** - See when messages are delivered and read
- ✅ **Typing Indicators** - Know when someone is typing
- ✅ **Profile Customization** - Update profile picture and status
- ✅ **Message Management** - Edit and delete your messages
- ✅ **Online Status** - See who's currently online
- ✅ **Real-Time Messaging**    : SignalR WebSockets
- ✅ **Private & Group Chats**   : Complete
- ✅ **File/Image/Video Upload** : Atomic with message (no separate endpoint)
- ✅ **Message Pinning**         : One per room
- ✅ **Read Receipts**           : Double check
- ✅ **Profile Avatar & Status** : Upload + live sync
- ✅ **User Search**             : Instant results
- ✅ **Block User**              : Privacy control
- ✅ **JWT Authentication**      : ASP.NET Identity
- ✅ **Pagination**              : Infinite scroll ready
- ✅ **Error Handling
  with Result Pattern**           :Employed a result pattern for structured error handling, providing clear and actionable feedback to users.
  ✅ **Exception Handling**       :Integrated centralized exception handling to manage errors gracefully, significantly enhancing the user experience.
- ✅ **CORS (Cross-Origin
   Resource Sharing)**            :a security feature implemented by web browsers to prevent web pages from making requests to a different domain than the one that served the web page. 
- ✅ **Background Jobs**          : Used Hangfire for managing background tasks like sending confirmation emails and processing password resets seamlessly.
- ✅ **Audit Logging**            :Implemented audit logging to track changes on resources, ensuring transparency and accountability in user actions.
- ✅ **Fluent Validation**        :Ensured data integrity by effectively validating inputs, leading to user-friendly error messages.

## 🛠️ Technologies Used

- Backend          : ASP.NET Core 10
- Real-Time        : SignalR
- ORM              : Entity Framework Core 
- Database         : SQL Server 
- Auth             : JWT + Identity
- Validation       : FluentValidation
- Mapping          : Manual using extension method
- Architecture     : Monolithic
- File Storage     : wwwroot/uploads 
- GUIDs            : Version 7 (sequential & fast)

## 📁 Project Structure

```
Chat--Application/
│   ├── public/            # Static files
│   ├── src/
│   │   ├── assets/        # Images, etc.
│   │   ├── components/    # Reusable components
│   │   │   ├── Chat/
│   │   │   ├── Auth/
│   │   │   ├── Profile/
│   │   │   └── Common/
│   │   ├── pages/         # Page components
│   │   ├── context/       # Context API
│   │   ├── services/      # API services
│   └── .env
│
├── server/                # Backend application
│   ├── config/            # Configuration files
│   ├── controllers/       # Route controllers
│   ├── models/            # Database models
│   ├── routes/            # API routes
│   ├── socket/            # Socket.io handlers
│   └── .env
│
├── .gitignore
├── LICENSE
└── README.md
```

## 📚 API Documentation

### Some Ex Auth Endpoints (`/api/auth`)

| Method | Endpoint                        | Description                 | Auth Required |
|--------|---------------------------------|-----------------------------|---------------|
| POST   | `/api/auth/login`               | Login user                  | ❌            |
| POST   | `/api/auth/register`            | Register new user           | ❌            |
| POST   | `/api/auth/refresh`             | Refresh JWT token           | ❌            |
| PUT    | `/api/auth/revoke-refresh-token`| Revoke refresh token        | ✅            |
| POST   | `/api/auth/confirm-email`       | Confirm email address       | ❌            |
| POST   | `/api/auth/forget-password`     | Initiate password reset     | ❌            |
| POST   | `/api/auth/reset-password`      | Complete password reset     | ❌            |

### Some Ex User Endpoints (`/api/users`)

| Method | Endpoint                    | Description                      | Auth Required |
|--------|-----------------------------|----------------------------------|---------------|
| GET    | `/api/users/profile`        | Get current user profile         | ✅            |
| PUT    | `/api/users/profile`        | Update user profile              | ✅            |
| POST   | `/api/users/avatar`         | Upload profile avatar            | ✅            |
| GET    | `/api/users/search`         | Search users by name/email       | ✅            |
| GET    | `/api/users/:id`            | Get user by ID                   | ✅            |
| GET    | `/api/users/online`         | Get all online users             | ✅            |

### Socket.io Events

#### Client → Server (Emit)

| Event Name          | Payload                                      | Description                    |
|---------------------|----------------------------------------------|--------------------------------|
| `join_room`         | `{ messageId: string }`                         | Join a chat room               |
| `leave_room`        | `{ messageId: string }`                         | Leave a chat room              |
| `send_message`      | `{ messageId, content, type, attachments }`     | Send a new message             |
| `typing_start`      | `{ messageId: string, userId: string }`         | User started typing            |
| `typing_stop`       | `{ messageId: string, userId: string }`         | User stopped typing            |
| `message_read`      | `{ messageId: string, chatId: string }`      | Mark message as read           |
| `user_online`       | `{ userId: string }`                         | User came online               |
| `user_offline`      | `{ userId: string }`                         | User went offline              |

#### Server → Client (Listen)

| Event Name          | Payload                                      | Description                    |
|---------------------|----------------------------------------------|--------------------------------|
| `message_received`  | `{ message: Object, chatId: string }`        | New message received           |
| `message_updated`   | `{ messageId: string, content: string }`     | Message was edited             |
| `message_deleted`   | `{ messageId: string, chatId: string }`      | Message was deleted            |
| `user_typing`       | `{ chatId: string, user: Object }`           | Someone is typing              |
| `user_stopped_typing`| `{ chatId: string, userId: string }`        | Someone stopped typing         |
| `user_online_status`| `{ userId: string, status: boolean }`        | User online status changed     |
| `message_read_receipt`| `{ messageId: string, readBy: string }`    | Message read by user           |
| `chat_updated`      | `{ chatId: string, updates: Object }`        | Chat details updated           |
| `error`             | `{ message: string, code: string }`          | Error occurred                 |

