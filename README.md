# OpenLingo
Basic C# application involving basic networking using SSL sockets and file IO
Uses a client-server pattern.

##Running OpenLingo

OpenLingo currently has no central server.
You are required to start a server locally first, once this gives the OK you may add clients.
Clients start by connecting to the lobby hosted by the server. You can confirm it works by seeing the server
print an acknowledgement that a client has connected. Once multiple clients are connected, they will be able to see
all other clients present in the lobby listed.

##Playing an OpenLingo game

Select a client from the clients list, and challenge that client to a game.
The other client will receive an invitation that it has been challenged and may choose to accept or decline the offer.
Once the other client accepts, the other clients activity in the lobby is interrupted and both clients will be taken to the game screen.
Now, the clients will start playing sequentially, being able to watch the others progress. 

The rest of the game has yet to be programmed out, give me a few days.

##How does OpenLingo work?

The clients handle a users input, locally stores user specific configurations and displays the current state as told by the server. 

A client has no further responsibility and can be considered "stupid", as it has no actual logic and cannot operate without the server.

The server has the most responsibility, as it keeps track of all clients states, and handles the game procedures for its connected clients. 

A small storage layer exists under the server, to hold the words used in the lingo games and possibly scores too.
All communication between client and server is done via a secure socket layer (SSL) connection, however as everybody should be able to host their own server locally, I have included a self signed certificate in the Certificate folder. Currently there is no option to set the server to use a designated certificate, this would be a nice.

To avoid concurrency issues, the connection uses a simple package system. A client sends a request to the server through a singleton instance of the package manager, this returns a promise-style ID which can later be used to aacquire the response from the server. This makes the requests thread-safe and allows packages to be read in any order.

##Acknowledgements

I have procrastinated on this project for 3 years now. That I managed to get myself together and actually make this program usable is all thanks to my classmates and team members of Comfy Corp. And a special thanks to my teacher for leaving me the freedom to make something fun and having the patience to let me procrastinate this much.
