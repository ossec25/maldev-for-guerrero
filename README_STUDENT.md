# Notes personnelles - MalDev for Guerrero

Ce dépôt contient mes travaux sur le mini-labo de développement malware, inspiré de la formation "maldev-for-dummies".

## Objectif général

Comprendre et mettre en pratique l’exécution de shellcode en mémoire sous Windows, à travers plusieurs exercices progressifs.

## Structure

- `Exercises/` : Exercices progressifs de shellcode, injection, évasion, etc.
- `lab/` : Documentation sur les environnements DEV-VM et TARGET-VM
- `README.md` : Ce fichier

## Usage

Chaque exercice a son propre dossier avec le code source (`src/`), le binaire compilé (`bin/` si besoin), et un `writeup.md` expliquant le fonctionnement et les observations.

## À venir

- Exercice 01 : Shellcode Runner
- Exercice 02 : Shellcode Injector
- Exercice 03 : Techniques d’évasion
- Exercice 04 : Injection avancée avec Process Hollowing
- Exercice 05 : Evasion d’Antivirus via Obfuscation
- Exercice 06 : Persistence et Communication C2 basique

---

Pour chaque exercice, consulter le dossier correspondant dans `Exercises/`.

## 

- Shellcode exécuté via allocation mémoire et thread Windows.
- Payload inoffensif : ouverture de calc.exe (calculatrice).
- Code en C# utilisant `VirtualAlloc`, `CreateThread`, etc.
- Exécution testée sur DEV-VM et TARGET-VM.
- Observations sur détection Defender et nécessité de publier avec `dotnet publish`.


## Exercice 02 - Shellcode Injector

- Injection de shellcode dans un processus cible déjà existant (ici notepad.exe).
- Payload utilisé : fermeture du processus Notepad (commande ExitProcess dans le shellcode).
- Code C# utilisant les API Windows OpenProcess, VirtualAllocEx, WriteProcessMemory, CreateRemoteThread, etc.
- Testé avec le processus Notepad ouvert au préalable sur la VM.
- Observations : pas d’effet visuel immédiat (contrairement à l’ouverture de calc.exe dans l’exercice 01), mais le processus ciblé se ferme après injection.
- Defender était actif lors des tests, sans blocage visible.
- Remarque : possibilité de varier le payload pour tester d’autres effets, notamment l’exécution de commandes ou affichage (MessageBox, Notepad, etc.).
- Travail préparatoire pour comprendre les techniques d’injection mémoire ciblée.

## Suite

- Préparation pour exercice 03 : Technique d'évasion
- Documentation progressive dans ce repo.

---

Ce fichier est mis à jour à chaque étape.