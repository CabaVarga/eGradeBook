# eGradeBook
Grade book project for the course

The project includes some quite helpful Swagger support. You need to append */swagger* to your base path, in order to use that funcionality. Example: *http://localhost:50663/swagger*.  

Another interesting feature is [Serilog](https://serilog.net/) logging to [Seq](https://datalust.co/seq). You need to install Seq if you want to follow the realtime logging and advanced filtering features. For Windows 7 users you will need an earlier version, 4.2+. Once installed, it can be accessed through *http://localhost:5341*.  

One more quirk of my solution is the customization of Identity for working with *int* based keys.  

Important **disclaimer**: the current state of the code base is not ready for any kind of educational or productive usage. I'm making the repository public in order to share my implementation with those of my colleagues after the stressful past few weeks. I'll try to refactor the project and clean it up, in the upcoming weeks.  
