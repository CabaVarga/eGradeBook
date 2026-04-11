# eGradeBook — Detailed Research Findings

## 1. Project Overview

**eGradeBook** is an ASP.NET Web API back-end application designed to act as an electronic grade book for a school. It was developed as a course project. The author explicitly notes in the README that the code is **not production-ready** and was made public to share the implementation with fellow students.

- **Framework:** ASP.NET Web API 2 on .NET 4.6.1
- **Target runtime:** IIS / OWIN self-host
- **Solution file:** `eGradeBook.sln`
- **Main projects:**
  - `eGradeBook` — the Web API application
  - `eGradeBook.Tests` — the test project (very minimal)

---

## 2. Repository Structure

```
eGradeBook.sln
├── eGradeBook/
│   ├── App_Start/           # Startup configuration (Swagger, Unity, WebApi)
│   ├── Controllers/         # 14 Web API controllers
│   ├── Infrastructure/      # EF DbContext, database initializer/seeder, utilities
│   ├── Migrations/          # EF Code First migrations (one migration)
│   ├── Models/              # Domain model classes + DTOs
│   │   └── Dtos/            # Data Transfer Objects (organised by entity)
│   ├── Providers/           # OAuth / JWT providers and OWIN middleware
│   ├── Repositories/        # Generic repository + Unit of Work
│   ├── Services/            # Business logic services + interfaces
│   │   ├── Converters/      # Entity ↔ DTO converters
│   │   └── Exceptions/      # Custom domain exceptions
│   ├── SwaggerHelpers/      # Swagger example request/response helpers
│   ├── Utilities/           # Cross-cutting concerns (exception filter, identity helpers)
│   ├── Startup.cs           # OWIN startup class
│   └── Web.config
└── eGradeBook.Tests/
    ├── Controllers/         # One example assertion test (effectively empty)
    └── Services/            # Two mostly empty service test stubs
```

---

## 3. Domain Model

The core domain models all live in `eGradeBook/Models/`.

### 3.1 User hierarchy

All user types inherit from `GradeBookUser`, which in turn extends ASP.NET Identity's `IdentityUser<int, ...>` (custom int-keyed identity).

| Class | Role | Notes |
|---|---|---|
| `GradeBookUser` (abstract) | Base user | Has `FirstName`, `LastName`, `Gender` |
| `AdminUser` | admins | No extra properties |
| `TeacherUser` | teachers | Adds `Title`, `Degree`; owns `Teachings` collection |
| `StudentUser` | students | Adds `PlaceOfBirth`, `DateOfBirth`, `ClassRoomId`; owns `Takings` |
| `ParentUser` | parents | Owns `StudentParents` collection |
| `ClassMasterUser` | (unused) | Placeholder; `ClassRoom` has a FK to it but no role is assigned |

### 3.2 Non-user entities

| Entity | Purpose |
|---|---|
| `Course` | A subject (e.g. Mathematics). Name must be unique. |
| `ClassRoom` | A school class (grade 1–8, unique name). Optionally has a ClassMaster. |
| `Teaching` | Association between a `TeacherUser` and a `Course` (teacher ↔ subject, unique pair). |
| `Program` | Association between a `ClassRoom`, a `Course`, and a `Teaching` (classroom curriculum). Contains `WeeklyHours`. |
| `Taking` | Association between a `StudentUser` and a `Program` (student ↔ curriculum entry, unique pair). This is the grading anchor. |
| `Grade` | An individual grade point (1–5) assigned to a `Taking`. Has `SchoolTerm` (1 or 2), `Assigned` date, optional `Notes`. |
| `FinalGrade` | A term-final grade for a `Taking` (unique per Taking + SchoolTerm). |
| `StudentParent` | Many-to-many association between `StudentUser` and `ParentUser`. |

### 3.3 Entity relationship summary

```
TeacherUser ──< Teaching >── Course
                     |
ClassRoom ─────< Program (ClassRoom + Teaching) >──< Taking >── StudentUser
                                                        |
                                                      Grade / FinalGrade
StudentUser ──< StudentParent >── ParentUser
```

