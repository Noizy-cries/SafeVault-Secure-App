# SafeVault Secure API
A secure ASP.NET Core Web API with JWT authentication, role-based authorization, and protection against SQL injection & XSS attacks.
## 🚀 Quick Start
# Run
cd SafeVault-Secure-App
dotnet run
🔑 Default Admin Login
Username: admin
Password: Admin123!
📡 API Endpoints
Method	Endpoint	Description	Auth
POST	/api/auth/register	Register new user	✅
POST	/api/auth/login	Login & get JWT token	✅
GET	/api/user/profile	Get user profile	✅
PUT	/api/user/profile	Update profile	✅
GET	/api/admin/users	Get all users	✅ Admin
POST	/api/admin/users/{id}/deactivate	Deactivate user	✅ Admin
🛡️ Security Features
✅ JWT Authentication (15 min expiry)
✅ BCrypt Password Hashing
✅ Role-Based Access Control (Admin, Manager, User)
✅ SQL Injection Prevention (Parameterized Queries)
✅ XSS Protection (HTML Encoding + Security Headers)
✅ Input Validation & Sanitization
🧪 Test with Postman
Register: POST http://localhost:5072/api/auth/register
Login: POST http://localhost:5072/api/auth/login
Use token in Header: Authorization: Bearer YOUR_TOKEN

