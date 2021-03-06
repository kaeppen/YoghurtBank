![example workflow](https://github.com/kaeppen/YoghurtBank/actions/workflows/build-and-test.yml/badge.svg?branch=main) <br>
![C#](https://img.shields.io/badge/c%23-%23239120.svg??style=flat-square&logo=appveyor&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=flat-square&logo=appveyor&logo=.net&logoColor=white)

YoghurtBank 
--------------------------------------------------------------------------------------------

Requirements for running the program: 
- dotnet 6.0 
- Docker 
- Powershell
- No docker containers running on port 5432
- Make sure that port 7095 is not already in use (the website will use this port)
- If PostgreSQL is installed, all of it's background processes needs to be terminated 

How to run the program: 
- Clone or download the repository 
- Navigate to the root directory 
- Run the file "start.ps1" with Powershell
- Open your webbrowser and go to the url: "https://localhost:7095/" 

Test user credentials have been provided in the report associated to this program. 


--------------------------------------------------------------------------------------------

A large part of the development was done in this repository: https://github.com/kaeppen/YoghurtBank1.0 for history

--------------------------------------------------------------------------------------------

Parts of this repository has provided inspiration for the project: https://github.com/ondfisk/BDSA2021

--------------------------------------------------------------------------------------------