---

## 4. Architecture & Patterns

### 4.1 Layered architecture
The project follows a classic three-layer pattern:

1. **Presentation layer** — Web API Controllers (14 controllers)
2. **Business logic layer** — Services (12 services + interfaces)
3. **Data access layer** — Unit of Work + Generic Repository

### 4.2 Repository pattern
- `GenericRepository<TEntity>` provides standard CRUD (`Get`, `GetByID`, `Insert`, `Update`, `Delete`) backed by EF's `DbSet<T>`.
- The `Get` method accepts LINQ `Expression<Func<TEntity, bool>>` filter, optional `orderBy`, and an `includeProperties` string for eager loading.
- `UnitOfWork` aggregates all repositories as `[Dependency]`-injected properties and exposes a single `Save()` method.

### 4.3 Dependency Injection
- **Unity IoC** container (`Unity` 5.11.1) is configured in `Startup.SetupUnity()`.
- All service interfaces and their implementations are registered with transient or hierarchical lifetime.
- `GradeBookContext` uses `HierarchicalLifetimeManager` (request-scoped).
- Services use `Lazy<T>` injected dependencies to break circular dependencies (e.g., `GradesService` takes `Lazy<IEmailsService>`).
- `Unity.NLog` extension is added to auto-inject `ILogger` into services.

### 4.4 DTO / Converter pattern
Each entity has a matching DTO folder under `Models/Dtos/`. Conversion is performed by static converter classes in `Services/Converters/` (e.g., `GradesConverter.GradeToGradeDto`).

---

## 5. Authentication & Authorization

### 5.1 Mechanism
- **OAuth 2 resource owner password credentials** grant: a `/token` endpoint issues JWTs.
- **JWT Bearer authentication** validates tokens on subsequent requests.
- Both are wired in `Startup.ConfigureOAuth` via OWIN middleware.
- Token lifetime is **1 day**.
- `AllowInsecureHttp = true` — tokens are accepted over plain HTTP.

### 5.2 JWT secret
The JWT is signed with HMAC-SHA256 using a **hardcoded** Base64Url-encoded key:
```
IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw
```
The same value appears in both `Startup.cs` and `CustomJwtFormat.cs` — this is a **security risk** in a real deployment.

### 5.3 Roles
Four roles are defined: `admins`, `teachers`, `students`, `parents`.

Role-based authorization is applied at the controller/action level with `[Authorize(Roles = "...")]`. Some endpoints use inline role-dispatch logic (checking `userInfo.IsAdmin`, `userInfo.IsTeacher`, etc.) to return role-appropriate data subsets.

### 5.4 Identity customization
ASP.NET Identity is customised to use `int` primary keys throughout:
- `CustomUserRole`, `CustomUserClaim`, `CustomUserLogin`
- `CustomRole` (extends `IdentityRole<int, CustomUserRole>`)
- `CustomUserStore`, `CustomRoleStore`

---

## 6. Database & ORM

- **Entity Framework 6** Code First with `MigrateDatabaseToLatestVersion` (a single migration: `201908191907038_First`).
- `GradeBookContext` inherits from `IdentityDbContext<GradeBookUser, CustomRole, int, ...>`.
- Cascade deletes are globally disabled (`modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>()`).
- Table-per-type (TPT) inheritance: each user type maps to its own table (`AdminUser`, `TeacherUser`, etc.).
- Unique constraints are enforced via `[Index(IsUnique = true)]` attributes.
- A separate `GradeBookInitializer` (`DropCreateDatabaseAlways`) with full seed data is present but commented out in favour of the standard `Configuration` migration seeder.

### 6.1 Seeder
`GradeBookInitializer.SeedByHand` (and the algorithmic `SeedByAlgorithm`) populate the database with sample data including:
- Roles (`admins`, `students`, `teachers`, `parents`)
- Admin, teacher, student, and parent users
- School courses (Mathematics, Chemistry, Physics, etc.)
- Classrooms (grades 5–8)
- Teaching assignments and Programs
- Student-parent links, Takings, Grades, and FinalGrades

