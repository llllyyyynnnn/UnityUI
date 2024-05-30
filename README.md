# UnityUI
Attach the script to a single GameObject to have a fully functional user interface instantly.

# Features
* Animations
* Customizability (colors, fonts, gradients, speed, padding, textsize, buttonsize)
* Quick and easy implementation
* No canvas system required
* Notification system (message, duration, animations)

# Requirements
* InputActions with a "UI" template
* Submit (enter / leftclick)
* Navigate (Keyboard arrow keys / controller)
* Cancel (Escape)

# Issues
* Only works with the new InputSystem.
* Some parts are slightly hardcoded

# WIP
* Support for both InputSystems
* Code cleanup

After the InputActions have been made, the script should immediately work as it should. If nothing shows up, make sure the serialized color variables are set to actual colors that have alpha > 0. A gradient can be added (as well as any other image) by attaching it to the script, alongside 3 fonts. 

The purpose of this project was to eliminate the need for me to use canvases in Unity as I found that to be slightly more annoying than simply managing everything in one script. This gave me the possibility to focus more on making it look and feel smoother as well as the portability between multiple projects as long as UI InputActions have been imported. As for the simplicity, it only requires a single object in a scene to function.
