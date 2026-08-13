# TodoMongoMvc

MVP de estudo: ASP.NET Core MVC + MongoDB. CRUD simples de lista de tarefas, sem autenticação, sem camadas extras — foco em ver o driver oficial do MongoDB funcionando com um app MVC.

## Stack

- ASP.NET Core MVC (.NET 10)
- [MongoDB.Driver](https://www.nuget.org/packages/MongoDB.Driver) (driver oficial, sem ORM/EF)
- MongoDB rodando em container Docker
- [mongo-express](https://github.com/mongo-express/mongo-express) — GUI web pra inspecionar os documentos
- [Pico.css](https://picocss.com) (vendorizado localmente) + tema claro próprio, acento num verde suave (inspirado na marca MongoDB) — badge de status pra cada tarefa, sem Bootstrap/jQuery

## Estrutura

```
Models/TodoItem.cs        # documento (Id, Title, Description, IsDone, CreatedAt)
Settings/MongoDbSettings.cs
Services/TodoService.cs   # CRUD direto via IMongoCollection<TodoItem>
Controllers/TodoController.cs
Views/Todo/               # Index, Create, Edit
docker-compose.yml        # mongo + mongo-express
```

Sem Repository/interface extra de propósito — a ideia é ver a API do MongoDB.Driver direto no `TodoService`, não abstraí-la.

## Rodando

Pré-requisitos: .NET 10 SDK, Docker Desktop.

```bash
# sobe MongoDB (porta 27017) + mongo-express (porta 8081)
docker compose up -d

# roda a app
dotnet run
```

Acesse `http://localhost:5050/Todo` (rota default) para o CRUD. A página "Sobre" (`/Home`) explica o projeto pra quem chegar sem contexto. `http://localhost:8081` mostra os documentos crus no Mongo.

Connection string e nomes de database/collection ficam em `appsettings.json`, seção `MongoDb`.

## O que MongoDB faz aqui

- `MongoClient` conecta no servidor; database e collection são criados automaticamente na primeira escrita — sem schema, sem migration.
- Cada `TodoItem` é serializado como documento BSON. `[BsonId]` + `[BsonRepresentation(BsonType.ObjectId)]` mapeiam o `_id` (ObjectId nativo do Mongo) pra `string` no C#.
- CRUD via `IMongoCollection<T>`: `Find`, `InsertOneAsync`, `ReplaceOneAsync`, `DeleteOneAsync`, com filtros em LINQ (`t => t.Id == id`) traduzidos pelo driver.
- `TodoService` é singleton no DI — `MongoClient` é thread-safe e caro de criar, então uma instância serve toda a app.

## Projetos irmãos

Existem também o [LeaderboardRedisMvc](https://github.com/YuriLucka/LeaderboardRedisMvc) (Redis) e o [SocialGraphMvc](https://github.com/YuriLucka/SocialGraphMvc) (Neo4j) — mesma ideia, cada um mostrando o ponto forte de uma categoria de banco NoSQL diferente: aqui o Mongo guarda documento estruturado (fonte de verdade); o Redis vira estrutura de ranking e cache (acelerar leitura); o Neo4j resolve travessia de relacionamento (quem conecta com quem).
