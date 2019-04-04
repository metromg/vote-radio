- (Project Setup: Console Startup)
- (Debugging / Watching Files)

- Test Umgebung

Local-DB:
docker run --rm -d --name local_db -v local_db_volume:/var/lib/postgresql/data -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2

Local-Messaging:
docker run --rm -d --name local_messaging -p 5672:5672 rabbitmq:3.7

- Display global errors (signalr, api)
- Styling