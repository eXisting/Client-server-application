# Client-server-application

## MVP ( Model-View-Presenter ) pattern application

Task statement: 
 * develop the program that allows you to establish connection between client and server folder in way that changes on server will automatically perfrom by client ( folder synchronization );
 * implement program with MVP pattern;
 * consider, that only one client can connect to server;
 * log all actions that occures on both sides;
 * synchronize all data (txt, img, music, video);

### Instruction: 

* You must choose folder on both sides, then connect to the server by typing it's ip adress and port ( look at ports that shown in server application ).

* After you have changed something inside the server side folder, client program gets byte command from server and interpreter it into one of expected commands ( rename, create, delete ).

##### Don't try to rename file directly when have just created it!

* After these actions, client folder will recieve all changes from server folder.
