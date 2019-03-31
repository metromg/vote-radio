- (Project Setup: Console Startup)
- (Debugging / Watching Files)
- (DB: Migration vs App User)

- Test Umgebung

Temp-DB:
docker run --rm -d --name local_db -v local_db_volume:/var/lib/postgresql/data -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2