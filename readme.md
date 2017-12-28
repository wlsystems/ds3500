# A6: Planning Phase for a Sprite Editor

Group Work
This is a group assignment. Groups can be of size 5-6. You will need to self-organize your groups, but I can step in if someone needs help. You should consider your ability to meet and communicate along with just who is a "buddy". 

Groups must be able to meet with a TA. The default is that you can all meet during a single Wednesday lab session. By petition, your group meeting can be during a TA office hour. In order to show a group formation, you should register your group with the instructor by email and specify when your lab meeting will be and who all the members are and an awesome team name. When the group seems set, we will form a Canvas group for your team for this assignment.

Assignment Structure
This assignment will develop a "larger" project split into 2 distinctly graded phases. Part 1 is largely planning and part 2 will be largely implementation.

The Project
Your team is going to develop a Qt-based sprite editor. A sprite is a small image used in gaming that is rendered at different locations on the screen. Most sprites have animation cycles associated with them, so that a sprite is really an array of small images. The sequence of images might show the sprite walking, or exploding, or powering-up, and so on.

A sprite editor usually includes a zoomed-in view of the image pixels and the ability to manipulate the pixels using simple tools (maybe even just clicking on pixels). A sprite editor has a sense of an array of images to handle to the sequence of frames. A useful feature in an editor is a preview, which animates the sequence of images at adjustable rates.

An online example is Piskell

http://www.piskelapp.com (Links to an external site.)Links to an external site.

If you are unfamiliar with digital images, please read background material, such as http://people.cs.clemson.edu/~dhouse/courses/405/notes/pixmaps-rgb.pdf
 (Links to an external site.)Links to an external site.

One thing to pay attention to is the idea of an alpha channel, which controls transparency for parts of the sprite image that should let the background through. You do not want a character to always be surrounded by a black or white background square, and transparency allows only the character image to be drawn.

Part 1  - Planning
For part 1, you will need to plan what your application should do, and start architecting how it will do it. I will provide a basic set of requirements, but you are encouraged and free to add your own. However, realize that all features have a cost, so you should balance your team's effort with the complexity of the project.

You should research existing alternatives, see what you like or do not like, and brainstorm any innovations of your own.

The project (Part 1 and Part 2) will culminate in a demonstration to the class. The class and instructional staff will rank projects, presentation, and best features. 

Qt Capabilities
Your team should spend some time seeing how Qt might support such an app. For example, look at drawing basics  http://doc.qt.io/qt-5/qtwidgets-painting-basicdrawing-example.html (Links to an external site.)Links to an external site..

Existing Market Document
For Part 1, you will need to generate a "Existing Market" document that lists existing projects you have found and which gives a short summary of the features and advantages and disadvantages of each main feature (a paragraph for each existing project). I would expect this to be about a page and each team member must contribute a researched project. Annotate each project description with the team member who contributed it.

Software Requirements Specification
After checking out the state of sprite sheet editors, you will need to brainstorm your project's desired features and produce a Requirements document. This should include requirements from this assignment description and any additional features you provide. There should be enough detail that a person using the finished project can see how the requirement is fulfilled. Use the srs template file linked in Canvas by the assignment. I expect this to be a few pages long (not the 90 pages of the example linked from the last lecture), so use detail where helpful and remove some sections that do not have useful information to add.

System Design
Begin architecting the system by producing a UML class diagram. You should not include all attributes and operations, just enough to get a sense of the interplay of classes. Try to tie in a few high-level Qt classes to the UML diagram, but not all the low-level gui objects. In other words, if you have a scrollable pane for the images, see what Qt provides that might be helpful and note it in the diagram, but do not tie in all the QPushButton signals and things like that. I expect this to be a single page, but maybe hard to view when printed on an 8.5" x 11" sheet.

Requirements
The application must be able to:

Set the size in pixels of the sprite. This can be bounded by a minimum, maximum, and by jumps in size if desired (historically, sprites rendered as textures on simple geometry were most efficient for graphics cards when sized at powers of 2).
Adjust the number of frames for the sprite animation - this can be incremental, like "add a frame".
Allow the user to modify the pixels of a sprite
You should represent a pixel as a (r,g,b,a) tuple where each element holds values from 0-255.
Provide a preview of the sprite animation cycle
The user should be able to adjust the frames per second of playback
There should be an option of seeing the playback at the actual size of the sprite (even when super tiny).
Save and load a project. All numbers in the project file should be represented by ASCII text. A project file should meet the following specification
The height and width of a sprite frame specified by 2 integers with a space between followed by a \n newline. 
The number of frames represented by a single integer followed by a newline.
Each frame in order from lowest to highest numbered. A frame is output by
starting at the top row and going to the bottom, list the pixels for each row as red green blue alpha values with spaces in-between two values. Finish a row with a newline. Do not add extra whitespace between color values or pixels or between rows or between frames. 
Use a sprite sheet project .ssp extension when saving. You may create your own extension and save option if you have fancy features that need additional information to be saved but you need to be able to read and save this format.
Export your sprite to an animated gif format. You will need to research possible C++ libraries for this.
All other features are up to you. Additional features are needed for full points. See your research into other sprite editor products for ideas on features.

Do not lose sight of the goal of this application - to make sprites. I have had groups get carried away and implement some tiny Mario game engine in their editor, which was impressive, but did not count as a useful feature for a sprite editor.

Relationship between Plan and Product
Part of the final grade for this project will be how well you match your design and final product. You should consider your requirements doc and design UML doc as a contractual agreement after submission of Part 1. Investigate your possibilities carefully! However, in the end, we are much more interested in a nice, working editor than something broken which fulfills your plan.

Management Plan
As part of Phase 1, you should produce a Gantt chart that shows the subtasks needed to implement the sprite editor. Each subtask should have group members associated with it. It is not allowed to just list everyone for every task. The Gantt chart is just a table with tasks along the vertical axis and time along the horizontal. The Gantt charts helps show possible dependencies between tasks.

I will provide an evaluation document next week to help you evaluate how the group worked and how individuals in the group contributed.

Phase I Evaluation
Your planning will be evaluated in terms of:

the quality of the planning
professionalism of the documentation
eventual realism of the plan compared to the final product
your self evaluation, team evaluation of you, and TA evaluation
The instructional staff reserves the right to discuss your contributions to the project and your understanding of your team's work as part of the evaluation process.

The lab next week will be an opportunity for the TAs to give suggestions about your planned features.

Phase 2
The implementation phase of the project will be two weeks long.

Submission
We will set up Canvas groups for this assignment once you send email confirming your group members. My understanding is that one person in the group submits one set of documents - your Existing Market document, Requirements document, UML diagram, and Management plan as pdf files and those are tied to everyone in the group.
