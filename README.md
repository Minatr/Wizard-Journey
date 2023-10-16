# Wizard-Journey
&emsp; A mobile game where, as a lost wizard, you have to explore a new region and fight for your life. To survive, you'll have to beat and capture monsters. But these one might be more useful than you think...

| Current Version | Last Update |
| :-------------: | :---------: |
| [0.1.2](#0.1.2)  |  16/10/2023 |

Click [here](https://drive.google.com/drive/folders/1f4IrIE74F7613GIdeKUhKwHYmx5zdhfh?usp=sharing) to download the game.  
Click [here](https://drive.google.com/drive/folders/1qk0YfYEGQyMGLkgiDCKypP7GYX3E1Q5K?usp=sharing) to dowload oldest versions.

## Update informations

| Version | What's new | Improvements | Date |
| :-----: | ---------- | ------------ | :--: |
| [0.1.2](#0.1.2) | | <ul><li> Save system (__enemies positions__) </li></ul> | 16/10/2023 |
| [0.1.1](#0.1.1) | <ul><li> Welcome Scene </li><li> __Save__ system (player position, actual scene, enemy, ...)</li></ul> | <ul><li> __UI__ </li><li>New __Font__ </li></ul> | 11/10/2023 |
| [0.1.0](#0.1.0) | <ul><li> __Fight__ system (zone, animations, visuals, ...) </li></ul> | | 16/09/2023 |
| [0.0.4](#0.0.4) | <ul><li> __MiniMap__ </li><li>__Names__ and __Levels__ of the map's enemies</li></ul> | <ul><li>Project __optimization__ (removed URP)</li><li>Fixed __Joystick__ first touch</li></ul> | 04/09/2023 |
| [0.0.3](#0.0.3) | <ul><li> __Battle__ starting when touching enemies (transitions, animations)</li></ul> | <ul><li> __Enemies__ (movements, detection system improved)</li><li> __Map__ (decoration, mountains)</li></ul> | 25/08/2023 |
| [0.0.2](#0.0.2) | <ul><li> __Enemies__ (visuals, movements, animations)</li><li>Enemy's __AI__ (wandering mode, chasing mode)</li></ul> | | 18/08/2023 |
| [0.0.1](#version-001-) | <ul><li> __Map__ </li><li> __Main character__ (animations, movements)</li><li> __Joystick__ </li></ul> | | 31/07/2023 |

---

## The Development Process

&emsp; The idea came after looking a [video](https://youtu.be/cqNBA9Pslg8?si=06BDQ_gbVYugMbp4) about game in 2.5D. 2D sprite in 3D world, it's that simple. There has been a long time since i wanted to start developing my first mobile game. The time had come.


### <h3>Version 0.0.1 :</h3>

&emsp; I started with creating the map with the Unity's terrain tool. Just a plane with a little path and some textures found on the [Unity Asset Store](https://assetstore.unity.com/packages/2d/textures-materials/25-free-stylized-textures-grass-ground-floors-walls-more-241895), enough for the moment.

&emsp; Motivated but without any kind of concept, I began to search for the main character's sprite. Surprisingly, I found it pretty quickly : a free wizard template on [CraftPix](https://craftpix.net/freebies/free-wizard-sprite-sheets-pixel-art/?num=1&count=191&sq=wizard%20spritesheet&pos=2). As we can see below, many animations were already made, I just had to choose one : the ice wizard.
 
![Characters GIF](/ReadmeAssets/Free-Wizard-Sprite-Sheets-Pixel-Art.gif)

&emsp; Thanks to some tutorials, I've developped the motion and animation systems with a [Joystick](https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631) already functional found, one more time, on the Unity Asset Store.  


### <h3 id="0.0.2">Version 0.0.2 :</h3>

&emsp; Remembering myself a [tutorial](https://youtu.be/xmUfhGpA5qA?si=V3IH66riS_IPqLCb) about enemy AI, I decided to implement it into the project. First of all, I searched for some sprite. Here, [CraftPix](https://craftpix.net/freebies/free-werewolf-sprite-sheets-pixel-art/?num=1&count=495&sq=werewolf%20sprite%20sheet&pos=4) helped me one more time with offering many choices and great sprite sheets. This werewolf (just below) appeared, all the visual work was already done, I just needed to develop the enemy system.

![Werewolf GIF](/ReadmeAssets/Free-Werewolf-Sprite-Sheets-Pixel-Art.gif)

&emsp; Now that I had the sprite, I've implemented the AI. This system is divided in many "mode", 3 to precise, and they all have their usefullness. It's quite simple : 
 - Wandering mode : if there's no one to attack around (they only attack the players at the moment), they just randomly walk and wait to differents points.
 - Chasing mode : if the player enter in the enemy's detection zone, the enemy will accelerate and start chasing the player.
 - Attack mode : if the player enter the enemy's combat zone, the battle scene is launched.

![Werewolf range](/ReadmeAssets/werewolf_range.png)


### <h3 id="0.0.3">Version 0.0.3 :</h3>

&emsp; It was at this point time for some fix. They were some problems with the game camera, and many imbalances with the way the enemies worked. The goal of this version was mainly to give a more comfortable experience to the player.


### <h3 id="0.0.4">Version 0.0.4 :</h3>

&emsp; Since the beginning of the project, I had in mind something : a minimap. By taking inspiration mostly from this [tutorial](https://youtu.be/28JTTXqMvOU?si=aHC2TFqMhSpCj0La), I managed to realize my own minimap.

![minimap](/ReadmeAssets/minimap.png)

&emsp; In addition, I wanted to add some identity to the enemies of the map. So I decide to add some UI around them in order to improve the immersion of the player.

![enemy_ui](/ReadmeAssets/werewolf_ui.png)

&emsp; The more the project progressed and the more lag there was. I was scared my code wasn't optimized enough, but I found out what was the problem. When I created the project while following the first tutorial about 2.5D games, I've included the [Universal Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@16.0/manual/index.html). It allows you to improve a lot your game graphics, the shader, the light, but it isn't yet optimized for mobile game (anyway, little mobile games don't need it, especially in my case). So I took it out from the project which allowed me to delete all lags and some useless storage.


### <h3 id="0.1.0">Version 0.1.0 :</h3>

&emsp; Here is the first major update for my game. A whole turn based fight system. 

![enemy_ui](/ReadmeAssets/fight_screen.png)

&emsp; This is for now very simple : you can either choose to make a simple attack or to use magic in order to make more damages. However, you gonna spend some mana while doing magic, and you can recover it only with doing simple attacks.

![enemy_ui](/ReadmeAssets/UI_animations.png)
![enemy_ui](/ReadmeAssets/mana_ui.png)

&emsp; Finally, you can win or lose the figth, if you make the wrong choices ! At the end, you can actually see only one statistic for the fight (the chrono), but it'll be improved in the next updates.

![enemy_ui](/ReadmeAssets/victory_panel.png)

> I have to say that I'm very proud of myself, I didn't know I could make something that cool (I hope it is). It gives me a lot of motivation for the project and simply my career in game dev !


### <h3 id="0.1.1">Version 0.1.1 :</h3>

&emsp; As I was adding more and more stuff to my game, I knew the more I would wait, the more it would be to implement a save system. So that's what the update is about : the game now remember the last scene you played, your last position and your current enemy if you have one. They aren't that many things saved actually, cause I keep it for a next version with a whole stat system (experience, attack, defense, HP, ...).


### <h3 id="0.1.2">Version 0.1.2 :</h3>

&emsp; This update is a little fix and improvement of the last version that added an efficient but incomplete save system. Now, after having upgraded and transformed the SaveSystem in a singleton (as I'm currently learning design patterns, why not using them when it's possible to), the game remember all the ennemies on the map and their position. If you kill one or if one beats you, he disappears from the area. The map creates new enemies if you leave to another area and come back (the fight area doesn't count). Unfortunately, they are no other area for now, that may be one of the next step ...




