# EmployeeVacationCalendar

Simple interactable employee vacation calendar web application built using ASP.NET Core MVC with simplified domain driven-design. Due to simplicity of the project some logic is done in controllers instead of service layer. Libraries are built using .NET Standard SDK. Web project backend is mainly used for views routing. Frontend calendar is build using React. Bower is used for web package manager. Layout design is made with CSS Grid.

</br>

## Getting started

### Requirements

* Visual Studio 2017
* .NET Standard 2.0 SDK
* .NET Core 2.0 SDK
* SQL Server Express 2016 LocalDB
* IIS

### How to use

* Open project
  * First load will be longer due to bower and nuget restoring web packages. 
* Build the solution.
  * Build has to be successful with 0 errors.
* Run application on IISExpress
  * First load will be longer due to database context code first migrations.
* Use the application
  * By default 2 users are created with migrations
    * Username: User Password: pass (User role)
    * Username: Admin Password: pass (Admin role)
  * Not logged in users can only view calendar and navigate through year, month and pages
  * Logged in user can interact only with his days on calendar
  * User with Admin role can interact with all employees calendar and can add new employees up in header
  * First and last name is clickable for changing name
  * Days are clickable for taking vacation (Vacation or sick type)
  
</br>

## Info

All functionalities from specification should be implemented. It's not implemented exactly as specified because I either didn't like the way it would look (for example calendar view is not exactly y and x axis) or didn't like the code i would have to write in order to support it due to architecture of project (for example changing name and taking vacations. I separated those). I guess that shouldn't be a problem. Contact me if you cannot run it or there is another dependency which I did not put. Make sure you check my other projects ([UMemory](https://github.com/Kahath/UMemory) and [KNet](https://github.com/Kahath/KNet)) :) 
