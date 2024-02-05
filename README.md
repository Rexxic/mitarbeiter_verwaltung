# Mitarbeiter Verwaltungs Programm

## Methoden
### Generell
- saveEmployee(Employee employee) -> bool -> Mitarbeiter speichern/neu anlegen
- loadEmployees() -> Employee\[] -> Mitarbeiterliste laden
- deleteEmployee(Employee employee) -> void -> Mitarbeiter löschen
- searchEmployee(String name) -> Employee\[] maybe as array? -> Mitarbeiter nach Namen suchen
### Employee Objekt
- Datenfelder für
	- ID - int
	- Name - String
	- Vorname - String
	- Adresse - String
	- Telefon - String
	- Email - String
	- Position - Enum
	- Firmeneintritt - Date
	- Renteneintritt - Date
	- Gehalt - float
	- Geburtsdatum - Date
- Getter / Setter Methoden
- toString() -> String -> Ruft die Datenfelder des objekt ab und gibt diese durch Kommas getrennt in einem String zurück
- static fromString(String s) -> Employee -> Nimmt einen String mit durch Kommas separierte Daten und erstellt ein neues Employee Objekt

## Aufgaben
- [ ] Struktogramm saveEmployee()
- [ ] Struktogramm toString()
- [ ] Use-Case-Diagramm User

## Use Case Diagram
![Use Case Diagram](docfiles/Use%20Case.svg)
![image](https://github.com/Rexxic/mitarbeiter_verwaltung/assets/156774741/120173a8-7321-48a0-a86c-b2355b72eb8d)
