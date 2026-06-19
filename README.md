
# AI Health Tracker

An AI-powered health tracking application that helps users monitor daily symptoms, mood, sleep and meals — then uses AI to find patterns and generate weekly health insights.

## Demo
[Live Demo Link] • [LinkedIn Post Link]

## Screenshots
Coming soon...

## Tech Stack

### Backend
- ASP.NET Core 9 — REST API
- Clean Architecture + CQRS + MediatR
- Entity Framework Core 9 + SQL Server
- JWT Authentication + BCrypt
- Hangfire background jobs
- Groq AI API (Llama 3.1) for health insights

### Frontend
- React + TypeScript
- Tailwind CSS + shadcn/ui
- Recharts for health data visualization
- Axios for API communication

## Features
- JWT Authentication (register, login, refresh tokens)
- Daily symptom, mood, sleep and meal logging
- AI-powered health pattern analysis
- Automated weekly AI reports every Sunday
- Beautiful dashboard with health trend charts
- Real-time data visualization

## Project Structure
ai-health-tracker/

├── backend/          .NET Core 9 REST API

├── frontend/         React TypeScript app

└── docker-compose.yml

## Running Locally
```bash
# Clone
git clone https://github.com/yourusername/ai-health-tracker.git

# Backend
cd backend
dotnet restore
dotnet ef database update --project HealthTracker.Infrastructure --startup-project HealthTracker.API
dotnet run --project HealthTracker.API

# Frontend
cd frontend
npm install
npm run dev
```
