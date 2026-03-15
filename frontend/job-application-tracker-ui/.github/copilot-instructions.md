# AI Coding Guidelines for Job Application Tracker UI

## Project Overview
This is a Vue 3 frontend application for tracking job applications, built with Vite, TypeScript, Pinia for state management, and Vue Router for navigation. It's currently based on the Vue CLI template and will be customized for job tracking features.

## Architecture
- **Framework**: Vue 3 with Composition API
- **Build Tool**: Vite
- **Language**: TypeScript
- **State Management**: Pinia stores (see `src/stores/`)
- **Routing**: Vue Router with lazy-loaded components (see `src/router/index.ts`)
- **Styling**: Scoped CSS in components, global styles in `src/assets/`

## Key Files and Patterns
- `src/App.vue`: Main app component with router-view and navigation
- `src/main.ts`: App initialization with Pinia and router
- `src/views/`: Page components (e.g., `HomeView.vue`, `AboutView.vue`)
- `src/components/`: Reusable components (e.g., `HelloWorld.vue`)
- `src/stores/`: Pinia stores for state management (example: `counter.ts`)

## Development Workflow
- **Start dev server**: `npm run dev`
- **Build for production**: `npm run build` (includes type checking)
- **Lint and fix**: `npm run lint` (runs oxlint and eslint with auto-fix)
- **Format code**: `npm run format` (prettier)
- **Type check**: `npm run type-check` (vue-tsc)

## Conventions
- Use `@` alias for `src/` imports (configured in `vite.config.ts`)
- Component names: PascalCase (e.g., `HelloWorld.vue`)
- Store names: camelCase with `use` prefix (e.g., `useCounterStore`)
- Route names: lowercase (e.g., 'home', 'about')

## Integration Points# Copilot Instructions – Job Application Tracker UI

This repository contains a Vue 3 frontend for a Job Application Tracker backend written in .NET.

## Tech Stack
- Vue 3
- TypeScript
- Vite
- Pinia (state management)
- Vue Router
- Composition API

## Architecture Rules

### Separation of Concerns
- Components must NOT call APIs directly.
- Components call **Pinia stores**.
- Stores call **API service layer**.
- API layer handles HTTP communication.

Flow:
Component → Store → API Service → Backend

### Folder Structure

src/
api/
client.ts
jobApplications.api.ts
interviews.api.ts

types/
jobApplication.types.ts
interview.types.ts
api.types.ts

stores/
jobApplications.store.ts
interviews.store.ts

views/
JobApplicationsListView.vue
JobApplicationDetailsView.vue
JobApplicationCreateView.vue
JobApplicationEditView.vue

components/
common/
job-applications/
interviews/

router/
index.ts

utils/

### Coding Standards

Use **Composition API only**

Use **TypeScript interfaces for all API models**

Avoid `any`

Use **async/await**

Keep components **small and reusable**

Views should be **thin**

Business logic belongs in **stores or composables**

---

# Backend API

Base URL from environment: VITE_API_BASE_URL


Endpoints:

GET /api/job-applications  
GET /api/job-applications/:id  
POST /api/job-applications  
PUT /api/job-applications/:id  
PATCH /api/job-applications/:id/status  

GET /api/job-applications/:id/interviews  
POST /api/job-applications/:id/interviews  
PUT /api/job-applications/:id/interviews/:interviewId  
DELETE /api/job-applications/:id/interviews/:interviewId  

---

# Entities

## JobApplication
- id
- companyName
- jobTitle
- status
- appliedDate
- notes

## InterviewRound
- id
- jobApplicationId
- roundType
- scheduledAt
- notes
- result

---

# UI Requirements

### Applications List
Display:
- Company
- Job Title
- Status
- Applied Date

Support:
- Search
- Status filter
- Sort by applied date

### Application Details
Show:
- Application information
- Interview rounds list

Allow:
- Add interview round
- Edit interview round
- Delete interview round
- Update application status

### Forms
Reusable forms for:
- Create application
- Edit application
- Interview round

Forms must:
- Validate required fields
- Prevent duplicate submission
- Show API errors
