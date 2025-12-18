# Exercise 02 – Shellcode Injector

## Objectif
L’objectif de cet exercice est de comprendre comment injecter du shellcode dans un processus existant sous Windows à l’aide des API système.

Le payload utilisé est ici l’ouverture de Notepad (éditeur de texte Windows), démontrant l’injection dans un processus cible.

---

## Environnement
- DEV-VM : Windows + Visual Studio
- TARGET-VM : Windows
- Langage : C#
- Framework : .NET

---

## Principe de fonctionnement

Le programme effectue les étapes suivantes :

1. Recherche le processus cible (ici notepad.exe) par son nom.
2. Ouvre un handle vers ce processus avec les droits nécessaires.
3. Alloue une zone mémoire dans le processus distant.
4. Écrit le shellcode dans cette zone mémoire.
5. Crée un thread dans le processus cible pour exécuter ce shellcode.
6. Attend la fin de ce thread.

---

## API Windows utilisées

- `OpenProcess` : ouvrir un handle vers un processus existant.
- `VirtualAllocEx` : allouer de la mémoire dans un autre processus.
- `WriteProcessMemory` : écrire dans la mémoire d’un autre processus.
- `CreateRemoteThread` : créer un thread distant pour exécuter le shellcode.
- `WaitForSingleObject` : attendre la fin du thread distant.

---

## Observations

- Windows Defender détecte ce comportement comme suspect.
- Le processus cible (notepad) doit être ouvert avant l’injection.
- L’injection s’est bien déroulée sans erreur, mais l’effet visible dépend du shellcode.
- Le payload ici lance/ferme Notepad (ou agit dans Notepad), contrairement à l’exercice 01 où la calculatrice était lancée directement.

---

## Conclusion

Cet exercice démontre la technique d’injection de code dans un processus existant, une méthode souvent utilisée par certains malwares et outils d’analyse.  
Il montre comment manipuler la mémoire et les threads d’un autre processus en C#.

---

