SocialNet

1.	Krótki opis
SocialNet to innowacyjna platforma społecznościowa, która umożliwia użytkownikom łatwe tworzenie wpisów tekstowych z pełną kontrolą nad opublikowanymi materiałami. Niezalogowani użytkownicy mogą przeglądać treści, natomiast reszta funkcji jest dostępna po zalogowaniu. System wykorzystuje bazę danych PostgreSQL do przechowywania danych użytkowników i treści, a także zapewnia bezpieczeństwo danych dzięki odpowiednim mechanizmom uwierzytelniania i autoryzacji.
2.	Specyfikacja wykorzystanych technologii
Backend: C# .NET 8
Baza danych: PostgreSQL
Framework: ASP.NET Core MVC
Autoryzacja: ASP.NET Identity
UI: HTML, CSS, JavaScript 
3.	Instrukcja pierwszego uruchomienia projektu

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

4.	Opis struktury projektu
Projekt ma strukturę klasycznego MVC (Model-View-Controller) w ASP.NET Core:
•	Modele: 
o	User
	Id: Klucz główny użytkownika.
	Email: Adres e-mail.
	Login: Unikalny login użytkownika.
	Password: Hasło użytkownika (haszowane).
	Role: Rola użytkownika (user, admin).
o	Post
	Id: Klucz główny wpisu.
	Content: Treść wpisu (z obsługą nowych linii).
	Author: Autor wpisu (nazwa użytkownika).
	CreatedAt: Data utworzenia wpisu (UTC).
•	Controllers:
o	AccountController 
Kontroler odpowiedzialny za logowanie, rejestrację oraz wylogowywanie użytkowników.
	Metody:
	Register (GET):
	Metoda HTTP: GET
	Opis: Wyświetla formularz rejestracji użytkownika.
	Przekazywane dane: Brak.
	Zwracane dane: Widok Register.cshtml.
	Register (POST):
	Metoda HTTP: POST
	Opis: Przetwarza dane przesłane z formularza rejestracji. Waliduje dane i tworzy nowego użytkownika w bazie danych.
	Przekazywane dane: email, login, password, confirmPassword.
	Działanie:
	Sprawdza poprawność hasła (czy oba pola są zgodne).
	Waliduje unikalność login i email.
	Zapisuje użytkownika z domyślną rolą user.
	Zwracane dane: Po udanej rejestracji przekierowuje do strony logowania.
	Login (GET):
	Metoda HTTP: GET
	Opis: Wyświetla formularz logowania.
	Przekazywane dane: Brak.
	Zwracane dane: Widok Login.cshtml.
	Login (POST):
	Metoda HTTP: POST
	Opis: Przetwarza dane logowania. Jeśli dane są poprawne, tworzy sesję użytkownika i przekierowuje na stronę główną odpowiednią do jego roli.
	Przekazywane dane: login, password.
	Działanie:
	Sprawdza poprawność danych logowania.
	Jeśli dane są poprawne, zapisuje informacje o użytkowniku w sesji przy użyciu Claims.
	Zwracane dane: Przekierowanie na stronę główną (AdminIndex lub UserIndex).
	Logout (POST):
	Metoda HTTP: POST
	Opis: Wylogowuje użytkownika i czyści sesję.
	Przekazywane dane: Brak.
	Zwracane dane: Przekierowanie na stronę logowania.
o	2. PostController
Kontroler obsługujący operacje CRUD na wpisach. Dostępne dla zalogowanych użytkowników.
	Metody:
	Create (GET):
	Metoda HTTP: GET
	Opis: Wyświetla formularz tworzenia nowego wpisu.
	Przekazywane dane: Brak.
	Zwracane dane: Widok Create.cshtml.
	Create (POST):
	Metoda HTTP: POST
	Opis: Przetwarza dane przesłane z formularza i zapisuje nowy wpis w bazie danych.
	Przekazywane dane: content.
	Działanie:
	Waliduje, czy treść wpisu nie jest pusta.
	Zapisuje wpis z informacjami o autorze i dacie utworzenia.
	Zwracane dane: Przekierowanie do widoku MyPosts.
	MyPosts (GET):
	Metoda HTTP: GET
	Opis: Wyświetla listę wpisów zalogowanego użytkownika.
	Przekazywane dane: Brak.
	Zwracane dane: Widok MyPosts.cshtml z listą wpisów użytkownika.
	DeletePost (POST):
	Metoda HTTP: POST
	Opis: Usuwa wpis z bazy danych. Tylko dla administratora.
	Przekazywane dane: id (ID wpisu).
	Działanie:
	Sprawdza, czy wpis istnieje w bazie danych.
	Usuwa wpis, jeśli użytkownik ma uprawnienia.
	Zwracane dane: Przekierowanie do widoku AdminIndex.
o	3. AdminController
Kontroler dostępny wyłącznie dla administratorów. Obsługuje zarządzanie użytkownikami i rolami.
	Metody:
	UserManagement (GET):
	Metoda HTTP: GET
	Opis: Wyświetla listę użytkowników z możliwością zmiany ról i usuwania kont.
	Przekazywane dane: Brak.
	Zwracane dane: Widok UserManagement.cshtml z listą użytkowników.
	ChangeRole (POST):
	Metoda HTTP: POST
	Opis: Zmienia rolę wybranego użytkownika.
	Przekazywane dane: id (ID użytkownika), role (nowa rola).
	Działanie:
	Sprawdza, czy użytkownik istnieje.
	Aktualizuje rolę użytkownika w bazie danych.
	Zwracane dane: Przekierowanie do widoku UserManagement.
	DeleteUser (POST):
	Metoda HTTP: POST
	Opis: Usuwa konto użytkownika z bazy danych.
	Przekazywane dane: id (ID użytkownika).
	Działanie:
	Sprawdza, czy użytkownik istnieje.
	Zapobiega usunięciu samego siebie.
	Usuwa użytkownika z bazy danych.
	Zwracane dane: Przekierowanie do widoku UserManagement.
5.	System użytkowników

Role:
•	Admin: Zarządza użytkownikami i wpisami, może zmieniać role.
•	User: Tworzy i zarządza swoimi wpisami.
•	Guest: Przegląda wpisy.
Powiązane dane:
•	Wpisy są powiązane z użytkownikami.
•	Admini mają dostęp do wszystkich wpisów i użytkowników.

Role:
Administrator: Zarządza wszystkimi treściami, użytkownikami i ustawieniami systemu.
Zalogowany użytkownik: Może tworzyć treści i edytować swój profil.
Gość: Może wyłącznie przeglądać publiczne treści.
Przypisywanie ról:
Role przypisywane są automatycznie na podstawie rejestracji. Administrator może zmieniać rolę użytkownika w panelu administracyjnym.


6.	Najciekawsze funkcjonalności
•	Role i autoryzacja:
System z jasno zdefiniowanymi rolami (admin, user, guest), które wpływają na dostępne funkcje.
•	Panel administracyjny:
Admin może zarządzać użytkownikami (zmieniać role, usuwać konta) i wpisami (usuwać treści).
•	Dynamiczne wyświetlanie wpisów:
Wpisy obsługują nowe linie (\n → <br>), co poprawia czytelność treści.
•	Responsywny interfejs:
Formularze logowania i rejestracji, a także widoki dashboardów zostały zaprojektowane z wykorzystaniem Bootstrap, co zapewnia ich estetyczny wygląd.

7.	Autorzy projektu

Patryk Marczyk
Magdalena Jasińska



