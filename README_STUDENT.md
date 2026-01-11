# Notes personnelles – MalDev for Guerrero

Ce dépôt contient mes travaux réalisés dans le cadre d’un mini-laboratoire de développement malware, inspiré de la formation **“maldev-for-dummies”**.

L’objectif n’est **en aucun cas** de produire un malware exploitable, mais de **démontrer la compréhension des mécanismes internes**, des techniques courantes et de leurs implications, dans un cadre strictement académique et contrôlé.

Ces travaux s’inscrivent dans une démarche de compréhension offensive à des fins défensives (blue team, détection, durcissement).

---

## Objectif général

Comprendre et mettre en pratique, sous Windows :

* l’exécution de shellcode en mémoire,
* les techniques d’injection de processus,
* des mécanismes d’évasion simples,
* des notions de persistance et de communication,

au travers d’une **progression d’exercices indépendants**, documentés et testés sur deux environnements distincts.

---

## Environnements de test

* **DEV-VM** : machine de développement (compilation, tests initiaux)
* **TARGET-VM** : machine cible dédiée aux tests d’exécution

Chaque binaire est d’abord testé sur DEV-VM, puis transféré et exécuté sur TARGET-VM afin d’observer les différences de comportement, de dépendances et de détection.

---

## Structure du dépôt

* `Exercises/`  
  Contient les exercices numérotés (01 à 06), chacun comprenant :
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

* Shellcode généré via **msfvenom**, avec des paramètres personnalisés (payload, format, encodage), distincts des exemples fournis dans le dépôt original.
* Exécution de shellcode via allocation mémoire et création d’un thread Windows.
* Payload **inoffensif** : ouverture de `calc.exe`.
* Code C# utilisant notamment `VirtualAlloc`, `CreateThread`, etc.
* Testé sur DEV-VM et TARGET-VM.
* Observations concernant Windows Defender et la nécessité d’utiliser `dotnet publish`.

---

### Exercice 02 – Shellcode Injector

* Injection de shellcode dans un processus existant (`notepad.exe`).
* Payload utilisé : fermeture contrôlée du processus ciblé (`ExitProcess`).
* APIs utilisées : `OpenProcess`, `VirtualAllocEx`, `WriteProcessMemory`, `CreateRemoteThread`.
* Test effectué avec Notepad lancé au préalable.
* Observation : effet moins visible que l’exercice 01, mais comportement clairement observable.
* Windows Defender actif lors des tests, sans blocage apparent.
* Travail préparatoire essentiel pour comprendre l’injection mémoire ciblée.
* Le bonus consistant à éviter l’appel à `CreateRemoteThread` a été étudié conceptuellement ; une implémentation complète n’a pas été intégrée afin de conserver un code lisible et pédagogique.

---

### Exercice 03 – Techniques d’évasion

* Introduction de mécanismes simples d’obfuscation (ex. XOR).
* Objectif : masquer le shellcode au repos et le reconstruire uniquement à l’exécution.
* Mise en évidence des limites des signatures statiques.

---

### Exercice 04 – Injection avancée (Process Hollowing)

* Remplacement du code d’un processus légitime par un code contrôlé.
* Étude des différentes étapes internes du process hollowing.
* Implémentation volontairement limitée et documentée à des fins pédagogiques.

---

### Exercice 05 – Évasion d’Antivirus par obfuscation

* Obfuscation de données stockées statiquement dans le binaire.
* Reconstruction dynamique uniquement à l’exécution.
* Aucun comportement malveillant réel.
* Mise en évidence des dépendances logicielles (.NET runtime) lors des tests sur TARGET-VM.

---

### Exercice 06 – Persistance et communication C2 basique

* Simulation de mécanismes de persistance et de communication.
* Implémentation **non autonome, non furtive et volontairement incomplète**.
* Arrêt intentionnel avant toute capacité exploitable.
* Objectif pédagogique : compréhension de l’architecture, pas la diffusion.
* Cet exercice ne fait pas partie du workshop original ; il a été ajouté comme prolongement conceptuel afin de comprendre les briques logiques d’une architecture malware.

---

## Positionnement pédagogique

Les binaires produits démontrent des **comportements potentiellement non désirés** au sens académique du terme, mais restent volontairement **inoffensifs** :

* pas de destruction,
* pas de propagation,
* pas de persistance cachée,
* pas de binaire autonome diffusé.

Chaque exercice s’arrête volontairement à un point défini et documenté dans les writeups associés.

---

## Suite et conclusion

Ce dépôt est mis à jour progressivement.

L’objectif final est de fournir :

* une **structure claire**,
* des **binaires compilés**,
* une **documentation explicative complète**,

afin de démontrer la compréhension des mécanismes internes du développement malware, sans produire d’outil dangereux ou exploitable.

---

### Note sur Windows Defender

Lors des tests, Windows Defender activé a automatiquement détecté et supprimé le binaire de l’Exercice 01 lors de son extraction, en raison de la présence de shellcode exécuté en mémoire.

Les autres binaires (02 à 06) ont pu être testés avec Defender actif sans blocage.

Les tests complets ont donc été réalisés avec Defender désactivé, dans un environnement de laboratoire contrôlé.

Cette différence de détection illustre la variabilité des mécanismes heuristiques et des signatures selon la technique utilisée.

