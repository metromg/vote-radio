- (Project Setup: Console Startup)
- (Debugging / Watching Files)

- Test Umgebung

Local-DB:
docker run --rm -d --name local_db -v local_db_volume:/var/lib/postgresql/data -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2

Local-Messaging:
docker run --rm -d --name local_messaging -p 5672:5672 rabbitmq:3.7

Integration-Test-DB:
docker run --rm -d --name local_db -p 5432:5432 -e POSTGRES_USER=radio -e POSTGRES_PASSWORD=Abc1234 postgres:11.2

Anforderungen: Mindestens 3 Songs, Mindestlänge: 30 Sekunden

- Ist ein Job wirklich das Richtige oder brauchen wir einen Filemonitor? -> Wie schlimm ist es, wenigstens Ordering der Kandidaten fixen... Welche Fälle sind betroffen: Löschen, Renamen, Moven
- Default cover fixen
- Voting stylen, Loading indicator for casting votes, Initiales setzen der remaining time
- Test music loading indicator
- Frontend Tests
- Retest UnitOfWork Initialization