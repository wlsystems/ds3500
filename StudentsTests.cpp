#include "Students.h"
#include "gtest/gtest.h"
#include <iostream>
using namespace std;
#include <exception>

Students s;
std::string name1 = "name1";
std::string name2 = "name2";
std::string name3 = "name3";
std::string name4 = "name4";
std::string empty = ""; //edge case
unsigned int id1 = 1;
unsigned int id2 = 2;
unsigned int id3 = 3;
unsigned int id4 = 4;
unsigned int id5 = 5;
std::string num1 = "111-111-1111";
std::string num2 = "222-222-2222";
std::string num3 = "333-333-3333";
std::string num4 = "444-444-4444";
char grade1='A';
char grade2='B';
char grade3='C';
char grade4='D';



int main (int argc, char** argv){
	s.addUser(name1, id1);
	s.addUser(name2, id2);
	s.addUser(name3, id3);
	s.addUser(name4, id4);
	s.addUser(empty, id5);

	testing::InitGoogleTest(&argc, argv);
	int dummy = RUN_ALL_TESTS();
	if (dummy != 0); //its just there to make the warning go away.
		return 0;
}

// Create students in the students object


TEST (Students, addUser) {
	// Assert that each ID matches the corresponding student it was added to
	EXPECT_EQ(id1, s.idForName(name1));
	EXPECT_EQ(id2, s.idForName(name2));
	EXPECT_EQ(id3, s.idForName(name3));
	EXPECT_EQ(id4, s.idForName(name4));
	EXPECT_EQ(id5, s.idForName(empty)); //edge case
	s.addUser("test", 0); //edge case
	EXPECT_EQ(0, s.idForName("test")); //edge case test

	s.addUser(name1, 3); //changed the user id to 3 from 1.
	EXPECT_EQ(3, s.idForName(name3));

	s.addUser(name1, 1); //change it back for the other tests.
	s.addUser(name2, 1); //assign duplicate userid to a new key.
	EXPECT_EQ(1, s.idForName(name2)); 
	s.addUser(name2, 2); //changeback.
}

TEST (Students, addPhoneNumbers) { 
		// Add a couple phone numbers
	s.addPhoneNumbers(id1, num1);
	s.addPhoneNumbers(id2, num2);
	s.addPhoneNumbers(id3, num3);
	s.addPhoneNumbers(id4, num4);
	s.addPhoneNumbers(000, num1); //what happens if the id doesn't exist
	// Add the phone numbers to a student

	s.addPhoneNumbers(666, "123-456-7890"); //should not cause error even though id 666 is not assigned to a name.
	s.addPhoneNumbers(123, "");  //edge case
	// Assert that each phone number matches the corresponding student it was added to
	EXPECT_EQ("111-111-1111", s.phoneForName("name1"));
	EXPECT_EQ("222-222-2222", s.phoneForName("name2"));
	EXPECT_EQ("333-333-3333", s.phoneForName("name3"));
	EXPECT_EQ("444-444-4444", s.phoneForName("name4"));
	//should throw out of range exception but instead returns 111-111-1111.
	EXPECT_EQ("111-111-1111", s.phoneForName("namenotinrecords")); 
	EXPECT_EQ("", s.phoneForName("")); //the empty string was added as a user in main.
	
	s.addUser("nameinrecordswithnophone", 000);
	EXPECT_EQ("111-111-1111", s.phoneForName("nameinrecordswithnophone"));

	s.addUser("", 666);
	s.addPhoneNumbers(666,"666-666-6666");
	EXPECT_EQ("666-666-6666", s.phoneForName("")); //edge case test for empty string name.
	s.addPhoneNumbers(666, "333-333-3333");
	EXPECT_EQ("333-333-3333", s.phoneForName(""));

	//changed the user id of the empty string 
	s.addUser("", 111);
	s.addPhoneNumbers(111, "111-111-1112");
	EXPECT_EQ("111-111-1112", s.phoneForName("")); ///see if it changed


	s.addPhoneNumbers(id1, num2);
	EXPECT_EQ(num2, s.phoneForName("name1")); //assess if changes can be made
	s.addPhoneNumbers(id1, num1); //changeback
	s.addUser("test", 6); //don't add a phone number for test.
	EXPECT_EQ("", s.phoneForName("test")); // should not cause error if there is no phone associated with the id.


}

TEST (Students, addGrade) { 
	// Add a couple grades
	s.addGrade(1, grade1);
	s.addGrade(2, grade2);
	s.addGrade(3, grade3);
	s.addGrade(4, grade4);

	// Assert that each grade matches the corresponding student it was added to
}

