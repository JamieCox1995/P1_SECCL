# P1_SECCL

This project was created using Visual Studio 2022 and .NET 6.0. Please install the latest version of Visual Studio to run the application.
The project is provided as a VS Solution and can be run through the debugger. A .EXE has also been provided in the solution which can be run.

## Running the Application
You can either run the .EXE file located within the "Build" folder which will run the applciation without any debugging, or you can run the app through VS to enable debugging.

## Project Structure
This repo contains a single C# WinForms solution called "P1FormsApp" used to create the application, and a "Build" folder which contains an executable file for a built version of the application. Within the solution there are 3 folders; Classes, Forms, and Services.

### Classes
Classes contains the data structure classes used throughout this project. 
- APIResponse.cs is a generic class which is used to return the data from the SECCL APIs and is then handled by the Middleware to convert it into useful classes.
- Authentication contains the AuthenticationToken and AuthData classes. AuthenicationToken is used to sort a valid Auth Token retrieved by the Authentication API.
- Portfolio contains all of the structures used to store data regarding a Firm's portfolios, and it's data. This class file contains the following structures:
  - PortfolioAccount; contains the header level data for a Firms Portfolio, this is retrieved by the an API which loads all Portfolios for a Firm
  - PortfolioSummary; detail level for a Portfolio containing data for Positions, Accounts, and Completed Transactions.
  - PortfolioPosition;
  - SubAccount; contains data for individual accounts/banking objects.
  - WrapperDetail;
  - Product
 
### Forms
Forms contains the PortfolioViewer class, which is used to construct and handle the GUI.

### Services
Services contains the folders API and DataHandling
- API; contains the interface file "IAPIService" and the concrete implementation "APIService". These are used for creating methods to call GET and POST calls to the SECCL RESTful APIs.
- DataHandling; contains the interface file "IMiddleware" and the concrete implementation "Middleware". These used to create all of the data processing for the GUI, such as calling different API's using the APIService and then convert to JSON returned from those APIs into relevant structures which the application can display to the screen.

Finally Program.cs is used start the application. Inside of here, it creates the service to use the Middleware for the rest of the application.
