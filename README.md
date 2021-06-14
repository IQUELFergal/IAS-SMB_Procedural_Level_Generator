# IAS - Générateur de niveau Super Mario Bros

	Afin de créer un générateur de niveau en 2D, j’ai utilisé Unity 2019.4.18f1 et son système de tilemap (grille de tile). En plus des fonctionnalités de base, j’ai installé un package en développement par Unity appelé 2D-extras pour pouvoir créer des ruletiles (des tiles qui auront un apparence différente en fonction des tiles voisines, suivant un ensemble de règles définies). Pour faciliter la création de ces règles, j’ai aussi installé le package Tilemap auto rule qui permet de créer automatiquement une ruletile et les règles pour toutes les configurations possibles à partir d’une texture et d’une ruletile de référence. 

	Le projet est pensé pour être un éditeur de niveau, l’ensemble de la génération a donc lieu dans l’éditeur Unity et lancer l’application ne sert qu’à lancer une partie avec le niveau actuel.

	Pour créer un niveau le programme à besoin d’une seed et de paramètres pour définir les caractéristiques du niveau.  Une fois les valeurs souhaitées fixées, le programme génère le niveau en créant des segments de niveau appelés « Chunks ». Chaque Chunk est peuplé de ChunkElements (plateformes, trous, ennemis, etc …) en fonction des paramètres donnés en entrée du générateur. Ils sont soumis à un ensemble de règles pour vérifier la jouabilité du résultat et sont ensuite convertis en tableau de tiles. Ces tableaux sont finalement juxtaposés pour créer l’ensemble du niveau et sont donnés au TilemapGenerator qui s’occupe de dessiner la tilemap en sortie.

	J’ai ensuite ajouté le package Unity WFC cité dans le sujet du projet comme base que j’ai dérivée pour pouvoir utiliser les tilemap Unity. J’ai développé une classe TilemapTraining qui parcourt la tilemap d’entrée et une classe TilemapOverlapWFC qui utilise les patterns issus de TilemapTraining pour créer une tilemap de sortie de la taille désirée.

	Je n’ai pas eu le temps de peaufiner les paramètres  et de créer des tiles spéciales pour utiliser pleinement le WFC, en effet, les résultats que j’obtiens ne sont pas vraiment exploitables pour un niveau. J’utilise donc le WFC pour générer le décor à l’arrière-plan de mon niveau. 

	Avec plus de temps j’aurais aimé mieux maîtriser cet outil pour pouvoir m’en servir pour générer des niveaux avec des plateformes volantes ou des niveaux souterrains avec des murs tout autour. Je pense que les résultats que j’ai obtenus auraient pu être améliorés grâce à l’algorithme des marching squares pour « lisser »  les plateformes trop escarpées et retirer les blocs volants.

	Je suis cependant très satisfait de mon générateur de base qui produit des niveaux très proches de ceux des jeux Super Mario Bros.

Ressources utilisés : 

    • 2D-extras (https://github.com/Unity-Technologies/2d-extras)
	Doc : https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.6/manual/	index.html

    • Tilemap auto rule (https://pandaroo.itch.io/tilemap-auto-rule-tile-unity-template)
      
    • Unity WFC (https://selfsame.itch.io/unitywfc)
	Tutoriel : https://gist.github.com/selfsame/e7ff11205c316888977f9cac04fe4035
