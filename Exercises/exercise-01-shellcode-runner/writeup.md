# Exercise 01 – Shellcode Runner

## Objectif
L’objectif de cet exercice est de comprendre comment exécuter du shellcode
directement en mémoire sous Windows à l’aide des API système.

Le payload utilisé est volontairement inoffensif (ouverture de calc.exe,
la calculatrice Windows), afin de se concentrer sur le mécanisme d’exécution
en mémoire plutôt que sur l’impact du payload.

---

## Environnement
- DEV-VM : Windows + Visual Studio Community Edition
- TARGET-VM : Windows 10/11
- Langage : C#
- Framework : .NET

---

## Principe de fonctionnement

Le programme effectue les étapes suivantes :
 1. Un shellcode est stocké sous forme de tableau de bytes (byte[])
 2. Une zone mémoire exécutable est allouée avec VirtualAlloc
 3. Le shellcode est copié dans cette zone mémoire
 4. Un thread est créé pour exécuter le shellcode
 5. Le programme attend la fin de l’exécution du thread

Ce flux représente le fonctionnement minimal d’un shellcode runner
dans un processus Windows.

---

## API Windows utilisées

- VirtualAlloc : allocation d’une zone mémoire avec des droits d’exécution
- CreateThread : création d’un thread démarrant à l’adresse du shellcode
- WaitForSingleObject : synchronisation et attente de la fin du thread
- Marshal.Copy : copie du shellcode depuis le code managé vers la mémoire native

---

## Observations

- Windows Defender détecte rapidement ce type de comportement comme suspect,
ce qui est attendu pour un shellcode runner simple
- Le binaire doit être compilé et publié via dotnet publish
- L’exécution sur la machine cible fonctionne lorsque le dossier publish
est copié intégralement sur la machine cible
- Ce binaire ne contient aucune technique d’évasion ou d’obfuscation

---

## Conclusion

Cet exercice démontre les bases de l’exécution de code en mémoire sous Windows.
Il met en évidence les mécanismes fondamentaux utilisés par certains malwares,
tout en restant dans un cadre pédagogique, avec un payload inoffensif
(l’ouverture de la calculatrice Windows).

Il constitue une base essentielle pour comprendre les exercices suivants,
qui introduiront des mécanismes plus avancés.
