
## CS3500  - A8: An Agile Educational Application

This group project is to build an educational program in C++. There are many parts to this assignment. All your code must be in C++. One member of your team should accept the GutHub assignment link at

https://classroom.github.com/a/vMdeT-ap (Links to an external site.)Links to an external site.

and add in team members.

Group Work
The project will be divided into a planning 1/2 week followed by 2 week-long sprints and a final half sprint. 

You will need to divide into teams of 5-7. You must have at least 2 new members in the team compared to the Sprite Editor project. A likely scenario is for one team to split and join two other teams or for 2 teams to swap halves.

Meeting Schedules
You will follow an approximation of Scrum development for this project. The Scrum sprints will look like:

Tuesday: End of lecture sprint planning meeting (form sprint backlog, time estimation, setup Trello board)
Wednesday: Sprint stand-up meeting (lab or external)
Thursday: Sprint stand-up meeting (end of lecture)
Monday: External or virtual stand-up. Submit valuable working code. Submit meeting records, burndown charts, freeze sprint Trello board.

Each sprint will be given a grade based on team-work, process and on results. The overall project will also have a score.

Agile pushes for engagement. You should plan on spending an hour a week-day outside of class working on this project. That way you can report something at the stand-ups. Try to break into small groups and pairs for that hour.

The Project
You will develop an educational program to teach a specific topic. Think carefully about the scope of the work, the target age or demographic, and your own interest level in the topic. While the obvious kind of direction might be something like "learning to count for young kids" (something like this must have a very innovative approach since it has been done to death), also think more broadly about education, like "teaching about phishing scams to the elderly" or "learning bird songs". Connecting to an interest or hobby of someone on your team could be a good way to get instant insight into a topic. This cannot be some boring flash card style program.

Specific Requirement

You must use Qt and C++ (not QML Qt Quick elements).
The educational program side:
The educational program must have at least 3 levels or phases to work through (or something obvious that has progress that can be measured).
The educational program must make use of a physics engine. I will discuss Box2D. This may seem like overkill for many topics, but even something as simple as making buttons quiver or letters on a title screen bounce around adds visual interest and "life" to a project.
You will need to have some kind of tools effort where you put some effort into doing some needed thing the right way. I will have a requirement of a sprite sheet library, which converts a large image grid into an array of images for use in animation. You should propose at least one other tool in your pitch for an application. These tools will be evaluated on how nicely they provide services for your application and also on how independent of your application's particulars they are. If you make a highly interactive game, look at adding pixel-perfect collision detection. If you have some large world to explore, maybe you need to map or tiled map tools. If you have lots of interaction, some kind of phrase parser could be a tool.
[In some past versions of this assignment I included a bunch of server-client requirements. Somewhere along the way this semester we lost some time. Instead, I would rather you focus on making your app interactive and fun.]

Here are some suggested tools:

Box2D
You can find information on Box2D at

https://github.com/erincatto/Box2D (Links to an external site.)Links to an external site.

The main thing to remember is that Box2D is not a drawing environment, it is a simulation environment. A typical setup is to create a bunch of physics objects in Box2D, run the world, then grab position and rotation from the simulation objects and use those to draw a sprite "pasted" on the simulation object". A tutorial is at

http://www.iforce2d.net/b2dtut/bodies (Links to an external site.)Links to an external site.

SFML
You will need a way to draw a sprite. The SFML library

http://www.sfml-dev.org/index.php (Links to an external site.)Links to an external site.

provides drawing and transformation capabilities, sound, and networking. Here is some info on using SMFL and Box2D together

https://veendeta.wordpress.com/2012/02/16/tutorial-getting-started-with-box2d/ (Links to an external site.)Links to an external site.

Qt also has a QScene structure that can approximate a sprite and QSocket and QTcpSocket for networking, so you can keep it all in Qt-land if you want. I think the SFML sprites are better than Qt sprites and the socket stuff is similar.

If using SFML sprites, you will likely want to inherit from both the QWidget and SMFL window class.

There are also tutorials on merging SFML and Qt. I have a few suggestions. If you want a widget that is primarily an SFML window inside a larger Qt application, look for a tutorial using multiple inheritance to join these together. If you have a more complex situation with Qt UI elements intermingled with SFML drawing, my suggestion is to use SFML to render to an image, then make the image the background of the Qt widget and overlay buttons and such on top.

Networking
You can use the SFML networking library -  http://www.sfml-dev.org/tutorials/1.6/network-packets.php (Links to an external site.)Links to an external site. or other C++ code, or Qt networking. You do not need networking, but SFML is a nice wrapper.

User Interface
Qt should provide all the power you need.

Schedule
Between now and next Tuesday you should

form teams
give me a one paragraph idea for the educational app and tools.
create a project backlog of user stories.
The next 2 1/2 weeks will be 1 week sprints and a finale.

Each of these little phases will have an assignment associated with it. Please see them for additional details and requirements.

The projects will be presented during the CS3505 final exam slot Thursday, Dec. 15th between 1-3PM.

Development
You should maintain your code in a CS3505 repository. I will create an assignment repo.

Everyone needs to have some involvement in coding. You cannot be a pure level designer. This is a development project and you are playing the role of a developer. You can specialize on a portion of the requirements but everyone should have an understanding of the system.

Submission
This assignment covers the main project. Submission is through Canvas's group assignment mechanism. The team should add a text entry with your team name and the repo storing the project. Please submit a link to a video showing the functionality of the project through. Look at the sub-assignments for additional requirements.

## About our project  - Gitty App
We created an application that teaches students how to use git.  
## Features
The user interface made using QML. 
JSON file is loaded which contains the content.
The buttons are generated dynamically depending on the number of questions and guides.
Animations created using SFML and Box2d.
Uses open gl for the background animations.
Minimum resolution of 1286 x 985 required.
QT 5.9.1 and MSVC 2015 64 bit needed to build.

Plot:
You are on a ship and the captain because sick with the flu.
So now you have to reprogram your HAL 9000 in order to safely
explore the planets.  In order to do this you need to use Git
in order to clone and compile a few repositories.  After you 
read the guides you will need to run the correct commands in
your computer and answer the questions correctly. 
