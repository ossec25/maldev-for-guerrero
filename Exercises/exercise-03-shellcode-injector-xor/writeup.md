# Exercice 03 – Shellcode Injector avec obfuscation XOR

## Objectif

L’objectif de cet exercice est d’introduire une première technique
d’évasion simple : l’obfuscation du shellcode via un XOR.

Contrairement à l’exercice 02, le shellcode n’est plus stocké en clair
dans le binaire, mais décodé dynamiquement en mémoire avant son injection.

À partir de l’exercice 03, les projets sont structurés comme de vrais projets .NET, avec fichiers .csproj, afin de faciliter
la compilation, le publish et la reproductibilité

---

## Principe général

Les étapes sont les suivantes :

1. Le shellcode est stocké sous forme XORée dans le code C#
2. Une clé XOR est utilisée pour décoder le shellcode à l’exécution
3. Le shellcode décodé est injecté dans un processus distant
4. Un thread distant est créé pour exécuter le shellcode

Le processus cible utilisé pour cet exercice est `notepad.exe`.

---

## Différences avec l’exercice 02

| Exercice 02 | Exercice 03 |
|------------|-------------|
| Shellcode en clair | Shellcode obfusqué (XOR) |
| Détection plus simple | Signature modifiée |
| Injection directe | Décodage préalable en mémoire |

La technique d’injection reste identique, seule la préparation du payload change.

---

## API Windows utilisées

- OpenProcess  
- VirtualAllocEx  
- WriteProcessMemory  
- CreateRemoteThread  
- WaitForSingleObject  

Ces API permettent l’injection et l’exécution de code dans un processus distant.

---

## Environnement de test

- DEV-VM : compilation et publication du binaire
- TARGET-VM : exécution du payload
- Système : Windows 10 / 11
- Processus cible : notepad.exe (lancé manuellement)

---

## Résultats des tests

### Windows Defender activé
- Aucun effet visible lors de l’exécution
- Le processus notepad.exe reste actif
- Injection vraisemblablement bloquée par analyse comportementale

### Windows Defender désactivé
- Comportement anormal du processus notepad.exe observé
- L’exécution du shellcode est confirmée

---

## Analyse

L’obfuscation XOR est une technique simple mais pédagogique :
- elle modifie la signature statique du shellcode
- elle ne suffit pas à contourner un antivirus moderne
- elle permet de comprendre les bases des techniques d’évasion

Les antivirus actuels reposent principalement sur :
- l’analyse comportementale
- la surveillance mémoire
- les heuristiques d’exécution

---

## Conclusion

Cet exercice démontre que :
- une injection mémoire classique est facilement détectée
- une simple obfuscation change déjà le comportement de détection
- la préparation du payload est une étape clé en maldev

---

## Suite

L’exercice suivant introduira une technique plus avancée :
**Process Hollowing**, permettant de masquer l’exécution du code
dans un processus légitime.
