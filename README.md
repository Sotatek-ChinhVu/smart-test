# emr-cloud-be
# Backend SMART-Karte Web application

## How to install
Clone project to local using git command or download a zip file

## Dependencies
See README.md on 
1 - API.EmrCloudApi for the API configuration
1 - API.EmrCalculateApi for the API configuration by Customer
2 - Domain.Domain Project for Model & repository interface,
2 - Domain.Interactor Project for handling business logic use case . receive input Usecase & return Output Usecase.
2 - Domain.UseCase Project define use cases with input and output
3 - Infrastructure.Infrastructure Project for repository implementation.
4 - Database.Entities Project for entity ORM 
4 - Database.PostgreDataContext Project contain DataContext & migrations snapshot
5 - Helper.Helper Project for common code , enum helper, extendsion & mapping object
6 - Tests.UnitTest project for X-unit testing

## How to run
1. Go to Appsettings.json in API.EmrCloudApi Project . Set value for "TenantDbSample" key is database connection string.
2. 
	2.1. With Visual studio : 
	Build Project : right click API.EmrCalculateApi => Build or Rebuild.
	Run project :
		right click API.EmrCalculateApi => view => view in browser to run normal.
		right click API.EmrCalculateApi => Set as startup project => F5 or click green triangle icon on top middle to run debug;
		
	2.2. With VS code :
		open folder project & terminal and run command. 
		 1. "cd EmrCloudApi" to go internal EmrCloudApi Project
		 2. "dotnet build" to build project
		 3. "dotnet run" to run.