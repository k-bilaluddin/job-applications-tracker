# Documentation Plan – Job Applications Tracker

> **Purpose:** This file is a precise documentation plan derived directly from the codebase.
> It does **not** contain full documentation; it describes *what* should be documented and *where*.

---

## 1. Full Project Structure Summary

```
job-applications-tracker/
├── .github/
│   └── workflows/
│       └── ci.yml                   # GitHub Actions CI pipeline
├── .gitignore
├── LICENSE
├── backend/
│   ├── JobApplicationTracker.sln    # .NET solution file
│   ├── JobApplicationTracker.Api/   # REST API project
│   ├── JobApplicationTracker.Contracts/  # Shared event contracts
│   └── JobApplicationTracker.Worker/    # Background worker project
└── frontend/
    └── job-application-tracker-ui/  # Vue 3 SPA
```

The repository is a **full-stack web application** that:

- Lets users create and manage job applications.
- Tracks salary details, interview rounds, and application status.
- Publishes and consumes domain events over RabbitMQ.
- Persists all data in MongoDB.
- Exposes a REST API consumed by a Vue 3 SPA.

---

## 2. Backend Projects / Modules Found

### 2.1 `JobApplicationTracker.Api` (ASP.NET Core 8 Web API)

| Folder | Contents |
|---|---|
| `Controllers/` | `ApplicationController` – CRUD for job applications; `ApplicationInterviewsController` – CRUD for interview rounds |
| `Handlers/` | `ApplicationHandler` – orchestrates business logic between controller and repository |
| `Interfaces/` | `IApplicationHandler`, `IApplicationRepository` – seam interfaces for DI |
| `Repository/` | `JobApplicationRepository` – MongoDB data access implementation |
| `Models/Entities/` | `JobApplication`, `InterviewRound` – core domain objects |
| `Models/Persistence/Documents/` | `JobApplicationDocument`, `InterviewRoundDocument` – MongoDB BSON-mapped documents |
| `Models/Request/` | Six request DTOs: `CreateJobApplicationRequest`, `UpdateJobApplicationRequest`, `UpdateApplicationStatusRequest`, `SearchApplicationsRequest`, `AddInterviewRoundRequest`, `UpdateInterviewRoundRequest` |
| `Models/Response/` | `JobApplicationResponse`, `InterviewRoundResponse` |
| `Messaging/` | RabbitMQ publisher, consumers, connection provider, hosted service, JSON serializer |
| `Errors/` | `ApiException`, `ErrorCache`, `ExceptionHandlingMiddleware` |
| `Program.cs` | Composition root – service registration, middleware pipeline |
| `appsettings.json` | MongoDB and RabbitMQ connection settings |

### 2.2 `JobApplicationTracker.Contracts` (Class Library)

| Folder | Contents |
|---|---|
| `Messaging/Events/` | `JobApplicationCreatedEvent`, `JobApplicationUpdatedEvent` – shared event contract DTOs used by both the API publisher and the Worker consumer |

### 2.3 `JobApplicationTracker.Worker` (.NET Worker Service)

| Folder | Contents |
|---|---|
| `Messaging/` | Mirrors the API messaging structure: RabbitMQ connection provider, consumer base, `JobApplicationCreatedConsumer`, JSON serializer, DI extensions |
| `Messaging/Handlers/` | `JobApplicationCreatedEventHandler` – processes consumed events |
| `MessagingHostedService.cs` | `IHostedService` that starts/stops consumers |
| `Program.cs` | Worker host configuration |

---

## 3. Frontend Projects / Modules Found

### `frontend/job-application-tracker-ui` (Vue 3 SPA)

| Folder | Contents |
|---|---|
| `src/api/` | `client.ts` (Axios instance), `jobApplications.api.ts`, `interviews.api.ts` |
| `src/stores/` | `jobApplications.store.ts`, `interviews.store.ts` (Pinia), `counter.ts` (scaffold) |
| `src/views/` | `HomeView.vue`, `JobApplicationsListView.vue`, `JobApplicationCreateView.vue`, `JobApplicationDetailsView.vue`, `JobApplicationEditView.vue`, `AboutView.vue` |
| `src/components/common/` | `AppError.vue`, `AppLoader.vue`, `ConfirmDialog.vue`, `EmptyState.vue`, `StatusBadge.vue` |
| `src/components/job-applications/` | `JobApplicationFilters.vue`, `JobApplicationForm.vue`, `JobApplicationStatusUpdater.vue`, `JobApplicationTable.vue` |
| `src/components/interviews/` | `InterviewForm.vue`, `InterviewItem.vue`, `InterviewList.vue` |
| `src/router/` | `index.ts` – Vue Router 5 route definitions |
| `src/types/` | `jobApplication.types.ts`, `interview.types.ts`, `api.types.ts` |
| `src/utils/` | `constants.ts`, `date.ts`, `status.ts` |

