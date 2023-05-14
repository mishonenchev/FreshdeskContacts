# FreshdeskContacts
 Console app for adding github accounts to freshdesk contacts

## The project is implemented using:
 **EntityFramework Core**: For database creation and repository pattern  
 **Dependency injection**: The built-in dependency injection for .Net Core applications for injecting repositories and services  
 **AutoMapper**: For mapping DTOs to EntityFramework Models - saving the github user data in SQLExpress Database  

## Unit tests:
 Unit tests are implemented using **XUnit** and **FluentAssertions**  
 Only few unit tests have been implemented as of now - For Creating and Updating GithubUser in the database using the repository and service  
 Running the tests is done using the Visual Studio's [*Run Tests*] button.  

## App.config
 The 'App.config' file contains the connection string for the database, the GithubAPI token and the FreshDeskAPI token:
 ```
 <configuration>
	<connectionStrings>
		<add name="DefaultConnection" connectionString="Server=.\SQLEXPRESS;Database=ContactsDB;Trusted_Connection=True;Encrypt=False;" />
	</connectionStrings>
	<appSettings>
		<add key="GithubToken" value="YOURTOKENHERE" />
		<add key="FreshdeskToken" value="YOURTOKENHERE" />
	</appSettings>
 </configuration>
 ```
 > Input your valid ConnectionString, GithubToken and FreshdeskToken before running the program.