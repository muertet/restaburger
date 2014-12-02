RestABurger
===================

RestABurger is a food restaurant game where you have to serve your clients on time.
Made with Unity 4.6

![restaburguer](https://github.com/muertet/restaburguer/blob/master/screenshots/1.jpg?raw=true)
![restaburguer2](https://github.com/muertet/restaburguer/blob/master/screenshots/2.jpg?raw=true)
![restaburguer3](https://github.com/muertet/restaburguer/blob/master/screenshots/3.jpg?raw=true)
![restaburguer4](https://github.com/muertet/restaburguer/blob/master/screenshots/4.jpg?raw=true)

[![Analytics](https://ga-beacon.appspot.com/UA-17476024-7/restaburguer/readme?pixel)](https://github.com/muertet/restaburguer)

----------


About the game
-------------

All of the workers left the restaurant ant and you are the only one left to take over!
- You must cook the food all the way and serve it on a clean plate to earn the maximum amount of cash.
- You have to be quick! The customers won’t wait forever!
- Ran out of food? Press the red button near the cash register to make your food distributor appear for $40!

Please, keep in mind that this is my first Unity project, so folders order is horrible and code may not be accurate.

Available modes
-------------

- Singleplayer
- Multiplayer


Game Controls
-------------
WASD : Moves your character
Left click : Holds an object
E: Action button (open crates / push buttons)



Adding a new plate
-------------
- Open "*scripts/FoodHelper.cs*" file and go to getMenu() function, there you can set the required ingredients and its price:

    		menu.Add (new MenuPlate(new int[]{
    			TYPE_TOPBUN,
    			TYPE_PATTY,
    			TYPE_BUN,
    		}, 4)); // 4 = plate price (4$)

(Ingredient list available on file top)
- Then put it's image on "*Assets/Resources/Food*" folder. Image name must be a join of the required ingredients id. So if plate ingredients are:

    		TYPE_TOPBUN,
    		TYPE_PATTY,
    		TYPE_BUN,

File name would be *657.jpg*, since TOPBUN = 6, PATTY = 5 and BUN = 7. I repeat, ingredient list is available  on file top.

Increase clients:
-------------

- Spawn limit is set at "scripts/ClientSpawn.cs" (constant: CLIENTS_LIMIT)

ToDo
-------------
- Put objects near to player's camera
- GUI : Make ESC menu work
- GUI : Show room's list with filters and allow players to create a new room.
- GUI : Redesign and make it fully responsive for diff resolutions. (Waiting for Unity GUI Editor)
- Find a better network than Photon. (like Bolt, but it should be free)

¿How can i help?
-------------

- Forking the project
- Buying me a book! (http://www.amazon.es/Introduction-Game-Design-Prototyping-Development-ebook/dp/B00LIYS9F0/ref=sr_1_1_twi_1?s=foreign-books&ie=UTF8&qid=1416134444&sr=1-1&keywords=Introduction+to+Game+Design%2C+Prototyping%2C+and+Development%3A+From+Concept+to+Playable+Game+with+Unity+and+C%23, http://www.amazon.co.uk/Pro-Unity-Game-Development-C-ebook/dp/B00K6N4JW6/ref=tmm_kin_swatch_0?_encoding=UTF8&sr=1-3&qid=1406190632)

Credits
-------------
- Human model: miaxmo.com
- Restaurant model: https://3dwarehouse.sketchup.com/model.html?id=u22544b0d-3762-47f1-9177-1bb6d4ef5562
- Grill model: https://3dwarehouse.sketchup.com/model.html?id=b62150029c4b87d051d3c864cc68e22e
- Dishwasher setup model: https://3dwarehouse.sketchup.com/model.html?id=cbbdd4a68537eb3cd464b9d9e1bc685f
- Vehicle model: https://3dwarehouse.sketchup.com/model.html?id=266a0d96d6152725afe1d4530f4c6e24
- Other models: internet.
- FPS bugs fixing:  quill18
- Speechbubble: DimasTheDriver
