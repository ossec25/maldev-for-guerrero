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

- Exercice 02 : Shellcode Injector
- Exercice 03 : Techniques d’évasion

---

Pour chaque exercice, consulter le dossier correspondant dans `Exercises/`.

## Exercice 01 - Shellcode Runner

- Shellcode exécuté via allocation mémoire et thread Windows.
- Payload inoffensif : ouverture de calc.exe (calculatrice).
- Code en C# utilisant `VirtualAlloc`, `CreateThread`, etc.
- Exécution testée sur DEV-VM et TARGET-VM.
- Observations sur détection Defender et nécessité de publier avec `dotnet publish`.

## Suite

- Préparation pour exercice 02 : Shellcode Injector.
- Documentation progressive dans ce repo.

---

Ce fichier est mis à jour à chaque étape.