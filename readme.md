# A4: Unit-Testing
Overview
The goal of this assignment is to practice writing unit tests within the Googletest framework. You will also collaborate on a GitHub repository, and use Travis to build and run your project.

Setup Googletest
To start, go to the Googletest page linked under the assignment and download the framework.

I was able to compile from the googletest/googletest directory the libgtest.a and libgtest_main.a with a "cmake ." followed by "make clean" and "make all". The libraries were built in that same folder and include files are inside ./include.

Teams
You will form teams of 2 for this assignment. You may self-select your teams. Advertise on the A4 Team Forming discussion if you need a team. If things get late, send me email and I will try to facilitate. 

GitHub
One team member should accept the assignment invitation

https://classroom.github.com/a/CW6m9Izt (Links to an external site.)Links to an external site.

Then, add your partner's GitHub account to the team for your assignment repository. The accepting student should have admin rights.

Testing
I am providing a Students class which stores names and ids and cross-links the id to a phone number or a grade. The Googletest documentation gives examples of writing a .cpp file that makes a test and several sub-tests. You will do something similar for my Students class with its interface accessible in Students.h and compiled implementation available in Students.o. This .o is compiled for the lab2 machines and also works in the Travis build environment. You will not be able to easily develop in Visual Studio or on a Mac.

There are problems with the Students class. You should find at least some of these errors.

You should write tests to exercise and test the Students class. Since you do not have access to the private data members of the class, you cannot test everything you might want to as you will have to rely on the interface to get information out of class objects. (As an aside, the process of writing tests can often point out limitations in a class interface and prompt revision). So if something just cannot be well-tested through the interface, you can leave it alone.

You should write tests that cover:

Basic cases (put information in, get information out)
Boundary cases
Specifications - test what the comments suggest a method should do.
I am not expecting a detailed dive into Googletest. I wrote all my tests using EXPECT_EQ, EXPECT_TRUE/FALSE and FAIL() (which causes a failure if the FAIL place in the code is reached). Use FAIL to fail a test when something does not happen (like an exception). You can try out some more advanced features, but it is not required.

The big question of testing is "how do you know when you have enough tests"? For this assignment, if you have a basic test of a method, something that you think covers some more unusual case, and any specified true/false/exception behaviors, I think you will be fine. I will add that I wrote two tests that pretty easily uncovered two different problems with my code. Then I have another issue that is still covered by the different test categories above, but maybe more unusual, and one more issue that is more subtle, but that has hints in the code about it. There are probably other cases in the code, or cases that propagate from some initial issue, so don't try to turn one small bad test case into a bunch of derived bad test cases.

So, if you have some basic tests and you have found one or two real problems and your tests look respectable, then you are in fine shape.

