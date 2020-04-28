# Dokumentation

## Nutzen

Dieses Projekt ist eine Klassenbibliothek, die in möglichst jedem anderen C# Projekt von Nutzen sein soll. Ziel ist es daher,
dass die Funktionen einfach gehalten sein sollen.

Es gibt folgende Module in dem Projekt:

- Aufgabenplanung
- Backup-System
- Kodierung
- Dateisystem
- Mathe
- Einstellungen
- Datenstrukturen
- Algorithmen

## Aufgabenplanung

In der Klassenbibliothek soll es eine Klasse geben, die dabei hilft, Aufgaben zu planen. Aufgaben könnne Aufgaben aus dem realen Leben, als auch
Aufgaben sein die in C# als Action oder Func bekannt sind. Dem Algorithmen wird eine Liste von Aufgaben gegeben und er gibt eine Liste von
Aufgaben zurück, nur in sortierter Reihenfolge.

Eine Aufgabe soll einen Namen, eine Beschreibung und eine Dauer haben. Weiterhin hat sie Abhängigkeiten. Diese Abhängigkeiten können andere Aufgaben
sein, die davor erledigt werden müssen. Dann hat eine Aufgabe eine Deadline und eine Priorität, welche eine Zahl von 1 bis 10 geht. 1 steht für niedrige
Priorität, 10 steht für hohe.

Wenn eine Aufgabe erledigt wird, kann man sich aktiv oder passiv erledigen. Eine passive Aufgabe muss meist jedoch erst aktiv gestartet und aktiv
beendet werden. Dies ist möglich, indem man einzelne Aufgabe als Abhängigkeiten erstellt.

Es soll möglich sein, die Anzahl der Arbeiter anzugeben, welche die Aufgaben erledigen. Ihre Aufteilung soll optimal sein. Wenn man an weitere
Informationen wie die Dauer der Erledigung aller Aufgaben rankommen möchte, muss man die entsprechende Methode der Aufgabenplanerklasse aufrufen.

... csharp

Console.WriteLine("Hello World!");

...