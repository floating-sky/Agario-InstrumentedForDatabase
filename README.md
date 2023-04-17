```
Author:     C Wyatt Bruchhauser
Partner:    Julia Thomas
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  wyattbworld and floating-sky
Repo:       https://github.com/uofu-cs3500-spring23/assignment8agario-julia-and-wyatt
Date:       APR-17-2023
Solution:   Agario
Copyright:  CS 3500, Julia Thomas, C Wyatt Bruchhauser - This work may not be copied for use in Academic Coursework.
```

# Contents of Program
This program provides a client to interact with the Agario Server as defined by the CS3500 class.
It includes:
An AgarioModels project that stores every object used in the game.
A ClientGUI class that represents the view of the players.
A FileLogger class that can log information about the game.
The TowardAgario Projects are just for practice and should not be considered a part of the codebase.
This solution assumes the user is running the server as provided by the CS3500 canvas page, and the networking.dll also provided by the networking page.

This client includes:
A way to change your user name.
A way to connect to different servers using server ids.
A view of the agario server.
A way to move the player character using the mouse.
A way to split using a button.
Statistics about the game both during and after.
A game over screen.
A display area for networking error messages.

# UI Decisions
-We decided to put the player names above the agario players, so that when a player is given a white skin the text is still legible.
-We decided to display mass as the most important statistic as it measures how well the player played the game.

# Time tracking
- Estimate: 18 hrs 
- Time to Completion: 16 hrs
Our estimate was pretty spot on, though I think this assignment looked a lot more intimidating than it ended up being and
things really fell into place during the final parts of the project. Though if you can't the practice assignments we did
individually beforehand the time to completion would probably go up to around 26 hrs.

# Performance:
The code runs pretty fluently on both of our computers. We tell the screen to invalidate every heartbeat. We suspect
that the network speed is the bottle neck for our performance as the screen can only draw when it gets a message from
the server which ties it to the screen, and our drawing code is pretty minimal.

# Partnership Info
This entire assignment was done using pair programming.

# Comments to Evaluators:
I chose to represent my dependency graph as a dependency graph because I already implemented so why repeat myself?
I chose to represent my spreadsheet as a dictionary because it allows me to lookup and modify cells in constant time.

# Assignment Specific Topics
N/A

# References

Assignment 4 instructions on Canvas

CS3500 Lectures