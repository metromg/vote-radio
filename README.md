[![Build Status](https://travis-ci.org/michaelguenter/vote-radio.svg?branch=master)](https://travis-ci.org/michaelguenter/vote-radio)

# Vote Radio
A web radio with song voting

## Overview
Vote Radio is a web radio with song voting. Songs are selected automatically based on user votes.

It uses Icecast for streaming the media to clients and LiquidSoap as a streaming source.
Everything is packaged into docker-images for easy deployment.

This is the rough data flow for broadcasting:
- LiquidSoap asks the internal backend what MP3 file should be played
- The backend replies with an MP3 file based on voting results
- LiquidSoap plays the song, encodes the stream, and sends the audio to Icecast
- Icecast streams the audio to clients

Please note, that only MP3 files are supported.

## Usage & Installation
TODO: Describe installation on production

## Development
To run the development environment on your local machine, just `git clone` this repository and follow these steps:

### Prerequisites
Install `docker` and `docker-compose` on your local machine.
To do this follow the official documentation.

### Setup local development database
Run the following command to spin up a local development instance of PostgreSQL:

`docker run --rm -d --name local_db -v local_db_volume:/var/lib/postgresql/data -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2`

If you are doing this for the first time, you need to create a volume for the local database, where the data will be persisted. To do this, run the following command first:

`docker volume create local_db_volume`

The local database is now up and running as a container on your local machine. To access the database via the CLI use the following command:

`docker exec -it local_db psql -h localhost -U radio`

### Setup local message queue
Run the following command to spin up a local development instance of RabbitMQ:

`docker run --rm -d --name local_messaging -p 5672:5672 rabbitmq:3.7`

The local message queue is now up and running as a container on your local machine. To access the rabbitmqctl use the following command:

`docker exec -it local_messaging rabbitmqctl <OPTIONS>`

### Backend
The backend is built with dotnet core and split apart into three different startup projects.
- Radio.Startup.Internal (Backend for LiquidSoap)
- Radio.Startup.External (Backend for Frontend)
- Radio.Startup.Console (Synchronization Jobs)

Additionaly there are two test projects.
- Radio.Tests.Unit
- Radio.Tests.Integration

**Please** make sure that the configuration values in **`appsettings.json`** are set accordingly for the following steps.

#### Running the backend
To run the startup projects use the following command:

`dotnet run -p <PATH_TO_CSPROJ>`

#### Running the tests
To run the unit or integration tests run the following command:

`dotnet test <PATH_TO_CSPROJ>`

Note: To run the integration tests, it is recommended to spin up a new local database without a volume attached, so that the test data is not persisted anywhere.

`docker run --rm -d --name local_db -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2`

#### Database migrations
To add a new database migration run the following commands:

`cd ./backend/src/Radio.Startup.Console`  
`dotnet ef migrations add <NAME> -p ../Radio.Infrastructure.DbAccess`

To update the database to the latest migration run the following commands:

`cd ./backend/src/Radio.Startup.Console`  
`dotnet ef database update -p ../Radio.Infrastructure.DbAccess`

### Frontend
The frontend is built with react. It follows the guidelines of create-react-app.  
**Please** make sure that the configuration values in **`public/config.js`** are set accordingly for the following steps.

#### Running the frontend
To run the frontend dev server run the following command:

`npm start`

#### Running the tests
To run the frontend tests use the following command:

`npm test`

### Running everything together
To run everything together you can use the predefined development `docker-compose.yml` file. Please make sure that you place some music into the ./data/ directory. The docker-compose file is configured to grab the MP3 files from this directory.

To spin up all the services at once run the following commands in the base directory of this repository:

`docker-compose build`  
`docker-compose up`

To shutdown all the services run the following command:

`docker-compose down`