---

## 4. Main Technologies Detected

### Backend

| Category | Technology | Version |
|---|---|---|
| Language | C# | 12 (implicit, .NET 8) |
| Runtime | .NET | 8.0 |
| Web Framework | ASP.NET Core | 8.0 |
| Database Driver | MongoDB.Driver | 3.7.0 |
| Message Broker Client | RabbitMQ.Client | 7.2.1 |

### Frontend

| Category | Technology | Version |
|---|---|---|
| Language | TypeScript | ~5.9.3 |
| Framework | Vue | ^3.5.29 |
| Build Tool | Vite | ^7.3.1 |
| State Management | Pinia | ^3.0.4 |
| Routing | Vue Router | ^5.0.3 |
| HTTP Client | Axios | ^1.13.6 |
| Linting | ESLint + Oxlint + Prettier | — |

### Infrastructure / DevOps

| Category | Technology |
|---|---|
| Database | MongoDB |
| Message Broker | RabbitMQ |
| CI/CD | GitHub Actions |
| Node requirement | ^20.19.0 \|\| >=22.12.0 |

---

## 5. Layers Detected

### 5.1 API Layer

- **Files:** `Controllers/ApplicationController.cs`, `Controllers/ApplicationInterviewsController.cs`
- **Pattern:** Thin controllers that delegate to `IApplicationHandler`
- **Endpoints (Job Applications):**
  - `POST   /api/job-applications`
  - `GET    /api/job-applications`
  - `GET    /api/job-applications/{id}`
  - `PUT    /api/job-applications/{id}`
  - `PATCH  /api/job-applications/{id}/status`
- **Endpoints (Interviews):**
  - `GET    /api/job-applications/{id}/interviews`
  - `GET    /api/job-applications/{id}/interviews/{interviewId}`
  - `POST   /api/job-applications/{id}/interviews`
  - `PUT    /api/job-applications/{id}/interviews/{interviewId}`
  - `DELETE /api/job-applications/{id}/interviews/{interviewId}`

### 5.2 Data Layer

- **Files:** `Repository/JobApplicationRepository.cs`, `Models/Persistence/Documents/`
- **Pattern:** Repository pattern behind `IApplicationRepository`
- **Database:** MongoDB; collection `job-applications` inside database `job-applications`
- **Mapping:** Domain entities ↔ BSON documents via attribute-annotated document classes

### 5.3 Messaging Layer

- **Publisher (API):** `Messaging/Publishers/RabbitMqMessagePublisher.cs` – publishes `JobApplicationCreatedEvent` / `JobApplicationUpdatedEvent` after writes
- **Consumer (API):** `Messaging/Consumers/JobApplicationCreatedConsumer.cs` + `MessagingHostedService.cs`
- **Consumer (Worker):** mirrors the API consumer structure; `JobApplicationCreatedEventHandler` processes the event
- **Shared contracts:** `JobApplicationTracker.Contracts` project holds event DTOs
- **Serialization:** JSON via `JsonEventSerializer`
- **Queues:** `job-application-created`, `job-application-updated` (configured in `appsettings.json`)

### 5.4 UI Layer

- **Framework:** Vue 3 (Composition API + `<script setup>`)
- **State:** Pinia stores (`jobApplications.store.ts`, `interviews.store.ts`)
- **Routing:** Vue Router 5; routes under `/applications`
- **HTTP:** Axios client configured in `src/api/client.ts`; domain API calls in `jobApplications.api.ts` and `interviews.api.ts`
- **Component hierarchy:**
  - Views (page-level) → domain-specific components → common/shared components
  - Status badge and filters are reusable across views

---

## 6. Existing README / Docs Files