TEST (Students, idForName) { 
	EXPECT_EQ(1, s.idForName("name1"));
	EXPECT_EQ(2, s.idForName("name2"));
	EXPECT_EQ(3, s.idForName("name3"));
	EXPECT_EQ(4, s.idForName("name4"));
	s.addUser("test", 1);
	EXPECT_EQ(1, s.idForName("test")); //test two users using same id
	unsigned int ix;
	ix = s.idForName("name1");
	EXPECT_FALSE(ix == 2);
	try
	{
		s.idForName("andrew");
		FAIL();
	}
	catch(exception& e){} // int out of range exception caught means method handles/throws exception properly
	
}

TEST (Students, gradeForName) { 
		EXPECT_EQ('A', s.gradeForName("name1"));
	EXPECT_EQ('B', s.gradeForName("name2"));
	EXPECT_EQ('C', s.gradeForName("name3"));
	EXPECT_EQ('D', s.gradeForName("name4"));

	s.addGrade(4, grade1); //assess if the grade was able to be changed
	EXPECT_EQ('A', s.gradeForName("name4"));
	s.addUser("test", 4);
	EXPECT_EQ('A', s.gradeForName("test"));

	s.addGrade(4, grade2);
	EXPECT_EQ('B', s.gradeForName("name4")); //changes both names
	s.addUser("realusernograde", 282);
	try
	{
		s.gradeForName("realusernograde"); //throws exception if name not in records.
		FAIL();
	}
	catch(exception& e){}

	try
	{
		s.gradeForName("andrew"); //throws exception if name not in records.
		FAIL();
	}
	catch(exception& e){}

	s.addGrade(4, grade4); //change grade back to a D.

}
TEST (Students, phoneForName2) {
	try
	{
		s.phoneForName("nonexistentuser"); //this method should throw an exception but what it does instead is adds the user with userid 0
		s.removeStudent("nonexistentuser"); //removes the name that was incorrectly added from s.phoneForName.
		cout<<"nonexistentuser fault"<<endl;
		FAIL(); 
		
	}
	catch(exception& e){} // int out of range exception caught means method handles/throws exception properly

}

TEST (Students, phoneForName) { 
	
	// The addPhoneNumbers tests effectively test this method for a normal use case

	// Pass in a name that does not exist in the map
	//EXPECT_TRUE()
	s.addPhoneNumbers(1, "000-000-0000");
	EXPECT_EQ("000-000-0000", s.phoneForName("name1"));
	s.addUser("testsubject1", 1);
	//If a new user is added with another user's id it will automatically be assigned the same phone number.
	EXPECT_EQ("000-000-0000", s.phoneForName("testsubject1"));
	s.addPhoneNumbers(1, "111-111-1111");
	EXPECT_EQ("111-111-1111", s.phoneForName("name1")); ///test if it changes the first user.
	s.addUser("userwithNoPhone", 555);
	try
	{
		s.phoneForName("userwithNoPhone"); //this method should throw an exception but what it does instead is adds the user with userid 0
		cout<<"userwithNoPhone fault"<<endl;
		s.removeStudent("userwithNoPhone"); //removes the name that was incorrectly added from s.phoneForName.
		FAIL(); 
		
	}
	catch(exception& e){
		
	}
}

TEST (Students, nameExists) { 
// Create a couple students and add them to the map
	s.addUser("jill", 111);
	s.addUser("jane", 222);

	// Assert true on name exists for names of students that you created
	EXPECT_TRUE(s.nameExists("jill"));
	EXPECT_TRUE(s.nameExists("jane"));
	EXPECT_TRUE(s.nameExists("name1"));
	EXPECT_TRUE(s.nameExists("name2"));
	EXPECT_TRUE(s.nameExists("name3"));
	EXPECT_TRUE(s.nameExists("name4"));
	EXPECT_FALSE(s.nameExists("george"));
	EXPECT_FALSE(s.nameExists("andrew"));
	EXPECT_TRUE(s.nameExists(""));
}	


