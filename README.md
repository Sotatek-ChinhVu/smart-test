# emr-cloud-be
# Backend SMART-Karte Web application

## How to install
Clone project to local using git command or download a zip file

## Dependencies
See README.md on 
- API.EmrCloudApi for the API configuration
- Domain.Domain Project for Model & repository interface,
- Domain.Interactor Project for handling business logic use case . receive input Usecase & return Output Usecase.
- Domain.UseCase Project define use cases with input and output
- Infrastructure.Infrastructure Project for repository implementation.
- Database.Entities Project for entity ORM 
- Database.PostgreDataContext Project contain DataContext & migrations snapshot
- Helper.Helper Project for common code , enum helper, extendsion & mapping object
- Tests.UnitTest project for X-unit testing

## How to run
- Go to Appsettings.json in API.EmrCloudApi Project . Set value for "TenantDbSample" key is database connection string.
- With Visual studio : 
Build Project : right click API.EmrCalculateApi => Build or Rebuild.
Run project :
	right click API.EmrCalculateApi => view => view in browser to run normal.
	right click API.EmrCalculateApi => Set as startup project => F5 or click green triangle icon on top middle to run debug;
		
- With VS code :
	open folder project & terminal and run command. 
	- ```cd EmrCloudApi``` to go internal EmrCloudApi Project
	- ```dotnet build``` to build project
	- ```dotnet run``` to run.
