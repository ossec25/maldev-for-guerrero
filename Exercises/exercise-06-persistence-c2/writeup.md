## 6. Persistence et Communication C2 (simulation académique)

Ce binaire a pour objectif de démontrer la compréhension des concepts de
persistance et de communication C2, sans implémenter de mécanisme réel
susceptible d’être exploité.

La persistance est simulée via la création d’un simple fichier marqueur
dans le répertoire utilisateur. Ce mécanisme permet d’illustrer la logique
(détection d’un premier lancement / exécution ultérieure), sans modifier
le système (registre, services, tâches planifiées).

La communication C2 est également simulée par une requête HTTP locale
vers l’adresse 127.0.0.1. Aucune commande n’est reçue, interprétée ou
exécutée. L’absence de serveur est un comportement attendu et normal.

Le code s’arrête volontairement à ce stade afin d’éviter toute création
de malware fonctionnel. Les étapes suivantes (persistance réelle,
commande distante, exécution conditionnelle) sont comprises sur le plan
théorique mais non implémentées, conformément au cadre académique.
