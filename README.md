SocialNet

1.	Krótki opis
SocialNet to innowacyjna platforma społecznościowa, która umożliwia użytkownikom łatwe tworzenie wpisów tekstowych z pełną kontrolą nad opublikowanymi materiałami. Niezalogowani użytkownicy mogą przeglądać treści, natomiast reszta funkcji jest dostępna po zalogowaniu. System wykorzystuje bazę danych PostgreSQL do przechowywania danych użytkowników i treści, a także zapewnia bezpieczeństwo danych dzięki odpowiednim mechanizmom uwierzytelniania i autoryzacji.
2.	Specyfikacja wykorzystanych technologii
   
Backend: C# .NET 8

Baza danych: PostgreSQL

Framework: ASP.NET Core MVC

Autoryzacja: ASP.NET Identity

UI: HTML, CSS, JavaScript 

5.	Instrukcja pierwszego uruchomienia projektu

•	Upewnij się, że masz zainstalowanego dockera
•	W katalogu głównym uruchom bazę danych PostgreSQL:
 
docker-compose up -d

 
•	Po kilku sekundach baza danych będzie dostępna pod adresem:

Host: localhost

Port: 5432

Username: postgres

Password: admin

Database: socialnet

•	Przywróć zależności projektu:

dotnet restore

•	Jeśli to konieczne, zaktualizuj bazę danych:

dotnet ef database update

Należy zainstalować pakiety NuGet

•	Microsoft.EntityFrameworkCore.Design

•	Microsoft.EntityFrameworkCore.Tools

•	Npgsql

•	Npgsql.EntityFrameworkCore.PostgreSQL

Baza danych zawiera dwóch zarejestrowanych użytkowników:

Użytkownik 1:

Login: admin

Haslo: admin

Rola: Admin

Użytkownik 2:

Login: user

Haslo: user

Rola: User

W razie problemów należy pobrać PostgreSQL i zaimportować backup bazy Database.sql który znajduje się w głównym katalogu. Bazę tam należy ustawić tak jak adres wyżej

Dokument techniczny zanjduje się w katalogu

Autorzy projektu

Patryk Marczyk
Magdalena Jasińska