| File | Location | Description |
|---|---|---|
| `README.md` | `frontend/job-application-tracker-ui/README.md` | Vite/Vue scaffold template README – covers `npm run dev/build/lint` only |

**Gaps:** No root-level README, no backend setup guide, no architecture overview, no API reference, no contributing guide.

---

## 7. Recommended Documentation Files to Create

| # | File | Location | Priority | Contents |
|---|---|---|---|---|
| 1 | `README.md` | `/` (root) | **High** | Project overview, tech stack, quick-start (backend + frontend + infra), CI badge, links to detailed docs |
| 2 | `docs/architecture.md` | `/docs/` | **High** | System diagram, service responsibilities, data flow (HTTP request → handler → repository → MongoDB), event flow (API → RabbitMQ → Worker) |
| 3 | `docs/backend-setup.md` | `/docs/` | **High** | Prerequisites (.NET 8 SDK, MongoDB, RabbitMQ), clone + restore + run steps, `appsettings.json` config reference, environment variable overrides |
| 4 | `docs/frontend-setup.md` | `/docs/` | **High** | Prerequisites (Node ≥20), `npm ci`, `npm run dev`, env variable for API base URL, build + preview steps |
| 5 | `docs/api-reference.md` | `/docs/` | **High** | All endpoints with HTTP method, URL, request body schema, response schema, status codes, example cURL calls |
| 6 | `docs/data-models.md` | `/docs/` | **Medium** | `JobApplication` and `InterviewRound` entity fields, types, constraints; MongoDB document structure; status and type enum values |
| 7 | `docs/messaging.md` | `/docs/` | **Medium** | RabbitMQ queue names, event schemas (`JobApplicationCreatedEvent`, `JobApplicationUpdatedEvent`), publisher/consumer lifecycle, Worker service role |
| 8 | `docs/contributing.md` | `/docs/` | **Medium** | Branch naming, PR process, coding conventions (C# style, Vue Composition API, TypeScript strict), linting commands |
| 9 | `docs/deployment.md` | `/docs/` | **Low** | Docker Compose example (API + Worker + MongoDB + RabbitMQ), environment variable reference for production, health-check endpoints |

---

## 8. Missing Areas That Need Clarification

The following questions cannot be answered from the code alone and require input from the team:

1. **Worker service purpose** – `JobApplicationTracker.Worker` and `JobApplicationTracker.Api` both contain near-identical messaging code. Is the Worker a separate deployable service or was the intent to extract shared code into a library? Which one should actually consume events in production?

2. **Duplicate event code** – `JobApplicationTracker.Api/Messaging/` and `JobApplicationTracker.Worker/Messaging/` are nearly identical. Is there a plan to consolidate them into `JobApplicationTracker.Contracts` or a new shared library?

3. **Authentication & authorisation** – No auth middleware, identity, or JWT configuration was found in `Program.cs`. Is this intentional (internal tool) or a planned future addition?

4. **Search/filter implementation** – `SearchApplicationsRequest.cs` exists as a request DTO but it is unclear how filtering is applied in the repository query (e.g., which fields are indexed or filterable). This should be described in `docs/api-reference.md`.

5. **Environment configuration** – `appsettings.json` contains placeholder strings (`"url"`) for MongoDB and RabbitMQ connection strings. A documented `.env.example` or `appsettings.Example.json` is missing.

6. **Frontend API base URL** – The Axios client in `src/api/client.ts` references a base URL. Where this is configured (environment variable, hardcoded, Vite proxy?) needs clarification for the setup guide.

7. **No tests** – Neither backend unit/integration tests (xUnit/NUnit) nor frontend tests (Vitest/Cypress) exist. The team should decide on a testing strategy before it is documented.

8. **`counter.ts` store** – A scaffold Pinia store (`counter.ts`) remains in `src/stores/`. It appears to be a leftover from the Vite template and should either be removed or documented as intentional.

9. **`HelloWorld.vue`, `TheWelcome.vue`, `WelcomeItem.vue`** – Vite scaffold components remain in `src/components/`. Their presence (or planned removal) should be clarified.

10. **CORS policy** – No CORS configuration is visible in `Program.cs`. For the frontend to communicate with the API in development, this must be addressed and documented.

11. **Deployment target** – Is this intended to run on-premises, in Docker, in Kubernetes, or as a cloud service? This affects the scope of `docs/deployment.md`.
