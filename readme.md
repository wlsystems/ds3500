
# A5: A Simon Game
## This is a short demo video of our finished project:
<div align="center">
  <a href="https://www.youtube.com/watch?v=3cncl2jl2I0"><img src="https://img.youtube.com/vi/3cncl2jl2I0/0.jpg" alt="IMAGE ALT TEXT"></a>
</div>

Pairs
You should do this assignment in pairs. One partner should accept the repo invitation at

https://classroom.github.com/a/GxrIq7g5 (Links to an external site.)Links to an external site.

and add the partner. The repo will save a submission at 11:00PM Thursday, Oct. 19th. The assignment is due at this same time. The odd time seems a quirk of the options for the repo submission thing - sorry!

There is not a Travis component to this assignment - there are dev-Qt libs that can be added, but it gets a bit complicated.

Using Qt
You can use Qt on any of the HTML5 pool machines and there should be an updated Qt install on the Linux lab 2 machines. You will probably most enjoy installing Qt on your own machine.

Look for the open-source Qt install.

You should be installing Qt 5.9.1. You will want to install the basic Qt setup and Qt Creator. I made sure I removed the iOS (huge) and Android packages as well as other extras. I think iOS was the only giant part. You should see Qt Creator under the Tools category, and that is their development environment we will be using.

For a Mac install, I indicated the clang compiler. For Windows, I used a recent MSVC - the Windows lab machines use 2015. We have free access to Visual Studio through CADE - you should install that before Qt if you do not have a recent version. Mingw might be fine. Discuss if it seems to be causing issues.

Do not add dependencies to other libraries for this project. With that restriction, Qt should do a reasonable job of moving from one machine type to another and there is no "gold-standard" machine this project should run on. If we have problems, we will contact you.

If you open Qt Creator and start a new project, choose a Widget app. This gives a baseline of functionality. You will need to add to the app using a mixture of the WYSIWYG Designer and custom C++ code.

Goals
You are to make a simple Simon game. 

https://www.youtube.com/watch?v=G6p7zRsECaI (Links to an external site.)Links to an external site.


In our version, there will be just two colors/buttons. 

Model-View Structure
In your application, the game logic should be pulled into its own class, the interface should be the MainWindow class (you can rename it to something else) and the view and the model should only communicate through signals and slots.

More specifically, you can construct the view with a model object and make connections there. The view class should not store or directly reference a model object (except through whatever magic happens in a connect function) and the model class should not store or directly reference a view.

Some Useful Qt Things
Some things that you might find helpful are:

QTimer: QTimer has a static method singleShot which calls a slot after a specified delay.
StyleSheets: You can set a style sheet on a button to change its color. For example, a fairly painful way of setting the regular color of a button and the color when it is pressed is 
ui->redButton->setStyleSheet( QString("QPushButton {background-color: rgb(200,50,50);} QPushButton:pressed {background-color: rgb(255,150,150);}"));
You can enable buttons or disable them with setEnabled(true/false)
If you declare a signal in the header of a class which inherits from QObject, you can emit that signal in your code. You do not need to implement that signal - it is declared in the header and just emitted in code. Signals can pass information along with the signal.
Requirements
The game should have

A start button. The start button should be disabled when the game is in progress and back enabled when no game is in progress. You may start a game off with an initial list of moves if starting at 1 move seems too easy.
Two buttons, Red and Blue. They should display the computer moves when it is not the player's turn and should let the player press them (with visual feedback) when it is the player's turn. The buttons should be disabled when it is not the player's turn.
Each time the computer plays again, it should add a new move to the end of the current sequence of moves.
A progress bar shows the percentage of moves the player has completed when it is the player's turn. Be careful to end at 100% when all the moves have been matched by the player.
Some kind of a "you lose" message when the player makes a mistake. You can decide what form this shold take.
The game should speed up as more moves are added. 
Something nice added beyond these basic requirements. Some acceptable examples would be nicer button display or best scores - but you are encouraged to do your own idea. It should be something that would cause us to say "oh, that is nice".  
Keep your .cpp,  .h, .ui, and .pro files on GitHub. We will clone that.

Make a short video (Macs have Quicktime player screen video capabilities, use your phone, etc.) showing key functionality of your game. Watch your file size, if it is 100s of MB you are doing something wrong.

On Canvas, one partner should enter their name and their partner's name and the repo used in the text entry area.  That same partner should submit the movie file or a URL to the movie (such as a YouTube). The other partner should submit nothing.

Previous Next
