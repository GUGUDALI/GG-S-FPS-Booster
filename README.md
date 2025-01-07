# GG'S FPS Booster

Counter Strike 2 FPS Booster

This program is designed to optimize the performance of a game process, specifically "CS2", by adjusting the priority and processor affinity of various system processes.

The ProcessList class is a simple data structure that holds information about a process, including its ID, priority class, priority boost status, and processor affinity. This class is used to store the original settings of processes so they can be restored later.

The program enters an infinite loop where it waits for the "CS2" process to start. Once the game is detected, it adjusts the priority and processor affinity of all other processes to idle, except for critical system processes and the game itself. The game's process is set to real-time priority with a high processor affinity to boost its performance.

When the game process exits, the program restores the original settings of all processes. This ensures that the system returns to its normal state after the game is closed.

Overall, the program aims to enhance the gaming experience by dynamically managing system resources to prioritize the game process.

Just start the program as administrator and enjoy your game.

**WARNING!!!**

Don't close the program while writing anything other than waiting for cs2 on the screen.
**You need to close cs2 firts**
