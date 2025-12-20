# Exercise 04 – Process Hollowing (analyse contrôlée)

## Objectif de l’exercice

Le but de cet exercice est de comprendre le fonctionnement interne d’une technique appelée process hollowing, souvent associée aux malwares, sans aller jusqu’à une implémentation malveillante réelle.
L’objectif est pédagogique :
comprendre comment Windows gère un processus, son thread principal et son contexte d’exécution, afin de mieux analyser et détecter ce type de comportement d’un point de vue sécurité défensive.

---

##Rappel : qu’est-ce que le process hollowing ?

Le process hollowing est une technique qui consiste à :
- créer un processus légitime (par exemple notepad.exe)
- le suspendre avant qu’il ne commence réellement à s’exécuter
- manipuler son état interne (mémoire / contexte)
- reprendre son exécution

Dans un contexte malveillant, cette technique permet de faire exécuter un autre code sous l’apparence d’un programme légitime.
Dans cet exercice, nous nous limitons volontairement à l’analyse et à la compréhension.

---

## Étape 1 – Création d’un processus suspendu
Dans le programme, un processus notepad.exe est créé avec le flag CREATE_SUSPENDED.
Cela signifie que :

- le processus existe
- son thread principal est créé
- aucune instruction n’a encore été exécutée

Cette étape est essentielle car elle permet d’observer et d’analyser l’état initial du processus avant toute exécution de code.

## Étape 2 – Récupération du contexte du thread principal
Une fois le processus suspendu, le programme récupère le contexte du thread principal à l’aide de GetThreadContext.
Le contexte contient notamment :

- les registres CPU
- le pointeur d’instruction (RIP)
- les registres généraux

Le registre RIP indique l’adresse de la prochaine instruction qui sera exécutée lorsque le thread sera repris.

## Étape 3 – Identification du PEB
Lors de la récupération du contexte, le registre RDX contient un pointeur vers le PEB (Process Environment Block).
Le PEB est une structure interne de Windows qui contient :

- des informations sur le processus
- l’adresse de l’image principale chargée en mémoire
- des informations sur les modules et l’environnement

Le fait d’identifier correctement le PEB montre que l’on comprend où Windows stocke les informations critiques d’un processus.
À ce stade, aucune lecture ou modification mémoire n’est effectuée.

## Étape suivante (théorique, non implémentée)
Dans une implémentation complète de process hollowing, l’étape suivante consisterait théoriquement à :

- lire les informations du PEB
- identifier l’adresse de l’image principale
- modifier la mémoire du processus
- rediriger le flux d’exécution via le registre RIP

Remarques: ces étapes ne sont volontairement pas implémentées dans ce projet afin d’éviter tout comportement assimilable à un malware fonctionnel.
Elles sont uniquement décrites ici pour démontrer la compréhension du mécanisme global.

---

7. Reprise du thread et comportement normal
Le thread principal est ensuite repris avec ResumeThread.
Comme aucune modification n’a été faite :

- le processus reprend son exécution normale
- notepad.exe démarre normalement

Cela confirme que les étapes précédentes étaient purement analytiques.

---

## Vision sécurité et détection
Les techniques de type process hollowing sont activement surveillées par les solutions de sécurité (EDR, antivirus) car elles présentent plusieurs indicateurs suspects :

- création de processus suspendus
- accès au contexte de threads
- manipulation de la mémoire inter-processus

Comprendre ces mécanismes est essentiel pour :

- analyser des malwares
- comprendre les alertes EDR
- renforcer la détection comportementale

---

## Conclusion
Cet exercice montre qu’il est possible de comprendre en profondeur une technique offensive sans jamais produire de code malveillant exploitable.

L’approche adoptée permet de :

- apprendre le fonctionnement interne de Windows
- comprendre les attaques modernes
- tout en respectant un cadre éthique et académique



