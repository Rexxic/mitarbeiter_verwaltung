# Mitarbeiter Verwaltungs Programm
#abgabe  Dokumentation und Projekt bis Do 21.03. 18:00 Uhr per E-Mail.
Präsentation bis 12.04 4-5 Folien Abage in der Unterrichtsstunde 
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

![image](https://github.com/Rexxic/mitarbeiter_verwaltung/assets/156774741/5deb52ab-a8ad-471d-9044-1616036271b2)




## CSV-Datei
ID,Name,Vorname,Adresse,Telefon,Email,Position,Firmeneintritt,Renteneintritt,Gehalt,Geburtsdatum
1,Schmidt,Hannah,Sonnenstraße 23,123456789,hannah.schmidt@example.com,Manager,2018-06-12,2048-08-15,60000,1978-03-20
2,Müller,Julia,Musterweg 7,987654321,julia.müller@example.com,Assistent,2019-03-25,2043-12-01,42000,1982-10-12
3,Schneider,Laura,Bahnhofstraße 15,456123789,laura.schneider@example.com,Entwickler,2020-11-18,2052-05-30,55000,1989-07-05
4,Fischer,Simon,Feldweg 8,789456123,simon.fischer@example.com,Designer,2017-09-03,2049-09-20,48000,1975-09-28
5,Weber,Max,Hauptplatz 12,234567890,max.weber@example.com,Manager,2022-01-10,2047-10-10,65000,1983-12-03
6,Wagner,Anna,Lindenallee 9,543210987,anna.wagner@example.com,Assistent,2016-08-22,2055-04-25,40000,1991-06-15
7,Becker,Kevin,Seestraße 33,321654987,kevin.becker@example.com,Entwickler,2015-05-30,2053-08-18,58000,1980-11-08
8,Hoffmann,Michael,Marktplatz 5,876543210,michael.hoffmann@example.com,Designer,2023-04-05,2046-07-12,50000,1973-07-30
9,Schäfer,Sarah,Birkenweg 4,135792468,sarah.schäfer@example.com,Manager,2014-02-14,2050-03-05,70000,1986-05-25
10,Koch,David,Tannenweg 17,678901234,david.koch@example.com,Assistent,2019-10-08,2056-01-15,45000,1977-09-18
11,Bauer,Lisa,Erlenstraße 29,987654321,lisa.bauer@example.com,Entwickler,2021-07-19,2048-11-22,62000,1984-08-09
12,Richter,Felix,Kiefernweg 3,123456789,felix.richter@example.com,Designer,2018-03-07,2054-09-30,53000,1979-04-14
13,Lehmann,Lena,Buchenweg 20,654321098,lena.lehmann@example.com,Manager,2016-11-25,2051-12-18,68000,1987-02-28
14,Krüger,Jan,Schulstraße 6,456789012,jan.krüger@example.com,Assistent,2017-06-14,2050-05-20,47000,1980-10-03
15,Fuchs,Emma,Kastanienweg 11,789012345,emma.fuchs@example.com,Entwickler,2020-09-30,2045-03-08,60000,1982-11-16

## Präsenatation angefangen 


