# Exercise 01 – Shellcode Runner

## Objectif
L’objectif de cet exercice est de comprendre comment exécuter du shellcode
directement en mémoire sous Windows à l’aide des API système.

Le payload utilisé est volontairement inoffensif (ouverture de `calc.exe`,
la calculatrice Windows).

---

## Environnement
- DEV-VM : Windows + Visual Studio
- TARGET-VM : Windows
- Langage : C#
- Framework : .NET

---

## Principe de fonctionnement

Le programme effectue les étapes suivantes :

1. Un shellcode est stocké sous forme de tableau de bytes
2. Une zone mémoire exécutable est allouée avec `VirtualAlloc`
3. Le shellcode est copié en mémoire
4. Un thread est créé pour exécuter ce shellcode
5. Le programme attend la fin du thread

---

## API Windows utilisées

- `VirtualAlloc` : allocation de mémoire exécutable
- `CreateThread` : création d’un thread d’exécution
- `WaitForSingleObject` : synchronisation
- `Marshal.Copy` : copie mémoire

---

## Observations

- Windows Defender détecte le comportement comme suspect
- Le binaire doit être publié via `dotnet publish`
- L’exécution fonctionne sur une machine cible uniquement
  lorsque le dossier `publish` est copié

---

## Conclusion

Cet exercice démontre les bases de l’exécution de code en mémoire sous Windows.
Il met en évidence les mécanismes fondamentaux utilisés par certains malwares,
sans implémenter de logique malveillante (calc.exe ouvre la calculatrice - payload inoffensif).
