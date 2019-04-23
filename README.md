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
To install the following prerequisites, consider consulting the official docs.

- Download & Install docker
- Download & Install docker-compose
- Download & Install the dotnet-sdk
- Download & Install node and npm

### Running everything together
To run everything together you can use the predefined development `docker-compose.yml` file. Please make sure that you place some music into the ./data/ directory. The docker-compose file is configured to grab the MP3 files from this directory.

To spin up all the services at once run the following commands in the base directory of this repository:

`docker-compose build`  
`docker-compose up`

To shutdown all the services run the following command:

`docker-compose down`

Running every service at once is very helpful for end to end testing. But for debugging and development it can also be helpful to spin up separate development instances of each service manually. To do this follow the rest of this development guide.

### Setup local development database
Run the following command to spin up a separate local development instance of PostgreSQL:

`docker run --rm -d --name local_db -v local_db_volume:/var/lib/postgresql/data -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2`

If you are doing this for the first time, you need to create a volume for the local database, where the data will be persisted. To do this, run the following command first:

`docker volume create local_db_volume`

The local database is now up and running as a container on your local machine. To access the database via the CLI use the following command:

`docker exec -it local_db psql -h localhost -U radio`

### Setup local message queue
Run the following command to spin up a separate local development instance of RabbitMQ:

`docker run --rm -d --name local_messaging -p 5672:5672 rabbitmq:3.7`

The local message queue is now up and running as a container on your local machine. To access the rabbitmqctl use the following command:

`docker exec -it local_messaging rabbitmqctl <OPTIONS>`

### Setup local LiquidSoap
Run the following command from the base directory to build the docker-image for LiquidSoap:

`docker build -t local_playback ./playback`

Run the following command to spin up a separate local development instance of LiquidSoap:

`docker run --rm -d --name local_playback -v local_data_volume:/home/liquidsoap/music -e BACKEND_HOST=<HOST OF BACKEND> -e BACKEND_PORT=<PORT OF BACKEND> -e ICECAST_HOST=<HOST OF ICECAST> -e ICECAST_PORT=<PORT OF ICECAST> -e ICECAST_SOURCE_PASSWORD=<CONFIGURED ICECAST SOURCE PASSWORD>`

### Setup local Icecast
Run the following command from the base directory to build the docker-image for Icecast:

`docker build -t local_streaming ./streaming`

Run the following command to spin up a local development instance of Icecast:

`docker run --rm -d --name local_streaming -p 8000:8000 -e ICECAST_LOCATION=<LOCATION> -e ICECAST_ADMIN_EMAIL=<ADMIN EMAIL> -e ICECAST_MAX_CLIENTS=<MAX NUMBER OF CLIENTS> -e ICECAST_SOURCE_PASSWORD=<SOURCE PASSWORD> -e ICECAST_RELAY_PASSWORD=<RELAY PASSWORD> -e ICECAST_ADMIN_USERNAME=<ADMIN USERNAME> -e ICECAST_ADMIN_PASSWORD=<ADMIN PASSWORD> -e ICECAST_HOSTNAME=<HOSTNAME> -e ICECAST_PORT=8000 -e ICECAST_PUBLIC=<0 or 1> -e ICECAST_STREAM_NAME=<STREAM NAME> -e ICECAST_STREAM_DESCRIPTION=<STREAM DESCRIPTION>`

### Backend
The backend is built with dotnet core and split apart into three different startup projects.
- Radio.Startup.Web.Internal (Backend for LiquidSoap)
- Radio.Startup.Web.External (Backend for Frontend)
- Radio.Startup.Console (Synchronization Jobs)

Additionaly there are two test projects.
- Radio.Tests.Unit
- Radio.Tests.Integration

#### Configuration
Please make sure that the configuration values in **`appsettings.json`** are set accordingly for the following steps.
The appsettings can be found in every Startup- or Test-Project. 

- DbConnectionString: PostgreSQL Connection String for the App
- MigrationsDbConnectionString: PostgreSQL Connection String for running migrations at startup
- MessagingHost: Hostname or Address of RabbitMQ Server
- DataDirectoryPath: Path to the local music data directory

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

#### Configuration
Please make sure that the configuration values in **`public/config.js`** are set accordingly for the following steps.

- apiBaseUrl: The url of the backend api, accessible from the browser
- streamBaseUrl: The url of the streaming server, accessible from the browser

#### Running the frontend
To run the frontend dev server run the following commands:

`npm install`  
`npm start`

#### Running the tests
To run the frontend tests use the following commands:

`npm install`  
`npm test`

