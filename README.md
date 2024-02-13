# Library Management System

This program is a simple albeit comprehensive Library Management System using the WPF .NET Framework. It is written in C# and uses a MySQL Database to store user and book information.

## Current Features

It contains features such as:
* Login and account registration menus.
* Password encryption using BCrypt.
* A comprehensive admin page that allows library employees to issue, return, add and delete books, manage users, charge library fees, and more.
* A user page that allows users to see what books are currently loaned to their account, browse the library catalog, pay fees, and more.

## How to Install

### 1. Clone Repository:

   ```bash
   git clone https://github.com/ColtonJudy/Library-Management-System
   cd .\Library-Management-System\
   ```

### 2. Setup Database

   This setup assumes that you have MySQL Workbench 8.0 CE installed. If you do not have it installed, you can download it here: https://dev.mysql.com/downloads/workbench/
   
   I have placed the SQL files for the database in a ZIP folder named `Database Files.zip`. Once you have those extracted, open MySQL Workbench and create a new MySQL Connection.

   After you have the connection open, right click in the SCHEMAS navigator and select Create Schema. Name it lmsdb, and select Apply.

   Once you have the lmsdb schema created, navigate at the top to Server > Data Import. Select the folder where you extracted the database files to, and click Start Import.


### 3. Create App.config

   You will need to create an App.config file in the main project directory, in order to store the connection string for the database. It should look like the following:
   ```
   <?xml version="1.0" encoding="utf-8" ?>
   <configuration>
     <connectionStrings>
       <add name="connectionString" connectionString="server=localhost;database=lmsdb;user=root;password=databasePassword"/>
     </connectionStrings>
   </configuration>
   ```
   Be sure to replace databasePassword with the correct password for your MySQL Server.

### 4. Build and Run Project

   Once you have the config file added, open Library Management System.sln in Visual Studio. If you do not have Visual Studio downloaded, you can download it here: https://visualstudio.microsoft.com/downloads/
   
   You may need to install the [MySql.Data](https://www.nuget.org/packages/MySql.Data/) and [BCrypt.Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next) packages using the Nuget Package Manager.

   You should now be able to build and run the application!

### 5. Logging in

   The default admin credential for the system is `admin` for both the username and password. Once logged in, you should have access to all of the admin functionality!

   New users can be added later in the Create New Account Menu.

## Features to be added later
* An option for admins to reset user account passwords, and for users to change their password and account information.
* An option for admins to upgrade and downgrade user account privileges.
* An admin feature to import large amounts of books and users from CSV.
* A button for admins to contact users via email.
* Improved book information (multiple authors, year published, publisher information, etc.)
* Automatic late fees after a certain amount of time.
* Search functionality for books and users.
* Improved UI Appearance.
