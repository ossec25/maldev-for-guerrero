# Exercise 04 – Process Hollowing (analyse contrôlée)

## Objectif de l’exercice

Le but de cet exercice est de comprendre le fonctionnement interne d’une technique appelée **process hollowing**, souvent associée aux malwares, sans aller jusqu’à une implémentation malveillante réelle.

L’objectif est volontairement pédagogique :
comprendre comment Windows gère un processus, son thread principal et son contexte d’exécution, afin de mieux analyser et détecter ce type de comportement d’un point de vue **sécurité défensive**.

---

## Rappel : qu’est-ce que le process hollowing ?

Le process hollowing est une technique qui consiste à :
- créer un processus légitime (par exemple `notepad.exe`)
- le suspendre avant qu’il ne commence réellement à s’exécuter
- manipuler son état interne (mémoire / contexte)
- reprendre son exécution

Dans un contexte malveillant, cette technique permet de faire exécuter un autre code sous l’apparence d’un programme légitime.

Dans cet exercice, nous nous limitons volontairement à **l’analyse et à la compréhension** du mécanisme, sans implémenter de comportement offensif fonctionnel.

---

## Étape 1 – Création d’un processus suspendu

Le programme crée un processus `notepad.exe` avec le flag `CREATE_SUSPENDED`.

Cela signifie que :
- le processus existe
- son thread principal est créé
- aucune instruction n’a encore été exécutée

Cette étape est essentielle car elle permet d’observer et d’analyser l’état initial du processus **avant toute exécution de code**.

---

## Étape 2 – Récupération du contexte du thread principal

Une fois le processus suspendu, le programme récupère le contexte du thread principal à l’aide de l’API `GetThreadContext`.

Le contexte contient notamment :
- les registres CPU
- le pointeur d’instruction (`RIP`)
- les registres généraux (`RAX`, `RBX`, `RDX`, etc.)

Le registre `RIP` indique l’adresse de la prochaine instruction qui sera exécutée lorsque le thread sera repris.

---

## Étape 3 – Identification du PEB

Lors de la récupération du contexte, le registre `RDX` contient un pointeur vers le **PEB (Process Environment Block)**.

Le PEB est une structure interne de Windows qui contient :
- des informations globales sur le processus
- l’adresse de l’image principale chargée en mémoire
- des informations sur les modules et l’environnement

Identifier correctement le PEB démontre une bonne compréhension de l’architecture interne de Windows.

À ce stade :
- aucune lecture mémoire distante n’est effectuée
- aucune modification du processus n’est réalisée

---

## Étape suivante (théorique, non implémentée)

Dans une implémentation complète de process hollowing, les étapes suivantes consisteraient théoriquement à :

- lire les informations du PEB
- identifier l’adresse de l’image principale
- modifier la mémoire du processus
- rediriger le flux d’exécution via le registre `RIP`

Ces étapes **ne sont volontairement pas implémentées** dans ce projet afin d’éviter tout comportement assimilable à un malware fonctionnel.

Elles sont uniquement décrites pour démontrer la compréhension du mécanisme global.

---

## Étape 4 – Reprise du thread et comportement normal

Le thread principal est ensuite repris avec `ResumeThread`.

Comme aucune modification n’a été effectuée :
- le processus reprend son exécution normale
- `notepad.exe` démarre normalement

Cela confirme que les étapes précédentes étaient purement analytiques.

---

## Vision sécurité et détection

Les techniques de type process hollowing sont activement surveillées par les solutions de sécurité (EDR, antivirus), car elles présentent plusieurs indicateurs suspects, notamment :

- création de processus avec le flag `CREATE_SUSPENDED`
- appel à `GetThreadContext` sur le thread principal
- enchaînement rapide `CreateProcess → GetThreadContext → ResumeThread`

Comprendre ces mécanismes est essentiel pour :
- analyser des malwares
- comprendre les alertes EDR
- renforcer la détection comportementale

---

## Conclusion

Cet exercice montre qu’il est possible de comprendre en profondeur une technique offensive **sans jamais produire de code malveillant exploitable**.

L’approche adoptée permet de :
- apprendre le fonctionnement interne de Windows
- comprendre les attaques modernes
- tout en respectant un cadre éthique et académique