I will also add that any problems are not based on any super-specific code (if (id == 10) { // add random data to the map}). I think they are based on common-issues or naively structured code.

Specifics
You must work with a partner.
Write a file called StudentsTests.cpp. Put tests in the file covering the requirements above. Use the Googletest framework. Do not add the googletest source or libs to your GitHub repository. Do not add .o (except for Students.o) or executables to the GitHub repo, they will likely interfere with the Travis build or Makefile dependencies.
Add a Makefile that uses a GOOGLETEST variable for the location of the googletest directory. $(GOOGLETEST) should be the directory that has the googletest lib files and $(GOOGLETEST)/include should have gtest/gtest.h. The makefile should have its first target build the test executable and there should be a second target 'test' which executes your test executable.
A makefile can get environment variables sent into it like
make VARIABLE=/path/to/something/ target
The makefile can provide a default value for a variable like
VARIABLE ?= /default/path/
The behavior is that the command-line setting will override the optional default, but if it is not provided, it will supply a value. Set your GOOGLETEST variable using the ?= form of assignment so that I can override the path.
Add a .travis.yml file which installs googletest. The Travis script should call make with variable values set to point to the googletest installation. The Googletest on Travis link under the assignment and Lab4 give the basics of how to install googletest, but it also does a lot of other stuff. You should use the ideas in the Updated.travis.yml section reduce the items installed to remove unnecessary components. Remove all the packages except lib-gtest-dev - if you don't do this the Travis builds will take a long time. The before_script mostly echos version numbers to the console. You can reduce that. The install section makes a gtest directory (note that this is different from the default directory structure when installing with a git clone) and copies the installed version into that directory. You can change those names to be consistent with what you have on your local machine or you can change your local machine or you can use the GOOGLETEST variable to match the local structure. All this stuff matters, so look carefully at what is going on. You can add other commands to the install, like 'ls', if you want to see what is located in the directory on the Travis machine. The script section really should just be your make and make test. 
Put all of the above, ideally during development, in your assignment repository. The end result should be that Travis builds your tests, runs them, and displays the results. The Travis build will report a failure since it is likely that your tests find bugs which fail tests. Fix this by adding your own main to the test .cpp file and remove the linking with gtest_main.a. A useable main can be found at https://stackoverflow.com/questions/32915860/can-i-make-google-test-return-0-even-when-tests-fail (Links to an external site.)Links to an external site.
Write a short document interpreting the output from running your test program and describing the failing test cases you have found. Describe your setup and results. Give a hypothesis that is your best guess as to what might be causing the issue for each failing test. Formulating a hypothesis based on execution data is an important debugging skill. Start the document with your team information - names and GitHub repository link. Show some organization and communication skills in the document.
Add to the document how you and your partner split up the work. If you worked together, say that. If you broke the problem into categories and each worked on part, say what those categories work. Save the document as Testing.pdf document.
Both partners should submit the Testing.pdf.
Previous Next

Dustin Shiozaki
https://github.com/University-of-Utah-CS3505/cs3505-f17-a4-googletest-wlsystems.git
TEST​ ​(Students,​ ​addPhoneNumbers)
- If the name is in the records but has no phone the method returns 111-111-1111
- If the name is not in the records the method returns 111-111-1111.
For adding phone numbers to a name that is not in the records, it returns a default phone
number rather than correctly handling it as an invalid input.
TEST​ ​(Students,​ ​phoneForName)
- If you add two users that have the same userID then changing the phone number
will result in both users having the same phone number.
- If you try to look up a non existent name then it will add that name and the total
number of records will increase by one.
When the phoneForName method is given a nonexistent name, it adds that name and
increases the number of entries in the Students object by one. This should be taken care of and
returned as an error, but it adds the user instead.
- If the name is an empty string it returns an empty string.
When an empty string is added with a user ID and then a phone number is added,
the phoneForName("") returns the correct phone. Then if you change the phone number
it is changed correctly. However, if call addUser("", 123) and assign it a different userId and
phone number then it incorrectly returns a memory address instead of the phone number.
TEST​ ​(Students,​ ​addGrade)
- Like the other methods if two users have the same ID they can still only have
one grade for both of them.
Each user ID can only contain one grade, so if there is another student with the same
user ID that is created, it is the same student rather than adding a new student with the same
user ID.
TEST​ ​(Students,​ ​fullRecord)
When an incomplete record containing a grade but not a phone number is entered, the
test should fail but returns true. So our guess is that it checks if the user is valid and has a
grade and then returns true but there is no logic to check if there is also a phone number. But in
the part where it checks if there is a phone number and no grade, it sees there is no grade and
correctly returns false. So the part that checks if it has a phone number is probably the last part
in a block of if statements and they just returned true if the previous logic (has grade) is true.
TEST​ ​(Students,​ ​removeList)
- Segmentation fault caused by passing in empty vector.
Our best guess as to what is causing this behavior is that the method in Students does
not know how to properly handle the empty vector as input. If I had to guess, I would say that it
tries to access slots in memory that it think the vector has, but then a segmentation fault occurs
because there is nothing there.
- When more than 5 users are added and then removed by calling removeList, it does not
remove them properly
-When I added 20 users calling removeList removed only 4.
-when I add 51-100 removeList removes 10.
-when I add 50 removiesLIst removes 5.
-when I added 2000 names removeList removes 10.
TEST​ ​(Students,​ ​numberOfNames)
- Results skewed by the phoneForName method
For every time the phoneForName method is called given a non-exsistent name, a new
user is added when it shouldn’t be added. This causes the numberOfNames count to go up
wrongly, resulting in an off count of the numberOfNames in the Students map.
Problem​ ​solving​ ​and​ ​organization:
Dustin​ ​shiozaki​ ​-​ ​ ​worked on .travis.yml. submitted initial makefile. Worked on initial and later
tests. Found and documented errors in the following methods: phoneforname, numberofnames,
removeList,​ ​ ​fullRecord,​ ​ ​addPhoneNumbers.​ ​ ​Formulated hypothesis as to why
fullRecord is malfunctioning. Did additional stress testing on removeList and
documented the cases with the different numbers of names. I also created the main
function and made it return 0 so that travis would build. I added the test variable to the
makefile and also added the ?= to allow the variable to be overridden.
