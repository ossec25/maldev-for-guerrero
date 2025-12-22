# Notes personnelles – MalDev for Guerrero

Ce dépôt contient mes travaux réalisés dans le cadre du mini-labo de développement malware, inspiré de la formation **“maldev-for-dummies”**.
L’objectif n’est pas de produire un malware exploitable, mais de **démontrer la compréhension des mécanismes internes**, des techniques courantes et de leurs implications, dans un cadre strictement académique et contrôlé.

---

## Objectif général

Comprendre et mettre en pratique, sous Windows :

* l’exécution de shellcode en mémoire,
* les techniques d’injection de processus,
* les mécanismes d’évasion simples,
* les notions de persistance et de communication,

à travers une **progression d’exercices indépendants**, documentés et testés sur deux environnements distincts.

---

## Environnements de test

* **DEV-VM** : machine de développement (compilation, tests initiaux)
* **TARGET-VM** : machine cible dédiée aux tests d’exécution

Chaque binaire est testé sur DEV-VM puis transféré et exécuté sur TARGET-VM afin d’observer les différences de comportement, de dépendances et de détection.

---

## Structure du dépôt

* 'Exercises/'
  Contient les exercices numérotés (01 à 06), chacun avec :

  * `src/` : code source C#
  * `publish/` ou `bin/` : binaire compilé
  * `writeup.md` : description, fonctionnement et observations

* `lab/`
  Documentation liée aux machines virtuelles et à l’environnement de test.

* `README.md`
  README d’origine du dépôt forké.

* `README_student.md`
  Ce document, servant de notes personnelles et de synthèse.

---

## Exercices réalisés

### Exercice 01 – Shellcode Runner

* Exécution de shellcode via allocation mémoire et création de thread Windows.
* Payload **inoffensif** : ouverture de `calc.exe`.
* Code C# utilisant `VirtualAlloc`, `CreateThread`, etc.
* Testé sur DEV-VM et TARGET-VM.
* Observations sur Windows Defender et sur la nécessité d’utiliser `dotnet publish`.

---

### Exercice 02 – Shellcode Injector

* Injection de shellcode dans un processus existant (`notepad.exe`).
* Payload utilisé : fermeture du processus ciblé (`ExitProcess`).
* APIs utilisées : `OpenProcess`, `VirtualAllocEx`, `WriteProcessMemory`, `CreateRemoteThread`.
* Test effectué avec Notepad lancé au préalable.
* Observation : effet moins visible que l’exercice 01, mais comportement clairement observable.
* Defender actif lors des tests, sans blocage apparent.
* Travail préparatoire essentiel pour comprendre l’injection mémoire ciblée.

---

### Exercice 03 – Techniques d’évasion

* Introduction de mécanismes simples d’obfuscation (ex. XOR).
* Objectif : masquer le shellcode au repos et le reconstruire à l’exécution.
* Mise en évidence des limites des signatures statiques.

---

### Exercice 04 – Injection avancée (Process Hollowing)

* Remplacement du code d’un processus légitime par un code contrôlé.
* Étude des étapes internes du process hollowing.
* Implémentation volontairement limitée et documentée.

---

### Exercice 05 – Évasion d’Antivirus par obfuscation

* Obfuscation de données stockées dans le binaire.
* Reconstruction à l’exécution uniquement.
* Aucun comportement malveillant réel.
* Mise en évidence des dépendances logicielles (.NET runtime) lors du test sur TARGET-VM.

---

### Exercice 06 – Persistance et communication C2 basique

* Simulation de mécanismes de persistance et de communication.
* Implémentation **non autonome et non furtive**.
* Arrêt volontaire avant toute capacité exploitable.
* Objectif pédagogique : architecture et compréhension, pas diffusion.

---

## Positionnement pédagogique

Les binaires produits démontrent des **comportements potentiellement non désirés**, au sens académique (didactique) du terme, mais restent volontairement **inoffensifs** :

* pas de destruction,
* pas de propagation,
* pas de persistance cachée,
* pas de binaire autonome diffusé.

Chaque exercice s’arrête volontairement à un point défini et documenté dans les writeups.

---

## Suite et conclusion

Ce dépôt est mis à jour progressivement.
L’objectif final est de fournir :

* une **structure claire**,
* des **binaires compilés**,
* une **documentation explicative complète**,

montrant la compréhension des mécanismes internes du développement malware, sans produire un outil dangereux ou exploitable.

---
