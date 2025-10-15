# Pubs Management System - Angular Frontend

## Overview

Modern Angular 17+ application for the Pubs Management System microservices. Features responsive design, PWA capabilities, real-time notifications, and comprehensive CRUD operations for all business entities.

## Features

### ✅ Implemented
- **Angular 17+** with standalone components and modular architecture
- **Authentication Module** with JWT token handling
  - Login and registration forms
  - Auth guards and HTTP interceptors
  - Secure token storage
- **Responsive Material Design** using Angular Material
- **Progressive Web App (PWA)** capabilities
  - Offline support with service workers
  - App manifest for installability
  - Optimized caching strategies
- **Lazy Loading** for optimal performance
- **Dashboard** with navigation to all features
- **Authors Management** module with CRUD operations (foundation)
- Feature modules for Publishers, Titles, Sales, and Employees

### 🚧 In Progress
- NgRx state management integration
- SignalR real-time notifications
- Analytics dashboard with charts
- Complete CRUD implementations for all modules
- Comprehensive unit and integration tests

## Architecture

### Project Structure
```
src/app/
├── auth/                       # Authentication module
│   ├── components/
│   │   ├── login/             # Login form
│   │   └── register/          # Registration form
│   └── services/
│       └── auth.service.ts    # Authentication service
├── core/                       # Core singleton services
│   ├── guards/
│   │   └── auth.guard.ts      # Route protection
│   ├── interceptors/
│   │   └── auth.interceptor.ts # JWT token injection
│   └── models/
│       ├── auth.models.ts     # Auth types
│       └── domain.models.ts   # Domain entities
├── shared/                     # Shared components and utilities
├── features/                   # Feature modules
│   ├── dashboard/             # Main dashboard
│   ├── authors/               # Authors management
│   ├── publishers/            # Publishers management
│   ├── titles/                # Titles management
│   ├── sales/                 # Sales management
│   └── employees/             # Employees management
└── environments/               # Environment configuration
```

### Key Technologies
- **Framework**: Angular 17+ (standalone components)
- **UI Library**: Angular Material
- **State Management**: NgRx (configured, integration in progress)
- **Real-time**: @microsoft/signalr (installed)
- **Charts**: Chart.js (installed)
- **PWA**: @angular/pwa with service worker

## Getting Started

### Prerequisites
- Node.js 18+ and npm
- Angular CLI 17+

### Installation

```bash
cd src/WebApps/PubsWebApp
npm install
```

### Development Server

```bash
npm start
```

Navigate to `http://localhost:4200/`. The application will automatically reload when you make changes.

### Environment Configuration

Two environment files are configured:
- `src/environments/environment.ts` - Development
- `src/environments/environment.prod.ts` - Production

Update API URLs to match your backend services:

```typescript
export const environment = {
  production: false,
  apiUrls: {
    identity: 'http://localhost:5007',
    authors: 'http://localhost:5001',
    publishers: 'http://localhost:5002',
    // ... other services
  }
};
```

## API Integration

### Backend Services

The frontend integrates with the following microservices:

| Service | Port | Purpose |
|---------|------|---------|
| Identity API | 5007 | Authentication and user management |
| Authors API | 5001 | Authors CRUD operations |
| Publishers API | 5002 | Publishers CRUD operations |
| Titles API | 5003 | Titles CRUD operations |
| Sales API | 5004 | Sales and orders management |
| Employees API | 5005 | Employee management |
| Notifications API | 5008 | Real-time notifications via SignalR |
| Analytics API | 5009 | Analytics and reporting |

### Authentication Flow

1. User submits credentials via login form
2. AuthService sends request to Identity API
3. JWT token received and stored in localStorage
4. AuthInterceptor adds token to all subsequent requests
5. AuthGuard protects routes requiring authentication

## Roadmap

### Next Steps

1. **NgRx State Management** - Implement store, actions, reducers
2. **Complete CRUD Operations** - Finish all feature modules
3. **Real-time Notifications** - SignalR hub integration
4. **Analytics Dashboard** - Sales charts and graphs
5. **Testing** - Unit tests for components and services
6. **Accessibility** - WCAG 2.2 Level AA compliance
