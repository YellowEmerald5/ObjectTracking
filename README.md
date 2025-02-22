This package is used to track the position of game objects and writing the results to a MySQL database.

Setup:
All game Objects mentioned below can be found in Editor/ObjectsToPlaceInScenes.

Main menu:\
1: Add the NicknameInput to any canvas used prior to starting the game.\
2: Add the StorageSetup object in the scene.

Game scenes (scenes With Objects to track):\
1: Add the GamestateObserver to the scene.\
2: Add the SetUpObjectTracking script to all Objects to track. This should also be added to any object instantiated in code or as prefabs. This script is located under Runtime/SetUpScripts.


NB!\
Control that the values in StorageSO are as follows before building the application.\
Nickname 			(This field should be empty)\
Session ID 		0\
Amount Of Arrays 	0

Scriptable Objects are located under Runtime/ScriptableObjects.\
Discrepancies may cause unexpected behavior.


Instructions for missing scriptable objects:

The following steps are not necessary if the ObjectTracking folder has been copied to the project.

If the RequiredScriptableObjectsStorage is not in the project.

1. Right-click in Runtime/ScriptableObjects, select create, then ScriptableObjects and RequiredScriptableObjectsStorage.
2. Add the required scriptable objects to the RequiredScriptableObjectsStorage by dragging and dropping them on the serialized fields.

If the other scriptable Objects are not present in the project, then create these by right-clicking in Runtime/ScriptableObjects, select create then ScriptableObjects, and StorageSO. The remaining scriptable objects are created by selecting GameEvents and GameEvent in the creation menu.
Give the scriptable objects meaningful names as they must be manually added to the RequiredScriptableObjectsStorage.

Scriptable objects:\
Scriptable objects keeps the information given to them while they are used in the editor. Information given to them while playing a built Version of the game is not stored after the game has ended and the scriptable object will return to the values it contained before the game is built.

Package behavior:\
This package is built on the idea of using scriptable objects as events. These events allow for objects to cause and react to actions in the program without being aware of the objects causing or reacting to the changes.
The game state tracker script keeps track of the objects added and removed from the scene through the use of game events and saves all objects to the database when all are accounted for.
