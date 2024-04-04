# ProductMicroservice
Some points should be done during development of this project:
1- to create database ProductsDB run the follwoing command in the NuGet Packge
Manager Consule:
▪ Open Package Manager Console from VS
▪ Run : PM> Add-Migration InitialCreate (this creates Migration folder and
initialCreate.cs file in VS
▪ Run : PM> Updae-Database (this creates database ProductsDB)
In case you get Permission denied in Master database to create databse do the followings:
▪ Open Sql Server managerment studio (login with Sql admin (sa))
▪ Under Security:Logins: find BUILTIN\User
▪ Doubble click on to see Login Properties
▪ Select Server Roles and select dbcreator and sysadmin (as shown in the following
image) and press to OK 
Run again the PM> Updae-Database
This time the database: ProductsDB shall be created as shown in the SSMS:

Testing this program:
Start from VS via IIS express and then copy URL and test with postman by using this URL and chosing  Get, Post, Put, Delete


Testing with post man you should use as following formats :
Use “Name ” : “Iphone” NOT Name : “Iphone”
This project with using of VS and runing with IIS Express going fine.
But with Runing with docker has follwoing problem with connection to the database:
# this comment is from vs code