`SeederHelper` provides utilities like `CreateSchoolClasses`, `CreateStudents`, and a `Deal<T>` randomizer.

---

## 7. Controllers

| Controller | Route prefix | Key responsibility |
|---|---|---|
| `AccountsController` | `/api/accounts` | Register/update/delete users; whoami / whois |
| `AdminsController` | `/api/admins` | CRUD for admin users |
| `TeachersController` | `/api/teachers` | CRUD + query for teachers |
| `StudentsController` | `/api/students` | CRUD + query, course assignment, reports |
| `ParentsController` | `/api/parents` | CRUD for parents |
| `ClassRoomsController` | `/api/classrooms` | CRUD for classrooms |
| `CoursesController` | `/api/courses` | CRUD for courses |
| `TeachingsController` | `/api/teachings` | Manage teacher↔course assignments |
| `ProgramsController` | `/api/programs` | Manage classroom curricula |
| `TakingsController` | `/api/takings` | Manage student↔program enrolments |
| `StudentParentsController` | `/api/studentparents` | Manage student↔parent links |
| `GradesController` | `/api/grades` | Create/read/update grades (role-dispatched) |
| `FinalGradesController` | `/api/finalgrades` | Manage final term grades |
| `HiddenController` | `/api/hidden` | Admin-only data reset and manual seed operations |

---

## 8. Services

One service class and interface pair exists for every major entity group:

`UsersService`, `StudentsService`, `TeachersService`, `ParentsService`, `AdminsService`, `ClassRoomsService`, `CoursesService`, `TeachingsService`, `ProgramsService`, `TakingsService`, `StudentParentsService`, `GradesService`, `FinalGradesService`, `EmailsService`.

Notable design choices:
- Many services accept `Lazy<T>` dependencies on sibling services to avoid circular Unity registration errors.
- `GradesService.GetGradesByParameters` uses a composite LINQ predicate over 11 optional filter parameters, supporting rich querying.
- `EmailsService.NotifyParents` sends HTML email notifications via SMTP when a grade is assigned. This feature is **temporarily disabled** (`// emailsService.Value.NotifyParents(grade);`) to avoid SMTP errors during development.

---

## 9. Logging

Two logging frameworks are used simultaneously:

| Framework | Configured in | Target |
|---|---|---|
| **NLog** | `NLog.config` | Per-class static `Logger` instances; used in controllers, services, and repositories |
| **Serilog** | `Startup` constructor | Writes structured JSON logs to `logs/serilog.txt`; optionally to a local Seq instance at `http://localhost:5341` (commented out) |

Serilog is enriched with:
- Correlation Id (`WithCorrelationId`)
- Username (`WithUserName`)
- `UserId` claim value
- Role claim value

A custom OWIN middleware `LoggingMiddleware` is registered to log incoming HTTP requests.

---

## 10. API Documentation

- **Swashbuckle** (Swagger) is integrated via `SwaggerConfig`.
- Accessible at `/swagger` appended to the base URL.
- `SwaggerHelpers/Examples/` contains example request payloads for registration and grade creation endpoints using `Swashbuckle.Examples`.

---

## 11. Error Handling

`CustomExceptionFilter` (an `ExceptionFilterAttribute`) is registered globally in `WebApiConfig`. It maps the following exception types to HTTP status codes:

| Exception | HTTP Status |
|---|---|
| `UnauthorizedAccessException` | 401 Unauthorized |
| `SqlException` | 404 (wraps as HttpResponseException) |
| `EntityException` | 404 (wraps as HttpResponseException) |
| `DbUpdateException` (unique constraint) | 409 Conflict |
| `DbUpdateException` (FK constraint) | 409 Conflict |
| `DbEntityValidationException` | 500 Internal Server Error |
| `ConflictException` (custom) | 409 Conflict |
| `UserRegistrationException` (custom) | 409 Conflict |
| `TeacherNotFoundException` (custom) | 404 Not Found |
| `CourseNotFoundException` (custom) | 404 Not Found |
| `TeachingNotFoundException` (custom) | 404 Not Found |
| All others | 500 Internal Server Error |

