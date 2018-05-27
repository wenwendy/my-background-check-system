### About
A hypothetical background check service for practise purpose

### Domain Ideas
- An imaginary background check domain
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

#### Run
```
dotnet run --project=1_Monolith/Monolith.Api/
dotnet run --project=2_Service/MyBackgroundCheckService.Api/
dotnet run --project=3_Provider/BobBackgroundCheckProvider.Api/
cd 2_Service/MyBackgroundCheckService.Processor
dotnet bin/Debug/netcoreapp2.0/MyBackgroundCheckService.Processor.dll
```

#### Initiate a background check invitation
- Update `monolith-invitation.json`
```
cd 1_Monolith/Monolith.BackgroundCheck
dotnet bin/Debug/netcoreapp2.0/Monolith.BackgroundCheck.dll
```

#### Send a background check result
- Update `provider-result.json`
```
cd 3_Provider/BobBackgroundCheckProvider.Processor
dotnet bin/Debug/netcoreapp2.0/BobBackgroundCheckProvider.Processor.dll
```