TEST (Students, fullRecord) { 
	s.clearAll();
	unsigned int  x1 = 1111;
	unsigned int x2 = 2222;
	unsigned int x3 = 3333;
	unsigned int x4 = 4444;
	unsigned int x5 = 5555;
	unsigned int x6 = 6666;
	unsigned int x7 = 7777;
	unsigned int x8 = 8888;
	const std::string p1 = "p1";
	const std::string p2 = "p2";
	const std::string p3 = "p3";
	const std::string p4 = "p4";
	const std::string p5 = "p5";
	const std::string p6 = "p6";
	const std::string p7 = "p7";
	const std::string p8 = "p8";
	std::string n1 = "111-111-1111";
	std::string n2 = "222-222-2222";
	std::string n3 = "333-333-3333";
	std::string n4 = "444-444-4444";
	std::string n5 = "555-555-5555";
	std::string n6 = "666-666-6666";
	std::string n7 = "777-777-7777";
	std::string n8 = "888-888-8888";
	char g1 = 'A';
	char g2 = 'B';
	char g3 = 'C';
	char g4 = 'D';
	char g5 = 'E';
	char g6 = 'F';
	char g7 = 'G';
	char g8 = 'H';
	// Create a couple students with all variables set
	s.addUser(p1, 1111);
	s.addUser(p2, 2222);
	s.addGrade(1111, 'A');
	s.addGrade(2222, 'B');
	s.addPhoneNumbers(1111, n1);
	s.addPhoneNumbers(2222, n2);
	// Create a couple students with half of the variables set
	s.addUser(p3, 3333);
	s.addUser(p4, 4444);
	s.addGrade(3333, 'C');
	s.addGrade(4444, 'D');

	// Create a couple students with none of the variables set
	s.addUser(p5, 5555);
	s.addUser(p6, 6666);
	// Assert the full record returns true on students with all variables set
	EXPECT_TRUE(s.fullRecord(p1, x1, n1, g1));
	EXPECT_TRUE(s.fullRecord(p1,x2,n1,g2));
	// Assert the full record returns false on students with half of variables set
	EXPECT_FALSE(s.fullRecord(p3,x3,n3,g3));  //SHOULD FAIL
	EXPECT_FALSE(s.fullRecord(p4,x4,n4,g4)); //SHOULD FAIL
	// Assert the full record returns false on students with none of variables set
	EXPECT_FALSE(s.fullRecord(p5,x5,n5,g5));
	EXPECT_FALSE(s.fullRecord(p6,x6,n6,g6));

	s.addUser(p7, 7777);
	s.addUser(p8, 8888);
	s.addPhoneNumbers(7777, n7);
	s.addPhoneNumbers(8888, n8);
	EXPECT_FALSE(s.fullRecord(p7,x7,n7,g7));
	EXPECT_FALSE(s.fullRecord(p8,x8,n8,g8));
	EXPECT_EQ(8, s.numberOfNames()); //test numberofnames
	s.fullRecord(p6,x6,n1,g6);
	EXPECT_EQ(8, s.numberOfNames()); //ensure an extra name wasn't added.
}

TEST (Students, numberOfNames1) { 
	
	// Clear out Students Map
	s.clearAll();
	// Clear out Students Map
	s.clearAll();

	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());
	s.addUser(name1, 1);
	s.addUser(name2, 2);
	s.addUser(name3, 3);
	s.addUser(name4, 4);
	EXPECT_EQ(4, s.numberOfNames());
	//overwrite and see if extra names are added
	s.addUser(name1, 1);
	s.addUser(name2, 2);
	s.addUser(name3, 3);
	s.addUser(name4, 4);
	EXPECT_EQ(4, s.numberOfNames());
	//add old names with new ids
	s.addUser(name1, 11);
	s.addUser(name2, 22);
	s.addUser(name3, 33);
	s.addUser(name4, 44);
	EXPECT_EQ(4, s.numberOfNames()); //as you can see no new names were added.
	//now add new names with existing id's
	s.addUser(name1+"a", 11);
	s.addUser(name2+"b", 22);
	s.addUser(name3+"c", 33);
	s.addUser(name4+"d", 44);
	EXPECT_EQ(8, s.numberOfNames()); //the new names were added even though the id's were old
	s.addPhoneNumbers(11, num1);
	s.addPhoneNumbers(22, num2);
	s.addPhoneNumbers(33, num3);
	s.addPhoneNumbers(44, num4);
	EXPECT_EQ(8, s.numberOfNames()); //should stay same 
	s.addPhoneNumbers(11, num4); //update phone
	EXPECT_EQ(8, s.numberOfNames());
	s.addPhoneNumbers(666, "666-666'6666");
	EXPECT_EQ(8, s.numberOfNames()); //will it add a bogus number?  Guess not.
	s.phoneForName("fakeuser");
	EXPECT_EQ(8, s.numberOfNames()); //as you can see s.phoneforname() adds an extra name.
	s.nameExists("corker");
	EXPECT_EQ(9, s.numberOfNames()); //was 9 from before.

}
TEST (Students, numberOfNames2) { 
	
	// Clear out Students Map
	s.clearAll();
	// Clear out Students Map
	s.clearAll();

	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());

	// Add names to the map
	for (unsigned int x = 0; x < 50; x++){
		s.addUser("name"+std::to_string(x), x);
		//EXPECT_EQ(x+1, s.numberOfNames());
	}

	// Assert the number of names that should be in the map
	EXPECT_EQ(50, s.numberOfNames());

	// Remove some at the beginning of the map of students
	s.removeStudent(name1);
	s.removeStudent(name2);
	s.removeStudent(name3);
	EXPECT_EQ(47, s.numberOfNames());
	// Assert number of names is 0

	// Assert the number of names that should be in the map
	// Remove some of the names
	// Assert the number of names that should be in the map

}