A set of 16 custom exception types lives in `Services/Exceptions/`.

---

## 12. Testing

The test project `eGradeBook.Tests` is essentially a placeholder:
- `Controllers/AssertExampleTest.cs` — a single trivially passing `[TestMethod]` with no assertions.
- `Services/TeachersServiceTest.cs` — one empty `[TestMethod]`.
- `Services/TeachingServiceTest.cs` — not read but expected to be similarly empty.

**There is no meaningful test coverage.** The author acknowledged the code is not production-ready.

---

## 13. Dependencies (NuGet packages)

| Package | Version | Purpose |
|---|---|---|
| EntityFramework | 6.1.0 | ORM |
| Microsoft.AspNet.Identity.* | 2.2.2 | User management |
| Microsoft.AspNet.WebApi.* | 5.2.x | Web API framework |
| Microsoft.Owin.* | 4.0.1 | OWIN pipeline |
| Microsoft.Owin.Security.OAuth | 4.0.1 | OAuth token endpoint |
| Microsoft.Owin.Security.Jwt | 4.0.1 | JWT validation |
| System.IdentityModel.Tokens.Jwt | 5.5.0 | JWT generation |
| Microsoft.IdentityModel.Tokens | 5.5.0 | Signing credentials |
| Unity / Unity.WebAPI | 5.11.1 / 5.4.0 | Dependency injection |
| Unity.NLog | 5.11.1 | NLog integration for Unity |
| NLog | 4.6.6 | Logging |
| Serilog + sinks (File, Seq) | 2.8.0 | Structured logging |
| Swashbuckle / Swashbuckle.Examples | 5.6.0 / 3.12.0 | Swagger API docs |
| Newtonsoft.Json | 12.0.2 | JSON serialisation |
| Microsoft.AspNet.Cors | 5.0.0 | CORS support |

---

## 14. Configuration

- **Database connection string** is named `DefaultConnection` (defined in `Web.config`, pointing to a local SQL Server / LocalDB instance).
- **SMTP settings** (`from`, `smtpServer`, `smtpPort`, `smtpSsl`, `password`) must be set in `Web.config` for email notifications to work.
- **JWT issuer** is hardcoded to `http://localhost/`.
- **CORS** is configured to allow all origins (`CorsOptions.AllowAll`).

---

## 15. Notable Issues / Technical Debt

The codebase contains several explicit `TODO` comments and known shortcomings:

1. **Hardcoded JWT secret** — The signing key appears in plain text in two source files. This must be moved to configuration and kept out of source control for any real deployment.
2. **`AllowInsecureHttp = true`** — The OAuth server accepts tokens over plain HTTP.
3. **Empty Unity DI config in `UnityConfig.cs`** — The legacy `App_Start/UnityConfig.cs` is a stub; the actual DI wiring happens in `Startup.SetupUnity()`.
4. **`ClassMasterUser`** — The model and table exist but the role is not registered and the type has no behaviour.
5. **`GradeBookInitializer` vs migrations** — The `DropCreateDatabaseAlways` initialiser exists alongside EF migrations; the seeder is partially commented out, creating confusion about the intended startup flow.
6. **Email notifications disabled** — `emailsService.Value.NotifyParents(grade)` is commented out in `GradesService`.
7. **`GetGradesByCourses` is misleadingly named** — It returns all grades with no grouping.
8. **No real tests** — All test methods are empty stubs.
9. **`Program.TakingStudents`** — The comment says "TODO URGENT" — navigating directly to students rather than Takings is modelled incorrectly.
10. **`UnauthorizedAccessException` thrown from controllers** — Rather than returning `IHttpActionResult` with 401, some actions `throw new UnauthorizedAccessException(...)`. These are caught by `CustomExceptionFilter`, but the pattern is inconsistent across the codebase.
11. **`FetchUserData` returns `UserId = 0` for unauthenticated users** — `int.TryParse` silently defaults to 0 on failure; `RequestUserData.UserId` is declared as `int?` but some callers still use `?? 0`.
