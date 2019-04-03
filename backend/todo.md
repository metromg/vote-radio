- (Project Setup: Console Startup)
- (Debugging / Watching Files)
- (DB: Migration vs App User)

- Test Umgebung

Local-DB:
docker run --rm -d --name local_db -v local_db_volume:/var/lib/postgresql/data -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2

Local-Messaging:
docker run --rm -d --name local_messaging -p 5672:5672 rabbitmq:3.7

In Kommentar ergänzen:
https://www.rabbitmq.com/releases/rabbitmq-dotnet-client/v3.3.0/rabbitmq-dotnet-client-3.3.0-user-guide.pdf 
IModel should not be shared between threads
- SignalR Connection Error Handling (Mobile etc.), API Error handling
- Fetch Polyfill
- Styling