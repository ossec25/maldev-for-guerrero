# Exercice 05 – Antivirus Evasion via Obfuscation

## 1. Objectif de l’exercice

L’objectif de cet exercice est de comprendre comment certains logiciels malveillants
tentent d’échapper à la détection des antivirus basés sur des signatures statiques.
L’exercice se concentre exclusivement sur le principe d’obfuscation, sans implémenter
de comportement malveillant.

## 2. Principe théorique

De nombreux antivirus analysent les binaires à la recherche de signatures connues
(chaînes de caractères, séquences d’octets, patterns spécifiques).
L’obfuscation consiste à modifier la représentation statique de ces données afin
qu’elles ne soient jamais stockées en clair dans le binaire.

Dans cet exercice, deux techniques simples sont utilisées :
- XOR avec une clé fixe
- Encodage Base64

Ces techniques ne sont pas suffisantes à elles seules pour contourner un antivirus
moderne, mais elles permettent d’illustrer le principe général.

## 3. Fonctionnement du binaire

Le programme suit les étapes suivantes :
1. Une chaîne de caractères inoffensive est définie en clair.
2. Cette chaîne est obfusquée à l’aide d’un XOR simple, puis encodée en Base64.
3. La version obfusquée est celle qui pourrait être stockée dans le binaire final.
4. À l’exécution, la chaîne est décodée et reconstruite dynamiquement en mémoire.
5. Le texte original est affiché pour démontrer la réussite de la reconstruction.

À aucun moment la donnée sensible n’est stockée en clair dans le binaire compilé.

## 4. Limites volontaires de l’exercice

Cet exercice s’arrête volontairement à la démonstration du principe d’obfuscation.
Le programme ne contient :
- Aucun shellcode
- Aucun chargement dynamique de code
- Aucun appel système sensible
- Aucune communication réseau
- Aucune persistance

L’étape suivante, qui consisterait à appliquer ces techniques à du code dynamique
ou à des charges actives, est volontairement exclue afin d’éviter tout risque de
comportement malveillant.

## 5. Test sur machine cible (DV-TARGET)

Le binaire compilé a été transféré sur une machine Windows distincte (DV-TARGET) pour y être exécuté.
Lors du lancement, l’exécution a échoué avec un message indiquant l’absence du runtime .NET sur cette machine.
Ce comportement est parfaitement normal et attendu :
le binaire généré est une application .NET qui dépend du runtime présent sur la machine cible pour fonctionner.
Cette étape met en lumière l’importance des dépendances logicielles lors du déploiement d’un binaire, qu’il soit légitime ou potentiellement malveillant.
Aucune modification n’a été apportée pour contourner ce mécanisme, ni pour rendre le binaire autonome, afin de rester dans un cadre strictement académique et sécurisé.
Enfin, aucune alerte antivirus ni comportement suspect n’a été détecté sur DV-TARGET, ce qui confirme que cet exercice reste purement démonstratif et sans danger.

## 6. Conclusion

Cet exercice permet de comprendre pourquoi les antivirus basés uniquement sur
des signatures statiques peuvent être contournés par des techniques simples
d’obfuscation, et souligne l’importance des analyses comportementales et dynamiques.
