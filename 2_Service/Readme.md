### My Background Check Service
- Api
  - POST: Create a background check invitation
  - GET: Query the status of an existing invitation
  - PUT: Update the status of an existing invitation
- Database setup
  - Run PostgreSQL `docker run -p 2345:5432 --name wwpostgres -e POSTGRES_PASSWORD=abc123 -d postgres`
  - Restart PostgreSQL `docker start wwpostgres`
  - Create DB and Tables
    - `docker run -it --rm --link wwpostgres:postgres postgres psql -h postgres -U postgres`
    - Or, use pgAdmin UI
- psql cheatsheet
  - List all exisitng DBs `\l`
  - Create a DB `CREATE DATABASE background_check;`
  - Switch to a DB `\c [database_name];`
  - List all existing tables `\dt`
  - Create a table 
  ```
    CREATE TABLE invitation (
      invitation integer,
      applicant_profile json NOT NULL,
      status varchar(20),
      PRIMARY KEY (invitation)
    );
  ```
  - Check table schema `\d [table_name]`
  - Insert a record
  ```
    INSERT INTO invitation (invitation, applicant_profile)
    VALUES (111, '{"name": "test", "education": "tertiary"}');
  ```
  - Upsert a record
  ```
    INSERT INTO invitation (invitation, applicant_profile)
    VALUES (111, '{"name": "test", "education": "tertiary", "address": "abc street"}')
    ON CONFLICT (invitation)
    DO
      UPDATE
        SET applicant_profile = EXCLUDED.applicant_profile;
  ```
  - Update a record
    `UPDATE invitation SET status = 'In Progress' WHERE invitation = 111;`
  