# Fitness Management System (FMS) Documentation

## System Overview
The Fitness Management System (FMS) is a web-based application designed to manage gym operations. It caters to three main user roles: **Admin**, **Trainer**, and **Member**. The system facilitates member management, trainer scheduling, workout/diet planning, and progress tracking.

## Functionalities

### Admin
1.  **Login**: Secure access to the admin dashboard.
2.  **Dashboard**: Overview of system stats (Total Members, Trainers, Appointments, Revenue).
3.  **Manage Members**: View list of members and delete member accounts.
4.  **Manage Trainers**: View list of trainers, create new trainer accounts, and delete trainers.
5.  **Trainer Scheduling**: Assign weekly shifts to trainers.
6.  **Monitor Feedback**: View feedback submitted by members.

### Trainer
1.  **Login**: Secure access to the trainer dashboard.
2.  **Dashboard**: View upcoming appointments and quick actions.
3.  **My Schedule**: View assigned weekly shifts.
4.  **Workout Plan Management**: Create personalized workout plans for members.
5.  **Diet Plan Management**: Create personalized diet plans for members.
6.  **Attendance Marking**: Mark appointments as "Completed" or "No-Show".
7.  **View Member Progress**: Track progress records of members.

### Member
1.  **Registration/Login**: Sign up and log in to the member portal.
2.  **Dashboard**: View active membership, upcoming sessions, and assigned plans.
3.  **Book Session**: Book training sessions with available trainers.
4.  **My Plans**: View assigned workout and diet plans.
5.  **Feedback**: Submit feedback for completed sessions.

## Testing Credentials

### Admin
-   **Email**: `admin@fms.com`
-   **Password**: `Admin@123`

### Trainer
-   **Email**: `trainer@fms.com`
-   **Password**: `Trainer@123`

### Member (Sample)
-   **Email**: `member@fms.com` (You may need to register this user first if not seeded)
-   **Password**: `Member@123`

## Technical Stack
-   **Framework**: ASP.NET Core MVC
-   **Database**: Entity Framework Core (SQL Server/LocalDB)
-   **Frontend**: Bootstrap 4, jQuery
-   **Authentication**: ASP.NET Core Identity

## Setup Instructions
1.  Open the solution in Visual Studio or VS Code.
2.  Update the connection string in `appsettings.json` if necessary.
3.  Run `Update-Database` in the Package Manager Console to apply migrations.
4.  Run the application. The database will be seeded with initial Admin and Trainer accounts.
