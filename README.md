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
To install vote radio on production follow these steps:

### Prerequisites
Vote Radio is packaed into multiple docker-images. For optimal performance you should run the software on a linux based system (Debian, CoreOS, etc.) where containers can run natively.

To install the following prerequisites, consider consulting the official docs.

- Download & Install docker
- Download & Install docker-compose
- Make sure the current date and time on your system is correct

### Step 1: Adding songs
First, you should place at least 3 MP3 files into a data directory on your target system.

Follow this data structure:

- Create a directory per album
- Place the MP3 files into the album directory
- Optionally, place the cover image into the album directory
- Read + Execute permissions on all album directories and Read permissions on all files
- Files not contained in an album directory will be ignored
- Subdirectories of top level album directories will be ignored

The MP3 files have to meet the following requirements:

- MP3 format
- Title-Tag has to be present
- Album-Tag has to be present
- Artist-Tag has to be present
- The duration must be at least 30 seconds

The cover images have to meet the following requirements:

- JPG or PNG format

### Step 2: Configuration
To configure vote radio you can use the [docker-compose.template.yml](https://raw.githubusercontent.com/michaelguenter/vote-radio/master/docker-compose.template.yml) file as a template. This docker-compose file contains all necessary services to run vote radio (including the database).

Please replace all \<\<Parameters\>\> in the file with your matching configuration values:

- `Frontend Port`: The port of the frontend
- `Public url to external backend`: URL to the external backend, accessible from the browser
- `Public url to streaming server`: URL to the streaming server, accessible from the browser
- `Backend Port`: The port of the backend
- `Database User`: Username to access the database
- `Database Password`: Secure password to access the database
- `Database Name`: The name of the database
- `Path to data directory`: Absolute path to the music data directory
- `Streaming Server Port`: The port of the streaming server
- `Icecast Location`: Icecast Location information
- `Icecast Admin Email`: E-Mail address of the administrator
- `Icecast Max-Clients`: Maximum number of listeners
- `Icecast Source Password`: Secure password for LiquidSoap
- `Icecast Relay Password`: Secure password for optional icecast relays
- `Icecast Admin Username`: Username for the web interface
- `Icecast Admin Password`: Password for the web interface
- `Icecast Hostname`: Public domain
- `Icecast Public`: 0 or 1, depending on whether you want your radio listed in the icecast directory.
- `Icecast Stream Name`: Name of the stream
- `Icecast Stream Description`: Description of the stream

Please make sure, that you replace all occurances of the parameters and that there are no more \<\<\>\> signs in the file.

### Step 3: Startup
You can start vote radio by running the following command in the directory where your docker-compose.yml file is located.

`docker-compose up -d`

This will download and start all necessary services.
Vote Radio will create the database import the metadata from your data directory. The frontend should be accessible in a few seconds.

This completes the installation of vote radio. Metadata of new MP3 files will be synced every 10 minutes.

### Updating the software
To update your running containers you can run the following commands:

`docker-compose pull`  
`docker-compose up -d`

This will download the new version of the docker images and then stop, destroy and recreate the running containers.

### Advanced configuration
If you need advanced configuration options for your web radio and if you are not satisfied with the official docker-compose template, you are free to change the container configurations.

The following docker images are available:

#### Image "[michaelguenter/radio-frontend](https://hub.docker.com/r/michaelguenter/radio-frontend)"
This image contains an nginx webserver to serve the static files for the frontend. It exposes port 80.

Environment variables:

- `CLIENT_API_BASE_URL`: URL to the external backend, accessible from the browser
- `CLIENT_STREAM_BASE_URL`: URL to the streaming server, accessible from the browser

This image is suitable for horizontal scale-out.

#### Image "[michaelguenter/radio-backend-external](https://hub.docker.com/r/michaelguenter/radio-backend-external)"
This image handles requests from the frontend. It exposes port 80. The container is depending on a running instance of PostgreSQL and RabbitMQ.

Volumes:

- `/var/log/radio`: Log files

Environment variables

- `Environment:DbConnectionString`: Connection-String for the PostgreSQL instance. Format: User ID=<<User>>;Password=<<Password>>;Host=<<Host>>;Port=5432;Database=<<Database>>;Pooling=true;
- `Environment:MessagingHost`: Hostname of the RabbitMQ instance

This image is suitable for horizontal scale-out.

#### Image "[michaelguenter/radio-backend-internal](https://hub.docker.com/r/michaelguenter/radio-backend-internal)"
This image handles requests from LiquidSoap. It exposes port 80. The container is depending on a running instance of PostgreSQL and RabbitMQ.

Volumes:

- `/var/log/radio`: Log files

Environment variables:

- `Environment:DbConnectionString`: Connection-String for the PostgreSQL instance. Format: User ID=<<User>>;Password=<<Password>>;Host=<<Host>>;Port=5432;Database=<<Database>>;Pooling=true;
- `Environment:MessagingHost`: Hostname of the RabbitMQ instance

This image is NOT suitable for horizontal scale-out. There should only be one running instance of this image. If you use docker swarm it has to run on the swarm manager.

#### Image "[michaelguenter/radio-backend-console](https://hub.docker.com/r/michaelguenter/radio-backend-console)"
This image runs a sync job, that will import the metadata from the filesystem to the database every 10 minutes. It is also responsible for updating the database schema to a new version. The container is depending on a running instance of PostgreSQL.

Volumes:

- `/app/data`: Volume for the music data directory
- `/var/log/radio`: Log files

Environment variables:

- `Environment:DbConnectionString`: Connection-String for the PostgreSQL instance. Format: User ID=<<User>>;Password=<<Password>>;Host=<<Host>>;Port=5432;Database=<<Database>>;Pooling=true;
- `Environment:MigrationsDbConnectionString`: Connection-String used to update the database schema
- `Environment:DataDirectoryPath`: Path to the data directory in the container

This image is NOT suitable for horizontal scale-out. There should only be one running instance of this image. If you use docker swarm it has to run on the swarm manager.

#### Image "[michaelguenter/radio-streaming](https://hub.docker.com/r/michaelguenter/radio-streaming)"
This image contains an instance of icecast for streaming the audio to the clients. You can also use your own instance of icecast without using this image. This image exposes port 8000.

Volumes:

- `/var/log/icecast2`: Logs files

Environment variables:

- `ICECAST_LOCATION`: Icecast Location information
- `ICECAST_ADMIN_EMAIL`: E-Mail address of the administrator
- `ICECAST_MAX_CLIENTS`: Maximum number of listeners
- `ICECAST_SOURCE_PASSWORD`: Secure password for LiquidSoap
- `ICECAST_RELAY_PASSWORD`: Secure password for optional icecast relays
- `ICECAST_ADMIN_USERNAME`: Username for the web interface
- `ICECAST_ADMIN_PASSWORD`: Password for the web interface
- `ICECAST_HOSTNAME`: Public domain
- `ICECAST_PORT`: The port to expose
- `ICECAST_PUBLIC`: 0 or 1, depending on whether you want your radio listed in the icecast directory.
- `ICECAST_STREAM_NAME`: Name of the stream
- `ICECAST_STREAM_DESCRIPTION`: Description of the stream

This image is NOT suitable for horizontal scale-out. There should only be one running instance of this image. If you use docker swarm it has to run on the swarm manager.

To scale out you should use seperate server instances with relay instances of icecast. You can use the relay password to connect to this master instance.

#### Image "[michaelguenter/radio-playback](https://hub.docker.com/r/michaelguenter/radio-streaming)"
This image contains an instance of LiquidSoap for playing and encoding the audio. The container is depending on a running instance of icecast and a running instance of the internal backend.

Volumes:

- `/home/liquidsoap/music`: Volume for the music data directory
- `/var/log/liquidsoap`: Log files

Environment variables:

- `BACKEND_HOST`: Hostname of the internal backend
- `BACKEND_PORT`: Port of the internal backend
- `ICECAST_HOST`: Hostname of the icecast server
- `ICECAST_PORT`: Port of the icecast server
- `ICECAST_SOURCE_PASSWORD`: Configured source password of the icecast server

This image is NOT suitable for horizontal scale-out. There should only be one running instance of this image. If you use docker swarm it has to run on the swarm manager.

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

