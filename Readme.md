### About
- A hypothetical background check service for practise purpose
- Domain responsibility is to maintain life cycles of background checks.

### Domain Ideas
- An imaginary background check domain.
- Create a background check invitation.
- When background check result pass, automatically move applicant to the next step of recruitment process.
- If a particular type of background check result fails, notify authority (?)
- More information required following initial invitation creation
- Pass / Fail / On Hold / In Progress / Finish / etc.
- Background check progress view
  - Candidate ("In Progress" without further details?)
  - Recruiter (more detailed steps?)


### Run Locally 
From root directory:
#### Build
```
dotnet build
```

#### Run BackgroundCheck Service API
```
cd 2_Service\MyBackgroundCheckService.Api
dotnet run
```

#### Run BackgroundCheck Provider API
```
cd 3_Provider\BackgroundCheckProvider.Api
dotnet run
```

#### Initiate a background check invitation
- Update `monolith-invitation.json`
```
cd 1_Monolith/Monolith.BackgroundCheck
dotnet bin/Debug/netcoreapp2.0/Monolith.BackgroundCheck.dll
```

#### Update a background check status
```
cd 2_Service/MyBackgroundCheckService.StatusUpdator
dotnet bin/Debug/netcoreapp2.1/MyBackgroundCheckService.StatusUpdator.dll
```

