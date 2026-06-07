# System Rezerwacji Wizyt - ManiSpa

Aplikacja internetowa stworzona do zarządzania rezerwacjami usług w małym salonie stylizacji paznokci ManiSpa. System umożliwia klientom przeglądanie aktualnej oferty oraz zapisywanie się na wizyty, natomiast administratorowi (stylistce) zapewnia pełen panel zarządzania usługami i wizytami.

## 1. Użyte technologie
* Język programowania: C#
* Framework: ASP.NET Core MVC
* Dostęp do bazy danych: Entity Framework Core
* Baza danych: Microsoft SQL Server / LocalDB
* Interfejs użytkownika: HTML, CSS, Bootstrap, JavaScript (Fetch API)
* System autoryzacji: ASP.NET Core Identity

## 2. Główne funkcjonalności
* Strona główna z podstawowymi informacjami kontaktowymi, lokalizacją salonu oraz godzinami otwarcia.
* Cennik ofert z wyświetlaniem zdjęć oraz responsywnym układem dla urządzeń mobilnych.
* Formularz rezerwacji wizyty dla klienta z automatycznym, dynamicznym podglądem szczegółów wybranej usługi bez przeładowywania strony.
* Panel administratora zabezpieczony hasłem, umożliwiający logowanie i wylogowanie.
* Zarządzanie bazą usług (dodawanie, edycja danych i zdjęć, usuwanie).
* Przeglądanie listy zaplanowanych wizyt posortowanych automatycznie chronologicznie według daty i godziny (również responsywny układ); możliwość etytwania wizyty (m.in. do zmiany statusu wizyty).

## 3. Wymagania systemowe
* .NET SDK (wersja zgodna z projektem)
* Visual Studio 2022 lub nowsze
* Serwer bazy danych SQL Server lub LocalDB zainstalowany lokalnie

## 4. Instrukcja uruchomienia lokalnego
1. Pobierz pliki projektu lub sklonuj repozytorium na swój komputer.
2. Otwórz plik rozwiązania (.sln) za pomocą programu Visual Studio.
3. Otwórz Konsolę Menedżera Pakietów (Package Manager Console) i wpisz polecenie tworzące bazę danych:
   Update-Database
4. Po pomyślnym zakończeniu migracji, uruchom aplikację klikając przycisk uruchamiania lub wciskając klawisz F5.
5. Przeglądarka automatycznie otworzy stronę główną aplikacji pod adresem lokalnym.
