# Dokumentation

## Nutzen

Dieses Projekt ist eine Klassenbibliothek, die in möglichst jedem anderen C# Projekt von Nutzen sein soll. Ziel ist es daher,
dass die Funktionen einfach gehalten sein sollen. Sonst enthält sie noch einige Programme und Algorithmen, die hier so lange untergebracht werden,
bis es nötig ist diese in ein weiteres anderes Projekt auszulagern.

Es gibt folgende Module in dem Projekt:

- Aufgabenplanung
- Backup-System
- Kodierung
- Dateisystem
- Mathe
- Einstellungen
- Datenstrukturen
- Algorithmen

## Aufgabenplanung (noch in Arbeit)

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

Das ganze wird mit der Hilfe von Graphen Algorithmen umgesetzt. Ein kleines Beispiel: Wenn man 2 Aufgaben hat, Aufgabe A und B und man Aufgabe A erledigen
muss bevor man Aufgabe B erledigt und Aufgabe B erledigen muss bevor man Aufgabe A erledigt, dann sind die Aufgaben jeweils gegenseitig abhängig.
In Graphen wird das ein Zyklus oder Kreis genannt. Der Algorithmus erkennt solche Zyklen und wird bei deren Auftreten einen Fehler.

## Backup-System

Das Backup-System funktioniert so: Man gibt die Ordner an, von denen man ein Backup machen möchte und in welchem Ordner das Backup
erstellt werden soll. Weiterhin wird das Programm **aescrypt** benötigt, zu dem man den Pfad angibt. Der Link zu dem Programm ist https://github.com/paulej/AESCrypt und es kann sicher
eine Datei mit der Angabe eines Passworts verschlüsseln.

Beim Backup geschieht folgendes:
- temp oder wird erstellt
- Backup Ordner werden in den temp Ordner kopiert
- temp Ordner wird gezippt
- die Zip Datei wird verschlüsselt
- alles außer die verschlüsselte Datei werden wieder gelöscht

Die Dateiendung der erstellten verschlüsselten Datei ist "zip.aes". Diese Datei kann auch wieder entschlüsselt und entpackt werden wie folgt:
- Datei wird entschlüsselt und Zip Datei wird erstellt
- Zip Datei wird entpackt
- alles außer der entpackte Ordner werden gelöscht

**Noch in Planung**

In der GUI die noch entwickelt wird kann ein Backup Interval eingestellt werden, sodass das Backup automatisch erstellt wird und der Benutzer benachrichtigt wird.
Auf diese Art und Weise werden die Daten gesichert und der Benutzer muss sich keine Gedanken darum machen, er muss nur die Backup Dateien an einem Speicherort seiner
Wahl ablegen.

## Kodierung

Man kann String komprimieren und dekomprimieren:

```c#
var compressed = StringCompression.Compress("mein string");

var decompressed = StringCompression.Decompress(compressed);
```

Es die Idee eines Kodierungsalgorithmus, der einen String kodieren und dekodieren kann. Dies wird durch das Interface IEncodingAlgorithm abstrahiert.
Die Klasse **ShannonAlgorithm** implementiert das Interface und ist vielleicht nicht von bedeutungsvoller Anwendung oder Relevanz, sonder mehr eine
interessante Spielerei. Hier helfen auch die Unit Tests zu verstehen, wie der Code benutzt werden soll.

```c#
var s = "JavaScript syntax highlighting";
Console.WriteLine("Hello World!");
```