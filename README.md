# Multiplayer Lobby Concurrency – 3rd Semester Project

This was our 3rd semester project, where we explored how to handle concurrency issues in a multiplayer environment.

We built a simple web-based game client with a lobby system, and focused on solving the problem of **multiple players trying to join the same lobby** when there are only a limited number of spots available.

The core challenge was to ensure that the lobby state remained consistent and no more players could join than allowed — even if several users attempted to join at the exact same time.

It was a great learning experience that gave us hands-on experience with concurrency control in web applications and how to handle race conditions in a real-time setting.
