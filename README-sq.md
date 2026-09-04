### Teknologjite e Perdorura

##### Backend

> C#
> ASP.NET Core Web API
> Entity Framework Core
> PostgreSQL
> xUnit
> EF Core InMemory provider per `unit tests`

##### Frontend

> Angular
> TypeScript
> Bootstrap
> Reactive Forms

### Perdorimi i aAplikacionit Lokalisht

##### Parakushtet

Ky `README-sq.md` supozon se:

- .NET SDK
- Node.js
- npm
- Angular CLI
- PostgreSQL

Jane te instaluara.

##### DB( Baza e Te Dhenave)

Aplikacioni perdor `PostgreSQL`.
Krijo nje databaze ne `PostgreSQL` te quajtur: `support_desk`.
Pastaj konfiguro `connection string` ne file-in: `backend/appsettings.json`.

```
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=support_desk;Username=postgres;Password=PASSWORD_I_YT"
    }
}
```

The pastaj hap terminalin ne folder-in `backend`:

> dotnet ef database update

##### Perdorimi Backend-it

Ne terminal ne folder-in `backend`:

> dotnet restore

> dotnet ef database update

> dotnet run

##### Perdorimi Frontend-it

Ne terminal ne folder-in `frontend`:

> npm install

> ng serve

Pastaj hap `URL`-in qe shfaqet ne terminal. Zakonisht:

> http://localhost:4200

### Rregullat e Biznesit

Rregullat kryesore te biznesit jane implementuar ne: `backend/Controllers/TicketsController.cs`.
Backend-i trajtohet si shtresa autoritative per nenshtrimin e rregullave te rrjedhes.

##### Pse Jane Rregullat e Biznesit ne Kontroller

Per kete detyre, rregullat e rrjedhes se punes u mbajten brenda `TicketsController.cs` sepse aplikacioni eshte relativisht i vogel (dhe per te shpejtuar procesin e kodimit, meqenese kisha vetem 48 ore) dhe kontrolluesi ofron nje pike te vetme hyrjeje autoritare per mutacionet e biletave.

Kjo gjithashtu e mban zbatimin te thjeshte dhe i ben rregullat e rrjedhes se punes te lehta per t'u gjetur dhe shqyrtuar.

Logjika e tranzicionit te statusit dhe llogaritja e dates se caktuar jane te ndara ne metoda private te dedikuara brenda kontrolluesit.

### Supozimet

Supozimet e mia kur fillova projektin kane qene:

1. Autentikimi dhe autorizimi jane jashte fusheveprimit te caktimit.
2. Autori i komentit eshte dhene nga klienti sepse nuk ka sistem Autentikimi/perdoruesi.
3. Vetem agjente aktive mund t'u caktohen biletave.
4. Frontend-i komunikon drejtperdrejt me API-ne REST permes sherbimeve Angular.

### Cfare Do Te Kisha Permirsuar

1. UI&UX. ​​Deri tani, per te kompensuar mungesen e kohes kam perdorur vetem bootstrap baze.
2. Autentifikimi dhe autorizimi.
3. Shtimi i validimit dhe trajtimit te gabimeve me te detajuar ne frontend.
4. Permiresimi i menaxhimit te gjendjes se frontend.
5. Shtimi i AuditLogs (historia e auditimit)

### Perafersisht Sa Me Ka Zgjate Projekti

Diku 12-14 ore.
