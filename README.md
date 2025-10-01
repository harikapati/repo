
# ATM Application

Welcome to the ATM Application! This project is a simple, web-based ATM simulator that lets you manage two bank accounts—Checking and Savings—from your browser.

## What Can You Do?
- **Deposit** money into Checking or Savings
- **Withdraw** money from either account
- **Transfer** funds between accounts
- **View balances** and see your full transaction history

## Tech Stack
- **Backend:** .NET 6 (C#), ASP.NET Core Web API
- **Frontend:** React (via CDN, no build tools required)
- **Testing:** xUnit for backend automation tests
- **Persistence:** In-memory (data resets when server restarts)

## How to Run

### 1. Backend (API)
1. Open a terminal and go to the `backend` folder:
	```powershell
	cd backend
	```
2. Build and start the server:
	```powershell
	dotnet build
	dotnet run
	```
3. The API will run on `http://localhost:5000` (or as shown in the console).

### 2. Frontend (Web UI)
1. Open `frontend/index.html` in your browser.
2. The app will connect to the backend API automatically.

### 3. Running Tests
1. Go to the root project folder.
2. Run:
	```powershell
	dotnet test tests/ATMApplication.Tests.csproj
	```
3. All unit tests for deposit, withdraw, transfer, and transaction history will run and show results.

## Project Structure
- `backend/` — .NET Core Web API (business logic, models, controllers)
- `frontend/` — React app (UI, API calls)
- `tests/` — xUnit tests for backend logic

## Notes 
- This is a single-user demo (no login/authentication).
- All data is stored in memory—refreshing or restarting the backend will reset accounts and history.
- Error handling and input validation are built-in for a smooth experience.
- For development, CORS is enabled so the frontend can talk to the backend.