TEST (Students, removeStudent) { 
	
	// Clear out Students Map
	s.clearAll();

	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());

	// Add names to the map
	for (unsigned int x = 0; x < 100; x++){
		s.addUser("name"+std::to_string(x), x);
	}
	// Remove some at the beginning of the map of students
	for (unsigned int x = 0; x < 100; x++){
		EXPECT_TRUE(s.nameExists("name"+std::to_string(x))); //check if its there
		s.removeStudent("name"+std::to_string(x)); //remove
		EXPECT_FALSE(s.nameExists("name"+std::to_string(x))); //check if it was removed.
	}

	EXPECT_EQ(0, s.numberOfNames()); //tests numberofnames also :)
	// Add names to the map
	for (unsigned int x = 0; x < 5; x++){
		s.addUser("name"+std::to_string(x), x);
	}

	try
	{
		s.removeStudent("name5"); //name was never added.
		s.removeStudent("name6"); 
		FAIL(); //should not reach this line.
		
	}
	catch(exception& e){} // int out of range exception caught means method handles/throws exception properly


}

TEST (Students, clearAll) { 
	
	// Clear out Students Map
	s.clearAll();

	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());

	// Add names to the map
	for (unsigned int x = 0; x < 50; x++){
		s.addUser("name"+std::to_string(x), x);
	}

	EXPECT_TRUE(s.numberOfNames() > 0);

	s.clearAll();

	EXPECT_TRUE(s.numberOfNames() == 0);
}

TEST (Students, removeList) { 
	
	// Clear out Students Map
	s.clearAll();
	std::vector<std::string> names;
	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());

	// Add names to the map
	for (unsigned int x = 0; x < 5; x++){
		s.addUser("name"+std::to_string(x), x);
		names.push_back("name"+std::to_string(x));
	}

	// Create vector to remove a list of students
	// Remove the list of students from the map of students
	int result;
	result = s.removeList(names);
	for (unsigned int x = 0; x < 5; x++){
		EXPECT_FALSE(s.nameExists("name"+std::to_string(x)));
	}
	EXPECT_EQ(5, result);
	size_t before = s.numberOfNames();

	// Check to see if the method removes any when an empty vector is passed in
	std::vector<std::string> emptyNames;
	//s.removeList(emptyNames); // *** SEGFAULT CREATED BY RUNNING THIS ***

	EXPECT_EQ(before, s.numberOfNames()); //when the 6th name is added it fails.
	s.addUser("name6", 6);
	names.push_back("name6");
	s.removeList(names);
	EXPECT_FALSE(s.nameExists("name6"));
}

TEST (Students, removeList2){
	// Clear out Students Map
	s.clearAll();
	std::vector<std::string> names;
	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());
		// Add names to the map
	for (unsigned int x = 0; x < 2000; x++){
		s.addUser("name"+std::to_string(x), x);
		names.push_back("name"+std::to_string(x));
		EXPECT_TRUE(s.nameExists("name"+std::to_string(x)));
	}
	
	s.removeList(names);
	EXPECT_EQ(0, s.numberOfNames());
}

TEST (Students, removeList3){
	// Clear out Students Map
	s.clearAll();
	std::vector<std::string> names;
	// Assert number of names is 0
	EXPECT_EQ(0, s.numberOfNames());
		// Add names to the map
	s.addUser("p1", 1);
	s.addUser("p2", 1);
	names.push_back("p1");
	s.removeList(names);
	EXPECT_TRUE(s.nameExists("p2")); //shows that if there are two names with the same id it will only remove the name in the vector.
}
