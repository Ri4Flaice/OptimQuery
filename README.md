# OptimQuery

## Project Description
Optimize the GET request to quickly retrieve data from a table with 10 million records.

---

## Goals and objectives
- The main goal: the response time should be as short as possible, while not using caching, a clean query in the database.
- Tasks:
  - Optimize SQL queries.
  - Use cursor pagination instead of OFFSET.
  - Configure indexes to improve performance.
 
---

## Technologies
List of technologies and tools used:
- **Programming language**: ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

- **Framework**: ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

- **Database**: ![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)

- **ORM**: ![EF Core](https://github.com/karenpayneoregon/ef-core5-logging/blob/master/assets/efcore.png)
 
- **Containerization**: ![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

---

## Architecture and optimization approach
1. **The original problem**:
   - A regular GET request for getting data using an offset and without indexing. 
Thus, getting the initial pages is not a problem, but with data growth, when there are 10 million rows in the table and getting data on 100 pages is already a performance problem.

2. **Decision**:
   - Switching to cursor pagination.
   - Using indexes.

3. **Result**:
   - The execution time has been significantly reduced to less than 1 ms, even on large pages, if you query directly through SQL, and when accessing data through the API, you can get up to ~5 ms.

---

## Installation and configuration
Instructions for running the project locally:
1. **Requirements**:
   - Docker.
   - Git.

2. **Cloning a repository**:
   ```bash
   git clone https://github.com/Ri4Flaice/OptimQuery.git

3. **Project Launch**
   ```bash
   docker-compose up --build -d

5. **Request**
  - Open Swagger via a browser by http://localhost:8081/swagger or make a request directly by http://http://localhost:8081/users
  - When making a request, you can specify a limit and cursor, the maximum limit value is 100, and cursor is not defined at the beginning, so in subsequent requests it will be in the response body.

## Note
When starting a project, if there are no records in the database, then 10 million records will be automatically created.
