# Wizard-Journey
&emsp; A mobile game where, as a lost wizard, you have to explore a new region and fight for your life. To survive, you'll have to beat and capture monsters. But these one might be more useful than you think...

| Current Version | Last Update |
| :-------------: | :---------: |
| [0.0.4](#0.0.4)  |  04/09/2023 |

Click [here](https://drive.google.com/drive/folders/1f4IrIE74F7613GIdeKUhKwHYmx5zdhfh?usp=sharing) to download the game.  
Click [here](https://drive.google.com/drive/folders/1qk0YfYEGQyMGLkgiDCKypP7GYX3E1Q5K?usp=sharing) to dowload oldest versions.

## Update informations

| Version | What's new | Changes | Date |
| :-----: | ---------- | ------- | :--: |
| [0.0.4](#0.0.4) | <ul><li>__MiniMap__</li><li>__Names__ and __Levels__ of the map's enemies</li></ul> | <ul><li>Project __optimization__ (removed URP)</li><li>Fixed __Joystick__ first touch</li></ul> | 04/09/2023 |
| [0.0.3](#0.0.3) | <ul><li> __Battle__ starting when touching enemies (transitions, animations)</li></ul> | <ul><li> __Enemies__ (movements, detection system improved)</li><li> __Map__ (decoration, mountains)</li></ul> | 25/08/2023 |
| [0.0.2](#0.0.2) | <ul><li> __Enemies__ (visuals, movements, animations)</li><li>Enemy's __AI__ (wandering mode, chasing mode)</li></ul> | | 18/08/2023 |
| [0.0.1](#0.0.1) | <ul><li> __Map__ </li><li> __Main character__ (animations, movements)</li><li> __Joystick__ </li></ul> | | 31/07/2023 |

---

## The Development Process

&emsp; The idea came after looking a [video](https://youtu.be/cqNBA9Pslg8?si=06BDQ_gbVYugMbp4) about game in 2.5D. 2D sprite in 3D world, it's that simple. There has been a long time since i wanted to start developing my first mobile game. The time had come.


### <h3 id="0.0.1">Version 0.0.1 :</h3>

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

&emsp; In addition, I wanted add give some identity to the enemies of the map. So I decide to add some UI around them in order to improve the immersion of the player.

![enemy_ui](/ReadmeAssets/werewolf_ui.png)





