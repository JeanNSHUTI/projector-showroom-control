# projector-showroom-control
A University prototype desktop projection application for mapping image, video and RSS streams. Made in Unity with C#.

## projector-showroom-control
Créer une interface graphique permettant d’afficher/projeter des images, vidéo et Flux RSS (de température par exemple) sur des partitions d’une écran ou projection. 
L’utilisateur devra pouvoir sélectionner les flux de données à importer sur le plan 2D à projeter/affiché. L’interface pourrait être une application web ou une application « standalone » destiné pour PC.

## Fontionnalités attendues
1.	Interface graphique permettant à l’utilisateur d’afficher/projeter des images ( .png, .jpg, .tif ) importés en local sur la surface total d'affichage.
2.	Interface graphique permettant à l’utilisateur d’afficher/projeter des vidéos ( .mp4, .mov, .avi ) importés en local sur la surface total d’affichage.
3.	Interface permettant à l’utilisateur d’afficher/projeter des flux de données RSS (météo) venant d’une source internet.
4.	Interface permettant à l’utilisateur d’afficher/projeter des images/vidéos d’origine internet (cloud/serveur).
5.	Interface graphique permettant à l’utilisateur de “diviser” la surface d’affichage total en parties de forme rectangulaires/triangulaires/circulaires.
6.	Capacité de changer les dimensions de chaque division par l'utilisateur.
7.	Capacité de changer la position de chaque division par l'utilisateur.
8.	Interface graphique permettant à l’utilisateur de coupler/associer chaque flux de médias importé ou sourcé d'internet à une partie ou division de la surface totale d’affichage.
9.	Interface permettant à l’utilisateur de sauvegarder une session ou design d’affichage particulière.

## Commandes fonctionnels
1.	« Upload Media » - permet à l’utilisateur d’importer une image locale.
2.	« Upload Video » - permet à l’utilisateur d’importer une vidéo locale.
3.	« Online Media » - permet à l’utilisateur de d’importer une image à partir d’une URL d’une ressource en ligne.
4.	« Online Video » - permet à l’utilisateur de jouer une vidéo à partir d’une URL d’une ressource en ligne.
5.	« RSS Feed » - permet à l’utilisateur d’ajouter un widget de température au plan 2D à partir d’une URL d’un flux RSS.
6.	« Render » - Projette le plan 2D sur la deuxième scène.

## Prefabs
-	« imageImportObject » : Game Object pour contenir les images.
-	« videoImportObject » : Game Object pour contenir les vidéos.
-	« weatherWidgetNight » et « weatherWidgetDay Variant » : Game Object pour afficher les données d’un flux RSS pour la température.
-	« onlineMediaPanel » : asset de préfabrication pour l’interface graphique
-	« scrollViewTemplates » : asset de préfabrication pour l’interface graphique

## Formats

| Formats                       	| Image 	| Vidéo 	| Flux RSS 	| Testé 	| Vérifié 	| Fail 	|
|-------------------------------	|-------	|-------	|----------	|-------	|---------	|------	|
| JPG, PNG                      	| X     	|       	|          	| X     	| X       	|      	|
| SVG, GIF, TIFF, WEBP          	| X     	|       	|          	| X     	|         	| X    	|
| MOV, MP4, AVI                 	|       	| X     	|          	| X     	| X       	|      	|
| WMV                           	|       	| X     	|          	| X     	| X      	|      	|
| ASF, DV, VP8, MPG, MPEG, WEBM 	|       	| X     	|          	|       	|         	|      	|
| XML                           	|       	| X     	| X        	| X     	| X       	|      	|

## Bibliographie

[1.]	Nshuti, Jean-René. 2022. Capture d’écran de la video de présentation du logiciel FaçadeSignage [Capture d'écran]. https://www.videomappingsoftware.com/.
[2.]	Zenhub. Zenhub. 2022. https://www.zenhub.com/.
[3.]	MapMap Team. Free video mapping software - Logiciel libre de video mapping. https://github.com/mapmapteam/mapmap.
[4.]	HeavyM. HeavyM. 2022. https://www.heavym.net/.
[5.]	FaçadeSignage. FaçadeSignage Easy Projection mapping. 2022. https://www.videomappingsoftware.com/.
[6.]	Danndx. How to get click drag a UI element. https://www.youtube.com/watch?v=CnJtca6FgUs.
[7.]	Reiter, Armin. Feed Reader. 2021. https://github.com/arminreiter/FeedReader
[8.]	Erreur d’affichage du Game Object sur l’interface graphique principale mais sans erreur sur la projection.


